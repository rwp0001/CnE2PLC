using CnE2PLC.PLC;
using libplctag.NativeImport;
using System.Xml;

namespace CnE2PLC.PLC;

public class REAL : PLCTag
{
    private new double _data;

    public REAL()
    {
        DataType = "REAL";
        TypeID = 0xca;
    }

    public REAL(XmlNode node) : base(node)
    {
        DataType = "REAL";
        TypeID = 0xca;
    }

    public override void Get()
    {
        base.Get();
        _data = plctag.plc_tag_get_float32(_TagID, _offset);

    }

    public override void Set()
    {
        if( _data > float.MaxValue | _data < float.MinValue ) throw new OverflowException();
        plctag.plc_tag_set_float32(_TagID, _offset, (float)_data);
        base.Set();
    }

}
