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
        public decimal precioUnitario { get; set; }
        public int id { get; set; }

        public producto(String nombre, int id, int cantidad, decimal total, decimal precioUnitario)
        {
            this.nombre = nombre;
            this.id = id;
            this.cantidad = cantidad;
            this.total = total;
            this.precioUnitario = precioUnitario;
        }



     
    }
}
