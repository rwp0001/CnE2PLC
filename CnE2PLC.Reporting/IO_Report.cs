using CnE2PLC.PLC.XTO;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;

namespace CnE2PLC.Reporting;

public static class IO_Report
{

    public static void CreateIOReport()
    {
        //Excel.Application? excelApp = null;
        //try
        //{

        //    excelApp = new Excel.Application();
        //    excelApp.Visible = false;
        //    excelApp.ScreenUpdating = false;

        //    if (Settings.Default.Debug)
        //    {
        //        excelApp.Visible = true;
        //        excelApp.ScreenUpdating = true;
        //        excelApp.Top = 0;
        //        excelApp.Left = 0;
        //        excelApp.Height = Screen.PrimaryScreen.WorkingArea.Height;
        //        excelApp.Width = Screen.PrimaryScreen.WorkingArea.Width;
        //    }


        //    Excel.Workbook Workbook = excelApp.Application.Workbooks.Add();
        //    Excel.Worksheet Sheet = Workbook.ActiveSheet;
        //    excelApp.Calculation = Excel.XlCalculation.xlCalculationManual;
        //    Sheet.Name = "AiData";
        //    CreateAiReport(Sheet);

        //    Sheet = Workbook.Sheets.Add();
        //    Sheet.Name = "DiData";
        //    Sheet.Activate();
        //    CreateDiReport(Sheet);

        //    Sheet = Workbook.Sheets.Add();
        //    Sheet.Name = "AoData";
        //    Sheet.Activate();
        //    CreateAoReport(Sheet);

        //    Sheet = Workbook.Sheets.Add();
        //    Sheet.Name = "DoData";
        //    Sheet.Activate();
        //    CreateDoReport(Sheet);

        //    Sheet = Workbook.Sheets.Add();
        //    Sheet.Name = "Intlk_8";
        //    Sheet.Activate();
        //    CreateIntlk8Report(Sheet);

        //    Sheet = Workbook.Sheets.Add();
        //    Sheet.Name = "Intlk_16";
        //    Sheet.Activate();
        //    CreateIntlk16Report(Sheet);

        //    excelApp.Visible = true;
        //    excelApp.ScreenUpdating = true;
        //    excelApp.Calculation = Excel.XlCalculation.xlCalculationAutomatic;

        //}
        //catch (Exception ex)
        //{
        //    if (excelApp != null)
        //    {
        //        excelApp.Visible = true;
        //        excelApp.ScreenUpdating = true;
        //        excelApp.Calculation = Excel.XlCalculation.xlCalculationAutomatic;
        //    }
        //    var result = MessageBox.Show(ex.Message, "Exception",
        //                    MessageBoxButtons.OK,
        //                    MessageBoxIcon.Error);
        //    if (result == DialogResult.OK) return;
        //}
    }


    #region AIData

    public static void CreateAiReport()//Excel.Worksheet Sheet)
    {
        //try
        //{
        //    int RowOffset = 1; // first row to use.
        //    AIData.ToHeaderRow(Sheet.Rows[RowOffset++]);

        //    foreach (XTO_AOI tag in AllTags)
        //    {
        //        try
        //        {
        //            if (tag.DataType != "AIData") continue;
        //            AIData AI = (AIData)tag;
        //            AI.ToDataRow(Sheet.Rows[RowOffset++]);
        //        }
        //        catch (Exception ex)
        //        {
        //            var r = MessageBox.Show($"Error: {ex.Message}\nTage Name: {tag.Name} ", $"Tag Exception", MessageBoxButtons.OK);
        //        }

        //    }

        //}
        //catch (Exception e)
        //{
        //    var result = MessageBox.Show(e.Message, "Exception",
        //                    MessageBoxButtons.OK,
        //                    MessageBoxIcon.Error);
        //    if (result == DialogResult.OK) return;

        //}
    }

    public static void ToHeaderRow(AIData tag)// (Excel.Range row)
    {
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
        //    row.Cells[1, i++].Value = "PV";
        //    row.Cells[1, i++].Value = "HiHi Enable";
        //    row.Cells[1, i++].Value = "HiHi Auto Ack";
        //    row.Cells[1, i++].Value = "HiHi SP Limit";
        //    row.Cells[1, i++].Value = "HiHi SP";
        //    row.Cells[1, i++].Value = "HiHi On Time";
        //    row.Cells[1, i++].Value = "HiHi Off Time";
        //    row.Cells[1, i++].Value = "HiHi Dly Time";
        //    row.Cells[1, i++].Value = "HiHi SD Time";
        //    row.Cells[1, i++].Value = "HiHi Alarm";
        //    row.Cells[1, i++].Value = "HiHi Count";
        //    row.Cells[1, i++].Value = "HSD Count";
        //    row.Cells[1, i++].Value = "Hi Enable";
        //    row.Cells[1, i++].Value = "Hi Auto Ack";
        //    row.Cells[1, i++].Value = "Hi SP";
        //    row.Cells[1, i++].Value = "Hi On Time";
        //    row.Cells[1, i++].Value = "Hi Off Time";
        //    row.Cells[1, i++].Value = "Hi Dly Time";
        //    row.Cells[1, i++].Value = "Hi Alarm";
        //    row.Cells[1, i++].Value = "Hi Count";
        //    row.Cells[1, i++].Value = "Lo Enable";
        //    row.Cells[1, i++].Value = "Lo Auto Ack";
        //    row.Cells[1, i++].Value = "Lo SP";
        //    row.Cells[1, i++].Value = "Lo On Time";
        //    row.Cells[1, i++].Value = "Lo Off Time";
        //    row.Cells[1, i++].Value = "Lo Dly Time";
        //    row.Cells[1, i++].Value = "Lo Alarm";
        //    row.Cells[1, i++].Value = "Lo Count";
        //    row.Cells[1, i++].Value = "LoLo Enable";
        //    row.Cells[1, i++].Value = "LoLo Auto Ack";
        //    row.Cells[1, i++].Value = "LoLo SP Limit";
        //    row.Cells[1, i++].Value = "LoLo SP";
        //    row.Cells[1, i++].Value = "LoLo On Time";
        //    row.Cells[1, i++].Value = "LoLo Off Time";
        //    row.Cells[1, i++].Value = "LoLo Dly Time";
        //    row.Cells[1, i++].Value = "LoLo SD Time";
        //    row.Cells[1, i++].Value = "LoLo Alarm";
        //    row.Cells[1, i++].Value = "LoLo Count";
        //    row.Cells[1, i++].Value = "LSD Count";
        //    row.Cells[1, i++].Value = "Sim";
        //    row.Cells[1, i++].Value = "Sim PV";
        //    row.Cells[1, i++].Value = "Bad PV Enable";
        //    row.Cells[1, i++].Value = "Bad PV Auto Ack";
        //    row.Cells[1, i++].Value = "Bad PV Alarm";
    }

    public static void ToDataRow(AIData tag)//Excel.Range row)
    {
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
        //    row.Cells[1, i++].Value = PV;

        //    row.Cells[1, i++].Value = HiHiEnable == true ? "Yes" : "No";
        //    row.Cells[1, i++].Value = HiHiAutoAck == true ? "Yes" : "No";
        //    row.Cells[1, i++].Value = HiHiSPLimit;
        //    row.Cells[1, i++].Value = HiHiSP;
        //    row.Cells[1, i++].Value = Cfg_HiHiOnTmr;
        //    row.Cells[1, i++].Value = Cfg_HiHiOffTmr;
        //    row.Cells[1, i++].Value = Cfg_HiHiDlyTmr;
        //    row.Cells[1, i++].Value = Cfg_HiHiSDTmr;
        //    row.Cells[1, i++].Value = HiHiAlarm == true ? "Alarm" : "Ok";
        //    row.Cells[1, i++].Value = HiHi_Count;
        //    row.Cells[1, i++].Value = HSD_Count;

        //    row.Cells[1, i++].Value = HiEnable == true ? "Yes" : "No";
        //    row.Cells[1, i++].Value = HiAutoAck == true ? "Yes" : "No";
        //    row.Cells[1, i++].Value = HiSP;
        //    row.Cells[1, i++].Value = Cfg_HiOnTmr;
        //    row.Cells[1, i++].Value = Cfg_HiOffTmr;
        //    row.Cells[1, i++].Value = Cfg_HiDlyTmr;
        //    row.Cells[1, i++].Value = HiAlarm == true ? "Alarm" : "Ok";
        //    row.Cells[1, i++].Value = Hi_Count;

        //    row.Cells[1, i++].Value = LoEnable == true ? "Yes" : "No";
        //    row.Cells[1, i++].Value = LoAutoAck == true ? "Yes" : "No";
        //    row.Cells[1, i++].Value = LoSP;
        //    row.Cells[1, i++].Value = Cfg_LoOnTmr;
        //    row.Cells[1, i++].Value = Cfg_LoOffTmr;
        //    row.Cells[1, i++].Value = Cfg_LoDlyTmr;
        //    row.Cells[1, i++].Value = LoAlarm == true ? "Alarm" : "Ok";
        //    row.Cells[1, i++].Value = Lo_Count;

        //    row.Cells[1, i++].Value = LoLoEnable == true ? "Yes" : "No";
        //    row.Cells[1, i++].Value = LoLoAutoAck == true ? "Yes" : "No";
        //    row.Cells[1, i++].Value = LoLoSPLimit;
        //    row.Cells[1, i++].Value = LoLoSP;
        //    row.Cells[1, i++].Value = Cfg_LoLoOnTmr;
        //    row.Cells[1, i++].Value = Cfg_LoLoOffTmr;
        //    row.Cells[1, i++].Value = Cfg_LoLoDlyTmr;
        //    row.Cells[1, i++].Value = Cfg_LoLoSDTmr;
        //    row.Cells[1, i++].Value = LoLoAlarm == true ? "Alarm" : "Ok";
        //    row.Cells[1, i++].Value = LoLo_Count;
        //    row.Cells[1, i++].Value = LSD_Count;

        //    row.Cells[1, i++].Value = Sim == true ? "Yes" : "No";
        //    row.Cells[1, i++].Value = SimPV;

        //    row.Cells[1, i++].Value = BadPVEnable == true ? "Yes" : "No";
        //    row.Cells[1, i++].Value = BadPVAutoAck == true ? "Yes" : "No";
        //    row.Cells[1, i++].Value = BadPVAlarm == true ? "Alarm" : "Ok";

    }

    #endregion

    #region AOData

    public static void CreateAoReport()//Excel.Worksheet Sheet)
    {
        //try
        //{
        //    int RowOffset = 1; // first row to use.
        //    AOData.ToHeaderRow(Sheet.Rows[RowOffset++]);

        //    foreach (XTO_AOI tag in AllTags)
        //    {
        //        try
        //        {
        //            if (tag.DataType != "AOData") continue;
        //            AOData I = (AOData)tag;
        //            I.ToDataRow(Sheet.Rows[RowOffset++]);
        //        }
        //        catch (Exception ex)
        //        {
        //            var r = MessageBox.Show($"Error: {ex.Message}\nTage Name: {tag.Name} ", $"Tag Exception", MessageBoxButtons.OK);
        //        }

        //    }
        //}
        //catch (Exception e)
        //{
        //    var result = MessageBox.Show(e.Message, "Exception",
        //                    MessageBoxButtons.OK,
        //                    MessageBoxIcon.Error);
        //    if (result == DialogResult.OK) return;

        //}

    }

    public static void ToIOReportHeaderRow(AOData tag, IRow row)
    {
        int i = 1;
        row.GetCell(i++).SetCellValue("Scope");
        row.GetCell(i++).SetCellValue("Tag Name");
        row.GetCell(i++).SetCellValue("IO");
        row.GetCell(i++).SetCellValue("Tag Description");
        row.GetCell(i++).SetCellValue("HMI EquipID");
        row.GetCell(i++).SetCellValue("HMI EquipDesc");
        row.GetCell(i++).SetCellValue("HMI EU");
        row.GetCell(i++).SetCellValue("AOI Calls");
        row.GetCell(i++).SetCellValue("Tag References");
        row.GetCell(i++).SetCellValue("InUse");
        row.GetCell(i++).SetCellValue("Raw");
        row.GetCell(i++).SetCellValue("Min Raw");
        row.GetCell(i++).SetCellValue("Max Raw");
        row.GetCell(i++).SetCellValue("Min EU");
        row.GetCell(i++).SetCellValue("Max EU");
        row.GetCell(i++).SetCellValue("CV");
        row.GetCell(i++).SetCellValue("Inc To Close");
        row.GetCell(i++).SetCellValue("Sim");
        row.GetCell(i++).SetCellValue("Sim CV");

    }

    public static void ToIOReportDataRow(AOData tag, IRow row)
    {
        int i = 1;
        row.GetCell(i++).SetCellValue(tag.Path);
        row.GetCell(i++).SetCellValue(tag.Name);
        row.GetCell(i++).SetCellValue(tag.IO);
        row.GetCell(i++).SetCellValue(tag.Description);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc);
        row.GetCell(i++).SetCellValue(tag.Cfg_EU);
        row.GetCell(i++).SetCellValue(tag.AOICalls);
        row.GetCell(i++).SetCellValue(tag.References);
        row.GetCell(i++).SetCellValue(tag.InUse == true ? "Yes" : "No");
        row.GetCell(i++).SetCellValue($"{tag.Raw}");
        row.GetCell(i++).SetCellValue($"{tag.MinRaw}");
        row.GetCell(i++).SetCellValue($"{tag.MaxRaw}");
        row.GetCell(i++).SetCellValue($"{tag.MinEU}");
        row.GetCell(i++).SetCellValue($"{tag.MaxEU}");
        row.GetCell(i++).SetCellValue($"{tag.CV}");
        row.GetCell(i++).SetCellValue(tag.Cfg_IncToClose == true ? "Yes" : "No");
        row.GetCell(i++).SetCellValue(tag.Sim == true ? "Yes" : "No");
        row.GetCell(i++).SetCellValue($"{tag.SimCV}");

    }

    #endregion

    #region DIData

    public static void ToHeaderRow(DIData tag, IRow row)
    {
        int i = 1;
        row.GetCell(i++).SetCellValue("Scope");
        row.GetCell(i++).SetCellValue("Tag Name");
        row.GetCell(i++).SetCellValue("IO");
        row.GetCell(i++).SetCellValue("Tag Description");
        row.GetCell(i++).SetCellValue("HMI EquipID");
        row.GetCell(i++).SetCellValue("HMI EquipDesc");
        row.GetCell(i++).SetCellValue("AOI Calls");
        row.GetCell(i++).SetCellValue("Tag References");
        row.GetCell(i++).SetCellValue("InUse");
        row.GetCell(i++).SetCellValue("Raw");
        row.GetCell(i++).SetCellValue("Value");
        row.GetCell(i++).SetCellValue("Alarm Enable");
        row.GetCell(i++).SetCellValue("Alarm Auto Ack");
        row.GetCell(i++).SetCellValue("Alarm State");
        row.GetCell(i++).SetCellValue("Alarm On Time");
        row.GetCell(i++).SetCellValue("Alarm Off Time");
        row.GetCell(i++).SetCellValue("Alarm Dly Time");
        row.GetCell(i++).SetCellValue("Alarm SD Time");
        row.GetCell(i++).SetCellValue("Alarm");
        row.GetCell(i++).SetCellValue("Alarm Count");
        row.GetCell(i++).SetCellValue("SD Count");
        row.GetCell(i++).SetCellValue("Sim");
        row.GetCell(i++).SetCellValue("Sim Value");
    }

    public static void ToDataRow(DIData tag, IRow row)
    {
        int i = 1;
        row.GetCell(i++).SetCellValue(tag.Path);
        row.GetCell(i++).SetCellValue(tag.Name);
        row.GetCell(i++).SetCellValue(tag.IO);
        row.GetCell(i++).SetCellValue(tag.Description);

        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc);

        row.GetCell(i++).SetCellValue(tag.AOICalls);
        row.GetCell(i++).SetCellValue(tag.References);

        row.GetCell(i++).SetCellValue(tag.InUse == true ? "Yes" : "No");

        row.GetCell(i++).SetCellValue(tag.Raw == true ? "On" : "Off");
        row.GetCell(i++).SetCellValue(tag.Value == true ? "On" : "Off");

        row.GetCell(i++).SetCellValue(tag.AlmEnable == true ? "Yes" : "No");
        row.GetCell(i++).SetCellValue(tag.AlmAutoAck == true ? "Yes" : "No");
        row.GetCell(i++).SetCellValue(tag.AlmState == 1 ? "On" : "Off");
        row.GetCell(i++).SetCellValue($"{tag.Cfg_AlmOnTmr}");
        row.GetCell(i++).SetCellValue($"{tag.Cfg_AlmOffTmr}");
        row.GetCell(i++).SetCellValue($"{tag.Cfg_AlmDlyTmr}");
        row.GetCell(i++).SetCellValue($"{tag.Cfg_SDDlyTmr}");
        row.GetCell(i++).SetCellValue(tag.  Alarm == true ? "Alarm" : "Ok");
        row.GetCell(i++).SetCellValue(tag.Alm_Count);
        row.GetCell(i++).SetCellValue(tag.SD_Count);

        row.GetCell(i++).SetCellValue(tag.Sim == true ? "Yes" : "No");
        row.GetCell(i++).SetCellValue(tag.SimValue == true ? "On" : "Off");


    }

    #endregion

    #region DOData

    public static void ToHeaderRow(DOData tag, IRow row)
    {
        int i = 0;
        row.GetCell(i++).SetCellValue("Scope");
        row.GetCell(i++).SetCellValue("Tag Name");
        row.GetCell(i++).SetCellValue("IO");
        row.GetCell(i++).SetCellValue("Tag Description");
        row.GetCell(i++).SetCellValue("HMI EquipID");
        row.GetCell(i++).SetCellValue("HMI EquipDesc");
        row.GetCell(i++).SetCellValue("AOI Calls");
        row.GetCell(i++).SetCellValue("Tag References");
        row.GetCell(i++).SetCellValue("InUse");
        row.GetCell(i++).SetCellValue("Raw");
        row.GetCell(i++).SetCellValue("Value");
        row.GetCell(i++).SetCellValue("Sim");
        row.GetCell(i++).SetCellValue("Sim Value");
    }

    public static void ToDataRow(DOData tag, IRow row)
    {
        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Path);
        row.GetCell(i++).SetCellValue(tag.Name);
        row.GetCell(i++).SetCellValue(tag.IO);
        row.GetCell(i++).SetCellValue(tag.Description);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc);
        row.GetCell(i++).SetCellValue(tag.AOICalls);
        row.GetCell(i++).SetCellValue(tag.References);
        row.GetCell(i++).SetCellValue(tag.InUse == true ? "Yes" : "No");
        row.GetCell(i++).SetCellValue(tag.Raw == true ? "On" : "Off");
        row.GetCell(i++).SetCellValue(tag.Value == true ? "On" : "Off");
        row.GetCell(i++).SetCellValue(tag.Sim == true ? "Yes" : "No");
        row.GetCell(i++).SetCellValue(tag.SimVal == 1 ? "On" : "Off");
    }

    #endregion



    public static void CreateDiReport()//Excel.Worksheet Sheet)
    {
        //try
        //{
        //    int RowOffset = 1; // first row to use.
        //    DIData.ToHeaderRow(Sheet.Rows[RowOffset++]);

        //    foreach (XTO_AOI tag in AllTags)
        //    {
        //        try
        //        {
        //            if (tag.DataType != "DIData") continue;
        //            DIData DI = (DIData)tag;
        //            DI.ToDataRow(Sheet.Rows[RowOffset++]);
        //        }
        //        catch (Exception ex)
        //        {
        //            var r = MessageBox.Show($"Error: {ex.Message}\nTage Name: {tag.Name} ", $"Tag Exception", MessageBoxButtons.OK);
        //        }

        //    }

        //}
        //catch (Exception e)
        //{
        //    var result = MessageBox.Show(e.Message, "Exception",
        //                    MessageBoxButtons.OK,
        //                    MessageBoxIcon.Error);
        //    if (result == DialogResult.OK) return;

        //}
    }

    public static void CreateIntlk8Report()//Excel.Worksheet Sheet)
    {
        //try
        //{
        //    int RowOffset = 1; // first row to use.
        //    Intlk_8.ToHeaderRow(Sheet.Rows[RowOffset++]);

        //    foreach (XTO_AOI tag in AllTags)
        //    {
        //        try
        //        {
        //            if (tag.DataType != "Intlk_8") continue;
        //            Intlk_8 I = (Intlk_8)tag;
        //            I.ToDataRow(Sheet.Rows[RowOffset++]);
        //        }
        //        catch (Exception ex)
        //        {
        //            var r = MessageBox.Show($"Error: {ex.Message}\nTage Name: {tag.Name} ", $"Tag Exception", MessageBoxButtons.OK);
        //        }

        //    }
        //}
        //catch (Exception e)
        //{
        //    var result = MessageBox.Show(e.Message, "Exception",
        //                    MessageBoxButtons.OK,
        //                    MessageBoxIcon.Error);
        //    if (result == DialogResult.OK) return;

        //}

    }

    public static void CreateIntlk16Report()//Excel.Worksheet Sheet)
    {
        //try
        //{
        //    int RowOffset = 1; // first row to use.
        //    Intlk_16.ToHeaderRow(Sheet.Rows[RowOffset++]);

        //    foreach (XTO_AOI tag in AllTags)
        //    {
        //        try
        //        {
        //            if (tag.DataType != "Intlk_16") continue;
        //            Intlk_16 I = (Intlk_16)tag;
        //            I.ToDataRow(Sheet.Rows[RowOffset++]);
        //        }
        //        catch (Exception ex)
        //        {
        //            var r = MessageBox.Show($"Error: {ex.Message}\nTage Name: {tag.Name} ", $"Tag Exception", MessageBoxButtons.OK);
        //        }

        //    }
        //}
        //catch (Exception e)
        //{
        //    var result = MessageBox.Show(e.Message, "Exception",
        //                    MessageBoxButtons.OK,
        //                    MessageBoxIcon.Error);
        //    if (result == DialogResult.OK) return;

        //}

    }

    public static void CreateDoReport()//Excel.Worksheet Sheet)
    {
        //try
        //{
        //    int RowOffset = 1; // first row to use.
        //    DOData.ToHeaderRow(Sheet.Rows[RowOffset++]);

        //    foreach (XTO_AOI tag in AllTags)
        //    {
        //        try
        //        {
        //            if (tag.DataType != "DOData") continue;
        //            DOData I = (DOData)tag;
        //            I.ToDataRow(Sheet.Rows[RowOffset++]);
        //        }
        //        catch (Exception ex)
        //        {
        //            var r = MessageBox.Show($"Error: {ex.Message}\nTage Name: {tag.Name} ", $"Tag Exception", MessageBoxButtons.OK);
        //        }

        //    }
        //}
        //catch (Exception e)
        //{
        //    var result = MessageBox.Show(e.Message, "Exception",
        //                    MessageBoxButtons.OK,
        //                    MessageBoxIcon.Error);
        //    if (result == DialogResult.OK) return;

        //}

    }


    #region INTK_8

    //public static void ToHeaderRow(Excel.Range row)
    //{
    //    int i = 1;
    //    row.Cells[1, i++].Value = "Scope";
    //    row.Cells[1, i++].Value = "Tag Name";
    //    row.Cells[1, i++].Value = "Tag Description";
    //    row.Cells[1, i++].Value = "HMI EquipID";
    //    row.Cells[1, i++].Value = "HMI EquipDesc";
    //    row.Cells[1, i++].Value = "Bypass";
    //    row.Cells[1, i++].Value = "OK State";
    //    row.Cells[1, i++].Value = "Intlk Active";
    //    row.Cells[1, i++].Value = "Intlk Latched";
    //    row.Cells[1, i++].Value = "Intlk First Out";
    //    row.Cells[1, i++].Value = "Intlk OK";
    //    row.Cells[1, i++].Value = "Byp Active";
    //    row.Cells[1, i++].Value = "Reset Rdy";
    //    row.Cells[1, i++].Value = "First Out";

    //    for (int x = 0; x < 7; x++) row.Cells[1, i++].Value = $"Intlk{x:D2}";
    //    for (int x = 0; x < 7; x++) row.Cells[1, i++].Value = $"Interlock: {x}";
    //}

    //public virtual void ToDataRow(Excel.Range row)
    //{
    //    int i = 1;
    //    row.Cells[1, i++].Value = Path;
    //    row.Cells[1, i++].Value = Name;
    //    row.Cells[1, i++].Value = Description;
    //    row.Cells[1, i++].Value = Cfg_EquipID;
    //    row.Cells[1, i++].Value = Cfg_EquipDesc;
    //    row.Cells[1, i++].Value = IntlkBypass;
    //    row.Cells[1, i++].Value = Cfg_OKState;
    //    row.Cells[1, i++].Value = IntlkActive;
    //    row.Cells[1, i++].Value = IntlkLatched;
    //    row.Cells[1, i++].Value = IntlkFirstOut;
    //    row.Cells[1, i++].Value = IntlkOK;
    //    row.Cells[1, i++].Value = BypActive;
    //    row.Cells[1, i++].Value = ResetRdy;
    //    row.Cells[1, i++].Value = FirstOut;
    //    row.Cells[1, i++].Value = Intlk00;
    //    row.Cells[1, i++].Value = Intlk01;
    //    row.Cells[1, i++].Value = Intlk02;
    //    row.Cells[1, i++].Value = Intlk03;
    //    row.Cells[1, i++].Value = Intlk04;
    //    row.Cells[1, i++].Value = Intlk05;
    //    row.Cells[1, i++].Value = Intlk06;
    //    row.Cells[1, i++].Value = Intlk07;

    //    for (int x = 0; x < 7; x++) row.Cells[1, i++].Value = Cfg_IntlkDesc[x];
    //}


    #endregion

    #region INTK_16

    //public static new void ToHeaderRow(Excel.Range row)
    //{
    //    int i = 1;
    //    row.Cells[1, i++].Value = "Scope";
    //    row.Cells[1, i++].Value = "Tag Name";
    //    row.Cells[1, i++].Value = "Tag Description";
    //    row.Cells[1, i++].Value = "HMI EquipID";
    //    row.Cells[1, i++].Value = "HMI EquipDesc";
    //    row.Cells[1, i++].Value = "Bypass";
    //    row.Cells[1, i++].Value = "Auto Ack";
    //    row.Cells[1, i++].Value = "Intlk Active";
    //    row.Cells[1, i++].Value = "Intlk Latched";
    //    row.Cells[1, i++].Value = "Intlk First Out";
    //    row.Cells[1, i++].Value = "Intlk OK";
    //    row.Cells[1, i++].Value = "Byp Active";
    //    row.Cells[1, i++].Value = "Reset Rdy";
    //    row.Cells[1, i++].Value = "First Out";

    //    for (int x = 0; x < 15; x++) row.Cells[1, i++].Value = $"Intlk{x:D2}";
    //    for (int x = 0; x < 15; x++) row.Cells[1, i++].Value = $"Interlock: {x}";
    //}

    //public override void ToDataRow(Excel.Range row)
    //{
    //    int i = 1;
    //    row.Cells[1, i++].Value = Path;
    //    row.Cells[1, i++].Value = Name;
    //    row.Cells[1, i++].Value = Description;
    //    row.Cells[1, i++].Value = Cfg_EquipID;
    //    row.Cells[1, i++].Value = Cfg_EquipDesc;
    //    row.Cells[1, i++].Value = IntlkBypass;
    //    row.Cells[1, i++].Value = Cfg_OKState;
    //    row.Cells[1, i++].Value = IntlkActive;
    //    row.Cells[1, i++].Value = IntlkLatched;
    //    row.Cells[1, i++].Value = IntlkFirstOut;
    //    row.Cells[1, i++].Value = IntlkOK;
    //    row.Cells[1, i++].Value = BypActive;
    //    row.Cells[1, i++].Value = ResetRdy;
    //    row.Cells[1, i++].Value = FirstOut;
    //    row.Cells[1, i++].Value = Intlk00;
    //    row.Cells[1, i++].Value = Intlk01;
    //    row.Cells[1, i++].Value = Intlk02;
    //    row.Cells[1, i++].Value = Intlk03;
    //    row.Cells[1, i++].Value = Intlk04;
    //    row.Cells[1, i++].Value = Intlk05;
    //    row.Cells[1, i++].Value = Intlk06;
    //    row.Cells[1, i++].Value = Intlk07;
    //    row.Cells[1, i++].Value = Intlk08;
    //    row.Cells[1, i++].Value = Intlk09;
    //    row.Cells[1, i++].Value = Intlk10;
    //    row.Cells[1, i++].Value = Intlk11;
    //    row.Cells[1, i++].Value = Intlk12;
    //    row.Cells[1, i++].Value = Intlk13;
    //    row.Cells[1, i++].Value = Intlk14;
    //    row.Cells[1, i++].Value = Intlk15;

    //    for (int x = 0; x < 15; x++) row.Cells[1, i++].Value = Cfg_IntlkDesc[x];
    //}

    #endregion

    #region INTLK_64

    //public static void ToHeaderRow(Excel.Range row)
    //{
    //    int i = 1;
    //    row.Cells[1, i++].Value = "Scope";
    //    row.Cells[1, i++].Value = "Tag Name";
    //    row.Cells[1, i++].Value = "Tag Description";
    //    row.Cells[1, i++].Value = "HMI EquipID";
    //    row.Cells[1, i++].Value = "HMI EquipDesc";
    //    row.Cells[1, i++].Value = "Bypass";
    //    row.Cells[1, i++].Value = "Auto Ack";
    //    row.Cells[1, i++].Value = "Intlk Active";
    //    row.Cells[1, i++].Value = "Intlk Latched";
    //    row.Cells[1, i++].Value = "Intlk First Out";
    //    row.Cells[1, i++].Value = "Intlk OK";
    //    row.Cells[1, i++].Value = "Byp Active";
    //    row.Cells[1, i++].Value = "Reset Rdy";
    //    row.Cells[1, i++].Value = "First Out";

    //}

    //public void ToDataRow(Excel.Range row)
    //{
    //    int i = 1;
    //    row.Cells[1, i++].Value = Path;
    //    row.Cells[1, i++].Value = Name;
    //    row.Cells[1, i++].Value = Description;
    //    row.Cells[1, i++].Value = Cfg_EquipID;
    //    row.Cells[1, i++].Value = Cfg_EquipDesc;
    //    //row.Cells[1, i++].Value = IntlkBypass;
    //    //row.Cells[1, i++].Value = Cfg_OKState;
    //    //row.Cells[1, i++].Value = IntlkActive;
    //    //row.Cells[1, i++].Value = IntlkLatched;
    //    //row.Cells[1, i++].Value = IntlkFirstOut;
    //    //row.Cells[1, i++].Value = IntlkOK;
    //    //row.Cells[1, i++].Value = BypActive;
    //    //row.Cells[1, i++].Value = ResetRdy;
    //    //row.Cells[1, i++].Value = FirstOut;

    //}

    #endregion

   

}
