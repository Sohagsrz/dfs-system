using System;

namespace Scash.Models
{
    public class SystemSettings
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public static class SystemSettingKeys
    {
        // Transaction Fees
        public const string SendMoneyFee = "SendMoneyFee";
        public const string CashOutFee = "CashOutFee";
        public const string MerchantPaymentFee = "MerchantPaymentFee";
        public const string MobileRechargeFee = "MobileRechargeFee";
        public const string UtilityBillFee = "UtilityBillFee";
        public const string TransactionFee = "TransactionFee";
        public const string QRPaymentFee = "QRPaymentFee";

        // Commission Rates
        public const string AgentCashInCommission = "AgentCashInCommission";
        public const string AgentCashOutCommission = "AgentCashOutCommission";
        public const string AgentCommission = "AgentCommission";

        // Transaction Limits
        public const string PersonalDailyLimit = "PersonalDailyLimit";
        public const string PersonalMonthlyLimit = "PersonalMonthlyLimit";
        public const string MerchantDailyLimit = "MerchantDailyLimit";
        public const string MerchantMonthlyLimit = "MerchantMonthlyLimit";
        public const string AgentDailyLimit = "AgentDailyLimit";
        public const string AgentMonthlyLimit = "AgentMonthlyLimit";

        // Security Settings
        public const string MaxFailedLoginAttempts = "MaxFailedLoginAttempts";
        public const string AccountLockoutDuration = "AccountLockoutDuration";
        public const string PINChangeInterval = "PINChangeInterval";
        public const string SessionTimeout = "SessionTimeout";

        // System Settings
        public const string SystemName = "SystemName";
        public const string SupportPhone = "SupportPhone";
        public const string SupportEmail = "SupportEmail";
        public const string MaintenanceMode = "MaintenanceMode";
    }
} 