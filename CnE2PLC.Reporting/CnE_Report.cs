using CnE2PLC.PLC;
using CnE2PLC.PLC.XTO;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Diagnostics;

namespace CnE2PLC.Reporting;

public static class CnE_Report
{

    /// <summary>
    /// Exports the Tags to a Excel Templete.
    /// </summary>
    /// <param name="plc">PLC controller object to export from.</param>
    /// <param name="File">File stream to write the content out.</param>
    public static void CreateReport(Controller plc, Stream File )
    {
        try
        {

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
                        /*
                        case "dodata":
                            DOData DoTag = (DOData)tag;
                            
                            InsertCol(CnE_Sheet, ColumnOffset);
                            DoTag.ToColumn(CnE_Sheet.Columns[ColumnOffset]);
                            Excel.Range r1 = CnE_Sheet.Range[CnE_Sheet.Cells[2, ColumnOffset], CnE_Sheet.Cells[12, ColumnOffset]];
                            MergeAndCenter(r1);
                            ColumnOffset++;
                            break;
                        */
                        case "aodata":
                            AOData AoTag = (AOData)tag;
                            
                            InsertCol(CnE_Sheet, ColumnOffset);
                            ToColumn((AOData)tag, CnE_Workbook, ColumnOffset);
                            ColumnOffset++;
                            break;
                    /*
                        case "didata":
                        case "didata_fims":
                            DIData DiTag = (DIData)tag;
                            
                            InsertRow(CnE_Sheet, RowOffset);
                            DiTag.ToValueRow(CnE_Sheet.Rows[RowOffset++]);

                            if (DiTag.AlmEnable == true)
                            {
                                InsertRow(CnE_Sheet, RowOffset);
                                DiTag.ToAlarmRow(CnE_Sheet.Rows[RowOffset++]);

                                if (DiTag.SD_Count != 0)
                                {
                                    InsertRow(CnE_Sheet, RowOffset);
                                    DiTag.ToShutdownRow(CnE_Sheet.Rows[RowOffset++]);
                                }
                            }
                            break;

                        */

                        case "aidata":
                        case "aidata_fims":
                            InsertRow(CnE_Sheet, RowOffset);
                            ToPVRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);

                            if (((AIData)tag).HiHiEnable == true)
                            {
                                if (((AIData)tag).HSD_Count != 0)
                                {
                                    InsertRow(CnE_Sheet, RowOffset);
                                    ToHSDRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                                }

                                InsertRow(CnE_Sheet, RowOffset);
                                ToHiHiAlarmRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            }
                            if (((AIData)tag).HiEnable == true)
                            {
                                InsertRow(CnE_Sheet, RowOffset);
                                ToHiAlarmRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            }
                            if (((AIData)tag).LoEnable == true)
                            {
                                InsertRow(CnE_Sheet, RowOffset);
                                ToLoAlarmRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            }
                            if (((AIData)tag).LoLoEnable == true)
                            {
                                InsertRow(CnE_Sheet, RowOffset);
                                ToLoLoAlarmRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);

                                if (((AIData)tag).LSD_Count != 0)
                                {
                                    InsertRow(CnE_Sheet, RowOffset);
                                    ToLSDRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                                }

                            }
                            if (((AIData)tag).BadPVEnable == true)
                            {
                                InsertRow(CnE_Sheet, RowOffset);
                                ToBadPVAlarmRow((AIData)tag, CnE_Sheet.GetRow(RowOffset++), CnE_Workbook);
                            }
                            break;

                        /*
                    case "twopositionvalve":
                    case "twopositionvalvev2":
                        TwoPositionValveV2 TPV2Tag = (TwoPositionValveV2)tag;

                        InsertCol(CnE_Sheet, ColumnOffset);
                        TPV2Tag.ToColumn(CnE_Sheet.Columns[ColumnOffset]);
                        Excel.Range r3 = CnE_Sheet.Range[CnE_Sheet.Cells[2, ColumnOffset], CnE_Sheet.Cells[12, ColumnOffset]];
                        MergeAndCenter(r3);
                        ColumnOffset++;

                        if (TPV2Tag.DisableFB != true)
                        {


                            //if (TPV2Tag.Open_Count != 0)
                            {
                                InsertRow(CnE_Sheet, RowOffset);
                                TPV2Tag.ToOpenRow(CnE_Sheet.Rows[RowOffset++]);
                            }

                            //if (TPV2Tag.Close_Count != 0)
                            {
                                InsertRow(CnE_Sheet, RowOffset);
                                TPV2Tag.ToCloseRow(CnE_Sheet.Rows[RowOffset++]);
                            }

                            //if (TPV2Tag.FTO_Count != 0)
                            {
                                InsertRow(CnE_Sheet, RowOffset);
                                TPV2Tag.ToFailedToOpenRow(CnE_Sheet.Rows[RowOffset++]);
                            }

                            //if (TPV2Tag.FTO_Count != 0)
                            {
                                InsertRow(CnE_Sheet, RowOffset);
                                TPV2Tag.ToFailedToCloseRow(CnE_Sheet.Rows[RowOffset++]);
                            }
                        }


                        break;

                    case "valveanalog":
                        ValveAnalog AAVTag = (ValveAnalog)tag;
                        CnE_Sheet.Columns[ColumnOffset].Insert();
                        AAVTag.ToColumn(CnE_Sheet.Columns[ColumnOffset]);
                        Excel.Range r4 = CnE_Sheet.Range[CnE_Sheet.Cells[2, ColumnOffset], CnE_Sheet.Cells[12, ColumnOffset]];
                        MergeAndCenter(r4);
                        ColumnOffset++;

                        if (AAVTag.DisableFB != true)
                        {
                            //if (AAVTag.Pos_Count != 0)
                            {
                                CnE_Sheet.Rows[RowOffset].Insert();
                                AAVTag.ToPosRow(CnE_Sheet.Rows[RowOffset++]);
                            }
                            //if (AAVTag.PosFail_Count != 0)
                            {
                                CnE_Sheet.Rows[RowOffset].Insert();
                                AAVTag.ToPosFailRow(CnE_Sheet.Rows[RowOffset++]);
                            }
                        }
                        break;

                    case "motor_vfd":
                        Motor_VFD motor_VFD = (Motor_VFD)tag;



                        break;

                        */

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"CnE: Tag Exception Error: {ex.Message}\nTage Name: {tag.Name}");
                }

            }

            // clean up for filtering.
            //CnE_Sheet.Rows[16].Delete();
            //CnE_Sheet.Columns[18].Delete();


            CnE_Workbook.Write(File, true);



        }
        catch (Exception e)
        {
            Debug.WriteLine($"CnE: Create Report Exception: {e.Message}");
        }

        static void InsertRow(ISheet ws, int offset)
        {
            
            ws.CopyRow(offset, offset + 1);
        }

        static void InsertCol(ISheet ws, int offset)
        {
            
            CopyColumn(ws, offset, offset + 1);
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
        row.GetCell(i++).SetCellValue("");
        row.GetCell(i++).SetCellValue(tag.BadPVAutoAck == true ? "Y" : "");
        row.GetCell(i++).SetCellValue("");

    }

    #endregion

    #region AOData

    public static void ToColumn(AOData tag, IWorkbook WB, int col )
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
        comment_Delay.String = new XSSFRichTextString($"HiHi Delay: {tag.Cfg_AlmDlyTmr} Sec.");
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
        row.GetCell(i++).SetCellValue("Not In Use");
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

    //public void ToRunFailRow(Excel.Range row)
    //{
    //    row.Cells[1, 1].Value = Cfg_EquipID;
    //    row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
    //    row.Cells[1, 3].Value = Name;
    //    row.Cells[1, 4].Value = $"{Name}.FailToRun";
    //    row.Cells[1, 5].Value = "AOI Output";
    //    row.Cells[1, 6].Value = InUse == true ? "Standard IO" : "Not In Use";
    //    row.Cells[1, 7].Value = "";
    //    row.Cells[1, 8].Value = "Bool";
    //    row.Cells[1, 9].Value = "";
    //    row.Cells[1, 10].Value = "";
    //    row.Cells[1, 11].Value = "";
    //    row.Cells[1, 12].Value = "";
    //    row.Cells[1, 13].Value = "";
    //    row.Cells[1, 14].Value = "";
    //    row.Cells[1, 15].Value = "";
    //    row.Cells[1, 16].Value = "";

    //    //if (PosFail_Count == 0)
    //    //{
    //    //    {
    //    //        row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
    //    //        row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
    //    //    }
    //    //}

    //}

    #endregion

    #region AnalogValve
    //private string ColComment
    //{
    //    get
    //    {
    //        string c = $"PLC Tag Description: {Description}\n";
    //        c += $"PLC Tag DataType: {DataType}\n";
    //        if (DisableFB == true) c += "No Feedback.\n";
    //        if (Cmd_Invert == true) c += "Command Inverted.\n";
    //        if (FBInv == true) c += "Feedback Inverted.\n";
    //        return c;
    //    }
    //}

    //private string RowComment
    //{
    //    get
    //    {
    //        string c = $"PLC Tag Description: {Description}\n";
    //        c += $"PLC Data Type: {DataType}\n";
    //        return c;
    //    }
    //}

    //public void ToColumn(Excel.Range col)
    //{
    //    col.Cells[2, 1].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
    //    col.Cells[13, 1].Value = InUse == true ? "Yes" : "No";
    //    col.Cells[14, 1].Value = Name;

    //    col.Cells[14, 1].AddComment(ColComment);

    //}

    //public void ToPosRow(Excel.Range row)
    //{
    //    row.Cells[1, 1].Value = Cfg_EquipID;
    //    row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
    //    row.Cells[1, 3].Value = Name;
    //    row.Cells[1, 4].Value = $"{Name}.Pos";
    //    row.Cells[1, 5].Value = "AOI Output";
    //    row.Cells[1, 6].Value = InUse == true ? "Standard IO" : "Not In Use";
    //    row.Cells[1, 7].Value = "";
    //    row.Cells[1, 8].Value = "%";
    //    row.Cells[1, 9].Value = "";
    //    row.Cells[1, 10].Value = "";
    //    row.Cells[1, 11].Value = "";
    //    row.Cells[1, 12].Value = "";
    //    row.Cells[1, 13].Value = "";
    //    row.Cells[1, 14].Value = "";
    //    row.Cells[1, 15].Value = "";
    //    row.Cells[1, 16].Value = "";

    //    if (Pos_Count == 0)
    //    {
    //        {
    //            row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
    //            row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
    //        }
    //    }

    //    row.Cells[1, 3].AddComment(RowComment);
    //}

    //public void ToPosFailRow(Excel.Range row)
    //{
    //    row.Cells[1, 1].Value = Cfg_EquipID;
    //    row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
    //    row.Cells[1, 3].Value = Name;
    //    row.Cells[1, 4].Value = $"{Name}.PosFail";
    //    row.Cells[1, 5].Value = "AOI Output";
    //    row.Cells[1, 6].Value = InUse == true ? "Standard IO" : "Not In Use";
    //    row.Cells[1, 7].Value = "";
    //    row.Cells[1, 8].Value = "Bool";
    //    row.Cells[1, 9].Value = $"{PosFaultTime} Sec.";
    //    row.Cells[1, 10].Value = "";
    //    row.Cells[1, 11].Value = "";
    //    row.Cells[1, 12].Value = "";
    //    row.Cells[1, 13].Value = "";
    //    row.Cells[1, 14].Value = "";
    //    row.Cells[1, 15].Value = "";
    //    row.Cells[1, 16].Value = "";

    //    if (PosFail_Count == 0)
    //    {
    //        {
    //            row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
    //            row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
    //        }
    //    }

    //}

    #endregion

    #region TwoPositionValve

    //public void ToColumn(Excel.Range col)
    //{
    //    col.Cells[2, 1].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
    //    col.Cells[13, 1].Value = InUse == true ? "Yes" : "No";
    //    col.Cells[14, 1].Value = Name;

    //    //comments
    //    string c = $"PLC Tag Description:\n{Description}\n";
    //    c += $"PLC Tag DataType: {DataType}\n";
    //    if (DisableFB == true) c += "No Feedback.\n";
    //    if (FBInv == true) c += "Feedback Inverted.\n";
    //    col.Cells[14, 1].AddComment(c);

    //}

    //public void ToOpenRow(Excel.Range row)
    //{
    //    row.Cells[1, 1].Value = Cfg_EquipID;
    //    row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
    //    row.Cells[1, 3].Value = Name;
    //    row.Cells[1, 4].Value = string.Format("{0}.Open", Name);
    //    row.Cells[1, 5].Value = "AOI Output";
    //    row.Cells[1, 6].Value = InUse == true ? "Standard IO" : "Not In Use";
    //    row.Cells[1, 7].Value = "";
    //    row.Cells[1, 8].Value = "Bool";
    //    row.Cells[1, 9].Value = "";
    //    row.Cells[1, 10].Value = "";
    //    row.Cells[1, 11].Value = "";
    //    row.Cells[1, 12].Value = "";
    //    row.Cells[1, 13].Value = "";
    //    row.Cells[1, 14].Value = "";
    //    row.Cells[1, 15].Value = "";
    //    row.Cells[1, 16].Value = "";

    //    if (Open_Count == 0)
    //    {
    //        {
    //            row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
    //            row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
    //        }
    //    }

    //    // comments
    //    string c = $"PLC Tag Description:\n{Description}\n";
    //    c += $"PLC Data Type:\n{DataType}\n";
    //    row.Cells[1, 3].AddComment(c);
    //}

    //public void ToCloseRow(Excel.Range row)
    //{
    //    row.Cells[1, 1].Value = Cfg_EquipID;
    //    row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
    //    row.Cells[1, 3].Value = Name;
    //    row.Cells[1, 4].Value = $"{Name}.Close";
    //    row.Cells[1, 5].Value = "AOI Output";
    //    row.Cells[1, 6].Value = InUse == true ? "Standard IO" : "Not In Use";
    //    row.Cells[1, 7].Value = "";
    //    row.Cells[1, 8].Value = "Bool";
    //    row.Cells[1, 9].Value = "";
    //    row.Cells[1, 10].Value = "";
    //    row.Cells[1, 11].Value = "";
    //    row.Cells[1, 12].Value = "";
    //    row.Cells[1, 13].Value = "";
    //    row.Cells[1, 14].Value = "";
    //    row.Cells[1, 15].Value = "";
    //    row.Cells[1, 16].Value = "";

    //    if (Close_Count == 0)
    //    {
    //        {
    //            row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
    //            row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
    //        }
    //    }

    //}

    //public void ToFailedToOpenRow(Excel.Range row)
    //{
    //    row.Cells[1, 1].Value = Cfg_EquipID;
    //    row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
    //    row.Cells[1, 3].Value = Name;
    //    row.Cells[1, 4].Value = $"{Name}.FailedToOpen";
    //    row.Cells[1, 5].Value = "AOI Output";
    //    row.Cells[1, 6].Value = InUse == true ? "Standard IO" : "Not In Use";
    //    row.Cells[1, 7].Value = "";
    //    row.Cells[1, 8].Value = "Bool";
    //    row.Cells[1, 9].Value = string.Format("{0} Sec.", OpenFaultTime);
    //    row.Cells[1, 10].Value = "";
    //    row.Cells[1, 11].Value = "";
    //    row.Cells[1, 12].Value = "";
    //    row.Cells[1, 13].Value = "";
    //    row.Cells[1, 14].Value = "";
    //    row.Cells[1, 15].Value = "";
    //    row.Cells[1, 16].Value = "";

    //    if (FTO_Count == 0)
    //    {
    //        {
    //            row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
    //            row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
    //        }
    //    }
    //}

    //public void ToFailedToCloseRow(Excel.Range row)
    //{
    //    row.Cells[1, 1].Value = Cfg_EquipID;
    //    row.Cells[1, 2].Value = Cfg_EquipDesc != string.Empty ? Cfg_EquipDesc : Description;
    //    row.Cells[1, 3].Value = Name;
    //    row.Cells[1, 4].Value = $"{Name}.FailedToClose";
    //    row.Cells[1, 5].Value = "AOI Output";
    //    row.Cells[1, 6].Value = InUse == true ? "Standard IO" : "Not In Use";
    //    row.Cells[1, 7].Value = "";
    //    row.Cells[1, 8].Value = "Bool";
    //    row.Cells[1, 9].Value = $"{CloseFaultTime} Sec.";
    //    row.Cells[1, 10].Value = "";
    //    row.Cells[1, 11].Value = "";
    //    row.Cells[1, 12].Value = "";
    //    row.Cells[1, 13].Value = "";
    //    row.Cells[1, 14].Value = "";
    //    row.Cells[1, 15].Value = "";
    //    row.Cells[1, 16].Value = "";

    //    if (FTC_Count == 0)
    //    {
    //        {
    //            row.Cells[1, 4].Interior.Color = ColorTranslator.ToOle(Color.Yellow);
    //            row.Cells[1, 4].Font.Color = ColorTranslator.ToOle(Color.Black);
    //        }
    //    }
    //}

    #endregion








}