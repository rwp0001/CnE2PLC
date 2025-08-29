using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{
    public class DIData : XTO_AOI
    {
        public DIData() { }

        public DIData(XmlNode node) : base (node) 
        {
            if (L5K_strings.Count > 1)
            {
                Cfg_EquipID = L5K_strings[2];
                Cfg_EquipDesc = L5K_strings[1];
            }
        }

        // tag counts
        public int SD_Count { get; set; } = 0;
        public int Val_Count { get; set; } = 0;
        public int Alm_Count { get; set; } = 0;
        public int Raw_Count { get; set; } = 0;

        private string AlarmText { 
            get {
                string c = string.Empty;
                c += Alarm==true ? "Alarm is active.\n" : "";
                c += Sim==true ? "Input is Simmed.\n" : "";
                if (InUse == false) c += "Not In Use.\n";
                if (AOICalled == false) c += "AOI Not Called.\n";
                if (AOICalls > 1) c += "AOI called more then once.\n";
                if (References == 0) c += "Not used in Program. SCADA Tag.\n";
                if (Placeholder == true) c += "Placeholder on IO.\n";
                return c;
            } }

        public override string ToString()
        {
            string c = $"Name: {Name}\n";
            c += $"PLC Tag Description: {Description}\n";
            c += $"PLC DataType: {DataType}\n";
            c += AlarmText;
            if (IO.Length > 0)
            {
                c += "IO: ";
                c += IO;
            }
            return c;
        }

        public void ToValueRow(Excel.Range row)
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

            if ( Val_Count == 0)
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
            row.Cells[1, 3].AddComment(ToString());
        }

        public void ToAlarmRow(Excel.Range row)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.Alarm";
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = (AlmEnable == true & InUse == true) ? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = "";
            row.Cells[1, 8].Value = "Bool";
            row.Cells[1, 9].Value = $"{Cfg_AlmOnTmr} Sec.";
            row.Cells[1, 9].AddComment($"Delay: {Cfg_AlmDlyTmr} secs.");
            row.Cells[1, 10].Value = $"{Cfg_AlmOffTmr} Sec.";
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = AlmAutoAck == true ? "Y" : "";
            row.Cells[1, 16].Value = "";

            if (Alm_Count == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

        }

        public void ToShutdownRow(Excel.Range row)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.Shutdown";
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = "Not In Use";
            row.Cells[1, 6].Value = (AlmEnable == true & InUse == true) ? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = "";
            row.Cells[1, 8].Value = "Bool";
            row.Cells[1, 9].Value = $"{Cfg_SDDlyTmr} Sec.";
            row.Cells[1, 10].Value = "";
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = "";
            row.Cells[1, 16].Value = "";

            if (SD_Count == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

        }

        public static void ToHeaderRow(Excel.Range row)
        {
            int i = 1;
            row.Cells[1, i++].Value = "Scope";
            row.Cells[1, i++].Value = "Tag Name";
            row.Cells[1, i++].Value = "IO";
            row.Cells[1, i++].Value = "Tag Description";
            row.Cells[1, i++].Value = "HMI EquipID";
            row.Cells[1, i++].Value = "HMI EquipDesc";
            row.Cells[1, i++].Value = "AOI Calls";
            row.Cells[1, i++].Value = "Tag References";
            row.Cells[1, i++].Value = "InUse";
            row.Cells[1, i++].Value = "Raw";
            row.Cells[1, i++].Value = "Value";
            row.Cells[1, i++].Value = "Alarm Enable";
            row.Cells[1, i++].Value = "Alarm Auto Ack";
            row.Cells[1, i++].Value = "Alarm State";
            row.Cells[1, i++].Value = "Alarm On Time";
            row.Cells[1, i++].Value = "Alarm Off Time";
            row.Cells[1, i++].Value = "Alarm Dly Time";
            row.Cells[1, i++].Value = "Alarm SD Time";
            row.Cells[1, i++].Value = "Alarm";
            row.Cells[1, i++].Value = "Alarm Count";
            row.Cells[1, i++].Value = "SD Count";
            row.Cells[1, i++].Value = "Sim";
            row.Cells[1, i++].Value = "Sim Value";
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

            row.Cells[1, i++].Value = AOICalls;
            row.Cells[1, i++].Value = References;

            row.Cells[1, i++].Value = InUse == true ? "Yes" : "No";

            row.Cells[1, i++].Value = Raw == true ? "On" : "Off";
            row.Cells[1, i++].Value = Value == true ? "On" : "Off";

            row.Cells[1, i++].Value = AlmEnable == true ? "Yes" : "No";
            row.Cells[1, i++].Value = AlmAutoAck == true ? "Yes" : "No";
            row.Cells[1, i++].Value = AlmState == 1 ? "On" : "Off";
            row.Cells[1, i++].Value = Cfg_AlmOnTmr;
            row.Cells[1, i++].Value = Cfg_AlmOffTmr;
            row.Cells[1, i++].Value = Cfg_AlmDlyTmr;
            row.Cells[1, i++].Value = Cfg_SDDlyTmr;
            row.Cells[1, i++].Value = Alarm == true ? "Alarm" : "Ok";
            row.Cells[1, i++].Value = Alm_Count;
            row.Cells[1, i++].Value = SD_Count;

            row.Cells[1, i++].Value = Sim == true ? "Yes" : "No";
            row.Cells[1, i++].Value = SimValue == true ? "On" : "Off";


        }

        public override void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (Simmed)
            {
                e.CellStyle.BackColor = Color.Red;
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                return;
            }

            if (NotInUse)
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

            if(Alarm == true)
            {
                e.CellStyle.ForeColor = Color.Red;
            }

        }

        public override bool Simmed {  get { return Sim ?? false; } }
        public override bool Bypassed { get { return BypActive ?? false; } }
        public override bool Alarmed { get { return AlmCode == 0 ? false : true; } }
        public override bool NotInUse
        {
            get
            {
                if( InUse == false ) return true;
                //if( AOICalled == false ) return true;
                //if(Placeholder == true ) return true;
                return false;
            }
        }
        public override bool Placeholder
        {
            get
            {
                if( IO.ToLower().Contains( "placeholder" ) ) return true;
                if( IO.ToLower().Contains( $"{Name}.Value".ToLower()) & Raw_Count < 2 ) return true;
                return false;
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
        public bool? BypActive { get; set; }
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

        public override void ClearCounts() 
        {
            SD_Count  = 0;
            Val_Count = 0;
            Alm_Count = 0;
        }

    }

}
