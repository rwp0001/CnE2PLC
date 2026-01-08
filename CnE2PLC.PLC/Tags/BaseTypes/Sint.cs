using CnE2PLC.PLC;
using libplctag.NativeImport;
using System.Xml;

namespace CnE2PLC.PLC;

public class SINT : PLCTag
{
    private new int _data;

    public SINT()
    {
        DataType = "SINT";
        TypeID = 0xc2;
    }

    public SINT(XmlNode node) : base(node)
    {
        DataType = "SINT";
        TypeID = 0xc2;
    }

    public int Value
    {
        get
        {
            if (Controller.Connected) Get();
            return field;
        }
        set
        {
            field = value;
            if (Controller.Connected) Set();
        }
    }

    public override void Get()
    {
        base.Get();
        _data = plctag.plc_tag_get_int8(_TagID, _offset);

    }

    public override void Set()
    {
        if ( _data > sbyte.MaxValue | _data < sbyte.MinValue ) throw new OverflowException();
        plctag.plc_tag_set_int8(_TagID, _offset, (sbyte)_data);
        base.Set();
    }

}
