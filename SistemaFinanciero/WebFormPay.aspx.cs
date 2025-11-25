using Entidades;
using Negocio;
using OfficeOpenXml.Style;
using SistemaFinanciero.Account;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;

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
                txtIdCaja.Text = detalleCaja.id_caja;
                txtCaja.Text = detalleCaja.nombre.TrimEnd();
                txtTipoCambio.Text = detalleCaja.tipocambio.ToString();
                txtUsuarioProcesa.Text = login;
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
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


        }

        protected void btnAbrir_Click(object sender, EventArgs e)
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

                // Si usas DataKeys
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
                    if (!string.IsNullOrEmpty(txtNombreFactura.Text))
                    {
                        txtNombreFactura.Enabled = true;
                        var transacciones = transaccionNegocio.ListaTransaccionesPendientesEstudiante(txtCodEstudiante.Text).OrderBy(x => x.DescripcionTransaccion).ToList();
                        if (transacciones != null)
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
                            ddlNombreTransaccion.DataSource = transacciones;
                            ddlNombreTransaccion.DataTextField = "DescripcionTransaccion";
                            ddlNombreTransaccion.DataValueField = "IdTransaccion";
                            ddlNombreTransaccion.DataBind();

                            ddlNombreTransaccion.Items.Insert(0, new ListItem("SELECCIONE", "0"));
                        }
                        else
                        {
                            txtPrecio.Enabled = false;
                            txtCantidad.Enabled = false;
                            btnAgregar.Enabled = false;
                            ddlNombreTransaccion.Items.Clear();
                            ddlNombreTransaccion.Items.Insert(0, new ListItem("SELECCIONE", "0"));

                            alert = @"swal('Aviso!', 'Estudiante seleccionado no tiene transacciones pendientes', 'error');";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
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
                        ddlNombreTransaccion.Items.Clear();
                        ddlNombreTransaccion.Items.Insert(0, new ListItem("SELECCIONE", "0"));

                        alert = @"swal('Aviso!', 'Seleccione el estudiante del aracel a cancelar', 'error');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                    }
                }
                else
                {

                    if (rbtProducto.Checked)
                    {

                        divStock.Visible = true;
                        var productos = consultaNegocio.ListaInventario().OrderBy(x => x.prod_nombre).ToList();
                        if (productos != null)
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
                            ddlNombreTransaccion.DataSource = productos;
                            ddlNombreTransaccion.DataTextField = "prod_nombre";
                            ddlNombreTransaccion.DataValueField = "id_inventario";
                            ddlNombreTransaccion.DataBind();

                            ddlNombreTransaccion.Items.Insert(0, new ListItem("SELECCIONE", "0"));
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

                        var concepto = consultaNegocio.ListaConceptos().OrderBy(x => x.nombre).ToList();
                        if (concepto != null)
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
                            ddlNombreTransaccion.DataSource = concepto;
                            ddlNombreTransaccion.DataTextField = "nombre";
                            ddlNombreTransaccion.DataValueField = "id_concepto";
                            ddlNombreTransaccion.DataBind();

                            ddlNombreTransaccion.Items.Insert(0, new ListItem("SELECCIONE", "0"));
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
                    List<tmetransacciones> listaTransacciones = (List<tmetransacciones>)Session["ListaTransaccionFactura"];
                    var seleccion = listaTransacciones.Where(x => x.idtrans == transaccion).FirstOrDefault();

                    if (seleccion.total_cordobas == 0)
                    {
                        txtPrecio.Text = string.Empty;
                        txtPrecio.Enabled = true;
                    }
                    else
                    {
                        txtPrecio.Enabled = false;
                        txtPrecio.Text = seleccion.total_cordobas.ToString();

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
            List<DetallePago> detalle = new List<DetallePago>();

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
                        var id = rows.Cells[0].Text;

                        if (int.Parse(id) == int.Parse(ddlNombreTransaccion.SelectedValue))
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
                    if (!rbtOtros.Checked)
                        row["Id"] = int.Parse(ddlNombreTransaccion.SelectedValue);

                    row["Item"] = rbtOtros.Checked ? TxtOtraTransaccion.Text : Convert.ToString(ddlNombreTransaccion.SelectedItem);
                    row["Precio"] = txtPrecio.Text;
                    row["Cantidad"] = txtCantidad.Text;
                    row["Subtotal"] = subtotal;

                    if (rbtProducto.Checked)
                        row["Stock"] = txtStock.Text;

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

            if (!string.IsNullOrEmpty(txtIngreso.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModalConfirm(); RemoveBackDrop();", true);

                var noombreTabla = "tmefacturas";
                var consecutivo = consultaNegocio.ConsultarContadorTabla(noombreTabla);
                var idFactura = consecutivo.tabla_contador + 1;

                // Insertar en la tabla factura
                tmefacturas factura = new tmefacturas();
                factura.id_factura = Convert.ToString(idFactura);
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
                consultaNegocio.ActualizarConsecutivoTabla(consecutivo.id_contador, (long)idFactura);

                foreach (GridViewRow rows in GridProductos.Rows)
                {
                    // insertar en detalle factura
                    tmefacturasdet detalleFactura = new tmefacturasdet();
                    detalleFactura.id_factura = factura.id_factura;
                    detalleFactura.nombre_item = rows.Cells[1].Text;
                    detalleFactura.cantidad = int.Parse(rows.Cells[2].Text);
                    detalleFactura.preciounitario = int.Parse(rows.Cells[3].Text);
                    detalleFactura.subtotal = Convert.ToDouble(rows.Cells[4].Text);
                    consultaNegocio.InsertarDetalleFactura(detalleFactura);

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

                alert = @"swal('Aviso!', 'Se registro la factura con No. " + factura.id_factura + "', 'success');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

            }
            else
            {
                GridProductos.UseAccessibleHeader = true;
                GridProductos.HeaderRow.TableSection = TableRowSection.TableHeader;

                alert = @"swal('Aviso!', 'Debe digitar el ingreso', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }
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


            LimpiarGridProducto();
        }

        public void LimpiarGridProducto()
        {
            DataTable dt = Session["tabla"] as DataTable;
            dt.Rows.Clear();
            Session["tabla"] = dt;

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


        public class DetallePago
        {
            public int ID { get; set; }
            public decimal Valor { get; set; }
            public decimal Cantidad { get; set; }
        }

    }
}