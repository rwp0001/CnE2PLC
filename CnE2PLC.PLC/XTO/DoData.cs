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
    //    row.Cells[1, i++].Value = "AOI Calls";
    //    row.Cells[1, i++].Value = "Tag References";
    //    row.Cells[1, i++].Value = "InUse";
    //    row.Cells[1, i++].Value = "Raw";
    //    row.Cells[1, i++].Value = "Value";
    //    row.Cells[1, i++].Value = "Sim";
    //    row.Cells[1, i++].Value = "Sim Value";

    //}
    //public void ToDataRow(Excel.Range row)
    //{
    //    int i = 1;
    //    row.Cells[1, i++].Value = Path;
    //    row.Cells[1, i++].Value = Name;
    //    row.Cells[1, i++].Value = IO;
    //    row.Cells[1, i++].Value = Description;
    //    row.Cells[1, i++].Value = Cfg_EquipID;
    //    row.Cells[1, i++].Value = Cfg_EquipDesc;
    //    row.Cells[1, i++].Value = AOICalls;
    //    row.Cells[1, i++].Value = References;
    //    row.Cells[1, i++].Value = InUse == true ? "Yes" : "No";
    //    row.Cells[1, i++].Value = Raw;
    //    row.Cells[1, i++].Value = Value;
    //    row.Cells[1, i++].Value = Sim == true ? "Yes" : "No";
    //    row.Cells[1, i++].Value = SimVal;

    //}
    #endregion

    //public void ToColumn(Excel.Range col, int TagCount = -1)
    //{
    //    col.Cells[2, 1].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
    //    col.Cells[13, 1].Value = InUse == true ? "Yes" : "No";
    //    col.Cells[14, 1].Value = Name;

    //    if(TagCount == 0)
    //    {
    //        {
    //            col.Cells[14, 1].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
    //            col.Cells[14, 1].Font.Color = ColorTranslator.ToOle(Color.Black);
    //        }
    //    }

    //    if (Sim == true)
    //    {
    //        col.Cells[14, 1].Interior.Color = ColorTranslator.ToOle(Color.DarkRed);
    //        col.Cells[14, 1].Font.Color = ColorTranslator.ToOle(Color.White);
    //    }

    //    col.Cells[14, 1].AddComment(ToString());
    //}


    public override void ClearCounts() { }
}
