using CnE2PLC.Properties;
using libplctag;
using libplctag.DataTypes;
using Microsoft.Office.Interop.Excel;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;
using static CnE2PLC.PLCTag;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{
    public class Controller
    {
        public Controller() { }
        public Controller(XmlNode node)
        {
            try
            {
                int n;
                DateTime t;
                XmlNode? s;

                s = node.Attributes.GetNamedItem("Use");
                if (s != null) Use = s.InnerText;

                s = node.Attributes.GetNamedItem("Name");
                if (s != null) Name = s.InnerText;

                s = node.Attributes.GetNamedItem("ProcessorType");
                if (s != null) ProcessorType = s.InnerText;

                s = node.Attributes.GetNamedItem("MajorRev");
                if (s != null)
                {
                    int.TryParse(s.InnerText, out n);
                    MajorRev = n;
                }

                s = node.Attributes.GetNamedItem("MinorRev");
                if (s != null)
                {
                    int.TryParse(s.InnerText, out n);
                    MinorRev = n;
                }

                s = node.Attributes.GetNamedItem("TimeSlice");
                if (s != null)
                {
                    int.TryParse(s.InnerText, out n);
                    TimeSlice = n;
                }

                s = node.Attributes.GetNamedItem("ShareUnusedTimeSlice");
                if (s != null)
                {
                    int.TryParse(s.InnerText, out n);
                    ShareUnusedTimeSlice = n;
                }

                s = node.Attributes.GetNamedItem("ProjectCreationDate");
                if (s != null) ProjectCreationDate = ParseABTime(s.InnerText);

                s = node.Attributes.GetNamedItem("LastModifiedDate");
                if (s != null) LastModifiedDate = ParseABTime(s.InnerText);

                s = node.Attributes.GetNamedItem("CommPath");
                if (s != null) CommPath = s.InnerText;

                // get global tags
                Tags = ProcessTags(node.SelectSingleNode("Tags"));
                // i may change the program to add all tags later, but for now i'm only doing the aoi's i need.

                // get all the task
                foreach (XmlNode p_node in node.SelectSingleNode("Tasks").ChildNodes) Tasks.Add(new Task(p_node));

                // get all the programs
                foreach (XmlNode p_node in node.SelectSingleNode("Programs").ChildNodes) Programs.Add(new PLC_Program(p_node));

                UpdateCounts();
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show(
                    $"Error: {ex.Message}",
                    "Contoller Import Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            [DebuggerStepThrough]
            DateTime ParseABTime(string input)
            {
                // Sat May 11 10:43:06 2024
                int n, year, month, day, hour, minute, second;
                string s;

                s = input.Substring(20, 4);
                int.TryParse(s, out n);
                year = n;

                s = input.Substring(8, 2);
                int.TryParse(s, out n);
                day = n;

                s = input.Substring(11, 2);
                int.TryParse(s, out n);
                hour = n;

                s = input.Substring(14, 2);
                int.TryParse(s, out n);
                minute = n;

                s = input.Substring(17, 2);
                int.TryParse(s, out n);
                second = n;

                s = input.Substring(4, 3);
                switch (s)
                {
                    case "Jan":
                        month = 1;
                        break;

                    case "Feb":
                        month = 2;
                        break;

                    case "Mar":
                        month = 3;
                        break;

                    case "Apr":
                        month = 4;
                        break;

                    case "May":
                        month = 5;
                        break;

                    case "Jun":
                        month = 6;
                        break;

                    case "Jul":
                        month = 7;
                        break;

                    case "Aug":
                        month = 8;
                        break;

                    case "Sep":
                        month = 9;
                        break;

                    case "Oct":
                        month = 10;
                        break;

                    case "Nov":
                        month = 11;
                        break;

                    case "Dec":
                        month = 12;
                        break;

                    default:
                        month = 0;
                        break;
                }

                return new DateTime(year, month, day, hour, minute, second);
            }
        }

        #region L5X Properties
        public string? Use { get; set; }
        public string? Name { get; set; }
        public string? ProcessorType { get; set; }
        public string? CommPath { get; set; }
        public int? MajorRev { get; set; }
        public int? MinorRev { get; set; }
        public int? TimeSlice { get; set; }
        public int? ShareUnusedTimeSlice { get; set; }
        public DateTime? ProjectCreationDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? Description { get; set; }

        #endregion
        public static string ControllerScopeName = "Controller Scope";

        private List<PLCTag> Tags { get; set; } = new();

        public List<Task> Tasks { get; set; } = new();

        public List<PLC_Program> Programs { get; set; } = new();

        public BindingList<PLCTag> AllTags
        {
            get {
                BindingList<PLCTag> list = new();
                foreach (PLCTag tag in Tags) list.Add(tag);
                foreach (PLC_Program program in Programs) foreach (PLCTag tag in program.LocalTags) list.Add(tag);
                return list;
            }
        }

        public BindingList <XTO_AOI> AOI_Tags
        {
            get
            {
                BindingList<XTO_AOI> list = new();

                foreach (XTO_AOI tag in Tags)
                {
                    if ( Filter_Alarmed & !tag.Alarmed ) continue;
                    if ( Filter_Bypassed & !tag.Bypassed ) continue;
                    if ( Filter_Simmed & !tag.Simmed ) continue;
                    if ( Filter_Placeholder & tag.Placeholder ) continue;
                    if ( Filter_InUse & (tag.NotInUse | !tag.AOICalled) ) continue;
                    list.Add(tag);
                }

                if (Filter_LocalTags) return list;

                foreach (PLC_Program program in Programs)
                {
                    foreach (XTO_AOI tag in program.LocalTags)
                    {
                        if (Filter_Alarmed & !tag.Alarmed) continue;
                        if (Filter_Bypassed & !tag.Bypassed) continue;
                        if (Filter_Simmed & !tag.Simmed) continue;
                        if (Filter_Placeholder & tag.Placeholder) continue;
                        if (Filter_InUse & (tag.NotInUse | !tag.AOICalled)) continue;
                        list.Add(tag);
                    }
                }

                return list;
            }
        }

        public string Version { get { return $"{MajorRev}.{MinorRev}"; } }

        public override string ToString() { return $"{Name} - {Version} - {ProcessorType}"; }

        /// <summary>
        /// Takes an XML list of Tags and converts them to a list of the converted cleasses.
        /// </summary>
        /// <param name="XMLTags">XML node containing the tags.</param>
        /// <returns>List of converted tags.</returns>
        public static List<PLCTag> ProcessTags(XmlNode XMLTags)
        {
            List<PLCTag> Tags = new();
            try
            {
                foreach (XmlNode item in XMLTags.ChildNodes)
                {
                    try
                    {
                        // Handle Arrays here.
                        XmlNode n = item.Attributes.GetNamedItem("Dimensions");
                        if ( n != null )
                        {
                            int.TryParse(n.InnerText, out int Dimensions);
                            foreach (XmlNode node in item.ChildNodes) {
                                n = node.Attributes.GetNamedItem("Format");
                                if (n != null) if (n.InnerText != "Decorated") continue; // skip L5Ks

                                if (node.Name == "Data") {
                                    foreach (XmlNode node2 in node.ChildNodes) {
                                        if (node2.Name == "Array") {
                                            foreach (XmlNode node3 in node2.ChildNodes)
                                            {
                                                PLCTag? t = CreateTag(node.FirstChild);
                                                if (t != null) Tags.Add(t);
                                            }
                                        }

                                    }
                                }

                            }
                        }
                        else
                        {
                            PLCTag? t = CreateTag(item);

                            if (t != null)
                            {
                                Tags.Add(t);

                                if ( t.DataType == "TankData") {
                                    TankData data = (TankData)t;
                                    Tags.Add(data.Level);
                                    Tags.Add(data.LIT_O);
                                    Tags.Add(data.LIT_W);
                                }

                            }
                      
                        }

                    }
                    catch (Exception ex)
                    {
                        var r = MessageBox.Show(
                            $"Error Processing node.\nEroor: {ex.Message}\nName: {item.Name}\nNode:\n{item.InnerText}\n",
                            "Tag Import Exception",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }

                }

            }

            catch (Exception ex)
            {
                var r = MessageBox.Show(
                    $"Error: {ex.Message}\nFrom: {XMLTags}",
                    "Process Tags Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            // Apply sort.
           
            Tags.Sort(CompareTAGS);


            return Tags;





        }

        private static int CompareTAGS(PLCTag? first, PLCTag? second)
        {
            if (first != null && second != null)
            {
                int r;
                if (first.GetType() != typeof(PLCTag) & second.GetType() != typeof(PLCTag))
                {
                    XTO_AOI t1 = (XTO_AOI)first;
                    XTO_AOI t2 = (XTO_AOI)second;
                    // Check EquipID first
                    r = t1.EquipNum.CompareTo(t2.EquipNum);
                    if (r != 0) return r;
                }

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



        /// <summary>
        /// Maps the Datatype of a node to one of the classes we are looking for.
        /// </summary>
        /// <param name="node">XML node to be checked.</param>
        /// <returns>Null if not a type we care about, or a converted tag.</returns>
        static public PLCTag? CreateTag(XmlNode node)
        {
            string Name, TagType, DataType;
            Name = string.Empty;
            DataType = string.Empty;
            TagType = string.Empty;
            int Dimensions, i;

            if (node.Attributes.Count != 0)
            {
                XmlNode n;

                n = node.Attributes.GetNamedItem("Name");
                if (n != null) Name = n.InnerText;
                if (n == null) return null;

                n = node.Attributes.GetNamedItem("TagType");
                if (n != null) TagType = n.InnerText;
                if (n == null) return null;

                n = node.Attributes.GetNamedItem("DataType");
                if (n != null) DataType = n.InnerText;
                if (n == null) return null;

                // handle differrent lenght strings
                if (DataType.ToLower().Contains("string")) DataType = "String";

                if (TagType == "Alias") DataType = TagType;

                n = node.Attributes.GetNamedItem("Dimensions");
                if (n != null)
                {
                    int.TryParse(n.InnerText, out i);
                    Dimensions = i;
                }

                n = node.Attributes.GetNamedItem("Index");
                if (n != null)
                {
                    Name += n.InnerText;
                }
            }

            switch (DataType)
            {

                // Base tags
                case "BOOL": return Settings.Default.BaseTypes ? new PLC_Bool(node) : null;
                case "REAL": return Settings.Default.BaseTypes ? new PLC_Real(node) : null;
                case "SINT": return Settings.Default.BaseTypes ? new PLC_Int(node) : null;
                case "INT": return Settings.Default.BaseTypes ? new PLC_Int(node) : null;
                case "DINT": return Settings.Default.BaseTypes ? new PLC_Int(node) : null;
                case "String": return Settings.Default.BaseTypes ? new ABString(node) : null;

                // Basic IO AIO
                case "AIData": return new AIData(node);
                case "AOData": return new AOData(node);
                case "DIData": return new DIData(node);
                case "DOData": return new DOData(node);

                // FIMS
                case "AIData_FIMS": return new AIData(node);
                case "DIData_FIMS": return new DIData(node);

                // Valves
                case "TwoPositionValveV2": return new TwoPositionValveV2(node);
                case "TwoPositionValve": return new TwoPositionValve(node);
                case "ValveAnalog": return new ValveAnalog(node);

                // Motors and Pumps
                case "Motor_VFD": return new Motor_VFD(node);
                case "PumpData": return new PumpData(node);
                case "PumpVData": return new PumpVData(node);

                // Interlocks
                case "Intlk_8": return new Intlk_8(node);
                case "Intlk_16": return new Intlk_16(node);
                case "Intlk_64V3": return new Intlk_64V3(node);
                case "Intlk_64V2": return new Intlk_64V2(node);
                case "Intlk_64": return new Intlk_64(node);
                /*    
                case "IntlkESD": return new IntlkESD(node);
                 */

                // TankData
                case "TankData": return new TankData(node);

                default: return null;
            }
        }

        public void UpdateCounts()
        {
            foreach (Task task in Tasks)
            {

            }


            // count the number of time a tag is used.
            foreach (XTO_AOI tag in AOI_Tags)
            {
                try
                {
                    tag.ClearCounts();
                    tag.IOs.Clear();

                    foreach (PLC_Program program in Programs.Where(p => !p.Disabled))
                    {
                        if (tag.Path != ControllerScopeName & tag.Path != program.Name) continue;

                        int c, r;
                        c = program.AOICount(tag.DataType, tag.Name);
                        r = program.TagCount($"{tag.Name}");
                        tag.AOICalls += c;
                        tag.References += r - c;

                        // record the io points found.
                        if (c != 0) tag.IOs.AddRange(program.GetIO(tag.DataType, tag.Name));

                        if (tag.DataType == "AIData" || tag.DataType == "AIData_FIMS")
                        {
                            AIData AI = (AIData)tag;
                            AI.HSD_Count += program.TagCount($"{tag.Name}.HSD");
                            AI.LSD_Count += program.TagCount($"{tag.Name}.LSD");
                            AI.HiHi_Count += program.TagCount($"{tag.Name}.HiHiAlarm");
                            AI.Hi_Count += program.TagCount($"{tag.Name}.HiAlarm");
                            AI.Lo_Count += program.TagCount($"{tag.Name}.LoAlarm");
                            AI.LoLo_Count += program.TagCount($"{tag.Name}.LoLoAlarm");
                            AI.PV_Count += program.TagCount($"{tag.Name}.PV");
                            AI.BadPV_Count += program.TagCount($"{tag.Name}.BadPVAlarm");
                            AI.Raw_Count += program.TagCount($"{tag.Name}.Raw");
                        }

                        if (tag.DataType == "DIData" || tag.DataType == "DIData_FIMS")
                        {
                            DIData DI = (DIData)tag;
                            DI.SD_Count += program.TagCount($"{tag.Name}.Shutdown");
                            DI.Val_Count += program.TagCount($"{tag.Name}.Value");
                            DI.Alm_Count += program.TagCount($"{tag.Name}.Alarm");
                            DI.Raw_Count += program.TagCount($"{tag.Name}.Raw");
                        }

                        if (tag.DataType == "TwoPositionValveV2" || tag.DataType == "TwoPositionValve")
                        {
                            TwoPositionValveV2 TPV2 = (TwoPositionValveV2)tag;
                            TPV2.Open_Count += program.TagCount($"{tag.Name}.Open");
                            TPV2.Close_Count += program.TagCount($"{tag.Name}.Close");
                            TPV2.FTO_Count += program.TagCount($"{tag.Name}.FailedToOpen");
                            TPV2.FTC_Count += program.TagCount($"{tag.Name}.FailedToClose");
                        }

                        if (tag.DataType == "ValveAnalog")
                        {
                            ValveAnalog AV = (ValveAnalog)tag;
                            AV.Pos_Count += program.TagCount($"{tag.Name}.Pos");
                            AV.PosFail_Count += program.TagCount($"{tag.Name}.PosFail");
                        }
                    }

                }
                catch (Exception ex)
                {
                    var r = MessageBox.Show(
                        $"Error: {ex.Message}\nFrom: {tag.Name}",
                        "Update Counts Exception",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

        }

        public bool Filter_InUse = false; // { get; set; }
        public bool Filter_Alarmed = false; // { get; set; }
        public bool Filter_Bypassed = false; // { get; set; }
        public bool Filter_Simmed = false; // { get; set; }
        public bool Filter_Placeholder = false; // { get; set; }
        public bool Filter_LocalTags = false; // { get; set; }

        #region Report Functions
        /// <summary>
        /// Exports the Tags to a Excel Templete.
        /// </summary>
        /// <param name="FileName">File name of the template.</param>
        public void CreateCnE(string FileName)
        {
            Excel.Application? excelApp = null;
            try
            {

                excelApp = new Excel.Application();


                if (Settings.Default.Debug)
                {
                    excelApp.Visible = true;
                    excelApp.ScreenUpdating = true;
                    //excelApp.Top = 0;
                    //excelApp.Left = 0;
                    //excelApp.Height = Screen.PrimaryScreen.WorkingArea.Height;
                    //excelApp.Width = Screen.PrimaryScreen.WorkingArea.Width;
                } else
                {
                    excelApp.Visible = false;
                    excelApp.ScreenUpdating = false;
                }

                Excel.Workbook? CnE_Workbook = null;
                Excel.Worksheet? CnE_Sheet = null;

                // open the file as readonly.
                excelApp.Workbooks.Open(FileName, false, true);
                CnE_Workbook = excelApp.ActiveWorkbook;
                CnE_Sheet = CnE_Workbook.ActiveSheet;
                Excel.Range range = CnE_Sheet.Cells;
                excelApp.Calculation = Excel.XlCalculation.xlCalculationManual;

                CnE_Sheet.Name = Name;

                int RowOffset = 17; // first row to use.
                int ColumnOffset = 19;

                foreach (XTO_AOI tag in Tags)
                {
                    try
                    {
                        //if (!tag.AOICalled) continue;
                        //if (tag.InUse != true) continue;

                        switch (tag.DataType.ToLower())
                        {
                            case "dodata":
                                DOData DoTag = (DOData)tag;
                                if (Settings.Default.Debug) CnE_Sheet.Cells[1, ColumnOffset].Activate();
                                InsertCol(CnE_Sheet, ColumnOffset);
                                DoTag.ToColumn(CnE_Sheet.Columns[ColumnOffset]);
                                Excel.Range r1 = CnE_Sheet.Range[CnE_Sheet.Cells[2, ColumnOffset], CnE_Sheet.Cells[12, ColumnOffset]];
                                MergeAndCenter(r1);
                                ColumnOffset++;
                                break;

                            case "aodata":
                                AOData AoTag = (AOData)tag;
                                if (Settings.Default.Debug) CnE_Sheet.Cells[1, ColumnOffset].Activate();
                                InsertCol(CnE_Sheet, ColumnOffset);
                                AoTag.ToColumn(CnE_Sheet.Columns[ColumnOffset]);
                                Excel.Range r2 = CnE_Sheet.Range[CnE_Sheet.Cells[2, ColumnOffset], CnE_Sheet.Cells[12, ColumnOffset]];
                                MergeAndCenter(r2);
                                ColumnOffset++;
                                break;

                            case "didata":
                            case "didata_fims":
                                DIData DiTag = (DIData)tag;
                                if (Settings.Default.Debug) CnE_Sheet.Cells[RowOffset, 1].Activate();
                                InsertRow(CnE_Sheet, RowOffset);
                                DiTag.ToValueRow(CnE_Sheet.Rows[RowOffset++]);

                                if (DiTag.AlmEnable == true)
                                {
                                    InsertRow(CnE_Sheet, RowOffset);
                                    DiTag.ToAlarmRow(CnE_Sheet.Rows[RowOffset++]);

                                    if (DiTag.SD_Count != 0)
                                    {
                                        InsertRow(CnE_Sheet, RowOffset);
                                        DiTag.ToShutdownRow(CnE_Sheet.Rows[RowOffset++]);
                                    }
                                }
                                break;

                            case "aidata":
                            case "aidata_fims":
                                AIData AiTag = (AIData)tag;
                                if (Settings.Default.Debug) CnE_Sheet.Cells[RowOffset, 1].Activate();
                                InsertRow(CnE_Sheet, RowOffset);
                                AiTag.ToPVRow(CnE_Sheet.Rows[RowOffset++]);

                                if (AiTag.HiHiEnable == true)
                                {
                                    if (AiTag.HSD_Count != 0)
                                    {
                                        InsertRow(CnE_Sheet, RowOffset);
                                        AiTag.ToHSDRow(CnE_Sheet.Rows[RowOffset++]);
                                    }

                                    InsertRow(CnE_Sheet, RowOffset);
                                    AiTag.ToHiHiAlarmRow(CnE_Sheet.Rows[RowOffset++]);
                                }
                                if (AiTag.HiEnable == true)
                                {
                                    InsertRow(CnE_Sheet, RowOffset);
                                    AiTag.ToHiAlarmRow(CnE_Sheet.Rows[RowOffset++]);
                                }
                                if (AiTag.LoEnable == true)
                                {
                                    InsertRow(CnE_Sheet, RowOffset);
                                    AiTag.ToLoAlarmRow(CnE_Sheet.Rows[RowOffset++]);
                                }
                                if (AiTag.LoLoEnable == true)
                                {
                                    InsertRow(CnE_Sheet, RowOffset);
                                    AiTag.ToLoLoAlarmRow(CnE_Sheet.Rows[RowOffset++]);

                                    if (AiTag.LSD_Count != 0)
                                    {
                                        InsertRow(CnE_Sheet, RowOffset);
                                        AiTag.ToLSDRow(CnE_Sheet.Rows[RowOffset++]);
                                    }

                                }
                                if (AiTag.BadPVEnable == true)
                                {
                                    InsertRow(CnE_Sheet, RowOffset);
                                    AiTag.ToBadPVAlarmRow(CnE_Sheet.Rows[RowOffset++]);
                                }
                                break;

                            case "twopositionvalve":
                            case "twopositionvalvev2":
                                TwoPositionValveV2 TPV2Tag = (TwoPositionValveV2)tag;
                                if (Settings.Default.Debug) CnE_Sheet.Cells[1, ColumnOffset].Activate();
                                InsertCol(CnE_Sheet, ColumnOffset);
                                TPV2Tag.ToColumn(CnE_Sheet.Columns[ColumnOffset]);
                                Excel.Range r3 = CnE_Sheet.Range[CnE_Sheet.Cells[2, ColumnOffset], CnE_Sheet.Cells[12, ColumnOffset]];
                                MergeAndCenter(r3);
                                ColumnOffset++;

                                if (TPV2Tag.DisableFB != true)
                                {
                                    if (Settings.Default.Debug) CnE_Sheet.Cells[RowOffset, 1].Activate();

                                    //if (TPV2Tag.Open_Count != 0)
                                    {
                                        InsertRow(CnE_Sheet, RowOffset);
                                        TPV2Tag.ToOpenRow(CnE_Sheet.Rows[RowOffset++]);
                                    }

                                    //if (TPV2Tag.Close_Count != 0)
                                    {
                                        InsertRow(CnE_Sheet, RowOffset);
                                        TPV2Tag.ToCloseRow(CnE_Sheet.Rows[RowOffset++]);
                                    }

                                    //if (TPV2Tag.FTO_Count != 0)
                                    {
                                        InsertRow(CnE_Sheet, RowOffset);
                                        TPV2Tag.ToFailedToOpenRow(CnE_Sheet.Rows[RowOffset++]);
                                    }

                                    //if (TPV2Tag.FTO_Count != 0)
                                    {
                                        InsertRow(CnE_Sheet, RowOffset);
                                        TPV2Tag.ToFailedToCloseRow(CnE_Sheet.Rows[RowOffset++]);
                                    }
                                }


                                break;

                            case "valveanalog":
                                ValveAnalog AAVTag = (ValveAnalog)tag;
                                CnE_Sheet.Columns[ColumnOffset].Insert();
                                AAVTag.ToColumn(CnE_Sheet.Columns[ColumnOffset]);
                                Excel.Range r4 = CnE_Sheet.Range[CnE_Sheet.Cells[2, ColumnOffset], CnE_Sheet.Cells[12, ColumnOffset]];
                                MergeAndCenter(r4);
                                ColumnOffset++;

                                if (AAVTag.DisableFB != true)
                                {
                                    //if (AAVTag.Pos_Count != 0)
                                    {
                                        CnE_Sheet.Rows[RowOffset].Insert();
                                        AAVTag.ToPosRow(CnE_Sheet.Rows[RowOffset++]);
                                    }
                                    //if (AAVTag.PosFail_Count != 0)
                                    {
                                        CnE_Sheet.Rows[RowOffset].Insert();
                                        AAVTag.ToPosFailRow(CnE_Sheet.Rows[RowOffset++]);
                                    }
                                }
                                break;

                            case "motor_vfd":
                                Motor_VFD motor_VFD = (Motor_VFD)tag;



                                break;


                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        var r = MessageBox.Show($"Error: {ex.Message}\nTage Name: {tag.Name} ", $"Tag Exception", MessageBoxButtons.OK);
                    }

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
                    //excelApp.Calculation = Excel.XlCalculation.xlCalculationAutomatic;
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

            static void InsertRow(Excel.Worksheet ws, int offset)
            {
                ws.Rows[offset].Insert();
                ws.Cells[offset, 3].Interior.Color = ws.Cells[16, 3].Interior.Color;
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
        }

        public void UpdateCnE(string FileName)
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
                                    DIData DiTag = (DIData)Tag;



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
                                    AIData AiTag = (AIData)Tag;

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


        public void CreateIOReport()
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


                Excel.Workbook Workbook = excelApp.Application.Workbooks.Add();
                Excel.Worksheet Sheet = Workbook.ActiveSheet;
                excelApp.Calculation = Excel.XlCalculation.xlCalculationManual;
                Sheet.Name = "AiData";
                CreateAiReport(Sheet);

                Sheet = Workbook.Sheets.Add();
                Sheet.Name = "DiData";
                Sheet.Activate();
                CreateDiReport(Sheet);

                Sheet = Workbook.Sheets.Add();
                Sheet.Name = "AoData";
                Sheet.Activate();
                CreateAoReport(Sheet);

                Sheet = Workbook.Sheets.Add();
                Sheet.Name = "DoData";
                Sheet.Activate();
                CreateDoReport(Sheet);

                Sheet = Workbook.Sheets.Add();
                Sheet.Name = "Intlk_8";
                Sheet.Activate();
                CreateIntlk8Report(Sheet);

                Sheet = Workbook.Sheets.Add();
                Sheet.Name = "Intlk_16";
                Sheet.Activate();
                CreateIntlk16Report(Sheet);

                excelApp.Visible = true;
                excelApp.ScreenUpdating = true;
                excelApp.Calculation = Excel.XlCalculation.xlCalculationAutomatic;

            }
            catch (Exception ex)
            {
                if (excelApp != null)
                {
                    excelApp.Visible = true;
                    excelApp.ScreenUpdating = true;
                    excelApp.Calculation = Excel.XlCalculation.xlCalculationAutomatic;
                }
                var result = MessageBox.Show(ex.Message, "Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                if (result == DialogResult.OK) return;
            }
        }


        public void CreateAiReport(Excel.Worksheet Sheet)
        {
            try
            {
                int RowOffset = 1; // first row to use.
                AIData.ToHeaderRow(Sheet.Rows[RowOffset++]);

                foreach (XTO_AOI tag in AllTags)
                {
                    try
                    {
                        if (tag.DataType != "AIData") continue;
                        AIData AI = (AIData)tag;
                        AI.ToDataRow(Sheet.Rows[RowOffset++]);
                    }
                    catch (Exception ex)
                    {
                        var r = MessageBox.Show($"Error: {ex.Message}\nTage Name: {tag.Name} ", $"Tag Exception", MessageBoxButtons.OK);
                    }

                }

            }
            catch (Exception e)
            {
                var result = MessageBox.Show(e.Message, "Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                if (result == DialogResult.OK) return;

            }
        }

        public void CreateDiReport(Excel.Worksheet Sheet)
        {
            try
            {
                int RowOffset = 1; // first row to use.
                DIData.ToHeaderRow(Sheet.Rows[RowOffset++]);

                foreach (XTO_AOI tag in AllTags)
                {
                    try
                    {
                        if (tag.DataType != "DIData") continue;
                        DIData DI = (DIData)tag;
                        DI.ToDataRow(Sheet.Rows[RowOffset++]);
                    }
                    catch (Exception ex)
                    {
                        var r = MessageBox.Show($"Error: {ex.Message}\nTage Name: {tag.Name} ", $"Tag Exception", MessageBoxButtons.OK);
                    }

                }

            }
            catch (Exception e)
            {
                var result = MessageBox.Show(e.Message, "Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                if (result == DialogResult.OK) return;

            }
        }

        public void CreateIntlk8Report(Excel.Worksheet Sheet)
        {
            try
            {
                int RowOffset = 1; // first row to use.
                Intlk_8.ToHeaderRow(Sheet.Rows[RowOffset++]);

                foreach (XTO_AOI tag in AllTags)
                {
                    try
                    {
                        if (tag.DataType != "Intlk_8") continue;
                        Intlk_8 I = (Intlk_8)tag;
                        I.ToDataRow(Sheet.Rows[RowOffset++]);
                    }
                    catch (Exception ex)
                    {
                        var r = MessageBox.Show($"Error: {ex.Message}\nTage Name: {tag.Name} ", $"Tag Exception", MessageBoxButtons.OK);
                    }

                }
            }
            catch (Exception e)
            {
                var result = MessageBox.Show(e.Message, "Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                if (result == DialogResult.OK) return;

            }

        }

        public void CreateIntlk16Report(Excel.Worksheet Sheet)
        {
            try
            {
                int RowOffset = 1; // first row to use.
                Intlk_16.ToHeaderRow(Sheet.Rows[RowOffset++]);

                foreach (XTO_AOI tag in AllTags)
                {
                    try
                    {
                        if (tag.DataType != "Intlk_16") continue;
                        Intlk_16 I = (Intlk_16)tag;
                        I.ToDataRow(Sheet.Rows[RowOffset++]);
                    }
                    catch (Exception ex)
                    {
                        var r = MessageBox.Show($"Error: {ex.Message}\nTage Name: {tag.Name} ", $"Tag Exception", MessageBoxButtons.OK);
                    }

                }
            }
            catch (Exception e)
            {
                var result = MessageBox.Show(e.Message, "Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                if (result == DialogResult.OK) return;

            }

        }

        public void CreateDoReport(Excel.Worksheet Sheet)
        {
            try
            {
                int RowOffset = 1; // first row to use.
                DOData.ToHeaderRow(Sheet.Rows[RowOffset++]);

                foreach (XTO_AOI tag in AllTags)
                {
                    try
                    {
                        if (tag.DataType != "DOData") continue;
                        DOData I = (DOData)tag;
                        I.ToDataRow(Sheet.Rows[RowOffset++]);
                    }
                    catch (Exception ex)
                    {
                        var r = MessageBox.Show($"Error: {ex.Message}\nTage Name: {tag.Name} ", $"Tag Exception", MessageBoxButtons.OK);
                    }

                }
            }
            catch (Exception e)
            {
                var result = MessageBox.Show(e.Message, "Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                if (result == DialogResult.OK) return;

            }

        }

        public void CreateAoReport(Excel.Worksheet Sheet)
        {
            try
            {
                int RowOffset = 1; // first row to use.
                AOData.ToHeaderRow(Sheet.Rows[RowOffset++]);

                foreach (XTO_AOI tag in AllTags)
                {
                    try
                    {
                        if (tag.DataType != "AOData") continue;
                        AOData I = (AOData)tag;
                        I.ToDataRow(Sheet.Rows[RowOffset++]);
                    }
                    catch (Exception ex)
                    {
                        var r = MessageBox.Show($"Error: {ex.Message}\nTage Name: {tag.Name} ", $"Tag Exception", MessageBoxButtons.OK);
                    }

                }
            }
            catch (Exception e)
            {
                var result = MessageBox.Show(e.Message, "Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                if (result == DialogResult.OK) return;

            }

        }

        #endregion

        #region Online Functions
        // ----------------- Online Functions --------------------------------
        private string ip = "192.168.1.10";
        private string path = "1,0";
        private PlcType plcType = PlcType.ControlLogix;
        private Protocol protocol = Protocol.ab_eip;
        private Tag<TagInfoPlcMapper, TagInfo[]>? onlinetags;
        private IEnumerable<int>? uniqueUdtTypeIds;
        private Dictionary<int, string>? udt_id_pairs;
        private bool connected = false;
        private static readonly ushort TYPE_IS_STRUCT = 0x8000;
        private static readonly ushort TYPE_IS_SYSTEM = 0x1000;
        private static readonly ushort TYPE_UDT_ID_MASK = 0x0FFF;

        static bool TagIsUdt(TagInfo tag) { return ((tag.Type & TYPE_IS_STRUCT) != 0) && !((tag.Type & TYPE_IS_SYSTEM) != 0); }

        static int GetUdtId(TagInfo tag) { return tag.Type & TYPE_UDT_ID_MASK; }

        static bool TagIsSystem(TagInfo tag) { return (tag.Type & TYPE_IS_SYSTEM) != 0; }

        static bool TagIsProgram(TagInfo tag, out string prefix)
        {
            if (tag.Name.StartsWith("Program:"))
            {
                prefix = tag.Name;
                return true;
            }
            else
            {
                prefix = string.Empty;
                return false;
            }
        }

        public string GetUDTName(int id) {
            string s;
            UDT_ID_Pairs.TryGetValue(id, out s);
            return s?? $"{id} Not Found.";
        }

        public string GetUDTName(TagInfo tag)
        {
            int id = GetUdtId(tag);
            return GetUDTName(id);
        }

        public int? GetUDTid(string name)
        {
            if (UDT_ID_Pairs.ContainsValue(name))
            {
                var key = UDT_ID_Pairs.Where(pair => pair.Value == name)
                    .Select(pair => pair.Key)
                    .FirstOrDefault();
                return key;
            }
            return null;
        }

        public Tag<TagInfoPlcMapper, TagInfo[]> OnlineTags
        {
            get
            {
                if (onlinetags == null)
                {
                    onlinetags = new Tag<TagInfoPlcMapper, TagInfo[]>()
                    {
                        Gateway = this.ip,
                        Path = this.path,
                        PlcType = this.plcType,
                        Protocol = this.protocol,
                        Name = "@tags"
                    };
                    onlinetags.Read();
                    if (onlinetags.GetStatus() != Status.Ok)
                    {
                        connected = false;
                        throw new Exception(($"PLC did not respond."));
                    }
                    connected = true;
                }

                Tag<TagInfoPlcMapper, TagInfo[]> returnvalue = new();
                foreach (var tag in onlinetags.Value)
                {
                    if (tag.Name.Contains("__DEFVAL_")) continue;
                    if (TagIsSystem(tag)) continue;
                    if (!TagIsUdt(tag)) continue;

                }

                return returnvalue;
            }
        }

        public IEnumerable<int> UniqueUdtTypeIds
        {
            get
            {
                if (uniqueUdtTypeIds == null)
                {
                    uniqueUdtTypeIds = OnlineTags.Value
                   .Where(tagInfo => TagIsUdt(tagInfo))
                   .Select(tagInfo => GetUdtId(tagInfo))
                   .Distinct();
                }
                return uniqueUdtTypeIds;
            }
        }

        public Dictionary<int, string> UDT_ID_Pairs
        {
            get
            {
                if (udt_id_pairs == null)
                {
                    udt_id_pairs = new Dictionary<int, string>();
                    foreach (var udtId in UniqueUdtTypeIds)
                    {
                        var udtTag = new Tag<UdtInfoPlcMapper, UdtInfo>()
                        {
                            Gateway = this.ip,
                            Path = this.path,
                            PlcType = this.plcType,
                            Protocol = this.protocol,
                            Name = $"@udt/{udtId}",
                        };

                        udtTag.Read();

                        if (udtTag.GetStatus() != Status.Ok)
                        {
                            connected = false;
                            throw new Exception(($"PLC did not respond."));
                        }

                        UdtInfo udt = udtTag.Value;
                        udt_id_pairs.Add(udtId, udt.Name);
                    }
                    connected = true;
                }
                return udt_id_pairs;
            }
        }

        private void ClearCaches()
        {
            connected = false;
            onlinetags = null;
            uniqueUdtTypeIds = null;
            udt_id_pairs = null;
        }

        public string PrintTags()
        {
            string s = "";
            UDT_ID_Pairs.Count();
            foreach (var f in OnlineTags.Value)
            {
                if(TagIsSystem(f)) continue;
                s += $"Name: {f.Name} Type: {f.Type}";
                if (TagIsUdt(f)) s += $" is UDT. ( {GetUdtId(f)} )";
                s += "\n";
            }
            return s;
        }

        public string PrintUdtTags()
        {
            string s = "";
            UDT_ID_Pairs.Count();
            foreach (var f in OnlineTags.Value)
            {
                if (f.Name.Contains("__DEFVAL_")) continue;
                if (TagIsSystem(f)) continue;
                if (!TagIsUdt(f)) continue;
                s += $"Name: {f.Name}  Type: {GetUDTName(f)}\n";
            }
            return s;
        }

        public string PrintUDTs()
        {
            string s = "";
            foreach (var udtId in UniqueUdtTypeIds)
            {
                try
                {
                    var udtTag = new Tag<UdtInfoPlcMapper, UdtInfo>()
                    {
                        Gateway = this.ip,
                        Path = this.path,
                        PlcType = this.plcType,
                        Protocol = this.protocol,
                        Name = $"@udt/{udtId}",
                    };
                    udtTag.Read();
                    UdtInfo udt = udtTag.Value;
                    s += $"UDT Id: {udt.Id} Name: {udt.Name} NumFields: {udt.NumFields} Size: {udt.Size}\n";
                    foreach (var f in udt.Fields) s += $"\tName: {f.Name} Offset: {f.Offset} Metadata: {f.Metadata} Type: {f.Type}\n";
                }
                catch (Exception e)
                {
                    s += ($"\n\tException: {e.Message}\n");
                }
            }
            return s;
        }

        #endregion

    }

    public class Task { 
        public Task() { }
        public Task(XmlNode node) 
        {
            try
            {
                int n;
                XmlNode? s;

                s = node.Attributes.GetNamedItem("Name");
                if (s != null) Name = s.InnerText;

                s = node.Attributes.GetNamedItem("Type");
                if (s != null) Type = s.InnerText;

                s = node.Attributes.GetNamedItem("Rate");
                if (s != null)
                {
                    int.TryParse(s.InnerText, out n);
                    Rate = n;
                }

                s = node.Attributes.GetNamedItem("Priority");
                if (s != null)
                {
                    int.TryParse(s.InnerText, out n);
                    Priority = n;
                }

                s = node.Attributes.GetNamedItem("Watchdog");
                if (s != null)
                {
                    int.TryParse(s.InnerText, out n);
                    Watchdog = n;
                }

                s = node.Attributes.GetNamedItem("Description");
                if (s != null) Description = s.InnerText;


                // get all the program names
                foreach (XmlNode p_node in node.SelectSingleNode("ScheduledPrograms").ChildNodes)
                {
                    XmlNode p = p_node.Attributes.GetNamedItem("Name");
                    if ( p != null ) ScheduledPrograms.Add(p.InnerText);
                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Error: {ex.Message}", "Contoller Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string? Name { get; set; }
        public string? Type { get; set; }
        public int? Rate { get; set; }
        public int? Priority { get; set; }
        public int? Watchdog { get; set; }
        public bool? DisableUpdateOutputs { get; set; }
        public bool? InhibitTask { get; set; }
        public string? Description { get; set; }
        public List<string> ScheduledPrograms { get; set; } = new();

        public override string ToString() { return $"{Name} {Description} Count:{ScheduledPrograms.Count}"; }

    }

}
