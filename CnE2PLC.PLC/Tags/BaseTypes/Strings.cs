using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;

namespace CnE2PLC.PLC;

public class ABString : UDT
{
    private new STRING _data;

    public ABString()
    {
        DataType = "STRING";
        FamilyType = FamilyTypes.StringFamily;
    }

    public ABString(XmlNode node)
    {
        DataType = "STRING";
        FamilyType = FamilyTypes.StringFamily;
    }





}




public class String_20 : UDT
{

    private STRING_20 _data;

    [DebuggerStepThrough]
    public String_20() { }

    [DebuggerStepThrough]
    public String_20(string s)
    {
        Name = s;
    }
}








// 88 bytes on PLC
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class STRING
{
    [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
    public Int32 LEN;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 82)]
    public string DATA;
    [MarshalAs(UnmanagedType.I1, SizeConst = 2)]
    private byte[] _padding;
}

// 24 bytes on PLC
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class STRING_20
{
    [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
    public Int32 LEN;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
    public string DATA;
}




