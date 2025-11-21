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
    public partial class WebFormUpdateWorker : System.Web.UI.Page
    {
        TrabajadorNegocio TrabNego = null;
        List<DetalleTrabajadorDto> ListaTrabajadores = null;
        UsuarioNegocio usuarioNeg = null;
        Utilitario utilitario = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request.Cookies["ycps"].Value != null)
                //{
                    //utilitario = new Utilitario();
                    //string IdUsuario = utilitario.Desencriptar(Request.Cookies["ycps"].Value);
                    //usuarioNeg = new UsuarioNegocio();
                    //usuarioNeg.ConsultarPermiso(IdUsuario);
                    //usuarioNeg.Dispose();

                    TrabNego = new TrabajadorNegocio();
                    ListaTrabajadores = new List<DetalleTrabajadorDto>();
                    ListaTrabajadores = TrabNego.ListaTrabajadores();
                    Session["lista"] = ListaTrabajadores;
                    panellista.Visible = true;
                    LoadTablesolicitud();
                //}
                //else
                //{
                //    Response.Redirect("WebFormLogin.aspx");
                //}
            }
        }

        private void LoadTablesolicitud()
        {

            GridDatasolicitud.DataSource = "";
            GridDatasolicitud.DataBind();
            List<Trabajador> Solicitudes = (List<Trabajador>)Session["lista"];

            GridDatasolicitud.DataSource = Solicitudes;
            GridDatasolicitud.DataBind();


            if (Solicitudes.Count > 0)
            {

                GridDatasolicitud.UseAccessibleHeader = true;
                GridDatasolicitud.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            else
            {

            }
            GridDatasolicitud.GridLines = GridLines.None;
        }

        protected void GridDatasolicitud_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DetalleSolicitud")
            {

                Session["ModTrabajador"] = e.CommandArgument.ToString();
                Response.Redirect("WebFormAddWorker.aspx");
            }

            if (e.CommandName == "ResetPassword")
            {
                string IdTrabajador = e.CommandArgument.ToString();

                usuarioNeg = new UsuarioNegocio();
                //usuarioNeg.ResetPassword(IdTrabajador);
                
            }

            if (e.CommandName == "InactivarUsuario")
            {
                string IdTrabajador = e.CommandArgument.ToString();

                usuarioNeg = new UsuarioNegocio();
                //usuarioNeg.Activar_InactivarUsuario(IdTrabajador);

                TrabNego = new TrabajadorNegocio();
                TrabNego.Activar_InactivarTrabajador(IdTrabajador);

                Response.Redirect("WebFormUpdateWorker.aspx");
            }

            

        }

        protected void GridDatasolicitud_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                // e.Row.Cells[3].Text = "Estado";

                var EstadoTrabajador = Convert.ToInt32(e.Row.Cells[7].Text);

                if (EstadoTrabajador == 1)
                {
                    e.Row.Cells[7].Text = "Activo";
                }
                else if (EstadoTrabajador == 2)
                {
                    e.Row.Cells[7].Text = "Inactivo";
                }
            }




        }
        protected void GridDatasolicitud_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}