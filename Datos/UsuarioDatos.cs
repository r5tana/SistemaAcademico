using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Datos
{
    public class UsuarioDatos
    {
      
        SistemaFacturacionEntities modeloFacturacion = null;
        tmxusuarios usuario = null;

        #region "BD SIGA"

        public void CrearUsuario(tmxusuarios usuario)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                modeloFacturacion.tmxusuarios.Add(usuario);
                modeloFacturacion.SaveChanges();
            }
            catch (Exception err)
            {

                throw new Exception("Error al guardar usuario del trabajador " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public tmxusuarios ConsultarUsuarioClave(string login, string pass)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                usuario = new tmxusuarios();

                return usuario = (from x in modeloFacturacion.tmxusuarios where x.usuario.TrimEnd() == login && x.PasswordWeb == pass && (x.estado.TrimEnd() == "ACTIVO" || x.estado.TrimEnd() == "ACTIVA") select x).FirstOrDefault();
            }
            catch (Exception err)
            {

                throw new Exception("Error al consultar usuario " + login + " " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public string NombreUsuario(string login)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                usuario = new tmxusuarios();
                var detalle = modeloFacturacion.tmxusuarios.Where(x => x.usuario.TrimEnd() == login).FirstOrDefault();
                if (detalle != null)
                {
                    return detalle.nombres + " " + detalle.apellidos;
                }
                else
                    return "";
            }
            catch (Exception error)
            {
                throw new Exception("Error al consultar el nombre del usuario" + error);
            }
        }

        public List<tmxusuarios> ListaUsuarios()
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                List<tmxusuarios> listaUsuarios = new List<tmxusuarios>();

                return listaUsuarios = (from x in modeloFacturacion.tmxusuarios select x).ToList();

            }
            catch (Exception error)
            {

                throw new Exception("Ocurrio un error al momento de cargar los usuarios " + error);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public void Activar_InactivarUsuario(int idUsuario)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                usuario = new tmxusuarios();

                usuario = (from x in modeloFacturacion.tmxusuarios where x.id_usuario == idUsuario select x).FirstOrDefault();

                if (usuario != null)
                {
                    if (usuario.estado.TrimEnd() == "ACTIVO" || usuario.estado.TrimEnd() == "ACTIVA")
                    {
                        usuario.estado = "INACTIVO";
                    }
                    else
                    {
                        usuario.estado = "ACTIVO";
                    }
                    modeloFacturacion.SaveChanges();
                }
            }
            catch (Exception error)
            {

                throw new Exception("Error al activar o inactivar el usuario " + error);
            }
        }

        public tmxusuarios ConsultarUsuario(string login)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                usuario = new tmxusuarios();

                return usuario = (from x in modeloFacturacion.tmxusuarios where x.usuario.TrimEnd() == login select x).FirstOrDefault();
            }
            catch (Exception err)
            {

                throw new Exception("Error al consultar trabajador " + login + " " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public tmxusuarios ConsultarUsuarioId(int idUsuario)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                usuario = new tmxusuarios();

                return usuario = (from x in modeloFacturacion.tmxusuarios where x.id_usuario == idUsuario & (x.estado == "ACTIVO" || x.estado == "ACTIVA") select x).FirstOrDefault();
            }
            catch (Exception err)
            {

                throw new Exception("Error al consultar usuario id " + idUsuario + " " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public void ActualizarUsuario(tmxusuarios usuarioModificado)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                usuario = new tmxusuarios();

                usuario = (from x in modeloFacturacion.tmxusuarios where x.id_usuario == usuarioModificado.id_usuario select x).FirstOrDefault();

                if (usuario != null)
                {
                    usuario.nombres = usuarioModificado.nombres;
                    usuario.apellidos = usuarioModificado.apellidos;
                    usuario.cargo = usuarioModificado.cargo;
                    modeloFacturacion.SaveChanges();
                }
            }
            catch (Exception error)
            {

                throw new Exception("Error al activar o inactivar el usuario " + error);
            }
        }
        
        public void ResetPassword(int idUsuario, string password)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();

                usuario = (from x in modeloFacturacion.tmxusuarios where x.id_usuario == idUsuario select x).FirstOrDefault();

                if (usuario != null)
                {
                    usuario.PasswordWeb = password;
                    modeloFacturacion.SaveChanges();
                }

            }
            catch (Exception error)
            {

                throw new Exception("Error al reestablecer contraseña del usuario " + error);
            }

        }

        public void ActualizarSesionUsuario(int idUsuario, string sesion)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                usuario = new tmxusuarios();

                usuario = (from x in modeloFacturacion.tmxusuarios where x.id_usuario == idUsuario select x).FirstOrDefault();

                if (usuario != null)
                {
                    usuario.sesion = sesion;
                    modeloFacturacion.SaveChanges();
                }
            }
            catch (Exception error)
            {

                throw new Exception("Error al actualizar la sesión de usuario " + error);
            }
        }

        public void CambioClaveUsuario(int idUsuario, string nuevaClave)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                usuario = new tmxusuarios();

                usuario = (from x in modeloFacturacion.tmxusuarios where x.id_usuario == idUsuario select x).FirstOrDefault();

                if (usuario != null)
                {
                    usuario.PasswordWeb = nuevaClave;
                    modeloFacturacion.SaveChanges();
                }

            }
            catch (Exception error)
            {

                throw new Exception("Error al cambiar la contraseña del usuario " + error);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        #endregion
    }
}
