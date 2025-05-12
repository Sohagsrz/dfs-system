using Microsoft.EntityFrameworkCore;
using Scash.Data;
using Scash.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scash.Services
{
    public class SystemSettingsService
    {
        private readonly ScashDbContext _context;

        public SystemSettingsService(ScashDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetSetting(string key)
        {
            var setting = await _context.SystemSettings.FirstOrDefaultAsync(s => s.Key == key);
            return setting?.Value;
        }

        public async Task<T> GetSetting<T>(string key, T defaultValue = default)
        {
            var value = await GetSetting(key);
            if (string.IsNullOrEmpty(value)) return defaultValue;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        public async Task SetSetting(string key, string value, string description = null)
        {
            var setting = await _context.SystemSettings.FirstOrDefaultAsync(s => s.Key == key);
            if (setting == null)
            {
                setting = new SystemSettings
                {
                    Key = key,
                    Description = description
                };
                _context.SystemSettings.Add(setting);
            }

            setting.Value = value;
            setting.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<string, string>> GetAllSettings()
        {
            var settings = await _context.SystemSettings.ToListAsync();
            return settings.ToDictionary(s => s.Key, s => s.Value);
        }

        public async Task InitializeDefaultSettings()
        {
            var defaultSettings = new Dictionary<string, (string Value, string Description)>
            {
                { SystemSettingKeys.SendMoneyFee, ("0.50", "Fee for sending money (percentage)") },
                { SystemSettingKeys.CashOutFee, ("1.00", "Fee for cash out (percentage)") },
                { SystemSettingKeys.MerchantPaymentFee, ("0.25", "Fee for merchant payments (percentage)") },
                { SystemSettingKeys.MobileRechargeFee, ("0.00", "Fee for mobile recharge (percentage)") },
                { SystemSettingKeys.UtilityBillFee, ("0.00", "Fee for utility bill payments (percentage)") },
                { SystemSettingKeys.AgentCashInCommission, ("1.00", "Commission for cash in (percentage)") },
                { SystemSettingKeys.AgentCashOutCommission, ("1.00", "Commission for cash out (percentage)") },
                { SystemSettingKeys.PersonalDailyLimit, ("1000.00", "Daily transaction limit for personal accounts") },
                { SystemSettingKeys.PersonalMonthlyLimit, ("25000.00", "Monthly transaction limit for personal accounts") },
                { SystemSettingKeys.MerchantDailyLimit, ("50000.00", "Daily transaction limit for merchants") },
                { SystemSettingKeys.MerchantMonthlyLimit, ("1000000.00", "Monthly transaction limit for merchants") },
                { SystemSettingKeys.AgentDailyLimit, ("100000.00", "Daily transaction limit for agents") },
                { SystemSettingKeys.AgentMonthlyLimit, ("2000000.00", "Monthly transaction limit for agents") },
                { SystemSettingKeys.MaxFailedLoginAttempts, ("3", "Maximum failed login attempts before account lockout") },
                { SystemSettingKeys.AccountLockoutDuration, ("30", "Account lockout duration in minutes") },
                { SystemSettingKeys.PINChangeInterval, ("90", "PIN change interval in days") },
                { SystemSettingKeys.SessionTimeout, ("30", "Session timeout in minutes") },
                { SystemSettingKeys.SystemName, ("sCash", "System name") },
                { SystemSettingKeys.SupportPhone, ("+8801234567890", "Support phone number") },
                { SystemSettingKeys.SupportEmail, ("support@scash.com", "Support email address") },
                { SystemSettingKeys.MaintenanceMode, ("false", "System maintenance mode") }
            };

            foreach (var setting in defaultSettings)
            {
                if (!await _context.SystemSettings.AnyAsync(s => s.Key == setting.Key))
                {
                    await SetSetting(setting.Key, setting.Value.Value, setting.Value.Description);
                }
            }
        }
    }
} 