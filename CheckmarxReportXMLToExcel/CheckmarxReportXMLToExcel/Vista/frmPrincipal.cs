using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckmarxXMLReportToExcel.Negocio;
using CheckmarxXMLReportToExcel.Entidades;

namespace CheckmarxXMLReportToExcel
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtbPath.Text = "Select a Checkmarx XML report";
            btnLimpiar.Enabled = false;
        }

        string sPathArchivoXML = string.Empty;
        string sPathDestinoExcel = string.Empty;
        string sNombreDeArchivo = string.Empty;
        bool bArchivoSeleccionado = false;
        bool bExcelGenerado = false;

        private void BtnSelecionar_Click(object sender, EventArgs e)
        {                                    
            if (bArchivoSeleccionado)
            {
                LoadXML();
            }
            else
            {
                SelectXML();
            }            
        }

        private void LoadXML()
        {
            bArchivoSeleccionado = false;
            try
            {
                using (var fldrDlg = new FolderBrowserDialog())
                {                    
                    if (fldrDlg.ShowDialog() == DialogResult.OK)
                    {
                        sPathDestinoExcel = fldrDlg.SelectedPath;
                    }
                }

                List<cCamposDeReporteExcel> listaCampos = new List<cCamposDeReporteExcel>();
                ConvertXMLtoExcelBO convertToExcel = new ConvertXMLtoExcelBO();

                listaCampos = convertToExcel.readXML(sPathArchivoXML);

                DataTable dataTable = new DataTable();

                if (listaCampos.Count > 0)
                {
                    dataTable = convertToExcel.listToDataTable<cCamposDeReporteExcel>(listaCampos);
                }

                if (dataTable.Rows.Count > 0)
                {
                    bExcelGenerado = convertToExcel.writeAndSaveExcel(dataTable, sPathDestinoExcel, sNombreDeArchivo);
                }

                ShowMessageBox(bExcelGenerado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SelectXML()
        {
            openFileDialog.InitialDirectory = "C:\\";
            openFileDialog.Filter = "XML files (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    btnSelecionar.Text = "Generate Excel";
                    btnLimpiar.Enabled = true;
                    sPathArchivoXML = openFileDialog.FileName;
                    txtbPath.Text = sPathArchivoXML;
                    bArchivoSeleccionado = true;

                    string[] subs = sPathArchivoXML.Split('\\');

                    foreach (var sub in subs)
                    {
                        sNombreDeArchivo = sub;                        
                    }
                    char[] chars = { '.', 'x', 'm', 'l', 'X', 'M', 'L' };

                    sNombreDeArchivo = sNombreDeArchivo.TrimEnd(chars);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar() 
        {
            bArchivoSeleccionado = false;
            txtbPath.Text = "Select a Checkmarx XML report";
            btnSelecionar.Text = "Select";
            btnLimpiar.Enabled = false;
        }

        private void ShowMessageBox(bool sHecho)
        {
            if (sHecho)
            {
                var Result = MessageBox.Show("The Excel report has been successfully generated on this path: " + sPathDestinoExcel + "\\" + sNombreDeArchivo + ".xls", ":D", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (Result == DialogResult.OK)
                {
                    Limpiar();
                }
            }
            else
            {
                MessageBox.Show("It was not possible to generate the Excel report...", ":(",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void TxtbPath_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.CloseProgram();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CloseProgram()
        {
            this.Close();
        }
    }
}
