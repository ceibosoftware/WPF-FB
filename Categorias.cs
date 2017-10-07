using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco
{
    public class Categorias
    {

        public string nombre { get; set; }
        public int id { get; set; }
        public Categorias(string nombre, int id)
        {
            this.nombre = nombre;
            this.id = id;
        }
    }
}
