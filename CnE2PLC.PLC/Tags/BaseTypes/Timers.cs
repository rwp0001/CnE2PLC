using System.Runtime.InteropServices;

namespace CnE2PLC.PLC;

public class ABTimer : UDT
{
    private new TIMER _data;

    // Standard AB Timer Data Type
    public int PRE
    {
        get { return _data.PRE; }
        set { _data.PRE = value; }
    }

    public int ACC
    {
        get { return _data.ACC; }
        set { _data.ACC = value; }
    }

    public bool EN
    {
        //get { return (Data.Bools & 0b_0000_0001) ; }
        get;
        set;
    }

    public bool TT { get; set; }
    public bool DN { get; set; }

    private new int _size { get; } = 12;

}



// 12 bytes on PLC
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TIMER
{
    [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
    public Int32 PRE;
    [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
    public Int32 ACC;
    [MarshalAs(UnmanagedType.I1, SizeConst = 1)]
    public Int32 Bools;
    [MarshalAs(UnmanagedType.I1, SizeConst = 3)]
    private byte[] _padding;
}
