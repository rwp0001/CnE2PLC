namespace CnE2PLC
{
    partial class SheetSelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            components = new System.ComponentModel.Container();
            Selection = new ComboBox();
            Select = new Button();
            Cancel = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            sheetSelectBindingSource = new BindingSource(components);
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)sheetSelectBindingSource).BeginInit();
            SuspendLayout();
            // 
            // Selection
            // 
            Selection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Selection.DataBindings.Add(new Binding("Text", sheetSelectBindingSource, "SheetName", true));
            Selection.FormattingEnabled = true;
            Selection.Location = new Point(2, 2);
            Selection.Margin = new Padding(2);
            Selection.Name = "Selection";
            Selection.Size = new Size(162, 23);
            Selection.TabIndex = 0;
            Selection.SelectedIndexChanged += Selection_SelectedIndexChanged;
            // 
            // Select
            // 
            Select.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Select.Location = new Point(2, 29);
            Select.Margin = new Padding(2);
            Select.Name = "Select";
            Select.Size = new Size(78, 20);
            Select.TabIndex = 1;
            Select.Text = "Select";
            Select.UseVisualStyleBackColor = true;
            Select.Click += Select_Click;
            // 
            // Cancel
            // 
            Cancel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Cancel.Location = new Point(84, 29);
            Cancel.Margin = new Padding(2);
            Cancel.Name = "Cancel";
            Cancel.Size = new Size(78, 20);
            Cancel.TabIndex = 2;
            Cancel.Text = "Cancel";
            Cancel.UseVisualStyleBackColor = true;
            Cancel.Click += Cancel_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(Selection);
            flowLayoutPanel1.Controls.Add(Select);
            flowLayoutPanel1.Controls.Add(Cancel);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Margin = new Padding(2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(176, 53);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // sheetSelectBindingSource
            // 
            sheetSelectBindingSource.DataSource = typeof(SheetSelect);
            // 
            // sheetSelectBindingSource1
            // 
            sheetSelectBindingSource1.DataSource = typeof(SheetSelect);
            // 
            // SheetSelect
            // 
            AcceptButton = Select;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = Cancel;
            ClientSize = new Size(176, 53);
            Controls.Add(flowLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SheetSelect";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Sheet to Use";
            flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)sheetSelectBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ComboBox Selection;
        private Button Select;
        private Button Cancel;
        private FlowLayoutPanel flowLayoutPanel1;
        private BindingSource sheetSelectBindingSource;
        private BindingSource sheetSelectBindingSource1;
    }
}