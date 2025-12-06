using Entidades;
using Negocio;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static iTextSharp.text.pdf.AcroFields;

namespace SistemaFinanciero
{
    public partial class WebFormInputFile : System.Web.UI.Page
    {
        TransaccionNegocio negocio = new TransaccionNegocio();
        ConsultaNegocio consultaNegocio = new ConsultaNegocio();
        EstudianteNegocio estudianteNegocio = new EstudianteNegocio();

        string alert = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }

        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (FileUpload2.FileName != "")
            {
                if (FileUpload2.HasFile)
                {
                    string carpeta = Server.MapPath("~/Soporte/");

                    // Si no existe la carpeta, crearla
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    FileUpload2.SaveAs(Server.MapPath("~/Soporte/") + Path.GetFileName(FileUpload2.FileName));
                    string filePath = Server.MapPath("~/Soporte/") + FileUpload2.FileName;
                    string[] allowdFile = { ".txt" };
                    string FileExt = System.IO.Path.GetExtension(FileUpload2.PostedFile.FileName);
                    bool isValidFile = allowdFile.Contains(FileExt);
                    if (isValidFile)
                    {
                        var listaDatos = LeerArchivo(filePath);

                        if (listaDatos.Count > 0)
                        {
                            btnGuardar.Visible = true;
                            btnCancelar.Visible = true;
                            pnlCargaArchivo.Visible = false;
                            GridDetalle.Visible = true;
                            GridDetalle.DataSource = listaDatos;
                            GridDetalle.DataBind();
                            GridDetalle.UseAccessibleHeader = true;
                            GridDetalle.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                        else
                        {
                            alert = @"swal('Aviso!', 'El archivo no contiente datos', 'error');";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                        }

                        File.Delete(Server.MapPath("~/Soporte/") + Path.GetFileName(FileUpload2.FileName));

                    }
                    else
                    {
                        alert = @"swal('Aviso!', 'Solo se permite archivos con formatos txt', 'error');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                    }
                }
                else
                {
                    alert = @"swal('Aviso!', 'El archivo no contiente datos', 'error');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                }
            }
            else
            {
                alert = @"swal('Aviso!', 'Debe seleccionar el archivo a cargar', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }
        }



        public List<tmeinfobancos> LeerArchivo(string rutaArchivo)
        {
            var lista = new List<tmeinfobancos>();
            var lineas = File.ReadAllLines(rutaArchivo);
            DateTime fechaProceso = Convert.ToDateTime(txtFecha.Text);

            foreach (var item in lineas)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;

                var registro = new tmeinfobancos
                {
                    NoRef = item.Substring(0, 20).Trim(),
                    CodAlum = item.Substring(20, 6).Trim(),
                    Tipo = item.Substring(26, 2).Trim(),
                    Descripcion = item.Substring(28, 30).Trim(),
                    Numero = item.Substring(58, 5).Trim(),
                    Monto = double.Parse(item.Substring(63, 8).Trim(),
                              System.Globalization.CultureInfo.InvariantCulture),
                    //System.Globalization.CultureInfo.GetCultureInfo("es-NI"))
                    Fecha = fechaProceso.Date,
                    TipoBanco = "",
                    Duplicado = false,
                    Procesado = true

                };

                lista.Add(registro);

            }



            Session["ListaBanco"] = lista;

            return lista;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            List<tmeinfobancos> lista = (List<tmeinfobancos>)Session["ListaBanco"];
            List<DetalleTransaccionDto> listaTransaccionDTO = new List<DetalleTransaccionDto>();
            DetalleTransaccionDto transaccionDTO = null;

            long idTransaccion = 0;
            negocio.InsertarInformacionBancoLista(lista);

            foreach (var item in lista)
            {
                negocio.ActualizarTrasaccionBanco(item.NoRef, item.CodAlum, item.Numero, item.Tipo, DateTime.Now.Year, out idTransaccion);

                transaccionDTO = new DetalleTransaccionDto();
                transaccionDTO.NumeroReferencia = item.NoRef;
                transaccionDTO.IdTransaccion = idTransaccion;
                transaccionDTO.Descripcion = item.Descripcion;
                transaccionDTO.CodigoAlumno = item.CodAlum;
                transaccionDTO.Monto = item.Monto;
                listaTransaccionDTO.Add(transaccionDTO);
            }

            GuardarFactura(listaTransaccionDTO);

            LimpiarCampos();

            alert = @"swal('Aviso!', 'Se registro la información con éxtio', 'success');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        public void LimpiarCampos()
        {
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
            btnGuardar.Visible = false;
            btnCancelar.Visible = false;
            pnlCargaArchivo.Visible = true;
            GridDetalle.Visible = false;
            GridDetalle.DataSource = null;
            GridDetalle.DataBind();
        }

        public void GuardarFactura(List<DetalleTransaccionDto> listaTransaccionDTO)
        {

            List<tmefacturasdet> listaDetalleFactura = new List<tmefacturasdet>();
            List<tmefacturas> listaFactura = new List<tmefacturas>();
            tmefacturasdet detalleFactura = null;

            var listaAgrupada = listaTransaccionDTO
            .GroupBy(x => new
            {
                x.NumeroReferencia,
                x.CodigoAlumno
            })
            .Select(g => new tmeinfobancos
            {
                NoRef = g.Key.NumeroReferencia,
                CodAlum = g.Key.CodigoAlumno,
                Monto = g.Sum(x => x.Monto)
            })
            .ToList();

            // Se tomará la serie A por defecto
            var nombreTabla = "A";
            var consecutivo = consultaNegocio.ConsultarContadorTabla(nombreTabla);

            foreach (var item in listaAgrupada)
            {
                // Consultar Consecutivo
                var idFactura =  consecutivo.tabla_contador;
                var idRecibo =  Convert.ToString(idFactura + "" + nombreTabla);
                var idContador =  idFactura + 1;

                // Obtener nombre estudiante
                var estudiante = estudianteNegocio.ConsultarEstudiante(item.CodAlum.ToString());
                var nombreEstudiante = estudiante != null ? estudiante.nombres.TrimEnd().ToUpper() + " " + estudiante.apellidos.TrimEnd().ToUpper() : "";

                // Insertar en la tabla factura
                tmefacturas factura = new tmefacturas();
                factura.id_factura = idRecibo;
                factura.tipo_factura = "MANUAL";
                factura.id_caja = "100"; // Por defecto, hay una en la tabla tmecajas con id 100
                factura.fecha = Convert.ToDateTime(txtFecha.Text);
                factura.tipo_cambio = 0;
                factura.id_estudiante = item.CodAlum.ToString();
                factura.anombrede = nombreEstudiante;
                factura.total_cordobas = Convert.ToDouble(item.Monto);
                factura.total_dolares = 0;
                factura.cliente_paga = 0;
                factura.cliente_cambio = 0;
                factura.fecha_cancela = Convert.ToDateTime(txtFecha.Text);
                factura.estado = "CANCELADO";
                factura.estado_por = Session["Login"].ToString() != null ? Session["Login"].ToString() : ""; // Usuario que sube txt
                factura.forma_pago = "EFECTIVO";
                factura.detalles = "PAGO REALIZADO EN BANCO";
                listaFactura.Add(factura);
                consultaNegocio.InsertarFactura(factura);
                consultaNegocio.ActualizarConsecutivoTabla(consecutivo.id_contador, (long)idContador);

            }

            foreach (var item in listaTransaccionDTO)
            {
                var factura = listaFactura.Where(x => x.id_estudiante == item.CodigoAlumno).FirstOrDefault();
                detalleFactura = new tmefacturasdet();
                detalleFactura.id_item = Convert.ToString(item.IdTransaccion);
                detalleFactura.id_factura = factura.id_factura;
                detalleFactura.nombre_item = item.Descripcion;
                detalleFactura.preciounitario = item.Monto;
                detalleFactura.cantidad = 1;
                detalleFactura.subtotal = item.Monto;
                detalleFactura.tipo_item = "TRANSACCIONES BANCO";
                listaDetalleFactura.Add(detalleFactura);
            }

            consultaNegocio.InsertarDetalleFacturaLista(listaDetalleFactura);

        }
    }
}