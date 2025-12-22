using System.Xml;

namespace CnE2PLC.PLC.XTO;

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
