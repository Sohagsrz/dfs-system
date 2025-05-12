using System;
using System.Windows.Forms;
using Scash.Models;
using Scash.Services;

namespace Scash.Forms
{
    public class TransactionForm : Form
    {
        private readonly User _currentUser;
        private readonly UserService _userService;
        private readonly TransactionService _transactionService;
        private ComboBox cmbTransactionType;
        private TextBox txtAmount;
        private TextBox txtRecipientUsername;
        private Button btnExecute;
        private Label lblBalance;

        public TransactionForm(User currentUser, UserService userService, TransactionService transactionService)
        {
            _currentUser = currentUser;
            _userService = userService;
            _transactionService = transactionService;
            InitializeComponent();
            UpdateBalance();
        }

        private void InitializeComponent()
        {
            this.Text = "Transaction";
            this.Width = 400;
            this.Height = 400;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Balance Label
            lblBalance = new Label { Text = "Balance: $0.00", Left = 20, Top = 20, Width = 200 };
            this.Controls.Add(lblBalance);

            // Transaction Type
            Label lblType = new Label { Text = "Transaction Type:", Left = 20, Top = 50, Width = 120 };
            cmbTransactionType = new ComboBox { Left = 150, Top = 50, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.AddRange(new Control[] { lblType, cmbTransactionType });

            // Amount
            Label lblAmount = new Label { Text = "Amount:", Left = 20, Top = 90, Width = 120 };
            txtAmount = new TextBox { Left = 150, Top = 90, Width = 200 };
            this.Controls.AddRange(new Control[] { lblAmount, txtAmount });

            // Recipient (for Send Money and Payment)
            Label lblRecipient = new Label { Text = "Recipient Username:", Left = 20, Top = 130, Width = 120 };
            txtRecipientUsername = new TextBox { Left = 150, Top = 130, Width = 200 };
            this.Controls.AddRange(new Control[] { lblRecipient, txtRecipientUsername });

            // Execute Button
            btnExecute = new Button { Text = "Execute Transaction", Left = 150, Top = 170, Width = 200 };
            btnExecute.Click += BtnExecute_Click;
            this.Controls.Add(btnExecute);

            // Load transaction types based on user role
            LoadTransactionTypes();
        }

        private void LoadTransactionTypes()
        {
            cmbTransactionType.Items.Clear();
            
            // All users can check balance
            cmbTransactionType.Items.Add("Balance Inquiry");

            switch (_currentUser.Role)
            {
                case UserRole.Personal:
                    cmbTransactionType.Items.AddRange(new object[] { "Send Money", "Cash Out", "Payment to Merchant" });
                    break;
                case UserRole.Merchant:
                    cmbTransactionType.Items.AddRange(new object[] { "Send Money", "Cash Out", "Withdraw to Bank" });
                    break;
                case UserRole.Agent:
                    cmbTransactionType.Items.AddRange(new object[] { "Cash In", "Send Money", "Cash Out" });
                    break;
                case UserRole.Admin:
                    // Admins don't perform regular transactions
                    MessageBox.Show("Admins don't perform regular transactions.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
            }

            cmbTransactionType.SelectedIndex = 0;
            UpdateFormFields();
            cmbTransactionType.SelectedIndexChanged += (s, e) => UpdateFormFields();
        }

        private void UpdateFormFields()
        {
            var selectedType = cmbTransactionType.SelectedItem?.ToString();
            txtAmount.Enabled = selectedType != "Balance Inquiry";
            txtRecipientUsername.Enabled = selectedType == "Send Money" || selectedType == "Payment to Merchant";
        }

        private async void UpdateBalance()
        {
            lblBalance.Text = $"Balance: ${_currentUser.Balance:N2}";
        }

        private async void BtnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedType = cmbTransactionType.SelectedItem?.ToString();
                decimal amount = 0;

                if (selectedType != "Balance Inquiry")
                {
                    if (!decimal.TryParse(txtAmount.Text, out amount) || amount <= 0)
                    {
                        MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                switch (selectedType)
                {
                    case "Balance Inquiry":
                        MessageBox.Show($"Current Balance: ${_currentUser.Balance:N2}", "Balance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "Send Money":
                        if (string.IsNullOrWhiteSpace(txtRecipientUsername.Text))
                        {
                            MessageBox.Show("Please enter recipient username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        await _transactionService.SendMoney(_currentUser.Id, txtRecipientUsername.Text, amount);
                        break;

                    case "Cash In":
                        if (_currentUser.Role != UserRole.Agent)
                        {
                            MessageBox.Show("Only agents can perform cash in transactions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(txtRecipientUsername.Text))
                        {
                            MessageBox.Show("Please enter recipient username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        await _transactionService.CashIn(_currentUser.Id, txtRecipientUsername.Text, amount);
                        break;

                    case "Cash Out":
                        await _transactionService.CashOut(_currentUser.Id, amount);
                        break;

                    case "Payment to Merchant":
                        if (_currentUser.Role != UserRole.Personal)
                        {
                            MessageBox.Show("Only personal accounts can make merchant payments.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(txtRecipientUsername.Text))
                        {
                            MessageBox.Show("Please enter merchant username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        await _transactionService.PayMerchant(_currentUser.Id, txtRecipientUsername.Text, amount);
                        break;

                    case "Withdraw to Bank":
                        if (_currentUser.Role != UserRole.Merchant)
                        {
                            MessageBox.Show("Only merchants can withdraw to bank.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        await _transactionService.WithdrawToBank(_currentUser.Id, amount);
                        break;
                }

                // Refresh user data and update balance
                _currentUser.Balance = (await _userService.GetUserById(_currentUser.Id)).Balance;
                UpdateBalance();
                MessageBox.Show("Transaction completed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Transaction failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 