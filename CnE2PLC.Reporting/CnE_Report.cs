using CnE2PLC.Helpers;
using CnE2PLC.PLC;
using CnE2PLC.PLC.XTO;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;


namespace CnE2PLC.Reporting;

public static class CnE_Report
{
    private static byte[] row00_bg_rgb = new byte[] { 196, 189, 151 };
    private static byte[] row01_bg_rgb = new byte[] { 242, 242, 242 };
    private static byte[] row12_bg_rgb = new byte[] { 192, 80, 77 };
    private static byte[] row13_bg_rgb = new byte[] { 242, 242, 242 };

    /// <summary>
    /// Exports the Tags to a Excel Templete.
    /// </summary>
    /// <param name="plc">PLC controller object to export from.</param>
    /// <param name="File">File stream to write the content out.</param>
    public static void CreateReport(Controller plc, Stream File )
    {
        try
        {
            DateTime StartTime = DateTime.Now;

            XSSFWorkbook CnE_Workbook = new XSSFWorkbook("CnE_Template.xlsx");
            CnE_Workbook.SetSheetName(0, plc.Name);

            ISheet CnE_Sheet = CnE_Workbook.GetSheetAt(0);

            int RowOffset = 16; // first row to use.
            int ColumnOffset = 18;

            foreach (XTO_AOI tag in plc.AllTags)
            {
                try
                {
                    //if (!tag.AOICalled) continue;
                    //if (tag.InUse != true) continue;

                    switch (tag.DataType.ToLower())
                    {

                        case "dodata":                          
                            InsertCol(CnE_Sheet, ColumnOffset);
                            ToColumn((DOData)tag, CnE_Workbook, ColumnOffset++);
                            break;

                        case "aodata":
                            AOData AoTag = (AOData)tag;
                            
                            InsertCol(CnE_Sheet, ColumnOffset);
                            ToColumn((AOData)tag, CnE_Workbook, ColumnOffset++);
                            break;

                        case "didata":
                        case "didata_fims":
                            InsertRow(CnE_Sheet, RowOffset);
                            ToValueRow((DIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToAlarmRow((DIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToShutdownRow((DIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            break;

                        case "aidata":
                        case "aidata_fims":
                            InsertRow(CnE_Sheet, RowOffset);
                            ToPVRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToHSDRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToHiHiAlarmRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToHiAlarmRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToLoAlarmRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToLoLoAlarmRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToLSDRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToBadPVAlarmRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            break;

                        case "twopositionvalve":
                        case "twopositionvalvev2":
                            InsertCol(CnE_Sheet, ColumnOffset);
                            ToColumn((TwoPositionValveV2)tag, CnE_Workbook, ColumnOffset++);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToOpenRow((TwoPositionValveV2)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToCloseRow((TwoPositionValveV2)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToFailedToOpenRow((TwoPositionValveV2)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToFailedToCloseRow((TwoPositionValveV2)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            break;

                        case "valveanalog":
                            InsertCol(CnE_Sheet, ColumnOffset);
                            ToColumn((ValveAnalog)tag, CnE_Workbook, ColumnOffset++);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToPosRow((ValveAnalog)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            InsertRow(CnE_Sheet, RowOffset);
                            ToPosFailRow((ValveAnalog)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            break;

                        case "motor_vfd":

                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.DebugPrint($"CnE: Tag Exception Error: {ex.Message}\nTage Name: {tag.Name}");
                }

            }

            // clean up for filtering.
            CnE_Sheet.RemoveRow(CnE_Sheet.GetRow(15));
            CnE_Sheet.ShiftRows(16, CnE_Sheet.LastRowNum, -1);

            //CnE_Sheet.Columns[18].Delete();


            CnE_Workbook.Write(File, true);

            DateTime EndTime = DateTime.Now;

            LogHelper.DebugPrint($"CnE Report for {plc.Name} was Generated taking {EndTime-StartTime} secs.");

        }
        catch (Exception e)
        {
            LogHelper.DebugPrint($"CnE: Create Report Exception: {e.Message}");
        }

        static void InsertRow(ISheet ws, int offset)
        {
            
            ws.CopyRow(offset, offset + 1);
        }

        static void InsertCol(ISheet ws, int offset)
        {
            int startRow = ws.FirstRowNum;
            int endRow = ws.LastRowNum;

            for(int rowIndex = startRow; rowIndex <= endRow; rowIndex++)
            {
                
                IRow row = ws.GetRow(rowIndex);
                if (row == null) continue;
                
                ICell cell = row.CreateCell(offset);
                if (cell == null) continue;
                
                ICell p_cell = row.GetCell(offset - 1);
                if (p_cell == null) continue;

                ICellStyle style = p_cell.CellStyle;
                if (style == null) continue; 

                cell.CellStyle = style;
            }

            CellRangeAddress cellRange = new CellRangeAddress(1, 11, offset, offset);
            ws.AddMergedRegion(cellRange);

            ws.SetColumnWidth(offset, ws.GetColumnWidth(offset-1));

            //CopyColumn(ws, offset, offset + 1);
        }

        static void CopyColumn(ISheet sheet, int sourceColumnIndex, int destinationColumnIndex)
        {
            // Iterate through all existing rows in the sheet
            for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                IRow sourceRow = sheet.GetRow(rowIndex);
                if (sourceRow != null)
                {
                    ICell sourceCell = sourceRow.GetCell(sourceColumnIndex);
                    if (sourceCell != null)
                    {
                        // Use the built-in CopyCellTo method to copy within the same sheet/row
                        sourceCell.CopyCellTo(destinationColumnIndex);

                        // Note: The built-in method handles basic copying of value and style
                        // For complex scenarios (like copying formulas with relative changes
                        // across columns), manual handling might be needed.
                    }
                }
            }

            // Optional: Copy column width
            sheet.SetColumnWidth(destinationColumnIndex, sheet.GetColumnWidth(sourceColumnIndex));
        }

    }

    public static void UpdateReport(Controller plc, string FileName)
    {
        //Excel.Application? excelApp = null;

        //try
        //{
        //    excelApp = new Excel.Application();
        //    excelApp.Visible = !Properties.Settings.Default.HideExcel;

        //    Excel.Workbook? CnE_Workbook = null;
        //    Excel.Worksheet? CnE_Sheet = null;

        //    // open the file as readonly.
        //    excelApp.Workbooks.Open(FileName, false, true);
        //    CnE_Workbook = excelApp.ActiveWorkbook;

        //    // select the sheet
        //    foreach (Excel.Worksheet ws in CnE_Workbook.Worksheets)
        //    {
        //        try
        //        {
        //            // Find the last real row
        //            int lastUsedRow = ws.Cells.Find("*", System.Reflection.Missing.Value,
        //                                           System.Reflection.Missing.Value, System.Reflection.Missing.Value,
        //                                           Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious,
        //                                           false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

        //            // Find the last real column
        //            int lastUsedColumn = ws.Cells.Find("*", System.Reflection.Missing.Value,
        //                                           System.Reflection.Missing.Value, System.Reflection.Missing.Value,
        //                                           Excel.XlSearchOrder.xlByColumns, Excel.XlSearchDirection.xlPrevious,
        //                                           false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;

        //            //search for the correct sheet
        //            //if (lastUsedColumn != 23) continue;
        //            //if (lastUsedRow < 15) continue;
        //            //if (ws.Cells[1, 1].Value.ToString() != "COLOR LEGEND") continue;
        //            //if (ws.Cells[1, 2].Value.ToString() != "CALLOUT CODES") continue;
        //            //if (ws.Cells[1, 11].Value.ToString() != "C&E ACRONYM LEGEND") continue;
        //            Excel.Range range = ws.Cells;

        //            int count = 0;

        //            for (int i = 16; i < lastUsedRow; i++)
        //            {

        //                // tage name is 3
        //                // full tagname is 4
        //                // IO Details is 5
        //                // setpoint is 7
        //                // alarm on time is 8
        //                // alarm off time is 10
        //                // Auto Ack is 15
        //                // Status / Not in use is 6

        //                Excel.Range row = range.Rows[i];
        //                row.Cells[1, 4].Activate();

        //                // check for tag name
        //                if (row.Cells[1, 4].Value == null) continue;

        //                // get the tag name
        //                string BaseTag = row.Cells[1, 4].Value;
        //                string Element = string.Empty;
        //                if (BaseTag.Contains(".")) Element = BaseTag.Split('.')[1];
        //                BaseTag = BaseTag.Split('.')[0];

        //                var Tag = Tags.SingleOrDefault(tag => tag.Name == BaseTag);
        //                if (Tag == null) continue;

        //                switch (Tag.DataType.ToLower())
        //                {
        //                    case "didata":
        //                        if (Element == string.Empty) continue;
        //                        DIData DiTag = (DIData)Tag;



        //                        row.Cells[1, 6].Value = "Not In Use";
        //                        row.Cells[1, 15].Value = "";

        //                        switch (Element.ToLower())
        //                        {
        //                            case "valve":
        //                                //if (DiTag.InUse == true) row.Cells[1, 6].Value = "Standard IO";
        //                                DiTag.ToValueRow(row);
        //                                break;

        //                            case "alarm":
        //                                //if (DiTag.InUse == true) row.Cells[1, 6].Value = "Soft IO";
        //                                //row.Cells[1, 9].Value = string.Format("{0} Sec.", DiTag.Cfg_AlmOnTmr);
        //                                //row.Cells[1, 10].Value = string.Format("{0} Sec.", DiTag.Cfg_AlmOffTmr);
        //                                //if (DiTag.AlmAutoAck == true) row.Cells[1, 15].Value = "Y";
        //                                DiTag.ToAlarmRow(row);
        //                                break;

        //                            default:
        //                                break;
        //                        }


        //                        break;

        //                    case "aidata":
        //                        if (Element == string.Empty) continue;
        //                        AIData AiTag = (AIData)Tag;

        //                        row.Cells[1, 6].Value = "Not In Use";
        //                        row.Cells[1, 15].Value = "";

        //                        switch (Element.ToLower())
        //                        {
        //                            case "pv":
        //                                if (AiTag.InUse == true) { row.Cells[1, 6].Value = "Standard IO"; }
        //                                row.Cells[1, 5].Value = "Analog Input";
        //                                break;

        //                            case "hihialarm":
        //                                if (AiTag.HiHiEnable == true) row.Cells[1, 6].Value = "Standard IO";
        //                                row.Cells[1, 5].Value = "Soft IO";
        //                                row.Cells[1, 7].Value = AiTag.HiHiSP;
        //                                row.Cells[1, 9].Value = string.Format("{0} Sec.", AiTag.Cfg_HiHiOnTmr);
        //                                row.Cells[1, 10].Value = string.Format("{0} Sec.", AiTag.Cfg_HiHiOffTmr);
        //                                if (AiTag.HiHiAutoAck == true) row.Cells[1, 15].Value = "Y";
        //                                break;

        //                            case "hialarm":
        //                                if (AiTag.HiEnable == true) row.Cells[1, 6].Value = "Standard IO";
        //                                row.Cells[1, 5].Value = "Soft IO";
        //                                row.Cells[1, 7].Value = AiTag.HiSP;
        //                                row.Cells[1, 9].Value = string.Format("{0} Sec.", AiTag.Cfg_HiOnTmr);
        //                                row.Cells[1, 10].Value = string.Format("{0} Sec.", AiTag.Cfg_HiOffTmr);
        //                                if (AiTag.HiAutoAck == true) row.Cells[1, 15].Value = "Y";
        //                                break;

        //                            case "loalarm":
        //                                if (AiTag.LoEnable == true) row.Cells[1, 6].Value = "Standard IO";
        //                                row.Cells[1, 5].Value = "Soft IO";
        //                                row.Cells[1, 7].Value = AiTag.LoSP;
        //                                row.Cells[1, 9].Value = string.Format("{0} Sec.", AiTag.Cfg_LoOnTmr);
        //                                row.Cells[1, 10].Value = string.Format("{0} Sec.", AiTag.Cfg_LoOffTmr);
        //                                if (AiTag.LoAutoAck == true) row.Cells[1, 15].Value = "Y";
        //                                break;

        //                            case "loloalarm":
        //                                if (AiTag.LoLoEnable == true) row.Cells[1, 6].Value = "Standard IO";
        //                                row.Cells[1, 5].Value = "Soft IO";
        //                                row.Cells[1, 7].Value = AiTag.LoLoSP;
        //                                row.Cells[1, 9].Value = string.Format("{0} Sec.", AiTag.Cfg_LoLoOnTmr);
        //                                row.Cells[1, 10].Value = string.Format("{0} Sec.", AiTag.Cfg_LoLoOffTmr);
        //                                if (AiTag.LoLoAutoAck == true) row.Cells[1, 15].Value = "Y";
        //                                break;

        //                            case "badpvalarm":
        //                                if (AiTag.BadPVEnable == true) row.Cells[1, 6].Value = "Standard IO";
        //                                row.Cells[1, 5].Value = "Soft IO";
        //                                if (AiTag.BadPVAutoAck == true) row.Cells[1, 15].Value = "Y";
        //                                break;



        //                            default:
        //                                break;
        //                        }

        //                        break;

        //                    default:
        //                        break;
        //                }


        //                count++;
        //                //Application.DoEvents();
        //            }

        //        }
        //        catch (Exception e)
        //        {

        //        }

        //    }

        //    //excelApp.Quit();

        //}
        //catch (Exception e)
        //{

        //}
    }


    #region AIData

    public static void ToPVRow(AIData tag, IRow row, IWorkbook WB)
    {

        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.PV_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        if (tag.Sim == true)
        {
            style.FillBackgroundColor = HSSFColor.DarkRed.Index;
            font.Color = HSSFColor.White.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        row.GetCell(i++).SetCellValue($"{tag.Name}.PV");
        row.GetCell(i++).SetCellValue("Analog Input");
        row.GetCell(i++).SetCellValue(tag.InUse == true ? "Standard IO" : "Not In Use");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue(tag.Cfg_EU);
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");

    }

    public static void ToHSDRow(AIData tag, IRow row, IWorkbook WB)
    {
        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.HSD_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        row.GetCell(i++).SetCellValue($"{tag.Name}.HSD");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue((tag.HiHiEnable == true & tag.InUse == true) ? "Standard IO" : "Not In Use");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("Bool");
        row.GetCell(i++).SetCellValue($"{tag.Cfg_HiHiSDTmr} Sec.");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");

    }

    public static void ToLSDRow(AIData tag, IRow row, IWorkbook WB)
    {

        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.LSD_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        row.GetCell(i++).SetCellValue($"{tag.Name}.LSD");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue((tag.LoLoEnable == true & tag.InUse == true) ? "Standard IO" : "Not In Use");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("Bool");
        row.GetCell(i++).SetCellValue($"{tag.Cfg_LoLoSDTmr} Sec.");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");

    }

    public static void ToHiHiAlarmRow(AIData tag, IRow row, IWorkbook WB)
    {

        int i = 0;
        IDrawing<IShape> patriarch = WB.GetSheetAt(0).CreateDrawingPatriarch();

        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.HiHi_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        row.GetCell(i++).SetCellValue($"{tag.Name}.HiHiAlarm");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue((tag.HiHiEnable == true & tag.InUse == true) ? "Standard IO" : "Not In Use");

        IClientAnchor anchor_Limit = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment_Limit = patriarch.CreateCellComment(anchor_Limit);
        comment_Limit.String = new XSSFRichTextString($"HiHi Limit: {tag.HiHiSPLimit} {tag.Cfg_EU}");
        comment_Limit.Author = "CnE2PLC";
        row.GetCell(i).CellComment = comment_Limit;

        row.GetCell(i++).SetCellValue($"{tag.HiHiSP}");
        row.GetCell(i++).SetCellValue(tag.Cfg_EU);

        IClientAnchor anchor_Delay = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment_Delay = patriarch.CreateCellComment(anchor_Delay);
        comment_Delay.String = new XSSFRichTextString($"HiHi Delay: {tag.Cfg_HiHiDlyTmr} Sec.");
        comment_Delay.Author = "CnE2PLC";
        row.GetCell(i).CellComment = comment_Delay;

        row.GetCell(i++).SetCellValue($"{tag.Cfg_HiHiOnTmr} Sec.");
        row.GetCell(i++).SetCellValue($"{tag.Cfg_HiHiOffTmr} Sec.");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue(tag.HiHiAutoAck == true ? "Y" : "");
        row.GetCell(i++).SetCellValue("");

    }

    public static void ToHiAlarmRow(AIData tag, IRow row, IWorkbook WB)
    {

        int i = 0;
        IDrawing<IShape> patriarch = WB.GetSheetAt(0).CreateDrawingPatriarch();

        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.Hi_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        row.GetCell(i++).SetCellValue($"{tag.Name}.HiAlarm");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue((tag.HiEnable == true & tag.InUse == true) ? "Standard IO" : "Not In Use");
        row.GetCell(i++).SetCellValue($"{tag.HiSP}");
        row.GetCell(i++).SetCellValue(tag.Cfg_EU);

        IClientAnchor anchor_Delay = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment_Delay = patriarch.CreateCellComment(anchor_Delay);
        comment_Delay.String = new XSSFRichTextString($"Hi Delay: {tag.Cfg_HiDlyTmr} Sec.");
        comment_Delay.Author = "CnE2PLC";
        row.GetCell(i).CellComment = comment_Delay;

        row.GetCell(i++).SetCellValue($"{tag.Cfg_HiOnTmr} Sec.");
        row.GetCell(i++).SetCellValue($"{tag.Cfg_HiOffTmr} Sec.");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue(tag.HiAutoAck == true ? "Y" : "");

    }

    public static void ToLoAlarmRow(AIData tag, IRow row, IWorkbook WB)
    {

        int i = 0;
        IDrawing<IShape> patriarch = WB.GetSheetAt(0).CreateDrawingPatriarch();

        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.Lo_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        row.GetCell(i++).SetCellValue($"{tag.Name}.LoAlarm");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue((tag.LoEnable == true & tag.InUse == true) ? "Standard IO" : "Not In Use");
        row.GetCell(i++).SetCellValue($"{tag.LoSP}");
        row.GetCell(i++).SetCellValue(tag.Cfg_EU);

        IClientAnchor anchor_Delay = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment_Delay = patriarch.CreateCellComment(anchor_Delay);
        comment_Delay.String = new XSSFRichTextString($"Lo Delay: {tag.Cfg_LoDlyTmr} secs.");
        comment_Delay.Author = "CnE2PLC";
        row.GetCell(i).CellComment = comment_Delay;

        row.GetCell(i++).SetCellValue($"{tag.Cfg_LoOnTmr} Sec.");
        row.GetCell(i++).SetCellValue($"{tag.Cfg_LoOffTmr} Sec.");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue(tag.LoAutoAck == true ? "Y" : "");
        row.GetCell(i++).SetCellValue("");

    }

    public static void ToLoLoAlarmRow(AIData tag, IRow row, IWorkbook WB)
    {

        int i = 0;
        IDrawing<IShape> patriarch = WB.GetSheetAt(0).CreateDrawingPatriarch();

        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.LoLo_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        row.GetCell(i++).SetCellValue($"{tag.Name}.LoLoAlarm");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue((tag.LoLoEnable == true & tag.InUse == true) ? "Standard IO" : "Not In Use");

        IClientAnchor anchor_Limit = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment_Limit = patriarch.CreateCellComment(anchor_Limit);
        comment_Limit.String = new XSSFRichTextString($"LoLo Limit: {tag.LoLoSPLimit} {tag.Cfg_EU}");
        comment_Limit.Author = "CnE2PLC";
        row.GetCell(i).CellComment = comment_Limit;

        row.GetCell(i++).SetCellValue($"{tag.LoLoSP}");
        row.GetCell(i++).SetCellValue(tag.Cfg_EU);

        IClientAnchor anchor_Delay = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment_Delay = patriarch.CreateCellComment(anchor_Delay);
        comment_Delay.String = new XSSFRichTextString($"LoLo Delay: {tag.Cfg_LoLoDlyTmr} secs.");
        comment_Delay.Author = "CnE2PLC";
        row.GetCell(i).CellComment = comment_Delay;

        row.GetCell(i++).SetCellValue($"{tag.Cfg_LoLoOnTmr} Sec.");
        row.GetCell(i++).SetCellValue($"{tag.Cfg_LoLoOffTmr} Sec.");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue(tag.LoLoAutoAck == true ? "Y" : "");
        row.GetCell(i++).SetCellValue("");

    }

    public static void ToBadPVAlarmRow(AIData tag, IRow row, IWorkbook WB)
    {

        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.BadPV_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        row.GetCell(i++).SetCellValue($"{tag.Name}.BadPVAlarm");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue((tag.BadPVEnable == true & tag.InUse == true) ? "Standard IO" : "Not In Use");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("Bool");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue(tag.BadPVAutoAck == true ? "Y" : "");
        row.GetCell(i++).SetCellValue("");

    }

    #endregion

    #region AOData

    public static void ToColumn(AOData tag, IWorkbook WB, int col )
    {

        ISheet s = WB.GetSheetAt(0);
        
        // row 1

        // row 2 - 12
        IRow row = s.GetRow(1);
        row.GetCell(col).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);

        // row 13
        row = s.GetRow(12);
        row.GetCell(col).SetCellValue(tag.InUse == true ? "Yes" : "No");

        // row 14
        row = s.GetRow(13);
        row.GetCell(col).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(col).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.References == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        if (tag.Sim == true)
        {
            style.FillBackgroundColor = HSSFColor.DarkRed.Index;
            font.Color = HSSFColor.White.Index;
        }

        style.SetFont(font);
        row.GetCell(col).CellStyle = style;

        IDrawing<IShape> patriarch = WB.GetSheetAt(0).CreateDrawingPatriarch();
        IClientAnchor anchor = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment = patriarch.CreateCellComment(anchor);
        comment.String = new XSSFRichTextString(tag.ToString());
        comment.Author = "CnE2PLC";
        row.GetCell(col).CellComment = comment;

    }

    #endregion

    #region DIData

    public static void ToValueRow(DIData tag, IRow row, IWorkbook WB)
    {

        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.Val_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        if (tag.Sim == true)
        {
            style.FillBackgroundColor = HSSFColor.DarkRed.Index;
            font.Color = HSSFColor.White.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        row.GetCell(i++).SetCellValue($"{tag.Name}.Value");
        row.GetCell(i++).SetCellValue("Digital Input");
        row.GetCell(i++).SetCellValue(tag.InUse == true ? "Standard IO" : "Not In Use");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("Bool");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");

    }

    public static void ToAlarmRow(DIData tag, IRow row, IWorkbook WB)
    {
        int i = 0;
        row.GetCell(i++).SetCellValue( tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue( tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue( tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.Alm_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        row.GetCell(i++).SetCellValue( $"{tag.Name}.Alarm");
        row.GetCell(i++).SetCellValue( "AOI Output");
        row.GetCell(i++).SetCellValue( (tag.AlmEnable == true & tag.InUse == true) ? "Standard IO" : "Not In Use");
        row.GetCell(i++).SetCellValue( "");
        row.GetCell(i++).SetCellValue( "Bool");

        IDrawing<IShape> patriarch = WB.GetSheetAt(0).CreateDrawingPatriarch();
        IClientAnchor anchor_Delay = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment_Delay = patriarch.CreateCellComment(anchor_Delay);
        comment_Delay.String = new XSSFRichTextString($"Delay: {tag.Cfg_AlmDlyTmr} Sec.");
        comment_Delay.Author = "CnE2PLC";
        row.GetCell(i).CellComment = comment_Delay;

        row.GetCell(i++).SetCellValue( $"{tag.Cfg_AlmOnTmr} Sec.");
        row.GetCell(i++).SetCellValue( $"{tag.Cfg_AlmOffTmr} Sec.");
        row.GetCell(i++).SetCellValue( "");
        row.GetCell(i++).SetCellValue( "");
        row.GetCell(i++).SetCellValue( "");
        row.GetCell(i++).SetCellValue( "");
        row.GetCell(i++).SetCellValue( tag.AlmAutoAck == true ? "Y" : "");
        row.GetCell(i++).SetCellValue("");

    }

    public static void ToShutdownRow(DIData tag, IRow row, IWorkbook WB)
    {
        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.SD_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        row.GetCell(i++).SetCellValue($"{tag.Name}.Shutdown");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue((tag.AlmEnable == true & tag.InUse == true) ? "Standard IO" : "Not In Use");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("Bool");
        row.GetCell(i++).SetCellValue($"{tag.Cfg_SDDlyTmr} Sec.");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");

    }

    #endregion

    #region DOData

    public static void ToColumn(DOData tag, IWorkbook WB, int col)
    {

        ISheet s = WB.GetSheetAt(0);
        IRow row = s.GetRow(1);
        row.GetCell(col).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);

        row = s.GetRow(12);
        row.GetCell(col).SetCellValue(tag.InUse == true ? "Yes" : "No");

        row = s.GetRow(13);
        row.GetCell(col).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(col).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.References == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        if (tag.Sim == true)
        {
            style.FillBackgroundColor = HSSFColor.DarkRed.Index;
            font.Color = HSSFColor.White.Index;
        }

        style.SetFont(font);
        row.GetCell(col).CellStyle = style;

        IDrawing<IShape> patriarch = WB.GetSheetAt(0).CreateDrawingPatriarch();
        IClientAnchor anchor = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment = patriarch.CreateCellComment(anchor);
        comment.String = new XSSFRichTextString(tag.ToString());
        comment.Author = "CnE2PLC";
        row.GetCell(col).CellComment = comment;
    }

    #endregion

    #region Motor_VFD

    public static void ToFailToRunRow(Motor_VFD tag, IRow row, IWorkbook WB)
    {
        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue($"{tag.Name} Fail to Run");

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.FTR_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        row.GetCell(i++).SetCellValue($"{tag.Name}.AlarmFTR");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue("Standard IO");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("Bool");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");

    }

    public static void ToFailToStopRow(Motor_VFD tag, IRow row, IWorkbook WB)
    {
        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue($"{tag.Name} Fail to Stop");

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.FTS_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        row.GetCell(i++).SetCellValue($"{tag.Name}.AlarmFTS");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue("Standard IO");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("Bool");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");

    }

    #endregion

    #region ValveAnalog

    public static void ToColumn(ValveAnalog tag, IWorkbook WB, int col)
    {

        string c = $"Plc Tag Description: {tag.Description}\n";
        c += $"Plc Tag Datatype: {tag.DataType}\n";
        if (tag.DisableFB == true) c += "No Feedback.\n";
        if (tag.Cmd_Invert == true) c += "Command Inverted.\n";
        if (tag.FBInv == true) c += "Feedback Inverted.\n";

        ISheet s = WB.GetSheetAt(0);
        IRow row = s.GetRow(1);
        row.GetCell(col).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);

        row = s.GetRow(12);
        row.GetCell(col).SetCellValue(tag.InUse == true ? "Yes" : "No");

        row = s.GetRow(13);
        row.GetCell(col).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(col).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.References == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(col).CellStyle = style;

        IDrawing<IShape> patriarch = WB.GetSheetAt(0).CreateDrawingPatriarch();
        IClientAnchor anchor = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment = patriarch.CreateCellComment(anchor);
        comment.String = new XSSFRichTextString(c);
        comment.Author = "CnE2PLC";
        row.GetCell(col).CellComment = comment;

    }

    public static void ToPosRow(ValveAnalog tag, IRow row, IWorkbook WB)
    {
        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID); 
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.Pos_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        IDrawing<IShape> patriarch = WB.GetSheetAt(0).CreateDrawingPatriarch();
        IClientAnchor anchor = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment = patriarch.CreateCellComment(anchor);
        comment.String = new XSSFRichTextString($"Plc Tag Description: {tag.Description}\nPlc Data Type: {tag.DataType}\n");
        comment.Author = "CnE2PLC";
        row.GetCell(i).CellComment = comment;

        row.GetCell(i++).SetCellValue($"{tag.Name}.Pos");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue(tag.InUse == true ? "Standard IO" : "Not in Use");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("%");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");

    }

    public static void ToPosFailRow(ValveAnalog tag, IRow row, IWorkbook WB)
    {
        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.PosFail_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        row.GetCell(i++).SetCellValue($"{tag.Name}.PosFail");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue(tag.InUse == true ? "Standard IO" : "Not in Use");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("Bool");
        row.GetCell(i++).SetCellValue($"{tag.PosFaultTime} sec.");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");

    }

    #endregion

    #region TwoPositionValve

    public static void ToColumn(TwoPositionValveV2 tag, IWorkbook WB, int col)
    {
        string c = $"Plc Tag Description:\n{tag.Description}\n";
        c += $"Plc Tag Datatype: {tag.DataType}\n";
        if (tag.DisableFB == true) c += "No Feedback.\n";
        if (tag.FBInv == true) c += "Feedback Inverted.\n";

        ISheet s = WB.GetSheetAt(0);
        IRow row = s.GetRow(1);
        row.GetCell(col).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);

        row = s.GetRow(12);
        row.GetCell(col).SetCellValue(tag.InUse == true ? "Yes" : "No");

        row = s.GetRow(13);
        row.GetCell(col).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(col).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.References == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(col).CellStyle = style;

        IDrawing<IShape> patriarch = WB.GetSheetAt(0).CreateDrawingPatriarch();
        IClientAnchor anchor = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment = patriarch.CreateCellComment(anchor);
        comment.String = new XSSFRichTextString(c);
        comment.Author = "CnE2PLC";
        row.GetCell(col).CellComment = comment;

    }

    public static void ToOpenRow(TwoPositionValveV2 tag, IRow row, IWorkbook WB)
    {
        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.Open_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        IDrawing<IShape> patriarch = WB.GetSheetAt(0).CreateDrawingPatriarch();
        IClientAnchor anchor = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment = patriarch.CreateCellComment(anchor);
        comment.String = new XSSFRichTextString($"Plc Tag Description:\n{tag.Description}\nPlc Tag Datatype: {tag.DataType}\n");
        comment.Author = "CnE2PLC";
        row.GetCell(i).CellComment = comment;

        row.GetCell(i++).SetCellValue($"{tag.Name}.Open");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue(tag.InUse == true ? "Standard IO" : "Not in Use");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("Bool");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");

    }

    public static void ToCloseRow(TwoPositionValveV2 tag, IRow row, IWorkbook WB)
    {
        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.Close_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        IDrawing<IShape> patriarch = WB.GetSheetAt(0).CreateDrawingPatriarch();
        IClientAnchor anchor = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment = patriarch.CreateCellComment(anchor);
        comment.String = new XSSFRichTextString($"Plc Tag Description:\n{tag.Description}\nPlc Tag Datatype: {tag.DataType}\n");
        comment.Author = "CnE2PLC";
        row.GetCell(i).CellComment = comment;

        row.GetCell(i++).SetCellValue($"{tag.Name}.Close");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue(tag.InUse == true ? "Standard IO" : "Not in Use");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("Bool");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");

        }

    public static void ToFailedToOpenRow(TwoPositionValveV2 tag, IRow row, IWorkbook WB)
    {
        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.FTO_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        IDrawing<IShape> patriarch = WB.GetSheetAt(0).CreateDrawingPatriarch();
        IClientAnchor anchor = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment = patriarch.CreateCellComment(anchor);
        comment.String = new XSSFRichTextString($"Plc Tag Description:\n{tag.Description}\nPlc Tag Datatype: {tag.DataType}\n");
        comment.Author = "CnE2PLC";
        row.GetCell(i).CellComment = comment;

        row.GetCell(i++).SetCellValue($"{tag.Name}.FailedToOpen");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue(tag.InUse == true ? "Standard IO" : "Not in Use");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("Bool");
        row.GetCell(i++).SetCellValue($"{tag.OpenFaultTime} Sec.");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");

    }

    public static void ToFailedToCloseRow(TwoPositionValveV2 tag, IRow row, IWorkbook WB)
    {
        int i = 0;
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipID);
        row.GetCell(i++).SetCellValue(tag.Cfg_EquipDesc != string.Empty ? tag.Cfg_EquipDesc : tag.Description);
        row.GetCell(i++).SetCellValue(tag.Name);

        ICellStyle style = row.GetCell(i).CellStyle;
        IFont font = style.GetFont(WB);

        if (tag.FTC_Count == 0)
        {
            style.FillBackgroundColor = HSSFColor.Yellow.Index;
            font.Color = HSSFColor.Black.Index;
        }

        style.SetFont(font);
        row.GetCell(i).CellStyle = style;

        IDrawing<IShape> patriarch = WB.GetSheetAt(0).CreateDrawingPatriarch();
        IClientAnchor anchor = WB.GetCreationHelper().CreateClientAnchor();
        IComment comment = patriarch.CreateCellComment(anchor);
        comment.String = new XSSFRichTextString($"Plc Tag Description:\n{tag.Description}\nPlc Tag Datatype: {tag.DataType}\n");
        comment.Author = "CnE2PLC";
        row.GetCell(i).CellComment = comment;

        row.GetCell(i++).SetCellValue($"{tag.Name}.FailedToClose");
        row.GetCell(i++).SetCellValue("AOI Output");
        row.GetCell(i++).SetCellValue(tag.InUse == true ? "Standard IO" : "Not in Use");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("Bool");
        row.GetCell(i++).SetCellValue($"{tag.CloseFaultTime} Sec.");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue("");

    }

    #endregion


}