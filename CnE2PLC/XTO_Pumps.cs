using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{
    public class Pump : XTO_AOI
    {
        public Pump() { }
        public Pump(XmlNode node) : base(node) { }

        public override void ClearCounts() { }
    }

    public class Motor_VFD : XTO_AOI
    {
        public Motor_VFD() { }
        public Motor_VFD(XmlNode node) : base(node) {
            Cfg_Faceplate = L5K_strings[0];
            Cfg_EquipID = L5K_strings[1];
            Cfg_EquipDesc = L5K_strings[2];
            Cfg_Detail = L5K_strings[3];
        }

        #region Parameters
        public bool? EN_FB_Running { get; set; }
        public bool? EN_HOA_device {  get; set; }
        public bool? AlarmFTR { get; set; }
        public bool? AlarmFTS { get; set; }
        public bool? Fault { get; set; }
        public bool? Reset { get; set; }
        public bool? MCC_Hand { get; set; }
        public bool? MCC_Auto { get; set; }
        public bool? Mode_AUTO { get; set; }
        public bool? Mode_MANUAL { get; set; }
        public bool? CMD_AutoRun { get; set; }
        public bool? CMD_ManualRun { get; set; }
        public bool? CMD_ManualStop { get; set; }
        public bool? AutoReq { get; set; }
        public bool? ManualReq { get; set; }
        public bool? MaintenanceHold { get; set; }
        public bool? MaintenanceHoldReq { get; set; }
        public bool? MaintenanceHoldRelease { get; set; }
        public bool? RUN_CMD { get; set; }
        public bool? Running { get; set; }
        public bool? Running_FB { get; set; }
        public float? RT_Hours { get; set; }
        public bool? Intlck_AUTO { get; set; }
        public bool? Intlck_MANUAL { get; set; }
        public float? AutoSpdReq { get; set; }
        public float? ManSpdReq { get; set; }
        public float? SpeedCMD { get; set; }
        public bool? AutoFixedSpd { get; set; }
        #endregion

        public override void ClearCounts() { }

        public void ToRunFailRow(Excel.Range row)
        {
            row.Cells[1, 1].Value = Cfg_EquipID;
            row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
            row.Cells[1, 3].Value = Name;
            row.Cells[1, 4].Value = $"{Name}.FailToRun";
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

            //if (PosFail_Count == 0)
            //{
            //    {
            //        row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
            //        row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
            //    }
            //}

        }

    }

    public class P_PF753 : Pump 
    {
        public P_PF753() { }
        public P_PF753(XmlNode node) : base(node) { }
    }

    public class P_PF755 : P_PF753
    {
        public P_PF755() { }
        public P_PF755(XmlNode node) : base(node) { }
    }
}
