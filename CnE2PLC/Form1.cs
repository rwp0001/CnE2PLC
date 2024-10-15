using System.ComponentModel;
using System.Xml;
using CnE2PLC.Properties;
using System.Reflection;


namespace CnE2PLC
{

    public partial class Form1 : Form
    {
        List<PLC_Program> PLCPrograms;

        public Form1()
        {

            InitializeComponent();
            TagsDataView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.TagsDataView_CellFormatting);

            LogText.Text = string.Format("Startup at time: {0}\n", DateTime.Now);

            //TagsDataView.AutoGenerateColumns = true;
            TagsDataView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            if (Properties.Settings.Default.WindowPos != Point.Empty)
            {
                if (isPointVisibleOnAScreen(Properties.Settings.Default.WindowPos)) this.Location = Properties.Settings.Default.WindowPos;
                this.Location = Properties.Settings.Default.WindowPos;
            }
            if (Properties.Settings.Default.WindowSize != Size.Empty) this.Size = Properties.Settings.Default.WindowSize;

            PLCPrograms = new();

            if (Settings.Default.Debug) statusStrip1.BackColor = Color.Yellow;
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
                //CnE_Device.ProcessCnEFile(openFileDialog1.FileName, out CnE_Devices);
            }
        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {
            Settings.Default.Debug = !Settings.Default.Debug;
            statusStrip1.BackColor = Control.DefaultBackColor;
            if (Settings.Default.Debug) statusStrip1.BackColor = Color.Yellow;
        }

        private void LogText_TextChanged(object sender, EventArgs e)
        {

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

        }

        private void getTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void connectToPLCToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.WindowPos = this.Location;
            Properties.Settings.Default.WindowSize = this.Size;
            Properties.Settings.Default.Save();

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
                XTO_AOI.CreateCnE(openFileDialog1.FileName, (BindingList<XTO_AOI>)Tags_DGV_Source.List, PLCPrograms );
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
                    XmlNodeList XMLTags = XmlDoc.SelectNodes("/RSLogix5000Content/Controller/Tags")[0].ChildNodes;
                    XmlNodeList Programs = XmlDoc.SelectNodes("/RSLogix5000Content/Controller/Programs")[0].ChildNodes;
                    string Target = XmlDoc.SelectNodes("/RSLogix5000Content")[0].Attributes.GetNamedItem("TargetName").Value;
                    string Verison = XmlDoc.SelectNodes("/RSLogix5000Content")[0].Attributes.GetNamedItem("SoftwareRevision").Value;
                    this.Text = $"CnE2PLC - {Target} - {Verison}";

                    Tags_DGV_Source.Clear();
                    PLCPrograms = new();

                    foreach (XmlNode node in Programs) PLCPrograms.Add(new PLC_Program(node));
                    foreach (XTO_AOI tag in XTO_AOI.ProcessL5XTags(XMLTags)) Tags_DGV_Source.Add(tag);
                    foreach (XTO_AOI tag in Tags_DGV_Source) foreach (PLC_Program program in PLCPrograms)
                            {
                                int c, r;
                                c = program.TagCount($"{tag.AOI_Name}({tag.Name},");
                                r = program.TagCount($"{tag.Name}");
                                tag.AOICalls += c;
                                tag.References += r - c;


                            }

                    if (Settings.Default.Debug) foreach (PLC_Program program in PLCPrograms) foreach (Routine item in program.Routines) LogText.AppendText(item.ToText());
                    toolStripTagCount.Text = $"Tags: {Tags_DGV_Source.Count}";
                    Application.DoEvents();
                }
                catch (Exception ex)
                {
                    var r = MessageBox.Show($"Error: {ex.Message}", "Import L5X Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Application.DoEvents();
            }

        }

    }

}

