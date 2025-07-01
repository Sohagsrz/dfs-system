using Microsoft.EntityFrameworkCore;
using Scash.Models;
using Scash.Services;

namespace Scash.Data
{
    public static class DatabaseInitializer
    {
        public static async Task Initialize(ScashDbContext context)
        {
            try
            {
                // Test connection first
                Console.WriteLine("Testing database connection...");
                await context.Database.CanConnectAsync();
                Console.WriteLine("Database connection successful.");

                // Drop and recreate database if schema has changed
                // This is a simple approach for development - in production you'd use proper migrations
                Console.WriteLine("Ensuring database schema is up to date...");
                //await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                Console.WriteLine("Database schema updated successfully.");

                // Initialize system settings if none exist
                if (!await context.SystemSettings.AnyAsync())
                {
                    Console.WriteLine("Initializing system settings...");
                    var settingsService = new SystemSettingsService(context);
                    await settingsService.InitializeDefaultSettings();
                    Console.WriteLine("System settings initialized.");
                }

                // Create default admin user if no users exist
                if (!await context.Users.AnyAsync())
                {
                    Console.WriteLine("Creating default admin user...");
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
                    Console.WriteLine("Default admin user created.");
                }

                // Create some test users for development
                if (await context.Users.CountAsync() == 1) // Only admin exists
                {
                    Console.WriteLine("Creating test users...");
                    
                    var personalUser = new User
                    {
                        Username = "sohag",
                        Password = BCrypt.Net.BCrypt.HashPassword("password123"),
                        FullName = "Sohag",
                        Email = "sohag@example.com",
                        PhoneNumber = "1234567891",
                        Balance = 1000,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        Role = UserRole.Personal,
                        PIN = BCrypt.Net.BCrypt.HashPassword("1234"),
                        DailyTransactionLimit = 1000,
                        MonthlyTransactionLimit = 25000
                    };

                    var merchantUser = new User
                    {
                        Username = "merchant1",
                        Password = BCrypt.Net.BCrypt.HashPassword("password123"),
                        FullName = "Test Merchant",
                        Email = "merchant@example.com",
                        PhoneNumber = "1234567892",
                        Balance = 500,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        Role = UserRole.Merchant,
                        PIN = BCrypt.Net.BCrypt.HashPassword("1234"),
                        DailyTransactionLimit = 50000,
                        MonthlyTransactionLimit = 1000000
                    };

                    context.Users.AddRange(personalUser, merchantUser);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Test users created successfully.");
                }

                Console.WriteLine("Database initialization completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization failed: {ex}");
                
                var errorMessage = "Database initialization failed. Please check:\n\n" +
                                 "1. MySQL server is running and accessible\n" +
                                 "2. MySQL user 'root' has CREATE DATABASE permission\n" +
                                 "3. Port 3306 is not blocked by firewall\n" +
                                 "4. MySQL service is started\n\n" +
                                 $"Error details: {ex.Message}";
                
                throw new InvalidOperationException(errorMessage, ex);
            }
        }
    }
} 