using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckmarxXMLReportToExcel.Entidades
{
    public class cCamposDeReporteExcel
    {
        public cCamposDeReporteExcel()
        {
            this.Vulnerabilidad = 0;           
            this.Estado = string.Empty;       
            this.Severidad = string.Empty;    
            this.NombreArchivo = string.Empty;
            this.Tipo = string.Empty;         
            this.Asignada = string.Empty;     
            this.Ruta = string.Empty;         
            this.Solucion = string.Empty;      
        }

        public int Vulnerabilidad { get; set; }

        public string Estado { get; set; }

        public string Severidad { get; set; }

        public string NombreArchivo { get; set; }

        public string Tipo { get; set; }

        public string Asignada { get; set; }

        public string Ruta { get; set; }

        public string Solucion { get; set; }
    }
}
