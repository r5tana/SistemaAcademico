using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DetalleTransaccionDto
    {
        public string NumeroReferencia { get; set; }
        public string CodigoAlumno { get; set; }
        public string Descripcion { get; set; }
        public Nullable<double> Monto { get; set; }
        public long IdTransaccion { get; set; }
    }
}
