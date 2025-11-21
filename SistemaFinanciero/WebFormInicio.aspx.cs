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
        string alert = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["acceso"] != null)
            {
                if (int.Parse(Session["acceso"].ToString()) == 2)
                {
                    alert = @"swal('Aviso!', 'No tiene permisos para acceder a esta opción', 'error');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                }
            }
        }

    }
}