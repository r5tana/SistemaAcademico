using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Entidades;
using Datos;

namespace Negocio
{
    public class UsuarioNegocio : IDisposable
    {
        string alert = "";

        public void CrearUsuario(Seguridad_Usuario usuario)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            UserDatos.CrearUsuario(usuario);
        }
        public Seguridad_Usuario ConsultarUsuario(string user, string pass)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            return UserDatos.ConsultarUsuario(user, pass);

        }

        public Seguridad_Usuario ConsultarUsuarioBloqueado(string user)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            return UserDatos.ConsultarUsuarioBloqueado(user);

        }


        public void ConsultarPermiso(string CedulaUsuario)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            List<Seguridad_Usuario_Rol> RolesUsuario = new List<Seguridad_Usuario_Rol>();
            int TienePermiso = 0;
            string IdUsuario = CedulaUsuario;//HttpContext.Current.Session["IdUsuario"].ToString();
            

            string Url = HttpContext.Current.Request.Url.AbsoluteUri.Substring(HttpContext.Current.Request.Url.AbsoluteUri.LastIndexOf('/') + 1);
            int IdAplicacion = UserDatos.ConsultarAplicacion(Url);

            RolesUsuario = UserDatos.UsuarioRoles(IdUsuario);

            foreach (var rol in RolesUsuario)
            {
                var Permiso = UserDatos.ConsultarPermiso(IdAplicacion, Convert.ToInt32(rol.IdRol));

                if (Permiso == 1)
                {
                    TienePermiso = 1;
                }
            }

            if (TienePermiso != 1)
            {
                HttpContext.Current.Response.Redirect("WebFormInicio.aspx?Error=rqa9");
            }


        }

        public List<Seguridad_Rol> ListasRoles()
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            return UserDatos.ListasRoles();
        }

        public List<Seguridad_Aplicacion> ListaAplicaciones()
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            return UserDatos.ListarAplicaciones();
        }

        public List<int?> UsuarioRoles(string IdUsuario)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            return UserDatos.ListaRolesUsuario(IdUsuario);

        }

        public List<int> ListaRolesApliaciones(int IdRol)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            return UserDatos.ListaRolesApliaciones(IdRol);
        }


        public void AgregarEliminarRol(string IdTrabajador, int IdRol, int TipoProceso)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            UserDatos.AgregarEliminarRol(IdTrabajador, IdRol, TipoProceso);

        }

        public void CambioContrasena(string Usuario, string NuevaContrasena)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            UserDatos.CambioContrasena(Usuario, NuevaContrasena);
        }

        public bool Administrador(string IdUsuario)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            return UserDatos.Administrador(IdUsuario);
        }

        public bool ValidarCargoSupervisor(string IdUsuario)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            return UserDatos.ValidarCargoSupervisor(IdUsuario);
        }
        public bool AnalistaCredito(string IdUsuario)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            return UserDatos.AnalistaCredito(IdUsuario);
        }

        public bool AnalistaCobranza(string IdUsuario)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            return UserDatos.AnalistaCobranza(IdUsuario);
        }

        public void AgregarRegistroSistema(RegistroSistema registro)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            UserDatos.AgregarRegistroSistema(registro);
        }

        public void EliminarRegistroSistema(string IdUsuario)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            UserDatos.EliminarRegistroSistema(IdUsuario);
        }

        public bool ConsultarRegistroSistema(string IdUsuario)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            return UserDatos.ConsultarRegistroSistema(IdUsuario);
        }

        public List<Seguridad_Aplicacion> ListarAplicaciones()
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            return UserDatos.ListarAplicaciones();
        }

        public int AgregarPermisosRolAplicacion(Seguridad_PermisoAplicacion aplicacion_rol)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            return UserDatos.AgregarPermisosRolAplicacion(aplicacion_rol);
        }

        public int EliminarPermisosRolAplicacion(int IdApliacion, int IdRol)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            return UserDatos.EliminarPermisosRolAplicacion(IdApliacion, IdRol);
        }

        public void Activar_InactivarUsuario(string user)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            UserDatos.Activar_InactivarUsuario(user);
        }

        /// Encripta una cadena
        public string Encriptar(string pass)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(pass);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        /// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
        public static string DesEncriptar(string Pass)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(Pass);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }

        public void ResetPassword(string IdUsuario)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            UserDatos.ResetPassword(IdUsuario);

        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(dc);
            GC.SuppressFinalize(this);

        }
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                // Note disposing has been done.
                //if (disposing)
                //{
                //    // Dispose managed resources.
                //    dc.Dispose();
                //}

                disposed = true;

            }
        }

    }
}
