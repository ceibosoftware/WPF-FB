using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpfFamiliaBlanco.SalidasNuevo.Clases.OrdenesPedido
{
    class ListaPrecio
    {
        CRUD conexion = new CRUD();
        String nombre;
        int idCliente;
        List<Producto> productos;
        private int id;
        public string Nombre { get => nombre; set => nombre = value; }
        public int Id { get => id; set => id = value; }
        public int IdCliente { get => idCliente; set => idCliente = value; }
        public List<Producto> Productos { get => productos; set => productos = value; }

        public List<Producto> GetLista(int idCliente)
        {
            string consulta = "SELECT p.nombre , pLista.precioLista, lp.nombre, p.idProductos, lp.idLista  FROM productos_has_listadeprecios pLista, productos p, clientesmi c, listadeprecios lp WHERE c.idClientemi = '" + idCliente+ "' and c.FK_idLista = pLista.FK_idLista and p.idProductos = pLista.FK_idProductos and lp.idLista = c.FK_idLista";
            SetDatos(conexion.coleccion(consulta));
            return productos;
        }
        private void SetDatos(DataTable productos)
        {
            int id;
            float precio;
            string nombre;
            this.id = (int)productos.Rows[0].ItemArray[4];
            this.productos = new List<Producto>();
            this.nombre = productos.Rows[0].ItemArray[2].ToString();
            for (int i = 0; i < productos.Rows.Count; i++)
            {
                id = (int)productos.Rows[i].ItemArray[3];
                precio = (float)productos.Rows[i].ItemArray[1];
                nombre = productos.Rows[i].ItemArray[0].ToString();
       
                this.productos.Add(new Producto(id, precio, nombre));
            }
            
        }
        public void actualizarPrecio(int idProducto, float precio)
        {
            string consulta = "UPDATE productos_has_listadeprecios SET precioLista = '"+precio+"' where FK_idProductos = '"+idProducto+"' and FK_idLista = '"+id+"'" ;
            conexion.operaciones(consulta);
            MessageBox.Show("Precio actualizado correctamente");
        }
    }
}
