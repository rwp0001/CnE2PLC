using CnE2PLC.Helpers;
using libplctag.NativeImport;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;

namespace CnE2PLC.PLC;

/// <summary>
/// Base class for a Logix Tag.
/// </summary>
public abstract class PLCTag
{
    [DebuggerStepThrough]
    public PLCTag() { }

    /// <summary>
    /// Convert a XML base tag to a class. 
    /// </summary>
    /// <param name="node">Node to be processed.</param>
    ///[DebuggerStepThrough]
    public PLCTag(XmlNode node)
    {
        try
        {
            Name = node.GetNamedAttributeItemInnerText("Name");

            string it = node.GetNamedAttributeItemInnerText("TagType");
            TagType = node.GetNamedAttributeItemInnerText("TagType").ParseEnum<TagTypes>();
                        
            DataType = node.GetNamedAttributeItemInnerText("DataType");
            Constant = GetBool(node.GetNamedAttributeItemInnerText("Constant"));
            GetDimensions(node.GetNamedAttributeItemInnerText("Dimensions"));
            
            switch (node.GetNamedAttributeItemInnerText("ExternalAccess"))
            {
                case "Read/Write":
                    ExternalAccess = TagAccess.ReadWrite;
                    break;

                case "Read Only":
                    ExternalAccess = TagAccess.ReadOnly;
                    break;

                case "None":
                    ExternalAccess = TagAccess.None;
                    break;
            }

            Radix = node.GetNamedAttributeItemInnerText("Radix").ParseEnum<TagRadix>();
            
            GetDimensions(node.GetNamedAttributeItemInnerText("Dimensions"));
            Description = node.GetChildNodeInnerText("Description", true); // Tag Comments

            //foreach (XmlNode item in node.ChildNodes)
            //{
            //    if (item.Name == "Data")
            //    {
            //        try
            //        {

            //            if (item.GetNamedAttributeItemValue("Format") == "Decorated")
            //            {
            //                foreach (XmlNode DataValue in item.ChildNodes)
            //                {
            //                    SetProperty(
            //                        "Value",
            //                        DataValue.GetNamedAttributeItemValue("DataType").Trim(),
            //                        DataValue.GetNamedAttributeItemValue("Value").Trim()
            //                        );
            //                }
            //                continue;
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            LogHelper.DebugPrint($"ERROR: PLC_Base: Import Child Node {node.Name} Exception: {ex.Message}");
            //        }
            //    }
            //}




        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: PLCTag: Create Tag {node.Name} failed. - Exception: {ex.Message}");
        }
    }

    ~PLCTag() 
    {
        if (_TagID != 0)
        {
            // Destroy the tag
            plctag.plc_tag_destroy(_TagID);
        }
    }

    #region Public Properties
    /// <summary>
    /// Tag Name from PLC Program
    /// </summary>
    [DisplayName("Tag Name")]
    public string Name { get; set; } = "Name not set.";


    [DisplayName("Tag Scope")]
    public string Path
    {
        get
        {
            if (field == string.Empty) return Controller.ControllerScopeName;
            return field;
        }
        set
        {
            field = value;
        }
    }

    [DisplayName("Tag DataType")]
    public string DataType { get; set; } = "DataType not set.";

    /// <summary>
    /// PLC Program Tag Comment 
    /// </summary>
    [DisplayName("Tag Description")]
    public string Description
    {
        get
        {
            return field;
        }
        set
        {
            value = CleanString(value);
            if (value.Length > 1)
            {
                field = value;
            }
        }
    }

    public int[] Dimension { get; set; } = { 0, 0, 0 };

    /// <summary>
    /// Base or Alias
    /// </summary>
    public TagTypes? TagType { get; set; }

    public TagUsages? Usage { get; set; } 

    public TagRadix? Radix { get; set; }

    public TagAccess? ExternalAccess { get; set; }

    public TagClasses? TagClass { get; set; }

    

    /// <summary>
    /// Is this tag modifable?
    /// </summary>
    public bool? Constant { get; set; }

    /// <summary>
    /// Used to select the base tag and qualifier that the tag references.
    /// </summary>
    public string? AliasFor { get; set; }


    /// <summary>
    /// How many time is the Tag used in the program.
    /// </summary>
    [DisplayName("References")]
    public int References { get; set; } = 0;

    /// <summary>
    /// Used to store the Type ID from the online PLC.
    /// </summary>
    public static int? TypeID { get; set; } = 0;

    public override string ToString()
    {
        return  $"Name: {Name} DataType: {DataType} Description: {Description}";
    }

    /// <summary>
    /// Cleans strings for use for the program.
    /// </summary>
    /// <param name="input">string to be cleaned up.</param>
    /// <returns></returns>
    [DebuggerStepThrough]
    static string CleanString(string input)
    {
        input.ReplaceLineEndings(" ");
        input = input.Replace('\n', ' ');
        input = input.Replace('\r', ' ');
        input = input.Trim();
        return input;
    }

    void GetDimensions(string input)
    {
        string[] values = input.Split(' ');
        int index = 0;
        Dimension = new int[3];
        foreach (var value in values)
        {
            int d = 0;
            int.TryParse(value, out d);
            Dimension[index] = d;
            index++;
        }
    }

    bool GetBool(string input)
    {
        if (input.ToLower().Contains("true")) return true;
        return false;
    }

    // used on udts for bool packing
    public bool Hidden { get; set; } = false;

    /*
    DataType        DataType    
    Type            Code
    Name            (hex)   Description                         Range
    BOOL*           C1      Boolean                             0 = FALSE; 1 = TRUE
    SINT            C2      Short Integer                       -128 SINT 127
    INT             C3      Integer                             -32768 INT 32767
    DINT            C4      Double Integer                      -231 DINT(231 – 1)
    LINT            C5      Long Integer                        -263 LINT(263 – 1)
    USINT           C6      Unsigned Short Integer              0 USINT 255
    UINT            C7      Unsigned Integer                    0 UINT 65536
    UDINT           C8      Unsigned Double Integer             0 UDINT(232 – 1)
    ULINT           C9      Unsigned Long Integer               0 ULINT(264 – 1)
    REAL            CA      Single Precision Float              See IEEE 754
    LREAL           CB      Double Precision Float              See IEEE 754
    BYTE            D1      bit string – 8 bits                 N/A
    WORD            D2      bit string – 16 bits                N/A
    DWORD           D3      bit string – 32 bits                N/A
    LWORD           D4      bit string – 64 bits                N/A
    SHORT STRING    DA      { length, 1-byte characters[n]}     N/A
    * BOOL data type is defined by the CIP standard to be an 8-bit unsigned integer with enumeration of 0 for False and 1 for True.
    */




    #endregion

    #region Private Values



    #endregion

    #region Online 

    protected object _data { get; set; }

    private string ConnectionString
    {
        get
        {
            if (string.IsNullOrEmpty(field))
            {
                field = $"{Controller.connection_string}&elem_ssize={_size}&elem_count=1&&name={Name}";
            }
            return field;
        }
    }

    protected int _TagID
    {
        get
        {

            if (field != 0) return field;

            try
            {
                int timeOut = (int)Controller.Timeout.TotalMilliseconds;

                // Create the tag
                LogHelper.DebugPrint($"INFO: Connecting to tag: {Name}");
                field = plctag.plc_tag_create(ConnectionString, timeOut);
                int rc = plctag.plc_tag_status(_TagID);
                if (rc != 0) throw new Exception($"{plctag.plc_tag_decode_error(rc)}");
            }
            catch (Exception ex)
            {
                field = 0;
                string s = $"ERROR: Failed to crate tag {Name} with error: {ex.Message}";
                LogHelper.DebugPrint(s);
            }

            return field;

        }
    }
    protected int _size = 0;

    protected int _offset = 0;

    public int PackingBytes { get; set; } = 0;

    public virtual void Get(){
        // Read the tag to the PLC.
        LogHelper.DebugPrint($"INFO: Writing tag: {Name}");
        int timeOut = (int)Controller.Timeout.TotalMilliseconds;
        int rc = plctag.plc_tag_read(_TagID, timeOut);
        if (rc != 0) throw new Exception($"Failed to write tag {Name} with code {rc}: {plctag.plc_tag_decode_error(rc)}");
    }

    public virtual void Set()
    {
        // Write the tag to the PLC.
        LogHelper.DebugPrint($"INFO: Writing tag: {Name}");
        int timeOut = (int)Controller.Timeout.TotalMilliseconds;
        int rc = plctag.plc_tag_write(_TagID, timeOut);
        if (rc != 0) throw new Exception($"Failed to write tag {Name} with code {rc}: {plctag.plc_tag_decode_error(rc)}");
    }

        #endregion


    /// <summary>
    /// Used to set the property of class without cast to the class.
    /// </summary>
    /// <param name="DLM_Name">Property Name</param>
    /// <param name="DLM_DataType">Property Data Type</param>
    /// <param name="DLM_Value">Property Value</param>
    /// <exception cref="Exception"></exception>
    //[DebuggerStepThrough]
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

                case "sint":
                case "int":
                case "dint":
                    int NewInt = 0;
                    int.TryParse(DLM_Value, out NewInt);
                    prop.SetValue(this, NewInt);
                    break;

                case "lint":
                    long NewLong = 0;
                    long.TryParse(DLM_Value, out NewLong);
                    prop.SetValue(this, NewLong);
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
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: PLCTag: SetProperty: Failed to set property {DLM_Name} to value {DLM_Value} with ex: {ex.Message}");
        }


    }

    /// <summary>
    /// This class used to sort tags.
    /// </summary>
    [DebuggerStepThrough]
    public class TagComparer : IComparer<PLCTag>
    {
        public int Compare(PLCTag? first, PLCTag? second)
        {
            if (first != null && second != null)
            {
                int r;
                if (first.GetType() == typeof(XTO_AOI) & first.GetType() == typeof(XTO_AOI))
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
    }




}

#region Enums

/// <summary>
/// How is a tag used.
/// </summary>
public enum TagUsages
{
    Local,
    Input,
    Output,
    InOut,
    Public
}

/// <summary>
/// Types of tags.
/// </summary>
public enum TagTypes
{
    Base,
    Alias,
    Produced,
    Consumed
}

/// <summary>
/// How to display the data.
/// </summary>
public enum TagRadix
{
    Decimal,
    Float,
    Hex,
    ASCII,
    Octal,
    Exponential,
    DateTime,
    DateTimeNS
}

/// <summary>
/// Used to select the access allowed to the tag from external applications such as HMIs. 
/// </summary>
public enum TagAccess
{
    ReadWrite,
    ReadOnly,
    None
}

/// <summary>
/// Indicates whether the tag is a Standard tag or a Safety tag.
/// </summary>
public enum TagClasses
{
    Standard,
    Safety
}

public enum FamilyTypes
{
    NoFamily,
    StringFamily
}

#endregion