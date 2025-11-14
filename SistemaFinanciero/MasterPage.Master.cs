using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

namespace SistemaFinanciero
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        UsuarioNegocio UserNeg = null;
        Utilitario utilitario = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            UserNeg = new UsuarioNegocio();
            bool Logueado = false;
            utilitario = new Utilitario();
            //string IdUsuario = utilitario.Desencriptar(Request.Cookies["ycps"].Value);

            //Logueado = UserNeg.ConsultarRegistroSistema(IdUsuario);
            //if (Logueado == false)
            //{
            //    Response.Cookies["ycps"].Expires = DateTime.Now.AddDays(-1);
            //    Response.Cookies["raqa"].Expires = DateTime.Now.AddDays(-1);
            //    Response.Redirect("WebFormLogin.aspx");
            //}
            //else
            //{
            //    lblNombreUsuario.Text = "Bienvenido " + (string)utilitario.Desencriptar(Request.Cookies["raqa"].Value);
            //}
            lblNombreUsuario.Text = "Bienvenido ";
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            //UsuarioNegocio UserNeg = new UsuarioNegocio();
            //utilitario = new Utilitario();
            //string IdUsuario = utilitario.Desencriptar(Request.Cookies["ycps"].Value);
            //UserNeg.EliminarRegistroSistema(IdUsuario);

          

            // alternativa corta:
            //string ip2 = Request.UserHostAddress;

            //Negocio.LogSistema log = new LogSistema();
            //log.GuardarLogSistema("Login " + "Cierra session del sistema con IP " + ip2, IdUsuario);

            Response.Cookies["ycps"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["raqa"].Expires = DateTime.Now.AddDays(-1);

            Response.Redirect("WebFormLogin.aspx");


        }
    }
}