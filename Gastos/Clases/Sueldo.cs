using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco.Gastos.Clases
{
    class Sueldo : Gasto
    {

        int idSueldo;
        String horasTrabajadas;
        float viaticos;

        public Sueldo(string nombre, String tipo, float monto, string observaciones, int formaPago, int gasto, String horasTrabajadas,DateTime fech, float viat)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.monto = monto;
            this.observaciones = observaciones;
            this.formaPago = formaPago;
            this.gastos = gasto;
            this.horasTrabajadas = horasTrabajadas;
            this.fecha = fech;
            this.viaticos = viat;
        }
        public  void Delete(String id)
        {
          
        }

        public  void Save()
        {
            String insert = "INSERT INTO gasto (nombre, tipo, monto, observaciones, horasTrabajadas,formaPago,gastot, fecha, viatico) VALUES('" + this.nombre + "','" + this.tipo + "','" + this.monto.ToString().Replace(",",".") + "', '" + this.observaciones + "', '" +this.horasTrabajadas  + "', '" + this.formaPago + "', '" + 2 + "', '" + this.fecha.ToString("yyyy/MM/dd") + "', '"+this.viaticos.ToString().Replace(",", ".") + "')";
            this.Conexion.operaciones(insert);
        }

        public  void Update(int id)
        {
            String updateSueldo = "UPDATE gasto SET nombre =  '" + this.nombre + "',tipo = '" + this.tipo + "' ,monto = '" + this.monto + "',observaciones= '" + this.observaciones + "',horasTrabajadas='" + this.horasTrabajadas + "',formaPago='" + this.formaPago + "' ,fecha = '" + this.fecha.ToString("yyyy/MM/dd") + "',viatico= '" + this.viaticos + "' WHERE idGasto = '" + id + "'";
            Conexion.operaciones(updateSueldo);
        }
    }
}
