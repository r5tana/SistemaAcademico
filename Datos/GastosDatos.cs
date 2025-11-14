using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Datos
{
    public class GastosDatos
    {
        SistemaFinancieroEntities Modelo = null;

        public void GuardarGastos(Gastos gasto) {
            Modelo = new SistemaFinancieroEntities();
            try
            {
                Modelo.Gastos.Add(gasto);
                Modelo.SaveChanges();
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error al guardar el gasto "  + ex);
            }

        }

        public List<Gastos> ListaGastos(DateTime FechaInicio,DateTime FechaFin) {
            Modelo = new SistemaFinancieroEntities();
            try
            {
                List<Gastos> Lista = new List<Gastos>();
                DateTime FechaFinal = FechaFin.AddHours(23);

              return  Lista = (from x in Modelo.Gastos where x.Fecha >= FechaInicio && x.Fecha <= FechaFinal && x.Estado == 1 select x).ToList();

            }
            catch (Exception ex)
            {
                
                throw new Exception("Error al listar los gastos " + ex);
            }

        }

    }
}
