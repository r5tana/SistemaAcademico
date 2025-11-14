using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;
using System.Globalization;

namespace SistemaFinanciero
{
    public partial class WebFormSpend : System.Web.UI.Page
    {
        string alert = "";
        UsuarioNegocio usuarioNeg = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdUsuario"] != null)
            {

                usuarioNeg = new UsuarioNegocio();
                usuarioNeg.ConsultarPermiso();
                usuarioNeg.Dispose();

                if (!IsPostBack)
                {


                }

            }
            else
            {
                Response.Redirect("WebFormLogin.aspx");
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (txtFechaInicio.Text == "" || TxtDescripcionGasto.Text == "" || TxtMontoGasto.Text == "")
            {
                alert = @"swal('Aviso!', 'Favor seleccionar la fecha, descripción del gasto y el monto del gasto', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }
            else
            {
                Gastos gasto = new Gastos();

                DateTime FechaGasto = DateTime.ParseExact(txtFechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                gasto.Fecha = FechaGasto;
                gasto.DescripcionGasto = TxtDescripcionGasto.Text.Trim().ToUpper();
                gasto.Monto = Convert.ToDecimal(TxtMontoGasto.Text);
                gasto.Usuario = Session["IdUsuario"].ToString();
                gasto.Estado = 1;

                GastoNegocio GastoNeg = new GastoNegocio();
                GastoNeg.GuardarGastos(gasto);

                txtFechaInicio.Text = "";
                TxtDescripcionGasto.Text = "";
                TxtMontoGasto.Text = "";

                alert = @"swal('Aviso!', 'Se ha agregado el gasto', 'success');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

            }
        }
    }
}