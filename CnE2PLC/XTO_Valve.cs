﻿using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{

    public class Valve : XTO_AOI
    {
        public Valve() { }

        public Valve(XmlNode node) : base(node) { }
        public bool? AutoReq { get; set; }
        public bool? ManualReq { get; set; }
        public bool? Intlck_AUTO { get; set; }
        public bool? Intlck_MANUAL { get; set; }
        public bool? Mode_AUTO { get; set; }
        public bool? Mode_MANUAL { get; set; }


        public override void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

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

        public override void ClearCounts() { }


    }

    public class TwoPositionValveV2 : Valve
    {

        public TwoPositionValveV2() { }

        public TwoPositionValveV2(XmlNode node) : base(node)
        {
            if (L5K_strings.Count > 1)
            {
                Cfg_EquipID = L5K_strings[1];
                Cfg_EquipDesc = L5K_strings[2];
            }
        }

        public bool? OpenedFB { get; set; }
        public bool? ClosedFB { get; set; }
        public bool? Cmd_Type { get; set; }
        public bool? AutoCmd { get; set; }
        public bool? ManualCmd { get; set; }
        public bool? FBInv { get; set; }
        public bool? DisableFB { get; set; }
        public int? OpenFaultTime { get; set; }
        public int? CloseFaultTime { get; set; }
        public bool? Out { get; set; }
        public bool? Open { get; set; }
        public bool? Closed { get; set; }
        public bool? FailedToOpen { get; set; }
        public bool? FailedToClose { get; set; }

        // tag counts
        public int Open_Count { get; set; } = 0;
        public int Close_Count { get; set; } = 0;
        public int FTO_Count { get; set; } = 0;
        public int FTC_Count { get; set; } = 0;

        #region CnE Outputs
        public void ToColumn(Excel.Range col)
        {
            col.Cells[2, 1].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            col.Cells[13, 1].Value = InUse == true ? "Yes" : "No";
            col.Cells[14, 1].Value = Name;

            //comments
            string c = $"PLC Tag Description:\n{Description}\n";
            c += $"PLC Tag DataType: {DataType}\n";
            if (DisableFB == true) c += "No Feedback.\n";
            if (FBInv == true) c += "Feedback Inverted.\n";
            col.Cells[14, 1].AddComment(c);

        }

        public void ToOpenRow(Excel.Range row)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = string.Format("{0}.Open", Name);
            row.Cells[1, 5].Value = "AOI Output";
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

            if (Open_Count == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

            // comments
            string c = $"PLC Tag Description:\n{Description}\n";
            c += $"PLC Data Type:\n{DataType}\n";
            row.Cells[1, 3].AddComment(c);
        }

        public void ToCloseRow(Excel.Range row)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.Close";
            row.Cells[1, 5].Value = "AOI Output";
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

            if (Close_Count == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

        }

        public void ToFailedToOpenRow(Excel.Range row)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.FailedToOpen";
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = InUse == true ? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = "";
            row.Cells[1, 8].Value = "Bool";
            row.Cells[1, 9].Value = string.Format("{0} Sec.", OpenFaultTime);
            row.Cells[1, 10].Value = "";
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = "";
            row.Cells[1, 16].Value = "";

            if (FTO_Count == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }
        }

        public void ToFailedToCloseRow(Excel.Range row)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.FailedToClose";
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = InUse == true ? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = "";
            row.Cells[1, 8].Value = "Bool";
            row.Cells[1, 9].Value = $"{CloseFaultTime} Sec.";
            row.Cells[1, 10].Value = "";
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = "";
            row.Cells[1, 16].Value = "";

            if (FTC_Count == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }
        }
        #endregion


    }

    public class TwoPositionValve : TwoPositionValveV2
    {

        public TwoPositionValve() { }

        public TwoPositionValve(XmlNode node) : base(node)
        {
            if (L5K_strings.Count > 1) // found a version with no strings at Maverick.
            {
                Cfg_EquipID = L5K_strings[2];
                Cfg_EquipDesc = L5K_strings[1];
            }
            GetTimers();
        }

        /// <summary>
        /// get the values from the timers.
        /// I'm sorry.
        /// </summary>
        private void GetTimers()
        {
            string[] s1 = L5K.Split("[");
            string[] s2 = s1[2].Split(",");
            string[] s3 = s1[3].Split(",");
            int O, C;
            int.TryParse(s2[1].Trim(), out C);
            int.TryParse(s2[1].Trim(), out O);
            CloseFaultTime = C / 1000;
            OpenFaultTime = O / 1000;

        }

    }

    public class ValveAnalog : Valve
    {
        public ValveAnalog() { }
        public ValveAnalog(XmlNode node) : base(node)
        {
            if (L5K_strings.Count > 1)
            {
                Cfg_EquipID = L5K_strings[1];
                Cfg_EquipDesc = L5K_strings[2];
            }
        }

        #region Public Properties
        public bool? Cmd_Invert {  get; set; }
        public float? PosFB { get; set; }
        public int? PosFaultTime { get; set; }
        public float? AutoCmd { get; set; }
        public float? ManualCmd { get; set; }
        public bool? FBInv { get; set; }
        public bool? DisableFB {  get; set; }
        public float? Out { get; set; }
        public bool? PosFail { get; set; }
        public float? Pos { get; set; }
        #endregion

        // Tag Counts
        public int Pos_Count { get; set; } = 0;
        public int PosFail_Count { get; set; } = 0;

        #region CnE Outputs
        private string ColComment { 
            get
            {
                string c = $"PLC Tag Description: {Description}\n";
                c += $"PLC Tag DataType: {DataType}\n";
                if (DisableFB == true) c += "No Feedback.\n";
                if (Cmd_Invert == true) c += "Command Inverted.\n";
                if (FBInv == true) c += "Feedback Inverted.\n";
                return c;
            } 
        }

        private string RowComment
        {
            get
            {
                string c = $"PLC Tag Description: {Description}\n";
                c += $"PLC Data Type: {DataType}\n";
                return c;
            }
        }

        public void ToColumn(Excel.Range col)
        {
            col.Cells[2, 1].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            col.Cells[13, 1].Value = InUse == true ? "Yes" : "No";
            col.Cells[14, 1].Value = Name;

            col.Cells[14, 1].AddComment(ColComment);

        }

        public void ToPosRow(Excel.Range row)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.Pos";
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = InUse == true ? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = "";
            row.Cells[1, 8].Value = "%";
            row.Cells[1, 9].Value = "";
            row.Cells[1, 10].Value = "";
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = "";
            row.Cells[1, 16].Value = "";

            if (Pos_Count == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

            row.Cells[1, 3].AddComment(RowComment);
        }

        public void ToPosFailRow(Excel.Range row)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.PosFail";
            row.Cells[1, 5].Value = "AOI Output";
            row.Cells[1, 6].Value = InUse == true ? "Standard IO" : "Not In Use";
            row.Cells[1, 7].Value = "";
            row.Cells[1, 8].Value = "Bool";
            row.Cells[1, 9].Value = $"{PosFaultTime} Sec.";
            row.Cells[1, 10].Value = "";
            row.Cells[1, 11].Value = "";
            row.Cells[1, 12].Value = "";
            row.Cells[1, 13].Value = "";
            row.Cells[1, 14].Value = "";
            row.Cells[1, 15].Value = "";
            row.Cells[1, 16].Value = "";

            if (PosFail_Count == 0)
            {
                {
                    row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
                    row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
                }
            }

        }
        #endregion


    }


}
