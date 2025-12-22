using System.Xml;

namespace CnE2PLC.PLC.XTO;

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

    /// <summary>
    /// AOI Values
    /// </summary>
    #region Tag Values
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
    #endregion

    /// <summary>
    /// These are used to count how many time something is used in the program.
    /// </summary>
    #region Tag Counts
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
    public int Raw_Count { get; set; } = 0;
    #endregion

    private string AlarmsText
    {
        get
        {
            string c = string.Empty;
            if (Sim == true) c += "Input is Simmed.\n";
            if (true == HiHiAlarm) c += "Input has a HiHi Alarm.\n";
            if (true == HiAlarm) c += "Input has a Hi Alarm.\n";
            if (true == LoAlarm) c += "Input has a Lo Alarm.\n";
            if (true == LoLoAlarm) c += "Input has a LoLo Alarm.\n";
            if (true == BadPVAlarm) c += "Input has a Bad PV Alarm.\n";
            if (InUse == false) c += "Not In Use.\n";
            if (AOICalled == false) c += "AOI Not Called.\n";
            if (AOICalls > 1) c += "AOI called more then once.\n";
            if (References == 0) c += "Not used in Program. SCADA Tag.\n";
            if (Placeholder == true) c += "Placeholder on IO.\n";
            return c;
        }

    }

    private string SetpointsText
    {
        get
        {
            string c = string.Empty;
            if (HiHiEnable == true) c += $"HiHi SP: {HiHiSP} {Cfg_EU} On Delay: {Cfg_HiHiOnTmr} sec. Off Delay: {Cfg_HiHiOffTmr} sec. Auto Ack: {HiHiAutoAck}\n";
            if (HiEnable == true) c += $"Hi SP:   {HiSP} {Cfg_EU} On Delay: {Cfg_HiOnTmr} sec. Off Delay: {Cfg_HiOffTmr} sec. Auto Ack: {HiAutoAck}\n";
            if (LoEnable == true) c += $"Lo SP:   {LoSP} {Cfg_EU} On Delay: {Cfg_LoOnTmr} sec. Off Delay: {Cfg_LoOffTmr} sec. Auto Ack: {LoAutoAck}\n";
            if (LoLoEnable == true) c += $"LoLo SP: {LoLoSP} {Cfg_EU} On Delay: {Cfg_LoLoOnTmr} sec. Off Delay: {Cfg_LoLoOffTmr} sec. Auto Ack: {LoLoAutoAck}\n";
            return c;
        }
    }

    public override string ToString()
    {
        string c = $"Name: {Name}\nPLC Tag Description: {Description}\n";
        c += $"PLC DataType: {DataType}\n";
        c += $"Min EU: {MinEU} {Cfg_EU}, Max EU: {MaxEU} {Cfg_EU}\nMin Raw: {MinRaw}, Max Raw: {MaxRaw}\n";
        c += SetpointsText;
        c += AlarmsText;
        if (IO.Length > 0)
        {
            c += "IO: ";
            c += IO;
        }
        return c;
    }

    public override bool Simmed { get { return Sim ?? false; } }
    public override bool Bypassed { get { return BypActive ?? false; } }
    public override bool Alarmed { get { return AlmCode == 0 ? false : true; } }
    public override bool NotInUse
    {
        get
        {
            if (InUse == false) return true;
            //if (AOICalled == false) return true;
            //if (Placeholder == true) return true;
            return false;
        }
    }
    /// <summary>
    /// Is this AOI using a real input.
    /// </summary>
    public override bool Placeholder
    {
        get
        {
            // input tag name contains placeholder.
            if (IO.ToLower().Contains("placeholder")) return true;

            // is set to its own raw value.
            // ( may not be a placehoder if raw is writen to somewheres else. )
            if (IO.ToLower().Contains($"{Name}.Raw".ToLower()) & ( Raw_Count - AOICalls > 1 ) ) return true;
            return false;
        }
    }

    /// <summary>
    /// Removes the counts found in the program.
    /// </summary>
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
        Raw_Count = 0;
    }



}
