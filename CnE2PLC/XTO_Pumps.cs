using System.Xml;

namespace CnE2PLC
{
    public class Pump : XTO_AOI
    {
        public Pump() { }
        public Pump(XmlNode node) : base(node) { }

        public override void ClearCounts() { }
    }

    public class P_PF753 : Pump 
    {
        public P_PF753() { }
        public P_PF753(XmlNode node) : base(node) { }
    }

    public class P_PF755 : P_PF753
    {
        public P_PF755() { }
        public P_PF755(XmlNode node) : base(node) { }
    }
}
