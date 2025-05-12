using System.Windows.Forms;
using Scash.Models;
using Scash.Services;

namespace Scash.Forms
{
    public partial class AdminDashboardForm : Form
    {
        private readonly User _user;
        private readonly UserService _userService;
        private readonly TransactionService _transactionService;
        private Button btnManageUsers;

        public AdminDashboardForm(User user, UserService userService, TransactionService transactionService)
        {
            _user = user;
            _userService = userService;
            _transactionService = transactionService;
            InitializeComponent();
            this.Text = $"Admin Dashboard - {_user.FullName}";
            this.Width = 600;
            this.Height = 400;
            btnManageUsers = new Button { Text = "Manage Users", Left = 30, Top = 30, Width = 200 };
            btnManageUsers.Click += BtnManageUsers_Click;
            this.Controls.Add(btnManageUsers);
        }

        private void BtnManageUsers_Click(object sender, System.EventArgs e)
        {
            var form = new UserManagementForm(_userService);
            form.ShowDialog();
        }
    }
} 