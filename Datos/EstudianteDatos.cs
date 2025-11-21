using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

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

                return listaEstudiante = (from x in modeloFacturacion.tmaestudiante select x).ToList();

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
