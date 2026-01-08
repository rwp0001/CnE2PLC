using libplctag.NativeImport;
using System.Runtime.InteropServices;
using System.Xml;

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

