using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco
{
   public class producto
    {

        public String nombre { get; set; }
        public int cantidad { get; set; }
        public decimal total { get; set; }
        public float precioUnitario { get; set; }

        public producto(String nombre, int cantidad, decimal total, float precioUnitario)
        {
            nombre = this.nombre;
            cantidad = this.cantidad;
            total = this.total;
            precioUnitario = this.precioUnitario;
        }



     
    }
}
