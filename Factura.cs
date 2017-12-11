using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco
{
    public class factura
    {


        public int id  { get; set; }
        public decimal subtotal { get; set; }
        public int numerofactura { get; set; }
        public decimal total { get; set; }
        public String iva { get; set; }
        public String tipoCambio { get; set; }
        public String cuotas { get; set; }
        public String fecha { get; set; }

        public factura( decimal subtotal, int numerofactura, decimal total, String iva, String tipoCambio, String cuotas, String fecha)
        {
            
            this.subtotal = subtotal;
            this.numerofactura = numerofactura;
            this.total = total;
            this.iva = iva;
            this.tipoCambio = tipoCambio;
            this.cuotas = cuotas;
            this.fecha = fecha;
        }

    
    }
}
