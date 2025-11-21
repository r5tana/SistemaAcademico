using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class EstudianteNegocio
    {
        public List<tmaestudiante> ListaEstudiante()
        {
            EstudianteDatos datosEstudiante = new EstudianteDatos();
            return datosEstudiante.ListaEstudiante();
        }

        public tmaestudiante ConsultarEstudiante(string codigoEstudiante)
        {
            EstudianteDatos datosEstudiante = new EstudianteDatos();
            return datosEstudiante.ConsultarEstudiante(codigoEstudiante);
        }
    }
}