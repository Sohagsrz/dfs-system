using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scash.Models;
using Scash.Services;

namespace Scash.Forms
{
    public partial class MerchantDashboardForm : Form
    {
        private readonly User _user;
        private readonly UserService _userService;
        private readonly TransactionService _transactionService;
        private DataGridView transactionHistoryGrid;

        public MerchantDashboardForm(User user, UserService userService, TransactionService transactionService)
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
            btnAcceptPayment.Click += BtnAcceptPayment_Click;
        }

        private async void LoadDashboard()
        {
            try
            {
                Console.WriteLine("Loading merchant dashboard...");
                var updatedUser = await _userService.GetUserById(_user.Id);
                if (updatedUser != null)
                {
                    _user.Balance = updatedUser.Balance;
                }

                UpdateBalance();
                welcomeTxt.Text = $"Welcome, {_user.FullName}";
                
                Console.WriteLine("Showing dashboard panel...");
                ShowDashboardPanel();
                Console.WriteLine("Merchant dashboard loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading merchant dashboard: {ex}");
                MessageBox.Show($"Error loading dashboard: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateBalance()
        {
            lblBalance.Text = $"Balance: ${_user.Balance:N2}";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = listBox1.SelectedItem?.ToString();
            switch (selectedItem)
            {
                case "Dashboard":
                    ShowDashboardPanel();
                    break;
                case "Send Money":
                    ShowSendMoneyPanel();
                    break;
                case "Cash Out":
                    ShowCashOutPanel();
                    break;
                case "Pay Merchant":
                    ShowPayMerchantPanel();
                    break;
                case "Request Cash-In":
                    ShowRequestCashInInfo();
                    break;
                case "Accept Payment":
                    ShowAcceptPaymentPanel();
                    break;
                case "Sales Report":
                    ShowSalesReportPanel();
                    break;
                case "Transaction Log":
                    ShowTransactionLogPanel();
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
                Text = "Merchant Dashboard Overview",
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
                BackColor = System.Drawing.Color.LightGreen,
                BorderStyle = BorderStyle.FixedSingle
            };

            var balanceLabel = new Label
            {
                Text = "Current Balance",
                Location = new System.Drawing.Point(15, 15),
                Size = new System.Drawing.Size(220, 25),
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkGreen
            };

            var balanceAmount = new Label
            {
                Text = $"${_user.Balance:N2}",
                Location = new System.Drawing.Point(15, 45),
                Size = new System.Drawing.Size(220, 40),
                Font = new System.Drawing.Font("Segoe UI", 20, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkGreen
            };

            balanceCard.Controls.Add(balanceLabel);
            balanceCard.Controls.Add(balanceAmount);
            panel1.Controls.Add(balanceCard);
            Console.WriteLine("Added balance card");

            var quickActionsLabel = new Label
            {
                Text = "Quick Actions",
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 270),
                Size = new System.Drawing.Size(250, 30),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(quickActionsLabel);

            // Personal Account Features
            var sendMoneyBtn = new Button
            {
                Text = "Send Money",
                Location = new System.Drawing.Point(30, 310),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            sendMoneyBtn.Click += (s, e) => ShowSendMoneyPanel();
            panel1.Controls.Add(sendMoneyBtn);

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

            var payMerchantBtn = new Button
            {
                Text = "Pay Merchant",
                Location = new System.Drawing.Point(330, 310),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.MediumPurple,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            payMerchantBtn.Click += (s, e) => ShowPayMerchantPanel();
            panel1.Controls.Add(payMerchantBtn);

            var requestCashInBtn = new Button
            {
                Text = "Request Cash-In",
                Location = new System.Drawing.Point(480, 310),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.Teal,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            requestCashInBtn.Click += (s, e) => ShowRequestCashInInfo();
            panel1.Controls.Add(requestCashInBtn);

            // Merchant Features
            var acceptPaymentBtn = new Button
            {
                Text = "Accept Payment",
                Location = new System.Drawing.Point(30, 360),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.SeaGreen,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            acceptPaymentBtn.Click += (s, e) => ShowAcceptPaymentPanel();
            panel1.Controls.Add(acceptPaymentBtn);

            var salesReportBtn = new Button
            {
                Text = "Sales Report",
                Location = new System.Drawing.Point(180, 360),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.Blue,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            salesReportBtn.Click += (s, e) => ShowSalesReportPanel();
            panel1.Controls.Add(salesReportBtn);

            var transactionLogBtn = new Button
            {
                Text = "Transaction Log",
                Location = new System.Drawing.Point(330, 360),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.DarkOrange,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            transactionLogBtn.Click += (s, e) => ShowTransactionLogPanel();
            panel1.Controls.Add(transactionLogBtn);

            var viewAllHistoryBtn = new Button
            {
                Text = "View Full Transaction History",
                Location = new System.Drawing.Point(480, 360),
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
                    Location = new System.Drawing.Point(350, 120),
                    Size = new System.Drawing.Size(250, 30),
                    ForeColor = System.Drawing.Color.DarkBlue
                };
                panel1.Controls.Add(transactionsLabel);

                transactionHistoryGrid = new DataGridView
                {
                    Location = new System.Drawing.Point(350, 160),
                    Size = new System.Drawing.Size(550, 300),
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
                transactionHistoryGrid.Columns.Add("Balance", "Balance");

                foreach (var transaction in recentTransactions)
                {
                    transactionHistoryGrid.Rows.Add(
                        transaction.CreatedAt.ToString("MM/dd/yyyy"),
                        transaction.Type.ToString(),
                        $"${transaction.Amount:N2}",
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

        private void ShowAcceptPaymentPanel()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Accept Digital Payment",
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(400, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(headerLabel);

            var customerLabel = new Label
            {
                Text = "Customer Username:",
                Location = new System.Drawing.Point(30, 120),
                Size = new System.Drawing.Size(180, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            panel1.Controls.Add(customerLabel);

            var customerTextBox = new TextBox
            {
                Location = new System.Drawing.Point(220, 120),
                Size = new System.Drawing.Size(250, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(customerTextBox);

            var amountLabel = new Label
            {
                Text = "Payment Amount ($):",
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
                Text = "Payment Description:",
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

            var acceptButton = new Button
            {
                Text = "Accept Payment",
                Location = new System.Drawing.Point(220, 240),
                Size = new System.Drawing.Size(140, 35),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            acceptButton.Click += async (s, e) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(customerTextBox.Text) || string.IsNullOrWhiteSpace(amountTextBox.Text))
                    {
                        MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!decimal.TryParse(amountTextBox.Text, out decimal amount) || amount <= 0)
                    {
                        MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // For merchant accepting payment, we need to find the customer and have them send money to the merchant
                    var customer = await _userService.GetUserByUsername(customerTextBox.Text);
                    if (customer == null)
                    {
                        MessageBox.Show("Customer not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Use SendMoney method where customer sends money to merchant
                    await _transactionService.SendMoney(customer.Id, _user.Username, amount);

                    var updatedUser = await _userService.GetUserById(_user.Id);
                    if (updatedUser != null)
                    {
                        _user.Balance = updatedUser.Balance;
                        UpdateBalance();
                    }

                    ShowDashboardPanel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error accepting payment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panel1.Controls.Add(acceptButton);

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

            // Force panel refresh
            panel1.Refresh();
            panel1.Invalidate();
        }

        private void ShowSalesReportPanel()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Sales Report",
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
            generateButton.Click += async (s, e) => await GenerateSalesReport(periodComboBox.SelectedItem?.ToString());
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
            reportGrid.Columns.Add("Customer", "Customer");
            reportGrid.Columns.Add("Amount", "Amount");
            reportGrid.Columns.Add("Description", "Description");
            reportGrid.Columns.Add("Reference", "Reference");

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

            // Force panel refresh
            panel1.Refresh();
            panel1.Invalidate();
        }

        private async Task GenerateSalesReport(string period)
        {
            try
            {
                // This would typically query the database for transactions based on the period
                // For now, we'll show a sample report
                var reportGrid = panel1.Controls.OfType<DataGridView>().FirstOrDefault();
                if (reportGrid != null)
                {
                    reportGrid.Rows.Clear();
                    
                    // Sample data - in a real application, this would come from the database
                    var sampleData = new[]
                    {
                        new { Date = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy"), Customer = "john_doe", Amount = 150.00m, Description = "Restaurant Payment", Reference = "REF001" },
                        new { Date = DateTime.Now.AddDays(-2).ToString("MM/dd/yyyy"), Customer = "jane_smith", Amount = 75.50m, Description = "Coffee Shop", Reference = "REF002" },
                        new { Date = DateTime.Now.AddDays(-3).ToString("MM/dd/yyyy"), Customer = "bob_wilson", Amount = 200.00m, Description = "Grocery Store", Reference = "REF003" }
                    };

                    foreach (var item in sampleData)
                    {
                        reportGrid.Rows.Add(
                            item.Date,
                            item.Customer,
                            $"${item.Amount:N2}",
                            item.Description,
                            item.Reference
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
            logGrid.Columns.Add("Amount", "Amount");
            logGrid.Columns.Add("Balance", "Balance");
            logGrid.Columns.Add("Description", "Description");
            logGrid.Columns.Add("Reference", "Reference");

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

            // Load initial data
            LoadFullTransactionLog();

            // Force panel refresh
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
                            $"${transaction.Amount:N2}",
                            $"${transaction.Balance:N2}",
                            transaction.Description,
                            transaction.ReferenceNumber
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading transaction log: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowSettingsPanel()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Merchant Settings",
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
                Text = $"Business Name: {_user.FullName}",
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

            var backButton = new Button
            {
                Text = "Back to Dashboard",
                Location = new System.Drawing.Point(30, 300),
                Size = new System.Drawing.Size(140, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            backButton.Click += (s, e) => ShowDashboardPanel();
            panel1.Controls.Add(backButton);

            // Force panel refresh
            panel1.Refresh();
            panel1.Invalidate();
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

        private void BtnAcceptPayment_Click(object sender, EventArgs e)
        {
            ShowAcceptPaymentPanel();
        }

        private void ShowSendMoneyPanel()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Send Money",
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(300, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(headerLabel);

            var recipientLabel = new Label
            {
                Text = "Recipient Username:",
                Location = new System.Drawing.Point(30, 120),
                Size = new System.Drawing.Size(180, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            panel1.Controls.Add(recipientLabel);

            var recipientTextBox = new TextBox
            {
                Location = new System.Drawing.Point(220, 120),
                Size = new System.Drawing.Size(250, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(recipientTextBox);

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

            var sendButton = new Button
            {
                Text = "Send Money",
                Location = new System.Drawing.Point(220, 240),
                Size = new System.Drawing.Size(140, 35),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            sendButton.Click += async (s, e) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(recipientTextBox.Text) || string.IsNullOrWhiteSpace(amountTextBox.Text))
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

                    await _transactionService.SendMoney(_user.Id, recipientTextBox.Text, amount);
                    MessageBox.Show("Money sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var updatedUser = await _userService.GetUserById(_user.Id);
                    if (updatedUser != null)
                    {
                        _user.Balance = updatedUser.Balance;
                        UpdateBalance();
                    }

                    ShowDashboardPanel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error sending money: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panel1.Controls.Add(sendButton);

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
                Text = "Cash Out",
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(300, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(headerLabel);

            var amountLabel = new Label
            {
                Text = "Amount ($):",
                Location = new System.Drawing.Point(30, 120),
                Size = new System.Drawing.Size(180, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            panel1.Controls.Add(amountLabel);

            var amountTextBox = new TextBox
            {
                Location = new System.Drawing.Point(220, 120),
                Size = new System.Drawing.Size(250, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(amountTextBox);

            var cashOutButton = new Button
            {
                Text = "Cash Out",
                Location = new System.Drawing.Point(220, 160),
                Size = new System.Drawing.Size(140, 35),
                BackColor = System.Drawing.Color.Orange,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            cashOutButton.Click += async (s, e) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(amountTextBox.Text))
                    {
                        MessageBox.Show("Please enter an amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    await _transactionService.CashOut(_user.Id, amount);
                    MessageBox.Show("Cash out request submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var updatedUser = await _userService.GetUserById(_user.Id);
                    if (updatedUser != null)
                    {
                        _user.Balance = updatedUser.Balance;
                        UpdateBalance();
                    }

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
                Location = new System.Drawing.Point(370, 160),
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

        private void ShowPayMerchantPanel()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Pay Merchant",
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(300, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(headerLabel);

            var merchantLabel = new Label
            {
                Text = "Merchant Username:",
                Location = new System.Drawing.Point(30, 120),
                Size = new System.Drawing.Size(180, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            panel1.Controls.Add(merchantLabel);

            var merchantTextBox = new TextBox
            {
                Location = new System.Drawing.Point(220, 120),
                Size = new System.Drawing.Size(250, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(merchantTextBox);

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

            var payButton = new Button
            {
                Text = "Pay Merchant",
                Location = new System.Drawing.Point(220, 240),
                Size = new System.Drawing.Size(140, 35),
                BackColor = System.Drawing.Color.MediumPurple,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            payButton.Click += async (s, e) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(merchantTextBox.Text) || string.IsNullOrWhiteSpace(amountTextBox.Text))
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

                    await _transactionService.PayMerchant(_user.Id, merchantTextBox.Text, amount);
                    MessageBox.Show("Payment to merchant successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var updatedUser = await _userService.GetUserById(_user.Id);
                    if (updatedUser != null)
                    {
                        _user.Balance = updatedUser.Balance;
                        UpdateBalance();
                    }

                    ShowDashboardPanel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error paying merchant: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panel1.Controls.Add(payButton);

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

        private void ShowRequestCashInInfo()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Request Cash-In",
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(300, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(headerLabel);

            var infoLabel = new Label
            {
                Text = "To request a cash-in, please contact an agent in your area or visit a partner location.",
                Location = new System.Drawing.Point(30, 120),
                Size = new System.Drawing.Size(600, 60),
                Font = new System.Drawing.Font("Segoe UI", 11),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(infoLabel);

            var backButton = new Button
            {
                Text = "Back to Dashboard",
                Location = new System.Drawing.Point(30, 200),
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
    }
} 