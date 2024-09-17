using libplctag.DataTypes;
using libplctag;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;
using System.IO;
using System.Xml;
using Windows.AI.MachineLearning;
using CnE2PLC.Properties;
using System.Xml.Linq;

namespace CnE2PLC
{

    public partial class Form1 : Form
    {
        PLC plc = new PLC();
        BindingList<CnE_Device> CnE_Devices = new BindingList<CnE_Device>();
        BindingList<PLCTag> CnE_Tags = new BindingList<PLCTag>();

        bool Debug = false;
        bool HideExcel = false;

        public Form1()
        {


            InitializeComponent();
            LogText.Text = string.Format("Startup at time: {0}\n", DateTime.Now);


            DevicesDataView.DataSource = CnE_Devices;
            DevicesDataView.AutoGenerateColumns = true;

            TagsDataView.DataSource = CnE_Tags;
            TagsDataView.AutoGenerateColumns = true;


            plc.Gateway = "192.168.250.250";

            if (Properties.Settings.Default.WindowPos != Point.Empty)
            {
                if (isPointVisibleOnAScreen(Properties.Settings.Default.WindowPos)) this.Location = Properties.Settings.Default.WindowPos;
                this.Location = Properties.Settings.Default.WindowPos;
            }
            if (Properties.Settings.Default.WindowSize != Size.Empty) this.Size = Properties.Settings.Default.WindowSize;

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

        private void SetupDevicesCol()
        {
            //CnE_Device device = new CnE_Device(); // used to populate colulms if needed.
            //DevicesDataView.ColumnCount = CnE_Device.Columns.Count;
            int idx = 0;

            // fix me
            //foreach (KeyValuePair<int, string> c in CnE_Device.Columns) DevicesDataView.Columns[idx++].Name = c.Value;
        }

        private void SetupDevicesTagCol()
        {
            int idx = 0;
            foreach (KeyValuePair<int, string> c in PLCTag.Columns) TagsDataView.Columns[idx++].Name = c.Value;
        }

        private void DeviceRowColoring()
        {
            DevicesDataView.SuspendLayout();

            foreach (DataGridViewRow row in DevicesDataView.Rows) DeviceCellFormatter(row);

            DevicesDataView.ResumeLayout();
        }

        private void DeviceCellFormatter(DataGridViewRow row)
        {
            Color BG = DevicesDataView.DefaultCellStyle.BackColor;
            Color FG = DevicesDataView.DefaultCellStyle.ForeColor;
            Font CellFont = DevicesDataView.DefaultCellStyle.Font;

            //bool EN = (bool)row.Cells["Enabled"].Value;
            //if (!EN)
            //{
            //    FG = Color.LightGray;
            //}

            string status = row.Cells["Status"].Value.ToString();

            if (status.ToLower().Contains("not in use"))
            {
                FG = Color.LightGray;
                BG = Color.DarkGray;
            }

            foreach (DataGridViewCell cell in row.Cells)
            {
                DataGridViewCellStyle style = cell.Style;
                style.BackColor = BG;
                style.ForeColor = FG;
                style.Font = Font;
            }

        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e) { System.Windows.Forms.Application.Exit(); }

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

            if (openFileDialog1.ShowDialog() == DialogResult.OK) ProcessL5K(openFileDialog1.FileName);

        }

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

            if (openFileDialog1.ShowDialog() == DialogResult.OK) ProcessCnEFile(openFileDialog1.FileName);
        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {
            Debug = !Debug;
            statusStrip1.BackColor = System.Windows.Forms.Control.DefaultBackColor;
            if (Debug) statusStrip1.BackColor = Color.Yellow;
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

        }

        private void DevicesDataView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DeviceCellFormatter(DevicesDataView.Rows[e.RowIndex]);
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
            LogText.AppendText(plc.PrintUDTs());
            toolStripUDTCount.Text = ($"UDTs: {plc.UDTCount}");
        }

        private void getTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogText.AppendText(plc.PrintTags());
            toolStripTagCount.Text = ($"Tags: {plc.TagCount}");
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

            if (openFileDialog1.ShowDialog() == DialogResult.OK) UpdateCnE(openFileDialog1.FileName);

        }

        #region XML Functions

        public void ProcessL5K(string FileName)
        {
            try
            {
                LogText.AppendText(string.Format("Opening File: {0}\n", FileName));
                toolStripProgressBar1.Value = 0;
                CnE_Tags = new BindingList<PLCTag>();
                int count = 0;

                TagsDataView.SuspendLayout();
                //TagsDataView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                //TagsDataView.RowHeadersVisible = false;


                string FileData = File.ReadAllText(FileName);
                XmlDocument XmlDocument = new XmlDocument();
                XmlDocument.LoadXml(FileData);
                XmlNodeList Tags = XmlDocument.SelectNodes("/RSLogix5000Content/Controller/Tags")[0].ChildNodes;

                string Name, TagType, DataType;



                foreach (XmlNode item in Tags)
                {
                    Name = string.Empty;
                    DataType = string.Empty;
                    TagType = string.Empty;

                    if (item.Attributes.Count != 0)
                    {
                        Name = item.Attributes.GetNamedItem("Name").Value;
                        TagType = item.Attributes.GetNamedItem("TagType").Value;
                        if (TagType == "Alias")
                        {
                            DataType = TagType;
                        }
                        else
                        {
                            DataType = item.Attributes.GetNamedItem("DataType").Value;
                        }

                        LogText.AppendText(string.Format("Found Tag: {0} : {1}\n", Name, DataType));
                    }

                    switch (DataType)
                    {
                        case "AIData":
                            AiData NewAiItem = new AiData(item);
                            CnE_Tags.Add(NewAiItem);
                            count++;
                            break;

                        case "AOData":
                            AoData NewAoItem = new AoData(item);
                            CnE_Tags.Add(NewAoItem);
                            count++;
                            break;

                        case "DIData":
                            DiData NewDiItem = new DiData(item);
                            CnE_Tags.Add(NewDiItem);
                            count++;
                            break;

                        case "DOData":
                            DoData NewDoItem = new DoData(item);
                            CnE_Tags.Add(NewDoItem);
                            count++;
                            break;

                        default:
                            break;
                    }
                    Application.DoEvents();
                }

                //TagsDataView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
                //TagsDataView.RowHeadersVisible = true;
                TagsDataView.DataSource = CnE_Tags;
                TagsDataView.ResumeLayout();
                TagsDataView.Refresh();
                LogText.AppendText(string.Format("Found {0} Tags.\n", count));
                toolStripTagCount.Text = ($"Tags: {CnE_Tags.Count}");
            }

            catch(Exception ex)
            {
                    LogText.AppendText($"\n\tException: {ex.Message}\n");
            }
        }


    #endregion


    #region Excel Functions

    public void UpdateCnE(string FileName)
        {
            Excel.Application? excelApp = null;

            try
            {
                LogText.AppendText(string.Format("Opening File: {0}\n", FileName));
                toolStripProgressBar1.Value = 0;

                excelApp = new Excel.Application();
                excelApp.Visible = !HideExcel;

                Excel.Workbook? CnE_Workbook = null;
                Excel.Worksheet? CnE_Sheet = null;

                // open the file as readonly.
                excelApp.Workbooks.Open(FileName, false, true);
                CnE_Workbook = excelApp.ActiveWorkbook;

                // select the sheet
                foreach (Excel.Worksheet ws in CnE_Workbook.Worksheets)
                {
                    try
                    {
                        LogText.AppendText(string.Format("Found Worksheet: {0}\n", ws.Name));

                        // Find the last real row
                        int lastUsedRow = ws.Cells.Find("*", System.Reflection.Missing.Value,
                                                       System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                                       Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious,
                                                       false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

                        // Find the last real column
                        int lastUsedColumn = ws.Cells.Find("*", System.Reflection.Missing.Value,
                                                       System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                                       Excel.XlSearchOrder.xlByColumns, Excel.XlSearchDirection.xlPrevious,
                                                       false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;

                        //search for the correct sheet
                        //if (lastUsedColumn != 23) continue;
                        //if (lastUsedRow < 15) continue;
                        //if (ws.Cells[1, 1].Value.ToString() != "COLOR LEGEND") continue;
                        //if (ws.Cells[1, 2].Value.ToString() != "CALLOUT CODES") continue;
                        //if (ws.Cells[1, 11].Value.ToString() != "C&E ACRONYM LEGEND") continue;
                        Excel.Range range = ws.Cells;

                        int count = 0;

                        for (int i = 16; i < lastUsedRow; i++)
                        {
                            // tage name is Col D -4
                            // full tagname is Col E - 5
                            // IO Details is Col F - 6
                            // setpoint is Col H - 8
                            // alarm on time is Col J - 10
                            // alarm off time is Col K - 11
                            // Auto Ack is Col P - 16
                            // Status / Not in use is Col G - 7

                            toolStripProgressBar1.Value = (int)(((double)i / (double)lastUsedRow) * 100);
                            Excel.Range row = range.Rows[i];


                            // check for tag name
                            if (row.Cells[1, 5].Value == null) continue;

                            // get the tag name
                            string BaseTag = row.Cells[1, 5].Value;
                            string Element = string.Empty;
                            if (BaseTag.Contains(".")) Element = BaseTag.Split('.')[1];
                            BaseTag = BaseTag.Split('.')[0];

                            var Tag = CnE_Tags.SingleOrDefault(tag => tag.Name == BaseTag);
                            if (Tag == null) continue;

                            switch (Tag.DataType.ToLower())
                            {
                                case "didata":
                                    DiData DiTag = (DiData)Tag;

                                    if (DiTag.InUse == true) { row.Cells[1, 7].Value = "Standard IO"; } else { row.Cells[1, 7].Value = "Not In Use"; }

                                    break;

                                case "aidata":
                                    AiData AiTag = (AiData)Tag;

                                    if (Element == string.Empty) continue;

                                    row.Cells[1, 7].Value = "Not In Use";
                                    row.Cells[1, 16].Value = "";

                                    switch (Element.ToLower())
                                    {
                                        case "pv":
                                            if (AiTag.InUse == true) { row.Cells[1, 7].Value = "Standard IO"; }
                                            row.Cells[1, 6].Value = "Analog Input";
                                            break;

                                        case "hihialarm":
                                            if (AiTag.HiHiEnable == true) row.Cells[1, 7].Value = "Standard IO";
                                            row.Cells[1, 6].Value = "Soft IO";
                                            row.Cells[1, 8].Value = AiTag.HiHiSP;
                                            row.Cells[1, 10].Value = string.Format("{0} Sec.", AiTag.Cfg_HiHiOnTmr);
                                            row.Cells[1, 11].Value = string.Format("{0} Sec.", AiTag.Cfg_HiHiOffTmr);
                                            if (AiTag.HiHiAutoAck == true) row.Cells[1, 16].Value = "Y";
                                            break;

                                        case "hialarm":
                                            if (AiTag.HiEnable == true) row.Cells[1, 7].Value = "Standard IO";
                                            row.Cells[1, 6].Value = "Soft IO";
                                            row.Cells[1, 8].Value = AiTag.HiSP;
                                            row.Cells[1, 10].Value = string.Format("{0} Sec.", AiTag.Cfg_HiOnTmr);
                                            row.Cells[1, 11].Value = string.Format("{0} Sec.", AiTag.Cfg_HiOffTmr);
                                            if (AiTag.HiAutoAck == true) row.Cells[1, 16].Value = "Y";
                                            break;

                                        case "loalarm":
                                            if (AiTag.LoEnable == true) row.Cells[1, 7].Value = "Standard IO";
                                            row.Cells[1, 6].Value = "Soft IO";
                                            row.Cells[1, 8].Value = AiTag.LoSP;
                                            row.Cells[1, 10].Value = string.Format("{0} Sec.", AiTag.Cfg_LoOnTmr);
                                            row.Cells[1, 11].Value = string.Format("{0} Sec.", AiTag.Cfg_LoOffTmr);
                                            if (AiTag.LoAutoAck == true) row.Cells[1, 16].Value = "Y";
                                            break;

                                        case "loloalarm":
                                            if (AiTag.LoLoEnable == true) row.Cells[1, 7].Value = "Standard IO";
                                            row.Cells[1, 6].Value = "Soft IO";
                                            row.Cells[1, 8].Value = AiTag.LoLoSP;
                                            row.Cells[1, 10].Value = string.Format("{0} Sec.", AiTag.Cfg_LoLoOnTmr);
                                            row.Cells[1, 11].Value = string.Format("{0} Sec.", AiTag.Cfg_LoLoOffTmr);
                                            if (AiTag.LoLoAutoAck == true) row.Cells[1, 16].Value = "Y";
                                            break;

                                        case "badpvalarm":
                                            if (AiTag.BadPVEnable == true) row.Cells[1, 7].Value = "Standard IO";
                                            row.Cells[1, 6].Value = "Soft IO";
                                            if (AiTag.BadPVAutoAck == true) row.Cells[1, 16].Value = "Y";
                                            break;



                                        default:
                                            break;
                                    }

                                    break;

                                default:
                                    break;
                            }


                            count++;
                            Application.DoEvents();
                        }


                        LogText.AppendText(string.Format("Sheet Process Complete. Found {0} Tags.\n", count));
                    }
                    catch (Exception e)
                    {
                        LogText.AppendText(string.Format("Exception: {0}\n", e.Message));
                        LogText.AppendText(string.Format("Sheet Process Failed. Found {0} Total Devices.\n", CnE_Devices.Count()));
                        toolStripProgressBar1.Value = 0;
                    }

                }

                //excelApp.Quit();
                toolStripProgressBar1.Value = 0;
            }
            catch (Exception e)
            {
                LogText.AppendText(string.Format("Exception: {0}\n", e.Message));
                toolStripProgressBar1.Value = 0;
                if (excelApp != null) excelApp.Quit();
            }
        }

        public void CreateCnE()
        {

        }

        private void ProcessCnEFile(string FileName)
        {
            Excel.Application? excelApp = null;
            CnE_Devices = new BindingList<CnE_Device>();
            //SetupDevicesCol();
            DevicesDataView.SuspendLayout();

            try
            {
                LogText.AppendText(string.Format("Opening File: {0}\n", FileName));
                toolStripProgressBar1.Value = 0;

                excelApp = new Excel.Application();
                excelApp.Visible = !HideExcel;

                Excel.Workbook? CnE_Workbook = null;
                Excel.Worksheet? CnE_Sheet = null;

                // open the file as readonly.
                excelApp.Workbooks.Open(FileName, false, true);
                CnE_Workbook = excelApp.ActiveWorkbook;



                // select the sheet
                foreach (Excel.Worksheet ws in CnE_Workbook.Worksheets)
                {
                    try
                    {
                        LogText.AppendText(string.Format("Found Worksheet: {0}\n", ws.Name));

                        // Find the last real row
                        int lastUsedRow = ws.Cells.Find("*", System.Reflection.Missing.Value,
                                                       System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                                       Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious,
                                                       false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

                        // Find the last real column
                        int lastUsedColumn = ws.Cells.Find("*", System.Reflection.Missing.Value,
                                                       System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                                       Excel.XlSearchOrder.xlByColumns, Excel.XlSearchDirection.xlPrevious,
                                                       false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;

                        //search for the correct sheet
                        //if (lastUsedColumn != 23) continue;
                        if (lastUsedRow < 15) continue;
                        //if (ws.Cells[1, 1].Value.ToString() != "COLOR LEGEND") continue;
                        //if (ws.Cells[1, 2].Value.ToString() != "CALLOUT CODES") continue;
                        //if (ws.Cells[1, 11].Value.ToString() != "C&E ACRONYM LEGEND") continue;
                        Excel.Range range = ws.Cells;

                        int count = 0;

                        for (int i = 16; i < lastUsedRow; i++)
                        {
                            toolStripProgressBar1.Value = (int)(((double)i / (double)lastUsedRow) * 100);
                            Excel.Range row = range.Rows[i];
                            CnE_Device ThisRow = new CnE_Device(row);
                            if (ThisRow.PLC_Tag_Name == string.Empty) continue;
                            //if (!Debug & !ThisRow.Enabled) continue;
                            CnE_Devices.Add(ThisRow);
                            count++;
                            if (Debug) LogText.AppendText(string.Format("Found: {0}\n", ThisRow.ToString()));
                            toolStripDeviceCount.Text = string.Format("Devices: {0}", CnE_Devices.Count);
                            Application.DoEvents();
                        }

                        //DevicesDataView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
                        //DevicesDataView.RowHeadersVisible = true;
                        DevicesDataView.ResumeLayout();
                        DevicesDataView.Refresh();
                        LogText.AppendText(string.Format("Sheet Process Complete. Found {0} Devices.\n", count));
                    }
                    catch (Exception e)
                    {
                        LogText.AppendText(string.Format("Exception: {0}\n", e.Message));
                        LogText.AppendText(string.Format("Sheet Process Failed. Found {0} Total Devices.\n", CnE_Devices.Count()));
                        toolStripProgressBar1.Value = 0;
                    }

                }

                excelApp.Quit();

                DevicesDataView.DataSource = CnE_Devices;
                DevicesDataView.ResumeLayout();
                DevicesDataView.Refresh();


                LogText.AppendText(string.Format("File Process Complete. Found {0} Total Devices.\n", CnE_Devices.Count()));
                toolStripProgressBar1.Value = 0;
            }
            catch (Exception e)
            {
                LogText.AppendText(string.Format("Exception: {0}\n", e.Message));
                LogText.AppendText(string.Format("File Process Failed. Found {0} Total Devices.\n", CnE_Devices.Count()));
                toolStripProgressBar1.Value = 0;
                if (excelApp != null) excelApp.Quit();
            }

        }

        #endregion
    }

}

