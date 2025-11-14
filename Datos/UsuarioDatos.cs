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
        SistemaFinancieroEntities Modelo = null;
        Seguridad_Usuario usuario = null;
        Seguridad_Usuario_Rol userrol = null;
        
        public void CrearUsuario(Seguridad_Usuario usuario) {

            try
            {
                Modelo = new SistemaFinancieroEntities();
                Modelo.Seguridad_Usuario.Add(usuario);
                Modelo.SaveChanges();
            }
            catch (Exception err)
            {

                throw new Exception("Error al guardar usuario del trabajador " + err);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        }

        public Seguridad_Usuario ConsultarUsuario(string user, string pass) {
            
            try
            {
                Modelo = new SistemaFinancieroEntities();
                usuario = new Seguridad_Usuario();

               return usuario = (from x in Modelo.Seguridad_Usuario where x.Usuario == user  && x.Contraseña == pass && x.Estado == 1 select x).FirstOrDefault();
            }
            catch (Exception err)
            {
                
                throw new Exception("Error al consultar usuario " + user + " " + err);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        }

        public Seguridad_Usuario ConsultarUsuarioBloqueado(string user)
        {

            try
            {
                Modelo = new SistemaFinancieroEntities();
                usuario = new Seguridad_Usuario();

                return usuario = (from x in Modelo.Seguridad_Usuario where x.Usuario == user && x.Estado == 1 select x).FirstOrDefault();
            }
            catch (Exception err)
            {

                throw new Exception("Error al consultar usuario " + user + " " + err);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        }

        public List<Seguridad_Usuario_Rol> UsuarioRoles(string IdUsuario) {

            List<Seguridad_Usuario_Rol> ListaRolesUsuario = new List<Seguridad_Usuario_Rol>();
            try
            {
                Modelo = new SistemaFinancieroEntities();
                return ListaRolesUsuario = (from x in Modelo.Seguridad_Usuario_Rol where x.IdUsuario == IdUsuario select x).ToList();

            }
            catch (Exception error)
            {
                
                throw new Exception("Error al obtener los roles del usuario " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }

        }

        public List<int?> ListaRolesUsuario(string IdUsuario)
        {

            List<int?> ListaRolesUsuario = new List<int?>();
            try
            {
                Modelo = new SistemaFinancieroEntities();
                return ListaRolesUsuario = (from x in Modelo.Seguridad_Usuario_Rol where x.IdUsuario == IdUsuario select x.IdRol).ToList();

            }
            catch (Exception error)
            {

                throw new Exception("Error al obtener los roles del usuario " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }

        }

        public List<int> ListaRolesApliaciones(int IdRol) {
            List<int> ListaRolesAplicacion = new List<int>();
            try
            {
                Modelo = new SistemaFinancieroEntities();
                return ListaRolesAplicacion = (from x in Modelo.Seguridad_PermisoAplicacion where x.IdRol == IdRol select x.IdAplicacion).ToList();

            }
            catch (Exception error)
            {

                throw new Exception("Error al obtener los roles del usuario " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        }

        public int ConsultarAplicacion(string Url) {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                int IdAplicacion = 0;

                return IdAplicacion = (from x in Modelo.Seguridad_Aplicacion where x.Url == Url select x.IdAplicacion).FirstOrDefault();

            }
            catch (Exception error)
            {
                
                throw new Exception("Error al consultar la apliacion " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        }

        public int ConsultarPermiso(int IdAplicacion,int IdRol) {
            int Permiso = 0;
            try
            {
               Modelo = new SistemaFinancieroEntities();
               var TienePermiso =  (from x in Modelo.Seguridad_PermisoAplicacion where x.IdAplicacion == IdAplicacion && x.IdRol == IdRol select x).FirstOrDefault();

               if (TienePermiso != null)
               {
                   Permiso = 1;                   
               }

               return Permiso;

            }
            catch (Exception error)
            {

                throw new Exception("Error al obtener los roles del usuario " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        
        }


        public List<Seguridad_Rol> ListasRoles() {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                List<Seguridad_Rol> Roles = new List<Seguridad_Rol>();

                return Roles = (from x in Modelo.Seguridad_Rol select x).ToList();

            }
            catch (Exception error)
            {
                
                throw new Exception("Ocurrio un error al momento de cargar los roles " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        }

        public List<Seguridad_Aplicacion> ListaAplicaciones() {

            try
            {
                Modelo = new SistemaFinancieroEntities();
                List<Seguridad_Aplicacion> Aplicaciones = new List<Seguridad_Aplicacion>();

                return Aplicaciones = (from x in Modelo.Seguridad_Aplicacion select x).ToList();

            }
            catch (Exception error)
            {

                throw new Exception("Ocurrio un error al momento de cargar los roles " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        }


        public void AgregarEliminarRol(string IdTrabajador, int IdRol, int TipoProceso) {

            try
            {
                Modelo = new SistemaFinancieroEntities();
                userrol = new Seguridad_Usuario_Rol();
                Seguridad_Usuario_Rol NuevoRol = null;

                userrol = (from x in Modelo.Seguridad_Usuario_Rol where x.IdUsuario == IdTrabajador && x.IdRol == IdRol select x).FirstOrDefault();
                if (TipoProceso == 1)
                {
                    if (userrol == null)
                    {
                        NuevoRol = new Seguridad_Usuario_Rol();

                        NuevoRol.IdUsuario = IdTrabajador;
                        NuevoRol.IdRol = IdRol;
                        Modelo.Seguridad_Usuario_Rol.Add(NuevoRol);
                        Modelo.SaveChanges();

                    }      
                }
                else
                {
                    if (userrol != null)
                    {
                        Modelo.Seguridad_Usuario_Rol.Remove(userrol);
                        Modelo.SaveChanges();
                    }

                }
              
            }
            catch (Exception error)
            {
                
                throw new Exception("Error agregar rol al usuario");
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        }


        public void CambioContrasena(string Usuario, string NuevaContrasena)
        {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                usuario = new Seguridad_Usuario();

                usuario = (from x in Modelo.Seguridad_Usuario where x.Usuario == Usuario select x).FirstOrDefault();
                
                if (usuario != null)
                {
                    usuario.Contraseña = NuevaContrasena;
                    Modelo.SaveChanges();
                }

            }
            catch (Exception error)
            {

                throw new Exception("Error al cambiar la contraseña del usuario " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        }


        public bool Administrador(string IdUsuario) {
            bool Retorno = false;

            try
            {
                Modelo = new SistemaFinancieroEntities();

                var Permiso = (from x in Modelo.Seguridad_Usuario_Rol where x.IdUsuario == IdUsuario && (x.IdRol == 1 || x.IdRol == 2) select x).FirstOrDefault();

                if (Permiso != null)
                {
                    Retorno = true;
                }

                return Retorno;

            }
            catch (Exception error)
            {
                
                throw new Exception("Error al consultar si el usuario es administrador " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        
        }

        public bool ValidarCargoSupervisor(string IdUsuario)
        {
            bool Retorno = false;

            try
            {
                Modelo = new SistemaFinancieroEntities();

                var Permiso = (from x in Modelo.Seguridad_Usuario_Rol where x.IdUsuario == IdUsuario && (x.IdRol == 6) select x).FirstOrDefault();

                if (Permiso != null)
                {
                    Retorno = true;
                }

                return Retorno;

            }
            catch (Exception error)
            {

                throw new Exception("Error al consultar si el usuario es supervisor " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }

        }

        public bool AnalistaCredito(string IdUsuario)
        {
            bool Retorno = false;

            try
            {
                Modelo = new SistemaFinancieroEntities();

                var Permiso = (from x in Modelo.Seguridad_Usuario_Rol where x.IdUsuario == IdUsuario && (x.IdRol == 7) select x).FirstOrDefault();

                if (Permiso != null)
                {
                    Retorno = true;
                }

                return Retorno;

            }
            catch (Exception error)
            {

                throw new Exception("Error al consultar si el usuario es analista de credito " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }

        }

        public bool AnalistaCobranza(string IdUsuario)
        {
            bool Retorno = false;

            try
            {
                Modelo = new SistemaFinancieroEntities();

                var Permiso = (from x in Modelo.Seguridad_Usuario_Rol where x.IdUsuario == IdUsuario && (x.IdRol == 4) select x).FirstOrDefault();

                if (Permiso != null)
                {
                    Retorno = true;
                }

                return Retorno;

            }
            catch (Exception error)
            {

                throw new Exception("Error al consultar si el usuario es analista de cobranza " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }

        }

        public void AgregarRegistroSistema(RegistroSistema registro) {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                Modelo.RegistroSistema.Add(registro);
                Modelo.SaveChanges();

            }
            catch (Exception err)
            {

                throw new Exception("Error al guardar Session del usuario al sistema " + err);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }

        }

        public void EliminarRegistroSistema(string IdUsuario) {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                RegistroSistema registro = new RegistroSistema();

                registro = (from x in Modelo.RegistroSistema where x.IdUsuarioLogin == IdUsuario select x).FirstOrDefault();

                if (registro != null)
                {
                    Modelo.RegistroSistema.Remove(registro);
                    Modelo.SaveChanges();
                }


            }
            catch (Exception err)
            {

                throw new Exception("Error al eliminar Session del usuario al sistema " + err);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        
        }


        public bool ConsultarRegistroSistema(string IdUsuario)
        {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                RegistroSistema registro = new RegistroSistema();
                bool Logueado = false;

                registro = (from x in Modelo.RegistroSistema where x.IdUsuarioLogin == IdUsuario select x).FirstOrDefault();

                if (registro != null)
                {
                    Logueado = true;
                }

                return Logueado;

            }
            catch (Exception err)
            {

                throw new Exception("Error al consultar Session del usuario al sistema " + err);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }

        }

        public List<Seguridad_Aplicacion> ListarAplicaciones()
        {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                List<Seguridad_Aplicacion> Roles = new List<Seguridad_Aplicacion>();

                return Roles = (from x in Modelo.Seguridad_Aplicacion select x).ToList();

            }
            catch (Exception error)
            {

                throw new Exception("Ocurrio un error al momento de cargar las aplicaciones " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        }

        public int AgregarPermisosRolAplicacion(Seguridad_PermisoAplicacion aplicacion_rol)
        {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                Seguridad_PermisoAplicacion AplicacionRol = new Seguridad_PermisoAplicacion();
                int Retorno = 0;

                AplicacionRol = (from x in Modelo.Seguridad_PermisoAplicacion where x.IdAplicacion == aplicacion_rol.IdAplicacion && x.IdRol == aplicacion_rol.IdRol select x).FirstOrDefault();

                if (AplicacionRol == null)
                {
                    Modelo.Seguridad_PermisoAplicacion.Add(aplicacion_rol);
                    Modelo.SaveChanges();

                    Retorno = 1;
                }
                else
                {
                    Retorno = 2;
                }

                return Retorno;

            }
            catch (Exception error)
            {

                throw new Exception("Ocurrio un error al momento agregar permisos de aplicación " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        }


        public int EliminarPermisosRolAplicacion(int IdAplicacion, int IdRol)
        {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                Seguridad_PermisoAplicacion AplicacionRol = new Seguridad_PermisoAplicacion();
                int Retorno = 0;

                AplicacionRol = (from x in Modelo.Seguridad_PermisoAplicacion where x.IdAplicacion == IdAplicacion && x.IdRol == IdRol select x).FirstOrDefault();

                if (AplicacionRol != null)
                {
                    Modelo.Seguridad_PermisoAplicacion.Remove(AplicacionRol);
                    Modelo.SaveChanges();

                    Retorno = 1;
                }
                else
                {
                    Retorno = 2;
                }

                return Retorno;

            }
            catch (Exception error)
            {
                throw new Exception("Ocurrio un error al momento agregar permisos de aplicación " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        }

        public void ResetPassword(string IdUsuario) {
            Modelo = new SistemaFinancieroEntities();
            try
            {
                Seguridad_Usuario usuario = new Seguridad_Usuario();

                usuario = (from x in Modelo.Seguridad_Usuario where x.Usuario == IdUsuario select x).FirstOrDefault();

                if (usuario != null)
                {
                    usuario.Contraseña = "Tempo123";
                    Modelo.SaveChanges();
                }            
    
            }
            catch (Exception error)
            {
                
                throw new Exception("Error al reestablecer contraseña del usuario " + error);
            }
        
        }

        public void Activar_InactivarUsuario(string user) {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                usuario = new Seguridad_Usuario();

                usuario = (from x in Modelo.Seguridad_Usuario where x.Usuario == user select x).FirstOrDefault();

                if (usuario != null)
                {
                    if (usuario.Estado == 1)
                    {
                        usuario.Estado = 2;
                    }
                    else
                    {
                        usuario.Estado = 1;
                    }
                    Modelo.SaveChanges();
                }
            }
            catch (Exception error)
            {
                
                throw new Exception("Error al activar o inactivar el usuario " + error);
            }
        }

 

    }
}
