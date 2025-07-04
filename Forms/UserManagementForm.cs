using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scash.Models;
using Scash.Services;

namespace Scash.Forms
{
    public class UserManagementForm : Form
    {
        private readonly UserService _userService;
        private DataGridView dgvUsers;
        private Button btnRefresh;

        public UserManagementForm(UserService userService)
        {
            _userService = userService;
            InitializeComponent();
            LoadUsers();
        }

        private void InitializeComponent()
        {
            this.Text = "User Management";
            this.Width = 900;
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterScreen;

            dgvUsers = new DataGridView { Left = 10, Top = 10, Width = 860, Height = 400, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            btnRefresh = new Button { Text = "Refresh", Left = 10, Top = 420, Width = 100 };
            btnRefresh.Click += (s, e) => LoadUsers();

            this.Controls.Add(dgvUsers);
            this.Controls.Add(btnRefresh);
        }

        private async void LoadUsers()
        {
            dgvUsers.Columns.Clear();
            dgvUsers.Rows.Clear();
            dgvUsers.Columns.Add("Id", "Id");
            dgvUsers.Columns.Add("Username", "Username");
            dgvUsers.Columns.Add("FullName", "Full Name");
            dgvUsers.Columns.Add("Email", "Email");
            dgvUsers.Columns.Add("Phone", "Phone");
            dgvUsers.Columns.Add("Role", "Role");
            dgvUsers.Columns.Add("IsActive", "Active");
            dgvUsers.Columns.Add("CreatedAt", "Created At");
            dgvUsers.Columns.Add("Actions", "Actions");

            var users = await _userService.GetAllUsers();
            foreach (var user in users)
            {
                int rowIndex = dgvUsers.Rows.Add(user.Id, user.Username, user.FullName, user.Email, user.PhoneNumber, user.Role, user.IsActive, user.CreatedAt, "");
                var row = dgvUsers.Rows[rowIndex];
                row.Tag = user;
            }
            dgvUsers.CellContentClick -= DgvUsers_CellContentClick;
            dgvUsers.CellContentClick += DgvUsers_CellContentClick;
            AddActionButtons();
        }

        private void AddActionButtons()
        {
            if (dgvUsers.Columns["Approve"] == null)
            {
                var approveBtn = new DataGridViewButtonColumn { Name = "Approve", Text = "Approve", UseColumnTextForButtonValue = true };
                dgvUsers.Columns.Add(approveBtn);
            }
            if (dgvUsers.Columns["Reject"] == null)
            {
                var rejectBtn = new DataGridViewButtonColumn { Name = "Reject", Text = "Reject", UseColumnTextForButtonValue = true };
                dgvUsers.Columns.Add(rejectBtn);
            }
            if (dgvUsers.Columns["Activate"] == null)
            {
                var activateBtn = new DataGridViewButtonColumn { Name = "Activate", Text = "Activate", UseColumnTextForButtonValue = true };
                dgvUsers.Columns.Add(activateBtn);
            }
            if (dgvUsers.Columns["Deactivate"] == null)
            {
                var deactivateBtn = new DataGridViewButtonColumn { Name = "Deactivate", Text = "Deactivate", UseColumnTextForButtonValue = true };
                dgvUsers.Columns.Add(deactivateBtn);
            }
            if (dgvUsers.Columns["Delete"] == null)
            {
                var deleteBtn = new DataGridViewButtonColumn { Name = "Delete", Text = "Delete", UseColumnTextForButtonValue = true };
                dgvUsers.Columns.Add(deleteBtn);
            }
        }

        private async void DgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var user = dgvUsers.Rows[e.RowIndex].Tag as User;
            if (user == null) return;
            var colName = dgvUsers.Columns[e.ColumnIndex].Name;
            try
            {
                switch (colName)
                {
                    case "Approve":
                        await _userService.SetUserActive(user.Id, true);
                        break;
                    case "Reject":
                        await _userService.DeleteUser(user.Id);
                        break;
                    case "Activate":
                        await _userService.SetUserActive(user.Id, true);
                        break;
                    case "Deactivate":
                        await _userService.SetUserActive(user.Id, false);
                        break;
                    case "Delete":
                        await _userService.DeleteUser(user.Id);
                        break;
                }
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Action failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 