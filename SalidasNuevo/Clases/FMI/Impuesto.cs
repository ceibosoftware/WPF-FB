using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco.SalidasNuevo.Clases.FMI
{
    class Impuesto
    {
        CRUD conexion = new CRUD();
        private int id;
        private string nombre;
        private float importe;

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public float Importe { get => importe; set => importe = value; }

        public void Insertar()
        {
            String insertarDatos = "INSERT INTO impuesto (nombre, importe) VALUES ('"+nombre+"', '"+importe+"')";
            conexion.operaciones(insertarDatos);
        }

        public void Eliminar(int idImpuesto)
        {
            String EliminarImporte = "DELETE FROM impuesto WHERE id = '"+idImpuesto+"'";
            conexion.operaciones(EliminarImporte);
        }
    }
}
