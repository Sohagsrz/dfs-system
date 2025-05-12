using System;
using System.Windows.Forms;
using Scash.Models;
using Scash.Services;
using BCrypt.Net;

namespace Scash.Forms
{
    public partial class RegisterForm : Form
    {
        private readonly UserService _userService;

        public RegisterForm(UserService userService)
        {
            _userService = userService;
            InitializeComponent();
            cmbRole.Items.AddRange(new object[] { UserRole.Personal, UserRole.Merchant, UserRole.Agent });
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Register button clicked");
                var user = new User
                {
                    Username = txtUsername.Text,
                    Password = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text),
                    PIN = BCrypt.Net.BCrypt.HashPassword(txtPIN.Text),
                    FullName = txtFullName.Text,
                    Email = txtEmail.Text,
                    PhoneNumber = txtPhone.Text,
                    Role = (UserRole)cmbRole.SelectedItem,
                    Balance = 0.00m,
                    CreatedAt = DateTime.Now
                };

                Console.WriteLine($"Selected Role: {user.Role}");

                _userService.RegisterUser(user);
                MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration failed: {ex.Message}");
                MessageBox.Show($"Registration failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 