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

        public MerchantDashboardForm(User user, UserService userService, TransactionService transactionService)
        {
            _user = user;
            _userService = userService;
            _transactionService = transactionService;
            InitializeComponent();
        }
    }
} 