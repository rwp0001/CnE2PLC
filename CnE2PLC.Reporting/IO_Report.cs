using CnE2PLC.Helpers;
using CnE2PLC.PLC;
using CnE2PLC.PLC.XTO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace CnE2PLC.Reporting;

public static class IO_Report
{

    public static void CreateReport(Controller plc, Stream File)
    {
        try
        {
            XSSFWorkbook IO_Workbook = new XSSFWorkbook();

            CreateAiReport(IO_Workbook, plc);
            CreateAoReport(IO_Workbook, plc);
            CreateDiReport(IO_Workbook, plc);
            CreateDoReport(IO_Workbook, plc);

            IO_Workbook.Write(File);

        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"Create IO Report Error: {ex.Message}\n");
        }
    }


    #region AIData

    public static void CreateAiReport(IWorkbook wb, Controller plc)
    {
        try
        {
            ISheet Sheet = wb.CreateSheet("AIData");
            int RowOffset = 0; // first row to use.

            foreach (XTO_AOI tag in plc.AllTags.Where(t => t.DataType == "AIData"))
            {
                IRow row;
                if (RowOffset == 0)
                {
                    row = Sheet.CreateRow(RowOffset++);
                    ToHeaderRow((AIData)tag, row);
                }
                row = Sheet.CreateRow(RowOffset++);
                ToDataRow((AIData)tag, row);
            }
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"IO Report AI Error: {ex.Message}\n");
        }
    }

    public static void ToHeaderRow(AIData tag, IRow row)
    {
        int i = 0;
        row.CreateCell(i++).SetCellValue("Scope");
        row.CreateCell(i++).SetCellValue("Tag Name");
        row.CreateCell(i++).SetCellValue("IO");
        row.CreateCell(i++).SetCellValue("Tag Description");
        row.CreateCell(i++).SetCellValue("HMI EquipID");
        row.CreateCell(i++).SetCellValue("HMI EquipDesc");
        row.CreateCell(i++).SetCellValue("HMI EU");
        row.CreateCell(i++).SetCellValue("AOI Calls");
        row.CreateCell(i++).SetCellValue("Tag References");
        row.CreateCell(i++).SetCellValue("InUse");
        row.CreateCell(i++).SetCellValue("Raw");
        row.CreateCell(i++).SetCellValue("Min Raw");
        row.CreateCell(i++).SetCellValue("Max Raw");
        row.CreateCell(i++).SetCellValue("Min EU");
        row.CreateCell(i++).SetCellValue("Max EU");
        row.CreateCell(i++).SetCellValue("PV");
        row.CreateCell(i++).SetCellValue("HiHi Enable");
        row.CreateCell(i++).SetCellValue("HiHi Auto Ack");
        row.CreateCell(i++).SetCellValue("HiHi SP Limit");
        row.CreateCell(i++).SetCellValue("HiHi SP");
        row.CreateCell(i++).SetCellValue("HiHi On Time");
        row.CreateCell(i++).SetCellValue("HiHi Off Time");
        row.CreateCell(i++).SetCellValue("HiHi Dly Time");
        row.CreateCell(i++).SetCellValue("HiHi SD Time");
        row.CreateCell(i++).SetCellValue("HiHi Alarm");
        row.CreateCell(i++).SetCellValue("HiHi Count");
        row.CreateCell(i++).SetCellValue("HSD Count");
        row.CreateCell(i++).SetCellValue("Hi Enable");
        row.CreateCell(i++).SetCellValue("Hi Auto Ack");
        row.CreateCell(i++).SetCellValue("Hi SP");
        row.CreateCell(i++).SetCellValue("Hi On Time");
        row.CreateCell(i++).SetCellValue("Hi Off Time");
        row.CreateCell(i++).SetCellValue("Hi Dly Time");
        row.CreateCell(i++).SetCellValue("Hi Alarm");
        row.CreateCell(i++).SetCellValue("Hi Count");
        row.CreateCell(i++).SetCellValue("Lo Enable");
        row.CreateCell(i++).SetCellValue("Lo Auto Ack");
        row.CreateCell(i++).SetCellValue("Lo SP");
        row.CreateCell(i++).SetCellValue("Lo On Time");
        row.CreateCell(i++).SetCellValue("Lo Off Time");
        row.CreateCell(i++).SetCellValue("Lo Dly Time");
        row.CreateCell(i++).SetCellValue("Lo Alarm");
        row.CreateCell(i++).SetCellValue("Lo Count");
        row.CreateCell(i++).SetCellValue("LoLo Enable");
        row.CreateCell(i++).SetCellValue("LoLo Auto Ack");
        row.CreateCell(i++).SetCellValue("LoLo SP Limit");
        row.CreateCell(i++).SetCellValue("LoLo SP");
        row.CreateCell(i++).SetCellValue("LoLo On Time");
        row.CreateCell(i++).SetCellValue("LoLo Off Time");
        row.CreateCell(i++).SetCellValue("LoLo Dly Time");
        row.CreateCell(i++).SetCellValue("LoLo SD Time");
        row.CreateCell(i++).SetCellValue("LoLo Alarm");
        row.CreateCell(i++).SetCellValue("LoLo Count");
        row.CreateCell(i++).SetCellValue("LSD Count");
        row.CreateCell(i++).SetCellValue("Sim");
        row.CreateCell(i++).SetCellValue("Sim PV");
        row.CreateCell(i++).SetCellValue("Bad PV Enable");
        row.CreateCell(i++).SetCellValue("Bad PV Auto Ack");
        row.CreateCell(i++).SetCellValue("Bad PV Alarm");
    }

    public static void ToDataRow(AIData tag, IRow row)
    {
        int i = 0;
        row.CreateCell(i++).SetCellValue(tag.Path);
        row.CreateCell(i++).SetCellValue(tag.Name);
        row.CreateCell(i++).SetCellValue(tag.IO);
        row.CreateCell(i++).SetCellValue(tag.Description);
        row.CreateCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.CreateCell(i++).SetCellValue(tag.Cfg_EquipDesc);
        row.CreateCell(i++).SetCellValue(tag.Cfg_EU);
        row.CreateCell(i++).SetCellValue(tag.AOICalls);
        row.CreateCell(i++).SetCellValue(tag.References);
        row.CreateCell(i++).SetCellValue(tag.InUse == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue((double)tag.Raw);
        row.CreateCell(i++).SetCellValue((double)tag.MinRaw);
        row.CreateCell(i++).SetCellValue((double)tag.MaxRaw);
        row.CreateCell(i++).SetCellValue((double)tag.MinEU);
        row.CreateCell(i++).SetCellValue((double)tag.MaxEU);
        row.CreateCell(i++).SetCellValue((double)tag.PV);
        row.CreateCell(i++).SetCellValue(tag.HiHiEnable == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue(tag.HiHiAutoAck == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue((double)tag.HiHiSPLimit);
        row.CreateCell(i++).SetCellValue((double)tag.HiHiSP);
        row.CreateCell(i++).SetCellValue((double)tag.Cfg_HiHiOnTmr);
        row.CreateCell(i++).SetCellValue((double)tag.Cfg_HiHiOffTmr);
        row.CreateCell(i++).SetCellValue((double)tag.Cfg_HiHiDlyTmr);
        row.CreateCell(i++).SetCellValue((double)tag.Cfg_HiHiSDTmr);
        row.CreateCell(i++).SetCellValue(tag.HiHiAlarm == true ? "Alarm" : "Ok");
        row.CreateCell(i++).SetCellValue(tag.HiHi_Count);
        row.CreateCell(i++).SetCellValue(tag.HSD_Count);
        row.CreateCell(i++).SetCellValue(tag.HiEnable == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue(tag.HiAutoAck == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue((double)tag.HiSP);
        row.CreateCell(i++).SetCellValue((double)tag.Cfg_HiOnTmr);
        row.CreateCell(i++).SetCellValue((double)tag.Cfg_HiOffTmr);
        row.CreateCell(i++).SetCellValue((double)tag.Cfg_HiDlyTmr);
        row.CreateCell(i++).SetCellValue(tag.HiAlarm == true ? "Alarm" : "Ok");
        row.CreateCell(i++).SetCellValue(tag.Hi_Count);
        row.CreateCell(i++).SetCellValue(tag.LoEnable == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue(tag.LoAutoAck == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue((double)tag.LoSP);
        row.CreateCell(i++).SetCellValue((double)tag.Cfg_LoOnTmr);
        row.CreateCell(i++).SetCellValue((double)tag.Cfg_LoOffTmr);
        row.CreateCell(i++).SetCellValue((double)tag.Cfg_LoDlyTmr);
        row.CreateCell(i++).SetCellValue(tag.LoAlarm == true ? "Alarm" : "Ok");
        row.CreateCell(i++).SetCellValue(tag.Lo_Count);
        row.CreateCell(i++).SetCellValue(tag.LoLoEnable == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue(tag.LoLoAutoAck == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue((double)tag.LoLoSPLimit);
        row.CreateCell(i++).SetCellValue((double)tag.LoLoSP);
        row.CreateCell(i++).SetCellValue((double)tag.Cfg_LoLoOnTmr);
        row.CreateCell(i++).SetCellValue((double)tag.Cfg_LoLoOffTmr);
        row.CreateCell(i++).SetCellValue((double)tag.Cfg_LoLoDlyTmr);
        row.CreateCell(i++).SetCellValue((double)tag.Cfg_LoLoSDTmr);
        row.CreateCell(i++).SetCellValue(tag.LoLoAlarm == true ? "Alarm" : "Ok");
        row.CreateCell(i++).SetCellValue(tag.LoLo_Count);
        row.CreateCell(i++).SetCellValue(tag.LSD_Count);
        row.CreateCell(i++).SetCellValue(tag.Sim == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue((double)tag.SimPV);
        row.CreateCell(i++).SetCellValue(tag.BadPVEnable == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue(tag.BadPVAutoAck == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue(tag.BadPVAlarm == true ? "Alarm" : "Ok");

    }

    #endregion

    #region AOData

    public static void CreateAoReport(IWorkbook wb, Controller plc)
    {
        try
        {
            ISheet Sheet = wb.CreateSheet("AOData");
            int RowOffset = 0; // first row to use.

            foreach (XTO_AOI tag in plc.AllTags.Where(t => t.DataType == "AOData"))
            {
                IRow row;
                if (RowOffset == 0)
                {
                    row = Sheet.CreateRow(RowOffset++);
                    ToHeaderRow((AOData)tag, row);
                }
                row = Sheet.CreateRow(RowOffset++);
                ToDataRow((AOData)tag, row);
            }
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"IO Report AO Error: {ex.Message}\n");
        }
    }

    public static void ToHeaderRow(AOData tag, IRow row)
    {
        int i = 0;
        row.CreateCell(i++).SetCellValue("Scope");
        row.CreateCell(i++).SetCellValue("Tag Name");
        row.CreateCell(i++).SetCellValue("IO");
        row.CreateCell(i++).SetCellValue("Tag Description");
        row.CreateCell(i++).SetCellValue("HMI EquipID");
        row.CreateCell(i++).SetCellValue("HMI EquipDesc");
        row.CreateCell(i++).SetCellValue("HMI EU");
        row.CreateCell(i++).SetCellValue("AOI Calls");
        row.CreateCell(i++).SetCellValue("Tag References");
        row.CreateCell(i++).SetCellValue("InUse");
        row.CreateCell(i++).SetCellValue("Raw");
        row.CreateCell(i++).SetCellValue("Min Raw");
        row.CreateCell(i++).SetCellValue("Max Raw");
        row.CreateCell(i++).SetCellValue("Min EU");
        row.CreateCell(i++).SetCellValue("Max EU");
        row.CreateCell(i++).SetCellValue("CV");
        row.CreateCell(i++).SetCellValue("Inc To Close");
        row.CreateCell(i++).SetCellValue("Sim");
        row.CreateCell(i++).SetCellValue("Sim CV");
    }

    public static void ToDataRow(AOData tag, IRow row)
    {
        int i = 0;
        row.CreateCell(i++).SetCellValue(tag.Path);
        row.CreateCell(i++).SetCellValue(tag.Name);
        row.CreateCell(i++).SetCellValue(tag.IO);
        row.CreateCell(i++).SetCellValue(tag.Description);
        row.CreateCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.CreateCell(i++).SetCellValue(tag.Cfg_EquipDesc);
        row.CreateCell(i++).SetCellValue(tag.Cfg_EU);
        row.CreateCell(i++).SetCellValue(tag.AOICalls);
        row.CreateCell(i++).SetCellValue(tag.References);
        row.CreateCell(i++).SetCellValue(tag.InUse == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue($"{tag.Raw}");
        row.CreateCell(i++).SetCellValue($"{tag.MinRaw}");
        row.CreateCell(i++).SetCellValue($"{tag.MaxRaw}");
        row.CreateCell(i++).SetCellValue($"{tag.MinEU}");
        row.CreateCell(i++).SetCellValue($"{tag.MaxEU}");
        row.CreateCell(i++).SetCellValue($"{tag.CV}");
        row.CreateCell(i++).SetCellValue(tag.Cfg_IncToClose == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue(tag.Sim == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue($"{tag.SimCV}");
    }

    #endregion

    #region DIData

    public static void CreateDiReport(IWorkbook wb, Controller plc)
    {
        try
        {
            ISheet Sheet = wb.CreateSheet("DIData");
            int RowOffset = 0; // first row to use.

            foreach (XTO_AOI tag in plc.AllTags.Where(t => t.DataType == "DIData"))
            {
                IRow row;
                if (RowOffset == 0)
                {
                    row = Sheet.CreateRow(RowOffset++);
                    ToHeaderRow((DIData)tag, row);
                }
                row = Sheet.CreateRow(RowOffset++);
                ToDataRow((DIData)tag, row);
            }
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"IO Report DI Error: {ex.Message}\n");
        }
    }

    public static void ToHeaderRow(DIData tag, IRow row)
    {
        int i = 0;
        row.CreateCell(i++).SetCellValue("Scope");
        row.CreateCell(i++).SetCellValue("Tag Name");
        row.CreateCell(i++).SetCellValue("IO");
        row.CreateCell(i++).SetCellValue("Tag Description");
        row.CreateCell(i++).SetCellValue("HMI EquipID");
        row.CreateCell(i++).SetCellValue("HMI EquipDesc");
        row.CreateCell(i++).SetCellValue("AOI Calls");
        row.CreateCell(i++).SetCellValue("Tag References");
        row.CreateCell(i++).SetCellValue("InUse");
        row.CreateCell(i++).SetCellValue("Raw");
        row.CreateCell(i++).SetCellValue("Value");
        row.CreateCell(i++).SetCellValue("Alarm Enable");
        row.CreateCell(i++).SetCellValue("Alarm Auto Ack");
        row.CreateCell(i++).SetCellValue("Alarm State");
        row.CreateCell(i++).SetCellValue("Alarm On Time");
        row.CreateCell(i++).SetCellValue("Alarm Off Time");
        row.CreateCell(i++).SetCellValue("Alarm Dly Time");
        row.CreateCell(i++).SetCellValue("Alarm SD Time");
        row.CreateCell(i++).SetCellValue("Alarm");
        row.CreateCell(i++).SetCellValue("Alarm Count");
        row.CreateCell(i++).SetCellValue("SD Count");
        row.CreateCell(i++).SetCellValue("Sim");
        row.CreateCell(i++).SetCellValue("Sim Value");
    }

    public static void ToDataRow(DIData tag, IRow row)
    {
        int i = 0;
        row.CreateCell(i++).SetCellValue(tag.Path);
        row.CreateCell(i++).SetCellValue(tag.Name);
        row.CreateCell(i++).SetCellValue(tag.IO);
        row.CreateCell(i++).SetCellValue(tag.Description);
        row.CreateCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.CreateCell(i++).SetCellValue(tag.Cfg_EquipDesc);
        row.CreateCell(i++).SetCellValue(tag.AOICalls);
        row.CreateCell(i++).SetCellValue(tag.References);
        row.CreateCell(i++).SetCellValue(tag.InUse == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue(tag.Raw == true ? "On" : "Off");
        row.CreateCell(i++).SetCellValue(tag.Value == true ? "On" : "Off");
        row.CreateCell(i++).SetCellValue(tag.AlmEnable == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue(tag.AlmAutoAck == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue(tag.AlmState == 1 ? "On" : "Off");
        row.CreateCell(i++).SetCellValue($"{tag.Cfg_AlmOnTmr}");
        row.CreateCell(i++).SetCellValue($"{tag.Cfg_AlmOffTmr}");
        row.CreateCell(i++).SetCellValue($"{tag.Cfg_AlmDlyTmr}");
        row.CreateCell(i++).SetCellValue($"{tag.Cfg_SDDlyTmr}");
        row.CreateCell(i++).SetCellValue(tag.  Alarm == true ? "Alarm" : "Ok");
        row.CreateCell(i++).SetCellValue(tag.Alm_Count);
        row.CreateCell(i++).SetCellValue(tag.SD_Count);
        row.CreateCell(i++).SetCellValue(tag.Sim == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue(tag.SimValue == true ? "On" : "Off");
    }

    #endregion

    #region DOData

    public static void CreateDoReport(IWorkbook wb, Controller plc)
    {
        try
        {
            ISheet Sheet = wb.CreateSheet("DOData");
            int RowOffset = 0; // first row to use.

            foreach (XTO_AOI tag in plc.AllTags.Where(t => t.DataType == "DOData"))
            {
                IRow row;
                if (RowOffset == 0)
                {
                    row = Sheet.CreateRow(RowOffset++);
                    ToHeaderRow((DOData)tag, row);
                }
                row = Sheet.CreateRow(RowOffset++);
                ToDataRow((DOData)tag, row);
            }
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"IO Report DO Error: {ex.Message}\n");
        }
    }

    public static void ToHeaderRow(DOData tag, IRow row)
    {
        int i = 0;
        row.CreateCell(i++).SetCellValue("Scope");
        row.CreateCell(i++).SetCellValue("Tag Name");
        row.CreateCell(i++).SetCellValue("IO");
        row.CreateCell(i++).SetCellValue("Tag Description");
        row.CreateCell(i++).SetCellValue("HMI EquipID");
        row.CreateCell(i++).SetCellValue("HMI EquipDesc");
        row.CreateCell(i++).SetCellValue("AOI Calls");
        row.CreateCell(i++).SetCellValue("Tag References");
        row.CreateCell(i++).SetCellValue("InUse");
        row.CreateCell(i++).SetCellValue("Raw");
        row.CreateCell(i++).SetCellValue("Value");
        row.CreateCell(i++).SetCellValue("Sim");
        row.CreateCell(i++).SetCellValue("Sim Value");
    }

    public static void ToDataRow(DOData tag, IRow row)
    {
        int i = 0;
        row.CreateCell(i++).SetCellValue(tag.Path);
        row.CreateCell(i++).SetCellValue(tag.Name);
        row.CreateCell(i++).SetCellValue(tag.IO);
        row.CreateCell(i++).SetCellValue(tag.Description);
        row.CreateCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.CreateCell(i++).SetCellValue(tag.Cfg_EquipDesc);
        row.CreateCell(i++).SetCellValue(tag.AOICalls);
        row.CreateCell(i++).SetCellValue(tag.References);
        row.CreateCell(i++).SetCellValue(tag.InUse == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue(tag.Raw == true ? "On" : "Off");
        row.CreateCell(i++).SetCellValue(tag.Value == true ? "On" : "Off");
        row.CreateCell(i++).SetCellValue(tag.Sim == true ? "Yes" : "No");
        row.CreateCell(i++).SetCellValue(tag.SimVal == 1 ? "On" : "Off");
    }

    #endregion

    #region INTK_8

    public static void CreateIntlk8Report(IWorkbook wb, Controller plc)
    {
        try
        {
            ISheet Sheet = wb.CreateSheet("Intlk_8");
            int RowOffset = 0; // first row to use.

            foreach (XTO_AOI tag in plc.AllTags.Where(t => t.DataType == "Intlk_8"))
            {
                IRow row;
                if (RowOffset == 0)
                {
                    row = Sheet.CreateRow(RowOffset++);
                    ToHeaderRow((Intlk_8)tag, row);
                }
                row = Sheet.CreateRow(RowOffset++);
                ToDataRow((Intlk_8)tag, row);
            }
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"IO Report Intlk_8 Error: {ex.Message}\n");
        }
    }

    public static void ToHeaderRow(Intlk_8 tag, IRow row)
    {
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
    }

    public static void ToDataRow(Intlk_8 tag, IRow row)
    {
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
    }


    #endregion

    #region INTK_16


    public static void CreateIntlk16Report(IWorkbook wb, Controller plc)
    {
        try
        {
            ISheet Sheet = wb.CreateSheet("Intlk_16");
            int RowOffset = 0; // first row to use.

            foreach (XTO_AOI tag in plc.AllTags.Where(t => t.DataType == "Intlk_16"))
            {
                IRow row;
                if (RowOffset == 0)
                {
                    row = Sheet.CreateRow(RowOffset++);
                    ToHeaderRow((Intlk_16)tag, row);
                }
                row = Sheet.CreateRow(RowOffset++);
                ToDataRow((Intlk_16)tag, row);
            }
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"IO Report Intlk_16 Error: {ex.Message}\n");
        }
    }


    public static void ToHeaderRow(Intlk_16 tag, IRow row)
    {
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
    }

    public static void ToDataRow(Intlk_16 tag, IRow row)
    {
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
    }

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
