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
            // Add DbContext
            services.AddDbContext<ScashDbContext>(options =>
            {
                try
                {
                    options.UseMySql(
                        "Server=localhost;Database=scash_db;User=root;Password=;",
                        ServerVersion.AutoDetect("Server=localhost;Database=scash_db;User=root;Password=;")
                    );
                    Console.WriteLine("Database context configured successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error configuring database context: {ex}");
                    throw;
                }
            });

            // Add Services
            services.AddScoped<UserService>();
            services.AddScoped<TransactionService>();
            services.AddScoped<SystemSettingsService>();

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