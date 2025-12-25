using System.Xml;

namespace CnE2PLC.PLC.XTO;

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

    #region Tag Counts
    
    public int FTR_Count { get; set; }
    public int FTS_Count { get; set; }

    #endregion

    public override void ClearCounts() 
    {
        FTR_Count = 0;
        FTS_Count = 0;
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

    public override void ClearCounts() { }

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

    /// <summary>
    /// Auto Run Command
    /// </summary>
    public bool? CMD_AutoRun { get; set; }

    /// <summary>
    /// Manual Run Command
    /// </summary>
    public bool? CMD_ManualRun { get; set; }

    /// <summary>
    /// Manual Stop Command
    /// </summary>
    public bool? CMD_ManualStop {  get; set; }

    /// <summary>
    /// HI Dischg Pressure Alarm Enable
    /// </summary>
    public bool? EN_AlmDischgP_HI { get; set; }

    /// <summary>
    /// LO Flow Alarm Enable
    /// </summary>
    public bool? EN_AlmFlow_LO { get; set; }

    /// <summary>
    /// LO Suction Pressure Alarm Enable
    /// </summary>
    public bool? EN_AlmSuctionP_LO { get; set; }

    /// <summary>
    /// Enable Running Status Feedback
    /// </summary>
		public bool? EN_FB_Running { get; set; }

    /// <summary>
    /// Enable HOA field device I/O
    /// </summary>
    public bool? EN_HOA_device {  get; set; }

    /// <summary>
    /// Fault condition exists (roll-up of other alarms)
    /// </summary>
    public bool? Fault {  get; set; }

    /// <summary>
    /// Flow (actual)
    /// </summary>
    public float? Flow {  get; set; }

    /// <summary>
    /// Flow Alarm SP - LO
    /// </summary>
    public float? Flow_AlarmSP_LO {  get; set; }

    /// <summary>
    /// Required Interlocks for AUTO MODE operation
    /// </summary>
    public bool? Intlck_AUTO {  get; set; }

    /// <summary>
    /// Required Interlocks for MANUAL MODE operation
    /// </summary>
    public bool? Intlck_MANUAL { get; set; }

    /// <summary>
    /// Maintenance Hold Enabled
    /// </summary>
    public bool? MaintenanceHold { get; set; }

    /// <summary>
    /// Maintenance Hold Mode Release fm HMI
    /// </summary>
    public bool? MaintenanceHoldRelease { get; set; }

    /// <summary>
    /// Maintenance Hold Mode Request  fm HMI
    /// </summary>
    public bool? MaintenanceHoldReq { get; set; }

    /// <summary>
    /// Manual Mode Operations Allowed
    /// </summary>
    public bool? Manual_Allowed { get; set; }

    /// <summary>
    /// Manual Mode request fm HMI
    /// </summary>
    public bool? ManualReq { get; set; }

    /// <summary>
    /// HOA in 'AUTO' at MCC
    /// </summary>
    public bool? MCC_Auto {  get; set; }

    /// <summary>
    /// HOA in 'HAND' at MCC
    /// </summary>
    public bool? MCC_Hand { get; set; }

    /// <summary>
    /// Remote Auto Mode is Active (Remote::HMI)
    /// </summary>
		public bool? Mode_AUTO { get; set; }

    /// <summary>
    /// Remote Manual Mode is Active (Remote::HMI)
    /// </summary>
    public bool? Mode_MANUAL { get; set; }

    /// <summary>
    /// Discharge Pressure (actual)
    /// </summary>
    public float? Press_Dischg {  get; set; }

    /// <summary>
    /// Discharge Pressure Alarm SP - HI
    /// </summary>
    public float? Press_Dischg_AlarmSP_HI { get; set; }

    /// <summary>
    /// Suction Pressure (actual)
    /// </summary>
		public float? Press_Suction {  get; set; }

    /// <summary>
    /// Suction Pressure Alarm SP - LO
    /// </summary>
    public float? Press_Suction_AlarmSP_LO { get; set; }

    /// <summary>
    /// Alarm Reset Request
    /// </summary>
    public bool? Reset {  get; set; }

    /// <summary>
    /// Device is ready for reset.
    /// </summary>
    public bool? ResetReady { get; set; }

    /// <summary>
    /// Device in 'HAND' after fault - Output Bit
    /// </summary>
    public bool? ResetReadyOB { get; set; }

    /// <summary>
    /// Device in 'HAND' after fault - Storage Bit
    /// </summary>
    public bool? ResetReadySB { get; set; }

    /// <summary>
    /// Run-Time Hours
    /// </summary>
	public float? RT_Hours { get; set; }

    /// <summary>
    /// Reset Run-time hours
    /// </summary>
    public bool? RT_Reset {  get; set; }

    /// <summary>
    /// RUN CMD Output
    /// </summary>
    public bool? RUN_CMD { get; set; }

    /// <summary>
    /// Running Status
    /// </summary>
    public bool? Running { get; set; }

    /// <summary>
    /// Running Feedback (from field device)
    /// </summary>
    public bool? Running_FB { get; set; }

/*

RunState_delay	Local	0	{...}	{ ...} TIMER Standard	Failed to reach Run/Stop State delay	None	0				
SuctionLO_delay	Local	0	{...}	{ ...} TIMER Standard	Suction Pressure LO Alarm Delay	        None	0				
TenthsTimer	    Local	0	{...}	{ ...} TIMER Standard	Hour-meter timer (tenths of hours)	    None	0			
FlowLO_delay	Local	0	{...}	{ ...} TIMER Standard	Flow LO Alarm Delay	                    None	0	
DischgHI_delay	Local	0	{...}	{ ...} TIMER Standard	Discharge Pressure HI Alarm Delay	    None	0	

    */


}

public class Pump_VFD : Pump_1Spd
{
    public Pump_VFD() { }
    public Pump_VFD(XmlNode node) : base(node) { }

    public override void ClearCounts() { }

    /// <summary>
    /// Auto Mode for ON/OFF with Fixed Speed CMD
    /// </summary>
    public bool ? AutoFixedSpd {  get; set; }

    /// <summary>
    /// Auto Speed Request (0-100%)
    /// </summary>
    public float? AutoSpdReq { get; set; }

    /// <summary>
    /// Manual Speed Request (0-100%)
    /// </summary>
    public float? ManSpdReq { get; set; }

    /// <summary>
    /// Minimum allowed speed (%)
    /// </summary>
    public float? MinSpeed { get; set; }

    /// <summary>
    /// Speed Command
    /// </summary>
    public float? SpeedCMD { get; set; }

    /*

DischgHI_delay	            Local	0	{...}	{...}		TIMER	Standard	Discharge Pressure HI Alarm Delay	None	0	
FlowLO_delay	            Local	0	{...}	{...}		TIMER	Standard	Flow LO Alarm Delay	None	0	
RunState_delay	            Local	0	{...}	{...}		TIMER	Standard	Failed to reach Run/Stop State delay	None	0	
SuctionLO_delay	            Local	0	{...}	{...}		TIMER	Standard	Suction Pressure LO Alarm Delay	None	0				
TenthsTimer	                Local	0	{...}	{...}		TIMER	Standard	Hour-meter timer (tenths of hours)	None	0				

     */





}

public class PumpData : XTO_AOI
{
    public PumpData() { }
    public PumpData(XmlNode node) : base(node) {
    

    }

    public override void ClearCounts() { }

    public bool? SelectPB { get; set; }
    public bool? SelectONS { get; set; }
    public bool? Active { get; set; }
    public bool? MasterStop { get; set; }
    public Pump_1Spd? Control { get; set; }

}

public class PumpVData : XTO_AOI
{
    public PumpVData() { }
    public PumpVData(XmlNode node) : base(node) { 
    
    
    }

    public override void ClearCounts() { }

    public bool? SelectPB { get; set; }
    public bool? SelectONS { get; set; }
    public bool? Active { get; set; }
    public bool? MasterStop { get; set; }
    public Pump_VFD? Control { get; set; }
}
