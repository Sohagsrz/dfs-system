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
        //private Button btnTransaction;
        //private Label lblBalance;

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
            Console.WriteLine(_user);
            //lblBalance.Text = $"Balance: ${_user.Balance}";
            lblBalance.Text = "Balance ";
        }

        private void BtnTransaction_Click(object sender, System.EventArgs e)
        {
            var form = new TransactionForm(_user, _userService, _transactionService);
            form.ShowDialog();
            UpdateBalance();
        }

        private void PersonalDashboardForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            var form = new TransactionForm(_user, _userService, _transactionService);
            form.ShowDialog();
            UpdateBalance();

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
} 