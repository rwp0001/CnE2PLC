using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{
    public class AIData : XTO_AOI
    {
        public AIData() { }

        public AIData(XmlNode node) : base(node)
        {
            if (L5K_strings.Count > 2)
            {
                Cfg_EquipID = L5K_strings[2];
                Cfg_EquipDesc = L5K_strings[1];
                Cfg_EU = L5K_strings[3];
            }
        }

        // tag counts
        public int PV_Count { get; set; } = 0;
        public int HSD_Count { get; set; } = 0;
        public int LSD_Count { get; set; } = 0;
        public int HiHi_Count { get; set; } = 0;
        public int Hi_Count { get; set; } = 0;
        public int Lo_Count { get; set; } = 0;
        public int LoLo_Count { get; set; } = 0;
        public int BadPV_Count { get; set; } = 0;
        public int SUHH_Count { get; set; } = 0;
        public int SUH_Count { get; set; } = 0;
        public int SUL_Count { get; set; } = 0;
        public int SULL_Count { get; set; } = 0;


        private string RowComment
        {
            get
            {
                string c = $"PLC Tag Description: {Description}\n";
                c += $"PLC DataType: {DataType}\n";
                c += $"Max EU: {MaxEU} {Cfg_EU}, Min EU: {MinEU} {Cfg_EU}\nMax Raw: {MaxRaw}, Min Raw: {MinRaw}\n";
                if (Sim == true) c += "Input is Simmed.\n";
                return c;
            }
        }

        #region CnE Outputs
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

            row.Cells[1, 3].AddComment(RowComment);
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

            if (HSD_Count == 0)
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

            if (LSD_Count == 0)
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
            row.Cells[1, 9].Value = $"{Cfg_HiHiOnTmr} Sec.";
            row.Cells[1, 10].Value = $"{Cfg_HiHiOffTmr} Sec.";
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = HiHiAutoAck==true ? "Y" : "";
            row.Cells[1, 16].Value = "";

            if (HiHi_Count == 0)
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
            row.Cells[1, 4].Value = $"{Name}.HiAlarm";
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = (HiEnable == true & InUse == true) ? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = HiSP;
            row.Cells[1, 8].Value = Cfg_EU;
            row.Cells[1, 9].Value = $"{Cfg_HiOnTmr} Sec.";
            row.Cells[1, 10].Value = $"{Cfg_HiOffTmr} Sec.";
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = HiAutoAck == true ? "Y" : "";
            row.Cells[1, 16].Value = "";

            if (Hi_Count == 0)
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

            if (Lo_Count == 0)
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

            if (LoLo_Count == 0)
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

            if (BadPV_Count == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

        }
        #endregion

        #region Data Outputs
        public static void ToHeaderRow(Excel.Range row)
        {
            int i = 1;
            row.Cells[1, i++].Value = "Scope";
            row.Cells[1, i++].Value = "Tag Name";
            row.Cells[1, i++].Value = "IO";
            row.Cells[1, i++].Value = "Tag Description";
            row.Cells[1, i++].Value = "HMI EquipID";
            row.Cells[1, i++].Value = "HMI EquipDesc";
            row.Cells[1, i++].Value = "HMI EU";
            row.Cells[1, i++].Value = "AOI Calls";
            row.Cells[1, i++].Value = "Tag References";
            row.Cells[1, i++].Value = "InUse";
            row.Cells[1, i++].Value = "Raw";
            row.Cells[1, i++].Value = "Min Raw";
            row.Cells[1, i++].Value = "Max Raw";
            row.Cells[1, i++].Value = "Min EU";
            row.Cells[1, i++].Value = "Max EU";
            row.Cells[1, i++].Value = "PV";
            row.Cells[1, i++].Value = "HiHi Enable";
            row.Cells[1, i++].Value = "HiHi Auto Ack";
            row.Cells[1, i++].Value = "HiHi SP Limit";
            row.Cells[1, i++].Value = "HiHi SP";
            row.Cells[1, i++].Value = "HiHi On Time";
            row.Cells[1, i++].Value = "HiHi Off Time";
            row.Cells[1, i++].Value = "HiHi Dly Time";
            row.Cells[1, i++].Value = "HiHi SD Time";
            row.Cells[1, i++].Value = "HiHi Alarm";
            row.Cells[1, i++].Value = "HiHi Count";
            row.Cells[1, i++].Value = "HSD Count";
            row.Cells[1, i++].Value = "Hi Enable";
            row.Cells[1, i++].Value = "Hi Auto Ack";
            row.Cells[1, i++].Value = "Hi SP";
            row.Cells[1, i++].Value = "Hi On Time";
            row.Cells[1, i++].Value = "Hi Off Time";
            row.Cells[1, i++].Value = "Hi Dly Time";
            row.Cells[1, i++].Value = "Hi Alarm";
            row.Cells[1, i++].Value = "Hi Count";
            row.Cells[1, i++].Value = "Lo Enable";
            row.Cells[1, i++].Value = "Lo Auto Ack";
            row.Cells[1, i++].Value = "Lo SP";
            row.Cells[1, i++].Value = "Lo On Time";
            row.Cells[1, i++].Value = "Lo Off Time";
            row.Cells[1, i++].Value = "Lo Dly Time";
            row.Cells[1, i++].Value = "Lo Alarm";
            row.Cells[1, i++].Value = "Lo Count";
            row.Cells[1, i++].Value = "LoLo Enable";
            row.Cells[1, i++].Value = "LoLo Auto Ack";
            row.Cells[1, i++].Value = "LoLo SP Limit";
            row.Cells[1, i++].Value = "LoLo SP";
            row.Cells[1, i++].Value = "LoLo On Time";
            row.Cells[1, i++].Value = "LoLo Off Time";
            row.Cells[1, i++].Value = "LoLo Dly Time";
            row.Cells[1, i++].Value = "LoLo SD Time";
            row.Cells[1, i++].Value = "LoLo Alarm";
            row.Cells[1, i++].Value = "LoLo Count";
            row.Cells[1, i++].Value = "LSD Count";
            row.Cells[1, i++].Value = "Sim";
            row.Cells[1, i++].Value = "Sim PV";
            row.Cells[1, i++].Value = "Bad PV Enable";
            row.Cells[1, i++].Value = "Bad PV Auto Ack";
            row.Cells[1, i++].Value = "Bad PV Alarm";
        }
        public void ToDataRow(Excel.Range row)
        {
            int i = 1;
            row.Cells[1, i++].Value = Path;
            row.Cells[1, i++].Value = Name;
            row.Cells[1, i++].Value = IO;
            row.Cells[1, i++].Value = Description;

            row.Cells[1, i++].Value = Cfg_EquipID;
            row.Cells[1, i++].Value = Cfg_EquipDesc;
            row.Cells[1, i++].Value = Cfg_EU;

            row.Cells[1, i++].Value = AOICalls;
            row.Cells[1, i++].Value = References;

            row.Cells[1, i++].Value = InUse == true ? "Yes" : "No";

            row.Cells[1, i++].Value = Raw;
            row.Cells[1, i++].Value = MinRaw;
            row.Cells[1, i++].Value = MaxRaw;
            row.Cells[1, i++].Value = MinEU;
            row.Cells[1, i++].Value = MaxEU;
            row.Cells[1, i++].Value = PV;

            row.Cells[1, i++].Value = HiHiEnable == true ? "Yes" : "No";
            row.Cells[1, i++].Value = HiHiAutoAck == true ? "Yes" : "No";
            row.Cells[1, i++].Value = HiHiSPLimit;
            row.Cells[1, i++].Value = HiHiSP;
            row.Cells[1, i++].Value = Cfg_HiHiOnTmr;
            row.Cells[1, i++].Value = Cfg_HiHiOffTmr;
            row.Cells[1, i++].Value = Cfg_HiHiDlyTmr;
            row.Cells[1, i++].Value = Cfg_HiHiSDTmr;
            row.Cells[1, i++].Value = HiHiAlarm == true ? "Alarm" : "Ok";
            row.Cells[1, i++].Value = HiHi_Count;
            row.Cells[1, i++].Value = HSD_Count;

            row.Cells[1, i++].Value = HiEnable == true ? "Yes" : "No";
            row.Cells[1, i++].Value = HiAutoAck == true ? "Yes" : "No";
            row.Cells[1, i++].Value = HiSP;
            row.Cells[1, i++].Value = Cfg_HiOnTmr;
            row.Cells[1, i++].Value = Cfg_HiOffTmr;
            row.Cells[1, i++].Value = Cfg_HiDlyTmr;
            row.Cells[1, i++].Value = HiAlarm == true ? "Alarm" : "Ok";
            row.Cells[1, i++].Value = Hi_Count;

            row.Cells[1, i++].Value = LoEnable == true ? "Yes" : "No";
            row.Cells[1, i++].Value = LoAutoAck == true ? "Yes" : "No";
            row.Cells[1, i++].Value = LoSP;
            row.Cells[1, i++].Value = Cfg_LoOnTmr;
            row.Cells[1, i++].Value = Cfg_LoOffTmr;
            row.Cells[1, i++].Value = Cfg_LoDlyTmr;
            row.Cells[1, i++].Value = LoAlarm == true ? "Alarm" : "Ok";
            row.Cells[1, i++].Value = Lo_Count;

            row.Cells[1, i++].Value = LoLoEnable == true ? "Yes" : "No";
            row.Cells[1, i++].Value = LoLoAutoAck == true ? "Yes" : "No";
            row.Cells[1, i++].Value = LoLoSPLimit;
            row.Cells[1, i++].Value = LoLoSP;
            row.Cells[1, i++].Value = Cfg_LoLoOnTmr;
            row.Cells[1, i++].Value = Cfg_LoLoOffTmr;
            row.Cells[1, i++].Value = Cfg_LoLoDlyTmr;
            row.Cells[1, i++].Value = Cfg_LoLoSDTmr;
            row.Cells[1, i++].Value = LoLoAlarm == true ? "Alarm" : "Ok";
            row.Cells[1, i++].Value = LoLo_Count;
            row.Cells[1, i++].Value = LSD_Count;

            row.Cells[1, i++].Value = Sim == true ? "Yes" : "No";
            row.Cells[1, i++].Value = SimPV;

            row.Cells[1, i++].Value = BadPVEnable == true ? "Yes" : "No";
            row.Cells[1, i++].Value = BadPVAutoAck == true ? "Yes" : "No";
            row.Cells[1, i++].Value = BadPVAlarm == true ? "Alarm" : "Ok";

        }
        #endregion

        public override void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.Sim == true)
            {
                e.CellStyle.BackColor = Color.Red;
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                return;
            }

            if (this.BadPVAlarm == true)
            {
                e.CellStyle.BackColor = Color.Red;
                e.CellStyle.ForeColor = Color.Cyan;
                //e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                return;
            }


            if (InUse != true)
            {
                e.CellStyle.BackColor = Color.LightGray;
            }

            if (AOICalls==0)
            {
                e.CellStyle.BackColor = Color.LightGray;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Italic);

            }

            if (References==0)
            {
                e.CellStyle.ForeColor = Color.DarkCyan;
            }

            if (HiAlarm == true || LoAlarm == true)
            {
                e.CellStyle.ForeColor = Color.DarkOrange;
            }

            if (HiHiAlarm == true || LoLoAlarm == true)
            {
                e.CellStyle.ForeColor = Color.Red;
            }
        }

        public override void ClearCounts()
        {
            PV_Count  = 0;
            HSD_Count = 0;
            LSD_Count = 0;
            HiHi_Count  = 0;
            Hi_Count = 0;
            Lo_Count = 0;
            LoLo_Count  = 0;
            BadPV_Count  = 0;
            SUHH_Count  = 0;
            SUH_Count  = 0;
            SUL_Count  = 0;
            SULL_Count  = 0;
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

        public string Cfg_EU { get; set; } = string.Empty;

    }
}
