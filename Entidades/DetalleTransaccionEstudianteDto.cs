using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DetalleTransaccionEstudianteDto
    {
        public long IdTransaccion { get; set; }
        public string CodigoEstudiante { get; set; }
        public string DescripcionTransaccion { get; set; }
        public double? Monto { get; set; }
        public string Estado { get; set; }

    }
}
