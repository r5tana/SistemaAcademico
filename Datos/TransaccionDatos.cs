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
                modeloFacturacion.Database.CommandTimeout = 300;
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

        public void ActualizarTrasaccionBanco(string idComprobante, string idEstudiante, string idConcepto, string idCategoria, int anio, out long idTransaccion)
        {
            try
            {
                idTransaccion = 0;
                modeloFacturacion = new SistemaFacturacionEntities();
                transacciones = new tmetransacciones();

                transacciones = (from x in modeloFacturacion.tmetransacciones
                                 where x.id_estudiante == idEstudiante & x.id_concepto.TrimEnd() == idConcepto
                                    & x.id_categoria == idCategoria & x.annio_lectivo == anio
                                 select x).FirstOrDefault();

                if (transacciones != null)
                {
                    idTransaccion = transacciones.idtrans;
                    transacciones.estadopor = "BANPRO";
                    transacciones.estado = "1";
                    transacciones.id_factura_banco = idComprobante;
                    modeloFacturacion.Database.CommandTimeout = 300;
                    modeloFacturacion.SaveChanges();
                }
            }
            catch (Exception error)
            {

                throw new Exception("Error al actualizar transacción por estudiante " + error);
            }
        }

        public List<tmetransacciones> ListaTransaccionesPorEstudiante(string codigoEstudiante)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                List<tmetransacciones> listaTransacciones = new List<tmetransacciones>();

                modeloFacturacion.Database.CommandTimeout = 300;
                return listaTransacciones = (from x in modeloFacturacion.tmetransacciones where x.id_estudiante == codigoEstudiante select x).ToList();

            }
            catch (Exception error)
            {

                throw new Exception("Ocurrio un error al momento de cargar transacción por estudiante " + error);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public List<DetalleTransaccionEstudianteDto> ListaTransaccionesPendientesEstudiante(string codigoEstudiante)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                List<DetalleTransaccionEstudianteDto> listaTransacciones = new List<DetalleTransaccionEstudianteDto>();
                modeloFacturacion.Database.CommandTimeout = 300;

                return listaTransacciones = (from x in modeloFacturacion.tmetransacciones
                                             where x.id_estudiante == codigoEstudiante
                                             & x.estado == "0"
                                             select new DetalleTransaccionEstudianteDto
                                             {
                                                 IdTransaccion = x.idtrans,
                                                 CodigoEstudiante = x.id_estudiante,
                                                 DescripcionTransaccion = x.descripcion,
                                                 Estado = x.estado,
                                                 Monto = x.total_cordobas

                                             }).ToList();

            }
            catch (Exception error)
            {

                throw new Exception("Ocurrio un error al momento de cargar las transacciones pendientes " + error);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public void ActualizarTrasaccionPorId(int idTransaccion)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                transacciones = new tmetransacciones();

                transacciones = (from x in modeloFacturacion.tmetransacciones
                                 where x.idtrans == idTransaccion
                                 select x).FirstOrDefault();

                if (transacciones != null)
                {
                    transacciones.estado = "1";
                    modeloFacturacion.SaveChanges();
                }
            }
            catch (Exception error)
            {

                throw new Exception("Error al actualizar transacción " + error);
            }
        }

        public tmetransacciones ConsultarTransaccion(string idComprobante, string idEstudiante, string idConcepto, string idCategoria, int anio)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                transacciones = new tmetransacciones();

                modeloFacturacion.Database.CommandTimeout = 300;
                return transacciones = (from x in modeloFacturacion.tmetransacciones
                                        where x.id_estudiante == idEstudiante & x.id_concepto.TrimEnd() == idConcepto
                                           & x.id_categoria == idCategoria & x.annio_lectivo == anio
                                        select x).FirstOrDefault();

            }
            catch (Exception error)
            {

                throw new Exception("Error al actualizar transacción por estudiante " + error);
            }
        }

    }
}
