using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco
{
    public class cuotas
    {

        public int cuota { get; set; }
        public int dias { get; set; }
        public DateTime fechadepago { get; set; }

        public  cuotas(int id, int dias, DateTime fecha)
        {
            this.cuota = id;
            this.dias = dias;
            this.fechadepago = fecha;
        }
    }
}
