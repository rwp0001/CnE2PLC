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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            toolStripContainer1 = new ToolStripContainer();
            statusStrip1 = new StatusStrip();
            toolStripProgressBar1 = new ToolStripProgressBar();
            toolStripTagCount = new ToolStripStatusLabel();
            toolStripUDTCount = new ToolStripStatusLabel();
            toolStripDeviceCount = new ToolStripStatusLabel();
            splitContainer1 = new SplitContainer();
            TagsDataView = new DataGridView();
            Tags_DGV_Source = new BindingSource(components);
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
            exportTagsToolStripMenuItem = new ToolStripMenuItem();
            aiReportToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            About_MenuItem = new ToolStripMenuItem();
            equipNumDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            pathDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            dataTypeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            AOICalls = new DataGridViewTextBoxColumn();
            References = new DataGridViewTextBoxColumn();
            IO = new DataGridViewTextBoxColumn();
            descriptionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            cfgEquipIDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            cfgEquipDescDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.TopToolStripPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TagsDataView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Tags_DGV_Source).BeginInit();
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
            splitContainer1.Panel1.Controls.Add(TagsDataView);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(LogText);
            splitContainer1.Size = new Size(575, 258);
            splitContainer1.SplitterDistance = 191;
            splitContainer1.TabIndex = 1;
            // 
            // TagsDataView
            // 
            TagsDataView.AllowUserToAddRows = false;
            TagsDataView.AllowUserToOrderColumns = true;
            TagsDataView.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            TagsDataView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            TagsDataView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            TagsDataView.Columns.AddRange(new DataGridViewColumn[] { equipNumDataGridViewTextBoxColumn, pathDataGridViewTextBoxColumn, dataTypeDataGridViewTextBoxColumn, nameDataGridViewTextBoxColumn, AOICalls, References, IO, descriptionDataGridViewTextBoxColumn, cfgEquipIDDataGridViewTextBoxColumn, cfgEquipDescDataGridViewTextBoxColumn });
            TagsDataView.DataSource = Tags_DGV_Source;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            TagsDataView.DefaultCellStyle = dataGridViewCellStyle2;
            TagsDataView.Dock = DockStyle.Fill;
            TagsDataView.EditMode = DataGridViewEditMode.EditProgrammatically;
            TagsDataView.Location = new Point(0, 0);
            TagsDataView.Name = "TagsDataView";
            TagsDataView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            TagsDataView.ShowEditingIcon = false;
            TagsDataView.Size = new Size(575, 191);
            TagsDataView.TabIndex = 1;
            TagsDataView.CellClick += DevicesDataView_CellContentClick;
            // 
            // Tags_DGV_Source
            // 
            Tags_DGV_Source.DataSource = typeof(XTO_AOI);
            Tags_DGV_Source.CurrentChanged += Tags_DGV_Source_CurrentChanged;
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
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, pLCToolStripMenuItem, outputToolStripMenuItem, helpToolStripMenuItem });
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
            toolStripMenuItem_CnE.Enabled = false;
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
            pLCToolStripMenuItem.Enabled = false;
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
            outputToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { updateCnEToolStripMenuItem, exportTagsToolStripMenuItem, aiReportToolStripMenuItem });
            outputToolStripMenuItem.Name = "outputToolStripMenuItem";
            outputToolStripMenuItem.Size = new Size(57, 20);
            outputToolStripMenuItem.Text = "Output";
            // 
            // updateCnEToolStripMenuItem
            // 
            updateCnEToolStripMenuItem.Enabled = false;
            updateCnEToolStripMenuItem.Name = "updateCnEToolStripMenuItem";
            updateCnEToolStripMenuItem.Size = new Size(136, 22);
            updateCnEToolStripMenuItem.Text = "Update CnE";
            updateCnEToolStripMenuItem.Click += updateCnEToolStripMenuItem_Click;
            // 
            // exportTagsToolStripMenuItem
            // 
            exportTagsToolStripMenuItem.Name = "exportTagsToolStripMenuItem";
            exportTagsToolStripMenuItem.Size = new Size(136, 22);
            exportTagsToolStripMenuItem.Text = "Export Tags";
            exportTagsToolStripMenuItem.Click += exportTagsToolStripMenuItem_Click;
            // 
            // aiReportToolStripMenuItem
            // 
            aiReportToolStripMenuItem.Name = "aiReportToolStripMenuItem";
            aiReportToolStripMenuItem.Size = new Size(136, 22);
            aiReportToolStripMenuItem.Text = "IO Report";
            aiReportToolStripMenuItem.Click += aiReportToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { About_MenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // About_MenuItem
            // 
            About_MenuItem.Name = "About_MenuItem";
            About_MenuItem.Size = new Size(107, 22);
            About_MenuItem.Text = "About";
            About_MenuItem.Click += About_MenuItem_Click;
            // 
            // equipNumDataGridViewTextBoxColumn
            // 
            equipNumDataGridViewTextBoxColumn.DataPropertyName = "EquipNum";
            equipNumDataGridViewTextBoxColumn.HeaderText = "Equipment Number";
            equipNumDataGridViewTextBoxColumn.Name = "equipNumDataGridViewTextBoxColumn";
            equipNumDataGridViewTextBoxColumn.ReadOnly = true;
            equipNumDataGridViewTextBoxColumn.Visible = false;
            // 
            // pathDataGridViewTextBoxColumn
            // 
            pathDataGridViewTextBoxColumn.DataPropertyName = "Path";
            pathDataGridViewTextBoxColumn.HeaderText = "Tag Scope";
            pathDataGridViewTextBoxColumn.Name = "pathDataGridViewTextBoxColumn";
            // 
            // dataTypeDataGridViewTextBoxColumn
            // 
            dataTypeDataGridViewTextBoxColumn.DataPropertyName = "DataType";
            dataTypeDataGridViewTextBoxColumn.HeaderText = "Tag Data Type";
            dataTypeDataGridViewTextBoxColumn.Name = "dataTypeDataGridViewTextBoxColumn";
            // 
            // nameDataGridViewTextBoxColumn
            // 
            nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            nameDataGridViewTextBoxColumn.HeaderText = "Tag Name";
            nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // AOICalls
            // 
            AOICalls.DataPropertyName = "AOICalls";
            AOICalls.HeaderText = "AOICalls";
            AOICalls.Name = "AOICalls";
            // 
            // References
            // 
            References.DataPropertyName = "References";
            References.HeaderText = "References";
            References.Name = "References";
            // 
            // IO
            // 
            IO.DataPropertyName = "IO";
            IO.HeaderText = "IO";
            IO.Name = "IO";
            IO.ReadOnly = true;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            descriptionDataGridViewTextBoxColumn.HeaderText = "Tag Description";
            descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // cfgEquipIDDataGridViewTextBoxColumn
            // 
            cfgEquipIDDataGridViewTextBoxColumn.DataPropertyName = "Cfg_EquipID";
            cfgEquipIDDataGridViewTextBoxColumn.HeaderText = "Equipment ID";
            cfgEquipIDDataGridViewTextBoxColumn.Name = "cfgEquipIDDataGridViewTextBoxColumn";
            // 
            // cfgEquipDescDataGridViewTextBoxColumn
            // 
            cfgEquipDescDataGridViewTextBoxColumn.DataPropertyName = "Cfg_EquipDesc";
            cfgEquipDescDataGridViewTextBoxColumn.HeaderText = "Equipment Description";
            cfgEquipDescDataGridViewTextBoxColumn.Name = "cfgEquipDescDataGridViewTextBoxColumn";
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
            ((System.ComponentModel.ISupportInitialize)TagsDataView).EndInit();
            ((System.ComponentModel.ISupportInitialize)Tags_DGV_Source).EndInit();
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
        private ToolStripMenuItem connectToPLCToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem_CnE;
        private BindingSource TagsBindingSource;
        private ToolStripMenuItem outputToolStripMenuItem;
        private ToolStripMenuItem updateCnEToolStripMenuItem;
        private ToolStripMenuItem exportTagsToolStripMenuItem;
        public DataGridView TagsDataView;
        public BindingSource Tags_DGV_Source;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem About_MenuItem;
        private ToolStripMenuItem aiReportToolStripMenuItem;
        private DataGridViewTextBoxColumn equipNumDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn pathDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn dataTypeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn AOICalls;
        private DataGridViewTextBoxColumn References;
        private DataGridViewTextBoxColumn IO;
        private DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn cfgEquipIDDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn cfgEquipDescDataGridViewTextBoxColumn;
    }
}
