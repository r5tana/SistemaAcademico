using Entidades;
using iTextSharp.text.pdf.codec.wmf;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Media.Media3D;

namespace SistemaFinanciero
{
    public partial class WebFormAddWorker : System.Web.UI.Page
    {
        UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
        ConsultaNegocio consultaNegocio = new ConsultaNegocio();
        string alert = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarUsuarios();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            btnGuardar.Visible = true;
            btnActualizar.Visible = false;
            btnAgregar.Visible = false;
            pnlDatos.Visible = true;
            ddlCargo.Visible = true;
            txtCargo.Visible = false;
            lblsms.Visible = true;
            txtEstado.Text = "ACTIVO";

            GridTrabajadores.UseAccessibleHeader = true;
            GridTrabajadores.HeaderRow.TableSection = TableRowSection.TableHeader;

            Session["Proceso"] = 1; // Nuevo registro


        }

        public void CargarUsuarios()
        {
            List<tmxusuarios> lista = new List<tmxusuarios>();
            lista = usuarioNegocio.ListaUsuarios();
            Session["listaUsuarios"] = lista;

            this.GridTrabajadores.DataSource = lista;
            GridTrabajadores.DataBind();
            GridTrabajadores.UseAccessibleHeader = true;
            GridTrabajadores.HeaderRow.TableSection = TableRowSection.TableHeader;

        }

        void LimpiarCampos()
        {
            txtlogin.Enabled = true;
            txtlogin.Text = string.Empty;
            txtNombres.Text = string.Empty;
            txtApellidos.Text = string.Empty;
            txtCargo.Text = string.Empty;
            ddlCargo.SelectedValue = "N/A";
            divSerie.Visible = false;
            Session["EsCajero"] = null;
            Session["Proceso"] = null;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            if (ValidarCampos(1))
            {
                var login = ConstruirUsuario().ToUpper();

                tmxusuarios usuario = new tmxusuarios();
                usuario.nombres = txtNombres.Text.TrimEnd().ToUpper();
                usuario.apellidos = txtApellidos.Text.TrimEnd().ToUpper();
                usuario.cargo = ddlCargo.SelectedValue;
                usuario.usuario = login;
                usuario.PasswordWeb = usuarioNegocio.Encriptar("2025");
                usuario.contrasena = usuario.PasswordWeb;
                usuario.fecha_inicio = Convert.ToString(DateTime.Now).ToString();
                usuario.estado = "ACTIVO";
                usuario.nivel = "";
                usuario.fecha_fin = "";
                usuario.id_docente = 0;
                usuario.sesion = "INACTIVA";


                var ultimoId = usuarioNegocio.ListaUsuarios().OrderBy(x => x.id_usuario).LastOrDefault().id_usuario;
                usuario.id_usuario = ultimoId + 1;

                usuarioNegocio.CrearUsuario(usuario);


                if (ddlCargo.SelectedValue == "CAJERO")
                    CrearCajaUsuario(usuario.id_usuario);


                LimpiarCampos();
                pnlDatos.Visible = false;
                btnAgregar.Visible = true;
                CargarUsuarios();

                alert = @"swal('Aviso!', 'Se registro el usuario " + usuario.usuario + " correctamente', 'success');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

            }
            else
            {
                alert = @"swal('Aviso!', 'Los campos Nombres, Primer Apellido y Cargo son requeridos', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }

            GridTrabajadores.UseAccessibleHeader = true;
            GridTrabajadores.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos(2))
            {

                tmxusuarios usuarioModificado = new tmxusuarios();

                var UsuarioId = int.Parse(Session["Id"].ToString());
                var usuarioEntidad = usuarioNegocio.ConsultarUsuarioId(UsuarioId);

                usuarioModificado.id_usuario = UsuarioId;
                usuarioModificado.nombres = txtNombres.Text.TrimEnd().ToUpper();
                usuarioModificado.apellidos = txtApellidos.Text.TrimEnd().ToUpper();

                if (ddlCargo.SelectedValue != "N/A")
                    usuarioModificado.cargo = ddlCargo.SelectedValue;
                else
                    usuarioModificado.cargo = txtCargo.Text;

                usuarioNegocio.ActualizarUsuario(usuarioModificado);


                bool validaSerie = false;
                bool actualizaSerie = false;

                //Se le cambio el cargo
                if (usuarioEntidad.cargo.ToUpper().TrimEnd() != usuarioModificado.cargo.ToUpper().TrimEnd())
                {
                    if (usuarioEntidad.cargo.ToUpper().TrimEnd() == "CAJERO") // Antes era cajero
                    {
                        consultaNegocio.ActivarInactivarCaja(usuarioEntidad.id_usuario, 2); // Inactivar
                    }
                    else if (usuarioModificado.cargo.ToUpper().TrimEnd() == "CAJERO") // Se le cambio cargo a cajero
                    {
                        var existeCaja = consultaNegocio.ListarCajas().Where(x => x.id_usuario == usuarioModificado.id_usuario).FirstOrDefault();

                        if (existeCaja != null)
                        {
                            validaSerie = true;
                            consultaNegocio.ActivarInactivarCaja(usuarioEntidad.id_usuario, 1); // Activar
                        }
                        else
                            CrearCajaUsuario(usuarioModificado.id_usuario);
                    }
                }
                else
                {
                    if (usuarioModificado.cargo.ToUpper().TrimEnd() == "CAJERO")
                        validaSerie = true;

                }

                if (validaSerie)
                {
                    string serie = Convert.ToString(ddlSerie.SelectedItem);
                    var cajaUsuario = consultaNegocio.ConsultarCajaUsuario(usuarioEntidad.id_usuario);

                    //Validaciones: 1. Se seleccino serie, 2. El usuario tiene serie registrada, 3. Se le hizo camio de serie
                    actualizaSerie = int.Parse(ddlSerie.SelectedValue) != 0
                                            ? cajaUsuario.Serie != null
                                                    ? cajaUsuario.Serie != serie
                                                            ? true
                                                    : false
                                            : true
                                    : false;



                    if (actualizaSerie)
                        consultaNegocio.ActualizarSerieCaja(usuarioEntidad.id_usuario, serie);

                }


                LimpiarCampos();
                btnAgregar.Visible = true;
                pnlDatos.Visible = false;

                alert = @"swal('Aviso!', 'Se actualizó el registro correctamente.', 'success');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);


            }
            else
            {
                alert = @"swal('Aviso!', 'Favor rellenar los campos requeridos.', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }

            CargarUsuarios();

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            pnlDatos.Visible = false;
            btnAgregar.Visible = true;

            GridTrabajadores.UseAccessibleHeader = true;
            GridTrabajadores.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        public bool ValidarCampos(int tipoProceso)
        {
            bool bandera = false;

            if (tipoProceso == 1) // Agregar usuario
            {
                if (!string.IsNullOrEmpty(txtNombres.Text) & !string.IsNullOrEmpty(txtApellidos.Text)
                & ddlCargo.SelectedValue != "N/A")
                {
                    bandera = true;

                }
            }
            else // modificar usuario
            {
                if (!string.IsNullOrEmpty(txtNombres.Text) & !string.IsNullOrEmpty(txtApellidos.Text))
                {
                    bandera = true;
                }
            }

            if (Session["EsCajero"] != null)
            {
                // Validación para modificar o crear usuario
                if (bool.Parse(Session["EsCajero"].ToString()))
                {
                    int total = ddlSerie.Items.Count - 1;
                    if (total > 0 & int.Parse(ddlSerie.SelectedValue) == 0)
                    {
                        bandera = false;
                        alert = @"swal('Aviso!', 'Existen series disponible, favor asigne una al usuario', 'error');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

                    }

                }

            }


            return bandera;

        }

        protected void GridTrabajadores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            Session["Id"] = index;

            var datos = usuarioNegocio.ConsultarUsuarioId(index);

            if (e.CommandName == "ModificarUsuario")
            {
                LimpiarCampos();
                pnlDatos.Visible = true;
                btnGuardar.Visible = false;
                btnActualizar.Visible = true;
                lblsms.Visible = false;

                Session["Proceso"] = 2; // Modificación de registro

                if (datos != null)
                {
                    txtlogin.Text = datos.usuario;
                    txtNombres.Text = datos.nombres;
                    txtApellidos.Text = datos.apellidos;
                    txtEstado.Text = datos.estado;
                    txtIdUsuarioActual.Text = datos.id_usuario.ToString();

                    if (datos.cargo.TrimEnd() != "ADMINISTRADOR" & datos.cargo.TrimEnd() != "CAJERO" & datos.cargo.TrimEnd() != "CONTADORA")
                    {
                        txtCargo.Visible = true;
                        txtCargo.Text = datos.cargo;
                        ddlCargo.Visible = false;
                        divSerie.Visible = false;

                        Session["EsCajero"] = false;
                    }
                    else
                    {
                        if (datos.cargo.TrimEnd() == "CAJERO")
                        {

                            Session["EsCajero"] = true;
                            ConsultarCajaSerie(2); //tipoProceso = 2 -> modificación de datos
                        }
                        else
                        {
                            Session["EsCajero"] = false;
                            divSerie.Visible = false;
                        }

                        txtCargo.Visible = false;
                        ddlCargo.Visible = true;
                        ddlCargo.SelectedValue = datos.cargo;
                    }

                    txtlogin.Enabled = false;
                    btnAgregar.Visible = false;
                }
            }
            else if (e.CommandName == "InactivarUsuario")
            {
                usuarioNegocio.Activar_InactivarUsuario(index);

                var usuarioInactivo = usuarioNegocio.ConsultarUsuarioActivoInactivoId(index);
                if (usuarioInactivo.cargo.TrimEnd() == "CAJERO")
                {
                    int estado = (usuarioInactivo.estado == "ACTIVO" | usuarioInactivo.estado == "ACTIVA") ? 1 : 2;
                    consultaNegocio.ActivarInactivarCaja(usuarioInactivo.id_usuario, estado);
                }

                alert = @"swal('Aviso!', 'Éxito', 'success');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }

            else if (e.CommandName == "ResetearClave")
            {
                if (datos != null)
                {
                    string password = usuarioNegocio.Encriptar("2025");

                    usuarioNegocio.ResetPassword(index, password);

                    alert = @"swal('Aviso!', 'Se reseteo contraseña correctamente', 'success');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

                }
            }


            GridTrabajadores.UseAccessibleHeader = true;
            GridTrabajadores.HeaderRow.TableSection = TableRowSection.TableHeader;

        }

        protected void GridTrabajadores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                var strEstado = e.Row.Cells[10].Text;

                if (strEstado == "ACTIVO" || strEstado == "ACTIVA")
                {
                    // button edit
                    e.Row.Cells[0].Enabled = true;
                    var lbtn = e.Row.Cells[0].FindControl("btnEditar") as LinkButton;
                    LiteralControl lc = new LiteralControl();
                    lc.Text = "<i  id='asd' class='glyphicon glyphicon-pencil' aria-hidden='true' ></i>";
                    lbtn.Controls.Add(lc);
                    lbtn.CssClass = "circle btn btn-success grow";

                    // button reset
                    e.Row.Cells[2].Enabled = true;
                    var lbtn2 = e.Row.Cells[2].FindControl("btnResetearClave") as LinkButton;
                    LiteralControl lc2 = new LiteralControl();
                    lc2.Text = "<i  id='asd' class='glyphicon glyphicon-refresh' aria-hidden='true' ></i>";
                    lbtn2.Controls.Add(lc2);
                    lbtn2.CssClass = "circle btn btn-info grow ";


                }
                else
                {
                    // button edit
                    e.Row.Cells[0].Enabled = false;
                    var lbtn = e.Row.Cells[0].FindControl("btnEditar") as LinkButton;
                    LiteralControl lc = new LiteralControl();
                    lc.Text = "<i  id='asd' class='glyphicon glyphicon-pencil' aria-hidden='true' ></i>";
                    lbtn.Controls.Add(lc);
                    lbtn.CssClass = "circle btn btn-success grow";

                    // button reset
                    e.Row.Cells[2].Enabled = false;
                    var lbtn2 = e.Row.Cells[2].FindControl("btnResetearClave") as LinkButton;
                    LiteralControl lc2 = new LiteralControl();
                    lc2.Text = "<i  id='asd'class='glyphicon glyphicon-refresh' aria-hidden='true' ></i>";
                    lbtn2.Controls.Add(lc2);
                    lbtn2.CssClass = "circle btn btn-info grow";

                }
            }
        }

        public string ConstruirUsuario()
        {
            string primeraLetraNombre = txtNombres.Text.Trim().Substring(0, 1);
            string apellido = txtApellidos.Text.Trim();

            string[] palabras = apellido.Split(' '); // Divide por espacios
            string primerApellido = palabras[0].TrimEnd();

            string nombreUsuario = (primeraLetraNombre + primerApellido).ToUpper();

            string nombreGenerado = nombreUsuario;
            bool existe;
            do
            {
                // Consulta la base de datos para verificar si el nombre existe
                existe = usuarioNegocio.ConsultarUsuario(nombreGenerado) != null ? true : false;

                if (existe)
                {
                    // Si existe, genera uno nuevo
                    nombreGenerado = nombreGenerado + "" + Guid.NewGuid().ToString().Substring(0, 2);
                }
            } while (existe);


            return nombreGenerado;

        }

        public void ConsultarCajaSerie(int tipoProceso = 0)
        {

            CargarSerie();
            List<tmxcontador> listaSeries1 = new List<tmxcontador>();
            var listaCajas = consultaNegocio.ListarCajas();
            var listaSeries = consultaNegocio.ListarContadorSeries();
            bool bandera = false;

            if (tipoProceso != 1) // Modificar usuario
            {
                var idUsuario = Convert.ToUInt32(txtIdUsuarioActual.Text);
                var cajaUsuario = consultaNegocio.ListarCajas().Where(x => x.id_usuario == idUsuario).FirstOrDefault();
                if (cajaUsuario == null)
                {
                    bandera = true;
                }
                else if (!string.IsNullOrEmpty(cajaUsuario.Serie))
                {
                    bandera = false;

                    // 1. Obtener la caja que voy a editar
                    var serieActual = cajaUsuario.Serie;
                    var idContadorSerie = consultaNegocio.ConsultarContadorTabla(serieActual).id_contador;

                    // Obtener las series que ya existen en caja a excepción de la que tiene el usuario
                    var seriesEnCaja = new HashSet<string>(listaCajas.Where(x => x.id_usuario != idUsuario & x.Serie != null).Select(x => x.Serie));

                    // Remover de la lista series todo lo que esté en caja (en este caso se usa tabla_nombre)
                    listaSeries.RemoveAll(s => seriesEnCaja.Contains(s.tabla_nombre));

                    foreach (var serie in seriesEnCaja)
                    {
                        var item = ddlSerie.Items.FindByText(serie.ToString());
                        if (item != null)
                            ddlSerie.Items.Remove(item);
                    }

                    ddlSerie.SelectedValue = idContadorSerie.ToString();

                }
                else
                    bandera = true;
            }
            else
                bandera = true;

            if (bandera)
            {
                // Obtener las series que ya existen en caja
                var seriesEnCaja = new HashSet<string>(listaCajas.Where(x => x.Serie != null).Select(x => x.Serie));

                // Remover de la lista series todo lo que esté en caja (en este caso se usa tabla_nombre)
                listaSeries.RemoveAll(s => seriesEnCaja.Contains(s.tabla_nombre));


                foreach (var serie in seriesEnCaja)
                {
                    var item = ddlSerie.Items.FindByText(serie.ToString());
                    if (item != null)
                        ddlSerie.Items.Remove(item);
                }
            }

            divSerie.Visible = true;
        }


        public void CrearCajaUsuario(int idUsuario)
        {

            var cajas = consultaNegocio.ListarCajas().OrderBy(x => int.Parse(x.id_caja)).ToList();
            var listaOrdenada = cajas.Count + 1;
            var id = cajas.Select(x => int.Parse(x.id_caja)).DefaultIfEmpty(0).Max() + 1;

            tmecajas caja = new tmecajas();
            caja.id_caja = Convert.ToString(id);
            caja.nombre = "CAJA" + " " + (listaOrdenada);
            caja.ubicacion = "SIGA";
            caja.id_usuario = idUsuario;
            caja.estado = "ACTIVO";
            caja.tipocambio = 36;
            caja.mensaje1 = "";
            caja.mensaje2 = "";
            if (int.Parse(ddlSerie.SelectedValue) != 0)
                caja.Serie = Convert.ToString(ddlSerie.SelectedItem);

            consultaNegocio.InsertarCaja(caja);

        }

        protected void ddlCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCargo.SelectedValue == "CAJERO")
            {
                Session["EsCajero"] = true;
                int tipoProceso = int.Parse(Session["Proceso"].ToString());
                ConsultarCajaSerie(tipoProceso);
                int total = ddlSerie.Items.Count - 1;

                if (total == 0)
                {
                    alert = @"swal('Aviso!', 'No existen series para asingan al cajero', 'error');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                }
            }
            else
            {
                Session["EsCajero"] = false;
                divSerie.Visible = false;
            }


            GridTrabajadores.UseAccessibleHeader = true;
            GridTrabajadores.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        public void CargarSerie()
        {
            List<tmxcontador> listaSeries = new List<tmxcontador>();
            listaSeries = consultaNegocio.ListarContadorSeries();
            ddlSerie.DataSource = listaSeries;
            ddlSerie.DataTextField = "tabla_nombre";
            ddlSerie.DataValueField = "id_contador";
            ddlSerie.DataBind();

            ddlSerie.Items.Insert(0, new ListItem("SELECCIONE", "0"));

        }
    }
}