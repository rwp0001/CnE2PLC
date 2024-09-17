using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CnE2PLC
{
    public class AoData : PLCTag
    {
        public AoData() { }

        public AoData(XmlNode node)
        {
            Import(node);
        }




        // 296 bytes on PLC
        static readonly int Length = 296;

        // Parameters
        public bool? EnableIn { get; set; }
        public bool? EnableOut { get; set; }
        public float? CV { get; set; }
        public float? MinEU { get; set; }
        public float? MaxEU { get; set; }
        public float? MinRaw { get; set; }
        public float? MaxRaw { get; set; }
        public float? Raw { get; set; }
        public bool? Sim { get; set; }
        public float? SimCV { get; set; }
        public bool? Cfg_IncToClose { get; set; }
        public bool? InUse { get; set; }
        public bool? SimReset { get; set; }
        public bool? SimActive { get; set; }

        // Local Tags
        public string? Cfg_EquipDesc { get; set; }
        public string? Cfg_EquipID { get; set; }
        public string? Cfg_EU { get; set; }
        public bool? SIM_ONS { get; set; }

    }
}
