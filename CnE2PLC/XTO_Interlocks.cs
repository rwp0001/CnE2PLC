﻿using CnE2PLC;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{
    public class Intlk_8 : XTO_AOI
    {
        public Intlk_8() { }

        public Intlk_8(XmlNode node) : base(node) {
            Cfg_EquipID = L5K_strings[8];
            Cfg_EquipDesc = L5K_strings[9];
            for (int i = 0; i < 7; i++) Cfg_IntlkDesc[i] = L5K_strings[i];
        }

        public bool? Intlk00 {  get; set; }
        public bool? Intlk01 { get; set; }
        public bool? Intlk02 { get; set; }
        public bool? Intlk03 { get; set; }
        public bool? Intlk04 { get; set; }
        public bool? Intlk05 { get; set; }
        public bool? Intlk06 { get; set; }
        public bool? Intlk07 { get; set; }
        public int? IntlkBypass { get; set; }
        public bool? IntlkReset { get; set; }
        public bool? AckResetAll { get; set; }
        public int? Cfg_OKState { get; set; }
        public int? Cfg_AutoAck { get; set; }
        public int? IntlkActive { get; set; }
        public int? IntlkLatched { get; set; }
        public int? IntlkFirstOut { get; set; }
        public bool? IntlkOK { get; set; }
        public bool? BypActive { get; set; }
        public bool? ResetRdy { get; set; }
        public bool? FirstOut { get; set; }

        public string[] Cfg_IntlkDesc { get; set; } = new string[8];


        public override void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (this.BypActive == true)
            {
                e.CellStyle.BackColor = Color.Red;
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                return;
            }

            if (AOICalls == 0)
            {
                e.CellStyle.BackColor = Color.LightGray;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Italic);

            }

            if (References == 0)
            {
                e.CellStyle.ForeColor = Color.DarkCyan;
            }
            if (IntlkOK != true )
            {
                e.CellStyle.ForeColor = Color.Red;
            }

        }

        public static void ToHeaderRow(Excel.Range row)
        {
            int i = 1;
            row.Cells[1, i++].Value = "Scope";
            row.Cells[1, i++].Value = "Tag Name";
            row.Cells[1, i++].Value = "Tag Description";
            row.Cells[1, i++].Value = "HMI EquipID";
            row.Cells[1, i++].Value = "HMI EquipDesc";
            row.Cells[1, i++].Value = "Bypass";
            row.Cells[1, i++].Value = "OK State";
            //row.Cells[1, i++].Value = "Auto Ack";
            row.Cells[1, i++].Value = "Intlk Active";
            row.Cells[1, i++].Value = "Intlk Latched";
            row.Cells[1, i++].Value = "Intlk First Out";
            row.Cells[1, i++].Value = "Intlk OK";
            row.Cells[1, i++].Value = "Byp Active";
            row.Cells[1, i++].Value = "Reset Rdy";
            row.Cells[1, i++].Value = "First Out";
            row.Cells[1, i++].Value = "Intlk00";
            row.Cells[1, i++].Value = "Intlk01";
            row.Cells[1, i++].Value = "Intlk02";
            row.Cells[1, i++].Value = "Intlk03";
            row.Cells[1, i++].Value = "Intlk04";
            row.Cells[1, i++].Value = "Intlk05";
            row.Cells[1, i++].Value = "Intlk06";
            row.Cells[1, i++].Value = "Intlk07";
            for (int x = 0; x < 7; x++) row.Cells[1, i++].Value = $"Interlock: {x}";
        }

        public virtual void ToDataRow(Excel.Range row)
        {
            int i = 1;
            row.Cells[1, i++].Value = Path;
            row.Cells[1, i++].Value = Name;
            row.Cells[1, i++].Value = Description;
            row.Cells[1, i++].Value = Cfg_EquipID;
            row.Cells[1, i++].Value = Cfg_EquipDesc;
            row.Cells[1, i++].Value = IntlkBypass;
            row.Cells[1, i++].Value = Cfg_OKState;
            //row.Cells[1, i++].Value = Cfg_AutoAck;
            row.Cells[1, i++].Value = IntlkActive;
            row.Cells[1, i++].Value = IntlkLatched;
            row.Cells[1, i++].Value = IntlkFirstOut;
            row.Cells[1, i++].Value = IntlkOK;
            row.Cells[1, i++].Value = BypActive;
            row.Cells[1, i++].Value = ResetRdy;
            row.Cells[1, i++].Value = FirstOut;
            row.Cells[1, i++].Value = Intlk00;
            row.Cells[1, i++].Value = Intlk01;
            row.Cells[1, i++].Value = Intlk02;
            row.Cells[1, i++].Value = Intlk03;
            row.Cells[1, i++].Value = Intlk04;
            row.Cells[1, i++].Value = Intlk05;
            row.Cells[1, i++].Value = Intlk06;
            row.Cells[1, i++].Value = Intlk07;

            for (int x = 0; x < 7; x++) row.Cells[1, i++].Value = Cfg_IntlkDesc[x];
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void ClearCounts() { }
    }

    public class Intlk_16 : Intlk_8
    {


        public Intlk_16() { }

        public Intlk_16(XmlNode node) : base(node) {
            Cfg_EquipID = L5K_strings[16];
            Cfg_EquipDesc = L5K_strings[17];
            Cfg_IntlkDesc = new string[16];
            for (int i = 0; i < 15; i++) Cfg_IntlkDesc[i] = L5K_strings[i];
        }

        public static void ToHeaderRow(Excel.Range row)
        {
            int i = 1;
            row.Cells[1, i++].Value = "Scope";
            row.Cells[1, i++].Value = "Tag Name";
            row.Cells[1, i++].Value = "Tag Description";
            row.Cells[1, i++].Value = "HMI EquipID";
            row.Cells[1, i++].Value = "HMI EquipDesc";
            row.Cells[1, i++].Value = "Bypass";
            //row.Cells[1, i++].Value = "OK State";
            row.Cells[1, i++].Value = "Auto Ack";
            row.Cells[1, i++].Value = "Intlk Active";
            row.Cells[1, i++].Value = "Intlk Latched";
            row.Cells[1, i++].Value = "Intlk First Out";
            row.Cells[1, i++].Value = "Intlk OK";
            row.Cells[1, i++].Value = "Byp Active";
            row.Cells[1, i++].Value = "Reset Rdy";
            row.Cells[1, i++].Value = "First Out";
            row.Cells[1, i++].Value = "Intlk00";
            row.Cells[1, i++].Value = "Intlk01";
            row.Cells[1, i++].Value = "Intlk02";
            row.Cells[1, i++].Value = "Intlk03";
            row.Cells[1, i++].Value = "Intlk04";
            row.Cells[1, i++].Value = "Intlk05";
            row.Cells[1, i++].Value = "Intlk06";
            row.Cells[1, i++].Value = "Intlk07";
            row.Cells[1, i++].Value = "Intlk08";
            row.Cells[1, i++].Value = "Intlk09";
            row.Cells[1, i++].Value = "Intlk10";
            row.Cells[1, i++].Value = "Intlk11";
            row.Cells[1, i++].Value = "Intlk12";
            row.Cells[1, i++].Value = "Intlk13";
            row.Cells[1, i++].Value = "Intlk14";
            row.Cells[1, i++].Value = "Intlk15";
            for (int x = 0; x < 15; x++) row.Cells[1, i++].Value = $"Interlock: {x}";
        }

        public override void ToDataRow(Excel.Range row)
        {
            int i = 1;
            row.Cells[1, i++].Value = Path;
            row.Cells[1, i++].Value = Name;
            row.Cells[1, i++].Value = Description;
            row.Cells[1, i++].Value = Cfg_EquipID;
            row.Cells[1, i++].Value = Cfg_EquipDesc;
            row.Cells[1, i++].Value = IntlkBypass;
            row.Cells[1, i++].Value = Cfg_OKState;
            //row.Cells[1, i++].Value = Cfg_AutoAck;
            row.Cells[1, i++].Value = IntlkActive;
            row.Cells[1, i++].Value = IntlkLatched;
            row.Cells[1, i++].Value = IntlkFirstOut;
            row.Cells[1, i++].Value = IntlkOK;
            row.Cells[1, i++].Value = BypActive;
            row.Cells[1, i++].Value = ResetRdy;
            row.Cells[1, i++].Value = FirstOut;
            row.Cells[1, i++].Value = Intlk00;
            row.Cells[1, i++].Value = Intlk01;
            row.Cells[1, i++].Value = Intlk02;
            row.Cells[1, i++].Value = Intlk03;
            row.Cells[1, i++].Value = Intlk04;
            row.Cells[1, i++].Value = Intlk05;
            row.Cells[1, i++].Value = Intlk06;
            row.Cells[1, i++].Value = Intlk07;
            row.Cells[1, i++].Value = Intlk08;
            row.Cells[1, i++].Value = Intlk09;
            row.Cells[1, i++].Value = Intlk10;
            row.Cells[1, i++].Value = Intlk11;
            row.Cells[1, i++].Value = Intlk12;
            row.Cells[1, i++].Value = Intlk13;
            row.Cells[1, i++].Value = Intlk14;
            row.Cells[1, i++].Value = Intlk15;

            for (int x = 0; x < 15; x++) row.Cells[1, i++].Value = Cfg_IntlkDesc[x];
        }



        public bool? Intlk08 { get; set; }
        public bool? Intlk09 { get; set; }
        public bool? Intlk10 { get; set; }
        public bool? Intlk11 { get; set; }
        public bool? Intlk12 { get; set; }
        public bool? Intlk13 { get; set; }
        public bool? Intlk14 { get; set; }
        public bool? Intlk15 { get; set; }


    }
}
