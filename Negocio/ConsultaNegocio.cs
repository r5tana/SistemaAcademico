using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ConsultaNegocio
    {
        public tmecajas ConsultarCajaUsuario(int idUsuario)
        {
            ConsultaDatos datosConsulta = new ConsultaDatos();
            return datosConsulta.ConsultarCajaUsuario(idUsuario);
        }


        public List<tmeconceptos> ListaConceptos()
        {
            ConsultaDatos datosConsulta = new ConsultaDatos();
            return datosConsulta.ListaConceptos();
        }

        public List<tmeinventario> ListaInventario()
        {
            ConsultaDatos datosConsulta = new ConsultaDatos();
            return datosConsulta.ListaInventario();
        }

        public void InsertarFactura(tmefacturas factura)
        {
            ConsultaDatos datosConsulta = new ConsultaDatos();
            datosConsulta.InsertarFactura(factura);
        }

        public tmxcontador ConsultarContadorTabla(string nombreTabla)
        {
            ConsultaDatos datosConsulta = new ConsultaDatos();
            return datosConsulta.ConsultarContadorTabla(nombreTabla);
        }

        public void ActualizarConsecutivoTabla(long id_contador, long consecutivoId)
        {

            ConsultaDatos datosConsulta = new ConsultaDatos();
            datosConsulta.ActualizarConsecutivoTabla(id_contador, consecutivoId);
        }


        public void InsertarDetalleFactura(tmefacturasdet factura)
        {
            ConsultaDatos datosConsulta = new ConsultaDatos();
            datosConsulta.InsertarDetalleFactura(factura);
        }

        public void ActualizarStock(string idProducto, int stockNuevo)
        {
            ConsultaDatos datosConsulta = new ConsultaDatos();
            datosConsulta.ActualizarStock(idProducto, stockNuevo);
        }


        public List<tmefacturasdet> ListarDetalleFactura(string idFactura)
        {
            ConsultaDatos datosConsulta = new ConsultaDatos();
            return datosConsulta.ListarDetalleFactura(idFactura);
        }

        public List<tmecajas> ListarCajas()
        {
            ConsultaDatos datosConsulta = new ConsultaDatos();
            return datosConsulta.ListarCajas();
        }

        public List<tmxcontador> ListarContadorSeries()
        {

            ConsultaDatos datosConsulta = new ConsultaDatos();
            return datosConsulta.ListarContadorSeries();
        }

        public void InsertarCaja(tmecajas caja)
        {

            ConsultaDatos datosConsulta = new ConsultaDatos();
            datosConsulta.InsertarCaja(caja);

        }

        public void ActivarInactivarCaja(int idUsuario, int tipo)
        {
            ConsultaDatos datosConsulta = new ConsultaDatos();
            datosConsulta.ActivarInactivarCaja(idUsuario, tipo);
        }

        public void ActualizarSerieCaja(int idUsuario, string serie)
        {
            ConsultaDatos datosConsulta = new ConsultaDatos();
            datosConsulta.ActualizarSerieCaja(idUsuario, serie);
        }

    }

}