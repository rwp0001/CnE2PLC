using CnE2PLC.Helpers;
using libplctag.NativeImport;
using System.Xml;

namespace CnE2PLC.PLC;

public class BOOL : PLCTag
{

    public BOOL()
    {
        DataType = "BOOL";
        TypeID = 0xc1;
        _data = new();
    }

    public BOOL(XmlNode node) : base(node)
    {
        DataType = "BOOL";
        TypeID = 0xc1;
        _data = new();
    }

    public bool Value 
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


    public override void Set()
    {
        plctag.plc_tag_set_bit(_TagID, _offset, Value ? 1 : 0 );
        base.Set();
    }

    public override void Get()
    {
        base.Get();
        Value = plctag.plc_tag_get_bit(_TagID, _offset) == 0 ? false : true;
    }

}