using System;
using System.Windows.Forms;
using Scash.Services;
using Scash.Models;

namespace Scash.Forms
{
    public partial class LoginForm : Form
    {
        private readonly UserService _userService;
        private readonly TransactionService _transactionService;

        public LoginForm(UserService userService, TransactionService transactionService)
        {
            try
            {
                Console.WriteLine("Initializing LoginForm...");
                _userService = userService;
                _transactionService = transactionService;
                InitializeComponent();
                Console.WriteLine("LoginForm initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing LoginForm: {ex}");
                MessageBox.Show($"Error initializing login form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var user = await _userService.AuthenticateUser(txtUsername.Text, txtPassword.Text);
                if (user != null)
                {
                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    Form dashboard = null;
                    switch (user.Role)
                    {
                        case UserRole.Admin:
                            dashboard = new AdminDashboardForm(user, _userService, _transactionService);
                            break;
                        case UserRole.Agent:
                            dashboard = new AgentDashboardForm(user, _userService, _transactionService);
                            break;
                        case UserRole.Merchant:
                            dashboard = new MerchantDashboardForm(user, _userService, _transactionService);
                            break;
                        case UserRole.Personal:
                            dashboard = new PersonalDashboardForm(user, _userService, _transactionService);
                            break;
                    }
                    if (dashboard != null)
                    {
                        dashboard.Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm(_userService);
            registerForm.ShowDialog();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("LoginForm loading...");
                // Add any initialization code here
                Console.WriteLine("LoginForm loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading LoginForm: {ex}");
                MessageBox.Show($"Error loading login form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load_1(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
} 