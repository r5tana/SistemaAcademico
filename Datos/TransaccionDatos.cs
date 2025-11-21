using Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class TransaccionDatos
    {
        SistemaFacturacionEntities modeloFacturacion = null;
        tmetransacciones transacciones = null;

        public void InsertarInformacionBancoLista(List<tmeinfobancos> banco)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                modeloFacturacion.tmeinfobancos.AddRange(banco);
                modeloFacturacion.SaveChanges();
            }
            catch (Exception err)
            {

                throw new Exception("Error al guardar detalle banco " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }

        }

        public void ActualizarTrasaccionBanco(string idComprobante ,string idEstudiante, string idConcepto, string idCategoria, int anio)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                transacciones = new tmetransacciones();

                transacciones = (from x in modeloFacturacion.tmetransacciones where x.id_estudiante.TrimEnd() == idEstudiante & x.id_concepto.TrimEnd() == idConcepto
                                    & x.id_categoria.TrimEnd() == idCategoria & x.annio_lectivo == anio select x).FirstOrDefault();

                if (transacciones != null)
                {
                    transacciones.estadopor = "BANPRO";
                    transacciones.estado = "1";
                    transacciones.id_factura_banco = idComprobante;
                    modeloFacturacion.SaveChanges();
                }
            }
            catch (Exception error)
            {

                throw new Exception("Error al activar o inactivar el usuario " + error);
            }
        }

        public List<tmetransacciones> ListaTransaccionesPorEstudiante(string codigoEstudiante)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                List<tmetransacciones> listaTransacciones = new List<tmetransacciones>();

                return listaTransacciones = (from x in modeloFacturacion.tmetransacciones where x.id_estudiante.TrimEnd() == codigoEstudiante select x).ToList();

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

    }
}
