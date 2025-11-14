using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;

namespace SistemaFinanciero
{
    public partial class WebFormInicio : System.Web.UI.Page
    {
        UsuarioNegocio UsuarioNeg = null;
        Utilitario utilitario = null;
        string alert = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            //if (Request.Cookies["ycps"].Value != null)
            //{
            //    string bloqueado = Request.QueryString["Error"];

            //    if (bloqueado == "rqa9")
            //    {
            //        alert = @"swal('Aviso!', 'NO TIENE PERMISO PARA EJECUTAR LA APLICACIÓN', 'error');";
            //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            //    }

            //    if (!IsPostBack)
            //    {
            //        UsuarioNeg = new UsuarioNegocio();

            //        utilitario = new Utilitario();
            //        string IdUsuario = utilitario.Desencriptar(Request.Cookies["ycps"].Value);

            //        if (UsuarioNeg.AnalistaCobranza(IdUsuario) == true)
            //        {

            //            panellista.Visible = true;
            //        }
            //        else
            //        {
            //            panellista.Visible = false;
            //        }
            //    }
            //}
            //else
            //{
            //    Response.Redirect("WebFormLogin.aspx");
            //}


        }

    }
}