using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using CnE2PLC.Properties;

namespace CnE2PLC
{

    public partial class Form1 : Form
    {
        /// <summary>
        /// Main datastore
        /// </summary>
        Controller PLC = new();

        int SortColumn = -1;
        ListSortDirection SortDir = ListSortDirection.Ascending;
        bool ExcelUseable = false;
        bool ScrollToBottom = true;

        public Form1()
        {

            InitializeComponent();
            TagsDataView.CellFormatting += new DataGridViewCellFormattingEventHandler(this.TagsDataView_CellFormatting);
            TagsDataView.CellToolTipTextNeeded += new DataGridViewCellToolTipTextNeededEventHandler(this.TagsDataView_CellToolTipTextNeeded);

            LogText.Text = string.Format("Startup at time: {0}\n", DateTime.Now);

            //TagsDataView.AutoGenerateColumns = true;
            TagsDataView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            if (Settings.Default.WindowPos != Point.Empty)
            {
                if (isPointVisibleOnAScreen(Settings.Default.WindowPos)) this.Location = Settings.Default.WindowPos;
                this.Location = Settings.Default.WindowPos;
            }
            if (Settings.Default.WindowSize != Size.Empty) this.Size = Settings.Default.WindowSize;
            if (Settings.Default.Debug)
            {
                statusStrip1.BackColor = Color.Yellow;
                splitContainer1.Panel2Collapsed = false;
            }
            else
            {
                statusStrip1.BackColor = Control.DefaultBackColor;
                splitContainer1.Panel2Collapsed = true;
            }
            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType != null) ExcelUseable = true;

            //if (!ExcelUseable)
            //{
            //    exportTagsToolStripMenuItem.Enabled = false;
            //    exportTagsToolStripMenuItem.ToolTipText = "Excel not Installed.";
            //    updateCnEToolStripMenuItem.Enabled = false;
            //    updateCnEToolStripMenuItem.ToolTipText = "Excel not Installed.";
            //    toolStripMenuItem_CnE.Enabled = false;
            //    toolStripMenuItem_CnE.ToolTipText = "Excel not Installed.";
            //}

        }

        bool isPointVisibleOnAScreen(Point p)
        {
            foreach (Screen s in Screen.AllScreens)
            {
                if (p.X < s.Bounds.Right && p.X > s.Bounds.Left && p.Y > s.Bounds.Top && p.Y < s.Bounds.Bottom)
                    return true;
            }
            return false;
        }

        bool isFormFullyVisible(Form f)
        {
            return isPointVisibleOnAScreen(new Point(f.Left, f.Top)) && isPointVisibleOnAScreen(new Point(f.Right, f.Top)) && isPointVisibleOnAScreen(new Point(f.Left, f.Bottom)) && isPointVisibleOnAScreen(new Point(f.Right, f.Bottom));
        }

        private void TagsDataView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            PLCTag tag = (PLCTag)TagsDataView.Rows[e.RowIndex].DataBoundItem;
            tag.CellFormatting(sender, e);
        }

        void TagsDataView_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow dataGridViewRow1 = TagsDataView.Rows[e.RowIndex];
                e.ToolTipText = $"{dataGridViewRow1.DataBoundItem.ToString()}";
            }
        }

        private void TagsDataView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (SortColumn == e.ColumnIndex)
            {
                switch (SortDir)
                {
                    case ListSortDirection.Ascending:
                        SortDir = ListSortDirection.Descending;
                        break;
                    case ListSortDirection.Descending:
                        SortDir = ListSortDirection.Ascending;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                SortDir = ListSortDirection.Ascending;
            }

            //TagsDataView.Sort(this.TagsDataView.Columns[e.ColumnIndex], SortDir);
        }


        private void quitToolStripMenuItem_Click(object sender, EventArgs e) { System.Windows.Forms.Application.Exit(); }



        private void toolStripMenuItem_CnE_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse C&E Files",
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //PLC.ProcessCnEFile(openFileDialog1.FileName, out CnE_Devices);
            }
        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {
            Settings.Default.Debug = !Settings.Default.Debug;

            if (Settings.Default.Debug)
            {
                statusStrip1.BackColor = Color.Yellow;
                splitContainer1.Panel2Collapsed = false;
            } else
            {
                statusStrip1.BackColor = Control.DefaultBackColor;
                splitContainer1.Panel2Collapsed = true;
            }
        }

        private void LogText_TextChanged(object sender, EventArgs e)
        {
            if (ScrollToBottom) { 
                
            }
        }

        private void DevivicesBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            //Application.DoEvents();
        }

        private void toolStripDeviceCount_Click(object sender, EventArgs e)
        {
            //Application.DoEvents();
        }

        private void DevicesDataView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                object obj = TagsDataView.Rows[e.RowIndex].DataBoundItem;
                Application.DoEvents();
            }
            catch (Exception)
            {


            }

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(LogText.SelectedText.ToString());
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogText.Clear();
        }

        private void getUDTsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogText.Text += PLC.PrintUdtTags();
        }

        private void getTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogText.Text += PLC.PrintTags();
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void connectToPLCToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.WindowPos = this.Location;
            Settings.Default.WindowSize = this.Size;
            Settings.Default.Save();

        }

        private void updateCnEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse C&E Files",
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            //if (openFileDialog1.ShowDialog() == DialogResult.OK) PLCTag.UpdateCnE(openFileDialog1.FileName, CnE_Tags);

        }

        private void exportTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(PLC.AllTags.Count ==  0) return;
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Select C&E Template File",
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                PLC.CreateCnE(openFileDialog1.FileName);
            }
        }


        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse L5X Files",
                Filter = "Logix L5X files (*.l5x)|*.l5x|All files (*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string FileData = File.ReadAllText(openFileDialog1.FileName);
                    XmlDocument XmlDoc = new XmlDocument();
                    XmlDoc.LoadXml(FileData);
                    XmlNode ControllerNode = XmlDoc.SelectNodes("/RSLogix5000Content/Controller")[0];
                    PLC = new(ControllerNode);
                    Tags_DGV_Source.DataSource = PLC.AOI_Tags;
                    this.Text = $"{AssemblyTitle()}  {PLC.ToString()}";
                    toolStripTagCount.Text = $"Tags: {PLC.AllTags.Count}";

                }
                catch (Exception ex)
                {
                    var r = MessageBox.Show($"Error: {ex.Message}", "Import L5X Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }

        private void About_MenuItem_Click(object sender, EventArgs e)
        {
            Form AboutBox = new AboutBox1();
            AboutBox.ShowDialog();
        }

        private void Tags_DGV_Source_CurrentChanged(object sender, EventArgs e)
        {

        }

        [DebuggerStepThrough]
        private string AssemblyTitle()
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attributes.Length > 0)
            {
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                if (titleAttribute.Title != "") return titleAttribute.Title;
            }
            return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
        }

        private void aiReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PLC.CreateIOReport();
        }

        private void inUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PLC.Filter_NotInUse = !PLC.Filter_NotInUse;
            inUseToolStripMenuItem.Checked = PLC.Filter_NotInUse;
            Tags_DGV_Source.Filter = PLC.Filter_NotInUse ? "NotInUse = true" : "NotInUse = false";
            TagsDataView.Refresh();
        }
    }

}

