using CnE2PLC.Helpers;
using System.Xml;

namespace CnE2PLC.PLC.XTO;


public class Bat_TnkCfgXTO { }

public class TankModeData { }

public class Bat_TnkLvlSPRtrv { }

public class Bat_TnkLvlSPSave { }

public class XTO_AnaSP_HR : XTO_AOI 
{

    public XTO_AnaSP_HR() { }

    public XTO_AnaSP_HR(XmlNode node) { }

    #region Tag Values
    /// <summary>
    /// HiHi Alarm Setpoint Limit(EU)
    /// </summary>
    public float? HiHiSPLimit { get; set; }

    /// <summary>
    /// HiHi Alarm Setpoint(EU)
    /// </summary>
    public float? HiHiSP { get; set; }

    /// <summary>
    /// Hi Alarm Setpoint(EU)
    /// </summary>
    public float? HiSP { get; set; }

    /// <summary>
    /// Lo Alarm Setpoint(EU)
    /// </summary>
    public float? LoSP { get; set; }

    /// <summary>
    /// LoLo Alarm Setpoint(EU)
    /// </summary>
    public float? LoLoSP { get; set; }

    /// <summary>
    /// LoLo Alarm Setpoint Limit(EU)
    /// </summary>
    public float? LoLoSPLimit { get; set; }

    /// <summary>
    /// HiHi, Hi, Lo, & LoLo Reset Deadband Value(EU)
    /// </summary>
    public float? ResetDeadband { get; set; }

    /// <summary>
    /// PV Out of Range Alarm Processing Enabled
    /// </summary>
    public bool? BadPVEnable { get; set; }

    /// <summary>
    /// PV Out of Range Alarm Auto Acknowledge Enabled
    /// </summary>
    public bool? BadPVAutoAck { get; set; }

    /// <summary>
    /// HiHi Alarm Processing Enabled
    /// </summary>
    public bool? HiHiEnable { get; set; }

    /// <summary>
    /// HiHi Alarm Auto Acknowledge Enabled
    /// </summary>
    public bool? HiHiAutoAck { get; set; }

    /// <summary>
    /// Hi Alarm Processing Enabled
    /// </summary>
    public bool? HiEnable { get; set; }

    /// <summary>
    /// Hi Alarm Auto Acknowledge Enabled
    /// </summary>
    public bool? HiAutoAck { get; set; }

    /// <summary>
    /// Lo Alarm Processing Enabled
    /// </summary>
    public bool? LoEnable { get; set; }

    /// <summary>
    /// Lo Alarm Auto Acknowledge Enabled
    /// </summary>
    public bool? LoAutoAck { get; set; }

    /// <summary>
    /// LoLo Alarm Processing Enabled
    /// </summary>
    public bool? LoLoEnable { get; set; }

    /// <summary>
    /// LoLo Alarm Auto Acknowledge Enabled
    /// </summary>
    public bool? LoLoAutoAck { get; set; }

    #endregion
}

public class TankData : XTO_AOI
{
    public TankData() { }

    public TankData(XmlNode node) : base(node)
    {
        foreach (XmlNode childNode in node.ChildNodes)
        {
            if (childNode.Name != "Data") continue;
            if (childNode.GetNamedAttributeItemInnerText("Format") != "Decorated" ) continue; // skip L5Ks

            XmlNode fc = childNode.FirstChild ?? XMLHelper.CreateGenericXmlNode();
            foreach (XmlNode node2 in fc.ChildNodes)
            {
                if (!node2.AttributeExists("Name", out XmlNode n)) continue;
                
                if (n.InnerText == "Level")
                {
                    Level = new AIData(node2);
                    Level.Cfg_EquipID = L5K_strings[12];
                    Level.Cfg_EquipDesc = L5K_strings[11];
                    Level.Cfg_EU = L5K_strings[13];
                    Level.Name = $"{Name}.Level";
                }

                if (n.InnerText == "LIT_O")
                {
                    LIT_O = new AIData(node2)
                    {
                        Cfg_EquipID = L5K_strings[2],
                        Cfg_EquipDesc = L5K_strings[1],
                        Cfg_EU = L5K_strings[3],
                        Name = $"{Name}.LIT_O"
                    };
                }

                if (n.InnerText == "LIT_W")
                {
                    LIT_W = new AIData(node2)
                    {
                        Cfg_EquipID = L5K_strings[7],
                        Cfg_EquipDesc = L5K_strings[6],
                        Cfg_EU = L5K_strings[8],
                        Name = $"{Name}.LIT_W"
                    };
                }
            }
        }
    }

    public override void ClearCounts() {
        Level?.ClearCounts();
        LIT_O?.ClearCounts();
        LIT_W?.ClearCounts();
    }

    #region Tag Values
    /// <summary>
    /// Mode
    /// </summary>
    public Bat_TnkCfgXTO? Mode { get; set; }

    /// <summary>
    /// Mode 1-shots
    /// </summary>
    public TankModeData? ModeOS { get; set; }

    /// <summary>
    /// Level Xmiter 'Oil' (HART fraction)
    /// </summary>
    public AIData? LIT_O {  get; set; }

    /// <summary>
    /// Level Xmiter 'Water' (HART fraction)
    /// </summary>
    public AIData? LIT_W { get; set; }

    /// <summary>
    /// Selected Level (Absolute PV)
    /// </summary>
    public AIData? Level { get; set; }

    /// <summary>
    /// Alarm SP/Confg storage for Run Mode - Sensor A
    /// </summary>
    public XTO_AnaSP_HR? AlmSPHldg_RunA {  get; set; }

    /// <summary>
    /// Alarm SP/Confg storage for Run Mode - Sensor B
    /// </summary>
    public XTO_AnaSP_HR? AlmSPHldg_RunB { get; set; }

    /// <summary>
    /// Alarm SP/Confg storage for Run Mode - Sensor C
    /// </summary>
    public XTO_AnaSP_HR? AlmSPHldg_RunC { get; set; }

    /// <summary>
    /// Alarm SP/Confg storage for Overflow Mode - Sensor A
    /// </summary>
    public XTO_AnaSP_HR? AlmSPHldg_OFlwA {  get; set; }

    /// <summary>
    /// Alarm SP/Confg storage for Overflow Mode - Sensor B
    /// </summary>
    public XTO_AnaSP_HR? AlmSPHldg_OFlwB {  get; set; }

    /// <summary>
    /// Alarm SP/Confg storage for Overflow Mode - Sensor C
    /// </summary>
    public XTO_AnaSP_HR? AlmSPHldg_OFlwC {  get; set; }

    /// <summary>
    /// SP Retrieval Logic
    /// </summary>
    public Bat_TnkLvlSPRtrv? SPRetrvLogic {  get; set; }

    /// <summary>
    /// SP Storage Logic
    /// </summary>
    public Bat_TnkLvlSPSave? SPSaveLogic {  get; set; }

    #endregion
}
