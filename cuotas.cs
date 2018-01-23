using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco
{
    public class Cuotas
    {

        public int cuota { get; set; }
        public int dias { get; set; }
        public DateTime fechadepago { get; set; }

        public  Cuotas(int id, int dias, DateTime fecha)
        {
            this.cuota = id;
            this.dias = dias;
            this.fechadepago = fecha;
        }
    }
}
