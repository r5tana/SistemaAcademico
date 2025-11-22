using Entidades;
using Microsoft.Ajax.Utilities;
using Negocio;
using SistemaFinanciero.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaFinanciero
{
    public partial class WebFormLogin : System.Web.UI.Page
    {
        UsuarioNegocio userNegocio = null;
        tmxusuarios usuario = null;
        string alert = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                // Hubo un cambio de contraseña temporal
                if (Session["bandera"] != null) 
                {
                    Session["bandera"] = null;

                    alert = @"swal('Aviso!', 'Se realizó cambio de contraseña, favor vuelva a iniciar sesión', 'success');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);

                }
            }
        }

        protected void btnAcceder_Click(object sender, EventArgs e)
        {
            Session["Login"] = "rquintana";
            Session["IdUsuario"] = "001";
            Session["RolUsuario"] = "Cajero";

            Response.Redirect("WebFormInicio.aspx");
            /*
                       Captcha1.ValidateCaptcha(txtCaptcha.Text.Trim());

                       if (Captcha1.UserValidated)
                       {

                           userNegocio = new UsuarioNegocio();
                           usuario = new tmxusuarios();
                           string login = txtLogin.Text.Trim().ToUpper();

                           var clave = userNegocio.Encriptar(txtPassword.Text.TrimEnd());

                           usuario = userNegocio.ConsultarUsuarioClave(login, clave);

                           if (usuario != null)
                           {
                               if (usuario.cargo.TrimEnd() != "ADMINISTRADOR" & usuario.cargo.TrimEnd() != "CAJERO" & usuario.cargo.TrimEnd() != "CONTADORA")
                               {
                                   alert = @"swal('Aviso!', 'El usuario no tiene permiso para acceder el sistema', 'error');";
                                   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                               }
                               else
                               {
                                   if (usuario.sesion.TrimEnd() == "INACTIVO" || usuario.sesion.TrimEnd() == "INACTIVA")
                                   {
                                       if (txtPassword.Text.TrimEnd() == "2025")
                                       {
                                           Session["IdTemporal"] = usuario.id_usuario;
                                           Response.Redirect("WebFormPassword.aspx");
                                       }
                                       else
                                       {

                                           Session["Login"] = login;
                                           Session["IdUsuario"] = usuario.id_usuario;
                                           Session["RolUsuario"] = usuario.cargo.TrimEnd();

                                           //Registrar Session
                                           userNegocio.ActualizarSesionUsuario(usuario.id_usuario, "ACTIVA");

                                           Response.Redirect("WebFormInicio.aspx");
                                       }

                                   }
                                   else
                                   {
                                       alert = @"swal('Aviso!', 'El usuario tiene una sesión activa', 'error');";
                                       ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                                   }
                               }

                           }
                           else
                           {
                               txtCaptcha.Text = "";
                               alert = @"swal('Aviso!', 'Usuario o Contraseña Incorrecta', 'error');";
                               ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                           }
                       }
                       else
                       {
                           txtCaptcha.Text = "";
                           alert = @"swal('Aviso!', 'Favor escribe el codigo de la imagen', 'error');";
                           ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                       }
            */
        }

        protected void btnDesbloquear_Click(object sender, EventArgs e)
        {
            userNegocio = new UsuarioNegocio();
            usuario = userNegocio.ConsultarUsuario(txtLogin.Text.TrimEnd().ToUpper());
            if (usuario != null)
            {
                if (usuario.cargo.TrimEnd() != "ADMINISTRADOR" & usuario.cargo.TrimEnd() != "CAJERO" & usuario.cargo.TrimEnd() != "CONTADORA")
                {
                    alert = @"swal('Aviso!', 'El usuario que intenta desbloquear no tiene permisos para el acceder al sistema', 'error');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                }
                else
                {
                    if (usuario.estado.TrimEnd() == "ACTIVO" | usuario.estado.TrimEnd() == "ACTIVA")
                    {
                        userNegocio.ActualizarSesionUsuario(usuario.id_usuario, "INACTIVA");

                        alert = @"swal('Aviso!', 'Se ha desbloqueado el usuario correctamente', 'success');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                    }
                    else
                    {
                        alert = @"swal('Aviso!', 'Usuario no existe', 'error');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
                    }
                }
            }
            else
            {
                alert = @"swal('Aviso!', 'Usuario no existe', 'error');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alerta", alert, true);
            }

        }
    }
}