using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            LogText.Text = "Startup at time: " + DateTime.Now + "\n";
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse C&E Files",
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK) ProcessFile(openFileDialog1.FileName);

        }

        private void ProcessFile(string FileName)
        {
            try {
                LogText.AppendText("Opening File: " + FileName + "\n");
                var excelApp = new Excel.Application();
                excelApp.Visible = true;
                excelApp.Workbooks.Open(FileName);
                SelectSheet(excelApp.Workbooks);
                

            } catch { }

        }

        private void SelectSheet(Excel.Workbooks Sheets)
        {

        }



    }
}
