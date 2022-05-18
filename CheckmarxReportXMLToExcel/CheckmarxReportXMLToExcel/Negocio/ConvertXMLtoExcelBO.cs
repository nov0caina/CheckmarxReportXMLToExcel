using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using CheckmarxXMLReportToExcel.Entidades;
using SpreadsheetLight;

namespace CheckmarxXMLReportToExcel.Negocio
{
    public class ConvertXMLtoExcelBO
    {
        public ConvertXMLtoExcelBO()
        {
            
        }

        public bool writeAndSaveExcel(DataTable dataTable, string sPathDestinoExcel, string sNombreDeArchivo)
        {
            bool bHecho = false;

            try
            {
                SLDocument oSLDocument = new SLDocument();
                oSLDocument.ApplyNamedCellStyle("A1", SLNamedCellStyleValues.Heading4);
                oSLDocument.ApplyNamedCellStyle("B1", SLNamedCellStyleValues.Heading4);
                oSLDocument.ApplyNamedCellStyle("C1", SLNamedCellStyleValues.Heading4);
                oSLDocument.ApplyNamedCellStyle("D1", SLNamedCellStyleValues.Heading4);
                oSLDocument.ApplyNamedCellStyle("E1", SLNamedCellStyleValues.Heading4);
                oSLDocument.ApplyNamedCellStyle("F1", SLNamedCellStyleValues.Heading4);
                oSLDocument.ApplyNamedCellStyle("G1", SLNamedCellStyleValues.Heading4);
                oSLDocument.ApplyNamedCellStyle("H1", SLNamedCellStyleValues.Heading4);
                
                oSLDocument.ImportDataTable(1, 1, dataTable, true);
                oSLDocument.SaveAs(sPathDestinoExcel + "\\" + sNombreDeArchivo + ".xlsx");

                bHecho = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return bHecho;
        }

        public  DataTable listToDataTable<cCamposDeReporteExcel>(List<cCamposDeReporteExcel> listParametros)
        {
            Type elementType = typeof(cCamposDeReporteExcel);
            DataTable table = new DataTable();            

            try
            {
                if (listParametros.Count > 0)
                {
                    foreach (var propInfo in elementType.GetProperties())
                    {
                        Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                        table.Columns.Add(propInfo.Name, ColType);
                    }

                    foreach (cCamposDeReporteExcel item in listParametros)
                    {
                        DataRow row = table.NewRow();

                        foreach (var propInfo in elementType.GetProperties())
                        {
                            row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                        }

                        table.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {        
                throw ex;
            }

            return table;
        }

        public List<cCamposDeReporteExcel> readXML(string sPathArchivo)
        {
            int iChildNodesQuery = 0;
            int iChilNodesResult = 0;
            int iConteo = 0;
            StringBuilder xmlResp = null;
            XmlDocument xmlDoc = null;
            XmlNamespaceManager nsmgr = null;
            XmlNode node;
            XmlNode nodeChildQuery;
            XmlNode nodeChildResult;
            XmlNodeList nodoQuery;
            XmlNodeList nodoResult;
                    
            List<cCamposDeReporteExcel> listReporte = null;
            cCamposDeReporteExcel cCamposReporte = null;


            if (sPathArchivo == string.Empty)
            {
                throw new Exception("La ruta del XML no se leyó correctamente.");
            }

            var xReporte = XDocument.Load(sPathArchivo);
            List<cXML> listXML = new List<cXML>();
            cXML cXML = new cXML();
            cXML.sXml = xReporte.ToString();
            listXML.Add(cXML);

            if (listXML == null)
            {
                throw new Exception("Ocurió un error al leer el XML.");
            }

            foreach (var sXml in listXML)
            {
                xmlResp = new StringBuilder(sXml.sXml);
                listReporte = new List<cCamposDeReporteExcel>();
                
                
                if (xmlResp.ToString() == string.Empty)
                {
                    throw new Exception("Ocurió un error al leer el XML.");
                }

                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlResp.ToString());
                nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);

                node = xmlDoc.SelectSingleNode("CxXMLResults", nsmgr);

                if (node == null)
                {
                    throw new Exception("Ocurrió un problema al leer los nodos del XML.");
                }

                iChildNodesQuery = node.ChildNodes.Count;

                nodoQuery = node.SelectNodes("Query", nsmgr);
                
                for (int i = 0; i < iChildNodesQuery; i++)
                {
                    
                    nodeChildQuery = nodoQuery.Item(i);        

                    iChilNodesResult = nodeChildQuery.ChildNodes.Count;

                    nodoResult = nodeChildQuery.SelectNodes("Result", nsmgr);
                    
                    for (int j = 0; j < iChilNodesResult; j++)
                    {
                        iConteo++;

                        cCamposReporte = new cCamposDeReporteExcel();

                        if (nodeChildQuery.Attributes["name"].Value.Trim() != null)
                        {
                            cCamposReporte.Tipo = nodeChildQuery.Attributes["name"].Value.Trim();
                        }

                        cCamposReporte.Estado = "Pendiente";
                        
                        cCamposReporte.Vulnerabilidad = iConteo;

                        nodeChildResult = nodoResult.Item(j);

                        if(nodeChildResult.Attributes["Severity"].Value.Trim() != null)
                        {
                            cCamposReporte.Severidad = nodeChildResult.Attributes["Severity"].Value.Trim();
                        }
                        if (nodeChildResult.Attributes["FileName"].Value.Trim() != null)
                        {
                            cCamposReporte.NombreArchivo = nodeChildResult.Attributes["FileName"].Value.Trim();
                        }
                        if (nodeChildResult.Attributes["DeepLink"].Value.Trim() != null)
                        {
                            cCamposReporte.Ruta = nodeChildResult.Attributes["DeepLink"].Value.Trim();
                        }

                        if (cCamposReporte.Tipo != string.Empty && cCamposReporte.NombreArchivo != string.Empty && cCamposReporte.Ruta != string.Empty)
                        {
                            listReporte.Add(cCamposReporte);
                        }
                    }                    
                }
            }

            return listReporte;
        }
    }
}
