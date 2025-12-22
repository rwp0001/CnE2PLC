using CnE2PLC.PLC.XTO;
using libplctag;
using libplctag.DataTypes;
using System.ComponentModel;
using System.Xml;
using CnE2PLC.Helpers;

namespace CnE2PLC.PLC;

public class Controller
{
    public Controller() { }
    public Controller(XmlNode node)
    {
        try
        {
            LogHelper.DebugPrint("Creating Controller");

            if (node == null) throw new ArgumentNullException(nameof(node));

            Use = node.GetNamedAttributeItemInnerText("Use");
            Name = node.GetNamedAttributeItemInnerText("Name");
            ProcessorType = node.GetNamedAttributeItemInnerText("ProcessorType");
            MajorRev = node.GetNamedAttributeItemInnerTextAsInt("MajorRev");
            MinorRev = node.GetNamedAttributeItemInnerTextAsInt("MinorRev");
            TimeSlice = node.GetNamedAttributeItemInnerTextAsInt("TimeSlice");
            ShareUnusedTimeSlice = node.GetNamedAttributeItemInnerTextAsInt("ShareUnusedTimeSlice");
            ProjectCreationDate = node.GetNameAttributeItemInnerTextAsDateTime("ProjectCreationDate");
            LastModifiedDate = node.GetNameAttributeItemInnerTextAsDateTime("LastModifiedDate");
            CommPath = node.GetNamedAttributeItemInnerText("CommPath");
            
            // get global tags
            Tags = ProcessTags(node?.SelectSingleNode("Tags") ?? XMLHelper.CreateGenericXmlNode());
            // i may change the program to add all tags later, but for now i'm only doing the aoi's i need.

            // get all the task
            XmlNode tasks = node?.SelectSingleNode("Tasks") ?? XMLHelper.CreateGenericXmlNode();
            foreach (XmlNode p_node in tasks.ChildNodes) Tasks.Add(new Task(p_node));

            // get all the programs
            XmlNode programs = node?.SelectSingleNode("Programs") ?? XMLHelper.CreateGenericXmlNode();
            foreach (XmlNode p_node in programs.ChildNodes) Programs.Add(new Program(p_node));

            UpdateCounts();
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"Controller Import Error: {ex.Message}");
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

    public List<Program> Programs { get; set; } = new();

    public BindingList<PLCTag> AllTags
    {
        get {
            BindingList<PLCTag> list = new();
            foreach (PLCTag tag in Tags) list.Add(tag);
            foreach (Program program in Programs) foreach (PLCTag tag in program.LocalTags) list.Add(tag);
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
                if (Filter_Alarmed & !tag.Alarmed) continue;
                if (Filter_Bypassed & !tag.Bypassed) continue;
                if (Filter_Simmed & !tag.Simmed) continue;
                if (Filter_Placeholder & tag.Placeholder) continue;
                if (Filter_InUse & (tag.NotInUse | !tag.AOICalled)) continue;
                list.Add(tag);
            }

            if (Filter_LocalTags) return list;

            foreach (Program program in Programs)
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
            if (XMLTags == null) XMLTags = XMLHelper.CreateGenericXmlNode();
            foreach (XmlNode item in XMLTags.ChildNodes)
            {
                try
                {
                    // Handle Arrays here.
                    XmlNode? n = item?.Attributes?.GetNamedItem("Dimensions");
                    if ( n != null )
                    {
                        int.TryParse(n.InnerText, out int Dimensions);
                        foreach (XmlNode node in n.ChildNodes) {
                            if (node.GetNamedAttributeItemInnerText("Format") != "Decorated") continue; // skip L5Ks

                            if (node.Name == "Data") {
                                foreach (XmlNode node2 in node.ChildNodes) {
                                    if (node2.Name == "Array") {
                                        foreach (XmlNode node3 in node2.ChildNodes)
                                        {
                                            XmlNode? fc = node3.FirstChild;
                                            if (fc != null)
                                            { 
                                                PLCTag? t = CreateTag(fc);
                                                if (t != null) Tags.Add(t);
                                            }
                                        }
                                    }

                                }
                            }

                        }
                    }
                    else
                    {
                        PLCTag? t = CreateTag(item ?? XMLHelper.CreateGenericXmlNode());

                        if (t != null)
                        {
                            Tags.Add(t);

                            if ( t.DataType == "TankData") {
                                TankData data = (TankData)t;
                                if (data.Level != null) Tags.Add(data.Level);
                                if (data.LIT_O != null) Tags.Add(data.LIT_O);
                                if (data.LIT_W != null) Tags.Add(data.LIT_W);
                            }

                        }
                  
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.DebugPrint($"Tag Import Exception: Error Processing node.\nEroor: {ex.Message}\nName: {item?.Name}\nNode:\n{item?.InnerText}\n");
                }

            }

        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"Process Tags Exception: Error: {ex.Message}\nFrom: {XMLTags}");
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
        //int Dimensions, i;

        if (node.Attributes?.Count != 0)
        {
            string it = String.Empty;
            it = node.GetNamedAttributeItemInnerText("Name");
            if (it.Length == 0) return null; // can't make a tag, abort
            Name = it;

            it = node.GetNamedAttributeItemInnerText("TagType");
            if (it.Length == 0) return null; // can't make a tag, abort
            TagType = it;
            
            it = node.GetNamedAttributeItemInnerText("DataType");
            if (it.Length == 0) return null; // can't make a tag, abort
            DataType = it;
            
            // handle differrent lenght strings
            if (DataType.ToLower().Contains("string")) DataType = "String";

            if (TagType == "Alias") DataType = TagType;

            //Dimensions = node.GetNamedAttributeItemInnerTextAsInt("Dimensions");

            Name += node.GetNamedAttributeItemInnerText("Index");
        }

        switch (DataType)
        {

            // Base tags
            //case "BOOL": return new PLC_Bool(node);
            //case "REAL": return new PLC_Real(node);
            //case "SINT": return new PLC_Int(node);
            //case "INT": return new PLC_Int(node);
            //case "DINT": return new PLC_Int(node);
            //case "String": return new ABString(node);

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

                foreach (Program program in Programs)
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
                LogHelper.DebugPrint($"Update Counts Exception: Error: {ex.Message}\nFrom: {tag.Name}");
            }
        }

    }

    public bool Filter_InUse = false; // { get; set; }
    public bool Filter_Alarmed = false; // { get; set; }
    public bool Filter_Bypassed = false; // { get; set; }
    public bool Filter_Simmed = false; // { get; set; }
    public bool Filter_Placeholder = false; // { get; set; }
    public bool Filter_LocalTags = false; // { get; set; }



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
        string? s;
        UDT_ID_Pairs.TryGetValue(id, out s);
        return s ?? $"{id} Not Found.";
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
                if (connected) LogHelper.DebugPrint("connected");
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

            Name = node.GetNamedAttributeItemInnerText("Name");
            Type = node.GetNamedAttributeItemInnerText("Type");
            Description = node.GetNamedAttributeItemInnerText("Description");

            int.TryParse(node.GetNamedAttributeItemInnerText("Rate"), out n);
            Rate = n;

            int.TryParse(node.GetNamedAttributeItemInnerText("Priority"), out n);
            Priority = n;
            
            int.TryParse(node.GetNamedAttributeItemInnerText("Watchdog"), out n);
            Watchdog = n;
            
            // get all the program names
            XmlNode sched = node.SelectSingleNode("ScheduledPrograms", XMLHelper.CreateGenericXmlNode());
            foreach (XmlNode p_node in sched.ChildNodes)
            {
                string it = p_node.GetNamedAttributeItemInnerText("Name");
                if ( it.Length > 0 ) ScheduledPrograms.Add(it);
            }
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"Contoller Import Error: {ex.Message}");
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
