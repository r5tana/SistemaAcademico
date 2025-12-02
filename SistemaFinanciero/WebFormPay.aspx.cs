using Entidades;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Negocio;
using OfficeOpenXml.Style;
using SistemaFinanciero.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;
using System.Xml.Linq;

namespace SistemaFinanciero
{
    public partial class WebFormPay : System.Web.UI.Page
    {
        EstudianteNegocio negocioEstudiante = new EstudianteNegocio();
        ConsultaNegocio consultaNegocio = new ConsultaNegocio();
        TransaccionNegocio transaccionNegocio = new TransaccionNegocio();

        string alert = string.Empty;
        int idUsuario = 0;
        string login = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                idUsuario = int.Parse(Session["IdUsuario"].ToString());
                login = Session["Login"].ToString();
                Session["listaEstudiantes"] = null;
                CargarDatosUsuario(idUsuario, login);

                CrearTabla();
                BindGrid();
            }
        }

        private void CrearTabla()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Item");
            dt.Columns.Add("Precio");
            dt.Columns.Add("Cantidad");
            dt.Columns.Add("Subtotal");
            dt.Columns.Add("Stock");

            Session["tabla"] = dt; // Guardar estructura en Session
        }

        private void BindGrid()
        {
            GridProductos.DataSource = Session["tabla"] as DataTable;
            GridProductos.DataBind();
        }

        public void CargarDatosUsuario(int idUsuario, string login)
        {
            var detalleCaja = consultaNegocio.ConsultarCajaUsuario(idUsuario);
            if (detalleCaja != null)
            {

                Session["DatosCaja"] = detalleCaja;
                txtIdCaja.Text = detalleCaja.id_caja;
                txtCaja.Text = detalleCaja.nombre.TrimEnd();
                txtTipoCambio.Text = detalleCaja.tipocambio.ToString();
                txtUsuarioProcesa.Text = login;
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                txtNombreFactura.Enabled = false;
                btnAbrirModalEstudiante.Enabled = false;
                ddlFormaPago.Enabled = false;
                rbtConcepto.Enabled = false;
                rbtProducto.Enabled = false;
                rbtTransacciones.Enabled = false;
                rbtOtros.Enabled = false;

                alert = @"swal('Aviso!', 'No tiene caja activa para ingresar pagos', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }
        }

        public void CargarEstudiantes()
        {

            var lista = negocioEstudiante.ListaEstudianteConTransacciones();

            Session["listaEstudiantesFactura"] = lista;
            this.GridEstudiantes.DataSource = lista;
            GridEstudiantes.DataBind();
            GridEstudiantes.UseAccessibleHeader = true;
            GridEstudiantes.HeaderRow.TableSection = TableRowSection.TableHeader;
            GridEstudiantes.Visible = true;


            if (GridProductos.Rows.Count != 0)
            {
                GridProductos.UseAccessibleHeader = true;
                GridProductos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

        }

        protected void btnAbrirModalEstudiante_Click(object sender, EventArgs e)
        {
            CargarEstudiantes();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);

        }

        protected void GridEstudiantes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row = GridEstudiantes.Rows[index];
            if (e.CommandName == "Buscar")
            {
                string idEstudiante = GridEstudiantes.DataKeys[index].Value.ToString();

                txtNombreFactura.Text = Server.HtmlDecode(row.Cells[2].Text.TrimEnd() + " " + row.Cells[3].Text.TrimEnd());
                txtCodEstudiante.Text = idEstudiante;

                GridEstudiantes.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal(); RemoveBackDrop();", true);
            }
        }

        protected void RbtTodos_CheckedChanged(object sender, EventArgs e)
        {
            // limpiar antes de cargar detalle            
            LimpiarGridProducto();

            if (rbtProducto.Checked || rbtConcepto.Checked || rbtTransacciones.Checked || rbtOtros.Checked)
            {

                if (rbtTransacciones.Checked)
                {
                    if (!string.IsNullOrEmpty(txtNombreFactura.Text) & !string.IsNullOrEmpty(txtCodEstudiante.Text))
                    {
                        txtNombreFactura.Enabled = true;
                        var transacciones = transaccionNegocio.ListaTransaccionesPendientesEstudiante(txtCodEstudiante.Text);
                        if (transacciones.Count > 0)
                        {
                            Session["ListaTransaccionFactura"] = transacciones;
                            txtPrecio.Enabled = false;
                            txtCantidad.Enabled = true;
                            btnAgregar.Enabled = true;
                            txtNombreFactura.Enabled = false;
                            ddlNombreTransaccion.Enabled = true;
                            ddlNombreTransaccion.Visible = true;

                            TxtOtraTransaccion.Text = string.Empty;
                            TxtOtraTransaccion.Visible = false;

                            ddlNombreTransaccion.Items.Clear();
                            ddlNombreTransaccion.DataSource = transacciones.OrderBy(x => x.DescripcionTransaccion).ToList();
                            ddlNombreTransaccion.DataTextField = "DescripcionTransaccion";
                            ddlNombreTransaccion.DataValueField = "IdTransaccion";
                            ddlNombreTransaccion.DataBind();

                            ddlNombreTransaccion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("SELECCIONE", "0"));
                        }
                        else
                        {
                            txtPrecio.Enabled = false;
                            txtCantidad.Enabled = false;
                            btnAgregar.Enabled = false;
                            txtNombreFactura.Enabled = false;
                            ddlNombreTransaccion.Enabled = false;
                            ddlNombreTransaccion.Items.Clear();
                            ddlNombreTransaccion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("SELECCIONE", "0"));

                            alert = @"swal('Aviso!', 'Estudiante " + txtNombreFactura.Text.TrimEnd() + " no tiene transacciones pendientes', 'error');";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

                            rbtTransacciones.Checked = false;
                            txtCodEstudiante.Text = string.Empty;
                            txtNombreFactura.Text = string.Empty;
                        }

                    }
                    else
                    {
                        rbtTransacciones.Checked = false;
                        txtPrecio.Enabled = false;
                        txtCantidad.Enabled = false;
                        btnAgregar.Enabled = false;
                        ddlNombreTransaccion.Enabled = false;
                        ddlNombreTransaccion.Visible = true;
                        TxtOtraTransaccion.Visible = false;
                        divStock.Visible = false;
                        txtNombreFactura.Enabled = false;
                        txtNombreFactura.Text = string.Empty;
                        txtCodEstudiante.Text = string.Empty;
                        ddlNombreTransaccion.Items.Clear();
                        ddlNombreTransaccion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("SELECCIONE", "0"));

                        alert = @"swal('Aviso!', 'Seleccione el estudiante del aracel a cancelar', 'error');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                    }
                }
                else
                {

                    if (rbtProducto.Checked)
                    {

                        divStock.Visible = true;
                        var productos = consultaNegocio.ListaInventario();
                        if (productos.Count > 0)
                        {
                            txtPrecio.Enabled = false;
                            txtCantidad.Enabled = true;
                            btnAgregar.Enabled = true;
                            txtNombreFactura.Enabled = false;
                            ddlNombreTransaccion.Enabled = true;
                            ddlNombreTransaccion.Visible = true;

                            TxtOtraTransaccion.Text = string.Empty;
                            TxtOtraTransaccion.Visible = false;

                            ddlNombreTransaccion.Items.Clear();
                            Session["ListaProductosFactura"] = productos;
                            ddlNombreTransaccion.DataSource = productos.OrderBy(x => x.prod_nombre).ToList();
                            ddlNombreTransaccion.DataTextField = "prod_nombre";
                            ddlNombreTransaccion.DataValueField = "id_inventario";
                            ddlNombreTransaccion.DataBind();

                            ddlNombreTransaccion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("SELECCIONE", "0"));
                        }
                        else
                        {
                            txtPrecio.Enabled = false;
                            txtCantidad.Enabled = false;
                            btnAgregar.Enabled = false;

                            alert = @"swal('Aviso!', 'No existe registro', 'error');";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                        }
                    }
                    else if (rbtConcepto.Checked)
                    {

                        var concepto = consultaNegocio.ListaConceptos();
                        if (concepto.Count > 0)
                        {
                            txtPrecio.Enabled = false;
                            txtCantidad.Enabled = true;
                            btnAgregar.Enabled = true;
                            txtNombreFactura.Enabled = false;
                            ddlNombreTransaccion.Enabled = true;
                            ddlNombreTransaccion.Visible = true;

                            TxtOtraTransaccion.Text = string.Empty;
                            TxtOtraTransaccion.Visible = false;

                            ddlNombreTransaccion.Items.Clear();
                            Session["ListaConceptoFactura"] = concepto;
                            ddlNombreTransaccion.DataSource = concepto.OrderBy(x => x.nombre).ToList();
                            ddlNombreTransaccion.DataTextField = "nombre";
                            ddlNombreTransaccion.DataValueField = "id_concepto";
                            ddlNombreTransaccion.DataBind();

                            ddlNombreTransaccion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("SELECCIONE", "0"));
                        }
                        else
                        {
                            txtPrecio.Enabled = false;
                            txtCantidad.Enabled = false;
                            btnAgregar.Enabled = false;

                            alert = @"swal('Aviso!', 'No existe registro', 'error');";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                        }
                    }
                    else if (rbtOtros.Checked)
                    {
                        txtNombreFactura.Enabled = true;
                        txtNombreFactura.Text = string.Empty;
                        txtCodEstudiante.Text = string.Empty;
                        TxtOtraTransaccion.Text = string.Empty;
                        TxtOtraTransaccion.Visible = true;
                        ddlNombreTransaccion.Items.Clear();
                        ddlNombreTransaccion.Visible = false;
                        txtPrecio.Enabled = true;
                        txtCantidad.Enabled = true;
                        btnAgregar.Enabled = true;
                    }
                }
            }
        }

        protected void ddlNombreTransaccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int transaccion = int.Parse(ddlNombreTransaccion.SelectedValue);

            if (transaccion != 0)
            {

                if (rbtTransacciones.Checked)
                {
                    List<DetalleTransaccionEstudianteDto> listaTransacciones = (List<DetalleTransaccionEstudianteDto>)Session["ListaTransaccionFactura"];
                    var seleccion = listaTransacciones.Where(x => x.IdTransaccion == transaccion).FirstOrDefault();

                    if (seleccion.Monto == 0)
                    {
                        txtPrecio.Text = string.Empty;
                        txtPrecio.Enabled = true;
                    }
                    else
                    {
                        txtPrecio.Enabled = false;
                        txtPrecio.Text = seleccion.Monto.ToString();

                    }

                }
                else if (rbtConcepto.Checked)
                {
                    var conceptoId = Convert.ToString(transaccion);
                    List<tmeconceptos> listaConceptos = (List<tmeconceptos>)Session["ListaConceptoFactura"];
                    var seleccion = listaConceptos.Where(x => x.id_concepto == conceptoId).FirstOrDefault();

                    if (seleccion.valor == 0)
                    {
                        txtPrecio.Text = string.Empty;
                        txtPrecio.Enabled = true;
                    }
                    else
                    {
                        txtPrecio.Enabled = false;
                        txtPrecio.Text = seleccion.valor.ToString();
                    }
                }
                else if (rbtProducto.Checked)
                {
                    var productoId = Convert.ToString(transaccion);
                    List<tmeinventario> listaProducto = (List<tmeinventario>)Session["ListaProductosFactura"];
                    var seleccion = listaProducto.Where(x => x.id_inventario == productoId).FirstOrDefault();

                    if (seleccion.valor == 0)
                    {
                        txtPrecio.Text = string.Empty;
                        txtPrecio.Enabled = true;
                        txtStock.Text = string.Empty;
                    }
                    else
                    {
                        txtPrecio.Enabled = false;
                        txtPrecio.Text = seleccion.valor.ToString();
                        txtStock.Text = seleccion.stockactual.ToString();
                    }
                }

                if (GridProductos.Rows.Count != 0)
                {
                    GridProductos.UseAccessibleHeader = true;
                    GridProductos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            else
            {
                txtCantidad.Text = string.Empty;
                txtPrecio.Text = string.Empty;
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {

            bool bandera = false;

            #region "Validar Seleccion Transacción"

            if (!rbtOtros.Checked)
            {
                if (int.Parse(ddlNombreTransaccion.SelectedValue) == 0)
                {
                    alert = @"swal('Aviso!', 'Seleccione la nombre de la trasacción', 'error');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

                    goto FINISH;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(TxtOtraTransaccion.Text))
                {
                    alert = @"swal('Aviso!', 'Debe digitar el nombre de la transacción', 'error');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

                    goto FINISH;
                }
            }

            #endregion


            if (!string.IsNullOrEmpty(txtCantidad.Text))
            {

                txtIngreso.Text = string.Empty;
                txtCambio.Text = string.Empty;

                if (rbtProducto.Checked)
                {
                    int stock = Convert.ToInt32(txtStock.Text.ToString());
                    if (stock == 0)
                    {
                        alert = @"swal('Aviso!', 'No hay producto disponible', 'error');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

                        goto FINISH;

                    }
                    else if (int.Parse(txtCantidad.Text) > stock)
                    {
                        alert = @"swal('Aviso!', 'La cantidad del producto a facturar es mayor al stock. Verifique!', 'error');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

                        goto FINISH;
                    }
                }

                if (!rbtOtros.Checked)
                {
                    foreach (GridViewRow rows in GridProductos.Rows)
                    {
                        int idTransaccionGrid = int.Parse(rows.Cells[0].Text);

                        if (idTransaccionGrid == int.Parse(ddlNombreTransaccion.SelectedValue))
                        {
                            bandera = true;
                            break;
                        }
                    }
                }


                if (!bandera)
                {

                    DataTable dt = Session["tabla"] as DataTable;

                    var subtotal = Convert.ToDouble(txtPrecio.Text) * Convert.ToDouble(txtCantidad.Text);
                    DataRow row = dt.NewRow();

                    btnGuardar.Visible = true;

                    row["Id"] = !rbtOtros.Checked ? int.Parse(ddlNombreTransaccion.SelectedValue) : 0;
                    row["Item"] = rbtOtros.Checked ? TxtOtraTransaccion.Text.ToUpper().TrimEnd() : Convert.ToString(ddlNombreTransaccion.SelectedItem);
                    row["Precio"] = txtPrecio.Text;
                    row["Cantidad"] = txtCantidad.Text;
                    row["Subtotal"] = subtotal;
                    row["Stock"] = rbtProducto.Checked ? Convert.ToInt32(txtStock.Text) : 0;

                    dt.Rows.Add(row);

                    Session["tabla"] = dt;   // Guardar la tabla con el nuevo registro
                    BindGrid();              // Actualizar Grid
                }
                else
                {
                    alert = @"swal('Aviso!', 'El tipo de trasacción ya está agregada', 'error');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

                }

                txtIngreso.Enabled = true;
                btnGuardar.Enabled = true;
                pnlDetalle.Visible = true;
                TxtOtraTransaccion.Text = string.Empty;
                txtCantidad.Text = string.Empty;
                txtPrecio.Text = string.Empty;
                txtCantidad.Text = string.Empty;
                txtStock.Text = string.Empty;
                ddlNombreTransaccion.SelectedValue = "0";

                CalculosGrid();

            }
            else
            {
                alert = @"swal('Aviso!', 'Debe ingresar la cantidad a facturar', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }

        FINISH:

            if (GridProductos.Rows.Count != 0)
            {
                GridProductos.UseAccessibleHeader = true;
                GridProductos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void GridProductos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = Session["tabla"] as DataTable;
            dt.Rows.RemoveAt(e.RowIndex);
            Session["tabla"] = dt;
            BindGrid();

            if (GridProductos.Rows.Count != 0)
            {
                btnGuardar.Visible = false;
                txtIngreso.Enabled = true;
                GridProductos.UseAccessibleHeader = true;
                GridProductos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            else
            {
                txtIngreso.Enabled = false;
                txtIngreso.Text = string.Empty;
                txtTotalCordoba.Text = string.Empty;
                txtTotalDolar.Text = string.Empty;
                txtCambio.Text = string.Empty;
                pnlDetalle.Visible = false;
                txtIngreso.Enabled = false;
            }

        }

        public void CalculosGrid()
        {
            double sutotal = 0;
            DataTable dt = Session["tabla"] as DataTable;
            foreach (DataRow row in dt.Rows)
            {
                sutotal += Convert.ToDouble(row["Subtotal"].ToString());

            }
            txtTotalCordoba.Text = Convert.ToString(sutotal);
            var conversionDolar = Convert.ToDouble(txtTotalCordoba.Text) / Convert.ToDouble(txtTipoCambio.Text);
            txtTotalDolar.Text = Convert.ToString(Math.Round(conversionDolar, 2));

        }

        protected void txtIngreso_TextChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtIngreso.Text))
            {
                if (Convert.ToDouble(txtIngreso.Text.ToString()) < Convert.ToDouble(txtTotalCordoba.Text.ToString()))
                {
                    alert = @"swal('Aviso!', 'El ingreso es menor al total C$. Verifique!', 'error');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

                }
                else
                {
                    var cambio = Convert.ToDouble(txtIngreso.Text) - Convert.ToDouble(txtTotalCordoba.Text);
                    txtCambio.Text = Convert.ToString(Math.Round(cambio, 2));
                }
            }
            GridProductos.UseAccessibleHeader = true;
            GridProductos.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombreFactura.Text))
            {
                alert = @"swal('Aviso!', 'Digite o seleccione el nombre de la persona a facturar.', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }
            else if (string.IsNullOrEmpty(txtIngreso.Text))
            {
                alert = @"swal('Aviso!', 'Debe digitar el ingreso', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalConfirm();", true);

            }
            GridProductos.UseAccessibleHeader = true;
            GridProductos.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            List<tmefacturasdet> listaDetalleFacturta = new List<tmefacturasdet>();
            if (!string.IsNullOrEmpty(txtIngreso.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModalConfirm(); RemoveBackDrop();", true);

                

                var datosCaja = (tmecajas)Session["DatosCaja"];
                // Si el cajero no tiene serie, se continua con el idFactura
                var nombreTabla = datosCaja.Serie !=  null ? datosCaja.Serie.TrimEnd() : "tmefacturas";
               
                var consecutivo = consultaNegocio.ConsultarContadorTabla(nombreTabla);
                var idFactura = consecutivo.id_contador != 43 ? consecutivo.tabla_contador : consecutivo.tabla_contador + 1;
                var idRecibo = consecutivo.id_contador != 43 ? Convert.ToString(idFactura + "" + nombreTabla) : Convert.ToString(idFactura);
                var idContador = consecutivo.id_contador != 43 ? idFactura + 1 : idFactura;

                // Insertar en la tabla factura
                tmefacturas factura = new tmefacturas();
                factura.id_factura = idRecibo;
                factura.tipo_factura = "MANUAL";
                factura.id_caja = txtIdCaja.Text.ToString();
                factura.fecha = Convert.ToDateTime(txtFecha.Text);
                factura.tipo_cambio = Convert.ToDouble(txtTipoCambio.Text);
                factura.id_estudiante = txtCodEstudiante.Text;
                factura.anombrede = txtNombreFactura.Text.ToUpper().TrimEnd();
                factura.total_cordobas = Convert.ToDouble(txtTotalCordoba.Text);
                factura.total_dolares = Convert.ToDouble(txtTotalDolar.Text);
                factura.cliente_paga = Convert.ToDouble(txtIngreso.Text);
                factura.cliente_cambio = Convert.ToDouble(txtCambio.Text);
                factura.fecha_cancela = Convert.ToDateTime(txtFecha.Text);
                factura.estado = Convert.ToString(ddlEstado.SelectedItem);
                factura.estado_por = txtUsuarioProcesa.Text;
                factura.forma_pago = Convert.ToString(ddlFormaPago.SelectedItem);
                factura.detalles = "";
                consultaNegocio.InsertarFactura(factura);
                consultaNegocio.ActualizarConsecutivoTabla(consecutivo.id_contador, (long)idContador);

                tmefacturasdet detalleFactura = null;
                foreach (GridViewRow rows in GridProductos.Rows)
                {
                    // insertar en detalle factura
                    detalleFactura = new tmefacturasdet();
                    detalleFactura.id_item = (!rbtOtros.Checked) ? rows.Cells[0].Text : "0";
                    detalleFactura.id_factura = factura.id_factura;
                    detalleFactura.nombre_item = rows.Cells[1].Text;
                    detalleFactura.preciounitario = Convert.ToDouble(rows.Cells[2].Text);
                    detalleFactura.cantidad = int.Parse(rows.Cells[3].Text);
                    detalleFactura.subtotal = Convert.ToDouble(rows.Cells[4].Text);
                    detalleFactura.tipo_item = rbtConcepto.Checked ? "CONCEPTO" : rbtProducto.Checked
                                                ? "INVENTARIO" : rbtTransacciones.Checked ? "TRANSACCIONES"
                                                : "OTROS/VARIOS";
                    consultaNegocio.InsertarDetalleFactura(detalleFactura);
                    listaDetalleFacturta.Add(detalleFactura);

                    // Si es un producto actualizar el stock
                    if (rbtProducto.Checked)
                    {
                        string idProducto = rows.Cells[0].Text;
                        var stockNuevo = Convert.ToInt32(rows.Cells[5].Text) - Convert.ToInt32(rows.Cells[3].Text.ToString());
                        consultaNegocio.ActualizarStock(idProducto, stockNuevo);
                    }

                    // Actualizar en la tabla transacciones
                    if (rbtTransacciones.Checked)
                    {
                        int IdTransaccion = int.Parse(rows.Cells[0].Text);
                        transaccionNegocio.ActualizarTrasaccionPorId(IdTransaccion);
                    }
                }

                LimpiarCampos();

                GenerarPDF(factura, listaDetalleFacturta);


            }
            else
            {
                GridProductos.UseAccessibleHeader = true;
                GridProductos.HeaderRow.TableSection = TableRowSection.TableHeader;

                alert = @"swal('Aviso!', 'Debe digitar el ingreso', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }
        }

        public void GenerarPDF(tmefacturas factura, List<tmefacturasdet> listaDetalle)
        {
            string rutaPdf = CrearEstructuraDocumento(factura, listaDetalle);

            string script = $@"
                    swal({{
                        title: 'Éxito',
                        text: 'Se registro el pago con Recibo No. {factura.id_factura}',
                        type: 'success'
                    }},
                    function() {{
                        window.open('{rutaPdf}', '_blank');
                    }});
                ";

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AlertaPDF", script, true);

        }

        private string CrearEstructuraDocumento(tmefacturas factura, List<tmefacturasdet> listaDetalle)
        {
            string nombrePdf = "Recibo_" + factura.id_factura + ".pdf";
            string carpeta = Server.MapPath("~/Soporte/");

            // Si no existe la carpeta, crearla
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            // Ruta física donde se guardará
            string rutaFisica = Server.MapPath("~/Soporte/" + nombrePdf);

            using (FileStream fs = new FileStream(rutaFisica, FileMode.Create))
            {
                Document document1 = new Document(PageSize.A4);
                PdfWriter writer1 = PdfWriter.GetInstance(document1, fs);

                //float anchoRecibo = 226.77f;
                //float altoRecibo = 566.93f;
                //Rectangle tamanioPersonalizado = new Rectangle(anchoRecibo, altoRecibo);
                //Document document2 = new Document(tamanioPersonalizado);

                Document document = new Document(new Rectangle(226.77f, 425.20f), 5f, 5f, 5f, 5f);

                PdfWriter writer = PdfWriter.GetInstance(document, fs);
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


                Paragraph Titulo7 = new Paragraph("Cajero : " + factura.estado_por , new Font(Font.FontFamily.TIMES_ROMAN, 9));
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
                writer.Close();
            }

            // Retornar la ruta virtual para abrir en el navegador
            return "/Soporte/" + nombrePdf;
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
            txtRecibo.Text = string.Empty;
            txtNombreFactura.Text = string.Empty;
            txtNombreFactura.Enabled = false;
            txtTotalCordoba.Text = string.Empty;
            txtTotalDolar.Text = string.Empty;
            txtIngreso.Enabled = false;
            txtIngreso.Text = string.Empty;
            txtCambio.Text = string.Empty;
            txtCodEstudiante.Text = string.Empty;

            rbtConcepto.Checked = false;
            rbtProducto.Checked = false;
            rbtTransacciones.Checked = false;
            rbtOtros.Checked = false;

            TxtOtraTransaccion.Visible = false;
            ddlNombreTransaccion.Visible = true;
            ddlNombreTransaccion.Enabled = false;
            btnAgregar.Enabled = false;

            ddlNombreTransaccion.Items.Clear();
            ddlNombreTransaccion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("SELECCIONE", "0"));

            GridEstudiantes.DataSource = null;
            GridEstudiantes.DataBind();

            LimpiarGridProducto();
        }

        public void LimpiarGridProducto()
        {
            DataTable dt = Session["tabla"] as DataTable;
            dt.Rows.Clear();
            Session["tabla"] = dt;
            BindGrid();

            Session["ListaConceptoFactura"] = null;
            Session["ListaProductosFactura"] = null;
            Session["ListaTransaccionFactura"] = null;

            divStock.Visible = false;
            pnlDetalle.Visible = false;

            txtPrecio.Enabled = false;
            txtStock.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            txtCantidad.Enabled = false;
            txtCantidad.Text = string.Empty;

        }
    }
}