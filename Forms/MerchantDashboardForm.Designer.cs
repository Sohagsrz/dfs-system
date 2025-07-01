namespace Scash.Forms
{
    partial class MerchantDashboardForm
    {
        private void InitializeComponent()
        {
            lblBalance = new Label();
            btnAcceptPayment = new Button();
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
            // btnAcceptPayment
            // 
            btnAcceptPayment.BackColor = Color.SeaGreen;
            btnAcceptPayment.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAcceptPayment.ForeColor = Color.White;
            btnAcceptPayment.Location = new Point(250, 15);
            btnAcceptPayment.Margin = new Padding(4);
            btnAcceptPayment.Name = "btnAcceptPayment";
            btnAcceptPayment.Size = new Size(175, 44);
            btnAcceptPayment.TabIndex = 2;
            btnAcceptPayment.Text = "Accept Payment";
            btnAcceptPayment.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Location = new Point(224, 75);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(918, 564);
            panel1.TabIndex = 4;
            // 
            // listBox1
            // 
            listBox1.BackColor = SystemColors.MenuHighlight;
            listBox1.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            listBox1.ForeColor = Color.White;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 36;
            listBox1.Items.AddRange(new object[] { "Dashboard", "Send Money", "Cash Out", "Pay Merchant", "Request Cash-In", "Accept Payment", "Sales Report", "Transaction Log", "Settings", "Log Out" });
            listBox1.Location = new Point(0, 75);
            listBox1.Margin = new Padding(4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(224, 564);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
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
            welcomeTxt.Text = "Welcome, [MerchantName]";
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.WhiteSmoke;
            topPanel.Controls.Add(lblBalance);
            topPanel.Controls.Add(btnAcceptPayment);
            topPanel.Controls.Add(welcomeTxt);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Margin = new Padding(4);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(1142, 75);
            topPanel.TabIndex = 5;
            // 
            // MerchantDashboardForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1142, 639);
            Controls.Add(listBox1);
            Controls.Add(topPanel);
            Controls.Add(panel1);
            Margin = new Padding(4);
            Name = "MerchantDashboardForm";
            Text = "Merchant Dashboard";
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            ResumeLayout(false);
        }
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Button btnAcceptPayment;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label welcomeTxt;
        private System.Windows.Forms.Panel topPanel;
    }
} 