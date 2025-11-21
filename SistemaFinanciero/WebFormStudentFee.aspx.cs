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

        }


        public void CargarTransacciones()
        {
            List<tmetransacciones> lista = new List<tmetransacciones>();
            lista = transaccionNegocio.ListaTransaccionesPorEstudiante(txtEstudiante.Text.TrimEnd().ToUpper());
            Session["listaTransacciones"] = lista;

            if (lista.Count > 0)
            {
                pnlResultado.Visible = true;
                this.GridAranceles.DataSource = lista;
                GridAranceles.DataBind();
                GridAranceles.UseAccessibleHeader = true;
                GridAranceles.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            else
            {
                LimpiarCampos();
                alert = @"swal('Aviso!', 'Código del estudiante no registro de transacciones', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }

        }

        public void LimpiarCampos()
        {
            pnlResultado.Visible = false;
            txtNombres.Text = string.Empty;
            txtEstado.Text = string.Empty;
            GridAranceles.DataSource = null;
            GridAranceles.DataBind();
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

        protected void bntBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEstudiante.Text.TrimEnd()))
            {
                var detalleEstudiante = estudiante.ConsultarEstudiante(txtEstudiante.Text.TrimEnd().ToUpper());
                if (detalleEstudiante != null)
                {
                    txtNombres.Text = detalleEstudiante.nombres.TrimEnd().ToUpper() + " " + detalleEstudiante.apellidos.TrimEnd().ToUpper();
                    txtEstado.Text = detalleEstudiante.estado.TrimEnd().ToUpper();

                    CargarTransacciones();
                }
                else
                {
                    alert = @"swal('Aviso!', 'Código del estudiante no se encuentra registrado', 'error');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                }
            }
            else
            {
                alert = @"swal('Aviso!', 'Debe digitar el código el estudiante', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }
        }
    }
}