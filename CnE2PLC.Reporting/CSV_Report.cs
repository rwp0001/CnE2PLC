using CnE2PLC.PLC.XTO;
using NPOI.SS.Formula.Functions;
using System.ComponentModel.DataAnnotations;

namespace CnE2PLC.Reporting;

public static class CSV_Report
{


    #region AIData



    #endregion

    #region AOData

    public static string[] ToHeaderCSVRow(AOData tag)
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

    public static string[] ToDataCSVRow(AOData tag)
    {
        string[] row = new string[20];
        int i = 0;
        row[i++] = $"{tag.Path}";
        row[i++] = $"{tag.Name}";
        row[i++] = $"{tag.IO}";
        row[i++] = $"{tag.Description}";
        row[i++] = $"{tag.Cfg_EquipID}";
        row[i++] = $"{tag.Cfg_EquipDesc}";
        row[i++] = $"{tag.Cfg_EU}";
        row[i++] = $"{tag.AOICalls}";
        row[i++] = $"{tag.References}";
        row[i++] = tag.InUse == true ? "Yes" : "No";
        row[i++] = $"{tag.Raw}";
        row[i++] = $"{tag.MinRaw}";
        row[i++] = $"{tag.MaxRaw}";
        row[i++] = $"{tag.MinEU}";
        row[i++] = $"{tag.MaxEU}";
        row[i++] = $"{tag.CV}";
        row[i++] = tag.Cfg_IncToClose == true ? "Yes" : "No";
        row[i++] = tag.Sim == true ? "Yes" : "No";
        row[i++] = $"{tag.SimCV}";
        return row;

    }

    #endregion

    #region DIData



    #endregion

    #region DOData


    #endregion

   


}
