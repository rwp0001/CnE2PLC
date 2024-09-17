using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CnE2PLC
{
    public class AiData : PLCTag
    {
        public AiData() { }

        public AiData(XmlNode node) { 
            Import(node);
        }


        // 688 Bytes on PLC
        static readonly int Length = 688;
        
        public bool? EnableIn { get; set; }
        public bool? EnableOut { get; set; }
        public float? Raw { get; set; }
        public float? MinRaw { get; set; }
        public float? MaxRaw { get; set; }
        public float? MinEU { get; set; }
        public float? MaxEU { get; set; }
        public float? PV { get; set; }
        public float? PctPV { get; set; }
        public bool? UseSqRt { get; set; }
        public bool? Sim { get; set; }
        public float? SimPV { get; set; }
        public bool? BypActive { get; set; }
        public bool? BadPVEnable { get; set; }
        public bool? BadPVAutoAck { get; set; }
        public bool? HiHiEnable { get; set; }
        public bool? HiHiAutoAck { get; set; }
        public bool? HiEnable { get; set; }
        public bool? HiAutoAck { get; set; }
        public bool? LoEnable { get; set; }
        public bool? LoAutoAck { get; set; }
        public bool? LoLoEnable { get; set; }
        public bool? LoLoAutoAck { get; set; }
        public float? HiHiSPLimit { get; set; }
        public float? HiHiSP { get; set; }
        public float? HiSP { get; set; }
        public float? LoSP { get; set; }
        public float? LoLoSP { get; set; }
        public float? LoLoSPLimit { get; set; }
        public float? ResetDeadband { get; set; }
        public bool? StartupDlyCondHH { get; set; }
        public bool? StartupDlyCondH { get; set; }
        public bool? StartupDlyCondL { get; set; }
        public bool? StartupDlyCondLL { get; set; }
        public int? Cfg_HiHiOnTmr { get; set; }
        public int? Cfg_HiHiOffTmr { get; set; }
        public int? Cfg_HiHiDlyTmr { get; set; }
        public int? Cfg_HiHiSDTmr { get; set; }
        public int? Cfg_HiOnTmr { get; set; }
        public int? Cfg_HiOffTmr { get; set; }
        public int? Cfg_HiDlyTmr { get; set; }
        public int? Cfg_LoOnTmr { get; set; }
        public int? Cfg_LoOffTmr { get; set; }
        public int? Cfg_LoDlyTmr { get; set; }
        public int? Cfg_LoLoOnTmr { get; set; }
        public int? Cfg_LoLoOffTmr { get; set; }
        public int? Cfg_LoLoDlyTmr { get; set; }
        public int? Cfg_LoLoSDTmr { get; set; }
        public bool? BadPVAlarm { get; set; }
        public bool? HSD { get; set; }
        public bool? HiHiAlarm { get; set; }
        public bool? HiAlarm { get; set; }
        public bool? LoAlarm { get; set; }
        public bool? LoLoAlarm { get; set; }
        public bool? LSD { get; set; }
        public bool? AlmAck { get; set; }
        public bool? AlmReset { get; set; }
        public bool? AlmResetAll { get; set; }
        public bool? AlmSupress { get; set; }
        public bool? InUse { get; set; }
        public bool? PVLimitEnable { get; set; }
        public bool? BadPVAlm_TriggerALL { get; set; }
        public int? AlmCode { get; set; }
        public int? FaultCode { get; set; }
        public bool? SimReset { get; set; }
    }
}
