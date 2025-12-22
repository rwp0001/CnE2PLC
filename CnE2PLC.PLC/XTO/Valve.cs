using System.Xml;

namespace CnE2PLC.PLC.XTO;


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

}
