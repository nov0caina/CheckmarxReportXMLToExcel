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
            this.Vulnerability = 0;           
            this.Condition = string.Empty;       
            this.Severity = string.Empty;    
            this.FileName = string.Empty;
            this.Type = string.Empty;         
            this.Assigned = string.Empty;     
            this.Path = string.Empty;         
            this.Solution = string.Empty;      
        }

        public int Vulnerability { get; set; }

        public string Condition { get; set; }

        public string Severity { get; set; }

        public string FileName { get; set; }

        public string Type { get; set; }

        public string Assigned { get; set; }

        public string Path { get; set; }

        public string Solution { get; set; }
    }
}
