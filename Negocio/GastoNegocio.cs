using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Entidades;

namespace Negocio
{
   public class GastoNegocio
    {
       Datos.GastosDatos GastoDat = null;
       public void GuardarGastos(Gastos gasto)
       {
           GastoDat = new GastosDatos();
           GastoDat.GuardarGastos(gasto);
       }

       public List<Gastos> ListaGastos(DateTime FechaInicio, DateTime FechaFin) {
           GastoDat = new GastosDatos();
           return GastoDat.ListaGastos(FechaInicio,FechaFin);
       }

    }
}
