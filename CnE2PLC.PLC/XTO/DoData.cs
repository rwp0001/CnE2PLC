using System.Xml;

namespace CnE2PLC.PLC.XTO;

public class DOData : XTO_AOI
{
    public DOData() { }
    public DOData(XmlNode node) : base(node) 
    {
        if (L5K_strings.Count > 1)
        {
            Cfg_EquipID = L5K_strings[1];
            Cfg_EquipDesc = L5K_strings[0];
        }
    }

    public string AlarmText { 
        get {
            string c = string.Empty;
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
        string c = $"Name: {Name}\nPLC Tag Description: {Description}\n";
        c += $"PLC Tag DataType: {DataType}\n";
        c += AlarmText;
        if (IO.Length > 0)
        {
            c += "IO: ";
            c += IO;
        }
        return c;
    }

    //Parameters
    public bool? Value { get; set; }
    public bool? Raw { get; set; }
    public bool? Sim { get; set; }
    public int? SimVal { get; set; }

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

    public override void ClearCounts() { }
}
