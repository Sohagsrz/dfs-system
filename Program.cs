using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scash.Data;
using Scash.Forms;
using Scash.Services;
using System;
using System.Windows.Forms;

namespace Scash
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Create service provider
                var services = new ServiceCollection();
                ConfigureServices(services);
                var serviceProvider = services.BuildServiceProvider();

                // Initialize database
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ScashDbContext>();
                    try
                    {
                        Console.WriteLine("Initializing database...");
                        await DatabaseInitializer.Initialize(context);
                        Console.WriteLine("Database initialized successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error initializing database: {ex}");
                        MessageBox.Show($"Error initializing database: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Start the application
                var loginForm = serviceProvider.GetRequiredService<LoginForm>();
                Application.Run(loginForm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application startup error: {ex}");
                MessageBox.Show($"Application startup error: {ex.Message}", "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Add DbContext with better connection handling
            services.AddDbContext<ScashDbContext>(options =>
            {
                try
                {
                    // Simple connection string for MySQL
                    var connectionString = "Server=localhost;Database=scash_db;User=root;Password=;Port=3306;" +
                                          "CharSet=utf8mb4;Convert Zero Datetime=True;Allow User Variables=True;" +
                                          "Connection Timeout=30;Command Timeout=30;";
                    
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                    
                    Console.WriteLine("Database context configured successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error configuring database context: {ex}");
                    
                    // Show user-friendly error message
                    var errorMessage = "Database connection failed. Please ensure:\n" +
                                     "1. MySQL server is running\n" +
                                     "2. MySQL is accessible on localhost:3306\n" +
                                     "3. User 'root' has proper permissions\n" +
                                     "4. Database 'scash_db' exists or can be created\n\n" +
                                     $"Technical error: {ex.Message}";
                    
                    MessageBox.Show(errorMessage, "Database Connection Error", 
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            });

            // Add Services with Transient lifetime to avoid connection reuse
            services.AddTransient<UserService>();
            services.AddTransient<TransactionService>();
            services.AddTransient<SystemSettingsService>();

            // Add Forms
            services.AddTransient<LoginForm>();
            services.AddTransient<RegistrationForm>();
            services.AddTransient<PersonalDashboardForm>();
            services.AddTransient<MerchantDashboardForm>();
            services.AddTransient<AgentDashboardForm>();
            services.AddTransient<AdminDashboardForm>();
        }
    }
}