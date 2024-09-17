using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CnE2PLC
{
    public class DoData : PLCTag
    {
        public DoData() { }
        public DoData(XmlNode node)
        {
            Import(node);
        }

        // 208 bytes on PLC
        static readonly int Length = 208;

        //Parameters
        public bool? EnableIn { get; set; }
        public bool? EnableOut { get; set; }
        public bool? Value { get; set; }
        public bool? Raw { get; set; }
        public bool? Sim { get; set; }
        public int? SimVal { get; set; }
        public bool? InUse { get; set; }

        //Local Tags

    }
}
