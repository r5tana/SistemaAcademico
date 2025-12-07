using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DetalleEstudianteDto
    {
        public string CodigoEstudiante { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Estado { get; set; }
        public string Seccion { get; set; }
        public int? Anio_Lectivo { get; set; }
        public string Grado { get; set; }

    }
}
