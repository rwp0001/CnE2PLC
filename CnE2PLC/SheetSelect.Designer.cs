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
            Selection = new ComboBox();
            Select = new Button();
            Cancel = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // Selection
            // 
            Selection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Selection.FormattingEnabled = true;
            Selection.Location = new Point(3, 3);
            Selection.Name = "Selection";
            Selection.Size = new Size(230, 33);
            Selection.TabIndex = 0;
            Selection.SelectedIndexChanged += Selection_SelectedIndexChanged;
            // 
            // Select
            // 
            Select.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Select.Location = new Point(3, 42);
            Select.Name = "Select";
            Select.Size = new Size(112, 34);
            Select.TabIndex = 1;
            Select.Text = "Select";
            Select.UseVisualStyleBackColor = true;
            Select.Click += Select_Click;
            // 
            // Cancel
            // 
            Cancel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Cancel.Location = new Point(121, 42);
            Cancel.Name = "Cancel";
            Cancel.Size = new Size(112, 34);
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
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(252, 89);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // SheetSelect
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(252, 89);
            Controls.Add(flowLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SheetSelect";
            ShowIcon = false;
            Text = "Select Sheet to Use";
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ComboBox Selection;
        private Button Select;
        private Button Cancel;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}