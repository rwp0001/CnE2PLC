using CnE2PLC.PLC.XTO;

namespace CnE2PLC
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            tsContainer = new ToolStripContainer();
            status = new StatusStrip();
            tsProgressBar = new ToolStripProgressBar();
            tsTagCount = new ToolStripStatusLabel();
            tsUDTCount = new ToolStripStatusLabel();
            tsDeviceCount = new ToolStripStatusLabel();
            splitContainer = new SplitContainer();
            dgvTags = new DataGridView();
            dgvTxtCol_EquipNum = new DataGridViewTextBoxColumn();
            dgvTxtCol_Name = new DataGridViewTextBoxColumn();
            dgvTxtCol_Path = new DataGridViewTextBoxColumn();
            dgvTxtCol_DataType = new DataGridViewTextBoxColumn();
            dgvTxtCol_AOICalls = new DataGridViewTextBoxColumn();
            dgvTxtCol_References = new DataGridViewTextBoxColumn();
            dgvTxtCol_IO = new DataGridViewTextBoxColumn();
            dgvTxtCol_Description = new DataGridViewTextBoxColumn();
            dgvTxtCol_CfgEquipID = new DataGridViewTextBoxColumn();
            dgvTxtCol_CfgEquipDesc = new DataGridViewTextBoxColumn();
            bsTags = new BindingSource(components);
            txtLogText = new RichTextBox();
            mnuContext_LogText = new ContextMenuStrip(components);
            mnuContext_Copy = new ToolStripMenuItem();
            mnuContext_ClearLog = new ToolStripMenuItem();
            mnu = new MenuStrip();
            mnuFile = new ToolStripMenuItem();
            mnuFile_OpenCnE = new ToolStripMenuItem();
            mnuFileOpenL5X = new ToolStripMenuItem();
            mnuFile_Quit = new ToolStripMenuItem();
            mnuPLC = new ToolStripMenuItem();
            mnuPLC_GetUDTs = new ToolStripMenuItem();
            mnuPLC_GetTags = new ToolStripMenuItem();
            mnuPLC_Connect = new ToolStripMenuItem();
            mnuOutput = new ToolStripMenuItem();
            mnuOutput_UpdateCnE = new ToolStripMenuItem();
            mnuOutput_ExportTags = new ToolStripMenuItem();
            mnuOutput_IOReport = new ToolStripMenuItem();
            mnuOutput_InUseSummary = new ToolStripMenuItem();
            mnuFilter = new ToolStripMenuItem();
            mnuFilter_InUse = new ToolStripMenuItem();
            mnuFilter_Simmed = new ToolStripMenuItem();
            mnuFilter_Bypassed = new ToolStripMenuItem();
            mnuFilter_Alarmed = new ToolStripMenuItem();
            mnuFilter_Placeholder = new ToolStripMenuItem();
            mnuHelp = new ToolStripMenuItem();
            mnuHelp_About = new ToolStripMenuItem();
            tsFilters = new ToolStrip();
            tslblFilters = new ToolStripLabel();
            tslblFilter_InUse = new ToolStripLabel();
            tsbtnFilter_InUse = new ToolStripDropDownButton();
            tsmnuFilter_InUse_All = new ToolStripMenuItem();
            tsmnuFilter_InUse_Only = new ToolStripMenuItem();
            tsmnuFilter_InUse_Not = new ToolStripMenuItem();
            tsContainer.BottomToolStripPanel.SuspendLayout();
            tsContainer.ContentPanel.SuspendLayout();
            tsContainer.TopToolStripPanel.SuspendLayout();
            tsContainer.SuspendLayout();
            status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTags).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bsTags).BeginInit();
            mnuContext_LogText.SuspendLayout();
            mnu.SuspendLayout();
            tsFilters.SuspendLayout();
            SuspendLayout();
            // 
            // tsContainer
            // 
            // 
            // tsContainer.BottomToolStripPanel
            // 
            tsContainer.BottomToolStripPanel.Controls.Add(status);
            // 
            // tsContainer.ContentPanel
            // 
            tsContainer.ContentPanel.Controls.Add(splitContainer);
            tsContainer.ContentPanel.Margin = new Padding(2);
            tsContainer.ContentPanel.Size = new Size(967, 529);
            tsContainer.Dock = DockStyle.Fill;
            tsContainer.Location = new Point(0, 0);
            tsContainer.Margin = new Padding(2);
            tsContainer.Name = "tsContainer";
            tsContainer.Size = new Size(967, 600);
            tsContainer.TabIndex = 0;
            tsContainer.Text = "toolStripContainer1";
            // 
            // tsContainer.TopToolStripPanel
            // 
            tsContainer.TopToolStripPanel.Controls.Add(mnu);
            tsContainer.TopToolStripPanel.Controls.Add(tsFilters);
            // 
            // status
            // 
            status.Dock = DockStyle.None;
            status.ImageScalingSize = new Size(24, 24);
            status.Items.AddRange(new ToolStripItem[] { tsProgressBar, tsTagCount, tsUDTCount, tsDeviceCount });
            status.Location = new Point(0, 0);
            status.Name = "status";
            status.Size = new Size(967, 22);
            status.TabIndex = 0;
            status.Text = "statusStrip1";
            // 
            // tsProgressBar
            // 
            tsProgressBar.Name = "tsProgressBar";
            tsProgressBar.Size = new Size(100, 16);
            tsProgressBar.Click += toolStripProgressBar1_Click;
            // 
            // tsTagCount
            // 
            tsTagCount.Name = "tsTagCount";
            tsTagCount.Size = new Size(50, 17);
            tsTagCount.Text = "No Tags";
            // 
            // tsUDTCount
            // 
            tsUDTCount.Name = "tsUDTCount";
            tsUDTCount.Size = new Size(52, 17);
            tsUDTCount.Text = "No UDTs";
            // 
            // tsDeviceCount
            // 
            tsDeviceCount.Name = "tsDeviceCount";
            tsDeviceCount.Size = new Size(66, 17);
            tsDeviceCount.Text = "No Devices";
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 0);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(dgvTags);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(txtLogText);
            splitContainer.Size = new Size(967, 529);
            splitContainer.SplitterDistance = 391;
            splitContainer.TabIndex = 1;
            // 
            // dgvTags
            // 
            dgvTags.AllowUserToAddRows = false;
            dgvTags.AllowUserToOrderColumns = true;
            dgvTags.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvTags.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvTags.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTags.Columns.AddRange(new DataGridViewColumn[] { dgvTxtCol_EquipNum, dgvTxtCol_Name, dgvTxtCol_Path, dgvTxtCol_DataType, dgvTxtCol_AOICalls, dgvTxtCol_References, dgvTxtCol_IO, dgvTxtCol_Description, dgvTxtCol_CfgEquipID, dgvTxtCol_CfgEquipDesc });
            dgvTags.DataSource = bsTags;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvTags.DefaultCellStyle = dataGridViewCellStyle2;
            dgvTags.Dock = DockStyle.Fill;
            dgvTags.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvTags.Location = new Point(0, 0);
            dgvTags.Name = "dgvTags";
            dgvTags.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvTags.ShowEditingIcon = false;
            dgvTags.Size = new Size(967, 391);
            dgvTags.TabIndex = 1;
            dgvTags.ColumnHeaderMouseClick += TagsDataView_ColumnHeaderMouseClick;
            // 
            // dgvTxtCol_EquipNum
            // 
            dgvTxtCol_EquipNum.DataPropertyName = "EquipNum";
            dgvTxtCol_EquipNum.HeaderText = "Equipment Number";
            dgvTxtCol_EquipNum.Name = "dgvTxtCol_EquipNum";
            dgvTxtCol_EquipNum.ReadOnly = true;
            dgvTxtCol_EquipNum.Visible = false;
            // 
            // dgvTxtCol_Name
            // 
            dgvTxtCol_Name.DataPropertyName = "Name";
            dgvTxtCol_Name.HeaderText = "Tag Name";
            dgvTxtCol_Name.Name = "dgvTxtCol_Name";
            // 
            // dgvTxtCol_Path
            // 
            dgvTxtCol_Path.DataPropertyName = "Path";
            dgvTxtCol_Path.HeaderText = "Tag Scope";
            dgvTxtCol_Path.Name = "dgvTxtCol_Path";
            // 
            // dgvTxtCol_DataType
            // 
            dgvTxtCol_DataType.DataPropertyName = "DataType";
            dgvTxtCol_DataType.HeaderText = "Tag Data Type";
            dgvTxtCol_DataType.Name = "dgvTxtCol_DataType";
            // 
            // dgvTxtCol_AOICalls
            // 
            dgvTxtCol_AOICalls.DataPropertyName = "AOICalls";
            dgvTxtCol_AOICalls.HeaderText = "AOICalls";
            dgvTxtCol_AOICalls.Name = "dgvTxtCol_AOICalls";
            // 
            // dgvTxtCol_References
            // 
            dgvTxtCol_References.DataPropertyName = "References";
            dgvTxtCol_References.HeaderText = "References";
            dgvTxtCol_References.Name = "dgvTxtCol_References";
            // 
            // dgvTxtCol_IO
            // 
            dgvTxtCol_IO.DataPropertyName = "IO";
            dgvTxtCol_IO.HeaderText = "IO";
            dgvTxtCol_IO.Name = "dgvTxtCol_IO";
            dgvTxtCol_IO.ReadOnly = true;
            // 
            // dgvTxtCol_Description
            // 
            dgvTxtCol_Description.DataPropertyName = "Description";
            dgvTxtCol_Description.HeaderText = "Tag Description";
            dgvTxtCol_Description.Name = "dgvTxtCol_Description";
            // 
            // dgvTxtCol_CfgEquipID
            // 
            dgvTxtCol_CfgEquipID.DataPropertyName = "Cfg_EquipID";
            dgvTxtCol_CfgEquipID.HeaderText = "Equipment ID";
            dgvTxtCol_CfgEquipID.Name = "dgvTxtCol_CfgEquipID";
            // 
            // dgvTxtCol_CfgEquipDesc
            // 
            dgvTxtCol_CfgEquipDesc.DataPropertyName = "Cfg_EquipDesc";
            dgvTxtCol_CfgEquipDesc.HeaderText = "Equipment Description";
            dgvTxtCol_CfgEquipDesc.Name = "dgvTxtCol_CfgEquipDesc";
            // 
            // bsTags
            // 
            bsTags.DataSource = typeof(XTO_AOI);
            // 
            // txtLogText
            // 
            txtLogText.ContextMenuStrip = mnuContext_LogText;
            txtLogText.Dock = DockStyle.Fill;
            txtLogText.Location = new Point(0, 0);
            txtLogText.Margin = new Padding(2);
            txtLogText.Name = "txtLogText";
            txtLogText.ReadOnly = true;
            txtLogText.Size = new Size(967, 134);
            txtLogText.TabIndex = 0;
            txtLogText.Text = "";
            // 
            // mnuContext_LogText
            // 
            mnuContext_LogText.Items.AddRange(new ToolStripItem[] { mnuContext_Copy, mnuContext_ClearLog });
            mnuContext_LogText.Name = "contextMenuStrip1";
            mnuContext_LogText.Size = new Size(125, 48);
            // 
            // mnuContext_Copy
            // 
            mnuContext_Copy.Name = "mnuContext_Copy";
            mnuContext_Copy.Size = new Size(124, 22);
            mnuContext_Copy.Text = "Copy";
            mnuContext_Copy.Click += copyToolStripMenuItem_Click;
            // 
            // mnuContext_ClearLog
            // 
            mnuContext_ClearLog.Name = "mnuContext_ClearLog";
            mnuContext_ClearLog.Size = new Size(124, 22);
            mnuContext_ClearLog.Text = "Clear Log";
            mnuContext_ClearLog.Click += clearLogToolStripMenuItem_Click;
            // 
            // mnu
            // 
            mnu.AllowMerge = false;
            mnu.Dock = DockStyle.None;
            mnu.ImageScalingSize = new Size(24, 24);
            mnu.Items.AddRange(new ToolStripItem[] { mnuFile, mnuPLC, mnuOutput, mnuFilter, mnuHelp });
            mnu.Location = new Point(0, 0);
            mnu.Name = "mnu";
            mnu.Size = new Size(967, 24);
            mnu.TabIndex = 0;
            mnu.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            mnuFile.DropDownItems.AddRange(new ToolStripItem[] { mnuFile_OpenCnE, mnuFileOpenL5X, mnuFile_Quit });
            mnuFile.Name = "mnuFile";
            mnuFile.Size = new Size(37, 20);
            mnuFile.Text = "File";
            // 
            // mnuFile_OpenCnE
            // 
            mnuFile_OpenCnE.Enabled = false;
            mnuFile_OpenCnE.Name = "mnuFile_OpenCnE";
            mnuFile_OpenCnE.Size = new Size(148, 22);
            mnuFile_OpenCnE.Text = "Open CnE File";
            mnuFile_OpenCnE.Click += toolStripMenuItem_CnE_Click;
            // 
            // mnuFileOpenL5X
            // 
            mnuFileOpenL5X.Name = "mnuFileOpenL5X";
            mnuFileOpenL5X.Size = new Size(148, 22);
            mnuFileOpenL5X.Text = "Open L5X File";
            mnuFileOpenL5X.Click += openFileToolStripMenuItem_Click;
            // 
            // mnuFile_Quit
            // 
            mnuFile_Quit.Name = "mnuFile_Quit";
            mnuFile_Quit.Size = new Size(148, 22);
            mnuFile_Quit.Text = "Quit";
            mnuFile_Quit.Click += quitToolStripMenuItem_Click;
            // 
            // mnuPLC
            // 
            mnuPLC.DropDownItems.AddRange(new ToolStripItem[] { mnuPLC_GetUDTs, mnuPLC_GetTags, mnuPLC_Connect });
            mnuPLC.Name = "mnuPLC";
            mnuPLC.Size = new Size(40, 20);
            mnuPLC.Text = "PLC";
            // 
            // mnuPLC_GetUDTs
            // 
            mnuPLC_GetUDTs.Name = "mnuPLC_GetUDTs";
            mnuPLC_GetUDTs.Size = new Size(157, 22);
            mnuPLC_GetUDTs.Text = "Get UDTs";
            mnuPLC_GetUDTs.Click += getUDTsToolStripMenuItem_Click;
            // 
            // mnuPLC_GetTags
            // 
            mnuPLC_GetTags.Name = "mnuPLC_GetTags";
            mnuPLC_GetTags.Size = new Size(157, 22);
            mnuPLC_GetTags.Text = "Get Tags";
            mnuPLC_GetTags.Click += getTagsToolStripMenuItem_Click;
            // 
            // mnuPLC_Connect
            // 
            mnuPLC_Connect.Name = "mnuPLC_Connect";
            mnuPLC_Connect.Size = new Size(157, 22);
            mnuPLC_Connect.Text = "Connect to PLC";
            // 
            // mnuOutput
            // 
            mnuOutput.DropDownItems.AddRange(new ToolStripItem[] { mnuOutput_UpdateCnE, mnuOutput_ExportTags, mnuOutput_IOReport, mnuOutput_InUseSummary });
            mnuOutput.Name = "mnuOutput";
            mnuOutput.Size = new Size(57, 20);
            mnuOutput.Text = "Output";
            // 
            // mnuOutput_UpdateCnE
            // 
            mnuOutput_UpdateCnE.Name = "mnuOutput_UpdateCnE";
            mnuOutput_UpdateCnE.Size = new Size(160, 22);
            mnuOutput_UpdateCnE.Text = "Update CnE";
            mnuOutput_UpdateCnE.Click += updateCnEToolStripMenuItem_Click;
            // 
            // mnuOutput_ExportTags
            // 
            mnuOutput_ExportTags.Name = "mnuOutput_ExportTags";
            mnuOutput_ExportTags.Size = new Size(160, 22);
            mnuOutput_ExportTags.Text = "Export Tags";
            mnuOutput_ExportTags.Click += exportTagsToolStripMenuItem_Click;
            // 
            // mnuOutput_IOReport
            // 
            mnuOutput_IOReport.Name = "mnuOutput_IOReport";
            mnuOutput_IOReport.Size = new Size(160, 22);
            mnuOutput_IOReport.Text = "IO Report";
            mnuOutput_IOReport.Click += aiReportToolStripMenuItem_Click;
            // 
            // mnuOutput_InUseSummary
            // 
            mnuOutput_InUseSummary.Name = "mnuOutput_InUseSummary";
            mnuOutput_InUseSummary.Size = new Size(160, 22);
            mnuOutput_InUseSummary.Text = "In Use Summary";
            mnuOutput_InUseSummary.Click += inUseSummaryToolStripMenuItem_Click;
            // 
            // mnuFilter
            // 
            mnuFilter.DropDownItems.AddRange(new ToolStripItem[] { mnuFilter_InUse, mnuFilter_Simmed, mnuFilter_Bypassed, mnuFilter_Alarmed, mnuFilter_Placeholder });
            mnuFilter.Name = "mnuFilter";
            mnuFilter.Size = new Size(45, 20);
            mnuFilter.Text = "Filter";
            // 
            // mnuFilter_InUse
            // 
            mnuFilter_InUse.Name = "mnuFilter_InUse";
            mnuFilter_InUse.Size = new Size(136, 22);
            mnuFilter_InUse.Text = "In Use";
            mnuFilter_InUse.Click += inUseToolStripMenuItem_Click;
            // 
            // mnuFilter_Simmed
            // 
            mnuFilter_Simmed.Name = "mnuFilter_Simmed";
            mnuFilter_Simmed.Size = new Size(136, 22);
            mnuFilter_Simmed.Text = "Simmed";
            mnuFilter_Simmed.Click += simmedToolStripMenuItem_Click;
            // 
            // mnuFilter_Bypassed
            // 
            mnuFilter_Bypassed.Name = "mnuFilter_Bypassed";
            mnuFilter_Bypassed.Size = new Size(136, 22);
            mnuFilter_Bypassed.Text = "Bypassed";
            mnuFilter_Bypassed.Click += bypassedToolStripMenuItem_Click;
            // 
            // mnuFilter_Alarmed
            // 
            mnuFilter_Alarmed.Name = "mnuFilter_Alarmed";
            mnuFilter_Alarmed.Size = new Size(136, 22);
            mnuFilter_Alarmed.Text = "Alarmed";
            mnuFilter_Alarmed.Click += alarmedToolStripMenuItem_Click;
            // 
            // mnuFilter_Placeholder
            // 
            mnuFilter_Placeholder.Name = "mnuFilter_Placeholder";
            mnuFilter_Placeholder.Size = new Size(136, 22);
            mnuFilter_Placeholder.Text = "Placeholder";
            mnuFilter_Placeholder.Click += placeholderToolStripMenuItem_Click;
            // 
            // mnuHelp
            // 
            mnuHelp.DropDownItems.AddRange(new ToolStripItem[] { mnuHelp_About });
            mnuHelp.Name = "mnuHelp";
            mnuHelp.Size = new Size(44, 20);
            mnuHelp.Text = "Help";
            // 
            // mnuHelp_About
            // 
            mnuHelp_About.Name = "mnuHelp_About";
            mnuHelp_About.Size = new Size(107, 22);
            mnuHelp_About.Text = "About";
            mnuHelp_About.Click += About_MenuItem_Click;
            // 
            // tsFilters
            // 
            tsFilters.Dock = DockStyle.None;
            tsFilters.Items.AddRange(new ToolStripItem[] { tslblFilters, tslblFilter_InUse, tsbtnFilter_InUse });
            tsFilters.Location = new Point(3, 24);
            tsFilters.Name = "tsFilters";
            tsFilters.Size = new Size(129, 25);
            tsFilters.TabIndex = 1;
            // 
            // tslblFilters
            // 
            tslblFilters.Name = "tslblFilters";
            tslblFilters.Size = new Size(41, 22);
            tslblFilters.Text = "Filters:";
            // 
            // tslblFilter_InUse
            // 
            tslblFilter_InUse.Name = "tslblFilter_InUse";
            tslblFilter_InUse.Size = new Size(42, 22);
            tslblFilter_InUse.Text = "In Use:";
            // 
            // tsbtnFilter_InUse
            // 
            tsbtnFilter_InUse.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsbtnFilter_InUse.DropDownItems.AddRange(new ToolStripItem[] { tsmnuFilter_InUse_All, tsmnuFilter_InUse_Only, tsmnuFilter_InUse_Not });
            tsbtnFilter_InUse.Image = (Image)resources.GetObject("tsbtnFilter_InUse.Image");
            tsbtnFilter_InUse.ImageTransparentColor = Color.Magenta;
            tsbtnFilter_InUse.Name = "tsbtnFilter_InUse";
            tsbtnFilter_InUse.Size = new Size(34, 22);
            tsbtnFilter_InUse.Text = "All";
            // 
            // tsmnuFilter_InUse_All
            // 
            tsmnuFilter_InUse_All.Checked = true;
            tsmnuFilter_InUse_All.CheckState = CheckState.Checked;
            tsmnuFilter_InUse_All.Name = "tsmnuFilter_InUse_All";
            tsmnuFilter_InUse_All.Size = new Size(157, 22);
            tsmnuFilter_InUse_All.Text = "All";
            tsmnuFilter_InUse_All.Click += tsmnuFilter_InUse_All_Click;
            // 
            // tsmnuFilter_InUse_Only
            // 
            tsmnuFilter_InUse_Only.Name = "tsmnuFilter_InUse_Only";
            tsmnuFilter_InUse_Only.Size = new Size(157, 22);
            tsmnuFilter_InUse_Only.Text = "In Use Only";
            tsmnuFilter_InUse_Only.Click += tsmnuFilter_InUse_Only_Click;
            // 
            // tsmnuFilter_InUse_Not
            // 
            tsmnuFilter_InUse_Not.Name = "tsmnuFilter_InUse_Not";
            tsmnuFilter_InUse_Not.Size = new Size(157, 22);
            tsmnuFilter_InUse_Not.Text = "Not In Use Only";
            tsmnuFilter_InUse_Not.Click += tsmnuFilter_InUse_Not_Click;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(967, 600);
            Controls.Add(tsContainer);
            MainMenuStrip = mnu;
            Margin = new Padding(2);
            Name = "frmMain";
            StartPosition = FormStartPosition.Manual;
            Text = "CnE2PLC";
            FormClosing += FormIsClosing;
            tsContainer.BottomToolStripPanel.ResumeLayout(false);
            tsContainer.BottomToolStripPanel.PerformLayout();
            tsContainer.ContentPanel.ResumeLayout(false);
            tsContainer.TopToolStripPanel.ResumeLayout(false);
            tsContainer.TopToolStripPanel.PerformLayout();
            tsContainer.ResumeLayout(false);
            tsContainer.PerformLayout();
            status.ResumeLayout(false);
            status.PerformLayout();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTags).EndInit();
            ((System.ComponentModel.ISupportInitialize)bsTags).EndInit();
            mnuContext_LogText.ResumeLayout(false);
            mnu.ResumeLayout(false);
            mnu.PerformLayout();
            tsFilters.ResumeLayout(false);
            tsFilters.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ToolStripContainer tsContainer;
        private StatusStrip status;
        private MenuStrip mnu;
        private ToolStripMenuItem mnuFile;
        private ToolStripMenuItem mnuFile_Quit;
        private ToolStripMenuItem mnuFileOpenL5X;
        private ToolStripProgressBar tsProgressBar;
        private ToolStripStatusLabel tsTagCount;
        private ToolStripStatusLabel tsDeviceCount;
        //private DataGridViewCheckBoxColumn maximizeBoxDataGridViewCheckBoxColumn;
        //private DataGridViewCheckBoxColumn mdiChildrenMinimizedAnchorBottomDataGridViewCheckBoxColumn;
        //private DataGridViewCheckBoxColumn minimizeBoxDataGridViewCheckBoxColumn;
        //private DataGridViewTextBoxColumn opacityDataGridViewTextBoxColumn;
        //private DataGridViewCheckBoxColumn rightToLeftLayoutDataGridViewCheckBoxColumn;
        //private DataGridViewCheckBoxColumn showInTaskbarDataGridViewCheckBoxColumn;
        //private DataGridViewCheckBoxColumn showIconDataGridViewCheckBoxColumn;
        //private DataGridViewTextBoxColumn sizeDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn sizeGripStyleDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn startPositionDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn textDataGridViewTextBoxColumn;
        //private DataGridViewCheckBoxColumn topMostDataGridViewCheckBoxColumn;
        //private DataGridViewTextBoxColumn transparencyKeyDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn windowStateDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn autoScrollMarginDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn autoScrollMinSizeDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn accessibleDescriptionDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn accessibleNameDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn accessibleRoleDataGridViewTextBoxColumn;
        //private DataGridViewCheckBoxColumn allowDropDataGridViewCheckBoxColumn;
        //private DataGridViewTextBoxColumn anchorDataGridViewTextBoxColumn;
        //private DataGridViewImageColumn backgroundImageDataGridViewImageColumn;
        //private DataGridViewTextBoxColumn backgroundImageLayoutDataGridViewTextBoxColumn;
        //private DataGridViewCheckBoxColumn causesValidationDataGridViewCheckBoxColumn;
        //private DataGridViewTextBoxColumn contextMenuStripDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn cursorDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn dataBindingsDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn dockDataGridViewTextBoxColumn;
        //private DataGridViewCheckBoxColumn enabledDataGridViewCheckBoxColumn;
        //private DataGridViewTextBoxColumn fontDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn foreColorDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn rightToLeftDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn tagDataGridViewTextBoxColumn;
        //private DataGridViewCheckBoxColumn useWaitCursorDataGridViewCheckBoxColumn;
        //private DataGridViewCheckBoxColumn visibleDataGridViewCheckBoxColumn;
        //private DataGridViewTextBoxColumn paddingDataGridViewTextBoxColumn;
        //private DataGridViewTextBoxColumn imeModeDataGridViewTextBoxColumn;
        private ContextMenuStrip mnuContext_LogText;
        private ToolStripMenuItem mnuContext_Copy;
        private ToolStripMenuItem mnuContext_ClearLog;
        private ToolStripMenuItem mnuPLC;
        private ToolStripMenuItem mnuPLC_GetUDTs;
        private ToolStripMenuItem mnuPLC_GetTags;
        private ToolStripStatusLabel tsUDTCount;
        private ToolStripMenuItem mnuPLC_Connect;
        private ToolStripMenuItem mnuFile_OpenCnE;
        //private BindingSource TagsBindingSource;
        private ToolStripMenuItem mnuOutput;
        private ToolStripMenuItem mnuOutput_UpdateCnE;
        private ToolStripMenuItem mnuOutput_ExportTags;
        public BindingSource bsTags;
        private ToolStripMenuItem mnuHelp;
        private ToolStripMenuItem mnuHelp_About;
        private ToolStripMenuItem mnuOutput_IOReport;
        private ToolStripMenuItem mnuFilter;
        private ToolStripMenuItem mnuOutput_InUseSummary;
        private ToolStripMenuItem mnuFilter_InUse;
        private ToolStripMenuItem mnuFilter_Simmed;
        private ToolStripMenuItem mnuFilter_Bypassed;
        private ToolStripMenuItem mnuFilter_Alarmed;
        private ToolStripMenuItem mnuFilter_Placeholder;
        private SplitContainer splitContainer;
        public DataGridView dgvTags;
        private DataGridViewTextBoxColumn dgvTxtCol_EquipNum;
        private DataGridViewTextBoxColumn dgvTxtCol_Name;
        private DataGridViewTextBoxColumn dgvTxtCol_Path;
        private DataGridViewTextBoxColumn dgvTxtCol_DataType;
        private DataGridViewTextBoxColumn dgvTxtCol_AOICalls;
        private DataGridViewTextBoxColumn dgvTxtCol_References;
        private DataGridViewTextBoxColumn dgvTxtCol_IO;
        private DataGridViewTextBoxColumn dgvTxtCol_Description;
        private DataGridViewTextBoxColumn dgvTxtCol_CfgEquipID;
        private DataGridViewTextBoxColumn dgvTxtCol_CfgEquipDesc;
        private RichTextBox txtLogText;
        private ToolStrip tsFilters;
        private ToolStripLabel tslblFilters;
        private ToolStripLabel tslblFilter_InUse;
        private ToolStripDropDownButton tsbtnFilter_InUse;
        private ToolStripMenuItem tsmnuFilter_InUse_All;
        private ToolStripMenuItem tsmnuFilter_InUse_Only;
        private ToolStripMenuItem tsmnuFilter_InUse_Not;
    }
}
