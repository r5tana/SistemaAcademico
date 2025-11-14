using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;
using System.Globalization;

namespace SistemaFinanciero
{
    public partial class WebFormListSpend : System.Web.UI.Page
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

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (txtFechaInicio.Text != "" || txtFechaFin.Text != "")
            {
                GridGastos.Visible = false;
                GridGastos.DataSource = "";
                GridGastos.DataBind();

                DateTime FechaInicial = DateTime.ParseExact(txtFechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime FechaFinal = DateTime.ParseExact(txtFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                List<Gastos> Lista = new List<Gastos>();
                GastoNegocio GastoNeg = new GastoNegocio();
                Lista = GastoNeg.ListaGastos(FechaInicial,FechaFinal);

                Session["lista"] = Lista;
                LoadTablesolicitud();
               

            }
            else
            {
                GridGastos.Visible = false;
                GridGastos.DataSource = "";
                GridGastos.DataBind();

                alert = @"swal('Aviso!', 'Favor seleccionar el rango de fechas', 'info');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }
        }

        private void LoadTablesolicitud()
        {

            GridGastos.DataSource = "";
            GridGastos.DataBind();
            List<Gastos> Solicitudes = (List<Gastos>)Session["lista"];

            GridGastos.DataSource = Solicitudes;
            GridGastos.DataBind();


            if (Solicitudes.Count > 0)
            {
                GridGastos.Visible = true;
                GridGastos.UseAccessibleHeader = true;
                GridGastos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            else
            {
                GridGastos.Visible = false;
                alert = @"swal('Aviso!', 'No hay información de gastos, en esa rango de fecha', 'info');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }
            GridGastos.GridLines = GridLines.None;
        }

        protected void GridGastos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {


            }
        }

        protected void GridGastos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}