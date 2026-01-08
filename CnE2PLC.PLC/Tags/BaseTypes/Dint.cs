using libplctag.NativeImport;
using System.Xml;

namespace CnE2PLC.PLC;

public class DINT : PLCTag
{
    private new int _data;

    public DINT()
    {
        DataType = "DINT";
        TypeID = 0xc4;
    }

    public DINT(XmlNode node) : base(node)
    {
        DataType = "DINT";
        TypeID = 0xc4;
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
        _data = plctag.plc_tag_get_int32(_TagID, _offset);

    }

    public override void Set()
    {
        if( _data > Int32.MaxValue | _data < Int32.MinValue) throw new OverflowException();
        plctag.plc_tag_set_int32(_TagID, _offset, _data);
        base.Set();
    }

}