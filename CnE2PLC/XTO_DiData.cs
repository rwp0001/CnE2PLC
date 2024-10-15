using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{
    public class DiData : XTO_AOI
    {
        public DiData() 
        {
            AOI_Name = "DIData";
        }

        public DiData(XmlNode node)
        {
            AOI_Name = "DIData";
            Import(node);
            if (L5K_strings.Count > 1)
            {
                Cfg_EquipID = L5K_strings[2];
                Cfg_EquipDesc = L5K_strings[1];
            }
        }

        public void ToValueRow(Excel.Range row, int TagCount = -1)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.Value";
            row.Cells[1, 5].Value = "Digital Input";
            row.Cells[1, 6].Value = InUse == true ? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = "";
            row.Cells[1, 8].Value = "Bool";
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

            // comments
            string c = $"PLC Tag Description:\n{Description}\n";
            c += $"PLC DataType:\n{DataType}\n";
            if (Sim == true) c += "Input is Simmed.\n";
            row.Cells[1, 3].AddComment(c);


        }

        public void ToAlarmRow(Excel.Range row, int TagCount = -1)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = string.Format("{0}.Alarm", Name);
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = (AlmEnable == true & InUse == true) ? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = "";
            row.Cells[1, 8].Value = "Bool";
            row.Cells[1, 9].Value = string.Format("{0} Sec.", Cfg_AlmOnTmr);
            row.Cells[1, 10].Value = string.Format("{0} Sec.", Cfg_AlmOffTmr);
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = AlmAutoAck == true ? "Y" : "";
            row.Cells[1, 16].Value = "";

            if (TagCount == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

        }

        public void ToShutdownRow(Excel.Range row, int TagCount = -1)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = string.Format("{0}.Shutdown", Name);
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = "Not In Use";
            row.Cells[1, 6].Value = (AlmEnable == true & InUse == true) ? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = "";
            row.Cells[1, 8].Value = "Bool";
            row.Cells[1, 9].Value = string.Format("{0} Sec.", Cfg_SDDlyTmr);
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

        public override void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.Sim == true)
            {
                e.CellStyle.BackColor = Color.Red;
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                return;
            }

            if (InUse != true)
            {
                e.CellStyle.BackColor = Color.LightGray;
            }

            if (AOICalls == 0)
            {
                e.CellStyle.BackColor = Color.LightGray;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Italic);

            }

            if (References == 0)
            {
                e.CellStyle.ForeColor = Color.DarkCyan;
            }
        }

        //Parameters

        /// <summary>
        /// the raw value being processed.
        /// </summary>
        public bool? Raw { get; set; }
        /// <summary>
        /// the processed output value.
        /// </summary>
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

        //Local Tags

    }
}
