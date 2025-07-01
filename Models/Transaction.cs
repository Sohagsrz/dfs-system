using System;

namespace Scash.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; } // Primary Key
        public int SenderId { get; set; }
        public int? RecipientId { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public decimal Commission { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        public string ReferenceNumber { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public UtilityType? UtilityType { get; set; }

        // Navigation properties
        public User Sender { get; set; }
        public User Recipient { get; set; }
    }

    public class TransactionHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TransactionId { get; set; }
        public string ReferenceNumber { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal Commission { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Description { get; set; }

        // Navigation property
        public User User { get; set; }
    }

    public enum TransactionType
    {
        SendMoney,
        CashIn,
        CashOut,
        MerchantPayment,
        BankWithdrawal,
        MobileRecharge,
        UtilityBillPayment,
        RequestMoney,
        QRPayment,
        Commission
    }

    public enum TransactionStatus
    {
        Pending,
        Completed,
        Failed,
        Cancelled
    }

    public enum UtilityType
    {
        Electricity,
        Gas,
        Water,
        Internet
    }
} 