using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaFinanciero
{
    public partial class WebFormChangePassword : System.Web.UI.Page
    {
        private UsuarioNegocio negocio = new UsuarioNegocio();
        string alert = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnguardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                int usuarioId = int.Parse(Session["IdUsuario"].ToString());
                string claveAnterior = txtclaveanterior.Text;
                string claveNueva = txtclavenueva.Text;
                string claveConfirmada = txtconfirmarclave.Text;

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
                    else if (claveNueva == claveAnterior)
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

                        txtclaveanterior.Text = string.Empty;
                        txtclavenueva.Text = string.Empty;
                        txtconfirmarclave.Text = string.Empty;

                        alert = @"swal('Aviso!', 'Se realizó en cambio de contraseña', 'success');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
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


            if (!string.IsNullOrEmpty(txtclaveanterior.Text) & !string.IsNullOrEmpty(txtclavenueva.Text)
            & !string.IsNullOrEmpty(txtconfirmarclave.Text))
            {
                bandera = true;
            }
            return bandera;

        }
    }
}