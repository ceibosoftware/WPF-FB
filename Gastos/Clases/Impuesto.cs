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

       

        public string Categoria { get => categoria; set => categoria = value; }
        public int IdImpuesto { get => idImpuesto; set => idImpuesto = value; }

        public Impuesto(string nombre,int tipo, float monto, string observaciones, int formaPago, int gasto, DateTime fech)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.monto = monto;
            this.observaciones = observaciones;
            this.formaPago = formaPago;
            this.gastos = gasto;
            this.fecha = fech;
       
        }

        public Impuesto()
        {

        }
        public  void Delete(String id)
        {
            String delete = "DELETE * FROM gasto WHERE idGasto = '"+id+"'";
            this.Conexion.operaciones(delete);
        }

        public  void Save()
        {
            String insert = "INSERT INTO gasto (nombre, tipo, monto, observaciones, formaPago, fecha) VALUES('"+this.nombre+"','"+this.tipo+"','"+this.monto.ToString().Replace(",", ".") + "', '"+this.observaciones+"', '"+this.formaPago+ "', '" + this.fecha.ToString("yyyy/MM/dd") + "')";
            this.Conexion.operaciones(insert);
        }

        public  void Update()
        {
   
        }
    }
}
