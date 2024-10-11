using CnE2PLC.Properties;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{
    /// <summary>
    /// Base class for a Logix Tag.
    /// </summary>
    public class PLCTag : INotifyPropertyChanged
    {
        public PLCTag() { }

        public PLCTag(XmlNode node)
        {
            try
            {
                Name = node.Attributes.GetNamedItem("Name").Value;
                DataType = node.Attributes.GetNamedItem("DataType").Value;

                foreach (XmlNode item in node.ChildNodes)
                {
                    if (item.Name == "Description")
                    {
                        Description = item.InnerText;
                        continue;
                    }

                    if (item.Name == "Data")
                    {
                        try
                        {


                            if (item.Attributes.GetNamedItem("Format").Value == "Decorated")
                            {
                                foreach (XmlNode DataValueMember in item.ChildNodes[0].ChildNodes)
                                {
                                    SetProperty(
                                        DataValueMember.Attributes.GetNamedItem("Name").Value.Trim(),
                                        DataValueMember.Attributes.GetNamedItem("DataType").Value.Trim(),
                                        DataValueMember.Attributes.GetNamedItem("Value").Value.Trim()
                                        );
                                }
                                continue;
                            }

                            if (item.Attributes.GetNamedItem("Format").Value == "L5K")
                            {
                                // need to add processing the L5K to get the local strings.
                                L5K = item.InnerText;
                                //DecodeL5K();
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            var r = MessageBox.Show($"Error: {ex.Message}\nNode: {node.Name}\n{item.InnerText}", "Import Child Node Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Error: {ex.Message}\nNode: {node.Name}", "Import Node Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        #region Public Properties
        /// <summary>
        /// Tag Name from PLC Program
        /// </summary>
        [DisplayName("Tag Name")]
        public string Name
        {
            get
            {
                return NameValue;
            }
            set
            {
                NameValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("Tag Scope")]
        public string Path
        {
            get
            {
                if(PathValue == string.Empty) return "Controller";
                return PathValue;
            }
            set
            {
                PathValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("Tag Data Type")]
        public string DataType
        {
            get
            {
                return DataTypeValue;
            }
            set
            {
                DataTypeValue = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// PLC Program Tag Comment 
        /// </summary>
        [DisplayName("Tag Description")]
        public string Description
        {
            get
            {
                return DescriptionValue;
            }
            set
            {
                value.ReplaceLineEndings(" ");
                value = value.Replace('\n', ' ');
                value = value.Replace('\r', ' ');
                value = value.Trim();
                if (value.Length > 1)
                {
                    DescriptionValue = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// How many time is the Tag used in the program.
        /// </summary>
        [DisplayName("References")]
        
        public int References { get; set; } = 0;

        public override string ToString() { return string.Format("{0} {1} {2}", Name, DataType, Description); }

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Private Values
        protected string NameValue = string.Empty;
        protected string DataTypeValue = string.Empty;
        protected string PathValue = string.Empty;
        protected string DescriptionValue = string.Empty;
        protected string L5K = string.Empty;
        #endregion

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Used to set the property of class without cast to the class.
        /// </summary>
        /// <param name="DLM_Name">Property Name</param>
        /// <param name="DLM_DataType">Property Data Type</param>
        /// <param name="DLM_Value">Property Value</param>
        /// <exception cref="Exception"></exception>
        protected void SetProperty(string DLM_Name, string DLM_DataType, string DLM_Value)
        {
            try
            {
                // Use reflection to set
                var prop = this.GetType().GetProperty(DLM_Name);
                if (prop == null) return;

                switch (DLM_DataType.ToLower())
                {
                    case "bool":
                        bool NewBool = false;
                        if (DLM_Value[0] == '1') NewBool = true;
                        prop.SetValue(this, NewBool);
                        break;

                    case "dint":
                        int NewInt32 = 0;
                        int.TryParse(DLM_Value, out NewInt32);
                        prop.SetValue(this, NewInt32);
                        break;

                    case "int":
                        int NewInt16 = 0;
                        int.TryParse(DLM_Value, out NewInt16);
                        prop.SetValue(this, NewInt16);
                        break;

                    case "real":
                        float NewFloat = 0;
                        float.TryParse(DLM_Value, out NewFloat);
                        prop.SetValue(this, NewFloat);
                        break;


                    default:
                        break;
                }

                return;
            }
            catch (Exception e)
            {
                string s = string.Format("Failed to set property {0} to value {1} with ex: {2}", DLM_Name, DLM_Value, e.Message);
                var r = MessageBox.Show(s, "Set Property Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

    }

    /// <summary>
    /// Base class for all XTO Add-on Instructions
    /// </summary>
    public class XTO_AOI : PLCTag
    {

        public XTO_AOI() { }
        public XTO_AOI(XmlNode node)
        {
            IOs.Add("Not Found");
            Import(node);
        }

        #region Public Properties

        /// <summary>
        /// Used to sort the devices
        /// </summary>
        [DisplayName("Equipment Number")]
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public int EquipNum
        {
            get
            {
                string returnvalue = string.Empty;
                bool found = false;
                foreach (char c in Name)
                {
                    if (!found & char.IsDigit(c)) found = true;

                    if (found & char.IsDigit(c)) returnvalue += c;

                    if (found & !char.IsDigit(c)) break;

                }
                int rv = 0;
                int.TryParse(returnvalue, out rv);
                if (rv < 20) rv = 0;
                return rv;
            }
        }



        public bool? EnableIn { get; set; }
        public bool? EnableOut { get; set; }
        public bool? InUse { get; set; }

        [DisplayName("Equipment ID")]
        public string? Cfg_EquipID { get; set; }
        public string? Cfg_Faceplate { get; set; }
        public string? Cfg_Detail { get; set; }

        [DisplayName("Connected IO")]
        public string IO
        {
            get
            {
                string s = string.Empty;
                foreach (string s2 in IOs) s += $"{s2} ";
                return s;
            }
        }

        /// <summary>
        /// Description from HMI faceplate data.
        /// </summary>
        [DisplayName("Equipment Description")]
        public string Cfg_EquipDesc
        {
            get
            {
                return Cfg_EquipDescValue;
            }
            set
            {
                value.ReplaceLineEndings(" ");
                value = value.Replace('\n', ' ');
                value = value.Replace('\r', ' ');
                value = value.Trim();
                if (value.Length > 1)
                {
                    Cfg_EquipDescValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public List<string> IOs { get; set; } = new();

        [DisplayName("AOI Called")]
        public bool AOICalled { get; set; } = false;

        public int AOICalls { get; set; } = 0;

        public static string AOI_Name = string.Empty;

        #endregion

        #region Private Values
        private string Cfg_EquipDescValue = string.Empty;
        protected List<string> L5K_strings = new List<string>();
        #endregion


        public static List<XTO_AOI> ProcessL5XTags(XmlNodeList XMLTags)
        {
            List<XTO_AOI> Tags = new List<XTO_AOI>();
            try
            {
                foreach (XmlNode item in XMLTags)
                {
                try
                {
                    string Name, TagType, DataType;
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

                        }

                        switch (DataType)
                        {
                            case "AIData":
                                Tags.Add(new AiData(item));
                                break;

                            case "AOData":
                                Tags.Add(new AoData(item));
                                break;

                            case "DIData":
                                Tags.Add(new DiData(item));
                                break;

                            case "DOData":
                                Tags.Add(new DoData(item));
                                break;

                            case "TwoPositionValveV2":
                                Tags.Add(new TwoPositionValveV2(item));
                                break;

                            case "TwoPositionValve":
                                Tags.Add(new TwoPositionValve(item));
                                break;

                            case "ValveAnalog":
                                Tags.Add(new ValveAnalog(item));
                                break;

                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        var r = MessageBox.Show($"Error Processing node.\nEroor: {ex.Message}\nName: {item.Name}\nNode:\n{item.InnerText}\n", "Tag Import Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

                Tags.Sort(new TagComparer());
                return Tags;
            }

            catch (Exception ex)
            {
                var r = MessageBox.Show($"Error: {ex.Message}", "Import L5X Tags Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return Tags;
            }
        }



        public void Import(XmlNode node)
        {
            try
            {
                Name = node.Attributes.GetNamedItem("Name").Value;
                DataType = node.Attributes.GetNamedItem("DataType").Value;

                foreach (XmlNode item in node.ChildNodes)
                {
                    if (item.Name == "Description")
                    {
                        Description = item.InnerText;
                        continue;
                    }

                    if (item.Name == "Data")
                    {
                        try
                        {


                            if (item.Attributes.GetNamedItem("Format").Value == "Decorated")
                            {
                                foreach (XmlNode DataValueMember in item.ChildNodes[0].ChildNodes)
                                {
                                    SetProperty(
                                        DataValueMember.Attributes.GetNamedItem("Name").Value.Trim(),
                                        DataValueMember.Attributes.GetNamedItem("DataType").Value.Trim(),
                                        DataValueMember.Attributes.GetNamedItem("Value").Value.Trim()
                                        );
                                }
                                continue;
                            }

                            if (item.Attributes.GetNamedItem("Format").Value == "L5K")
                            {
                                L5K = item.InnerText;
                                DecodeL5K();
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            var r = MessageBox.Show($"Error: {ex.Message}\nNode: {node.Name}\n{item.InnerText}", "Import Child Node Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Error: {ex.Message}\nNode: {node.Name}", "Import Node Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Finds strings inside of L5K and exports them to the L5K_strings array.
        /// </summary>
        private void DecodeL5K()
        {
            L5K_strings = new List<string>();
            try
            {
                foreach (string s1 in L5K.Split("["))
                {
                    if (!s1.Contains("'")) continue;
                    string L = s1.Split(",")[0];
                    string D = s1.Split(",")[1];
                    D = D.Substring(1, D.Length - 1);
                    D = D.Substring(0, D.Length - 2);
                    string O = string.Empty;
                    int len = 0;
                    int.TryParse(L, out len);
                    if (len != 0)
                    {
                        D = D.Replace("$00", "");
                        D = D.Replace("$24", "$");
                        D = D.Replace("$27", "'");
                        D = D.Replace("\t", " ");
                        D = D.Replace("\r", "");
                        D = D.Replace("\n", "");
                        D = D.Trim();
                        if (D.EndsWith('\'')) D = D.Substring(0, D.Length - 1);
                        O = D;
                    }
                    L5K_strings.Add(O);
                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Error: {ex.Message}\nNode: {this.Name}", "L5K Strings Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            int CodeToChar(string code)
            {
                int c = 0;
                int.TryParse(code, out c);
                if (c == 9) return '\t';
                if (c == 13) return ' ';
                if (c < 32) return '\0';
                if (c > 126) return '\0';
                char r = (char)c;
                return c;
            }

        }


        /// <summary>
        /// Exports the Tags to a Excel Templete.
        /// </summary>
        /// <param name="FileName">File name of the template.</param>
        /// <param name="Tags">The tage to export.</param>
        public static void CreateCnE(string FileName, BindingList<XTO_AOI> Tags, List<PLC_Program> Programs)
        {
            Excel.Application? excelApp = null;
            try
            {

                excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.ScreenUpdating = false;

                if (Settings.Default.Debug)
                {
                    excelApp.Visible = true;
                    excelApp.ScreenUpdating = true;
                    excelApp.Top = 0;
                    excelApp.Left = 0;
                    excelApp.Height = Screen.PrimaryScreen.WorkingArea.Height;
                    excelApp.Width = Screen.PrimaryScreen.WorkingArea.Width;
                }

                Excel.Workbook? CnE_Workbook = null;
                Excel.Worksheet? CnE_Sheet = null;

                // open the file as readonly.
                excelApp.Workbooks.Open(FileName, false, true);
                CnE_Workbook = excelApp.ActiveWorkbook;
                CnE_Sheet = CnE_Workbook.ActiveSheet;
                Excel.Range range = CnE_Sheet.Cells;
                excelApp.Calculation = Excel.XlCalculation.xlCalculationManual;

                int RowOffset = 17; // first row to use.
                int ColumnOffset = 19;

                foreach (PLCTag tag in Tags)
                {
                    try
                    {
                        switch (tag.DataType.ToLower())
                        {
                            case "dodata":
                                DoData DoTag = (DoData)tag;
                                if (Settings.Default.Debug) CnE_Sheet.Cells[1, ColumnOffset].Activate();
                                InsertCol(CnE_Sheet, ColumnOffset);
                                DoTag.ToColumn(CnE_Sheet.Columns[ColumnOffset]);
                                Excel.Range r1 = CnE_Sheet.Range[CnE_Sheet.Cells[2, ColumnOffset], CnE_Sheet.Cells[12, ColumnOffset]];
                                MergeAndCenter(r1);
                                ColumnOffset++;
                                break;

                            case "aodata":
                                AoData AoTag = (AoData)tag;
                                if (Settings.Default.Debug) CnE_Sheet.Cells[1, ColumnOffset].Activate();
                                InsertCol(CnE_Sheet, ColumnOffset);
                                AoTag.ToColumn(CnE_Sheet.Columns[ColumnOffset]);
                                Excel.Range r2 = CnE_Sheet.Range[CnE_Sheet.Cells[2, ColumnOffset], CnE_Sheet.Cells[12, ColumnOffset]];
                                MergeAndCenter(r2);
                                ColumnOffset++;
                                break;

                            case "didata":
                                DiData DiTag = (DiData)tag;
                                if (Settings.Default.Debug) CnE_Sheet.Cells[RowOffset, 1].Activate();
                                InsertRow(CnE_Sheet, RowOffset);
                                DiTag.ToValueRow(CnE_Sheet.Rows[RowOffset++], TagCount($"{tag.Name}.Value"));
                                InsertRow(CnE_Sheet, RowOffset);
                                DiTag.ToAlarmRow(CnE_Sheet.Rows[RowOffset++], TagCount($"{tag.Name}.Alarm"));
                                if (DiTag.Shutdown != null)
                                {
                                    InsertRow(CnE_Sheet, RowOffset);
                                    DiTag.ToShutdownRow(CnE_Sheet.Rows[RowOffset++], TagCount($"{tag.Name}.Shutdown"));
                                }
                                break;

                            case "aidata":
                                AiData AiTag = (AiData)tag;
                                if (Settings.Default.Debug) CnE_Sheet.Cells[RowOffset, 1].Activate();
                                InsertRow(CnE_Sheet, RowOffset);
                                AiTag.ToPVRow(CnE_Sheet.Rows[RowOffset++], TagCount($"{tag.Name}.Value"));
                                InsertRow(CnE_Sheet, RowOffset);
                                AiTag.ToHSDRow(CnE_Sheet.Rows[RowOffset++], TagCount($"{tag.Name}.HSD"));
                                InsertRow(CnE_Sheet, RowOffset);
                                AiTag.ToHiHiAlarmRow(CnE_Sheet.Rows[RowOffset++], TagCount($"{tag.Name}.HiHiAlarm"));
                                InsertRow(CnE_Sheet, RowOffset);
                                AiTag.ToHiAlarmRow(CnE_Sheet.Rows[RowOffset++], TagCount($"{tag.Name}.HiAlarm"));
                                InsertRow(CnE_Sheet, RowOffset);
                                AiTag.ToLoAlarmRow(CnE_Sheet.Rows[RowOffset++], TagCount($"{tag.Name}.LoAlarm"));
                                InsertRow(CnE_Sheet, RowOffset);
                                AiTag.ToLoLoAlarmRow(CnE_Sheet.Rows[RowOffset++], TagCount($"{tag.Name}.LoLoAlarm"));
                                InsertRow(CnE_Sheet, RowOffset);
                                AiTag.ToLSDRow(CnE_Sheet.Rows[RowOffset++], TagCount($"{tag.Name}.LSD"));
                                InsertRow(CnE_Sheet, RowOffset);
                                AiTag.ToBadPVAlarmRow(CnE_Sheet.Rows[RowOffset++], TagCount($"{tag.Name}.BadPVAlarm"));
                                break;

                            case "twopositionvalve":
                            case "twopositionvalvev2":
                                TwoPositionValveV2 TPV2Tag = (TwoPositionValveV2)tag;
                                if(Settings.Default.Debug) CnE_Sheet.Cells[RowOffset, 1].Activate();
                                InsertRow(CnE_Sheet, RowOffset);
                                TPV2Tag.ToOpenRow(CnE_Sheet.Rows[RowOffset++]);
                                InsertRow(CnE_Sheet, RowOffset);
                                TPV2Tag.ToCloseRow(CnE_Sheet.Rows[RowOffset++]);
                                InsertRow(CnE_Sheet, RowOffset);
                                TPV2Tag.ToFailedToOpenRow(CnE_Sheet.Rows[RowOffset++]);
                                InsertRow(CnE_Sheet, RowOffset);
                                TPV2Tag.ToFailedToCloseRow(CnE_Sheet.Rows[RowOffset++]);
                                if (Settings.Default.Debug) CnE_Sheet.Cells[1, ColumnOffset].Activate();
                                InsertCol(CnE_Sheet, ColumnOffset);
                                TPV2Tag.ToColumn(CnE_Sheet.Columns[ColumnOffset]);
                                Excel.Range r3 = CnE_Sheet.Range[CnE_Sheet.Cells[2, ColumnOffset], CnE_Sheet.Cells[12, ColumnOffset]];
                                MergeAndCenter(r3);
                                ColumnOffset++;
                                break;

                            case "valveanalog":
                                ValveAnalog AAVTag = (ValveAnalog)tag;
                                if(AAVTag.DisableFB != true)
                                {
                                    CnE_Sheet.Rows[RowOffset].Insert();
                                    AAVTag.ToPosRow(CnE_Sheet.Rows[RowOffset++]);
                                    CnE_Sheet.Rows[RowOffset].Insert();
                                    AAVTag.ToPosFailRow(CnE_Sheet.Rows[RowOffset++]);
                                }
                                CnE_Sheet.Columns[ColumnOffset].Insert();
                                AAVTag.ToColumn(CnE_Sheet.Columns[ColumnOffset]);
                                Excel.Range r4 = CnE_Sheet.Range[CnE_Sheet.Cells[2, ColumnOffset], CnE_Sheet.Cells[12, ColumnOffset]];
                                MergeAndCenter(r4);
                                ColumnOffset++;
                                break;


                            default:
                                break;
                        }
                        Application.DoEvents();
                    }
                    catch (Exception ex)
                    {
                        var r = MessageBox.Show($"Error: {ex.Message}\nTage Name: {tag.Name} ",$"Tag Exception",MessageBoxButtons.OK);
                    }
                    

                    Application.DoEvents();
                }

                // clean up for filtering.
                CnE_Sheet.Rows[16].Delete();
                CnE_Sheet.Columns[18].Delete();

                excelApp.Visible = true;
                excelApp.ScreenUpdating = true;
                excelApp.Calculation = Excel.XlCalculation.xlCalculationAutomatic;

            }
            catch (Exception e)
            {
                if (excelApp != null)
                {
                    excelApp.Visible = true;
                    excelApp.ScreenUpdating = true;
                    excelApp.Calculation = Excel.XlCalculation.xlCalculationAutomatic;
                }
                var result = MessageBox.Show(e.Message, "Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                if (result == DialogResult.OK) return;

            }


            /// <summary>
            /// Helper function to merge and format cells in Excel.
            /// </summary>
            /// <param name="MergeRange">The range to merge.</param>
            static void MergeAndCenter(Excel.Range MergeRange)
            {
                MergeRange.Select();
                MergeRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                MergeRange.VerticalAlignment = Excel.XlVAlign.xlVAlignBottom;
                MergeRange.WrapText = false;
                MergeRange.Orientation = 90;
                MergeRange.AddIndent = false;
                MergeRange.IndentLevel = 0;
                MergeRange.ShrinkToFit = false;
                MergeRange.ReadingOrder = (int)(Excel.Constants.xlContext);
                MergeRange.MergeCells = false;
                MergeRange.Merge(System.Type.Missing);
            }

            static void InsertRow(Excel.Worksheet ws,int offset)
            {
                ws.Rows[offset].Insert();
                ws.Cells[offset, 3].Interior.Color = ws.Cells[16,3].Interior.Color;
                ws.Cells[offset, 3].Font.Color = ws.Cells[16, 3].Font.Color;
                ws.Cells[offset, 4].Interior.Color = ws.Cells[16, 3].Interior.Color;
                ws.Cells[offset, 4].Font.Color = ws.Cells[16, 3].Font.Color;
            }

            static void InsertCol(Excel.Worksheet ws, int offset)
            {
                ws.Columns[offset].Insert();
                ws.Cells[14, offset].Interior.Color = ws.Cells[14, 18].Interior.Color;
                ws.Cells[14, offset].Font.Color = ws.Cells[14, 18].Font.Color;
            }

            int TagCount(string tag)
            {
                int r = 0;
                foreach (PLC_Program program in Programs)  r += program.TagCount(tag);
                return r;
            }

        }


        public static void UpdateCnE(string FileName, BindingList<PLCTag> Tags)
        {
            Excel.Application? excelApp = null;

            try
            {
                excelApp = new Excel.Application();
                excelApp.Visible = !Properties.Settings.Default.HideExcel;

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

                            // tage name is 3
                            // full tagname is 4
                            // IO Details is 5
                            // setpoint is 7
                            // alarm on time is 8
                            // alarm off time is 10
                            // Auto Ack is 15
                            // Status / Not in use is 6

                            Excel.Range row = range.Rows[i];
                            row.Cells[1, 4].Activate();

                            // check for tag name
                            if (row.Cells[1, 4].Value == null) continue;

                            // get the tag name
                            string BaseTag = row.Cells[1, 4].Value;
                            string Element = string.Empty;
                            if (BaseTag.Contains(".")) Element = BaseTag.Split('.')[1];
                            BaseTag = BaseTag.Split('.')[0];

                            var Tag = Tags.SingleOrDefault(tag => tag.Name == BaseTag);
                            if (Tag == null) continue;

                            switch (Tag.DataType.ToLower())
                            {
                                case "didata":
                                    if (Element == string.Empty) continue;
                                    DiData DiTag = (DiData)Tag;



                                    row.Cells[1, 6].Value = "Not In Use";
                                    row.Cells[1, 15].Value = "";

                                    switch (Element.ToLower())
                                    {
                                        case "valve":
                                            //if (DiTag.InUse == true) row.Cells[1, 6].Value = "Standard IO";
                                            DiTag.ToValueRow(row);
                                            break;

                                        case "alarm":
                                            //if (DiTag.InUse == true) row.Cells[1, 6].Value = "Soft IO";
                                            //row.Cells[1, 9].Value = string.Format("{0} Sec.", DiTag.Cfg_AlmOnTmr);
                                            //row.Cells[1, 10].Value = string.Format("{0} Sec.", DiTag.Cfg_AlmOffTmr);
                                            //if (DiTag.AlmAutoAck == true) row.Cells[1, 15].Value = "Y";
                                            DiTag.ToAlarmRow(row);
                                            break;

                                        default:
                                            break;
                                    }


                                    break;

                                case "aidata":
                                    if (Element == string.Empty) continue;
                                    AiData AiTag = (AiData)Tag;

                                    row.Cells[1, 6].Value = "Not In Use";
                                    row.Cells[1, 15].Value = "";

                                    switch (Element.ToLower())
                                    {
                                        case "pv":
                                            if (AiTag.InUse == true) { row.Cells[1, 6].Value = "Standard IO"; }
                                            row.Cells[1, 5].Value = "Analog Input";
                                            break;

                                        case "hihialarm":
                                            if (AiTag.HiHiEnable == true) row.Cells[1, 6].Value = "Standard IO";
                                            row.Cells[1, 5].Value = "Soft IO";
                                            row.Cells[1, 7].Value = AiTag.HiHiSP;
                                            row.Cells[1, 9].Value = string.Format("{0} Sec.", AiTag.Cfg_HiHiOnTmr);
                                            row.Cells[1, 10].Value = string.Format("{0} Sec.", AiTag.Cfg_HiHiOffTmr);
                                            if (AiTag.HiHiAutoAck == true) row.Cells[1, 15].Value = "Y";
                                            break;

                                        case "hialarm":
                                            if (AiTag.HiEnable == true) row.Cells[1, 6].Value = "Standard IO";
                                            row.Cells[1, 5].Value = "Soft IO";
                                            row.Cells[1, 7].Value = AiTag.HiSP;
                                            row.Cells[1, 9].Value = string.Format("{0} Sec.", AiTag.Cfg_HiOnTmr);
                                            row.Cells[1, 10].Value = string.Format("{0} Sec.", AiTag.Cfg_HiOffTmr);
                                            if (AiTag.HiAutoAck == true) row.Cells[1, 15].Value = "Y";
                                            break;

                                        case "loalarm":
                                            if (AiTag.LoEnable == true) row.Cells[1, 6].Value = "Standard IO";
                                            row.Cells[1, 5].Value = "Soft IO";
                                            row.Cells[1, 7].Value = AiTag.LoSP;
                                            row.Cells[1, 9].Value = string.Format("{0} Sec.", AiTag.Cfg_LoOnTmr);
                                            row.Cells[1, 10].Value = string.Format("{0} Sec.", AiTag.Cfg_LoOffTmr);
                                            if (AiTag.LoAutoAck == true) row.Cells[1, 15].Value = "Y";
                                            break;

                                        case "loloalarm":
                                            if (AiTag.LoLoEnable == true) row.Cells[1, 6].Value = "Standard IO";
                                            row.Cells[1, 5].Value = "Soft IO";
                                            row.Cells[1, 7].Value = AiTag.LoLoSP;
                                            row.Cells[1, 9].Value = string.Format("{0} Sec.", AiTag.Cfg_LoLoOnTmr);
                                            row.Cells[1, 10].Value = string.Format("{0} Sec.", AiTag.Cfg_LoLoOffTmr);
                                            if (AiTag.LoLoAutoAck == true) row.Cells[1, 15].Value = "Y";
                                            break;

                                        case "badpvalarm":
                                            if (AiTag.BadPVEnable == true) row.Cells[1, 6].Value = "Standard IO";
                                            row.Cells[1, 5].Value = "Soft IO";
                                            if (AiTag.BadPVAutoAck == true) row.Cells[1, 15].Value = "Y";
                                            break;



                                        default:
                                            break;
                                    }

                                    break;

                                default:
                                    break;
                            }


                            count++;
                            //Application.DoEvents();
                        }

                    }
                    catch (Exception e)
                    {

                    }

                }

                //excelApp.Quit();

            }
            catch (Exception e)
            {

            }
        }

        public class TagComparer : IComparer<XTO_AOI>
        {
            public int Compare(XTO_AOI first, XTO_AOI second)
            {
                if (first != null && second != null)
                {
                    // Check EquipID first
                    int r = first.EquipNum.CompareTo(second.EquipNum);
                    if (r != 0) return r;

                    // next check the scope
                    r = first.Path.CompareTo(second.Path);
                    if (r != 0) return r;

                    // check the name
                    return first.Name.CompareTo(second.Name);
                }

                if (first == null && second == null)
                {
                    // We can't compare any properties, so they are essentially equal.
                    return 0;
                }

                if (first != null)
                {
                    // Only the first instance is not null, so prefer that.
                    return -1;
                }

                // Only the second instance is not null, so prefer that.
                return 1;
            }
        }


    }









    public class ABTimer : PLCTag
    {
        // Standard AB Timer Data Type
        // 12 bytes on PLC
        public Int32 PRE;
        public Int32 ACC;
        public bool EN;
        public bool TT;
        public bool DN;

    }
    public class ABString : PLCTag
    {
        private int Max_Length = 82;
        public Int32 LEN = 0;
        public byte[] DATA;
        public string test;

        public ABString()
        {
            DATA = new byte[Max_Length];
        }
        public ABString(string Input, int Length = 82)
        {
            SetLength(Length);
            Set(Input);
        }
        public ABString(int Length)
        {
            SetLength(Length);
            DATA = new byte[Max_Length];
        }

        public void Set(string Input)
        {
            DATA = new byte[Max_Length];
            if (Input.Length > Max_Length) Input = Input.Substring(0, Max_Length);
            for (int i = 0; i < Input.Length; i++) DATA[i] = (byte)Input[i];
            LEN = Input.Length;
        }
        public void SetLength(int Input)
        {
            if (Input < 0) throw new Exception("Must be greater then zero.");
            string Old_Value = ToString();
            Max_Length = Input;
            Set(Old_Value);
        }

        public override string ToString()
        {
            string value = "";
            for (int i = 0; i < LEN; i++)
            {
                if (DATA[i] == 0) break;
                value.Append((char)DATA[i]);
            }
            return value;
        }

    }

    public class AlmData : PLCTag
    {
        // 80 bytes on PLC
        static readonly int Length = 80;

        //Parameters
        public bool? EnableIn { get; }
        public bool? EnableOut { get; }
        public bool? Enable { get; set; }
        public bool? Status { get; set; }
        public bool? Active { get; set; }
        public bool? AutoAck { get; set; }
        public bool? Ack { get; set; }
        public bool? Alm { get; set; }
        public bool? SD { get; set; }
        public bool? AckCmd { get; set; }
        public bool? ResetCmd { get; set; }
        public bool? StartupDlyCond { get; set; }
        public Int32? Cfg_DlyTmr { get; set; }
        public Int32? Cfg_OnTmr { get; set; }
        public Int32? Cfg_OffTmr { get; set; }
        public Int32? Cfg_SDTmr { get; set; }

        // Local Tags
        public ABTimer? AckTmr { get; set; }
        public ABTimer? DlyTmr { get; set; }
        public ABTimer? OffTmr { get; set; }
        public ABTimer? OnTmr { get; set; }
        public ABTimer? SDTmr { get; set; }
    }

    public class BypData : PLCTag
    {
        // 24 bytes on PLC
        static readonly int Length = 24;

        //Parameters
        public bool? EnableIn { get; }
        public bool? EnableOut { get; }
        public bool? TimedReq { get; set; }
        public bool? SupvReq { get; set; }
        public bool? StopReq { get; set; }
        public bool? Cmd { get; set; }
        public bool? Active { get; set; }
        public Int32? Tmr_PRE { get; set; }
        public Int32? Tmr_ACC { get; set; }


        // Local Tags
        public ABTimer? Tmr { get; set; }
    }


    




}