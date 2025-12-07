using Entidades;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Negocio;
using OfficeOpenXml.Style;
using SistemaFinanciero.Account;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;
using System.Xml.Linq;
using Image = iTextSharp.text.Image;

namespace SistemaFinanciero
{
    public partial class WebFormPay : System.Web.UI.Page
    {
        EstudianteNegocio negocioEstudiante = new EstudianteNegocio();
        ConsultaNegocio consultaNegocio = new ConsultaNegocio();
        TransaccionNegocio transaccionNegocio = new TransaccionNegocio();
        UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
        Utilitario utilitario = new Utilitario();

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
                var nombreTabla = datosCaja.Serie != null ? datosCaja.Serie.TrimEnd() : "tmefacturas";

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

            List<DetalleEstudianteDto> listaEstudiante = (List<DetalleEstudianteDto>)Session["listaEstudiantesFactura"];

            var detalleAlumno = (factura.id_estudiante != null || !string.IsNullOrEmpty(factura.id_estudiante))
                                    ? listaEstudiante.Where(x => x.CodigoEstudiante == factura.id_estudiante).FirstOrDefault()
                                : null;

            var nombreCajero = (factura.estado_por != null || !string.IsNullOrEmpty(factura.estado_por))
                                    ? usuarioNegocio.NombreUsuario(factura.estado_por)
                                : null;

            string Grado = detalleAlumno != null ? detalleAlumno.Grado : "";
            string Seccion = detalleAlumno != null ? detalleAlumno.Seccion : "";
            string cajeroNombre = !string.IsNullOrEmpty(nombreCajero) ? nombreCajero : "";
            string valorEnLetras = Utilitario.ConvertirMontoALetras(Convert.ToDecimal(factura.total_cordobas));

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

            using (FileStream fs = new FileStream(rutaFisica, FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
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

                    // Simular el checkbox no seleccionado con un carácter unicode o ASCII '☐'
                    //string check = "☐";

                    //Paragraph pago = new Paragraph();
                    //pago.Add(new Chunk("Pagó en: ", fontNormal));
                    //pago.Add(new Chunk($"Efectivo {check}  ", fontNormal));
                    //pago.Add(new Chunk($"Tarjeta {check}  ", fontNormal));
                    //pago.Add(new Chunk($"Cheque {check}", fontNormal));


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

                    // Simular chequeo basado en el campo forma_pago de tu objeto factura
                    // if (factura.forma_pago.ToUpper().Contains("EFECTIVO"))
                    // { pago = new Paragraph().Add(new Chunk($"Pagó en: Efectivo \u2611  Tarjeta ☐  Cheque ☐", fontNormal)); }

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


                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    return "ERROR";
                }
                finally
                {
                    if (document.IsOpen())
                    {
                        document.Close();
                    }
                }
            }

            return "/Soporte/" + nombrePdf;
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