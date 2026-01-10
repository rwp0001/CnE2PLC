using libplctag.NativeImport;
using System.Runtime.InteropServices;
using System.Xml;
using CnE2PLC.Helpers;

namespace CnE2PLC.PLC;

public abstract class UDT : PLCTag
{
    public UDT(){}

    public UDT(XmlNode node) : base(node){}

    public FamilyTypes? FamilyType { get; set; }

    public override void Set()
    {
        if (_data == null) return;
        IntPtr ptr = IntPtr.Zero;
        int size = Marshal.SizeOf(_data);
        byte[] arr = new byte[size];
        ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(_data, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        plctag.plc_tag_set_raw_bytes(_TagID, 0,arr, size);
        base.Set();

    }

    public override void Get()
    {
        if (_data == null) return;
        base.Get();
        IntPtr ptr = IntPtr.Zero;
        int size = Marshal.SizeOf(_data);
        byte[] arr = new byte[size];
        plctag.plc_tag_get_raw_bytes(_TagID, 0, arr, size);
        ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(arr, 0, ptr, size);
        object? obj = Marshal.PtrToStructure(ptr, _data.GetType());
        if (obj is object safeStruct) _data = safeStruct;
        Marshal.FreeHGlobal(ptr);
    }

}

public class DataType
{
    public DataType(){ }

    public DataType(XmlNode node)
    {
        try
        {
            Name = node.GetNamedAttributeItemInnerText("Name");
            Description = node.GetNamedAttributeItemInnerText("Description");
            Family = Enum.TryParse(node.GetNamedAttributeItemInnerText("Family"), out FamilyTypes fam) ? fam : FamilyTypes.NoFamily;
            Class = Enum.TryParse(node.GetNamedAttributeItemInnerText("Class"), out ClassTypes cls) ? cls : ClassTypes.User;

            XmlNode Membersnode = node?.SelectSingleNode("Members") ?? XMLHelper.CreateGenericXmlNode();

            foreach ( XmlNode memberNode in Membersnode.ChildNodes )
            {
                Members.Add(new Member(memberNode));
            }
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: DataType: Import node {node.Name} failed with {ex.Message}");
        }
    }

    #region Parameters
    public string Name { get; set; } = string.Empty;
    public FamilyTypes Family { get; set; } = FamilyTypes.NoFamily;
    public ClassTypes Class { get; set; } = ClassTypes.User;
    public string Description { get; set; } = string.Empty;
    public List<Member> Members { get; set; } = new List<Member>();
    #endregion

    public override string ToString()
    {
        return $"Name: {Name} Description: {Description})";
    }
}

public class Member
{
    public Member(){ }

    public Member(XmlNode node)
    {
        try
        {
            Name = node.GetNamedAttributeItemInnerText("Name");
            DataType = node.GetNamedAttributeItemInnerText("DataType");
            Dimension = node.GetNamedAttributeItemInnerText("Dimension");
            Radix = Enum.TryParse(node.GetNamedAttributeItemInnerText("Radix"), out Radixes rdx) ? rdx : Radixes.NullType;
            Hidden = node.GetNamedAttributeItemInnerTextAsBool("Hidden") ?? false;
            ExternalAccess = Enum.TryParse(node.GetNamedAttributeItemInnerText("ExternalAccess"), out Accesses access) ? access : Accesses.ReadWrite;
            Target = node.GetNamedAttributeItemInnerText("Target");
            BitNumber = node.GetNamedAttributeItemInnerTextAsInt("BitNumber") ?? -1;
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: Member: Import node {node.Name} failed with {ex.Message}");
        }
    }

    #region Parameters
    public string Name { get; set; }= string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string Dimension { get; set; } = string.Empty;
    public Radixes Radix { get; set; } = Radixes.NullType;
    public bool Hidden { get; set; } = false;
    public Accesses ExternalAccess { get; set; } = Accesses.ReadWrite;
    public string Target { get; set; } = string.Empty;
    public int BitNumber { get; set; } = -1;
    #endregion

    public override string ToString()
    {
        return $"Name: {Name} DataType: {DataType}";
    }
}
