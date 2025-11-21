using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class TransaccionNegocio
    {
        public void InsertarInformacionBancoLista(List<tmeinfobancos> banco)
        {
            TransaccionDatos transaccionDatos = new TransaccionDatos();
            transaccionDatos.InsertarInformacionBancoLista(banco);
        }


        public void ActualizarTrasaccionBanco(string idComprobante, string idEstudiante, string idConcepto, string idCategoria, int anio)
        {
            TransaccionDatos transaccionDatos = new TransaccionDatos();
            transaccionDatos.ActualizarTrasaccionBanco(idComprobante, idEstudiante, idConcepto, idCategoria, anio);
        }

        public List<tmetransacciones> ListaTransaccionesPorEstudiante(string codigoEstudiante)
        {
            TransaccionDatos transaccionDatos = new TransaccionDatos();
            return transaccionDatos.ListaTransaccionesPorEstudiante(codigoEstudiante);

        }

    }
}
