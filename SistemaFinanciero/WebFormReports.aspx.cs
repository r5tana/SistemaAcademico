using Entidades;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Negocio;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Image = iTextSharp.text.Image;

namespace SistemaFinanciero
{
    public partial class WebFormReports : System.Web.UI.Page
    {
        private ConsultaNegocio consultaNegocio = new ConsultaNegocio();
        private UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
        private EstudianteNegocio estudiante = new EstudianteNegocio();
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

            tmaestudiante detalleAlumno = null;
            if (factura.id_estudiante != null || !string.IsNullOrEmpty(factura.id_estudiante))
            {
                detalleAlumno = new tmaestudiante();
                detalleAlumno = estudiante.ConsultarEstudiante(factura.id_estudiante);

            }

            var nombreCajero = (factura.estado_por != null || !string.IsNullOrEmpty(factura.estado_por))
                                    ? usuarioNegocio.NombreUsuario(factura.estado_por)
                                : null;

            string Grado = detalleAlumno != null ? detalleAlumno.nivel : "";
            string Seccion = detalleAlumno != null ? detalleAlumno.seccion : "";
            string cajeroNombre = !string.IsNullOrEmpty(nombreCajero) ? nombreCajero : "";
            string valorEnLetras = Utilitario.ConvertirMontoALetras(Convert.ToDecimal(factura.total_cordobas)).ToUpper();

            string logoPath = Server.MapPath("~/Fotos/LaSalle.jpeg");

            string nombrePdf = "Recibo_" + factura.id_factura + ".pdf";
            string carpeta = Server.MapPath("~/Soporte/");

            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            string rutaFisica = Path.Combine(carpeta, nombrePdf);

            // Definición de fuentes
            Font fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);
            Font fontTitle = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, BaseColor.RED); // Títulos en rojo como en la imagen
            Font fontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.BLACK);
            Font fontNegrita = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.BLACK);
            Font fontSmall = FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.BLACK);
            Font fontSmallNegrita = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, BaseColor.BLACK);

            // Tamaño: Ancho A4, Medio Alto A4 (595.28f, 420.95f) con márgenes reducidos
            Rectangle tamanioDocumento = new Rectangle(595.28f, 420.95f);
            Document document = new Document(tamanioDocumento, 15f, 15f, 15f, 15f);

            //using (FileStream fs = new FileStream(rutaFisica, FileMode.Create))
            //{
            PdfWriter writer = PdfWriter.GetInstance(document, HttpContext.Current.Response.OutputStream);
            document.Open();

            try
            {
                // ======================================================
                // --- BLOQUE 1: ENCABEZADO (LOGO Y TEXTO EN TABLA) ---
                // ======================================================

                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 1.5f, 6f });

                // --- Columna 1: LOGO ---
                try
                {
                    Image logo = Image.GetInstance(logoPath);
                    logo.ScaleToFit(80f, 80f);
                    PdfPCell logoCell = new PdfPCell(logo, false);
                    logoCell.Border = 0;
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    logoCell.Padding = 5f;
                    headerTable.AddCell(logoCell);
                }
                catch (Exception)
                {
                    // Placeholder
                    headerTable.AddCell(new PdfPCell(new Phrase("LOGO LA SALLE", fontSmall)) { Border = 0, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER });
                }

                // --- Columna 2: Información de la Institución ---
                PdfPCell infoCell = new PdfPCell();
                infoCell.Border = 0;
                infoCell.Padding = 0;

                infoCell.AddElement(new Paragraph("ASOCIACIÓN EDUCATIVA LASALLISTA", fontHeader) { Alignment = Element.ALIGN_LEFT });
                infoCell.AddElement(new Paragraph("Instituto Pedagógico La Salle", fontHeader) { Alignment = Element.ALIGN_LEFT });
                infoCell.AddElement(new Paragraph("Dirección: De los semáforos de Claro Villa Fontana, 150 metros al Oeste.", fontNormal) { Alignment = Element.ALIGN_LEFT });
                infoCell.AddElement(new Paragraph("Teléfono: 2278 0165 Ruc: J0810000067913", fontNormal) { Alignment = Element.ALIGN_LEFT });

                headerTable.AddCell(infoCell);

                document.Add(headerTable);
                document.Add(new Chunk("\n"));

                // ======================================================
                // --- BLOQUE 2: RECIBO OFICIAL Y NÚMERO ---
                // ======================================================

                // Obtener Serie y Correlativo
                string idFacturaStr = factura.id_factura.ToString();
                string correlativo = new string(idFacturaStr.TakeWhile(char.IsDigit).ToArray());
                string serie = new string(idFacturaStr.SkipWhile(char.IsDigit).ToArray());

                PdfPTable tableTitleNum = new PdfPTable(2);
                tableTitleNum.WidthPercentage = 100;
                tableTitleNum.SetWidths(new float[] { 1.5f, 1f });

                // Columna 1: RECIBO OFICIAL DE CAJA
                Paragraph title = new Paragraph("RECIBO OFICIAL DE CAJA", fontTitle);
                title.Alignment = Element.ALIGN_RIGHT;

                tableTitleNum.AddCell(new PdfPCell(title) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

                // Columna 2: Serie y Número (usando el formato de la imagen)
                Paragraph numInfo = new Paragraph();
                numInfo.Add(new Chunk("Serie: ", fontNormal));
                numInfo.Add(new Chunk(serie, fontNegrita));
                numInfo.Add(new Chunk("  Nº ", fontNormal));
                numInfo.Add(new Chunk(correlativo, fontTitle)); // Número en rojo

                tableTitleNum.AddCell(new PdfPCell(numInfo) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

                document.Add(tableTitleNum);
                document.Add(new Chunk("\n"));

                // ======================================================
                // --- BLOQUE 3: FECHA, RECIBIMOS DE, GRADO Y SECCIÓN ---
                // ======================================================

                PdfPTable tableFecha = new PdfPTable(3);
                tableFecha.WidthPercentage = 30; // 30% del ancho para la tabla de fecha
                tableFecha.HorizontalAlignment = Element.ALIGN_LEFT;

                // Encabezados de Fecha
                tableFecha.AddCell(new PdfPCell(new Phrase("Día", fontSmall)) { HorizontalAlignment = Element.ALIGN_CENTER });
                tableFecha.AddCell(new PdfPCell(new Phrase("Mes", fontSmall)) { HorizontalAlignment = Element.ALIGN_CENTER });
                tableFecha.AddCell(new PdfPCell(new Phrase("Año", fontSmall)) { HorizontalAlignment = Element.ALIGN_CENTER });

                // Valores de Fecha
                tableFecha.AddCell(new PdfPCell(new Phrase(factura.fecha?.Day.ToString("00"), fontNegrita)) { HorizontalAlignment = Element.ALIGN_CENTER });
                tableFecha.AddCell(new PdfPCell(new Phrase(factura.fecha?.Month.ToString("00"), fontNegrita)) { HorizontalAlignment = Element.ALIGN_CENTER });
                tableFecha.AddCell(new PdfPCell(new Phrase(factura.fecha?.Year.ToString(), fontNegrita)) { HorizontalAlignment = Element.ALIGN_CENTER });

                document.Add(tableFecha);
                document.Add(new Chunk("\n")); // Espacio

                // Tabla para la línea "Recibimos de:" y "Grado/Sección"
                PdfPTable tableRecibimosDe = new PdfPTable(6);
                tableRecibimosDe.WidthPercentage = 100;
                tableRecibimosDe.SetWidths(new float[] { 1.5f, 4.5f, 1f, 1f, 1.2f, 1f });

                // Recibimos de
                tableRecibimosDe.AddCell(new PdfPCell(new Phrase("Recibimos de:", fontNormal)) { Border = 0 });
                tableRecibimosDe.AddCell(new PdfPCell(new Phrase(factura.anombrede.ToString(), fontNegrita)) { Colspan = 1, Border = Rectangle.BOTTOM_BORDER });

                // Grado y Sección
                tableRecibimosDe.AddCell(new PdfPCell(new Phrase("Grado:", fontNormal)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                tableRecibimosDe.AddCell(new PdfPCell(new Phrase(Grado, fontNegrita)) { Colspan = 1, Border = Rectangle.BOTTOM_BORDER });
                tableRecibimosDe.AddCell(new PdfPCell(new Phrase("Sección:", fontNormal)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                tableRecibimosDe.AddCell(new PdfPCell(new Phrase(Seccion, fontNegrita)) { Colspan = 1, Border = Rectangle.BOTTOM_BORDER });

                document.Add(tableRecibimosDe);
                document.Add(new Chunk("\n"));

                // ======================================================
                // --- BLOQUE 4: CANTIDAD Y VALOR EN LETRAS ---
                // ======================================================

                PdfPTable tableMonto = new PdfPTable(4);
                tableMonto.WidthPercentage = 100;
                tableMonto.SetWidths(new float[] { 1.5f, 2.5f, 1.5f, 4.5f });

                decimal MontoTotal = Convert.ToDecimal(factura.total_cordobas);
                // Si tienes MontoALetras:
                //string valorEnLetras = "MIL DOSCIENTOS CINCUENTA CON 75/100 CÓRDOBAS"; // Ejemplo, usar tu método

                // La cantidad de: (Monto Numérico)
                tableMonto.AddCell(new PdfPCell(new Phrase("La cantidad de:", fontNormal)) { Border = 0 });
                tableMonto.AddCell(new PdfPCell(new Phrase(MontoTotal.ToString("N2"), fontNegrita)) { Border = Rectangle.BOTTOM_BORDER });

                // Valor en letras:
                tableMonto.AddCell(new PdfPCell(new Phrase("Valor en letras:", fontNormal)) { Border = 0 });
                tableMonto.AddCell(new PdfPCell(new Phrase(valorEnLetras, fontNegrita)) { Border = Rectangle.BOTTOM_BORDER });

                document.Add(tableMonto);
                document.Add(new Chunk("\n"));

                // ======================================================
                // --- BLOQUE 5: EN CONCEPTO DE ---
                // ======================================================

                PdfPTable tableConcepto = new PdfPTable(1);
                tableConcepto.WidthPercentage = 100;

                List<string> Transacciones = new List<string>();
                foreach (var item in listaDetalle)
                {
                    Transacciones.Add($"{item.nombre_item} - {item.subtotal:N2}");
                }
                string conceptoStr = string.Join(", ", Transacciones);

                // El concepto va debajo de la etiqueta
                tableConcepto.AddCell(new PdfPCell(new Phrase("En concepto de:", fontNormal)) { Border = 0 });

                // Línea de contenido
                tableConcepto.AddCell(new PdfPCell(new Phrase(conceptoStr, fontNegrita)) { Border = Rectangle.BOTTOM_BORDER });
                tableConcepto.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = Rectangle.BOTTOM_BORDER }); // Línea vacía extra

                document.Add(tableConcepto);

                document.Add(new Chunk("\n\n")); // Espacio

                // ======================================================
                // --- BLOQUE 6: PAGO Y CAJERO ---
                // ======================================================


                // Normalizar el texto del tipo de pago
                string tipoPago = factura.forma_pago != null ? factura.forma_pago.Trim().ToUpper() : "";

                // Marcar solo el correcto
                string chkEfectivo = tipoPago == "EFECTIVO" ? "X" : "  ";
                string chkTarjeta = tipoPago == "TARJETA" ? "X" : "  ";
                string chkCheque = tipoPago == "CHEQUE" ? "X" : "  ";

                // Texto con cuadros
                Paragraph pago = new Paragraph();
                pago.Add(new Chunk("Pagó en:  ", fontNormal));
                pago.Add(new Chunk($"[{chkEfectivo}] Efectivo   ", fontNormal));
                pago.Add(new Chunk($"[{chkTarjeta}] Tarjeta   ", fontNormal));
                pago.Add(new Chunk($"[{chkCheque}] Cheque", fontNormal));

                document.Add(pago);
                document.Add(new Chunk("\n"));

                // Cajero y Firma
                PdfPTable tableFirma = new PdfPTable(1);
                tableFirma.WidthPercentage = 25;
                tableFirma.HorizontalAlignment = Element.ALIGN_LEFT;

                tableFirma.AddCell(new PdfPCell(new Phrase("____________________", fontNormal)) { Border = 0 });
                tableFirma.AddCell(new PdfPCell(new Phrase($"CAJER@ {cajeroNombre}", fontSmall)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });

                document.Add(tableFirma);

                // Pie de Imprenta (usando un párrafo simple centrado al final)
                document.Add(new Chunk("\n\n"));
                Paragraph pieImprenta = new Paragraph("PIE DE IMPRENTA", fontSmall);
                pieImprenta.Alignment = Element.ALIGN_RIGHT;
                document.Add(pieImprenta);


                document.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Recibo_" + factura.id_factura.ToString() + ".pdf");
                HttpContext.Current.Response.Write(document);
                Response.Flush();
                Response.End();


            }
            catch (Exception ex)
            {
                alert = @"swal('Aviso!', ' Ha ocurrido un error al impirmir el recibo. Favor cominiquese con el administrador del sistema ', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

                GridRecibo.UseAccessibleHeader = true;
                GridRecibo.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            finally
            {
                if (document.IsOpen())
                {
                    document.Close();
                }
            }
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