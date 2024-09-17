using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CnE2PLC
{
    public class DiData : PLCTag
    {
        public DiData() { }

        public DiData(XmlNode node)
        {
            Import(node);
        }





        // ? bytes on PLC
        static readonly int Length = 0;

        //Parameters
        public bool? EnableIn { get; set; }
        public bool? EnableOut { get; set; }
        public bool? Raw { get; set; }
        public bool? Value { get; set; }
        public int? AlmState { get; set; }
        public bool? Sim { get; set; }
        public bool? SimValue { get; set; }
        public bool? BpyActive { get; set; }
        public bool? AlmEnable { get; set; }
        public bool? AlmAutoAck { get; set; }
        public bool? StartupDlyCond { get; set; }
        public float? Cfg_InpOnTmr { get; set; }
        public float? Cfg_InpOffTmr { get; set; }
        public int? Cfg_AlmDlyTmr { get; set; }
        public int? Cfg_AlmOnTmr { get; set; }
        public int? Cfg_AlmOffTmr { get; set; }
        public int? Cfg_SDDlyTmr { get; set; }
        public bool? Alarm { get; set; }
        public bool? Shutdown { get; set; }
        public bool? AlmAck { get; set; }
        public bool? AlmReset { get; set; }
        public bool? AckResetAll { get; set; }
        public int? AlmCode { get; set; }
        public bool? InUse { get; set; }


        //Local Tags

    }
}
