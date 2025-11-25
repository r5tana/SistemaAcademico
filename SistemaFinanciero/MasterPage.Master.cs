using Entidades;
using Negocio;
using SistemaFinanciero.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaFinanciero
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        private UsuarioNegocio userNegocio = new UsuarioNegocio();
        private ConsultaNegocio consultaNegocio = new ConsultaNegocio();
        string alert = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Login"] != null)
            {
                var idUsuario = int.Parse(Session["IdUsuario"].ToString());
                var cargo = Session["RolUsuario"].ToString();
                lblNombreUsuario.Text = "Bienvenido/a " + userNegocio.NombreUsuario(Session["Login"].ToString());

                string Url = HttpContext.Current.Request.Url.AbsoluteUri.Substring(HttpContext.Current.Request.Url.AbsoluteUri.LastIndexOf('/') + 1);

                if (Url == "WebFormAddWorker.aspx" && cargo.TrimEnd() != "ADMINISTRADOR")
                {

                    Session["acceso"] = 2; // No tiene permisos de acceder al menu
                    Response.Redirect("WebFormInicio.aspx");

                }
                else if (Url == "WebFormPay.aspx" && cargo.TrimEnd() != "CAJERO")
                {
                    Session["acceso"] = 2; // No tiene permisos de acceder al menu
                    Response.Redirect("WebFormInicio.aspx");
                }
                else
                    Session["acceso"] = 1;

            }
            else
                Response.Redirect("WebFormLogin.aspx");

        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            // Limpiar Sesión
            userNegocio.ActualizarSesionUsuario(int.Parse(Session["IdUsuario"].ToString()), "INACTIVA");
            Session["Login"] = null;
            Session["RolUsuario"] = null;
            Session["IdUsuario"] = null;
            Response.Redirect("WebFormLogin.aspx");

        }
    }
}