using System.Xml;

namespace CnE2PLC.PLC.XTO;

public class AOData : XTO_AOI
{
    public AOData() { }

    public AOData(XmlNode node) : base(node) {
        if (L5K_strings.Count > 2)
        {
            Cfg_EquipID = L5K_strings[1];
            Cfg_EquipDesc = L5K_strings[0];
            Cfg_EU = L5K_strings[2];
        }
    }

    public string AlarmText
    {
        get
        {
            string c = string.Empty;
            if (Cfg_IncToClose == true) c += "Output is Reversed.\n";
            if (Sim == true) c += "Output is Simmed.\n";
            if (InUse == false) c += "Not In Use.\n";
            if (AOICalled == false) c += "AOI Not Called.\n";
            if (AOICalls > 1) c += "AOI called more then once.\n";
            if (References == 0) c += "Not used in Program. SCADA Tag.\n";
            if (Placeholder == true) c += "Placeholder on IO.\n";
            return c;
        }
    }

    public override string ToString()
    {
        string c = $"Name: {Name}\n";
        c += $"PLC Tag Description: {Description}\n";
        c += $"PLC Tag DataType: {DataType}\n";
        c += $"Min EU: {MinEU} {Cfg_EU}, Max EU: {MaxEU} {Cfg_EU}\n";
        c += $"Min Raw: {MinRaw}, Max Raw: {MaxRaw}\n";
        c += AlarmText;
        if (IO.Length > 0)
        {
            c += "IO: ";
            c += IO;
        }
        return c;
    }

    // Parameters
    public float? CV { get; set; }
    public float? MinEU { get; set; }
    public float? MaxEU { get; set; }
    public float? MinRaw { get; set; }
    public float? MaxRaw { get; set; }
    public float? Raw { get; set; }
    public bool? Sim { get; set; }
    public float? SimCV { get; set; }
    public bool? Cfg_IncToClose { get; set; }
    public bool? SimReset { get; set; }
    public bool? SimActive { get; set; }

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
    public override bool Placeholder
    {
        get
        {
            if (IO.ToLower().Contains("placeholder")) return true;
            if (IO.ToLower().Contains($"{Name}.Raw".ToLower())) return true;
            return false;
        }
    }

    // Local Tags
    public string? Cfg_EU { get; set; }

    public override void ClearCounts() {}
}
