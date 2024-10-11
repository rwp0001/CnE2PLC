using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{
    public class AiData : XTO_AOI
    {
        public AiData() { }

        public AiData(XmlNode node) : base(node)
        { 
            Import(node);
            if (L5K_strings.Count > 2)
            {
                Cfg_EquipID = L5K_strings[2];
                Cfg_EquipDesc = L5K_strings[1];
                Cfg_EU = L5K_strings[3];
            }
        }

        public static new string AOI_Name = "AiData";

        public void ToPVRow(Excel.Range row, int TagCount = -1)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.PV";
            row.Cells[1, 5].Value = "Analog Input";
            row.Cells[1, 6].Value = InUse == true? "Standard IO": "Not In Use";
            row.Cells[1, 7].Value = "";
            row.Cells[1, 8].Value = Cfg_EU;
            row.Cells[1, 9].Value = "";
            row.Cells[1, 10].Value = "";
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = "";
            row.Cells[1, 16].Value = "";

            if (TagCount == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }


            if (Sim == true)
            {
                row.Cells[1, 3].Interior.Color = ColorTranslator.ToOle(Color.DarkRed);
                row.Cells[1, 3].Font.Color = ColorTranslator.ToOle(Color.White);
            }

            //comments
            string c = $"PLC Tag Description:\n{Description}\n";
            c += $"PLC DataType:\n{DataType}\n";
            c += $"Max EU:  {MaxEU} {Cfg_EU}\nMin EU:  {MinEU} {Cfg_EU}\nMax Raw:  {MaxRaw}\nMin Raw:  {MinRaw}\n";
            if (Sim == true) c += "Input is Simmed.\n";
            row.Cells[1, 3].AddComment(c);
        }

        public void ToHSDRow(Excel.Range row, int TagCount = -1)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.HSD";
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = (HiHiEnable == true & InUse == true)? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = "";
            row.Cells[1, 8].Value = "Bool";
            row.Cells[1, 9].Value = $"{Cfg_HiHiSDTmr} Sec.";
            row.Cells[1, 10].Value = ""; 
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = "";
            row.Cells[1, 16].Value = "";

            if (TagCount == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

        }

        public void ToLSDRow(Excel.Range row, int TagCount = -1)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.LSD";
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = (LoLoEnable == true & InUse == true)? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = "";
            row.Cells[1, 8].Value = "Bool";
            row.Cells[1, 9].Value = $"{Cfg_LoLoSDTmr} Sec.";
            row.Cells[1, 10].Value = "";
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = "";
            row.Cells[1, 16].Value = "";

            if (TagCount == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

        }

        public void ToHiHiAlarmRow(Excel.Range row, int TagCount = -1)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.HiHiAlarm";
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = (HiHiEnable == true & InUse == true)? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = HiHiSP;
            row.Cells[1, 8].Value = Cfg_EU;
            row.Cells[1, 9].Value = string.Format("{0} Sec.", Cfg_HiHiOnTmr);
            row.Cells[1, 10].Value = string.Format("{0} Sec.", Cfg_HiHiOffTmr);
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = HiHiAutoAck==true ? "Y" : "";
            row.Cells[1, 16].Value = "";

            if (TagCount == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

            //comments
            row.Cells[1, 7].AddComment($"HiHi Limit: {HiHiSPLimit} {Cfg_EU}");
            row.Cells[1, 9].AddComment($"HiHi Delay: {Cfg_HiHiDlyTmr} Sec.");
        }

        public void ToHiAlarmRow(Excel.Range row, int TagCount = -1)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = string.Format("{0}.HiAlarm", Name);
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = (HiEnable == true & InUse == true) ? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = HiSP;
            row.Cells[1, 8].Value = Cfg_EU;
            row.Cells[1, 9].Value = string.Format("{0} Sec.", Cfg_HiOnTmr);
            row.Cells[1, 10].Value = string.Format("{0} Sec.", Cfg_HiOffTmr);
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = HiAutoAck == true ? "Y" : "";
            row.Cells[1, 16].Value = "";

            if (TagCount == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

            row.Cells[1, 9].AddComment($"Hi Delay: {Cfg_HiDlyTmr} Secs." );

        }

        public void ToLoAlarmRow(Excel.Range row, int TagCount = -1)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.LoAlarm";
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = (LoEnable == true & InUse == true) ? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = LoSP;
            row.Cells[1, 8].Value = Cfg_EU;
            row.Cells[1, 9].Value = $"{Cfg_LoOnTmr} Sec.";
            row.Cells[1, 10].Value = $"{Cfg_LoOffTmr} Sec.";
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = LoAutoAck == true ? "Y" : "";
            row.Cells[1, 16].Value = "";

            if (TagCount == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

            // comments
            row.Cells[1, 9].AddComment($"Lo Delay: {Cfg_LoDlyTmr} secs.");

        }

        public void ToLoLoAlarmRow(Excel.Range row, int TagCount = -1)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.LoLoAlarm";
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = (LoLoEnable == true & InUse == true) ? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = LoLoSP;
            row.Cells[1, 8].Value = Cfg_EU;
            row.Cells[1, 9].Value = $"{Cfg_LoLoOnTmr} Sec.";
            row.Cells[1, 10].Value = $"{Cfg_LoLoOffTmr} Sec.";
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = LoLoAutoAck == true ? "Y" : "";
            row.Cells[1, 16].Value = "";

            if (TagCount == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

            // comments
            row.Cells[1, 7].AddComment($"LoLo Limit: {LoLoSPLimit} {Cfg_EU}");
            row.Cells[1, 9].AddComment($"LoLo Delay: {Cfg_LoLoDlyTmr} Secs." );

        }

        public void ToBadPVAlarmRow(Excel.Range row, int TagCount = -1)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.BadPVAlarm";
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = (BadPVEnable == true & InUse == true) ? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = "";
            row.Cells[1, 8].Value = "Bool";
            row.Cells[1, 9].Value = "";
            row.Cells[1, 10].Value = "";
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = "";
            row.Cells[1, 15].Value = BadPVAutoAck == true ? "Y" : "";
            row.Cells[1, 16].Value = "";

            if (TagCount == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

        }

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
        public bool? PVLimitEnable { get; set; }
        public bool? BadPVAlm_TriggerALL { get; set; }
        public int? AlmCode { get; set; }
        public int? FaultCode { get; set; }
        public bool? SimReset { get; set; }

        public string? Cfg_EU { get; set; }

    }
}
