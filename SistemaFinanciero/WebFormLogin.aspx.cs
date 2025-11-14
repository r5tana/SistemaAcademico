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
    public partial class WebFormLogin : System.Web.UI.Page
    {
        UsuarioNegocio UserNeg = null;
        Seguridad_Usuario usuario = null;
        Trabajador trabajador = null;
        RegistroSistema registrosis = null;
        Utilitario utilitario = null;   

        string alert = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAcceder_Click(object sender, EventArgs e)
        {
            Captcha1.ValidateCaptcha(txtCaptcha.Text.Trim());

            if (Captcha1.UserValidated)
            {
                Response.Redirect("WebFormInicio.aspx");
                //UserNeg = new UsuarioNegocio();
                //usuario = new Seguridad_Usuario();
                //trabajador = new Trabajador();

                //bool Logueado = false;

                //usuario = UserNeg.ConsultarUsuario(txtLogin.Text.Trim().ToUpper(), txtPassword.Text.Trim());

                //if (usuario != null)
                //{

                //    Logueado = UserNeg.ConsultarRegistroSistema(txtLogin.Text.Trim().ToUpper());

                //    if (Logueado == false)
                //    {
                //                    HttpCookie cookieUser = new HttpCookie("ycps");
                //                    HttpCookie cookieName = new HttpCookie("raqa");

                //                    utilitario = new Utilitario();
                //                    cookieUser.Value = utilitario.Encriptar(trabajador.CedulaTrabajador);
                //                    cookieName.Value = utilitario.Encriptar(trabajador.NombreTrabajador);

                //                    cookieUser.Expires = DateTime.Now.AddDays(1);
                //                    cookieName.Expires = DateTime.Now.AddDays(1);

                //                    Response.Cookies.Add(cookieUser);
                //                    Response.Cookies.Add(cookieName);


                //                    registrosis = new RegistroSistema();
                //                    registrosis.IdUsuarioLogin = txtLogin.Text.Trim().ToUpper();
                //                    registrosis.Ingreso = DateTime.Now;
                //                    UserNeg.AgregarRegistroSistema(registrosis);


                //                        UserNeg.Dispose();
                //                        Response.Redirect("WebFormInicio.aspx");

                //    }
                //    else
                //    {

                //        alert = @"swal('Aviso!', 'Usuario ya accedió al sistema', 'error');";
                //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                //    }

            //}
            //    else
            //    {
                    
            //        txtCaptcha.Text = "";
            //        alert = @"swal('Aviso!', 'Usuario o Contraseña Incorrecta', 'error');";
            //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            //    }

              
            }
            else
            {
              
                txtCaptcha.Text = "";
                alert = @"swal('Aviso!', 'Favor escribe el codigo de la imagen', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }
        }

        protected void btnDesbloquear_Click(object sender, EventArgs e)
        {
            UserNeg = new UsuarioNegocio();
            usuario = new Seguridad_Usuario();

            usuario = UserNeg.ConsultarUsuarioBloqueado(txtLogin.Text.Trim().ToUpper());

            if (usuario != null)
            {
                //string ip2 = Request.UserHostAddress;
                //Negocio.LogSistema log = new LogSistema();
                //log.GuardarLogSistema("Login " + "Desbloquea el usuario " + txtLogin.Text.Trim() + " Con IP " + ip2, txtLogin.Text.Trim());

                UserNeg.EliminarRegistroSistema(usuario.Usuario);
                UserNeg.Dispose();
                alert = @"swal('Aviso!', 'Se ha desbloqueado el usuario correctamente', 'success');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }
            else
            {
                //string ip2 = Request.UserHostAddress;
                //Negocio.LogSistema log = new LogSistema();
                //log.GuardarLogSistema("Login " + "Intenta Desbloquear usuario que no existe " + txtLogin.Text.Trim() + " Con IP " + ip2, txtLogin.Text.Trim());

                UserNeg.Dispose();
                alert = @"swal('Aviso!', 'Usuario no existe', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }

        }
    }
}