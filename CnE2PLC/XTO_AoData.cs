using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{
    public class AOData : XTO_AOI
    {
        public AOData() { }

        public AOData(XmlNode node) : base(node) {
            if (L5K_strings.Count > 2)
            {
                Cfg_EquipID = L5K_strings[1];
                Cfg_EquipDesc = L5K_strings[0];
                Cfg_EU = L5K_strings[2];
            }
        }


        // Parameters
        public float? CV { get; set; }
        public float? MinEU { get; set; }
        public float? MaxEU { get; set; }
        public float? MinRaw { get; set; }
        public float? MaxRaw { get; set; }
        public float? Raw { get; set; }
        public bool? Sim { get; set; }
        public float? SimCV { get; set; }
        public bool? Cfg_IncToClose { get; set; }
        public bool? SimReset { get; set; }
        public bool? SimActive { get; set; }

        // Local Tags
        public string? Cfg_EU { get; set; }
        public bool? SIM_ONS { get; set; }



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
            row.Cells[1, i++].Value = "CV";
            row.Cells[1, i++].Value = "Inc To Close";
            row.Cells[1, i++].Value = "Sim";
            row.Cells[1, i++].Value = "Sim CV";

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
            row.Cells[1, i++].Value = CV;
            row.Cells[1, i++].Value = Cfg_IncToClose == true ? "Yes" : "No";
            row.Cells[1, i++].Value = Sim == true ? "Yes" : "No";
            row.Cells[1, i++].Value = SimCV;

        }
        #endregion

        private string ColComment
        {
            get
            {
                string c = $"PLC Tag Description: {Description}\n";
                c += $"PLC Tag DataType: {DataType}\n";
                c += $"Max EU: {MaxEU} {Cfg_EU}, Min EU: {MinEU} {Cfg_EU}\n";
                c += $"Max Raw: {MaxRaw}, Min Raw: {MinRaw}\n";
                if (Sim == true) c += "Output is Simmed.\n";
                return c;
            }
        }

        public void ToColumn(Excel.Range col)
        {
            col.Cells[2, 1].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            col.Cells[13, 1].Value = InUse == true ? "Yes" : "No";
            col.Cells[14, 1].Value = Name;

            if (Sim == true)
            {
                col.Cells[14, 1].Interior.Color = ColorTranslator.ToOle(Color.DarkRed);
                col.Cells[14, 1].Font.Color = ColorTranslator.ToOle(Color.White);
            }

            col.Cells[14, 1].AddComment(ColComment);

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

        public override void ClearCounts() {}
    }
}
