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
        public float montoCuota { get; set; }
        public int numeroCuota { get; set; }
        public int estado { get; set; }

        public  Cuotas(int id, int dias, DateTime fecha, float montoCuota, int numeroCuota, int estado)
        {
            this.cuota = id;
            this.dias = dias;
            this.fechadepago = fecha;
            this.montoCuota = montoCuota;
            this.numeroCuota = numeroCuota;
            this.estado = estado;
        }

        public Cuotas(int id, int dias, DateTime fecha, float montoCuota, int numeroCuota)
        {
            this.cuota = id;
            this.dias = dias;
            this.fechadepago = fecha;
            this.montoCuota = montoCuota;
            this.numeroCuota = numeroCuota;
        
        }
    }
}
