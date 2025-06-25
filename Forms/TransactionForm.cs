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
        private Label lblType;
        private Label lblAmount;
        private Label lblRecipient;
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
            lblBalance = new Label();
            lblType = new Label();
            cmbTransactionType = new ComboBox();
            lblAmount = new Label();
            txtAmount = new TextBox();
            lblRecipient = new Label();
            txtRecipientUsername = new TextBox();
            btnExecute = new Button();
            SuspendLayout();
            // 
            // lblBalance
            // 
            lblBalance.Location = new Point(0, 0);
            lblBalance.Name = "lblBalance";
            lblBalance.Size = new Size(100, 23);
            lblBalance.TabIndex = 0;
            // 
            // lblType
            // 
            lblType.Location = new Point(0, 0);
            lblType.Name = "lblType";
            lblType.Size = new Size(100, 23);
            lblType.TabIndex = 1;
            // 
            // cmbTransactionType
            // 
            cmbTransactionType.Location = new Point(0, 0);
            cmbTransactionType.Name = "cmbTransactionType";
            cmbTransactionType.Size = new Size(121, 28);
            cmbTransactionType.TabIndex = 2;
            cmbTransactionType.SelectedIndexChanged += cmbTransactionType_SelectedIndexChanged;
            // 
            // lblAmount
            // 
            lblAmount.Location = new Point(0, 0);
            lblAmount.Name = "lblAmount";
            lblAmount.Size = new Size(100, 23);
            lblAmount.TabIndex = 3;
            // 
            // txtAmount
            // 
            txtAmount.Location = new Point(0, 0);
            txtAmount.Name = "txtAmount";
            txtAmount.Size = new Size(100, 27);
            txtAmount.TabIndex = 4;
            // 
            // lblRecipient
            // 
            lblRecipient.Location = new Point(0, 0);
            lblRecipient.Name = "lblRecipient";
            lblRecipient.Size = new Size(100, 23);
            lblRecipient.TabIndex = 5;
            // 
            // txtRecipientUsername
            // 
            txtRecipientUsername.Location = new Point(0, 0);
            txtRecipientUsername.Name = "txtRecipientUsername";
            txtRecipientUsername.Size = new Size(100, 27);
            txtRecipientUsername.TabIndex = 6;
            // 
            // btnExecute
            // 
            btnExecute.Location = new Point(0, 0);
            btnExecute.Name = "btnExecute";
            btnExecute.Size = new Size(75, 23);
            btnExecute.TabIndex = 7;
            btnExecute.Click += BtnExecute_Click;
            // 
            // TransactionForm
            // 
            ClientSize = new Size(495, 409);
            Controls.Add(lblBalance);
            Controls.Add(lblType);
            Controls.Add(cmbTransactionType);
            Controls.Add(lblAmount);
            Controls.Add(txtAmount);
            Controls.Add(lblRecipient);
            Controls.Add(txtRecipientUsername);
            Controls.Add(btnExecute);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "TransactionForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Transaction";
            Load += TransactionForm_Load;
            ResumeLayout(false);
            PerformLayout();
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

        private void cmbTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TransactionForm_Load(object sender, EventArgs e)
        {

        }
    }
} 