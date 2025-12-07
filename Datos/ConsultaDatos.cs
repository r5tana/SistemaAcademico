using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Datos
{
    public class ConsultaDatos
    {
        SistemaFacturacionEntities modeloFacturacion = null;
        tmecajas caja = null;
        tmefacturas factura = null;
        tmxcontador consecutivo = null;
        tmeinventario inventario = null;

        public tmecajas ConsultarCajaUsuario(int idUsuario)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                caja = new tmecajas();

                return caja = (from x in modeloFacturacion.tmecajas where x.id_usuario == idUsuario & x.estado == "ACTIVO" select x).FirstOrDefault();
            }
            catch (Exception err)
            {

                throw new Exception("Error al consultar caja del usuario id " + idUsuario + " " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }


        public List<tmeconceptos> ListaConceptos()
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                List<tmeconceptos> lista = new List<tmeconceptos>();

                return lista = (from x in modeloFacturacion.tmeconceptos where x.estado.TrimEnd() == "ACTIVO" select x).ToList();
            }
            catch (Exception err)
            {

                throw new Exception("Error al consultar la lista de concepto " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public List<tmeinventario> ListaInventario()
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                List<tmeinventario> lista = new List<tmeinventario>();

                return lista = (from x in modeloFacturacion.tmeinventario where x.enventa == "SI" & x.estado == "ACTIVO" select x).ToList();
            }
            catch (Exception err)
            {

                throw new Exception("Error al consultar la lista de inventario " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public void InsertarFactura(tmefacturas factura)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                modeloFacturacion.tmefacturas.Add(factura);
                modeloFacturacion.SaveChanges();
            }
            catch (Exception err)
            {

                throw new Exception("Error al guardar la factura " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public void InsertarDetalleFactura(tmefacturasdet factura)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                modeloFacturacion.tmefacturasdet.Add(factura);
                modeloFacturacion.SaveChanges();
            }
            catch (Exception err)
            {

                throw new Exception("Error al guardar detalle factura " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public tmxcontador ConsultarContadorTabla(string nombreTabla)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                consecutivo = new tmxcontador();

                return consecutivo = (from x in modeloFacturacion.tmxcontador where x.tabla_nombre == nombreTabla select x).FirstOrDefault();
            }
            catch (Exception err)
            {

                throw new Exception("Error al consultar contador " + nombreTabla + " " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public void ActualizarConsecutivoTabla(long id_contador, long consecutivoId)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                consecutivo = new tmxcontador();

                consecutivo = (from x in modeloFacturacion.tmxcontador where x.id_contador == id_contador select x).FirstOrDefault();
                if (consecutivo != null)
                {
                    consecutivo.tabla_contador = consecutivoId;
                    modeloFacturacion.SaveChanges();
                }
            }
            catch (Exception err)
            {

                throw new Exception("Error al actualizar contador " + id_contador + " " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public void ActualizarStock(string idProducto, int stockNuevo)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                inventario = new tmeinventario();

                inventario = (from x in modeloFacturacion.tmeinventario where x.id_inventario == idProducto select x).FirstOrDefault();
                if (inventario != null)
                {
                    inventario.stockactual = stockNuevo;
                    modeloFacturacion.SaveChanges();
                }
            }
            catch (Exception err)
            {
                throw new Exception("Error al actualizar el stock del producto " + idProducto + " " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }



        public List<tmefacturasdet> ListarDetalleFactura(string idFactura)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                var factura = new List<tmefacturasdet>();

                modeloFacturacion.Database.CommandTimeout = 300;
                return factura = (from x in modeloFacturacion.tmefacturasdet where x.id_factura == idFactura select x).ToList();
            }
            catch (Exception err)
            {

                throw new Exception("Error a listar detalle factura  " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }


        public List<tmecajas> ListarCajas()
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                List<tmecajas> listaCaja = new List<tmecajas>();

                return listaCaja = (from x in modeloFacturacion.tmecajas select x).ToList();
            }
            catch (Exception err)
            {

                throw new Exception("Error al listar cajar " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }


        public List<tmxcontador> ListarContadorSeries()
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                List<tmxcontador> listaContador = new List<tmxcontador>();

                return listaContador = (from x in modeloFacturacion.tmxcontador where x.TipoContador == 2 select x).ToList();
            }
            catch (Exception err)
            {

                throw new Exception("Error al listar contador de series " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public void InsertarCaja(tmecajas caja)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                modeloFacturacion.tmecajas.Add(caja);
                modeloFacturacion.SaveChanges();
            }
            catch (Exception err)
            {

                throw new Exception("Error al guardar la caja " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public void ActivarInactivarCaja(int idUsuario, int tipo)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                caja = new tmecajas();

                caja = (from x in modeloFacturacion.tmecajas where x.id_usuario == idUsuario select x).FirstOrDefault();
                if (caja != null)
                {
                    if (tipo == 1) //Activar
                        caja.estado = "ACTIVO";
                    else
                    {
                        caja.Serie = null;
                        caja.estado = "INACTIVO";
                    }

                    modeloFacturacion.SaveChanges();
                }
            }
            catch (Exception err)
            {
                throw new Exception("Error al actualizar activar/inactivar caja del usuario " + idUsuario + " " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public void ActualizarSerieCaja(int idUsuario, string serie)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                caja = new tmecajas();

                caja = (from x in modeloFacturacion.tmecajas where x.id_usuario == idUsuario select x).FirstOrDefault();
                if (caja != null)
                {
                    caja.Serie = serie;
                    modeloFacturacion.SaveChanges();
                }
            }
            catch (Exception err)
            {
                throw new Exception("Error al actualizar serie del usuario " + idUsuario + " " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public List<tmefacturas> ListarFacturaPorId(string idFactura)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                List<tmefacturas> listaFactura = new List<tmefacturas>();

                return listaFactura = (from x in modeloFacturacion.tmefacturas where x.id_factura == idFactura select x).ToList();
            }
            catch (Exception err)
            {

                throw new Exception("Error al listar número de recibo " + idFactura + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public List<tmefacturas> ListarFacturaPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                List<tmefacturas> listaFactura = new List<tmefacturas>();

                modeloFacturacion.Database.CommandTimeout = 300;
                return listaFactura = (from x in modeloFacturacion.tmefacturas where x.fecha >= fechaInicio & x.fecha <= fechaFin select x).ToList();
            }
            catch (Exception err)
            {

                throw new Exception("Error al listar facturas por rango de fecha" + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public void InsertarDetalleFacturaLista(List<tmefacturasdet> facturasDetalle)
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                modeloFacturacion.Database.CommandTimeout = 300;
                modeloFacturacion.tmefacturasdet.AddRange(facturasDetalle);
                modeloFacturacion.SaveChanges();
            }
            catch (Exception err)
            {

                throw new Exception("Error al guardar lista detalle factura " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }

        }

    }
}
