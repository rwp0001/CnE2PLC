namespace CnE2PLC
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            toolStripContainer1 = new ToolStripContainer();
            statusStrip1 = new StatusStrip();
            toolStripProgressBar1 = new ToolStripProgressBar();
            toolStripTagCount = new ToolStripStatusLabel();
            toolStripUDTCount = new ToolStripStatusLabel();
            toolStripDeviceCount = new ToolStripStatusLabel();
            splitContainer1 = new SplitContainer();
            DataContainer = new SplitContainer();
            DevicesDataView = new DataGridView();
            TagsDataView = new DataGridView();
            LogText = new RichTextBox();
            contextMenuStrip_LogText = new ContextMenuStrip(components);
            copyToolStripMenuItem = new ToolStripMenuItem();
            clearLogToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem_CnE = new ToolStripMenuItem();
            openFileToolStripMenuItem_L5X = new ToolStripMenuItem();
            quitToolStripMenuItem = new ToolStripMenuItem();
            pLCToolStripMenuItem = new ToolStripMenuItem();
            getUDTsToolStripMenuItem = new ToolStripMenuItem();
            getTagsToolStripMenuItem = new ToolStripMenuItem();
            connectToPLCToolStripMenuItem = new ToolStripMenuItem();
            outputToolStripMenuItem = new ToolStripMenuItem();
            updateCnEToolStripMenuItem = new ToolStripMenuItem();
            toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.TopToolStripPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataContainer).BeginInit();
            DataContainer.Panel1.SuspendLayout();
            DataContainer.Panel2.SuspendLayout();
            DataContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DevicesDataView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TagsDataView).BeginInit();
            contextMenuStrip_LogText.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            toolStripContainer1.BottomToolStripPanel.Controls.Add(statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            toolStripContainer1.ContentPanel.Controls.Add(splitContainer1);
            toolStripContainer1.ContentPanel.Margin = new Padding(2);
            toolStripContainer1.ContentPanel.Size = new Size(575, 258);
            toolStripContainer1.Dock = DockStyle.Fill;
            toolStripContainer1.Location = new Point(0, 0);
            toolStripContainer1.Margin = new Padding(2);
            toolStripContainer1.Name = "toolStripContainer1";
            toolStripContainer1.Size = new Size(575, 304);
            toolStripContainer1.TabIndex = 0;
            toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            toolStripContainer1.TopToolStripPanel.Controls.Add(menuStrip1);
            // 
            // statusStrip1
            // 
            statusStrip1.Dock = DockStyle.None;
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripProgressBar1, toolStripTagCount, toolStripUDTCount, toolStripDeviceCount });
            statusStrip1.Location = new Point(0, 0);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(575, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            toolStripProgressBar1.Name = "toolStripProgressBar1";
            toolStripProgressBar1.Size = new Size(100, 16);
            toolStripProgressBar1.Click += toolStripProgressBar1_Click;
            // 
            // toolStripTagCount
            // 
            toolStripTagCount.Name = "toolStripTagCount";
            toolStripTagCount.Size = new Size(49, 17);
            toolStripTagCount.Text = "No Tags";
            // 
            // toolStripUDTCount
            // 
            toolStripUDTCount.Name = "toolStripUDTCount";
            toolStripUDTCount.Size = new Size(51, 17);
            toolStripUDTCount.Text = "No UDTs";
            toolStripUDTCount.Click += toolStripStatusLabel1_Click;
            // 
            // toolStripDeviceCount
            // 
            toolStripDeviceCount.Name = "toolStripDeviceCount";
            toolStripDeviceCount.Size = new Size(66, 17);
            toolStripDeviceCount.Text = "No Devices";
            toolStripDeviceCount.Click += toolStripDeviceCount_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(DataContainer);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(LogText);
            splitContainer1.Size = new Size(575, 258);
            splitContainer1.SplitterDistance = 191;
            splitContainer1.TabIndex = 1;
            // 
            // DataContainer
            // 
            DataContainer.Dock = DockStyle.Fill;
            DataContainer.Location = new Point(0, 0);
            DataContainer.Name = "DataContainer";
            // 
            // DataContainer.Panel1
            // 
            DataContainer.Panel1.Controls.Add(DevicesDataView);
            // 
            // DataContainer.Panel2
            // 
            DataContainer.Panel2.Controls.Add(TagsDataView);
            DataContainer.Size = new Size(575, 191);
            DataContainer.SplitterDistance = 191;
            DataContainer.TabIndex = 0;
            // 
            // DevicesDataView
            // 
            DevicesDataView.AllowUserToAddRows = false;
            DevicesDataView.AllowUserToOrderColumns = true;
            DevicesDataView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DevicesDataView.Dock = DockStyle.Fill;
            DevicesDataView.EditMode = DataGridViewEditMode.EditProgrammatically;
            DevicesDataView.Location = new Point(0, 0);
            DevicesDataView.Name = "DevicesDataView";
            DevicesDataView.ShowEditingIcon = false;
            DevicesDataView.Size = new Size(191, 191);
            DevicesDataView.TabIndex = 0;
            DevicesDataView.CellContentClick += DevicesDataView_CellContentClick;
            DevicesDataView.RowsAdded += DevicesDataView_RowsAdded;
            // 
            // TagsDataView
            // 
            TagsDataView.AllowUserToAddRows = false;
            TagsDataView.AllowUserToOrderColumns = true;
            TagsDataView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            TagsDataView.Dock = DockStyle.Fill;
            TagsDataView.EditMode = DataGridViewEditMode.EditProgrammatically;
            TagsDataView.Location = new Point(0, 0);
            TagsDataView.Name = "TagsDataView";
            TagsDataView.ShowEditingIcon = false;
            TagsDataView.Size = new Size(380, 191);
            TagsDataView.TabIndex = 1;
            // 
            // LogText
            // 
            LogText.ContextMenuStrip = contextMenuStrip_LogText;
            LogText.Dock = DockStyle.Fill;
            LogText.Location = new Point(0, 0);
            LogText.Margin = new Padding(2);
            LogText.Name = "LogText";
            LogText.ReadOnly = true;
            LogText.Size = new Size(575, 63);
            LogText.TabIndex = 0;
            LogText.Text = "";
            LogText.TextChanged += LogText_TextChanged;
            // 
            // contextMenuStrip_LogText
            // 
            contextMenuStrip_LogText.Items.AddRange(new ToolStripItem[] { copyToolStripMenuItem, clearLogToolStripMenuItem });
            contextMenuStrip_LogText.Name = "contextMenuStrip1";
            contextMenuStrip_LogText.Size = new Size(125, 48);
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(124, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // clearLogToolStripMenuItem
            // 
            clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            clearLogToolStripMenuItem.Size = new Size(124, 22);
            clearLogToolStripMenuItem.Text = "Clear Log";
            clearLogToolStripMenuItem.Click += clearLogToolStripMenuItem_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Dock = DockStyle.None;
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, pLCToolStripMenuItem, outputToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(575, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem_CnE, openFileToolStripMenuItem_L5X, quitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem_CnE
            // 
            toolStripMenuItem_CnE.Name = "toolStripMenuItem_CnE";
            toolStripMenuItem_CnE.Size = new Size(148, 22);
            toolStripMenuItem_CnE.Text = "Open CnE File";
            toolStripMenuItem_CnE.Click += toolStripMenuItem_CnE_Click;
            // 
            // openFileToolStripMenuItem_L5X
            // 
            openFileToolStripMenuItem_L5X.Name = "openFileToolStripMenuItem_L5X";
            openFileToolStripMenuItem_L5X.Size = new Size(148, 22);
            openFileToolStripMenuItem_L5X.Text = "Open L5X File";
            openFileToolStripMenuItem_L5X.Click += openFileToolStripMenuItem_Click;
            // 
            // quitToolStripMenuItem
            // 
            quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            quitToolStripMenuItem.Size = new Size(148, 22);
            quitToolStripMenuItem.Text = "Quit";
            quitToolStripMenuItem.Click += quitToolStripMenuItem_Click;
            // 
            // pLCToolStripMenuItem
            // 
            pLCToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { getUDTsToolStripMenuItem, getTagsToolStripMenuItem, connectToPLCToolStripMenuItem });
            pLCToolStripMenuItem.Name = "pLCToolStripMenuItem";
            pLCToolStripMenuItem.Size = new Size(40, 20);
            pLCToolStripMenuItem.Text = "PLC";
            // 
            // getUDTsToolStripMenuItem
            // 
            getUDTsToolStripMenuItem.Name = "getUDTsToolStripMenuItem";
            getUDTsToolStripMenuItem.Size = new Size(157, 22);
            getUDTsToolStripMenuItem.Text = "Get UDTs";
            getUDTsToolStripMenuItem.Click += getUDTsToolStripMenuItem_Click;
            // 
            // getTagsToolStripMenuItem
            // 
            getTagsToolStripMenuItem.Name = "getTagsToolStripMenuItem";
            getTagsToolStripMenuItem.Size = new Size(157, 22);
            getTagsToolStripMenuItem.Text = "Get Tags";
            getTagsToolStripMenuItem.Click += getTagsToolStripMenuItem_Click;
            // 
            // connectToPLCToolStripMenuItem
            // 
            connectToPLCToolStripMenuItem.Name = "connectToPLCToolStripMenuItem";
            connectToPLCToolStripMenuItem.Size = new Size(157, 22);
            connectToPLCToolStripMenuItem.Text = "Connect to PLC";
            connectToPLCToolStripMenuItem.Click += connectToPLCToolStripMenuItem_Click;
            // 
            // outputToolStripMenuItem
            // 
            outputToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { updateCnEToolStripMenuItem });
            outputToolStripMenuItem.Name = "outputToolStripMenuItem";
            outputToolStripMenuItem.Size = new Size(57, 20);
            outputToolStripMenuItem.Text = "Output";
            // 
            // updateCnEToolStripMenuItem
            // 
            updateCnEToolStripMenuItem.Name = "updateCnEToolStripMenuItem";
            updateCnEToolStripMenuItem.Size = new Size(136, 22);
            updateCnEToolStripMenuItem.Text = "Update CnE";
            updateCnEToolStripMenuItem.Click += updateCnEToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(575, 304);
            Controls.Add(toolStripContainer1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(2);
            Name = "Form1";
            Text = "CnE2PLC";
            FormClosing += FormIsClosing;
            toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            toolStripContainer1.BottomToolStripPanel.PerformLayout();
            toolStripContainer1.ContentPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.PerformLayout();
            toolStripContainer1.ResumeLayout(false);
            toolStripContainer1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            DataContainer.Panel1.ResumeLayout(false);
            DataContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DataContainer).EndInit();
            DataContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DevicesDataView).EndInit();
            ((System.ComponentModel.ISupportInitialize)TagsDataView).EndInit();
            contextMenuStrip_LogText.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ToolStripContainer toolStripContainer1;
        private StatusStrip statusStrip1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem quitToolStripMenuItem;
        private RichTextBox LogText;
        private ToolStripMenuItem openFileToolStripMenuItem_L5X;
        private ToolStripProgressBar toolStripProgressBar1;
        private ToolStripStatusLabel toolStripTagCount;
        private ToolStripStatusLabel toolStripDeviceCount;
        private SplitContainer splitContainer1;
        private SplitContainer DataContainer;
        private DataGridView DevicesDataView;
        private DataGridViewTextBoxColumn acceptButtonDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn autoScrollDataGridViewCheckBoxColumn;
        private DataGridViewCheckBoxColumn autoSizeDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn autoSizeModeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn autoValidateDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn backColorDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn formBorderStyleDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn cancelButtonDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn controlBoxDataGridViewCheckBoxColumn;
        private DataGridViewCheckBoxColumn helpButtonDataGridViewCheckBoxColumn;
        private DataGridViewImageColumn iconDataGridViewImageColumn;
        private DataGridViewCheckBoxColumn isMdiContainerDataGridViewCheckBoxColumn;
        private DataGridViewCheckBoxColumn keyPreviewDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn locationDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn maximumSizeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn mainMenuStripDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn minimumSizeDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn maximizeBoxDataGridViewCheckBoxColumn;
        private DataGridViewCheckBoxColumn mdiChildrenMinimizedAnchorBottomDataGridViewCheckBoxColumn;
        private DataGridViewCheckBoxColumn minimizeBoxDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn opacityDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn rightToLeftLayoutDataGridViewCheckBoxColumn;
        private DataGridViewCheckBoxColumn showInTaskbarDataGridViewCheckBoxColumn;
        private DataGridViewCheckBoxColumn showIconDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn sizeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn sizeGripStyleDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn startPositionDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn textDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn topMostDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn transparencyKeyDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn windowStateDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn autoScrollMarginDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn autoScrollMinSizeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn accessibleDescriptionDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn accessibleNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn accessibleRoleDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn allowDropDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn anchorDataGridViewTextBoxColumn;
        private DataGridViewImageColumn backgroundImageDataGridViewImageColumn;
        private DataGridViewTextBoxColumn backgroundImageLayoutDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn causesValidationDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn contextMenuStripDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn cursorDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn dataBindingsDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn dockDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn enabledDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn fontDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn foreColorDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn rightToLeftDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn tagDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn useWaitCursorDataGridViewCheckBoxColumn;
        private DataGridViewCheckBoxColumn visibleDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn paddingDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn imeModeDataGridViewTextBoxColumn;
        private ContextMenuStrip contextMenuStrip_LogText;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem clearLogToolStripMenuItem;
        private ToolStripMenuItem pLCToolStripMenuItem;
        private ToolStripMenuItem getUDTsToolStripMenuItem;
        private ToolStripMenuItem getTagsToolStripMenuItem;
        private ToolStripStatusLabel toolStripUDTCount;
        private DataGridView TagsDataView;
        private ToolStripMenuItem connectToPLCToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem_CnE;
        private BindingSource TagsBindingSource;
        private ToolStripMenuItem outputToolStripMenuItem;
        private ToolStripMenuItem updateCnEToolStripMenuItem;
    }
}
