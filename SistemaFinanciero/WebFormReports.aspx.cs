using Entidades;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaFinanciero
{
    public partial class WebFormReports : System.Web.UI.Page
    {
        private ConsultaNegocio consultaNegocio = new ConsultaNegocio();
        string alert = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string script = @"<script type='text/javascript'> cargarFechasMaxima(); </script>";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta5", script, false);


            if (!IsPostBack)
            {
                LimpiarCampos();
            }

        }

        protected void ddlTipoSeleccionBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(ddlTipoSeleccionBusqueda.SelectedValue) != 0)
            {
                if (int.Parse(ddlTipoSeleccionBusqueda.SelectedValue) == 1) // Busqueda por no. de recibo
                {
                    pnlRangoFecha.Visible = false;
                    pnlRecibo.Visible = true;
                    txtNumeroRecibo.Text = string.Empty;
                    txtFechaInicio.Text = string.Empty;
                    txtFechaFin.Text = string.Empty;
                    btnBuscar.Visible = true;

                }
                else if (int.Parse(ddlTipoSeleccionBusqueda.SelectedValue) == 2) // Busqueda por fecha
                {
                    pnlRangoFecha.Visible = true;
                    pnlRecibo.Visible = false;
                    txtNumeroRecibo.Text = string.Empty;
                    txtFechaInicio.Text = string.Empty;
                    txtFechaFin.Text = string.Empty;
                    btnBuscar.Visible = true;


                    GridRecibo.Visible = false;
                    GridRecibo.DataSource = null;
                    GridRecibo.DataBind();
                    GridDetalleRecibo.Visible = false;
                    GridDetalleRecibo.DataSource = null;
                    GridDetalleRecibo.DataBind();

                }
            }
            else
            {

                LimpiarCampos();


                alert = @"swal('Aviso!', 'Seleccione el tipo de busqueda que desea realizar', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }


            GridRecibo.Visible = false;
            GridRecibo.DataSource = null;
            GridRecibo.DataBind();
            GridDetalleRecibo.Visible = false;
            GridDetalleRecibo.DataSource = null;
            GridDetalleRecibo.DataBind();

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            int tipoBusqueda = int.Parse(ddlTipoSeleccionBusqueda.SelectedValue);

            if (ValidarCampos())
            {
                if (tipoBusqueda == 1)
                {
                    //Metodo para buscar el número del recibo
                    var recibo = consultaNegocio.ListarFacturaPorId(txtNumeroRecibo.Text.TrimEnd().ToUpper()).ToList();

                    if (recibo.Count > 0)
                    {
                        Session["Factura"] = recibo;
                        GridRecibo.Visible = true;
                        GridRecibo.DataSource = recibo;
                        GridRecibo.DataBind();
                        GridRecibo.UseAccessibleHeader = true;
                        GridRecibo.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                    else
                    {
                        GridRecibo.Visible = false;
                        GridRecibo.DataSource = null;
                        GridRecibo.DataBind();

                        alert = @"swal('Aviso!', ' No existe número de recibo ', 'error');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                    }


                }
                else
                {


                    if (VerificarFecha())
                    {
                        //Metodo para buscar todos los recibos por rango de fechas
                        var fechaInicio = Convert.ToDateTime(txtFechaInicio.Text).Date;
                        var fechaFin = Convert.ToDateTime(txtFechaFin.Text).Date;
                        var recibo = consultaNegocio.ListarFacturaPorFecha(fechaInicio, fechaFin).ToList();

                        if (recibo.Count > 0)
                        {
                            Session["Factura"] = recibo;
                            GridRecibo.Visible = true;
                            GridRecibo.DataSource = recibo;
                            GridRecibo.DataBind();
                            GridRecibo.UseAccessibleHeader = true;
                            GridRecibo.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                        else
                        {
                            GridRecibo.Visible = false;
                            GridRecibo.DataSource = null;
                            GridRecibo.DataSource = null;
                            GridRecibo.DataBind();

                            alert = @"swal('Aviso!', ' No existe número de recibo ', 'error');";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                        }

                    }
                    else
                    {

                        alert = @"swal('Aviso!', ' La fecha inicio no puede ser mayor a la fecha fin ', 'error');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                    }



                }
            }
            else
            {

                var mensaje = tipoBusqueda == 1 ? "Debe digitar el No. del Recibo" : "Debe seleccionar la Fecha Inicio y Fecha Fin para realizar la búsqueda";
                alert = @"swal('Aviso!', '" + mensaje + "', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }


        }

        public bool ValidarCampos()
        {
            bool bandera = false;
            int tipoBusqueda = int.Parse(ddlTipoSeleccionBusqueda.SelectedValue);

            if (tipoBusqueda == 1 & !string.IsNullOrEmpty(txtNumeroRecibo.Text.TrimEnd()))
            {
                bandera = true;
            }
            else if (tipoBusqueda == 2 & !string.IsNullOrEmpty(txtFechaInicio.Text) & !string.IsNullOrEmpty(txtFechaFin.Text))
            {
                bandera = true;
            }


            return bandera;
        }


        protected void GridRecibo_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = GridRecibo.Rows[index];
            string numeroRecibo = GridRecibo.DataKeys[index].Value.ToString();

            if (e.CommandName == "Detalle")
            {

                //Metodo para buscar el número del recibo
                var detalleFactura = consultaNegocio.ListarDetalleFactura(numeroRecibo);

                if (detalleFactura.Count > 0)
                {
                    txtNumRecibo.Text = Server.HtmlDecode(numeroRecibo.TrimEnd());
                    txtNombreFactura.Text = Server.HtmlDecode(row.Cells[3].Text.TrimEnd());

                    divBusqueda.Visible = false;
                    pnlRangoFecha.Visible = false;
                    pnlRecibo.Visible = false;
                    GridRecibo.Visible = false;
                    btnBuscar.Visible = false;

                    pnlDetalleFactura.Visible = true;
                    btnRegresar.Visible = true;
                    GridDetalleRecibo.Visible = true;
                    GridDetalleRecibo.DataSource = detalleFactura;
                    GridDetalleRecibo.DataBind();
                    GridDetalleRecibo.UseAccessibleHeader = true;
                    GridDetalleRecibo.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    GridDetalleRecibo.Visible = false;
                    GridDetalleRecibo.DataSource = null;
                    GridDetalleRecibo.DataBind();


                    GridRecibo.UseAccessibleHeader = true;
                    GridRecibo.HeaderRow.TableSection = TableRowSection.TableHeader;

                    alert = @"swal('Aviso!', ' No existe detalle para el número de recibo ', 'error');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                }
            }


            if (e.CommandName == "ImpirmirRecibo")
            {
                var listaFactura = (List<tmefacturas>)Session["Factura"];
                var factura = listaFactura.Where(x => x.id_factura == numeroRecibo).FirstOrDefault();
                var detalleFactura = consultaNegocio.ListarDetalleFactura(numeroRecibo);
                GenerarPDF(factura, detalleFactura);

                GridRecibo.UseAccessibleHeader = true;
                GridRecibo.HeaderRow.TableSection = TableRowSection.TableHeader;

            }
        }


        public void GenerarPDF(tmefacturas factura, List<tmefacturasdet> listaDetalle)
        {

            Document document = new Document(new Rectangle(226.77f, 425.20f), 5f, 5f, 5f, 5f);
            PdfWriter writer = PdfWriter.GetInstance(document, HttpContext.Current.Response.OutputStream);

            // Para hacer crecer la hoja
            if (listaDetalle.Count > 4)
                document.SetPageSize(new Rectangle(226.77f, document.BottomMargin + writer.GetVerticalPosition(false)));


            document.Open();

            // Título
            Font fontTitle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10);
            Font font9 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10);

            Paragraph Titulo1 = new Paragraph("Instituto Pedagógico La Salle", new Font(Font.FontFamily.TIMES_ROMAN, 12));
            Titulo1.Alignment = Element.ALIGN_CENTER;
            document.Add(Titulo1);

            Paragraph Titulo2 = new Paragraph("Reporte detallado Caja general", new Font(Font.FontFamily.TIMES_ROMAN, 11));
            Titulo2.Alignment = Element.ALIGN_CENTER;
            document.Add(Titulo2);

            Paragraph Titulo3 = new Paragraph("No. Recibo: " + factura.id_factura.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 10));
            Titulo3.Alignment = Element.ALIGN_CENTER;
            document.Add(Titulo3);

            document.Add(new Chunk("\n"));

            Paragraph Titulo4 = new Paragraph("A Nombre : " + factura.anombrede.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 9));
            Titulo4.Alignment = Element.ALIGN_LEFT;
            document.Add(Titulo4);

            Paragraph Titulo5 = new Paragraph("Fecha : " + factura.fecha?.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 9));
            Titulo5.Alignment = Element.ALIGN_LEFT;
            document.Add(Titulo5);


            Paragraph Titulo6 = new Paragraph("Forma de Pago : " + factura.forma_pago, new Font(Font.FontFamily.TIMES_ROMAN, 9));
            Titulo6.Alignment = Element.ALIGN_LEFT;
            document.Add(Titulo6);


            Paragraph Titulo7 = new Paragraph("Cajero : " + factura.estado_por, new Font(Font.FontFamily.TIMES_ROMAN, 9));
            Titulo7.Alignment = Element.ALIGN_LEFT;
            document.Add(Titulo7);

            document.Add(new Chunk("\n"));

            Paragraph EncabezadoTabla = new Paragraph("Detalle de pagos", new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD | Font.UNDERLINE));
            EncabezadoTabla.Alignment = Element.ALIGN_CENTER;
            document.Add(EncabezadoTabla);

            document.Add(new Chunk("\n"));

            DataTable dt = new DataTable();
            dt = ConvertirListaToDataTablePersonalizado(listaDetalle);

            PdfPTable table = new PdfPTable(dt.Columns.Count);

            float[] widths = new float[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
                widths[i] = 4f;

            table.SetWidths(widths);
            table.WidthPercentage = 90;

            PdfPCell cell = new PdfPCell(new Phrase("columns"));
            cell.Colspan = dt.Columns.Count;

            table.WidthPercentage = 100;

            // Encabezados en negrita y centrados
            Font fontHeader = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, Font.BOLD);

            foreach (DataColumn c in dt.Columns)
            {
                PdfPCell headerCell = new PdfPCell(new Phrase(c.ColumnName, fontHeader));
                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                headerCell.BackgroundColor = new BaseColor(230, 230, 230); // gris suave
                table.AddCell(headerCell);
            }

            Font fontCell = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9);

            foreach (DataRow r in dt.Rows)
            {
                for (int h = 0; h < dt.Columns.Count; h++)
                {
                    PdfPCell cellValue = new PdfPCell(new Phrase(r[h].ToString(), fontCell));

                    // PRIMERA COLUMNA = IZQUIERDA + AJUSTE DE TEXTO
                    if (h == 0)
                    {
                        cellValue.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellValue.NoWrap = false;
                    }
                    else
                    {
                        cellValue.HorizontalAlignment = Element.ALIGN_CENTER;
                    }

                    cellValue.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cellValue);
                }
            }


            document.Add(table);

            document.Add(new Chunk("\n"));

            Paragraph P11 = new Paragraph("Total Factura: C$ " + factura.total_cordobas, new Font(Font.FontFamily.TIMES_ROMAN, 10));
            P11.Alignment = Element.ALIGN_LEFT;
            document.Add(P11);

            document.Close();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Recibo_" + factura.id_factura.ToString() + ".pdf");
            HttpContext.Current.Response.Write(document);
            Response.Flush();
            Response.End();
        }


        public DataTable ConvertirListaToDataTablePersonalizado(List<tmefacturasdet> detalle)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Item", typeof(string));
            table.Columns.Add("Precio (C$)", typeof(decimal));
            table.Columns.Add("Cantidad", typeof(int));
            table.Columns.Add("Subtotal (C$)", typeof(decimal));


            // Llenado de filas con los datos de tu lista
            foreach (var item in detalle)
            {
                DataRow row = table.NewRow();
                row["Item"] = item.nombre_item;
                row["Precio (C$)"] = item.preciounitario;
                row["Cantidad"] = item.cantidad;
                row["Subtotal (C$)"] = item.subtotal;
                table.Rows.Add(row);
            }

            return table;
        }


        public void LimpiarCampos()
        {
            divBusqueda.Visible = true;
            ddlTipoSeleccionBusqueda.SelectedValue = "0";
            pnlRecibo.Visible = false;
            txtNumeroRecibo.Text = string.Empty;
            pnlRangoFecha.Visible = false;
            txtFechaInicio.Text = string.Empty;
            txtFechaFin.Text = string.Empty;
            pnlDetalleFactura.Visible = false;
            btnRegresar.Visible = false;
            btnBuscar.Visible = false;
            GridRecibo.Visible = false;
            GridRecibo.DataSource = null;
            GridRecibo.DataBind();
            GridDetalleRecibo.Visible = false;
            GridDetalleRecibo.DataSource = null;
            GridDetalleRecibo.DataBind();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }


        public bool VerificarFecha()
        {

            bool answer = true;

            if (!IsDate(txtFechaInicio.Text)) { return false; }
            if (!IsDate(txtFechaFin.Text)) { return false; }

            DateTime f1 = DateTime.Parse(txtFechaInicio.Text);
            DateTime f2 = DateTime.Parse(txtFechaFin.Text);

            if (f1 > f2) { return false; }


            return answer;

        }

        private static bool IsDate(string inputDate)
        {
            DateTime dt;
            bool isdate = DateTime.TryParse(inputDate, out dt);
            return isdate;
        }
    }
}