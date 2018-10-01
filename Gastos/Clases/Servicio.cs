using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco.Gastos.Clases
{
    class Servicio : Gasto
    {

        int idServicio;
        String unidad;
        String proveedor;
        DateTime fechaV;

        public Servicio(string nombre, String tipo, float monto, string observaciones, int formaPago, int gasto, String unidad, DateTime fech, String proveedor, DateTime fechavv)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.monto = monto;
            this.observaciones = observaciones;
            this.formaPago = formaPago;
            this.gastos = gasto;
            this.unidad = unidad;
            this.fecha = fech;
            this.proveedor = proveedor;
            this.fechaV = fechavv;
        }

        public void Delete(String id)
        {
            String delete = "DELETE * FROM gasto WHERE idGasto = '" + id + "'";
            this.Conexion.operaciones(delete);
        }

        public  void Save()
        {
            String insert = "INSERT INTO gasto (nombre, tipo, monto, observaciones, unidad,formaPago,gastot,fecha, proveedor) VALUES('" + this.nombre + "','" + this.tipo + "','" + this.monto.ToString().Replace(",", ".") + "', '" + this.observaciones + "', '" + this.unidad + "', '" + this.formaPago + "', '" + 1 + "', '" + this.fecha.ToString("yyyy/MM/dd") + "', '" + this.proveedor + "', '" + this.fechaV.ToString("yyyy/MM/dd") + "')";
            this.Conexion.operaciones(insert);
        }

        public  void Update(int id)
        {
            String updateserv = "UPDATE gasto SET nombre =  '" + this.nombre + "',tipo = '" + this.tipo + "' ,monto = '" + this.monto + "',observaciones= '" + this.observaciones + "',formaPago='" + this.formaPago + "' ,fecha = '" + this.fecha.ToString("yyyy/MM/dd") + "',fechaVencimiento= '" + this.fechaV.ToString("yyyy/MM/dd") + "',proveedor = '" + this.proveedor + "',unidad = '" + this.unidad + "' WHERE idGasto = '" + id + "'";
            Conexion.operaciones(updateserv);
        }


    }
}
