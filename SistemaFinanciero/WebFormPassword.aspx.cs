using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaFinanciero
{
    public partial class WebFormPassword : System.Web.UI.Page
    {
        private UsuarioNegocio negocio = new UsuarioNegocio();
        string alert = string.Empty;
        int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["IdTemporal"] != null)
                {
                    id = int.Parse(Session["IdTemporal"].ToString());
                }
            }
        }


        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                int usuarioId = int.Parse(Session["IdTemporal"].ToString());
                string claveAnterior = txtClaveAnterior.Text;
                string claveNueva = txtClaveNueva.Text;
                string claveConfirmada = txtConfirmarClave.Text;

                var detalleUsuario = negocio.ConsultarUsuarioId(usuarioId);

                if (detalleUsuario != null)
                {
                    var claveAnteriorEnctipt = negocio.Encriptar(claveAnterior);
                    var claveNuevaEnctipt = negocio.Encriptar(claveNueva);

                    if (claveAnteriorEnctipt != detalleUsuario.PasswordWeb)
                    {
                        alert = @"swal('Aviso!', 'Contraseña anterior no coincide con la registrada. Verifique!', 'error');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

                        return;
                    }
                    else if (claveNueva == txtClaveAnterior.Text)
                    {
                        alert = @"swal('Aviso!', 'Contraseña nueva y anterior no deben ser iguales. Verifique!', 'error');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

                        return;
                    }
                    else if (claveNueva != claveConfirmada)
                    {
                        alert = @"swal('Aviso!', 'Contraseña nueva y confirmada no coinciden. Verifique!', 'error');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

                        return;
                    }
                    else
                    {
                        negocio.CambioClaveUsuario(usuarioId, claveNuevaEnctipt);

                        txtClaveAnterior.Text = string.Empty;
                        txtClaveNueva.Text = string.Empty;
                        txtConfirmarClave.Text = string.Empty;

                        //Limpiar sesiones
                        Session["IdTemporal"] = null;
                        Session["bandera"] = true;

                        Response.Redirect("WebFormLogin.aspx");

                    }

                }
            }
            else
            {
                alert = @"swal('Aviso!', 'Todos los campos son obligatorios', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }
        }

        public bool ValidarCampos()
        {
            bool bandera = false;


            if (!string.IsNullOrEmpty(txtClaveAnterior.Text) & !string.IsNullOrEmpty(txtClaveNueva.Text)
            & !string.IsNullOrEmpty(txtConfirmarClave.Text))
            {
                bandera = true;
            }
            return bandera;

        }

    }
}