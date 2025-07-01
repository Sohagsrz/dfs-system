namespace Scash.Forms
{
    partial class AdminDashboardForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel sidebarPanel;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnActivity;
        private System.Windows.Forms.Button btnAlerts;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.Button btnAnalytics;
        private System.Windows.Forms.Panel topBarPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel mainPanel;
        
        // Dashboard Panel Controls
        private System.Windows.Forms.Panel panelDashboard;
        private System.Windows.Forms.Label lblTotalUsers;
        private System.Windows.Forms.Label lblTotalTransactions;
        private System.Windows.Forms.Label lblTotalVolume;
        private System.Windows.Forms.Label lblActiveUsers;
        private System.Windows.Forms.ProgressBar progressBarUsers;
        private System.Windows.Forms.ProgressBar progressBarTransactions;
        
        // Activity Panel Controls
        private System.Windows.Forms.Panel panelActivity;
        private System.Windows.Forms.ListView listViewActivity;
        
        // Alerts Panel Controls
        private System.Windows.Forms.Panel panelAlerts;
        private System.Windows.Forms.ListView listViewAlerts;
        private System.Windows.Forms.Button btnDismissAlert;
        private System.Windows.Forms.Button btnViewAlertDetails;
        
        // Settings Panel Controls
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.NumericUpDown numTransactionFee;
        private System.Windows.Forms.NumericUpDown numCashOutFee;
        private System.Windows.Forms.NumericUpDown numCashInFee;
        private System.Windows.Forms.NumericUpDown numAgentCommission;
        private System.Windows.Forms.CheckBox chkMaintenanceMode;
        private System.Windows.Forms.Button btnSaveSettings;
        
        // Users Panel Controls
        private System.Windows.Forms.Panel panelUsers;
        private System.Windows.Forms.DataGridView dataGridViewUsers;
        private System.Windows.Forms.Label lblPersonalUsers;
        private System.Windows.Forms.Label lblMerchants;
        private System.Windows.Forms.Label lblAgents;
        private System.Windows.Forms.Label lblAdmins;
        private System.Windows.Forms.Button btnFreezeUser;
        private System.Windows.Forms.Button btnUnfreezeUser;
        private System.Windows.Forms.Button btnDeleteUser;
        
        // Analytics Panel Controls
        private System.Windows.Forms.Panel panelAnalytics;
        
        // Timer for activity updates
        private System.Windows.Forms.Timer activityTimer;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            sidebarPanel = new Panel();
            btnDashboard = new Button();
            btnActivity = new Button();
            btnAlerts = new Button();
            btnSettings = new Button();
            btnUsers = new Button();
            btnAnalytics = new Button();
            topBarPanel = new Panel();
            lblTitle = new Label();
            mainPanel = new Panel();
            panelDashboard = new Panel();
            lblTotalUsers = new Label();
            lblTotalTransactions = new Label();
            lblTotalVolume = new Label();
            lblActiveUsers = new Label();
            progressBarUsers = new ProgressBar();
            progressBarTransactions = new ProgressBar();
            panelActivity = new Panel();
            listViewActivity = new ListView();
            panelAlerts = new Panel();
            listViewAlerts = new ListView();
            btnDismissAlert = new Button();
            btnViewAlertDetails = new Button();
            panelSettings = new Panel();
            numTransactionFee = new NumericUpDown();
            numCashOutFee = new NumericUpDown();
            numCashInFee = new NumericUpDown();
            numAgentCommission = new NumericUpDown();
            chkMaintenanceMode = new CheckBox();
            btnSaveSettings = new Button();
            panelUsers = new Panel();
            dataGridViewUsers = new DataGridView();
            lblPersonalUsers = new Label();
            lblMerchants = new Label();
            lblAgents = new Label();
            lblAdmins = new Label();
            btnFreezeUser = new Button();
            btnUnfreezeUser = new Button();
            btnDeleteUser = new Button();
            panelAnalytics = new Panel();
            activityTimer = new System.Windows.Forms.Timer(components);
            sidebarPanel.SuspendLayout();
            topBarPanel.SuspendLayout();
            mainPanel.SuspendLayout();
            panelDashboard.SuspendLayout();
            panelActivity.SuspendLayout();
            panelAlerts.SuspendLayout();
            panelSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTransactionFee).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCashOutFee).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCashInFee).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numAgentCommission).BeginInit();
            panelUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsers).BeginInit();
            SuspendLayout();
            // 
            // sidebarPanel
            // 
            sidebarPanel.BackColor = Color.FromArgb(44, 62, 80);
            sidebarPanel.Controls.Add(btnDashboard);
            sidebarPanel.Controls.Add(btnActivity);
            sidebarPanel.Controls.Add(btnAlerts);
            sidebarPanel.Controls.Add(btnSettings);
            sidebarPanel.Controls.Add(btnUsers);
            sidebarPanel.Controls.Add(btnAnalytics);
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.Location = new Point(0, 0);
            sidebarPanel.Name = "sidebarPanel";
            sidebarPanel.Size = new Size(180, 800);
            sidebarPanel.TabIndex = 2;
            // 
            // btnDashboard
            // 
            btnDashboard.BackColor = Color.FromArgb(44, 62, 80);
            btnDashboard.Dock = DockStyle.Top;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.ForeColor = Color.White;
            btnDashboard.Location = new Point(0, 250);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(180, 50);
            btnDashboard.TabIndex = 0;
            btnDashboard.Text = "Dashboard";
            btnDashboard.UseVisualStyleBackColor = false;
            // 
            // btnActivity
            // 
            btnActivity.BackColor = Color.FromArgb(44, 62, 80);
            btnActivity.Dock = DockStyle.Top;
            btnActivity.FlatStyle = FlatStyle.Flat;
            btnActivity.ForeColor = Color.White;
            btnActivity.Location = new Point(0, 200);
            btnActivity.Name = "btnActivity";
            btnActivity.Size = new Size(180, 50);
            btnActivity.TabIndex = 1;
            btnActivity.Text = "Activity";
            btnActivity.UseVisualStyleBackColor = false;
            // 
            // btnAlerts
            // 
            btnAlerts.BackColor = Color.FromArgb(44, 62, 80);
            btnAlerts.Dock = DockStyle.Top;
            btnAlerts.FlatStyle = FlatStyle.Flat;
            btnAlerts.ForeColor = Color.White;
            btnAlerts.Location = new Point(0, 150);
            btnAlerts.Name = "btnAlerts";
            btnAlerts.Size = new Size(180, 50);
            btnAlerts.TabIndex = 2;
            btnAlerts.Text = "Alerts";
            btnAlerts.UseVisualStyleBackColor = false;
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.FromArgb(44, 62, 80);
            btnSettings.Dock = DockStyle.Top;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.ForeColor = Color.White;
            btnSettings.Location = new Point(0, 100);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(180, 50);
            btnSettings.TabIndex = 3;
            btnSettings.Text = "Settings";
            btnSettings.UseVisualStyleBackColor = false;
            // 
            // btnUsers
            // 
            btnUsers.BackColor = Color.FromArgb(44, 62, 80);
            btnUsers.Dock = DockStyle.Top;
            btnUsers.FlatStyle = FlatStyle.Flat;
            btnUsers.ForeColor = Color.White;
            btnUsers.Location = new Point(0, 50);
            btnUsers.Name = "btnUsers";
            btnUsers.Size = new Size(180, 50);
            btnUsers.TabIndex = 4;
            btnUsers.Text = "Users";
            btnUsers.UseVisualStyleBackColor = false;
            // 
            // btnAnalytics
            // 
            btnAnalytics.BackColor = Color.FromArgb(44, 62, 80);
            btnAnalytics.Dock = DockStyle.Top;
            btnAnalytics.FlatStyle = FlatStyle.Flat;
            btnAnalytics.ForeColor = Color.White;
            btnAnalytics.Location = new Point(0, 0);
            btnAnalytics.Name = "btnAnalytics";
            btnAnalytics.Size = new Size(180, 50);
            btnAnalytics.TabIndex = 5;
            btnAnalytics.Text = "Analytics";
            btnAnalytics.UseVisualStyleBackColor = false;
            // 
            // topBarPanel
            // 
            topBarPanel.BackColor = Color.FromArgb(52, 152, 219);
            topBarPanel.Controls.Add(lblTitle);
            topBarPanel.Dock = DockStyle.Top;
            topBarPanel.Location = new Point(180, 0);
            topBarPanel.Name = "topBarPanel";
            topBarPanel.Size = new Size(1020, 60);
            topBarPanel.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1020, 60);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Admin Dashboard";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.WhiteSmoke;
            mainPanel.Controls.Add(panelDashboard);
            mainPanel.Controls.Add(panelActivity);
            mainPanel.Controls.Add(panelAlerts);
            mainPanel.Controls.Add(panelSettings);
            mainPanel.Controls.Add(panelUsers);
            mainPanel.Controls.Add(panelAnalytics);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(180, 60);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(1020, 740);
            mainPanel.TabIndex = 0;
            // 
            // panelDashboard
            // 
            panelDashboard.BackColor = Color.White;
            panelDashboard.Controls.Add(lblTotalUsers);
            panelDashboard.Controls.Add(lblTotalTransactions);
            panelDashboard.Controls.Add(lblTotalVolume);
            panelDashboard.Controls.Add(lblActiveUsers);
            panelDashboard.Controls.Add(progressBarUsers);
            panelDashboard.Controls.Add(progressBarTransactions);
            panelDashboard.Dock = DockStyle.Fill;
            panelDashboard.Location = new Point(0, 0);
            panelDashboard.Name = "panelDashboard";
            panelDashboard.Size = new Size(1020, 740);
            panelDashboard.TabIndex = 0;
            // 
            // lblTotalUsers
            // 
            lblTotalUsers.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalUsers.Location = new Point(30, 30);
            lblTotalUsers.Name = "lblTotalUsers";
            lblTotalUsers.Size = new Size(200, 30);
            lblTotalUsers.TabIndex = 0;
            lblTotalUsers.Text = "Total Users: 0";
            lblTotalUsers.ForeColor = Color.DarkBlue;
            // 
            // lblTotalTransactions
            // 
            lblTotalTransactions.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalTransactions.Location = new Point(30, 80);
            lblTotalTransactions.Name = "lblTotalTransactions";
            lblTotalTransactions.Size = new Size(200, 30);
            lblTotalTransactions.TabIndex = 1;
            lblTotalTransactions.Text = "Total Transactions: 0";
            lblTotalTransactions.ForeColor = Color.DarkGreen;
            // 
            // lblTotalVolume
            // 
            lblTotalVolume.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalVolume.Location = new Point(30, 130);
            lblTotalVolume.Name = "lblTotalVolume";
            lblTotalVolume.Size = new Size(200, 30);
            lblTotalVolume.TabIndex = 2;
            lblTotalVolume.Text = "Total Volume: ₱0.00";
            lblTotalVolume.ForeColor = Color.DarkOrange;
            // 
            // lblActiveUsers
            // 
            lblActiveUsers.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblActiveUsers.Location = new Point(30, 180);
            lblActiveUsers.Name = "lblActiveUsers";
            lblActiveUsers.Size = new Size(200, 30);
            lblActiveUsers.TabIndex = 3;
            lblActiveUsers.Text = "Active Users: 0";
            lblActiveUsers.ForeColor = Color.DarkRed;
            // 
            // progressBarUsers
            // 
            progressBarUsers.Location = new Point(30, 220);
            progressBarUsers.Name = "progressBarUsers";
            progressBarUsers.Size = new Size(300, 25);
            progressBarUsers.TabIndex = 4;
            progressBarUsers.Style = ProgressBarStyle.Continuous;
            // 
            // progressBarTransactions
            // 
            progressBarTransactions.Location = new Point(30, 260);
            progressBarTransactions.Name = "progressBarTransactions";
            progressBarTransactions.Size = new Size(300, 25);
            progressBarTransactions.TabIndex = 5;
            progressBarTransactions.Style = ProgressBarStyle.Continuous;
            // 
            // panelActivity
            // 
            panelActivity.BackColor = Color.White;
            panelActivity.Controls.Add(listViewActivity);
            panelActivity.Dock = DockStyle.Fill;
            panelActivity.Location = new Point(0, 0);
            panelActivity.Name = "panelActivity";
            panelActivity.Size = new Size(1020, 740);
            panelActivity.TabIndex = 1;
            // 
            // listViewActivity
            // 
            listViewActivity.Dock = DockStyle.Fill;
            listViewActivity.Font = new Font("Segoe UI", 10F);
            listViewActivity.FullRowSelect = true;
            listViewActivity.GridLines = true;
            listViewActivity.Location = new Point(0, 0);
            listViewActivity.Name = "listViewActivity";
            listViewActivity.Size = new Size(1020, 740);
            listViewActivity.TabIndex = 0;
            listViewActivity.UseCompatibleStateImageBehavior = false;
            listViewActivity.View = View.Details;
            // 
            // panelAlerts
            // 
            panelAlerts.BackColor = Color.White;
            panelAlerts.Controls.Add(listViewAlerts);
            panelAlerts.Controls.Add(btnDismissAlert);
            panelAlerts.Controls.Add(btnViewAlertDetails);
            panelAlerts.Dock = DockStyle.Fill;
            panelAlerts.Location = new Point(0, 0);
            panelAlerts.Name = "panelAlerts";
            panelAlerts.Size = new Size(1020, 740);
            panelAlerts.TabIndex = 2;
            // 
            // listViewAlerts
            // 
            listViewAlerts.Dock = DockStyle.Top;
            listViewAlerts.Font = new Font("Segoe UI", 10F);
            listViewAlerts.FullRowSelect = true;
            listViewAlerts.GridLines = true;
            listViewAlerts.Location = new Point(0, 0);
            listViewAlerts.Name = "listViewAlerts";
            listViewAlerts.Size = new Size(1020, 600);
            listViewAlerts.TabIndex = 0;
            listViewAlerts.UseCompatibleStateImageBehavior = false;
            listViewAlerts.View = View.Details;
            // 
            // btnDismissAlert
            // 
            btnDismissAlert.BackColor = Color.Orange;
            btnDismissAlert.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnDismissAlert.ForeColor = Color.White;
            btnDismissAlert.Location = new Point(30, 620);
            btnDismissAlert.Name = "btnDismissAlert";
            btnDismissAlert.Size = new Size(150, 40);
            btnDismissAlert.TabIndex = 1;
            btnDismissAlert.Text = "Dismiss Alert";
            btnDismissAlert.UseVisualStyleBackColor = false;
            // 
            // btnViewAlertDetails
            // 
            btnViewAlertDetails.BackColor = Color.Blue;
            btnViewAlertDetails.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnViewAlertDetails.ForeColor = Color.White;
            btnViewAlertDetails.Location = new Point(200, 620);
            btnViewAlertDetails.Name = "btnViewAlertDetails";
            btnViewAlertDetails.Size = new Size(150, 40);
            btnViewAlertDetails.TabIndex = 2;
            btnViewAlertDetails.Text = "View Details";
            btnViewAlertDetails.UseVisualStyleBackColor = false;
            // 
            // panelSettings
            // 
            panelSettings.BackColor = Color.White;
            panelSettings.Controls.Add(numTransactionFee);
            panelSettings.Controls.Add(numCashOutFee);
            panelSettings.Controls.Add(numCashInFee);
            panelSettings.Controls.Add(numAgentCommission);
            panelSettings.Controls.Add(chkMaintenanceMode);
            panelSettings.Controls.Add(btnSaveSettings);
            panelSettings.Dock = DockStyle.Fill;
            panelSettings.Location = new Point(0, 0);
            panelSettings.Name = "panelSettings";
            panelSettings.Size = new Size(1020, 740);
            panelSettings.TabIndex = 3;
            // 
            // numTransactionFee
            // 
            numTransactionFee.DecimalPlaces = 2;
            numTransactionFee.Font = new Font("Segoe UI", 12F);
            numTransactionFee.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numTransactionFee.Location = new Point(30, 80);
            numTransactionFee.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numTransactionFee.Name = "numTransactionFee";
            numTransactionFee.Size = new Size(200, 35);
            numTransactionFee.TabIndex = 0;
            numTransactionFee.Value = new decimal(new int[] { 5, 0, 0, 131072 });
            // 
            // numCashOutFee
            // 
            numCashOutFee.DecimalPlaces = 2;
            numCashOutFee.Font = new Font("Segoe UI", 12F);
            numCashOutFee.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numCashOutFee.Location = new Point(30, 140);
            numCashOutFee.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numCashOutFee.Name = "numCashOutFee";
            numCashOutFee.Size = new Size(200, 35);
            numCashOutFee.TabIndex = 1;
            numCashOutFee.Value = new decimal(new int[] { 10, 0, 0, 131072 });
            // 
            // numCashInFee
            // 
            numCashInFee.DecimalPlaces = 2;
            numCashInFee.Font = new Font("Segoe UI", 12F);
            numCashInFee.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numCashInFee.Location = new Point(30, 200);
            numCashInFee.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numCashInFee.Name = "numCashInFee";
            numCashInFee.Size = new Size(200, 35);
            numCashInFee.TabIndex = 2;
            numCashInFee.Value = new decimal(new int[] { 10, 0, 0, 131072 });
            // 
            // numAgentCommission
            // 
            numAgentCommission.DecimalPlaces = 2;
            numAgentCommission.Font = new Font("Segoe UI", 12F);
            numAgentCommission.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numAgentCommission.Location = new Point(30, 260);
            numAgentCommission.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numAgentCommission.Name = "numAgentCommission";
            numAgentCommission.Size = new Size(200, 35);
            numAgentCommission.TabIndex = 3;
            numAgentCommission.Value = new decimal(new int[] { 10, 0, 0, 131072 });
            // 
            // chkMaintenanceMode
            // 
            chkMaintenanceMode.Font = new Font("Segoe UI", 12F);
            chkMaintenanceMode.Location = new Point(30, 320);
            chkMaintenanceMode.Name = "chkMaintenanceMode";
            chkMaintenanceMode.Size = new Size(200, 30);
            chkMaintenanceMode.TabIndex = 4;
            chkMaintenanceMode.Text = "Maintenance Mode";
            chkMaintenanceMode.UseVisualStyleBackColor = true;
            // 
            // btnSaveSettings
            // 
            btnSaveSettings.BackColor = Color.Green;
            btnSaveSettings.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSaveSettings.ForeColor = Color.White;
            btnSaveSettings.Location = new Point(30, 380);
            btnSaveSettings.Name = "btnSaveSettings";
            btnSaveSettings.Size = new Size(150, 45);
            btnSaveSettings.TabIndex = 5;
            btnSaveSettings.Text = "Save Settings";
            btnSaveSettings.UseVisualStyleBackColor = false;
            // 
            // panelUsers
            // 
            panelUsers.BackColor = Color.White;
            panelUsers.Controls.Add(dataGridViewUsers);
            panelUsers.Controls.Add(lblPersonalUsers);
            panelUsers.Controls.Add(lblMerchants);
            panelUsers.Controls.Add(lblAgents);
            panelUsers.Controls.Add(lblAdmins);
            panelUsers.Controls.Add(btnFreezeUser);
            panelUsers.Controls.Add(btnUnfreezeUser);
            panelUsers.Controls.Add(btnDeleteUser);
            panelUsers.Dock = DockStyle.Fill;
            panelUsers.Location = new Point(0, 0);
            panelUsers.Name = "panelUsers";
            panelUsers.Size = new Size(1020, 740);
            panelUsers.TabIndex = 4;
            // 
            // dataGridViewUsers
            // 
            dataGridViewUsers.AllowUserToAddRows = false;
            dataGridViewUsers.AllowUserToDeleteRows = false;
            dataGridViewUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUsers.BackgroundColor = Color.White;
            dataGridViewUsers.ColumnHeadersHeight = 40;
            dataGridViewUsers.Font = new Font("Segoe UI", 10F);
            dataGridViewUsers.Location = new Point(30, 100);
            dataGridViewUsers.MultiSelect = false;
            dataGridViewUsers.Name = "dataGridViewUsers";
            dataGridViewUsers.ReadOnly = true;
            dataGridViewUsers.RowHeadersWidth = 50;
            dataGridViewUsers.RowTemplate.Height = 35;
            dataGridViewUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsers.Size = new Size(960, 400);
            dataGridViewUsers.TabIndex = 0;
            // 
            // lblPersonalUsers
            // 
            lblPersonalUsers.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblPersonalUsers.Location = new Point(30, 30);
            lblPersonalUsers.Name = "lblPersonalUsers";
            lblPersonalUsers.Size = new Size(150, 25);
            lblPersonalUsers.TabIndex = 1;
            lblPersonalUsers.Text = "Personal: 0";
            lblPersonalUsers.ForeColor = Color.Blue;
            // 
            // lblMerchants
            // 
            lblMerchants.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblMerchants.Location = new Point(200, 30);
            lblMerchants.Name = "lblMerchants";
            lblMerchants.Size = new Size(150, 25);
            lblMerchants.TabIndex = 2;
            lblMerchants.Text = "Merchants: 0";
            lblMerchants.ForeColor = Color.Green;
            // 
            // lblAgents
            // 
            lblAgents.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblAgents.Location = new Point(370, 30);
            lblAgents.Name = "lblAgents";
            lblAgents.Size = new Size(150, 25);
            lblAgents.TabIndex = 3;
            lblAgents.Text = "Agents: 0";
            lblAgents.ForeColor = Color.Orange;
            // 
            // lblAdmins
            // 
            lblAdmins.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblAdmins.Location = new Point(540, 30);
            lblAdmins.Name = "lblAdmins";
            lblAdmins.Size = new Size(150, 25);
            lblAdmins.TabIndex = 4;
            lblAdmins.Text = "Admins: 0";
            lblAdmins.ForeColor = Color.Red;
            // 
            // btnFreezeUser
            // 
            btnFreezeUser.BackColor = Color.Orange;
            btnFreezeUser.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnFreezeUser.ForeColor = Color.White;
            btnFreezeUser.Location = new Point(30, 520);
            btnFreezeUser.Name = "btnFreezeUser";
            btnFreezeUser.Size = new Size(120, 40);
            btnFreezeUser.TabIndex = 5;
            btnFreezeUser.Text = "Freeze User";
            btnFreezeUser.UseVisualStyleBackColor = false;
            // 
            // btnUnfreezeUser
            // 
            btnUnfreezeUser.BackColor = Color.Green;
            btnUnfreezeUser.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnUnfreezeUser.ForeColor = Color.White;
            btnUnfreezeUser.Location = new Point(170, 520);
            btnUnfreezeUser.Name = "btnUnfreezeUser";
            btnUnfreezeUser.Size = new Size(120, 40);
            btnUnfreezeUser.TabIndex = 6;
            btnUnfreezeUser.Text = "Unfreeze User";
            btnUnfreezeUser.UseVisualStyleBackColor = false;
            // 
            // btnDeleteUser
            // 
            btnDeleteUser.BackColor = Color.Red;
            btnDeleteUser.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnDeleteUser.ForeColor = Color.White;
            btnDeleteUser.Location = new Point(310, 520);
            btnDeleteUser.Name = "btnDeleteUser";
            btnDeleteUser.Size = new Size(120, 40);
            btnDeleteUser.TabIndex = 7;
            btnDeleteUser.Text = "Delete User";
            btnDeleteUser.UseVisualStyleBackColor = false;
            // 
            // panelAnalytics
            // 
            panelAnalytics.BackColor = Color.White;
            panelAnalytics.Dock = DockStyle.Fill;
            panelAnalytics.Location = new Point(0, 0);
            panelAnalytics.Name = "panelAnalytics";
            panelAnalytics.Size = new Size(1020, 740);
            panelAnalytics.TabIndex = 5;
            // 
            // AdminDashboardForm
            // 
            ClientSize = new Size(1200, 800);
            Controls.Add(mainPanel);
            Controls.Add(topBarPanel);
            Controls.Add(sidebarPanel);
            Name = "AdminDashboardForm";
            Text = "Admin Dashboard";
            sidebarPanel.ResumeLayout(false);
            topBarPanel.ResumeLayout(false);
            mainPanel.ResumeLayout(false);
            panelDashboard.ResumeLayout(false);
            panelActivity.ResumeLayout(false);
            panelAlerts.ResumeLayout(false);
            panelSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numTransactionFee).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCashOutFee).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCashInFee).EndInit();
            ((System.ComponentModel.ISupportInitialize)numAgentCommission).EndInit();
            panelUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsers).EndInit();
            ResumeLayout(false);
        }
    }
} 