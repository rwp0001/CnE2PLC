using Microsoft.Office.Interop.Excel;
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

    public class Pump_1Spd : XTO_AOI
    {
        public Pump_1Spd() { }

        public Pump_1Spd(XmlNode node) : base(node) { }

        /// <summary>
        /// Alarm - Failed to Run
        /// </summary>
        public bool? AlarmFTR {  get; set; }

        /// <summary>
        /// Alarm - Failed to Stop
        /// </summary>
        public bool? AlarmFTS {  get; set; }

        /// <summary>
        /// Alarm - No Flow on Running Pump
        /// </summary>
        public bool? AlarmNoFlow {  get; set; }

        /// <summary>
        /// Alarm - Discharge Pressure HI
        /// </summary>
        public bool? AlarmPressDischgHI {  get; set; }

        /// <summary>
        /// Standard Alarm - Suction Pressure LO
        /// </summary>
        public bool? AlarmPressSuctLO {  get; set; }

        /// <summary>
        /// Standard Auto Mode Operations Allowed
        /// </summary>
        public bool? Auto_Allowed {  get; set; }

        /// <summary>
        /// Auto Mode request fm HMI
        /// </summary>
        public bool? AutoReq {  get; set; }

        /*
					
CMD_AutoRun	Input	0	0		Decimal	BOOL	Standard	Auto Run Command	Read Only	0				
CMD_ManualRun	Input	0	0		Decimal	BOOL	Standard	Manual Run Command	Read/Write	0				
CMD_ManualStop	Input	0	0		Decimal	BOOL	Standard	Manual Stop Command	Read/Write	0				
			
EN_AlmDischgP_HI	Input	0	0		Decimal	BOOL	Standard	HI Dischg Pressure Alarm Enable	Read/Write	0				
EN_AlmFlow_LO	Input	0	0		Decimal	BOOL	Standard	LO Flow Alarm Enable	Read/Write	0				
EN_AlmSuctionP_LO	Input	0	0		Decimal	BOOL	Standard	LO Suction Pressure Alarm Enable	Read/Write	0				
EN_FB_Running	Input	0	0		Decimal	BOOL	Standard	Enable Running Status Feedback	Read/Write	0				
EN_HOA_device	Input	0	0		Decimal	BOOL	Standard	Enable HOA field device I/O	Read/Write	0				
EnableIn	Input	0	1		Decimal	BOOL	Standard	Enable Input - System Defined Parameter	Read Only	0				
EnableOut	Output	0	0		Decimal	BOOL	Standard	Enable Output - System Defined Parameter	Read Only	0				
Fault	Output	0	0		Decimal	BOOL	Standard	Fault condition exists (roll-up of other alarms)	Read Only	0				
Flow	Input	0	0.0		Float	REAL	Standard	Flow (actual)	Read/Write	0				
Flow_AlarmSP_LO	Input	0	0.0		Float	REAL	Standard	Flow Alarm SP - LO	Read/Write	0				
			
Intlck_AUTO	Input	0	0		Decimal	BOOL	Standard	Required Interlocks for AUTO MODE operation	Read Only	0				
Intlck_MANUAL	Input	0	0		Decimal	BOOL	Standard	Required Interlocks for MANUAL MODE operation	Read Only	0				
MaintenanceHold	Output	0	0		Decimal	BOOL	Standard	Maintenance Hold Enabled	Read Only	0				
MaintenanceHoldRelease	Input	0	0		Decimal	BOOL	Standard	Maintenance Hold Mode Release fm HMI	Read/Write	0				
MaintenanceHoldReq	Input	0	0		Decimal	BOOL	Standard	Maintenance Hold Mode Request  fm HMI	Read/Write	0				
Manual_Allowed	Local	0	0		Decimal	BOOL	Standard	Manual Mode Operations Allowed	None	0				
ManualReq	Input	0	0		Decimal	BOOL	Standard	Manual Mode request fm HMI	Read/Write	0				
MCC_Auto	Input	0	0		Decimal	BOOL	Standard	HOA in 'AUTO' at MCC	Read/Write	0				
MCC_Hand	Input	0	0		Decimal	BOOL	Standard	HOA in 'HAND' at MCC	Read/Write	0				
Mode_AUTO	Output	0	0		Decimal	BOOL	Standard	Remote Auto Mode is Active (Remote::HMI)	Read Only	0				
Mode_MANUAL	Output	0	0		Decimal	BOOL	Standard	Remote Manual Mode is Active (Remote::HMI)	Read Only	0				
Press_Dischg	Input	0	0.0		Float	REAL	Standard	Discharge Pressure (actual)	Read/Write	0				
Press_Dischg_AlarmSP_HI	Input	0	0.0		Float	REAL	Standard	Discharge Pressure Alarm SP - HI	Read/Write	0				
Press_Suction	Input	0	0.0		Float	REAL	Standard	Suction Pressure (actual)	Read/Write	0				
Press_Suction_AlarmSP_LO	Input	0	0.0		Float	REAL	Standard	Suction Pressure Alarm SP - LO	Read/Write	0				
Reset	Input	0	0		Decimal	BOOL	Standard	Alarm Reset Request	Read/Write	0				
ResetReady	Local	0	0		Decimal	BOOL	Standard	Device is ready for reset.	None	0				
ResetReadyOB	Local	0	0		Decimal	BOOL	Standard	Device in 'HAND' after fault - Output Bit	None	0				
ResetReadySB	Local	0	0		Decimal	BOOL	Standard	Device in 'HAND' after fault - Storage Bit	None	0				
RT_Hours	Output	0	0.0		Float	REAL	Standard	Run-Time Hours	Read Only	0				
RT_Reset	Input	0	0		Decimal	BOOL	Standard	Reset Run-time hours	Read/Write	0				
RUN_CMD	Output	0	0		Decimal	BOOL	Standard	RUN CMD Output	Read Only	0				
Running	Output	0	0		Decimal	BOOL	Standard	Running Status	Read Only	0				
Running_FB	Input	0	0		Decimal	BOOL	Standard	Running Feedback (from field device)	Read/Write	0				





RunState_delay	Local	0	{...}	{ ...}
TIMER Standard	Failed to reach Run/Stop State delay	None	0				
SuctionLO_delay	Local	0	{...}	{ ...}
TIMER Standard	Suction Pressure LO Alarm Delay	None	0				
TenthsTimer	Local	0	{...}	{ ...}
TIMER Standard	Hour-meter timer (tenths of hours)	None	0			
FlowLO_delay	Local	0	{...}	{ ...}
TIMER Standard	Flow LO Alarm Delay	None	0	

DischgHI_delay	Local	0	{...}	{ ...}
TIMER Standard	Discharge Pressure HI Alarm Delay	None	0	


        */
















    }

    public class Pump_VFD : XTO_AOI
    {
        public Pump_VFD() { }
        public Pump_VFD(XmlNode node) : base(node) { }
    }

    public class PumpData : XTO_AOI
    {
        public PumpData() { }
        public PumpData(XmlNode node) : base(node) {
        

        }

        public bool? SelectPB { get; set; }
        public bool? SelectONS { get; set; }
        public bool? Active { get; set; }
        public bool? MasterStop { get; set; }
        public Pump_1Spd? Control { get; set; }

    }

    public class PumpVData : XTO_AOI
    {
        public PumpVData() { }
        public PumpVData(XmlNode node) : base(node) { }

        public bool? SelectPB { get; set; }
        public bool? SelectONS { get; set; }
        public bool? Active { get; set; }
        public bool? MasterStop { get; set; }
        public Pump_VFD? Control { get; set; }
    }
}
