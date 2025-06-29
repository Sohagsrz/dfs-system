namespace Scash.Forms
{
    partial class PersonalDashboardForm
    {
        private void InitializeComponent()
        {
            lblBalance = new Label();
            btnTransaction = new Button();
            panel1 = new Panel();
            listBox1 = new ListBox();
            welcomeTxt = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // lblBalance
            // 
            lblBalance.AutoSize = true;
            lblBalance.Location = new Point(773, 9);
            lblBalance.Name = "lblBalance";
            lblBalance.Size = new Size(110, 25);
            lblBalance.TabIndex = 0;
            lblBalance.Text = "Balance $0.0";
            lblBalance.Click += label1_Click;
            // 
            // btnTransaction
            // 
            btnTransaction.BackColor = Color.Brown;
            btnTransaction.Font = new Font("Showcard Gothic", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTransaction.ForeColor = Color.Snow;
            btnTransaction.Location = new Point(190, -5);
            btnTransaction.Name = "btnTransaction";
            btnTransaction.Size = new Size(158, 42);
            btnTransaction.TabIndex = 1;
            btnTransaction.Text = "Quick Send";
            btnTransaction.UseVisualStyleBackColor = false;
            btnTransaction.Click += button1_Click;
            // 
            // panel1
            // 
            panel1.Location = new Point(209, 43);
            panel1.Name = "panel1";
            panel1.Size = new Size(693, 478);
            panel1.TabIndex = 2;
            // 
            // listBox1
            // 
            listBox1.BackColor = SystemColors.MenuHighlight;
            listBox1.Font = new Font("Segoe UI Black", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            listBox1.ForeColor = Color.White;
            listBox1.FormattingEnabled = true;
            listBox1.ImeMode = ImeMode.NoControl;
            listBox1.ItemHeight = 38;
            listBox1.Items.AddRange(new object[] { "Home", "Cash Out", "Send Money", "Settings", "Log Out" });
            listBox1.Location = new Point(3, -5);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(181, 498);
            listBox1.TabIndex = 3;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // welcomeTxt
            // 
            welcomeTxt.AutoSize = true;
            welcomeTxt.Font = new Font("Showcard Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            welcomeTxt.Location = new Point(433, 9);
            welcomeTxt.Name = "welcomeTxt";
            welcomeTxt.Size = new Size(197, 23);
            welcomeTxt.TabIndex = 4;
            welcomeTxt.Text = "Welcome, Mr Sohag";
            welcomeTxt.Click += label1_Click_1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(47, 429);
            label2.Name = "label2";
            label2.Size = new Size(58, 25);
            label2.TabIndex = 0;
            label2.Text = "sCash";
            // 
            // PersonalDashboardForm
            // 
            AutoSize = true;
            ClientSize = new Size(914, 511);
            Controls.Add(label2);
            Controls.Add(welcomeTxt);
            Controls.Add(btnTransaction);
            Controls.Add(lblBalance);
            Controls.Add(listBox1);
            Controls.Add(panel1);
            Name = "PersonalDashboardForm";
            Text = "Personal Dashboard";
            Load += PersonalDashboardForm_Load;
            ResumeLayout(false);
            PerformLayout();

        }
        private Label lblBalance;
        private Button btnTransaction;
        private Panel panel1;
        private ListBox listBox1;
        private Label welcomeTxt;
        private Label label2;
    }
} 