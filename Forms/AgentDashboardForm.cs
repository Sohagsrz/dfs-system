using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scash.Models;
using Scash.Services;

namespace Scash.Forms
{
    public partial class AgentDashboardForm : Form
    {
        private readonly User _user;
        private readonly UserService _userService;
        private readonly TransactionService _transactionService;
        private DataGridView transactionHistoryGrid;
        private decimal _totalCommission = 0.00m;

        public AgentDashboardForm(User user, UserService userService, TransactionService transactionService)
        {
            _user = user;
            _userService = userService;
            _transactionService = transactionService;
            InitializeComponent();
            
            // Ensure panel is properly initialized
            panel1.Visible = true;
            panel1.BackColor = System.Drawing.Color.White;
            panel1.BringToFront();
            
            LoadDashboard();
            SetupEventHandlers();
        }

        private void SetupEventHandlers()
        {
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
        }

        private async void LoadDashboard()
        {
            try
            {
                Console.WriteLine("Loading agent dashboard...");
                var updatedUser = await _userService.GetUserById(_user.Id);
                if (updatedUser != null)
                {
                    _user.Balance = updatedUser.Balance;
                }

                await CalculateTotalCommission();
                UpdateBalance();
                UpdateCommission();
                welcomeTxt.Text = $"Welcome, {_user.FullName}";
                
                Console.WriteLine("Showing dashboard panel...");
                ShowDashboardPanel();
                Console.WriteLine("Agent dashboard loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading agent dashboard: {ex}");
                MessageBox.Show($"Error loading dashboard: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CalculateTotalCommission()
        {
            try
            {
                var transactions = await _transactionService.GetUserTransactionHistoryAsync(_user.Id, null, null, null);
                _totalCommission = transactions
                    .Where(t => t.Type == TransactionType.CashIn || t.Type == TransactionType.CashOut)
                    .Sum(t => t.Commission);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating commission: {ex.Message}");
                _totalCommission = 0.00m;
            }
        }

        private void UpdateBalance()
        {
            lblBalance.Text = $"Balance: ${_user.Balance:N2}";
        }

        private void UpdateCommission()
        {
            lblCommission.Text = $"Commission: ${_totalCommission:N2}";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = listBox1.SelectedItem?.ToString();
            switch (selectedItem)
            {
                case "Dashboard":
                    ShowDashboardPanel();
                    break;
                case "Cash In":
                    ShowCashInPanel();
                    break;
                case "Cash Out":
                    ShowCashOutPanel();
                    break;
                case "Earnings Report":
                    ShowEarningsReportPanel();
                    break;
                case "Transaction Log":
                    ShowTransactionLogPanel();
                    break;
                case "Performance":
                    ShowPerformancePanel();
                    break;
                case "Settings":
                    ShowSettingsPanel();
                    break;
                case "Log Out":
                    LogOut();
                    break;
            }
        }

        private void ShowDashboardPanel()
        {
            Console.WriteLine("ShowDashboardPanel called");
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;
            Console.WriteLine($"Panel visible: {panel1.Visible}, Panel size: {panel1.Size}");

            var headerLabel = new Label
            {
                Text = "Agent Dashboard Overview",
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(500, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(headerLabel);
            Console.WriteLine("Added header label");

            var balanceCard = new Panel
            {
                Location = new System.Drawing.Point(30, 120),
                Size = new System.Drawing.Size(250, 120),
                BackColor = System.Drawing.Color.LightBlue,
                BorderStyle = BorderStyle.FixedSingle
            };

            var balanceLabel = new Label
            {
                Text = "Current Balance",
                Location = new System.Drawing.Point(15, 15),
                Size = new System.Drawing.Size(220, 25),
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkBlue
            };

            var balanceAmount = new Label
            {
                Text = $"${_user.Balance:N2}",
                Location = new System.Drawing.Point(15, 45),
                Size = new System.Drawing.Size(220, 40),
                Font = new System.Drawing.Font("Segoe UI", 20, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkBlue
            };

            balanceCard.Controls.Add(balanceLabel);
            balanceCard.Controls.Add(balanceAmount);
            panel1.Controls.Add(balanceCard);
            Console.WriteLine("Added balance card");

            var commissionCard = new Panel
            {
                Location = new System.Drawing.Point(300, 120),
                Size = new System.Drawing.Size(250, 120),
                BackColor = System.Drawing.Color.LightGreen,
                BorderStyle = BorderStyle.FixedSingle
            };

            var commissionLabel = new Label
            {
                Text = "Total Commission",
                Location = new System.Drawing.Point(15, 15),
                Size = new System.Drawing.Size(220, 25),
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkGreen
            };

            var commissionAmount = new Label
            {
                Text = $"${_totalCommission:N2}",
                Location = new System.Drawing.Point(15, 45),
                Size = new System.Drawing.Size(220, 40),
                Font = new System.Drawing.Font("Segoe UI", 20, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkGreen
            };

            commissionCard.Controls.Add(commissionLabel);
            commissionCard.Controls.Add(commissionAmount);
            panel1.Controls.Add(commissionCard);
            Console.WriteLine("Added commission card");

            var quickActionsLabel = new Label
            {
                Text = "Quick Actions",
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 270),
                Size = new System.Drawing.Size(250, 30),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(quickActionsLabel);

            var cashInBtn = new Button
            {
                Text = "Cash In",
                Location = new System.Drawing.Point(30, 310),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            cashInBtn.Click += (s, e) => ShowCashInPanel();
            panel1.Controls.Add(cashInBtn);

            var cashOutBtn = new Button
            {
                Text = "Cash Out",
                Location = new System.Drawing.Point(180, 310),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.Orange,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            cashOutBtn.Click += (s, e) => ShowCashOutPanel();
            panel1.Controls.Add(cashOutBtn);

            var earningsReportBtn = new Button
            {
                Text = "Earnings Report",
                Location = new System.Drawing.Point(330, 310),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.Blue,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            earningsReportBtn.Click += (s, e) => ShowEarningsReportPanel();
            panel1.Controls.Add(earningsReportBtn);

            var performanceBtn = new Button
            {
                Text = "Performance",
                Location = new System.Drawing.Point(480, 310),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.Purple,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            performanceBtn.Click += (s, e) => ShowPerformancePanel();
            panel1.Controls.Add(performanceBtn);

            var transactionLogBtn = new Button
            {
                Text = "Transaction Log",
                Location = new System.Drawing.Point(30, 360),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.Teal,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            transactionLogBtn.Click += (s, e) => ShowTransactionLogPanel();
            panel1.Controls.Add(transactionLogBtn);

            var viewAllHistoryBtn = new Button
            {
                Text = "View Full Transaction History",
                Location = new System.Drawing.Point(180, 360),
                Size = new System.Drawing.Size(200, 40),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            viewAllHistoryBtn.Click += (s, e) => ShowFullTransactionHistory();
            panel1.Controls.Add(viewAllHistoryBtn);

            Console.WriteLine($"Total controls in panel: {panel1.Controls.Count}");

            // Force panel refresh
            panel1.Refresh();
            panel1.Invalidate();
            Console.WriteLine("Panel refreshed and invalidated");

            LoadRecentTransactions();
        }

        private async void LoadRecentTransactions()
        {
            try
            {
                var transactions = await _transactionService.GetUserTransactionHistoryAsync(_user.Id, null, null, null);
                var recentTransactions = transactions.Take(5).ToList();

                var transactionsLabel = new Label
                {
                    Text = "Recent Transactions",
                    Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                    Location = new System.Drawing.Point(400, 120),
                    Size = new System.Drawing.Size(250, 30),
                    ForeColor = System.Drawing.Color.DarkBlue
                };
                panel1.Controls.Add(transactionsLabel);

                transactionHistoryGrid = new DataGridView
                {
                    Location = new System.Drawing.Point(400, 160),
                    Size = new System.Drawing.Size(500, 300),
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    BackgroundColor = System.Drawing.Color.White,
                    BorderStyle = BorderStyle.Fixed3D,
                    GridColor = System.Drawing.Color.LightGray
                };

                transactionHistoryGrid.Columns.Add("Date", "Date");
                transactionHistoryGrid.Columns.Add("Type", "Type");
                transactionHistoryGrid.Columns.Add("Amount", "Amount");
                transactionHistoryGrid.Columns.Add("Commission", "Commission");
                transactionHistoryGrid.Columns.Add("Balance", "Balance");

                foreach (var transaction in recentTransactions)
                {
                    transactionHistoryGrid.Rows.Add(
                        transaction.CreatedAt.ToString("MM/dd/yyyy"),
                        transaction.Type.ToString(),
                        $"${transaction.Amount:N2}",
                        $"${transaction.Commission:N2}",
                        $"${transaction.Balance:N2}"
                    );
                }

                panel1.Controls.Add(transactionHistoryGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading transactions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowCashInPanel()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Cash In - Add Money to User Account",
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(500, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(headerLabel);

            var userLabel = new Label
            {
                Text = "User Username:",
                Location = new System.Drawing.Point(30, 120),
                Size = new System.Drawing.Size(180, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            panel1.Controls.Add(userLabel);

            var userTextBox = new TextBox
            {
                Location = new System.Drawing.Point(220, 120),
                Size = new System.Drawing.Size(250, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(userTextBox);

            var amountLabel = new Label
            {
                Text = "Amount ($):",
                Location = new System.Drawing.Point(30, 160),
                Size = new System.Drawing.Size(180, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            panel1.Controls.Add(amountLabel);

            var amountTextBox = new TextBox
            {
                Location = new System.Drawing.Point(220, 160),
                Size = new System.Drawing.Size(250, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(amountTextBox);

            var descriptionLabel = new Label
            {
                Text = "Description:",
                Location = new System.Drawing.Point(30, 200),
                Size = new System.Drawing.Size(180, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            panel1.Controls.Add(descriptionLabel);

            var descriptionTextBox = new TextBox
            {
                Location = new System.Drawing.Point(220, 200),
                Size = new System.Drawing.Size(250, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(descriptionTextBox);

            var cashInButton = new Button
            {
                Text = "Process Cash In",
                Location = new System.Drawing.Point(220, 240),
                Size = new System.Drawing.Size(140, 35),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            cashInButton.Click += async (s, e) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(userTextBox.Text) || string.IsNullOrWhiteSpace(amountTextBox.Text))
                    {
                        MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!decimal.TryParse(amountTextBox.Text, out decimal amount) || amount <= 0)
                    {
                        MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!PromptForPIN())
                    {
                        return;
                    }

                    await _transactionService.CashIn(_user.Id, userTextBox.Text, amount);
                    MessageBox.Show("Cash in processed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var updatedUser = await _userService.GetUserById(_user.Id);
                    if (updatedUser != null)
                    {
                        _user.Balance = updatedUser.Balance;
                        UpdateBalance();
                    }

                    await CalculateTotalCommission();
                    UpdateCommission();

                    ShowDashboardPanel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing cash in: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panel1.Controls.Add(cashInButton);

            var backButton = new Button
            {
                Text = "Back to Dashboard",
                Location = new System.Drawing.Point(370, 240),
                Size = new System.Drawing.Size(140, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            backButton.Click += (s, e) => ShowDashboardPanel();
            panel1.Controls.Add(backButton);

            panel1.Refresh();
            panel1.Invalidate();
        }

        private void ShowCashOutPanel()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Cash Out - Withdraw to Bank",
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(500, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(headerLabel);

            var userLabel = new Label
            {
                Text = "User Username:",
                Location = new System.Drawing.Point(30, 120),
                Size = new System.Drawing.Size(180, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            panel1.Controls.Add(userLabel);

            var userTextBox = new TextBox
            {
                Location = new System.Drawing.Point(220, 120),
                Size = new System.Drawing.Size(250, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(userTextBox);

            var amountLabel = new Label
            {
                Text = "Amount ($):",
                Location = new System.Drawing.Point(30, 160),
                Size = new System.Drawing.Size(180, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            panel1.Controls.Add(amountLabel);

            var amountTextBox = new TextBox
            {
                Location = new System.Drawing.Point(220, 160),
                Size = new System.Drawing.Size(250, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(amountTextBox);

            var bankInfoLabel = new Label
            {
                Text = "Bank Information:",
                Location = new System.Drawing.Point(30, 200),
                Size = new System.Drawing.Size(180, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            panel1.Controls.Add(bankInfoLabel);

            var bankInfoTextBox = new TextBox
            {
                Location = new System.Drawing.Point(220, 200),
                Size = new System.Drawing.Size(250, 25),
                Font = new System.Drawing.Font("Segoe UI", 11),
                PlaceholderText = "Bank Name, Account Number"
            };
            panel1.Controls.Add(bankInfoTextBox);

            var cashOutButton = new Button
            {
                Text = "Process Cash Out",
                Location = new System.Drawing.Point(220, 240),
                Size = new System.Drawing.Size(140, 35),
                BackColor = System.Drawing.Color.Orange,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            cashOutButton.Click += async (s, e) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(userTextBox.Text) || string.IsNullOrWhiteSpace(amountTextBox.Text))
                    {
                        MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!decimal.TryParse(amountTextBox.Text, out decimal amount) || amount <= 0)
                    {
                        MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!PromptForPIN())
                    {
                        return;
                    }

                    // For cash out, we need to find the user and process their withdrawal
                    var user = await _userService.GetUserByUsername(userTextBox.Text);
                    if (user == null)
                    {
                        MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    await _transactionService.CashOut(user.Id, amount);
                    MessageBox.Show("Cash out request submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var updatedUser = await _userService.GetUserById(_user.Id);
                    if (updatedUser != null)
                    {
                        _user.Balance = updatedUser.Balance;
                        UpdateBalance();
                    }

                    await CalculateTotalCommission();
                    UpdateCommission();

                    ShowDashboardPanel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing cash out: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panel1.Controls.Add(cashOutButton);

            var backButton = new Button
            {
                Text = "Back to Dashboard",
                Location = new System.Drawing.Point(370, 240),
                Size = new System.Drawing.Size(140, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            backButton.Click += (s, e) => ShowDashboardPanel();
            panel1.Controls.Add(backButton);

            panel1.Refresh();
            panel1.Invalidate();
        }

        private void ShowEarningsReportPanel()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Earnings Report",
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(300, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(headerLabel);

            var periodLabel = new Label
            {
                Text = "Report Period:",
                Location = new System.Drawing.Point(30, 120),
                Size = new System.Drawing.Size(150, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            panel1.Controls.Add(periodLabel);

            var periodComboBox = new ComboBox
            {
                Location = new System.Drawing.Point(200, 120),
                Size = new System.Drawing.Size(150, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            periodComboBox.Items.AddRange(new object[] { "Today", "This Week", "This Month", "Last Month", "This Year" });
            periodComboBox.SelectedIndex = 0;
            panel1.Controls.Add(periodComboBox);

            var generateButton = new Button
            {
                Text = "Generate Report",
                Location = new System.Drawing.Point(370, 120),
                Size = new System.Drawing.Size(140, 25),
                BackColor = System.Drawing.Color.Blue,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            generateButton.Click += async (s, e) => await GenerateEarningsReport(periodComboBox.SelectedItem?.ToString());
            panel1.Controls.Add(generateButton);

            var reportGrid = new DataGridView
            {
                Location = new System.Drawing.Point(30, 160),
                Size = new System.Drawing.Size(850, 400),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                GridColor = System.Drawing.Color.LightGray
            };

            reportGrid.Columns.Add("Date", "Date");
            reportGrid.Columns.Add("Type", "Type");
            reportGrid.Columns.Add("User", "User");
            reportGrid.Columns.Add("Amount", "Amount");
            reportGrid.Columns.Add("Commission", "Commission");
            reportGrid.Columns.Add("Description", "Description");

            panel1.Controls.Add(reportGrid);

            var backButton = new Button
            {
                Text = "Back to Dashboard",
                Location = new System.Drawing.Point(740, 120),
                Size = new System.Drawing.Size(140, 25),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            backButton.Click += (s, e) => ShowDashboardPanel();
            panel1.Controls.Add(backButton);

            panel1.Refresh();
            panel1.Invalidate();
        }

        private async Task GenerateEarningsReport(string period)
        {
            try
            {
                var reportGrid = panel1.Controls.OfType<DataGridView>().FirstOrDefault();
                if (reportGrid != null)
                {
                    reportGrid.Rows.Clear();
                    
                    // Sample data - in a real application, this would come from the database
                    var sampleData = new[]
                    {
                        new { Date = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy"), Type = "Cash In", User = "john_doe", Amount = 500.00m, Commission = 5.00m, Description = "Cash In Service" },
                        new { Date = DateTime.Now.AddDays(-2).ToString("MM/dd/yyyy"), Type = "Cash Out", User = "jane_smith", Amount = 300.00m, Commission = 3.00m, Description = "Bank Withdrawal" },
                        new { Date = DateTime.Now.AddDays(-3).ToString("MM/dd/yyyy"), Type = "Cash In", User = "bob_wilson", Amount = 750.00m, Commission = 7.50m, Description = "Cash In Service" }
                    };

                    foreach (var item in sampleData)
                    {
                        reportGrid.Rows.Add(
                            item.Date,
                            item.Type,
                            item.User,
                            $"${item.Amount:N2}",
                            $"${item.Commission:N2}",
                            item.Description
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowTransactionLogPanel()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Transaction Log",
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(300, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(headerLabel);

            var refreshButton = new Button
            {
                Text = "Refresh Log",
                Location = new System.Drawing.Point(30, 120),
                Size = new System.Drawing.Size(120, 30),
                BackColor = System.Drawing.Color.Orange,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            refreshButton.Click += async (s, e) => await LoadFullTransactionLog();
            panel1.Controls.Add(refreshButton);

            var logGrid = new DataGridView
            {
                Location = new System.Drawing.Point(30, 160),
                Size = new System.Drawing.Size(850, 400),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                GridColor = System.Drawing.Color.LightGray
            };

            logGrid.Columns.Add("Date", "Date");
            logGrid.Columns.Add("Type", "Type");
            logGrid.Columns.Add("User", "User");
            logGrid.Columns.Add("Amount", "Amount");
            logGrid.Columns.Add("Commission", "Commission");
            logGrid.Columns.Add("Balance", "Balance");
            logGrid.Columns.Add("Description", "Description");

            panel1.Controls.Add(logGrid);

            var backButton = new Button
            {
                Text = "Back to Dashboard",
                Location = new Point(170, 120),
                Size = new Size(120, 30),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            backButton.Click += (s, e) => ShowDashboardPanel();
            panel1.Controls.Add(backButton);

            LoadFullTransactionLog();

            panel1.Refresh();
            panel1.Invalidate();
        }

        private async Task LoadFullTransactionLog()
        {
            try
            {
                var logGrid = panel1.Controls.OfType<DataGridView>().FirstOrDefault();
                if (logGrid != null)
                {
                    logGrid.Rows.Clear();
                    var transactions = await _transactionService.GetUserTransactionHistoryAsync(_user.Id, null, null, null);
                    foreach (var transaction in transactions.OrderByDescending(t => t.CreatedAt))
                    {
                        logGrid.Rows.Add(
                            transaction.CreatedAt.ToString("MM/dd/yyyy HH:mm"),
                            transaction.Type.ToString(),
                            "N/A", // User info would need to be retrieved separately
                            $"${transaction.Amount:N2}",
                            $"${transaction.Commission:N2}",
                            $"${transaction.Balance:N2}",
                            transaction.Description
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading transaction log: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowPerformancePanel()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Agent Performance",
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(300, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(headerLabel);

            var performanceLabel = new Label
            {
                Text = "Performance Metrics",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 120),
                Size = new System.Drawing.Size(300, 30),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(performanceLabel);

            var totalTransactionsLabel = new Label
            {
                Text = $"Total Transactions: 150",
                Location = new System.Drawing.Point(30, 160),
                Size = new System.Drawing.Size(400, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(totalTransactionsLabel);

            var totalCommissionLabel = new Label
            {
                Text = $"Total Commission Earned: ${_totalCommission:N2}",
                Location = new System.Drawing.Point(30, 190),
                Size = new System.Drawing.Size(400, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(totalCommissionLabel);

            var avgCommissionLabel = new Label
            {
                Text = $"Average Commission per Transaction: ${(_totalCommission > 0 ? _totalCommission / 150 : 0):N2}",
                Location = new System.Drawing.Point(30, 220),
                Size = new System.Drawing.Size(400, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(avgCommissionLabel);

            var ratingLabel = new Label
            {
                Text = $"Performance Rating: ⭐⭐⭐⭐⭐ (5.0/5.0)",
                Location = new System.Drawing.Point(30, 250),
                Size = new System.Drawing.Size(400, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.Green
            };
            panel1.Controls.Add(ratingLabel);

            var feedbackLabel = new Label
            {
                Text = "Admin Feedback:",
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 290),
                Size = new System.Drawing.Size(300, 25),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(feedbackLabel);

            var feedbackText = new Label
            {
                Text = "Excellent performance! You have consistently provided high-quality service to users. Keep up the great work!",
                Location = new System.Drawing.Point(30, 320),
                Size = new System.Drawing.Size(600, 60),
                Font = new System.Drawing.Font("Segoe UI", 11),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(feedbackText);

            var backButton = new Button
            {
                Text = "Back to Dashboard",
                Location = new System.Drawing.Point(30, 400),
                Size = new System.Drawing.Size(140, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            backButton.Click += (s, e) => ShowDashboardPanel();
            panel1.Controls.Add(backButton);

            panel1.Refresh();
            panel1.Invalidate();
        }

        private void ShowSettingsPanel()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Agent Settings",
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(300, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(headerLabel);

            var profileLabel = new Label
            {
                Text = "Profile Information",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 120),
                Size = new System.Drawing.Size(300, 30),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(profileLabel);

            var nameLabel = new Label
            {
                Text = $"Agent Name: {_user.FullName}",
                Location = new System.Drawing.Point(30, 160),
                Size = new System.Drawing.Size(400, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(nameLabel);

            var emailLabel = new Label
            {
                Text = $"Email: {_user.Email}",
                Location = new System.Drawing.Point(30, 190),
                Size = new System.Drawing.Size(400, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(emailLabel);

            var phoneLabel = new Label
            {
                Text = $"Phone: {_user.PhoneNumber}",
                Location = new System.Drawing.Point(30, 220),
                Size = new System.Drawing.Size(400, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(phoneLabel);

            var balanceLabel = new Label
            {
                Text = $"Current Balance: ${_user.Balance:N2}",
                Location = new System.Drawing.Point(30, 250),
                Size = new System.Drawing.Size(400, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkGreen
            };
            panel1.Controls.Add(balanceLabel);

            var commissionLabel = new Label
            {
                Text = $"Total Commission: ${_totalCommission:N2}",
                Location = new System.Drawing.Point(30, 280),
                Size = new System.Drawing.Size(400, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkGreen
            };
            panel1.Controls.Add(commissionLabel);

            var backButton = new Button
            {
                Text = "Back to Dashboard",
                Location = new System.Drawing.Point(30, 320),
                Size = new System.Drawing.Size(140, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            backButton.Click += (s, e) => ShowDashboardPanel();
            panel1.Controls.Add(backButton);

            panel1.Refresh();
            panel1.Invalidate();
        }

        private void ShowFullTransactionHistory()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Full Transaction History",
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(300, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(headerLabel);

            var refreshButton = new Button
            {
                Text = "Refresh",
                Location = new System.Drawing.Point(30, 120),
                Size = new System.Drawing.Size(120, 30),
                BackColor = System.Drawing.Color.Orange,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            refreshButton.Click += async (s, e) => await LoadFullTransactionHistory();
            panel1.Controls.Add(refreshButton);

            var historyGrid = new DataGridView
            {
                Location = new System.Drawing.Point(30, 160),
                Size = new System.Drawing.Size(850, 400),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                GridColor = System.Drawing.Color.LightGray
            };

            historyGrid.Columns.Add("Date", "Date");
            historyGrid.Columns.Add("Type", "Type");
            historyGrid.Columns.Add("Amount", "Amount");
            historyGrid.Columns.Add("Commission", "Commission");
            historyGrid.Columns.Add("Balance", "Balance");
            historyGrid.Columns.Add("Description", "Description");
            historyGrid.Columns.Add("Reference", "Reference");

            panel1.Controls.Add(historyGrid);

            var backButton = new Button
            {
                Text = "Back to Dashboard",
                Location = new Point(170, 120),
                Size = new Size(120, 30),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            backButton.Click += (s, e) => ShowDashboardPanel();
            panel1.Controls.Add(backButton);

            LoadFullTransactionHistory();

            panel1.Refresh();
            panel1.Invalidate();
        }

        private async Task LoadFullTransactionHistory()
        {
            try
            {
                var historyGrid = panel1.Controls.OfType<DataGridView>().FirstOrDefault();
                if (historyGrid != null)
                {
                    historyGrid.Rows.Clear();
                    var transactions = await _transactionService.GetUserTransactionHistoryAsync(_user.Id, null, null, null);
                    foreach (var transaction in transactions.OrderByDescending(t => t.CreatedAt))
                    {
                        historyGrid.Rows.Add(
                            transaction.CreatedAt.ToString("MM/dd/yyyy HH:mm"),
                            transaction.Type.ToString(),
                            $"${transaction.Amount:N2}",
                            $"${transaction.Commission:N2}",
                            $"${transaction.Balance:N2}",
                            transaction.Description,
                            transaction.ReferenceNumber
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading transaction history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool PromptForPIN()
        {
            var pinForm = new Form
            {
                Text = "Enter PIN",
                Size = new System.Drawing.Size(300, 150),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var pinLabel = new Label
            {
                Text = "Enter your PIN:",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(100, 20)
            };

            var pinTextBox = new TextBox
            {
                Location = new System.Drawing.Point(20, 50),
                Size = new System.Drawing.Size(100, 20),
                PasswordChar = '*',
                MaxLength = 4
            };

            var okButton = new Button
            {
                Text = "OK",
                Location = new System.Drawing.Point(140, 50),
                Size = new System.Drawing.Size(60, 25),
                DialogResult = DialogResult.OK
            };

            var cancelButton = new Button
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(210, 50),
                Size = new System.Drawing.Size(60, 25),
                DialogResult = DialogResult.Cancel
            };

            pinForm.Controls.AddRange(new Control[] { pinLabel, pinTextBox, okButton, cancelButton });
            pinForm.AcceptButton = okButton;
            pinForm.CancelButton = cancelButton;

            var result = pinForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                return pinTextBox.Text == _user.PIN;
            }
            return false;
        }

        private void LogOut()
        {
            var result = MessageBox.Show("Are you sure you want to log out?", "Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
                Application.Restart();
            }
        }
    }
} 