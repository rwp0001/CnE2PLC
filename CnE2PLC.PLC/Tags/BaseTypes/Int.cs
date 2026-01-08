using CnE2PLC.PLC;
using libplctag.NativeImport;
using System.Xml;

namespace CnE2PLC.PLC;

public class INT : PLCTag
{
    private new int _data;

    public INT()
    {
        DataType = "INT";
        TypeID = 0xc3;
    }

    public INT(XmlNode node) : base(node)
    {
        DataType = "INT";
        TypeID = 0xc3;
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
        _data = plctag.plc_tag_get_int16(_TagID, _offset);

    }

    public override void Set()
    {
        if( _data > Int16.MaxValue | _data < Int16.MinValue) throw new OverflowException();
        plctag.plc_tag_set_int16(_TagID, _offset, (Int16)_data);
        base.Set();
    }



}
