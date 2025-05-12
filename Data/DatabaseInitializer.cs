using Microsoft.EntityFrameworkCore;
using Scash.Models;
using Scash.Services;

namespace Scash.Data
{
    public static class DatabaseInitializer
    {
        public static async Task Initialize(ScashDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Initialize system settings if none exist
            if (!await context.SystemSettings.AnyAsync())
            {
                var settingsService = new SystemSettingsService(context);
                await settingsService.InitializeDefaultSettings();
            }

            // Create default admin user if no users exist
            if (!await context.Users.AnyAsync())
            {
                var adminUser = new User
                {
                    Username = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    FullName = "System Administrator",
                    Email = "admin@scash.com",
                    PhoneNumber = "1234567890",
                    Balance = 0,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    Role = UserRole.Admin,
                    PIN = BCrypt.Net.BCrypt.HashPassword("1234"),
                    DailyTransactionLimit = 100000,
                    MonthlyTransactionLimit = 1000000
                };

                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
} 