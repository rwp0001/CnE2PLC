using System.Xml;

namespace CnE2PLC.PLC.XTO;

public class Intlk_8 : XTO_AOI
{
    public Intlk_8() { }

    public Intlk_8(XmlNode node) : base(node) {
        Cfg_EquipID = L5K_strings[8];
        Cfg_EquipDesc = L5K_strings[9];
        for (int i = 0; i < 7; i++) Cfg_IntlkDesc[i] = L5K_strings[i];
    }

    #region Parameters
    public bool? Intlk00 {  get; set; }
    public bool? Intlk01 { get; set; }
    public bool? Intlk02 { get; set; }
    public bool? Intlk03 { get; set; }
    public bool? Intlk04 { get; set; }
    public bool? Intlk05 { get; set; }
    public bool? Intlk06 { get; set; }
    public bool? Intlk07 { get; set; }
    public int? IntlkBypass { get; set; }
    public bool? IntlkReset { get; set; }
    public bool? AckResetAll { get; set; }
    public int? Cfg_OKState { get; set; }
    public int? Cfg_AutoAck { get; set; }
    public int? IntlkActive { get; set; }
    public int? IntlkLatched { get; set; }
    public int? IntlkFirstOut { get; set; }
    public bool? IntlkOK { get; set; }
    public bool? BypActive { get; set; }
    public bool? ResetRdy { get; set; }
    public bool? FirstOut { get; set; }
    public string[] Cfg_IntlkDesc { get; set; } = new string[8];
    #endregion

    private string AlarmText
    {
        get
        {
            string c = string.Empty;
            c += IntlkOK == false ? "Interlock is tripped.\n" : "Interlock is OK.\n";
            c += BypActive == true ? "A Bypass is Active.\n" : "";
            if (InUse == false) c += "Not In Use.\n";
            if (AOICalled == false) c += "AOI Not Called.\n";
            if (References == 0) c += "Not used in Program. SCADA Tag.\n";
            return c;
        }
    }

    public override string ToString()
    {
        string c = $"Name: {Name}\n";
        c += $"PLC Tag Description: {Description}\n";
        c += $"PLC DataType: {DataType}\n";
        c += AlarmText;
        return c;
    }

    public override void ClearCounts() { }
}

public class Intlk_16 : Intlk_8
{
    public Intlk_16() { }

    public Intlk_16(XmlNode node) : base(node) {
        Cfg_EquipID = L5K_strings[16];
        Cfg_EquipDesc = L5K_strings[17];
        Cfg_IntlkDesc = new string[16];
        for (int i = 0; i < 15; i++) Cfg_IntlkDesc[i] = L5K_strings[i];
    }

    public bool? Intlk08 { get; set; }
    public bool? Intlk09 { get; set; }
    public bool? Intlk10 { get; set; }
    public bool? Intlk11 { get; set; }
    public bool? Intlk12 { get; set; }
    public bool? Intlk13 { get; set; }
    public bool? Intlk14 { get; set; }
    public bool? Intlk15 { get; set; }

}

public class IntlkESD : XTO_AOI
{
    public IntlkESD() { }
    public IntlkESD(XmlNode node) : base(node) {
        //throw new NotImplementedException("IntlkESD not ready");
    }

    #region Parameters
    public bool? IN {  get; set; }
    public bool? Bypass { get; set; }
    public bool? OKState { get; set; }
    public string? Desc { get; set; }
    public bool? Active { get; set; }
    public bool? Latched { get; set; }
    public bool? FirstOut { get; set; }
    public bool? StatusPrev { get; set; }
    #endregion


}

/// <summary>
/// Interlock w/ 64 inputs & First-Out latch
/// </summary>
public class Intlk_64 : XTO_AOI
{
    public Intlk_64() { }

    public Intlk_64(XmlNode node) : base(node)
    {
        Cfg_EquipID = L5K_strings[1];
        Cfg_EquipDesc = L5K_strings[2];
        FODesc = L5K_strings[4];
        BlankSTR = L5K_strings[5];



    }

    #region Parameters
    /// <summary>
    /// Interlock Inputs
    /// </summary>
    //IntlkESD[]? IntlkIO;

    /// <summary>
    /// 1=At Least One Interlock Bypass is Active
    /// </summary>
    public bool? BypActiveAny { get; set; }

    ///<summary>
    /// Overall Interlock Status(1=OK to run, 0=Stop)
    /// </summary>
    public bool? IntlkOK { get; set; }

    /// <summary>
    /// 1=Ready to receive Reset Command (reset required)
    /// </summary>
    public bool? ResetRdy { get; set; }

    /// <summary>
    /// 1= Interlock First Out is Active
    /// </summary>
    public bool? FirstOut { get; set; }

    /// <summary>
    /// Reset Latched Interlocks and First-Out
    /// </summary>
    public bool? IntlkReset { get; set; }

    /// <summary>
    /// Master Alarm Ack/Reset
    /// </summary>
    public bool? AckResetAll { get; set; }

    /// <summary>
    /// Bypass Reset
    /// </summary>
    public bool? BypResetAll { get; set; }

    #endregion

    #region Local Tags
    /// <summary>
    /// The firstout string.
    /// </summary>
    public string? FODesc { get; set; }
    /// <summary>
    /// String to be displayed when there is not a firstout.
    /// </summary>
    public string? BlankSTR { get; set; }
    #endregion

    public override void ClearCounts() { }

    private string AlarmText
    {
        get
        {
            string c = string.Empty;
            c += IntlkOK == false ? "Interlock is tripped.\n" : "Interlock is OK.\n";
            c += BypActiveAny == true ? "A Bypass is Active.\n" : "";
            if (InUse == false) c += "Not In Use.\n";
            if (AOICalled == false) c += "AOI Not Called.\n";
            if (References == 0) c += "Not used in Program. SCADA Tag.\n";
            return c;
        }
    }

    public override string ToString()
    {
        string c = $"Name: {Name}\n";
        c += $"PLC Tag Description: {Description}\n";
        c += $"PLC DataType: {DataType}\n";
        c += AlarmText;
        return c;
    }

}

public class Intlk_64V2 : Intlk_64
{
    public Intlk_64V2() { }
    public Intlk_64V2(XmlNode node) : base(node) { }
}

public class Intlk_64V3 : Intlk_64 {
    public Intlk_64V3() { }
    public Intlk_64V3(XmlNode node) : base(node){}
}
