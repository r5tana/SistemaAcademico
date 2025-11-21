using Entidades;
using Negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Media.Media3D;

namespace SistemaFinanciero
{
    public partial class WebFormAddWorker : System.Web.UI.Page
    {
        List<tmxusuarios> listaUsuarios = null;
        UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
        TransaccionNegocio cajeroNegocio = new TransaccionNegocio();
        Utilitario utilitario = null;
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

                tmxusuarios usuario = new tmxusuarios();

                var UsuarioId = int.Parse(Session["Id"].ToString());

                usuario.id_usuario = UsuarioId;
                usuario.nombres = txtNombres.Text.TrimEnd().ToUpper();
                usuario.apellidos = txtApellidos.Text.TrimEnd().ToUpper();

                if (ddlCargo.SelectedValue != "N/A")
                    usuario.cargo = ddlCargo.SelectedValue;
                else
                    usuario.cargo = txtCargo.Text;

                usuarioNegocio.ActualizarUsuario(usuario);

                LimpiarCampos();
                btnAgregar.Visible = true;
                pnlDatos.Visible = false;

                alert = @"swal('Aviso!', 'Se actualizo el registro correctamente.', 'success');";
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

                if (datos != null)
                {
                    txtlogin.Text = datos.usuario;
                    txtNombres.Text = datos.nombres;
                    txtApellidos.Text = datos.apellidos;
                    txtEstado.Text = datos.estado;

                    if (datos.cargo.TrimEnd() != "ADMINISTRADOR" & datos.cargo.TrimEnd() != "CAJERO" & datos.cargo.TrimEnd() != "CONTADORA")
                    {
                        txtCargo.Visible = true;
                        txtCargo.Text = datos.cargo;
                        ddlCargo.Visible = false;
                    }
                    else
                    {
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

            CargarUsuarios();

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
    }
}