using CnE2PLC.PLC;
using CnE2PLC.PLC.XTO;
using CnE2PLC.Properties;
using CnE2PLC.Reporting;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace CnE2PLC
{

    public partial class frmMain : Form
    {
        /// <summary>
        /// Main datastore
        /// </summary>
        Controller PLC = new();

        int SortColumn = -1;
        ListSortDirection SortDir = ListSortDirection.Ascending;

        public frmMain()
        {

            InitializeComponent();
            TagsDataView.CellFormatting += new DataGridViewCellFormattingEventHandler(TagsDataView_CellFormatting);
            TagsDataView.CellToolTipTextNeeded += new DataGridViewCellToolTipTextNeededEventHandler(TagsDataView_CellToolTipTextNeeded);

            LogText.Text = string.Format("Startup at time: {0}\n", DateTime.Now);

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

        private void TagsDataView_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            PLCTag? tag = (PLCTag?)TagsDataView.Rows[e.RowIndex].DataBoundItem;

            if (tag is XTO_AOI)
            {
                if (((XTO_AOI)tag).Simmed)
                {
                    e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.ForeColor = Color.White;
                    e.CellStyle.Font = new Font(e.CellStyle.Font ?? Control.DefaultFont, FontStyle.Bold);
                    return;
                }

                if (((XTO_AOI)tag).NotInUse || ((XTO_AOI)tag).Placeholder)
                {
                    e.CellStyle.BackColor = Color.LightGray;
                }

                if (((XTO_AOI)tag).AOICalls == 0)
                {
                    e.CellStyle.BackColor = Color.LightGray;
                    e.CellStyle.Font = new Font(e.CellStyle.Font ?? Control.DefaultFont, FontStyle.Italic);

                }

                if (((XTO_AOI)tag).References == 0)
                {
                    e.CellStyle.ForeColor = Color.DarkCyan;
                }
            }
            if (tag is AIData)
            {

                if (((AIData)tag).BadPVAlarm == true)
                {
                    e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.ForeColor = Color.Cyan;
                    return;
                }

                if (((AIData)tag).HiAlarm == true || ((AIData)tag).LoAlarm == true)
                {
                    e.CellStyle.ForeColor = Color.DarkOrange;
                }

                if (((AIData)tag).HiHiAlarm == true || ((AIData)tag).LoLoAlarm == true)
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
            }
            if (tag is DIData)
            {
                if (((DIData)tag).Alarm == true)
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
            }
            if (tag is Intlk_8)
            {
                if (((Intlk_8)tag).BypActive == true)
                {
                    e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.ForeColor = Color.White;
                    e.CellStyle.Font = new Font(e.CellStyle.Font ?? Control.DefaultFont, FontStyle.Bold);
                    return;
                }
                if (((Intlk_8)tag).IntlkOK != true)
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
            }
        }

        void TagsDataView_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow dataGridViewRow1 = TagsDataView.Rows[e.RowIndex];
                e.ToolTipText = $"{dataGridViewRow1.DataBoundItem}";
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


        private void quitToolStripMenuItem_Click(object sender, EventArgs e) { Application.Exit(); }



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
            }
            else
            {
                statusStrip1.BackColor = Control.DefaultBackColor;
                splitContainer1.Panel2Collapsed = true;
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
            if (PLC.AllTags.Count == 0) return;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Title = "Select C&E File",
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                FileName = $"{PLC.Name}_CnE_Report.xlsx",
                RestoreDirectory = true,
                FilterIndex = 1

            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream myStream = saveFileDialog1.OpenFile();
                if (myStream == null) return;
                
                CnE_Report.CreateReport(PLC, myStream);
                myStream.Close();

                Process.Start(new ProcessStartInfo
                {
                    FileName = saveFileDialog1.FileName,
                    UseShellExecute = true
                });

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
                    
                    XmlNodeList? controllerNodes = XmlDoc?.SelectNodes("/RSLogix5000Content/Controller");
                    XmlNode? ControllerNode = (controllerNodes != null && controllerNodes.Count > 0) ? controllerNodes[0] : null;
                    if (ControllerNode == null)
                    {
                        throw new InvalidOperationException("Controller node not found in the L5X file.");
                    }
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
            Form AboutBox = new frmAbout();
            AboutBox.ShowDialog();
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
            return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
        }

        private void aiReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Reporting.CreateIOReport();
        }

        private void inUseSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int aiCount = 0;
            int aoCount = 0;
            int diCount = 0;
            int doCount = 0;
            int totalCount = 0;

            foreach (XTO_AOI tag in PLC.AOI_Tags.Where(t => !t.NotInUse))
            {
                switch (tag.DataType)
                {
                    case "AIData":
                        aiCount++;
                        break;
                    case "AOData":
                        aoCount++;
                        break;
                    case "DIData":
                        diCount++;
                        break;
                    case "DOData":
                        doCount++;
                        break;
                    default:
                        break;
                }
            }
            totalCount = aiCount + aoCount + diCount + doCount;
            MessageBox.Show($"In Use Summary:\n\nAI Tags: {aiCount}\nAO Tags: {aoCount}\nDI Tags: {diCount}\nDO Tags: {doCount}\n\nTotal: {totalCount}", "In Use Tag Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void inUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter(ref PLC.Filter_InUse, ref inUseToolStripMenuItem);
        }

        private void placeholderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter(ref PLC.Filter_Placeholder, ref placeholderToolStripMenuItem);
        }

        private void simmedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter(ref PLC.Filter_Simmed, ref simmedToolStripMenuItem);
        }

        private void bypassedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter(ref PLC.Filter_Bypassed, ref bypassedToolStripMenuItem);
        }

        private void alarmedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter(ref PLC.Filter_Alarmed, ref alarmedToolStripMenuItem);
        }

        /// <summary>
        /// Toggles the filter state and updates the checked status of the specified menu item to reflect the new filter
        /// state.
        /// </summary>
        /// <remarks>This method also updates the data source of the TagsDataView to reflect the current
        /// filter state. The caller should ensure that both parameters are valid references before calling this
        /// method.</remarks>
        /// <param name="filter_">A reference to a Boolean value indicating the current filter state. The value is toggled by this method.</param>
        /// <param name="toolStripMenuItem">A reference to the ToolStripMenuItem whose Checked property is updated to match the new filter state.</param>
        private void filter(ref bool filter_, ref ToolStripMenuItem toolStripMenuItem)
        {
            filter_ = !filter_;
            toolStripMenuItem.Checked = filter_;
            TagsDataView.DataSource = PLC.AOI_Tags;
        }
    }

}

