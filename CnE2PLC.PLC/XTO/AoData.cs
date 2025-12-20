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

    #region Data Outputs
    //public static void ToHeaderRow(Excel.Range row)
    //{
    //    int i = 1;
    //    row.Cells[1, i++].Value = "Scope";
    //    row.Cells[1, i++].Value = "Tag Name";
    //    row.Cells[1, i++].Value = "IO";
    //    row.Cells[1, i++].Value = "Tag Description";
    //    row.Cells[1, i++].Value = "HMI EquipID";
    //    row.Cells[1, i++].Value = "HMI EquipDesc";
    //    row.Cells[1, i++].Value = "HMI EU";
    //    row.Cells[1, i++].Value = "AOI Calls";
    //    row.Cells[1, i++].Value = "Tag References";
    //    row.Cells[1, i++].Value = "InUse";
    //    row.Cells[1, i++].Value = "Raw";
    //    row.Cells[1, i++].Value = "Min Raw";
    //    row.Cells[1, i++].Value = "Max Raw";
    //    row.Cells[1, i++].Value = "Min EU";
    //    row.Cells[1, i++].Value = "Max EU";
    //    row.Cells[1, i++].Value = "CV";
    //    row.Cells[1, i++].Value = "Inc To Close";
    //    row.Cells[1, i++].Value = "Sim";
    //    row.Cells[1, i++].Value = "Sim CV";

    //}
    public static string[] ToHeaderCSVRow()
    {
        string[] row = new string[20];
        int i = 0;
        row[i++] = "Scope";
        row[i++] = "Tag Name";
        row[i++] = "IO";
        row[i++] = "Tag Description";
        row[i++] = "HMI EquipID";
        row[i++] = "HMI EquipDesc";
        row[i++] = "HMI EU";
        row[i++] = "AOI Calls";
        row[i++] = "Tag References";
        row[i++] = "InUse";
        row[i++] = "Raw";
        row[i++] = "Min Raw";
        row[i++] = "Max Raw";
        row[i++] = "Min EU";
        row[i++] = "Max EU";
        row[i++] = "CV";
        row[i++] = "Inc To Close";
        row[i++] = "Sim";
        row[i++] = "Sim CV";
        return row;

    }
    //public void ToDataRow(Excel.Range row)
    //{
    //    int i = 1;
    //    row.Cells[1, i++].Value = Path;
    //    row.Cells[1, i++].Value = Name;
    //    row.Cells[1, i++].Value = IO;
    //    row.Cells[1, i++].Value = Description;
    //    row.Cells[1, i++].Value = Cfg_EquipID;
    //    row.Cells[1, i++].Value = Cfg_EquipDesc;
    //    row.Cells[1, i++].Value = Cfg_EU;
    //    row.Cells[1, i++].Value = AOICalls;
    //    row.Cells[1, i++].Value = References;
    //    row.Cells[1, i++].Value = InUse == true ? "Yes" : "No";
    //    row.Cells[1, i++].Value = Raw;
    //    row.Cells[1, i++].Value = MinRaw;
    //    row.Cells[1, i++].Value = MaxRaw;
    //    row.Cells[1, i++].Value = MinEU;
    //    row.Cells[1, i++].Value = MaxEU;
    //    row.Cells[1, i++].Value = CV;
    //    row.Cells[1, i++].Value = Cfg_IncToClose == true ? "Yes" : "No";
    //    row.Cells[1, i++].Value = Sim == true ? "Yes" : "No";
    //    row.Cells[1, i++].Value = SimCV;

    //}
    public string[] ToDataCSVRow()
    {
        string[] row = new string[20];
        int i = 0;
        row[i++] = Path;
        row[i++] = Name;
        row[i++] = IO;
        row[i++] = Description;
        row[i++] = Cfg_EquipID;
        row[i++] = Cfg_EquipDesc;
        row[i++] = Cfg_EU;
        row[i++] = $"{AOICalls}";
        row[i++] = $"{References}";
        row[i++] = InUse == true ? "Yes" : "No";
        row[i++] = $"{Raw}";
        row[i++] = $"{MinRaw}";
        row[i++] = $"{MaxRaw}";
        row[i++] = $"{MinEU}";
        row[i++] = $"{MaxEU}";
        row[i++] = $"{CV}";
        row[i++] = Cfg_IncToClose == true ? "Yes" : "No";
        row[i++] = Sim == true ? "Yes" : "No";
        row[i++] = $"{SimCV}";
        return row;

    }
    #endregion



    //public void ToColumn(Excel.Range col)
    //{
    //    col.Cells[2, 1].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
    //    col.Cells[13, 1].Value = InUse == true ? "Yes" : "No";
    //    col.Cells[14, 1].Value = Name;

    //    if (Sim == true)
    //    {
    //        col.Cells[14, 1].Interior.Color = ColorTranslator.ToOle(Color.DarkRed);
    //        col.Cells[14, 1].Font.Color = ColorTranslator.ToOle(Color.White);
    //    }

    //    col.Cells[14, 1].AddComment(ToString());

    //}


    public override void ClearCounts() {}
}
