using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.ComponentModel;
using System.Globalization;


namespace SistemaFinanciero
{
    public partial class WebFormPrinterCountCash : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void BtnImprimir_Click(object sender, EventArgs e)
        {
            List<DetallePago> ListaDetallePago = new List<DetallePago>();
           
            Pago pago = new Pago();
            pago.NumeroRecibo = 10;
            pago.NombreEstudiante = "La peach";
            pago.Monto = 1250;

            DetallePago dt1 = new DetallePago();
            dt1.Producto = "Calzonsillo";
            dt1.Precio = 250;
            ListaDetallePago.Add(dt1);

            DetallePago dt2 = new DetallePago();
            dt2.Producto = "Perfume";
            dt2.Precio = 1000;
            ListaDetallePago.Add(dt2);

            ImprimirReporte(pago,ListaDetallePago);
            

        }

        void ImprimirReporte(Pago pago,List<DetallePago> detalles)
        {
            // Tamaño de prueba
            float anchoRecibo = 226.77f;
            float altoRecibo = 425.20f;

            Rectangle tamanioPersonalizado = new Rectangle(anchoRecibo, altoRecibo);

            Document document = new Document(tamanioPersonalizado);
            PdfWriter writer = PdfWriter.GetInstance(document, HttpContext.Current.Response.OutputStream);

            document.Open();
            Font fontTitle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10);
            Font font9 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10);

            Paragraph Titulo1 = new Paragraph("Instituto Pedagógico La Salle", new Font(Font.FontFamily.TIMES_ROMAN, 12));
            Titulo1.Alignment = Element.ALIGN_CENTER;
            document.Add(Titulo1);

            Paragraph Titulo2 = new Paragraph("Reporte detallado Caja general", new Font(Font.FontFamily.TIMES_ROMAN, 11));
            Titulo2.Alignment = Element.ALIGN_CENTER;
            document.Add(Titulo2);

            Paragraph Titulo = new Paragraph("N.Recibo: " + pago.NumeroRecibo.ToString() + "     " + pago.NombreEstudiante.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10));
            Titulo.Alignment = Element.ALIGN_CENTER;
            document.Add(Titulo);

            document.Add(new Chunk("\n"));


            Paragraph EncabezadoTabla = new Paragraph("Detalle de pagos", new Font(Font.FontFamily.TIMES_ROMAN, 10));
            EncabezadoTabla.Alignment = Element.ALIGN_CENTER;
            document.Add(EncabezadoTabla);

            document.Add(new Chunk("\n"));

            DataTable dt = new DataTable();
            dt = ConvertirListaToDataTable(detalles);

            PdfPTable table = new PdfPTable(dt.Columns.Count);

            float[] widths = new float[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
                widths[i] = 4f;

            table.SetWidths(widths);
            table.WidthPercentage = 90;

            PdfPCell cell = new PdfPCell(new Phrase("columns"));
            cell.Colspan = dt.Columns.Count;

            foreach (DataColumn c in dt.Columns)
            {
                table.AddCell(new Phrase(c.ColumnName, font9));
            }

            foreach (DataRow r in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int h = 0; h < dt.Columns.Count; h++)
                    {
                        table.AddCell(new Phrase(r[h].ToString(), font9));
                    }
                }
            }
            document.Add(table);

            document.Add(new Chunk("\n"));

            Paragraph P11 = new Paragraph("Total Factura: C$ " + pago.Monto, new Font(Font.FontFamily.TIMES_ROMAN, 10));
            P11.Alignment = Element.ALIGN_LEFT;
            document.Add(P11);

            document.Add(new Chunk("\n"));


            document.Close();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Recibo_" + pago.NumeroRecibo + ".pdf");
            HttpContext.Current.Response.Write(document);
            Response.Flush();
            Response.End();


        }

        public DataTable ConvertirListaToDataTable(List<DetallePago> detalle)
        {

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(DetallePago));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties) table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (DetallePago item in detalle)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties) row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }


    }

    public class Pago
    {
        public int NumeroRecibo { get; set; }

        public string NombreEstudiante { get; set; }

        public decimal Monto{ get; set; }
    }

    public class DetallePago
    {

        public string Producto { get; set; }

        public decimal Precio { get; set; }
    }
}