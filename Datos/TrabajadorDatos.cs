using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Datos
{
    public class TrabajadorDatos
    {
        SistemaFinancieroEntities Modelo = null;
        Trabajador trabajador = null;
        public void GuardarTrabjador(Trabajador trabajador)
        {

            try
            {
                Modelo = new SistemaFinancieroEntities();
                Modelo.Trabajador.Add(trabajador);
                Modelo.SaveChanges();

            }
            catch (Exception ex)
            {

                throw new Exception("Error al guardar los datos del trabajador " + ex);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }
        }

        public Trabajador ConsultarTrabajabor(string Cedula)
        {


            try
            {
                Modelo = new SistemaFinancieroEntities();
                trabajador = new Trabajador();

                trabajador = (from x in Modelo.Trabajador where x.CedulaTrabajador == Cedula select x).FirstOrDefault();
                return trabajador;


            }
            catch (Exception ex)
            {

                throw new Exception("Error al consultar los datos del trabajador " + ex);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }

        }

        public List<Trabajador> ListarTrabajadores()
        {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                List<Trabajador> Lista = new List<Trabajador>();

                Lista = (from x in Modelo.Trabajador select x).ToList();
                return Lista;
            }
            catch (Exception error)
            {

                throw new Exception("Error al consultar la lista de trabajadores " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }


        }

        public List<Trabajador> ListarTrabajadoresConPagosHoy()
        {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                List<Trabajador> Lista = new List<Trabajador>();


                DateTime FechaActual_Ini = DateTime.Now.Date;
                DateTime FechaActual_Fin = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                Modelo = new SistemaFinancieroEntities();


               var ListaTrabajadoresConPago = (from x in Modelo.Pago
                         join y in Modelo.Trabajador on x.IdTrabajador equals y.CedulaTrabajador
                         where x.FechaPago >= FechaActual_Ini && x.FechaPago <= FechaActual_Fin && x.Estado == 1
                         select y).GroupBy(x => x.CedulaTrabajador).ToList();


                foreach (var item in ListaTrabajadoresConPago)
                {
                    Lista.Add(item.FirstOrDefault());
                }

                return Lista;
            }
            catch (Exception error)
            {

                throw new Exception("Error al consultar la lista de trabajadores con pagos " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }


        }


        public List<Trabajador> ListarTrabajadoresConArqueosHoy()
        {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                List<Trabajador> Lista = new List<Trabajador>();


                DateTime FechaActual_Ini = DateTime.Now.Date;
                DateTime FechaActual_Fin = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                Modelo = new SistemaFinancieroEntities();


                var ListaTrabajadoresConPago = (from x in Modelo.Arqueo
                                                join y in Modelo.Trabajador on x.IdAnalista equals y.CedulaTrabajador
                                                where x.FechaArqueo >= FechaActual_Ini && x.FechaArqueo <= FechaActual_Fin && x.Estado == 1
                                                select y).GroupBy(x => x.CedulaTrabajador).ToList();


                foreach (var item in ListaTrabajadoresConPago)
                {
                    Lista.Add(item.FirstOrDefault());
                }

                return Lista;
            }
            catch (Exception error)
            {

                throw new Exception("Error al consultar la lista de trabajadores con pagos " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }


        }


        public List<Trabajador> ListarTrabajadoresAnalistas()
        {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                List<Trabajador> Lista = new List<Trabajador>();


                Modelo = new SistemaFinancieroEntities();


                var ListaTrabajadoresAnalistas = (from x in Modelo.Trabajador
                                                join y in Modelo.Seguridad_Usuario_Rol on x.CedulaTrabajador equals y.IdUsuario
                                                where y.IdRol == 4
                                                select x).GroupBy(x => x.CedulaTrabajador).ToList();


                foreach (var item in ListaTrabajadoresAnalistas)
                {
                    Lista.Add(item.FirstOrDefault());
                }

                return Lista;
            }
            catch (Exception error)
            {

                throw new Exception("Error al consultar la lista de trabajadores con pagos " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }


        }

        public List<Trabajador> ListarTrabajadoresDeSupervisor(string IdSupervisor)
        {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                List<Trabajador> Lista = new List<Trabajador>();

                int OficinaSupervisor = (from x in Modelo.Trabajador where x.CedulaTrabajador == IdSupervisor select x.IdOficina).FirstOrDefault();

                Lista = (from x in Modelo.Trabajador where x.IdOficina == OficinaSupervisor select x).ToList();


                return Lista;
            }
            catch (Exception error)
            {

                throw new Exception("Error al consultar la lista de trabajadores " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }


        }


        public void ActualizarTrabajador(Trabajador ModTrabajador)
        {

            try
            {
                Modelo = new SistemaFinancieroEntities();
                trabajador = new Trabajador();

                trabajador = (from x in Modelo.Trabajador where x.CedulaTrabajador == ModTrabajador.CedulaTrabajador select x).FirstOrDefault();

                if (trabajador != null)
                {
                    // trabajador.NombreTrabajador = ModTrabajador.NombreTrabajador;
                    trabajador.Direccion = ModTrabajador.Direccion;
                    trabajador.Telefono1 = ModTrabajador.Telefono1;
                    trabajador.Telefono2 = ModTrabajador.Telefono2;
                    //trabajador.Sexo = ModTrabajador.Sexo;
                    trabajador.EstadoCivil = ModTrabajador.EstadoCivil;
                    trabajador.PersonaEmergencia = ModTrabajador.PersonaEmergencia;
                    trabajador.TelefonoEmergencia = ModTrabajador.TelefonoEmergencia;
                    trabajador.IdOficina = ModTrabajador.IdOficina;
                    Modelo.SaveChanges();
                }
            }
            catch (Exception error)
            {

                throw new Exception("Error al actualizar al trabajador " + error);
            }
            finally
            {
                if (Modelo != null)
                    Modelo.Dispose();
            }

        }

        public void Activar_InactivarTrabajador(string IdTrabajador)
        {
            try
            {
                Modelo = new SistemaFinancieroEntities();
                Trabajador trabajador = new Trabajador();

                trabajador = (from x in Modelo.Trabajador where x.CedulaTrabajador == IdTrabajador select x).FirstOrDefault();
                if (trabajador != null)
                {
                    if (trabajador.Estado == 1)
                    {
                        trabajador.Estado = 2;
                    }
                    else
                    {
                        trabajador.Estado = 1;
                    }
                    Modelo.SaveChanges();
                }
            }
            catch (Exception error)
            {

                throw new Exception("Error al activar o inactivar trabajador " + error);
            }

        }
    }
}
