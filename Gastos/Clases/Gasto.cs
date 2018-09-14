using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfFamiliaBlanco.Gastos.Vistas;

namespace wpfFamiliaBlanco.Gastos.Clases
{
 
    public  class Gasto
    {
        CRUD conexion = new CRUD();
        public int idGasto { get; set; }
        public String nombre { get; set; }
        public int tipo { get; set; }
        public float monto { get; set; }
        public String observaciones { get; set; }
        public int formaPago { get; set; }
        public int gastos { get; set; }
        public DateTime fecha { get; set; }

        internal CRUD Conexion { get => conexion; set => conexion = value; }

      
       

        public static DataTable GetGastos(int tipo)
        {
            CRUD conex = new CRUD();
            if (tipo == 0)
            {
                String getgasto = "SELECT * FROM gasto WHERE gastot = '"+0+"'";
                DataTable gastos = conex.coleccion(getgasto);
                return gastos;
            }
            else if (tipo == 1)
            {
                String getgasto = "SELECT * FROM gasto WHERE gastot = '" + 1 + "'";
                DataTable gastos = conex.coleccion(getgasto);
                return gastos;
            }
            else if (tipo == 2)
            {
                String getgasto = "SELECT * FROM gasto WHERE gastot = '" + 2 + "'";
                DataTable gastos = conex.coleccion(getgasto);
                return gastos;
            }
            else
            {
                String getgasto = "SELECT * FROM gasto WHERE gastot = '" + 3 + "'";
                DataTable gastos = conex.coleccion(getgasto);
                return gastos;
            }



        }
    }
}
