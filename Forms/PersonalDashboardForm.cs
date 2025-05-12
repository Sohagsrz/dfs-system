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
        private Button btnTransaction;
        private Label lblBalance;

        public PersonalDashboardForm(User user, UserService userService, TransactionService transactionService)
        {
            _user = user;
            _userService = userService;
            _transactionService = transactionService;
            InitializeComponent();
            UpdateBalance();
        }

        private void UpdateBalance()
        {
            lblBalance.Text = $"Balance: ${_user.Balance:N2}";
        }

        private void BtnTransaction_Click(object sender, System.EventArgs e)
        {
            var form = new TransactionForm(_user, _userService, _transactionService);
            form.ShowDialog();
            UpdateBalance();
        }
    }
} 