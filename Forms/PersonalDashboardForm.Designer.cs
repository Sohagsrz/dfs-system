namespace Scash.Forms
{
    partial class PersonalDashboardForm
    {
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PersonalDashboardForm));
            btnTransaction = new Button();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // btnTransaction
            // 
            btnTransaction.BackColor = Color.Transparent;
            btnTransaction.BackgroundImage = (Image)resources.GetObject("btnTransaction.BackgroundImage");
            btnTransaction.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            btnTransaction.ForeColor = SystemColors.ActiveCaptionText;
            btnTransaction.Location = new Point(282, 183);
            btnTransaction.Name = "btnTransaction";
            btnTransaction.Size = new Size(139, 132);
            btnTransaction.TabIndex = 1;
            btnTransaction.Text = "Send Money";
            btnTransaction.UseVisualStyleBackColor = false;
            btnTransaction.Click += button1_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.DialogResult = DialogResult.Ignore;
            button1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            button1.ForeColor = SystemColors.ActiveCaptionText;
            button1.Location = new Point(85, 183);
            button1.Name = "button1";
            button1.Size = new Size(139, 132);
            button1.TabIndex = 2;
            button1.Text = "View Balance";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.Location = new Point(604, 43);
            button2.Name = "button2";
            button2.Size = new Size(108, 39);
            button2.TabIndex = 3;
            button2.Text = "Log Out";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.BackColor = Color.Transparent;
            button3.BackgroundImage = (Image)resources.GetObject("button3.BackgroundImage");
            button3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            button3.ForeColor = SystemColors.ActiveCaptionText;
            button3.Location = new Point(487, 183);
            button3.Name = "button3";
            button3.Size = new Size(139, 132);
            button3.TabIndex = 4;
            button3.Text = "Transaction History";
            button3.UseVisualStyleBackColor = false;
            // 
            // PersonalDashboardForm
            // 
            AutoSize = true;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(752, 529);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(btnTransaction);
            DoubleBuffered = true;
            Name = "PersonalDashboardForm";
            Text = "Personal Dashboard";
            Load += PersonalDashboardForm_Load;
            ResumeLayout(false);

        }
        private Button btnTransaction;
        private Button button1;
        private Button button2;
        private Button button3;
    }
} 