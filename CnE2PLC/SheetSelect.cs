using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{
    public partial class SheetSelect : Form
    {
        public SheetSelect(Excel.Worksheets In)
        {
            ws = In;
            InitializeComponent();
        }

        public Worksheets ws;

        public string SheetName { get; set; }
        public int SheetIndex { get; set; }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Visible = false;
        }

        private void Select_Click(object sender, EventArgs e)
        {
            SheetIndex = Selection.SelectedIndex;
            SheetName = Selection.SelectedText;
            this.DialogResult = DialogResult.OK;
            this.Visible = false;
        }

        private void Selection_SelectedIndexChanged(object sender, EventArgs e)
        {
            SheetIndex = Selection.SelectedIndex;
            SheetName = Selection.SelectedText;
        }
    }
}
