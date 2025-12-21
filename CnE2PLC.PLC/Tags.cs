using CnE2PLC.Helpers;
using CnE2PLC.PLC.XTO;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;

namespace CnE2PLC.PLC;

/// <summary>
/// Base class for a Logix Tag.
/// </summary>
public class PLCTag
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

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Import Node Exception: Error: {ex.Message}\nNode: {node.Name}");
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
            if (PathValue == string.Empty) return Controller.ControllerScopeName;
            return PathValue;
        }
        set
        {
            PathValue = value;
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
            return DescriptionValue;
        }
        set
        {
            value = CleanString(value);
            if (value.Length > 1)
            {
                DescriptionValue = value;
            }
        }
    }

    public int[]? Dimension { get; set; } = { 0, 0, 0 };

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
    static int? TypeID {  get; set; }

    public override string ToString()
    {
        string c = $"Name: {Name}\n";
        c += $"PLC Tag Description: {Description}\n";
        c += $"PLC DataType: {DataType}\n";
        return c;
    }

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

    #endregion

    #region Private Values
    protected string NameValue = string.Empty;
    protected string DataTypeValue = string.Empty;
    protected string PathValue = string.Empty;
    protected string DescriptionValue = string.Empty;
    #endregion

    /// <summary>
    /// Used to set the property of class without cast to the class.
    /// </summary>
    /// <param name="DLM_Name">Property Name</param>
    /// <param name="DLM_DataType">Property Data Type</param>
    /// <param name="DLM_Value">Property Value</param>
    /// <exception cref="Exception"></exception>
    [DebuggerStepThrough]
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
        catch (Exception e)
        {
            Debug.WriteLine($"Set Property Exception: Failed to set property {DLM_Name} to value {DLM_Value} with ex: {e.Message}");
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

public class PLC_Base : PLCTag
{
    public PLC_Base() { }
    public PLC_Base(XmlNode node) : base(node)
    {
        try
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == "Data")
                {
                    try
                    {

                        if (item.GetNamedAttributeItemValue("Format") == "Decorated")
                        {
                            foreach (XmlNode DataValue in item.ChildNodes)
                            {
                                SetProperty(
                                    "Value",
                                    DataValue.GetNamedAttributeItemValue("DataType").Trim(),
                                    DataValue.GetNamedAttributeItemValue("Value").Trim()
                                    );
                            }
                            continue;
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Import Child Node Exception: Error: {ex.Message}\nNode: {node.Name}\n{item.InnerText}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Import Child Node Exception: Error: {ex.Message}\nNode: {node.Name}");
        }
    }

    //[DebuggerStepThrough]
    //public override void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) { }
}

public class PLC_Bool : PLC_Base
{
    public PLC_Bool() { DataType = "BOOL"; }
    public PLC_Bool(XmlNode node) : base(node) { DataType = "BOOL"; }
    public bool? Value { get; set; }
    public override string ToString() { return $"{base.ToString()}Value: {Value}\n"; }
}
public class PLC_Int : PLC_Base
{
    public PLC_Int() { DataType = "INT"; }
    public PLC_Int(XmlNode node) : base(node) { DataType = "INT"; }
    public int? Value { get; set; }
    public override string ToString() { return $"{base.ToString()}Value: {Value}\n"; }
}
public class PLC_Real : PLC_Base
{
    public PLC_Real() { DataType = "REAL"; }
    public PLC_Real(XmlNode node) : base(node) { DataType = "REAL"; }
    public float? Value { get; set; }
    public override string ToString() { return $"{base.ToString()}Value: {Value}\n"; }
}
public class ABTimer : PLC_Base
{
    // Standard AB Timer Data Type
    // 12 bytes on PLC
    public Int32 PRE;
    public Int32 ACC;
    public bool EN;
    public bool TT;
    public bool DN;

}
public class ABString : PLC_Base
{
    private static int Max_Length = 82;
    public Int32 LEN = 0;
    public byte[] DATA;

    public ABString()
    {
        DataType = "STRING";
        DATA = new byte[Max_Length];
    }
    public ABString(string Input, int Length = 82)
    {
        DataType = "STRING";
        SetLength(Length);
        DATA = new byte[Max_Length];
        Set(Input);
    }
    public ABString(int Length)
    {
        DataType = "STRING";
        SetLength(Length);
        DATA = new byte[Max_Length];
    }
    public ABString(XmlNode node)
    {
        DataType = "STRING";
        DATA = new byte[Max_Length];
        throw new NotImplementedException();


    }

    public void Set(string Input)
    {
        DATA = new byte[Max_Length];
        if (Input.Length > Max_Length) Input = Input.Substring(0, Max_Length);
        for (int i = 0; i < Input.Length; i++) DATA[i] = (byte)Input[i];
        LEN = Input.Length;
        if (LEN > Max_Length) LEN = Max_Length;
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
