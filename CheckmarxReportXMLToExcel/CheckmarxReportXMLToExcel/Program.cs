using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CheckmarxXMLReportToExcel
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]

        static void Main()
        {
            iniciar();
        }

        static void iniciar()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmPrincipal());
            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        } 
    }
}
