using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Datos;


namespace Negocio
{
    public class TrabajadorNegocio: IDisposable
    {
        public void GuardarTrabjador(Trabajador trabajador)
        {
            TrabajadorDatos TrabDatos = new TrabajadorDatos();
            TrabDatos.GuardarTrabjador(trabajador);
        }

        public Trabajador ConsultarTrabajabor(string Cedula) {
            TrabajadorDatos TrabDatos = new TrabajadorDatos();
            return TrabDatos.ConsultarTrabajabor(Cedula);
        }

        public List<Trabajador> ListarTrabajadores() {
            TrabajadorDatos TrabDatos = new TrabajadorDatos();
            return TrabDatos.ListarTrabajadores();
        }

        public List<Trabajador> ListarTrabajadoresDeSupervisor(string IdSupervisor)
        {
            TrabajadorDatos TrabDatos = new TrabajadorDatos();
            return TrabDatos.ListarTrabajadoresDeSupervisor(IdSupervisor);
        }
        public List<Trabajador> ListarTrabajadoresConPagosHoy()
        {
            TrabajadorDatos TrabDatos = new TrabajadorDatos();
            return TrabDatos.ListarTrabajadoresConPagosHoy();
        }

        public List<Trabajador> ListarTrabajadoresConArqueosHoy()
        {
            TrabajadorDatos TrabDatos = new TrabajadorDatos();
            return TrabDatos.ListarTrabajadoresConArqueosHoy();
        }

        public List<Trabajador> ListarTrabajadoresAnalistas()
        {
            TrabajadorDatos TrabDatos = new TrabajadorDatos();
            return TrabDatos.ListarTrabajadoresAnalistas();
        }

        public void ActualizarTrabajador(Trabajador ModTrabajador) {
            TrabajadorDatos TrabDatos = new TrabajadorDatos();
            TrabDatos.ActualizarTrabajador(ModTrabajador);
        }

        public void Activar_InactivarTrabajador(string IdTrabajador)
        {
            TrabajadorDatos TrabDatos = new TrabajadorDatos();
            TrabDatos.Activar_InactivarTrabajador(IdTrabajador);
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
