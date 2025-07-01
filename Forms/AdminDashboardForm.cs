using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scash.Models;
using Scash.Services;
using Scash.Data;
using Microsoft.EntityFrameworkCore;

namespace Scash.Forms
{
    public partial class AdminDashboardForm : Form
    {
        private readonly User _user;
        private UserService _userService;
        private TransactionService _transactionService;
        private SystemSettingsService _settingsService;
        private ScashDbContext _context;
        private System.Windows.Forms.Timer _activityTimer;
        private List<ActivityLog> _activityLogs;
        private List<Alert> _alerts;

        public AdminDashboardForm(User user, UserService userService, TransactionService transactionService)
        {
            _user = user;
            _userService = userService;
            _transactionService = transactionService;
            
            InitializeComponent();
            InitializeServices();
            InitializeTimer();
            LoadDashboardData();
            SetupEventHandlers();
        }

        private void InitializeServices()
        {
            // Get the context from the existing services
            _context = _userService.GetType().GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_userService) as ScashDbContext;
            if (_context == null)
            {
                // Fallback: create a new context if we can't get it from the service
                var connectionString = "Server=localhost;Database=scash;Uid=root;Pwd=;";
                var options = new DbContextOptionsBuilder<ScashDbContext>()
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .Options;
                _context = new ScashDbContext(options);
            }
            
            _settingsService = new SystemSettingsService(_context);
            _activityLogs = new List<ActivityLog>();
            _alerts = new List<Alert>();
        }

        private void InitializeTimer()
        {
            _activityTimer = new System.Windows.Forms.Timer();
            _activityTimer.Interval = 5000; // Update every 5 seconds
            _activityTimer.Tick += ActivityTimer_Tick;
            _activityTimer.Start();
        }

        private void ActivityTimer_Tick(object sender, EventArgs e)
        {
            UpdateActivityFeed();
            CheckForAlerts();
            UpdateDashboardMetrics();
        }

        private void LoadDashboardData()
        {
            LoadDashboardMetrics();
            LoadActivityFeed();
            LoadAlerts();
            LoadSystemSettings();
            LoadUserManagementData();
            LoadAnalyticsData();
        }

        private async void LoadDashboardMetrics()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                var transactions = await _transactionService.GetAllTransactionsAsync();
                
                var totalUsers = users.Count();
                var totalTransactions = transactions.Count();
                var totalVolume = transactions.Sum(t => t.Amount);
                var activeUsers = users.Count(u => u.LastLogin > DateTime.Now.AddDays(-7));

                lblTotalUsers.Text = totalUsers.ToString("N0");
                lblTotalTransactions.Text = totalTransactions.ToString("N0");
                lblTotalVolume.Text = $"₱{totalVolume:N2}";
                lblActiveUsers.Text = activeUsers.ToString("N0");

                // Update progress bars
                progressBarUsers.Value = Math.Min((int)((double)activeUsers / totalUsers * 100), 100);
                progressBarTransactions.Value = Math.Min((int)((double)totalTransactions / 1000 * 100), 100);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard metrics: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadActivityFeed()
        {
            try
            {
                // Simulate real-time activity data
                var activities = new List<ActivityLog>
                {
                    new ActivityLog { Timestamp = DateTime.Now.AddMinutes(-2), User = "john_doe", Action = "Completed transaction", Details = "Sent ₱500 to merchant_123", Type = ActivityType.Transaction },
                    new ActivityLog { Timestamp = DateTime.Now.AddMinutes(-5), User = "agent_001", Action = "Cash-out processed", Details = "Processed ₱1,000 cash-out for user_456", Type = ActivityType.CashOut },
                    new ActivityLog { Timestamp = DateTime.Now.AddMinutes(-8), User = "merchant_abc", Action = "Payment received", Details = "Received ₱750 payment from customer", Type = ActivityType.Payment },
                    new ActivityLog { Timestamp = DateTime.Now.AddMinutes(-12), User = "admin", Action = "System setting updated", Details = "Transaction fee updated to 2.5%", Type = ActivityType.System },
                    new ActivityLog { Timestamp = DateTime.Now.AddMinutes(-15), User = "user_789", Action = "Account registered", Details = "New personal account created", Type = ActivityType.Registration }
                };

                _activityLogs = activities;
                UpdateActivityFeed();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading activity feed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateActivityFeed()
        {
            if (listViewActivity.InvokeRequired)
            {
                listViewActivity.Invoke((Action)UpdateActivityFeed);
                return;
            }

            listViewActivity.Items.Clear();
            foreach (var activity in _activityLogs.OrderByDescending(a => a.Timestamp).Take(20))
            {
                var item = new ListViewItem(activity.Timestamp.ToString("HH:mm:ss"));
                item.SubItems.Add(activity.User);
                item.SubItems.Add(activity.Action);
                item.SubItems.Add(activity.Details);
                
                // Color code by activity type
                switch (activity.Type)
                {
                    case ActivityType.Transaction:
                        item.BackColor = Color.LightBlue;
                        break;
                    case ActivityType.CashOut:
                        item.BackColor = Color.LightGreen;
                        break;
                    case ActivityType.Payment:
                        item.BackColor = Color.LightYellow;
                        break;
                    case ActivityType.System:
                        item.BackColor = Color.LightCoral;
                        break;
                    case ActivityType.Registration:
                        item.BackColor = Color.LightGray;
                        break;
                }

                listViewActivity.Items.Add(item);
            }
        }

        private void LoadAlerts()
        {
            try
            {
                // Simulate alert data
                _alerts = new List<Alert>
                {
                    new Alert { Id = 1, Type = AlertType.SuspiciousActivity, Message = "Unusual transaction pattern detected for user_123", Severity = AlertSeverity.High, Timestamp = DateTime.Now.AddMinutes(-10) },
                    new Alert { Id = 2, Type = AlertType.SystemIssue, Message = "Database connection timeout", Severity = AlertSeverity.Medium, Timestamp = DateTime.Now.AddMinutes(-25) },
                    new Alert { Id = 3, Type = AlertType.FraudAttempt, Message = "Multiple failed login attempts from IP 192.168.1.100", Severity = AlertSeverity.Critical, Timestamp = DateTime.Now.AddMinutes(-45) }
                };

                UpdateAlertsList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading alerts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateAlertsList()
        {
            if (listViewAlerts.InvokeRequired)
            {
                listViewAlerts.Invoke((Action)UpdateAlertsList);
                return;
            }

            listViewAlerts.Items.Clear();
            foreach (var alert in _alerts.OrderByDescending(a => a.Timestamp))
            {
                var item = new ListViewItem(alert.Timestamp.ToString("HH:mm:ss"));
                item.SubItems.Add(alert.Type.ToString());
                item.SubItems.Add(alert.Message);
                item.SubItems.Add(alert.Severity.ToString());

                // Color code by severity
                switch (alert.Severity)
                {
                    case AlertSeverity.Low:
                        item.BackColor = Color.LightYellow;
                        break;
                    case AlertSeverity.Medium:
                        item.BackColor = Color.Orange;
                        break;
                    case AlertSeverity.High:
                        item.BackColor = Color.LightCoral;
                        break;
                    case AlertSeverity.Critical:
                        item.BackColor = Color.Red;
                        item.ForeColor = Color.White;
                        break;
                }

                listViewAlerts.Items.Add(item);
            }
        }

        private void CheckForAlerts()
        {
            // Simulate new alerts
            var random = new Random();
            if (random.Next(100) < 5) // 5% chance of new alert
            {
                var newAlert = new Alert
                {
                    Id = _alerts.Count + 1,
                    Type = (AlertType)random.Next(4),
                    Message = "New system alert generated",
                    Severity = (AlertSeverity)random.Next(4),
                    Timestamp = DateTime.Now
                };
                _alerts.Add(newAlert);
                UpdateAlertsList();
            }
        }

        private async void LoadSystemSettings()
        {
            try
            {
                // Initialize default settings if needed
                await _settingsService.InitializeDefaultSettings();

                // Load current settings
                var transactionFee = await _settingsService.GetSetting<double>(SystemSettingKeys.TransactionFee, 2.5);
                var cashOutFee = await _settingsService.GetSetting<double>(SystemSettingKeys.CashOutFee, 1.0);
                var cashInFee = await _settingsService.GetSetting<double>(SystemSettingKeys.AgentCashInCommission, 1.0);
                var agentCommission = await _settingsService.GetSetting<double>(SystemSettingKeys.AgentCommission, 1.0);
                var maintenanceMode = await _settingsService.GetSetting<bool>(SystemSettingKeys.MaintenanceMode, false);

                numTransactionFee.Value = (decimal)transactionFee;
                numCashOutFee.Value = (decimal)cashOutFee;
                numCashInFee.Value = (decimal)cashInFee;
                numAgentCommission.Value = (decimal)agentCommission;
                chkMaintenanceMode.Checked = maintenanceMode;

                // Load all settings and display them
                await LoadAllSettingsDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading system settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadAllSettingsDisplay()
        {
            try
            {
                // Clear existing controls in settings panel
                var controlsToRemove = panelSettings.Controls.OfType<Control>()
                    .Where(c => c != numTransactionFee && c != numCashOutFee && c != numCashInFee && 
                               c != numAgentCommission && c != chkMaintenanceMode && c != btnSaveSettings)
                    .ToList();

                foreach (var control in controlsToRemove)
                {
                    panelSettings.Controls.Remove(control);
                    control.Dispose();
                }

                // Get all settings
                var allSettings = await _settingsService.GetAllSettings();
                var yPosition = 450; // Start below the existing controls

                // Add header
                var headerLabel = new Label
                {
                    Text = "All System Settings (Key-Value Pairs)",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    Location = new Point(30, yPosition),
                    Size = new Size(400, 30),
                    ForeColor = Color.DarkBlue
                };
                panelSettings.Controls.Add(headerLabel);
                yPosition += 40;

                // Create a panel for scrollable settings
                var scrollPanel = new Panel
                {
                    Location = new Point(30, yPosition),
                    Size = new Size(900, 250),
                    AutoScroll = true,
                    BorderStyle = BorderStyle.FixedSingle
                };
                panelSettings.Controls.Add(scrollPanel);

                var settingsY = 10;
                foreach (var setting in allSettings.OrderBy(s => s.Key))
                {
                    // Setting key label
                    var keyLabel = new Label
                    {
                        Text = setting.Key,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        Location = new Point(10, settingsY),
                        Size = new Size(250, 20),
                        ForeColor = Color.DarkGreen
                    };
                    scrollPanel.Controls.Add(keyLabel);

                    // Setting value label
                    var valueLabel = new Label
                    {
                        Text = setting.Value,
                        Font = new Font("Segoe UI", 10),
                        Location = new Point(270, settingsY),
                        Size = new Size(300, 20),
                        ForeColor = Color.Black
                    };
                    scrollPanel.Controls.Add(valueLabel);

                    // Description (if available)
                    var descriptionLabel = new Label
                    {
                        Text = GetSettingDescription(setting.Key),
                        Font = new Font("Segoe UI", 8),
                        Location = new Point(580, settingsY),
                        Size = new Size(300, 20),
                        ForeColor = Color.Gray
                    };
                    scrollPanel.Controls.Add(descriptionLabel);

                    settingsY += 25;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings display: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetSettingDescription(string key)
        {
            return key switch
            {
                SystemSettingKeys.TransactionFee => "General transaction fee (%)",
                SystemSettingKeys.SendMoneyFee => "Fee for sending money (%)",
                SystemSettingKeys.CashOutFee => "Fee for cash out (%)",
                SystemSettingKeys.AgentCashInCommission => "Agent cash-in commission (%)",
                SystemSettingKeys.MerchantPaymentFee => "Fee for merchant payments (%)",
                SystemSettingKeys.MobileRechargeFee => "Fee for mobile recharge (%)",
                SystemSettingKeys.UtilityBillFee => "Fee for utility bill payments (%)",
                SystemSettingKeys.QRPaymentFee => "Fee for QR payments (%)",
                SystemSettingKeys.AgentCommission => "Agent commission rate (%)",
                SystemSettingKeys.AgentCashOutCommission => "Agent cash-out commission (%)",
                SystemSettingKeys.PersonalDailyLimit => "Daily limit for personal users",
                SystemSettingKeys.PersonalMonthlyLimit => "Monthly limit for personal users",
                SystemSettingKeys.MerchantDailyLimit => "Daily limit for merchants",
                SystemSettingKeys.MerchantMonthlyLimit => "Monthly limit for merchants",
                SystemSettingKeys.AgentDailyLimit => "Daily limit for agents",
                SystemSettingKeys.AgentMonthlyLimit => "Monthly limit for agents",
                SystemSettingKeys.MaxFailedLoginAttempts => "Max failed login attempts",
                SystemSettingKeys.AccountLockoutDuration => "Account lockout duration (minutes)",
                SystemSettingKeys.PINChangeInterval => "PIN change interval (days)",
                SystemSettingKeys.SessionTimeout => "Session timeout (minutes)",
                SystemSettingKeys.SystemName => "System name",
                SystemSettingKeys.SupportPhone => "Support phone number",
                SystemSettingKeys.SupportEmail => "Support email address",
                SystemSettingKeys.MaintenanceMode => "System maintenance mode",
                _ => "System setting"
            };
        }

        private async void LoadUserManagementData()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                dataGridViewUsers.DataSource = users;

                // Update user statistics
                var personalUsers = users.Count(u => u.Role == UserRole.Personal);
                var merchants = users.Count(u => u.Role == UserRole.Merchant);
                var agents = users.Count(u => u.Role == UserRole.Agent);
                var admins = users.Count(u => u.Role == UserRole.Admin);

                lblPersonalUsers.Text = personalUsers.ToString();
                lblMerchants.Text = merchants.ToString();
                lblAgents.Text = agents.ToString();
                lblAdmins.Text = admins.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user management data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadAnalyticsData()
        {
            try
            {
                // Load transaction data for analytics
                var transactions = await _transactionService.GetAllTransactionsAsync();
                var totalVolume = transactions.Sum(t => t.Amount);
                var avgTransaction = transactions.Any() ? transactions.Average(t => t.Amount) : 0;
                var transactionCount = transactions.Count();

                // Update analytics labels (we'll add these to the designer later)
                // lblTotalVolume.Text = $"₱{totalVolume:N2}";
                // lblAvgTransaction.Text = $"₱{avgTransaction:N2}";
                // lblTransactionCount.Text = transactionCount.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading analytics data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDashboardMetrics()
        {
            // Update real-time metrics
            var random = new Random();
            var currentVolume = decimal.Parse(lblTotalVolume.Text.Replace("₱", "").Replace(",", ""));
            currentVolume += random.Next(-100, 500);
            lblTotalVolume.Text = $"₱{currentVolume:N2}";

            var currentTransactions = int.Parse(lblTotalTransactions.Text.Replace(",", ""));
            currentTransactions += random.Next(1, 10);
            lblTotalTransactions.Text = currentTransactions.ToString("N0");
        }

        private void SetupEventHandlers()
        {
            // Sidebar navigation
            btnDashboard.Click += (s, e) => ShowPanel(panelDashboard);
            btnActivity.Click += (s, e) => ShowPanel(panelActivity);
            btnAlerts.Click += (s, e) => ShowPanel(panelAlerts);
            btnSettings.Click += (s, e) => ShowPanel(panelSettings);
            btnUsers.Click += (s, e) => ShowPanel(panelUsers);
            btnAnalytics.Click += (s, e) => ShowPanel(panelAnalytics);

            // Settings save
            btnSaveSettings.Click += BtnSaveSettings_Click;

            // User management
            btnFreezeUser.Click += BtnFreezeUser_Click;
            btnUnfreezeUser.Click += BtnUnfreezeUser_Click;
            btnDeleteUser.Click += BtnDeleteUser_Click;

            // Alert actions
            btnDismissAlert.Click += BtnDismissAlert_Click;
            btnViewAlertDetails.Click += BtnViewAlertDetails_Click;
        }

        private void ShowPanel(Panel panel)
        {
            // Hide all panels
            panelDashboard.Visible = false;
            panelActivity.Visible = false;
            panelAlerts.Visible = false;
            panelSettings.Visible = false;
            panelUsers.Visible = false;
            panelAnalytics.Visible = false;

            // Show selected panel
            panel.Visible = true;

            // Update sidebar button states
            UpdateSidebarButtonStates(panel);
        }

        private void UpdateSidebarButtonStates(Panel activePanel)
        {
            var buttons = new[] { btnDashboard, btnActivity, btnAlerts, btnSettings, btnUsers, btnAnalytics };
            var panels = new[] { panelDashboard, panelActivity, panelAlerts, panelSettings, panelUsers, panelAnalytics };

            for (int i = 0; i < buttons.Length; i++)
            {
                if (panels[i] == activePanel)
                {
                    buttons[i].BackColor = Color.FromArgb(52, 152, 219);
                    buttons[i].ForeColor = Color.White;
                }
                else
                {
                    buttons[i].BackColor = Color.FromArgb(44, 62, 80);
                    buttons[i].ForeColor = Color.White;
                }
            }
        }

        private async void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                await _settingsService.SetSetting(SystemSettingKeys.TransactionFee, numTransactionFee.Value.ToString());
                await _settingsService.SetSetting(SystemSettingKeys.CashOutFee, numCashOutFee.Value.ToString());
                await _settingsService.SetSetting(SystemSettingKeys.AgentCashInCommission, numCashInFee.Value.ToString());
                await _settingsService.SetSetting(SystemSettingKeys.AgentCommission, numAgentCommission.Value.ToString());
                await _settingsService.SetSetting(SystemSettingKeys.MaintenanceMode, chkMaintenanceMode.Checked.ToString());

                MessageBox.Show("System settings updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnFreezeUser_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                var user = (User)dataGridViewUsers.SelectedRows[0].DataBoundItem;
                try
                {
                    await _userService.SetUserActive(user.Id, false);
                    LoadUserManagementData();
                    MessageBox.Show($"User {user.Username} has been frozen.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error freezing user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a user to freeze.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void BtnUnfreezeUser_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                var user = (User)dataGridViewUsers.SelectedRows[0].DataBoundItem;
                try
                {
                    await _userService.SetUserActive(user.Id, true);
                    LoadUserManagementData();
                    MessageBox.Show($"User {user.Username} has been unfrozen.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error unfreezing user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a user to unfreeze.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                var user = (User)dataGridViewUsers.SelectedRows[0].DataBoundItem;
                var result = MessageBox.Show($"Are you sure you want to delete user {user.Username}? This action cannot be undone.", 
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        await _userService.DeleteUser(user.Id);
                        LoadUserManagementData();
                        MessageBox.Show($"User {user.Username} has been deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDismissAlert_Click(object sender, EventArgs e)
        {
            if (listViewAlerts.SelectedItems.Count > 0)
            {
                var selectedIndex = listViewAlerts.SelectedIndices[0];
                if (selectedIndex < _alerts.Count)
                {
                    _alerts.RemoveAt(selectedIndex);
                    UpdateAlertsList();
                    MessageBox.Show("Alert dismissed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select an alert to dismiss.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnViewAlertDetails_Click(object sender, EventArgs e)
        {
            if (listViewAlerts.SelectedItems.Count > 0)
            {
                var selectedIndex = listViewAlerts.SelectedIndices[0];
                if (selectedIndex < _alerts.Count)
                {
                    var alert = _alerts[selectedIndex];
                    MessageBox.Show($"Alert Details:\n\nType: {alert.Type}\nSeverity: {alert.Severity}\nMessage: {alert.Message}\nTime: {alert.Timestamp}", 
                        "Alert Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select an alert to view details.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _activityTimer?.Stop();
            _activityTimer?.Dispose();
            _context?.Dispose();
            base.OnFormClosing(e);
        }
    }

    // Supporting classes
    public class ActivityLog
    {
        public DateTime Timestamp { get; set; }
        public string User { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public ActivityType Type { get; set; }
    }

    public enum ActivityType
    {
        Transaction,
        CashOut,
        Payment,
        System,
        Registration
    }

    public class Alert
    {
        public int Id { get; set; }
        public AlertType Type { get; set; }
        public string Message { get; set; }
        public AlertSeverity Severity { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public enum AlertType
    {
        SuspiciousActivity,
        SystemIssue,
        FraudAttempt,
        Maintenance
    }

    public enum AlertSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }
} 