using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco.Gastos.Clases
{
   
    class Impuesto : Gasto
    {

        int idImpuesto;
        String categoria;
        DateTime fechav;

       

        public string Categoria { get => categoria; set => categoria = value; }
        public int IdImpuesto { get => idImpuesto; set => idImpuesto = value; }

        public Impuesto(string nombre,String tipo, float monto, string observaciones, int formaPago, int gasto, DateTime fech, DateTime fechaven)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.monto = monto;
            this.observaciones = observaciones;
            this.formaPago = formaPago;
            this.gastos = gasto;
            this.fecha = fech;
            this.fechav = fechaven;
       
        }

        public Impuesto()
        {

        }
        public  void Delete(String id)
        {
            String delete = "DELETE  FROM gasto WHERE idGasto = '"+id+"'";
            this.Conexion.operaciones(delete);
        }

        public  void Save()
        {
            String insert = "INSERT INTO gasto (nombre, tipo, monto, observaciones, formaPago, fecha, fechaVencimiento) VALUES('"+this.nombre+"','"+this.tipo+"','"+this.monto.ToString().Replace(",", ".") + "', '"+this.observaciones+"', '"+this.formaPago+ "', '" + this.fecha.ToString("yyyy/MM/dd") + "', '" + this.fechav.ToString("yyyy/MM/dd") + "')";
            this.Conexion.operaciones(insert);
        }

        public  void Update(int id)
        {
            String updateImpuesto = "UPDATE gasto SET nombre =  '" + this.nombre + "',tipo = '" + this.tipo + "' ,monto = '" + this.monto + "',observaciones= '" + this.observaciones + "',formaPago='" + this.formaPago + "' ,fecha = '" + this.fecha.ToString("yyyy/MM/dd") + "',fechaVencimiento= '" + this.fechav.ToString("yyyy/MM/dd") + "' WHERE idGasto = '" + id + "'";
            Conexion.operaciones(updateImpuesto);
        }
    }
}
