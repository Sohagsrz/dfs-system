namespace Scash.Forms
{
    partial class AgentDashboardForm
    {
        private void InitializeComponent()
        {
            lblBalance = new Label();
            lblCommission = new Label();
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
            // lblCommission
            // 
            lblCommission.AutoSize = true;
            lblCommission.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCommission.Location = new Point(875, 50);
            lblCommission.Margin = new Padding(4, 0, 4, 0);
            lblCommission.Name = "lblCommission";
            lblCommission.Size = new Size(200, 32);
            lblCommission.TabIndex = 2;
            lblCommission.Text = "Commission: $0.00";
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
            listBox1.Items.AddRange(new object[] { "Dashboard", "Cash In", "Cash Out", "Earnings Report", "Transaction Log", "Performance", "Settings", "Log Out" });
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
            welcomeTxt.Text = "Welcome, [AgentName]";
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.WhiteSmoke;
            topPanel.Controls.Add(lblBalance);
            topPanel.Controls.Add(lblCommission);
            topPanel.Controls.Add(welcomeTxt);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Margin = new Padding(4);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(1142, 75);
            topPanel.TabIndex = 5;
            // 
            // AgentDashboardForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1142, 639);
            Controls.Add(listBox1);
            Controls.Add(topPanel);
            Controls.Add(panel1);
            Margin = new Padding(4);
            Name = "AgentDashboardForm";
            Text = "Agent Dashboard";
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            ResumeLayout(false);
        }
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblCommission;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label welcomeTxt;
        private System.Windows.Forms.Panel topPanel;
    }
} 