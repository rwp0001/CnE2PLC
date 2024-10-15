using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{
    public class AoData : XTO_AOI
    {
        public AoData() 
        {
            AOI_Name = "AOData";
        }

        public AoData(XmlNode node) {

            Import(node);
            if (L5K_strings.Count > 2)
            {
                AOI_Name = "AOData";
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

            //comments
            string c = $"PLC Tag Description:\n{Description}\n";
            c += $"PLC Tag DataType: {DataType}\n";
            c += $"Max EU:  {MaxEU} {Cfg_EU}\n";
            c += $"Min EU:  {MinEU} {Cfg_EU}\n";
            c += $"Max Raw:  {MaxRaw}\n";
            c += $"Min Raw:  {MinRaw}\n";
            if (Sim == true) c += "Output is Simmed.\n";
            col.Cells[14, 1].AddComment(c);

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
    }
}
