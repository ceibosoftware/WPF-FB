using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco.Gastos.Clases
{
    class Otro : Gasto
    {
        int idOtro;
        String observacion;

        public Otro(string nombre, int tipo, float monto, string observaciones, int formaPago, int gasto, DateTime fech)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.monto = monto;
            this.observaciones = observaciones;
            this.formaPago = formaPago;
            this.gastos = gasto;
            this.fecha = fech;
        }
        public  void Delete(String id)
        {
     
        }

        public  void Save()
        {
            String insert = "INSERT INTO gasto (nombre, tipo, monto, observaciones, formaPago,gastot, fecha) VALUES('" + this.nombre + "','" + this.tipo + "','" + this.monto.ToString().Replace(",", ".") + "', '" + this.observaciones + "', '" + this.formaPago + "', '" + 3 + "', '" + this.fecha.ToString("yyyy/MM/dd") + "')";
            this.Conexion.operaciones(insert);
        }

        public  void Update()
        {
            
        }
    }
}
