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
                    fee = amount * await _settingsService.GetSetting<decimal>(SystemSettingKeys.TransactionFee);
                    break;
                case TransactionType.MobileRecharge:
                    fee = amount * await _settingsService.GetSetting<decimal>(SystemSettingKeys.MobileRechargeFee);
                    break;
                case TransactionType.UtilityBillPayment:
                    fee = amount * await _settingsService.GetSetting<decimal>(SystemSettingKeys.UtilityBillFee);
                    break;
                case TransactionType.QRPayment:
                    fee = amount * await _settingsService.GetSetting<decimal>(SystemSettingKeys.QRPaymentFee);
                    break;
            }

            // Calculate commission for agents
            if (sender.Role == UserRole.Agent)
            {
                commission = amount * await _settingsService.GetSetting<decimal>(SystemSettingKeys.AgentCommission);
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
            var transaction = await _context.Transactions
                .Include(t => t.Sender)
                .Include(t => t.Recipient)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);

            if (transaction == null)
                throw new ArgumentException("Transaction not found");

            if (transaction.Status != TransactionStatus.Pending)
                throw new InvalidOperationException("Transaction is not in pending status");

            using var dbTransaction = await _context.Database.BeginTransactionAsync();
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
                await dbTransaction.CommitAsync();

                return transaction;
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
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

        // Add stubs for legacy methods to avoid build errors
        public Task<Transaction> SendMoney(int senderId, string recipientUsername, decimal amount) => throw new NotImplementedException();
        public Task<Transaction> CashIn(int agentId, string recipientUsername, decimal amount) => throw new NotImplementedException();
        public Task<Transaction> CashOut(int userId, decimal amount) => throw new NotImplementedException();
        public Task<Transaction> PayMerchant(int senderId, string merchantUsername, decimal amount) => throw new NotImplementedException();
        public Task<Transaction> WithdrawToBank(int merchantId, decimal amount) => throw new NotImplementedException();
    }
} 