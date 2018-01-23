using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco
{
    public class Cuotas2
    {

        public int id { get; set; }
        public int dias { get; set; }
        public String fecha { get; set; }

        public Cuotas2(int id, int dias, String fecha)
        {
            this.id = id;
            this.dias = dias;
            this.fecha = fecha;
        }
    }
}