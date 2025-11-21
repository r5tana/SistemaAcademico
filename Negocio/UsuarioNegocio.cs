using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Negocio
{
    public class UsuarioNegocio : IDisposable
    {

        #region "BD SIGA"

        public void CrearUsuario(tmxusuarios usuario)
        {
            UsuarioDatos datosUsuario = new UsuarioDatos();
            datosUsuario.CrearUsuario(usuario);
        }

        public tmxusuarios ConsultarUsuarioClave(string login, string pass)
        {
            UsuarioDatos datosUsuario = new UsuarioDatos();
            return datosUsuario.ConsultarUsuarioClave(login, pass);
        }
        public string NombreUsuario(string login)
        {

            UsuarioDatos datosUsuario = new UsuarioDatos();
            return datosUsuario.NombreUsuario(login);
        }

        public List<tmxusuarios> ListaUsuarios()
        {
            UsuarioDatos datosUsuario = new UsuarioDatos();
            return datosUsuario.ListaUsuarios();
        }

        public void Activar_InactivarUsuario(int idUsuario)
        {
            UsuarioDatos datosUsuario = new UsuarioDatos();
            datosUsuario.Activar_InactivarUsuario(idUsuario);
        }


        public tmxusuarios ConsultarUsuario(string login)
        {
            UsuarioDatos datosUsuario = new UsuarioDatos();
            return datosUsuario.ConsultarUsuario(login);

        }
        public tmxusuarios ConsultarUsuarioId(int idUsuario)
        {
            UsuarioDatos datosUsuario = new UsuarioDatos();
            return datosUsuario.ConsultarUsuarioId(idUsuario);

        }

        public void ActualizarUsuario(tmxusuarios usuario)
        {
            UsuarioDatos datosUsuario = new UsuarioDatos();
            datosUsuario.ActualizarUsuario(usuario);
        }

        public void ResetPassword(int idUsuario, string password)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            UserDatos.ResetPassword(idUsuario, password);

        }

        public void ActualizarSesionUsuario(int idUsuario, string sesion)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            UserDatos.ActualizarSesionUsuario(idUsuario, sesion);

        }

        public void CambioClaveUsuario(int idUsuario, string nuevaClave)
        {
            UsuarioDatos UserDatos = new UsuarioDatos();
            UserDatos.CambioClaveUsuario(idUsuario, nuevaClave);
        }


        #endregion

        #region "Métodos para desencriptar y encriptar contraseña"

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


        #endregion


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
