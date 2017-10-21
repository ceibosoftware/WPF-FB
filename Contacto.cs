using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco
{
   public  class Contacto
    {

      //  public int id { get; set; }
        public String NombreContacto { get; set; }
        public String Email { get; set; }
        public String NumeroTelefono { get; set; }


        public Contacto(String nomContacto, String ema, String numTelefono)
        {
            NombreContacto = nomContacto;
       
            Email = ema;
            NumeroTelefono = numTelefono;

        }
    }
}
