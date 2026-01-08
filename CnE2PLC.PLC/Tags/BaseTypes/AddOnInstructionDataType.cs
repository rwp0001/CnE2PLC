using System.Xml;

namespace CnE2PLC.PLC;

public class AddOnInstructionDataType : PLCTag
{
    public AddOnInstructionDataType() { }
    public AddOnInstructionDataType(XmlNode node) : base(node) { }

    /// <summary>
    /// all AOIs have this bit.
    /// </summary>
    public bool EnableIn { get; set; }

    /// <summary>
    /// all AOIs have this bit.
    /// </summary>
    public bool EnableOut { get; set; }


    public override void Set()
    {
        base.Set();
    }
    public override void Get()
    {
        base.Get();
    }
}