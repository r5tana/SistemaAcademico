using Entidades;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaFinanciero
{
    public partial class WebFormStudentFee : System.Web.UI.Page
    {
        private TransaccionNegocio transaccionNegocio = new TransaccionNegocio();
        private EstudianteNegocio estudiante = new EstudianteNegocio();
        string alert = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEstudiantes();
            }
        }

        public void LimpiarCampos()
        {
            pnlResultado.Visible = false;
            txtNombres.Text = string.Empty;
            txtEstado.Text = string.Empty;
            GridAranceles.DataSource = null;
            GridAranceles.DataBind();
            GridEstudiantes.DataSource = null;
            GridEstudiantes.DataBind();
        }

        protected void GridAranceles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                var estado = int.Parse(e.Row.Cells[4].Text);

                if (estado == 1)
                {
                    e.Row.Cells[4].Text = "CANCELADO";
                }
                else
                {
                    e.Row.Cells[4].Text = "PENDIENTE";
                }
            }
        }

        public void CargarEstudiantes()
        {
            LimpiarCampos();
            List<tmaestudiante> lista = new List<tmaestudiante>();
            lista = estudiante.ListaEstudiante();
            Session["listaEstudiantes"] = lista;

            if (lista.Count > 0)
            {
                pnlResultado.Visible = false;
                this.GridEstudiantes.DataSource = lista;
                GridEstudiantes.DataBind();
                GridEstudiantes.UseAccessibleHeader = true;
                GridEstudiantes.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            else
            {
                
                alert = @"swal('Aviso!', 'No hay registros de estudiante', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }

        }


        protected void GridEstudiantes_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "BuscarArancel")
            {
                string codigoEstudiante = e.CommandArgument.ToString();

                List<tmetransacciones> lista = new List<tmetransacciones>();
                lista = transaccionNegocio.ListaTransaccionesPorEstudiante(codigoEstudiante.TrimEnd().ToUpper());
                Session["listaTransacciones"] = lista;

                if (lista.Count > 0)
                {
                    var detalleEstudiante = estudiante.ConsultarEstudiante(codigoEstudiante.TrimEnd().ToUpper());

                    txtEstudiante.Text = codigoEstudiante;
                    txtNombres.Text = detalleEstudiante.nombres.TrimEnd().ToUpper() + " " + detalleEstudiante.apellidos.TrimEnd().ToUpper();
                    txtEstado.Text = detalleEstudiante.estado.TrimEnd().ToUpper();


                    GridEstudiantes.Visible = false;
                    pnlResultado.Visible = true;
                    this.GridAranceles.DataSource = lista;
                    GridAranceles.DataBind();
                    GridAranceles.UseAccessibleHeader = true;
                    GridAranceles.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    pnlResultado.Visible = false;
                    GridEstudiantes.Visible = true;
                    GridAranceles.UseAccessibleHeader = true;
                    GridAranceles.HeaderRow.TableSection = TableRowSection.TableHeader;

                    LimpiarCampos();
                    alert = @"swal('Aviso!', 'Código del estudiante no registro de transacciones', 'error');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                }

            }
        }

        protected void bntRetornar_Click(object sender, EventArgs e)
        {
            GridEstudiantes.Visible = true;
            CargarEstudiantes();
        }
    }
}