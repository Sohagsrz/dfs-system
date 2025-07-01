using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scash.Models;
using Scash.Services;

namespace Scash.Forms
{
    public partial class PersonalDashboardForm : Form
    {
        private readonly User _user;
        private readonly UserService _userService;
        private readonly TransactionService _transactionService;
        private DataGridView transactionHistoryGrid;

        public PersonalDashboardForm(User user, UserService userService, TransactionService transactionService)
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
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged_1;
            btnTransaction.Click += BtnTransaction_Click;
        }

        private async void LoadDashboard()
        {
            try
            {
                Console.WriteLine("Loading dashboard...");
                var updatedUser = await _userService.GetUserById(_user.Id);
                if (updatedUser != null)
                {
                    _user.Balance = updatedUser.Balance;
                }

            UpdateBalance();
                welcomeTxt.Text = $"Welcome, {_user.FullName}";

                Console.WriteLine("Showing home panel...");
                ShowHomePanel();
                Console.WriteLine("Dashboard loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading dashboard: {ex}");
                MessageBox.Show($"Error loading dashboard: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateBalance()
        {
            lblBalance.Text = $"Balance: ${_user.Balance:N2}";
        }
        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            var selectedItem = listBox1.SelectedItem?.ToString();
            switch (selectedItem)
            {
                case "Home":
                    ShowHomePanel();
                    break;
                case "Cash Out":
                    ShowCashOutPanel();
                    break;
                case "Send Money":
                    ShowSendMoneyPanel();
                    break;
                case "Settings":
                    ShowSettingsPanel();
                    break;
                case "Log Out":
                    LogOut();
                    break;
            }
        }

        private void ShowHomePanel()
        {
            Console.WriteLine("ShowHomePanel called");
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true; // Ensure panel is visible
            Console.WriteLine($"Panel visible: {panel1.Visible}, Panel size: {panel1.Size}");

            var headerLabel = new Label
            {
                Text = "Dashboard Overview",
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(400, 40),
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
                Size = new System.Drawing.Size(220, 60),
                Font = new System.Drawing.Font("Segoe UI", 20, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkBlue
            };

            balanceCard.Controls.Add(balanceLabel);
            balanceCard.Controls.Add(balanceAmount);
            panel1.Controls.Add(balanceCard);
            Console.WriteLine("Added balance card");

            var quickActionsLabel = new Label
            {
                Text = "Quick Actions",
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(25, 270),
                Size = new System.Drawing.Size(250, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(quickActionsLabel);

            var sendMoneyBtn = new Button
            {
                Text = "Send Money",
                Location = new System.Drawing.Point(30, 310),
                Size = new System.Drawing.Size(130, 40),
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
                Size = new System.Drawing.Size(130, 40),
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
                Size = new System.Drawing.Size(130, 40),
                BackColor = System.Drawing.Color.MediumPurple,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            payMerchantBtn.Click += (s, e) => ShowPayMerchantPanel();
            panel1.Controls.Add(payMerchantBtn);

            var requestCashInBtn = new Button
            {
                Text = "Request Cash-In",
                Location = new System.Drawing.Point(30, 360),
                Size = new System.Drawing.Size(130, 40),
                BackColor = System.Drawing.Color.Teal,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            requestCashInBtn.Click += (s, e) => ShowRequestCashInInfo();
            panel1.Controls.Add(requestCashInBtn);

            var viewAllHistoryBtn = new Button
            {
                Text = "View Full Transaction History",
                Location = new System.Drawing.Point(180, 360),
                Size = new System.Drawing.Size(180,40),
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
                    Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
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

        private void ShowSendMoneyPanel()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Send Money",
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 50),
                Size = new System.Drawing.Size(300, 40),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            panel1.Controls.Add(headerLabel);

            var recipientLabel = new Label
            {
                Text = "Recipient Username:",
                Location = new System.Drawing.Point(30, 120),
                Size = new System.Drawing.Size(150, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            panel1.Controls.Add(recipientLabel);

            var recipientTextBox = new TextBox
            {
                Location = new System.Drawing.Point(200, 120),
                Size = new System.Drawing.Size(250, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(recipientTextBox);

            var amountLabel = new Label
            {
                Text = "Amount ($):",
                Location = new System.Drawing.Point(30, 160),
                Size = new System.Drawing.Size(150, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            panel1.Controls.Add(amountLabel);

            var amountTextBox = new TextBox
            {
                Location = new System.Drawing.Point(200, 160),
                Size = new System.Drawing.Size(250, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(amountTextBox);

            var sendButton = new Button
            {
                Text = "Send Money",
                Location = new System.Drawing.Point(200, 200),
                Size = new System.Drawing.Size(120, 35),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            sendButton.Click += async (s, e) =>
            {
                if (!PromptForPIN()) return;
                try
                {
                    if (string.IsNullOrWhiteSpace(recipientTextBox.Text) || string.IsNullOrWhiteSpace(amountTextBox.Text))
                    {
                        MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!decimal.TryParse(amountTextBox.Text, out decimal amount) || amount <= 0)
                    {
                        MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    ShowHomePanel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error sending money: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panel1.Controls.Add(sendButton);

            var backButton = new Button
            {
                Text = "Back to Home",
                Location = new System.Drawing.Point(340, 200),
                Size = new System.Drawing.Size(120, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            backButton.Click += (s, e) => ShowHomePanel();
            panel1.Controls.Add(backButton);

            // Force panel refresh
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
                Size = new System.Drawing.Size(150, 25),
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            panel1.Controls.Add(amountLabel);

            var amountTextBox = new TextBox
            {
                Location = new System.Drawing.Point(200, 120),
                Size = new System.Drawing.Size(250, 25),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };
            panel1.Controls.Add(amountTextBox);

            var cashOutButton = new Button
            {
                Text = "Cash Out",
                Location = new System.Drawing.Point(200, 160),
                Size = new System.Drawing.Size(120, 35),
                BackColor = System.Drawing.Color.Orange,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            cashOutButton.Click += async (s, e) =>
            {
                if (!PromptForPIN()) return;
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

                    await _transactionService.CashOut(_user.Id, amount);
                    MessageBox.Show("Cash out successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var updatedUser = await _userService.GetUserById(_user.Id);
                    if (updatedUser != null)
                    {
                        _user.Balance = updatedUser.Balance;
                        UpdateBalance();
                    }

                    ShowHomePanel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing cash out: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panel1.Controls.Add(cashOutButton);

            var backButton = new Button
            {
                Text = "Back to Home",
                Location = new System.Drawing.Point(340, 160),
                Size = new System.Drawing.Size(120, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            backButton.Click += (s, e) => ShowHomePanel();
            panel1.Controls.Add(backButton);

            // Force panel refresh
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
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(200, 30)
            };
            panel1.Controls.Add(headerLabel);

            var merchantLabel = new Label
            {
                Text = "Merchant Username:",
                Location = new System.Drawing.Point(20, 70),
                Size = new System.Drawing.Size(150, 25)
            };
            panel1.Controls.Add(merchantLabel);

            var merchantTextBox = new TextBox
            {
                Location = new System.Drawing.Point(180, 70),
                Size = new System.Drawing.Size(200, 25)
            };
            panel1.Controls.Add(merchantTextBox);

            var amountLabel = new Label
            {
                Text = "Amount ($):",
                Location = new System.Drawing.Point(20, 110),
                Size = new System.Drawing.Size(150, 25)
            };
            panel1.Controls.Add(amountLabel);

            var amountTextBox = new TextBox
            {
                Location = new System.Drawing.Point(180, 110),
                Size = new System.Drawing.Size(200, 25)
            };
            panel1.Controls.Add(amountTextBox);

            var payButton = new Button
            {
                Text = "Pay Merchant",
                Location = new System.Drawing.Point(180, 150),
                Size = new System.Drawing.Size(120, 35),
                BackColor = System.Drawing.Color.MediumPurple,
                ForeColor = System.Drawing.Color.White
            };
            payButton.Click += async (s, e) =>
            {
                if (!PromptForPIN()) return;
                try
                {
                    if (string.IsNullOrWhiteSpace(merchantTextBox.Text) || string.IsNullOrWhiteSpace(amountTextBox.Text))
                    {
                        MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (!decimal.TryParse(amountTextBox.Text, out decimal amount) || amount <= 0)
                    {
                        MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    await _transactionService.PayMerchant(_user.Id, merchantTextBox.Text, amount);
                    MessageBox.Show("Merchant payment successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var updatedUser = await _userService.GetUserById(_user.Id);
                    if (updatedUser != null)
                    {
                        _user.Balance = updatedUser.Balance;
            UpdateBalance();
                    }
                    ShowHomePanel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error paying merchant: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panel1.Controls.Add(payButton);

            var backButton = new Button
            {
                Text = "Back to Home",
                Location = new System.Drawing.Point(320, 150),
                Size = new System.Drawing.Size(120, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White
            };
            backButton.Click += (s, e) => ShowHomePanel();
            panel1.Controls.Add(backButton);

            // Force panel refresh
            panel1.Refresh();
            panel1.Invalidate();
        }

        private void ShowRequestCashInInfo()
        {
            MessageBox.Show("To cash-in, please visit your nearest agent and provide your username. The agent will process the cash-in for you.", "Request Cash-In", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowFullTransactionHistory()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Full Transaction History",
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(350, 30)
            };
            panel1.Controls.Add(headerLabel);
            var closeBtn = new Button
            {
                Text = "Back to Home",
                Location = new System.Drawing.Point(400, 20),
                Size = new System.Drawing.Size(120, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White
            };
            closeBtn.Click += (s, e) => ShowHomePanel();
            panel1.Controls.Add(closeBtn);
            var grid = new DataGridView
            {
                Location = new System.Drawing.Point(20, 70),
                Size = new System.Drawing.Size(850, 350),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = System.Drawing.Color.White
            };
            grid.Columns.Add("Date", "Date");
            grid.Columns.Add("Type", "Type");
            grid.Columns.Add("Amount", "Amount");
            grid.Columns.Add("Balance", "Balance");
            grid.Columns.Add("Description", "Description");
            grid.Columns.Add("Reference", "Reference");
            LoadFullTransactionHistory(grid);
            panel1.Controls.Add(grid);

            // Force panel refresh
            panel1.Refresh();
            panel1.Invalidate();
        }

        private async void LoadFullTransactionHistory(DataGridView grid)
        {
            try
            {
                var transactions = await _transactionService.GetUserTransactionHistoryAsync(_user.Id, null, null, null);
                foreach (var transaction in transactions.OrderByDescending(t => t.CreatedAt))
                {
                    grid.Rows.Add(
                        transaction.CreatedAt.ToString("MM/dd/yyyy"),
                        transaction.Type.ToString(),
                        $"${transaction.Amount:N2}",
                        $"${transaction.Balance:N2}",
                        transaction.Description,
                        transaction.ReferenceNumber
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading transactions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool PromptForPIN()
        {
            var pinPrompt = new Form { Width = 300, Height = 180, Text = "Enter PIN", FormBorderStyle = FormBorderStyle.FixedDialog, StartPosition = FormStartPosition.CenterParent };
            var label = new Label { Left = 20, Top = 20, Text = "Enter your PIN:", Width = 200 };
            var pinBox = new TextBox { Left = 20, Top = 50, Width = 240, PasswordChar = '*' };
            var okBtn = new Button { Text = "OK", Left = 60, Width = 80, Top = 90, DialogResult = DialogResult.OK };
            var cancelBtn = new Button { Text = "Cancel", Left = 150, Width = 80, Top = 90, DialogResult = DialogResult.Cancel };
            pinPrompt.Controls.Add(label);
            pinPrompt.Controls.Add(pinBox);
            pinPrompt.Controls.Add(okBtn);
            pinPrompt.Controls.Add(cancelBtn);
            pinPrompt.AcceptButton = okBtn;
            pinPrompt.CancelButton = cancelBtn;
            if (pinPrompt.ShowDialog() == DialogResult.OK)
            {
                if (BCrypt.Net.BCrypt.Verify(pinBox.Text, _user.PIN))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Incorrect PIN.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return false;
        }

        private void ShowSettingsPanel()
        {
            panel1.Controls.Clear();
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Visible = true;

            var headerLabel = new Label
            {
                Text = "Account Settings",
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(200, 30)
            };
            panel1.Controls.Add(headerLabel);

            var profileLabel = new Label
            {
                Text = "Profile Information",
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 70),
                Size = new System.Drawing.Size(200, 25)
            };
            panel1.Controls.Add(profileLabel);

            var nameLabel = new Label
            {
                Text = $"Name: {_user.FullName}",
                Location = new System.Drawing.Point(20, 110),
                Size = new System.Drawing.Size(300, 25)
            };
            panel1.Controls.Add(nameLabel);

            var emailLabel = new Label
            {
                Text = $"Email: {_user.Email}",
                Location = new System.Drawing.Point(20, 140),
                Size = new System.Drawing.Size(300, 25)
            };
            panel1.Controls.Add(emailLabel);

            var phoneLabel = new Label
            {
                Text = $"Phone: {_user.PhoneNumber}",
                Location = new System.Drawing.Point(20, 170),
                Size = new System.Drawing.Size(300, 25)
            };
            panel1.Controls.Add(phoneLabel);

            var balanceLabel = new Label
            {
                Text = $"Balance: ${_user.Balance:N2}",
                Location = new System.Drawing.Point(20, 200),
                Size = new System.Drawing.Size(300, 25)
            };
            panel1.Controls.Add(balanceLabel);

            var dailyLimitLabel = new Label
            {
                Text = $"Daily Limit: ${_user.DailyTransactionLimit:N2}",
                Location = new System.Drawing.Point(20, 230),
                Size = new System.Drawing.Size(300, 25)
            };
            panel1.Controls.Add(dailyLimitLabel);

            var monthlyLimitLabel = new Label
            {
                Text = $"Monthly Limit: ${_user.MonthlyTransactionLimit:N2}",
                Location = new System.Drawing.Point(20, 260),
                Size = new System.Drawing.Size(300, 25)
            };
            panel1.Controls.Add(monthlyLimitLabel);

            var backButton = new Button
            {
                Text = "Back to Home",
                Location = new System.Drawing.Point(20, 300),
                Size = new System.Drawing.Size(120, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White
            };
            backButton.Click += (s, e) => ShowHomePanel();
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

        private void BtnTransaction_Click(object sender, EventArgs e)
        {
            var form = new TransactionForm(_user, _userService, _transactionService);
            form.ShowDialog();
            LoadDashboard();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
