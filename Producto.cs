using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco
{
   public class Producto
    {

        public String nombre { get; set; }
        public int cantidad { get; set; }
        public float   total { get; set; }
        public float precioUnitario { get; set; }
        public int id { get; set; }
        public float preciolista { get; set; }

        public Producto(String nombre, int id, int cantidad, float total, float precioUnitario)
        {
            this.nombre = nombre;
            this.id = id;
            this.cantidad = cantidad;
            this.total = total;
            this.precioUnitario = precioUnitario;
        }
        public Producto(String nombre, int id, int cantidad)
        {
            this.nombre = nombre;
            this.id = id;
            this.cantidad = cantidad;
         
        }
        public Producto (int id, String nombre, float precioUnitario)
        {
            this.id = id;
            this.nombre = nombre;
            this.precioUnitario = precioUnitario;


        }
        public Producto(int id, float preciolista, String nombre)
        {
            this.id = id;
            this.nombre = nombre;
            this.preciolista = preciolista;


        }
       


    }
}
