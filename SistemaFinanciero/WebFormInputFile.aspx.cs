using Entidades;
using Negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaFinanciero
{
    public partial class WebFormInputFile : System.Web.UI.Page
    {
        TransaccionNegocio negocio = new TransaccionNegocio();
        string alert = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }

        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (FileUpload2.FileName != "")
            {
                if (FileUpload2.HasFile)
                {
                    string carpeta = Server.MapPath("~/Soporte/");

                    // Si no existe la carpeta, crearla
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    FileUpload2.SaveAs(Server.MapPath("~/Soporte/") + Path.GetFileName(FileUpload2.FileName));
                    string filePath = Server.MapPath("~/Soporte/") + FileUpload2.FileName;
                    string[] allowdFile = { ".txt" };
                    string FileExt = System.IO.Path.GetExtension(FileUpload2.PostedFile.FileName);
                    bool isValidFile = allowdFile.Contains(FileExt);
                    if (isValidFile)
                    {
                        var listaDatos = LeerArchivo(filePath);

                        if (listaDatos.Count > 0)
                        {
                            btnGuardar.Visible = true;
                            btnCancelar.Visible = true;
                            pnlCargaArchivo.Visible = false;
                            GridDetalle.Visible = true;
                            GridDetalle.DataSource = listaDatos;
                            GridDetalle.DataBind();
                            GridDetalle.UseAccessibleHeader = true;
                            GridDetalle.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                        else
                        {
                            alert = @"swal('Aviso!', 'El archivo no contiente datos', 'error');";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                        }

                        File.Delete(Server.MapPath("~/Soporte/") + Path.GetFileName(FileUpload2.FileName));

                    }
                    else
                    {
                        alert = @"swal('Aviso!', 'Solo se permite archivos con formatos txt', 'error');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                    }
                }
                else
                {
                    alert = @"swal('Aviso!', 'El archivo no contiente datos', 'error');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                }
            }
            else
            {
                alert = @"swal('Aviso!', 'Debe seleccionar el archivo a cargar', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }
        }



        public List<tmeinfobancos> LeerArchivo(string rutaArchivo)
        {
            var lista = new List<tmeinfobancos>();
            var lineas = File.ReadAllLines(rutaArchivo);
            DateTime fechaProceso = Convert.ToDateTime(txtFecha.Text);

            foreach (var item in lineas)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;

                var registro = new tmeinfobancos
                {
                    NoRef = item.Substring(0, 20).Trim(),
                    CodAlum = item.Substring(20, 6).Trim(),
                    Tipo = item.Substring(26, 2).Trim(),
                    Descripcion = item.Substring(28, 30).Trim(),
                    Numero = item.Substring(58, 5).Trim(),
                    Monto = double.Parse(item.Substring(63, 8).Trim(),
                              System.Globalization.CultureInfo.InvariantCulture),
                    //System.Globalization.CultureInfo.GetCultureInfo("es-NI"))
                    Fecha = fechaProceso.Date,
                    TipoBanco = "",
                    Duplicado = false,
                    Procesado = true

                };

                lista.Add(registro);

            }

            Session["ListaBanco"] = lista;

            return lista;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            List<tmeinfobancos> lista = (List<tmeinfobancos>)Session["ListaBanco"];

            negocio.InsertarInformacionBancoLista(lista);

            foreach (var item in lista)
            {
                negocio.ActualizarTrasaccionBanco(item.NoRef, item.CodAlum, item.Numero, item.Tipo, DateTime.Now.Year);
            }

            LimpiarCampos();

            alert = @"swal('Aviso!', 'Se registro la información con éxtio', 'success');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        public void LimpiarCampos()
        {
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
            btnGuardar.Visible = false;
            btnCancelar.Visible = false;
            pnlCargaArchivo.Visible = true;
            GridDetalle.Visible = false;
            GridDetalle.DataSource = null;
            GridDetalle.DataBind();
        }
    }
}