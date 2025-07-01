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
            topPanel = new Panel();
            topPanel.SuspendLayout();
            SuspendLayout();
            // 
            // lblBalance
            // 
            lblBalance.AutoSize = true;
            lblBalance.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBalance.Location = new Point(875, 22);
            lblBalance.Margin = new Padding(4, 0, 4, 0);
            lblBalance.Name = "lblBalance";
            lblBalance.Size = new Size(179, 32);
            lblBalance.TabIndex = 1;
            lblBalance.Text = "Balance: $0.00";
            // 
            // btnTransaction
            // 
            btnTransaction.BackColor = Color.SeaGreen;
            btnTransaction.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTransaction.ForeColor = Color.White;
            btnTransaction.Location = new Point(13, 13);
            btnTransaction.Margin = new Padding(4);
            btnTransaction.Name = "btnTransaction";
            btnTransaction.Size = new Size(175, 44);
            btnTransaction.TabIndex = 2;
            btnTransaction.Text = "Quick Send";
            btnTransaction.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.ImeMode = ImeMode.NoControl;
            panel1.Location = new Point(219, 75);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(923, 564);
            panel1.TabIndex = 4;
            panel1.Paint += panel1_Paint;
            // 
            // listBox1
            // 
            listBox1.BackColor = SystemColors.MenuHighlight;
            listBox1.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            listBox1.ForeColor = Color.White;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 36;
            listBox1.Items.AddRange(new object[] { "Home", "Cash Out", "Send Money", "Settings", "Log Out" });
            listBox1.Location = new Point(0, 75);
            listBox1.Margin = new Padding(4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(224, 544);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged_1;
            // 
            // welcomeTxt
            // 
            welcomeTxt.AutoSize = true;
            welcomeTxt.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            welcomeTxt.Location = new Point(450, 22);
            welcomeTxt.Margin = new Padding(4, 0, 4, 0);
            welcomeTxt.Name = "welcomeTxt";
            welcomeTxt.Size = new Size(244, 30);
            welcomeTxt.TabIndex = 3;
            welcomeTxt.Text = "Welcome, [UserName]";
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.WhiteSmoke;
            topPanel.Controls.Add(lblBalance);
            topPanel.Controls.Add(btnTransaction);
            topPanel.Controls.Add(welcomeTxt);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Margin = new Padding(4);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(1142, 75);
            topPanel.TabIndex = 5;
            // 
            // PersonalDashboardForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1142, 639);
            Controls.Add(listBox1);
            Controls.Add(topPanel);
            Controls.Add(panel1);
            Margin = new Padding(4);
            Name = "PersonalDashboardForm";
            Text = "Personal Dashboard";
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            ResumeLayout(false);
        }
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Button btnTransaction;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label welcomeTxt;
        private System.Windows.Forms.Panel topPanel;
    }
} 