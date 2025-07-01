using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Scash.Data;
using Scash.Models;

namespace Scash.Services
{
    public class TransactionService
    {
        private readonly ScashDbContext _context;
        private readonly SystemSettingsService _settingsService;
        private const decimal DAILY_LIMIT_PERSONAL = 1000.00m;
        private const decimal COMMISSION_RATE = 0.01m; // 1% per transaction

        public TransactionService(ScashDbContext context)
        {
            _context = context;
            _settingsService = new SystemSettingsService(context);
        }

        public async Task<Transaction> CreateTransactionAsync(
            int senderId,
            int? recipientId,
            decimal amount,
            TransactionType type,
            string description = null,
            UtilityType? utilityType = null)
        {
            var sender = await _context.Users.FindAsync(senderId);
            if (sender == null)
                throw new ArgumentException("Sender not found");

            if (!sender.IsActive)
                throw new InvalidOperationException("Sender account is inactive");

            if (sender.IsLocked)
                throw new InvalidOperationException("Sender account is locked");

            // Check daily and monthly transaction limits
            var today = DateTime.UtcNow.Date;
            var monthStart = new DateTime(today.Year, today.Month, 1);

            var dailyTotal = await _context.Transactions
                .Where(t => t.SenderId == senderId && t.CreatedAt.Date == today)
                .SumAsync(t => t.Amount);

            var monthlyTotal = await _context.Transactions
                .Where(t => t.SenderId == senderId && t.CreatedAt >= monthStart)
                .SumAsync(t => t.Amount);

            if (dailyTotal + amount > sender.DailyTransactionLimit)
                throw new InvalidOperationException("Daily transaction limit exceeded");

            if (monthlyTotal + amount > sender.MonthlyTransactionLimit)
                throw new InvalidOperationException("Monthly transaction limit exceeded");

            // Calculate fees and commission
            decimal fee = 0;
            decimal commission = 0;

            switch (type)
            {
                case TransactionType.SendMoney:
                    fee = amount * (await _settingsService.GetSetting<decimal>(SystemSettingKeys.SendMoneyFee, 0.005m) / 100m);
                    break;
                case TransactionType.MobileRecharge:
                    fee = amount * (await _settingsService.GetSetting<decimal>(SystemSettingKeys.MobileRechargeFee, 0m) / 100m);
                    break;
                case TransactionType.UtilityBillPayment:
                    fee = amount * (await _settingsService.GetSetting<decimal>(SystemSettingKeys.UtilityBillFee, 0m) / 100m);
                    break;
                case TransactionType.QRPayment:
                    fee = amount * (await _settingsService.GetSetting<decimal>(SystemSettingKeys.QRPaymentFee, 0.0025m) / 100m);
                    break;
                case TransactionType.CashIn:
                    fee = amount * (await _settingsService.GetSetting<decimal>(SystemSettingKeys.AgentCashInCommission, 0.01m) / 100m);
                    break;
                case TransactionType.CashOut:
                    fee = amount * (await _settingsService.GetSetting<decimal>(SystemSettingKeys.CashOutFee, 0.01m) / 100m);
                    break;
                case TransactionType.MerchantPayment:
                    fee = amount * (await _settingsService.GetSetting<decimal>(SystemSettingKeys.MerchantPaymentFee, 0.0025m) / 100m);
                    break;
                default:
                    fee = amount * (await _settingsService.GetSetting<decimal>(SystemSettingKeys.TransactionFee, 0.005m) / 100m);
                    break;
            }

            // Calculate commission for agents
            if (sender.Role == UserRole.Agent)
            {
                commission = amount * (await _settingsService.GetSetting<decimal>(SystemSettingKeys.AgentCommission, 0.01m) / 100m);
            }

            var totalAmount = amount + fee + commission;

            if (sender.Balance < totalAmount)
                throw new InvalidOperationException("Insufficient balance");

            var transaction = new Transaction
            {
                SenderId = senderId,
                RecipientId = recipientId,
                Amount = amount,
                Fee = fee,
                Commission = commission,
                Type = type,
                Status = TransactionStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                Description = description,
                UtilityType = utilityType,
                ReferenceNumber = GenerateReferenceNumber()
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<Transaction> CompleteTransactionAsync(int transactionId)
        {
            // Ensure we have a fresh context state
            await _context.Database.CloseConnectionAsync();
            
            var transaction = await _context.Transactions
                .Include(t => t.Sender)
                .Include(t => t.Recipient)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);

            if (transaction == null)
                throw new ArgumentException("Transaction not found");

            if (transaction.Status != TransactionStatus.Pending)
                throw new InvalidOperationException("Transaction is not in pending status");

            try
            {
                // Update sender balance
                transaction.Sender.Balance -= (transaction.Amount + transaction.Fee + transaction.Commission);

                // Update recipient balance if applicable
                if (transaction.Recipient != null)
                {
                    transaction.Recipient.Balance += transaction.Amount;
                }

                // Create transaction history entries
                var senderHistory = new TransactionHistory
                {
                    UserId = transaction.SenderId,
                    TransactionId = transaction.TransactionId,
                    Amount = -(transaction.Amount + transaction.Fee + transaction.Commission),
                    Balance = transaction.Sender.Balance,
                    Commission = transaction.Commission,
                    Type = transaction.Type,
                    ReferenceNumber = transaction.ReferenceNumber,
                    Description = transaction.Description,
                    CreatedAt = DateTime.UtcNow
                };

                if (transaction.Recipient != null)
                {
                    var recipientHistory = new TransactionHistory
                    {
                        UserId = transaction.RecipientId.Value,
                        TransactionId = transaction.TransactionId,
                        Amount = transaction.Amount,
                        Balance = transaction.Recipient.Balance,
                        Commission = 0,
                        Type = transaction.Type,
                        ReferenceNumber = transaction.ReferenceNumber,
                        Description = transaction.Description,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.TransactionHistories.Add(recipientHistory);
                }

                _context.TransactionHistories.Add(senderHistory);
                transaction.Status = TransactionStatus.Completed;
                transaction.CompletedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return transaction;
            }
            catch (Exception ex)
            {
                // Reset transaction status on error
                transaction.Status = TransactionStatus.Failed;
                await _context.SaveChangesAsync();
                throw new InvalidOperationException($"Transaction failed: {ex.Message}", ex);
            }
        }

        public async Task<Transaction> CancelTransactionAsync(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null)
                throw new ArgumentException("Transaction not found");

            if (transaction.Status != TransactionStatus.Pending)
                throw new InvalidOperationException("Transaction cannot be cancelled");

            transaction.Status = TransactionStatus.Cancelled;
            transaction.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<IEnumerable<TransactionHistory>> GetUserTransactionHistoryAsync(
            int userId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            TransactionType? type = null)
        {
            var query = _context.TransactionHistories
                .Where(t => t.UserId == userId);

            if (startDate.HasValue)
                query = query.Where(t => t.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.CreatedAt <= endDate.Value);

            if (type.HasValue)
                query = query.Where(t => t.Type == type.Value);

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        private string GenerateReferenceNumber()
        {
            return $"TRX{DateTime.UtcNow:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
        }

        public async Task<Transaction> SendMoney(int senderId, string recipientUsername, decimal amount)
        {
            var recipient = await _context.Users.FirstOrDefaultAsync(u => u.Username == recipientUsername);
            if (recipient == null)
                throw new ArgumentException("Recipient not found");

            if (!recipient.IsActive)
                throw new InvalidOperationException("Recipient account is inactive");
            
            var transaction = await CreateTransactionAsync(senderId, recipient.Id, amount, TransactionType.SendMoney, "Send Money");
            return await CompleteTransactionAsync(transaction.TransactionId);
        }

        public async Task<Transaction> CashIn(int agentId, string recipientUsername, decimal amount)
        {
            var agent = await _context.Users.FindAsync(agentId);
            if (agent == null)
                throw new ArgumentException("Agent not found");

            if (agent.Role != UserRole.Agent)
                throw new InvalidOperationException("Only agents can perform cash-in operations");

            if (!agent.IsActive)
                throw new InvalidOperationException("Agent account is inactive");

            var recipient = await _context.Users.FirstOrDefaultAsync(u => u.Username == recipientUsername);
            if (recipient == null)
                throw new ArgumentException("Recipient not found");

            if (!recipient.IsActive)
                throw new InvalidOperationException("Recipient account is inactive");

            // For cash-in, the agent provides the money, so the recipient gets the full amount
            var transaction = await CreateTransactionAsync(agentId, recipient.Id, amount, TransactionType.CashIn, "Cash In");
            return await CompleteTransactionAsync(transaction.TransactionId);
        }

        public async Task<Transaction> CashOut(int userId, decimal amount)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            if (!user.IsActive)
                throw new InvalidOperationException("User account is inactive");

            if (user.IsLocked)
                throw new InvalidOperationException("User account is locked");

            // For cash-out, the user withdraws money (no recipient)
            var transaction = await CreateTransactionAsync(userId, null, amount, TransactionType.CashOut, "Cash Out");
            return await CompleteTransactionAsync(transaction.TransactionId);
        }

        public async Task<Transaction> PayMerchant(int senderId, string merchantUsername, decimal amount)
        {
            var sender = await _context.Users.FindAsync(senderId);
            if (sender == null)
                throw new ArgumentException("Sender not found");

            if (!sender.IsActive)
                throw new InvalidOperationException("Sender account is inactive");

            if (sender.IsLocked)
                throw new InvalidOperationException("Sender account is locked");

            var merchant = await _context.Users.FirstOrDefaultAsync(u => u.Username == merchantUsername && u.Role == UserRole.Merchant);
            if (merchant == null)
                throw new ArgumentException("Merchant not found");

            if (!merchant.IsActive)
                throw new InvalidOperationException("Merchant account is inactive");

            var transaction = await CreateTransactionAsync(senderId, merchant.Id, amount, TransactionType.MerchantPayment, "Merchant Payment");
            return await CompleteTransactionAsync(transaction.TransactionId);
        }

        public async Task<Transaction> WithdrawToBank(int merchantId, decimal amount)
        {
            var merchant = await _context.Users.FindAsync(merchantId);
            if (merchant == null)
                throw new ArgumentException("Merchant not found");

            if (merchant.Role != UserRole.Merchant)
                throw new InvalidOperationException("Only merchants can perform bank withdrawals");

            if (!merchant.IsActive)
                throw new InvalidOperationException("Merchant account is inactive");

            if (merchant.IsLocked)
                throw new InvalidOperationException("Merchant account is locked");

            var transaction = await CreateTransactionAsync(merchantId, null, amount, TransactionType.BankWithdrawal, "Bank Withdrawal");
            return await CompleteTransactionAsync(transaction.TransactionId);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Sender)
                .Include(t => t.Recipient)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<TransactionHistory>> GetAllTransactionHistoriesAsync()
        {
            return await _context.TransactionHistories
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
} 