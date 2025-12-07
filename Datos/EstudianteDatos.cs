using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class EstudianteDatos
    {
        SistemaFacturacionEntities modeloFacturacion = null;
        tmaestudiante estudiante = null;

        public List<tmaestudiante> ListaEstudiante()
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                List<tmaestudiante> listaEstudiante = new List<tmaestudiante>();
                modeloFacturacion.Database.CommandTimeout = 300;
                return listaEstudiante = (from x in modeloFacturacion.tmaestudiante
                                          where x.estado != "INACTIVO"
                                          select x).ToList();

            }
            catch (Exception error)
            {

                throw new Exception("Ocurrio un error al momento de cargar estudiantes " + error);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public List<DetalleEstudianteDto> ListaEstudianteConTransacciones()
        {
            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                List<DetalleEstudianteDto> listaEstudiante = new List<DetalleEstudianteDto>();

                modeloFacturacion.Database.CommandTimeout = 300;

                return listaEstudiante = (from x in modeloFacturacion.tmaestudiante
                                              //join z in modeloFacturacion.tmetransacciones on x.id_estudiante equals z.id_estudiante
                                          where x.estado != "INACTIVO"
                                          select new DetalleEstudianteDto
                                          {
                                              CodigoEstudiante = x.id_estudiante,
                                              Nombres = x.nombres,
                                              Apellidos = x.apellidos,
                                              Estado = x.estado,
                                              Seccion = x.seccion,
                                              Anio_Lectivo = x.annio_lectivo,
                                              Grado = x.nivel

                                          }).Distinct().ToList();

            }
            catch (Exception error)
            {

                throw new Exception("Ocurrio un error al momento de cargar estudiantes " + error);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }

        public tmaestudiante ConsultarEstudiante(string codigoEstudiante)
        {

            try
            {
                modeloFacturacion = new SistemaFacturacionEntities();
                estudiante = new tmaestudiante();

                modeloFacturacion.Database.CommandTimeout = 300;

                return estudiante = (from x in modeloFacturacion.tmaestudiante where x.id_estudiante.TrimEnd() == codigoEstudiante select x).FirstOrDefault();
            }
            catch (Exception err)
            {

                throw new Exception("Error al consultar estudiante " + codigoEstudiante + " " + err);
            }
            finally
            {
                if (modeloFacturacion != null)
                    modeloFacturacion.Dispose();
            }
        }
    }

}
