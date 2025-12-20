namespace CnE2PLC
{
    partial class frmAbout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            tableLayoutPanel = new TableLayoutPanel();
            lblGitBranchIsDirty = new Label();
            lblGitCommitID = new Label();
            lblGitBranch = new Label();
            logoPictureBox = new PictureBox();
            lblProductName = new Label();
            lblVersion = new Label();
            lblCopyright = new Label();
            lblGitVersion = new Label();
            textBoxDescription = new TextBox();
            okButton = new Button();
            lblGitRepoLink = new LinkLabel();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67F));
            tableLayoutPanel.Controls.Add(lblGitBranchIsDirty, 1, 7);
            tableLayoutPanel.Controls.Add(lblGitCommitID, 1, 4);
            tableLayoutPanel.Controls.Add(lblGitBranch, 1, 5);
            tableLayoutPanel.Controls.Add(logoPictureBox, 0, 0);
            tableLayoutPanel.Controls.Add(lblProductName, 1, 0);
            tableLayoutPanel.Controls.Add(lblVersion, 1, 1);
            tableLayoutPanel.Controls.Add(lblCopyright, 1, 2);
            tableLayoutPanel.Controls.Add(lblGitVersion, 1, 3);
            tableLayoutPanel.Controls.Add(textBoxDescription, 1, 8);
            tableLayoutPanel.Controls.Add(okButton, 1, 9);
            tableLayoutPanel.Controls.Add(lblGitRepoLink, 1, 6);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(10, 10);
            tableLayoutPanel.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 10;
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanel.Size = new Size(487, 326);
            tableLayoutPanel.TabIndex = 0;
            // 
            // lblGitBranchIsDirty
            // 
            lblGitBranchIsDirty.Dock = DockStyle.Fill;
            lblGitBranchIsDirty.ForeColor = Color.FromArgb(192, 0, 0);
            lblGitBranchIsDirty.Location = new Point(167, 135);
            lblGitBranchIsDirty.Margin = new Padding(7, 0, 4, 0);
            lblGitBranchIsDirty.MaximumSize = new Size(0, 20);
            lblGitBranchIsDirty.Name = "lblGitBranchIsDirty";
            lblGitBranchIsDirty.Size = new Size(316, 20);
            lblGitBranchIsDirty.TabIndex = 27;
            lblGitBranchIsDirty.Text = "Branch Is Dirty";
            lblGitBranchIsDirty.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblGitCommitID
            // 
            lblGitCommitID.Dock = DockStyle.Fill;
            lblGitCommitID.Location = new Point(167, 80);
            lblGitCommitID.Margin = new Padding(7, 0, 4, 0);
            lblGitCommitID.MaximumSize = new Size(0, 20);
            lblGitCommitID.Name = "lblGitCommitID";
            lblGitCommitID.Size = new Size(316, 20);
            lblGitCommitID.TabIndex = 26;
            lblGitCommitID.Text = "Git Commit ID";
            lblGitCommitID.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblGitBranch
            // 
            lblGitBranch.Dock = DockStyle.Fill;
            lblGitBranch.Location = new Point(167, 100);
            lblGitBranch.Margin = new Padding(7, 0, 4, 0);
            lblGitBranch.MaximumSize = new Size(0, 20);
            lblGitBranch.Name = "lblGitBranch";
            lblGitBranch.Size = new Size(316, 20);
            lblGitBranch.TabIndex = 25;
            lblGitBranch.Text = "Git Branch";
            lblGitBranch.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // logoPictureBox
            // 
            logoPictureBox.Dock = DockStyle.Fill;
            logoPictureBox.Image = (Image)resources.GetObject("logoPictureBox.Image");
            logoPictureBox.Location = new Point(4, 3);
            logoPictureBox.Margin = new Padding(4, 3, 4, 3);
            logoPictureBox.Name = "logoPictureBox";
            tableLayoutPanel.SetRowSpan(logoPictureBox, 10);
            logoPictureBox.Size = new Size(152, 321);
            logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            logoPictureBox.TabIndex = 12;
            logoPictureBox.TabStop = false;
            // 
            // lblProductName
            // 
            lblProductName.Dock = DockStyle.Fill;
            lblProductName.Location = new Point(167, 0);
            lblProductName.Margin = new Padding(7, 0, 4, 0);
            lblProductName.MaximumSize = new Size(0, 20);
            lblProductName.Name = "lblProductName";
            lblProductName.Size = new Size(316, 20);
            lblProductName.TabIndex = 19;
            lblProductName.Text = "Product Name";
            lblProductName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVersion
            // 
            lblVersion.Dock = DockStyle.Fill;
            lblVersion.Location = new Point(167, 20);
            lblVersion.Margin = new Padding(7, 0, 4, 0);
            lblVersion.MaximumSize = new Size(0, 20);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(316, 20);
            lblVersion.TabIndex = 0;
            lblVersion.Text = "Version";
            lblVersion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCopyright
            // 
            lblCopyright.Dock = DockStyle.Fill;
            lblCopyright.Location = new Point(167, 40);
            lblCopyright.Margin = new Padding(7, 0, 4, 0);
            lblCopyright.MaximumSize = new Size(0, 20);
            lblCopyright.Name = "lblCopyright";
            lblCopyright.Size = new Size(316, 20);
            lblCopyright.TabIndex = 21;
            lblCopyright.Text = "Copyright";
            lblCopyright.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblGitVersion
            // 
            lblGitVersion.Dock = DockStyle.Fill;
            lblGitVersion.Location = new Point(167, 60);
            lblGitVersion.Margin = new Padding(7, 0, 4, 0);
            lblGitVersion.MaximumSize = new Size(0, 20);
            lblGitVersion.Name = "lblGitVersion";
            lblGitVersion.Size = new Size(316, 20);
            lblGitVersion.TabIndex = 22;
            lblGitVersion.Text = "Git Version";
            lblGitVersion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBoxDescription
            // 
            textBoxDescription.Dock = DockStyle.Fill;
            textBoxDescription.Location = new Point(167, 158);
            textBoxDescription.Margin = new Padding(7, 3, 4, 3);
            textBoxDescription.Multiline = true;
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.ReadOnly = true;
            textBoxDescription.ScrollBars = ScrollBars.Both;
            textBoxDescription.Size = new Size(316, 133);
            textBoxDescription.TabIndex = 23;
            textBoxDescription.TabStop = false;
            textBoxDescription.Text = "Description";
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            okButton.DialogResult = DialogResult.Cancel;
            okButton.Location = new Point(395, 297);
            okButton.Margin = new Padding(4, 3, 4, 3);
            okButton.Name = "okButton";
            okButton.Size = new Size(88, 27);
            okButton.TabIndex = 24;
            okButton.Text = "&OK";
            // 
            // lblGitRepoLink
            // 
            lblGitRepoLink.AutoSize = true;
            lblGitRepoLink.Location = new Point(163, 120);
            lblGitRepoLink.Name = "lblGitRepoLink";
            lblGitRepoLink.Size = new Size(76, 15);
            lblGitRepoLink.TabIndex = 28;
            lblGitRepoLink.TabStop = true;
            lblGitRepoLink.Text = "Git Repo URL";
            lblGitRepoLink.LinkClicked += lblGitRepoLink_LinkClicked;
            // 
            // frmAbout
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(507, 346);
            Controls.Add(tableLayoutPanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAbout";
            Padding = new Padding(10);
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "About";
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblGitVersion;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Button okButton;
        private Label lblGitBranch;
        private Label lblGitBranchIsDirty;
        private Label lblGitCommitID;
        private LinkLabel lblGitRepoLink;
    }
}
