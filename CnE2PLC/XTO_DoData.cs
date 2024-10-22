using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{
    public class DoData : XTO_AOI
    {
        public DoData() 
        {
            AOI_Name = "DOData";
        }
        public DoData(XmlNode node) : base(node) 
        {
            AOI_Name = "DOData";
            if (L5K_strings.Count > 1)
            {
                Cfg_EquipID = L5K_strings[1];
                Cfg_EquipDesc = L5K_strings[0];
            }
        }

        private string ColComment { 
            get 
            {
                string c = $"PLC Tag Description: {Description}\n";
                c += $"PLC Tag DataType: {DataType}\n";
                if (Sim == true) c += "Output is Simmed.\n";
                return c;
            } 
        }

        //Parameters
        public bool? Value { get; set; }
        public bool? Raw { get; set; }
        public bool? Sim { get; set; }
        public int? SimVal { get; set; }

        public void ToColumn(Excel.Range col, int TagCount = -1)
        {
            col.Cells[2, 1].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            col.Cells[13, 1].Value = InUse == true ? "Yes" : "No";
            col.Cells[14, 1].Value = Name;

            if(TagCount == 0)
            {
                {
                    col.Cells[14, 1].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    col.Cells[14, 1].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

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
    }
}
