using System;
using System.Collections.Generic;
using System.Linq;
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

                return lista = (from x in modeloFacturacion.tmeinventario where x.enventa.TrimEnd() == "SI" select x).ToList();
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

                throw new Exception("Error al consultar contrado " + nombreTabla + " " + err);
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


    }
}
