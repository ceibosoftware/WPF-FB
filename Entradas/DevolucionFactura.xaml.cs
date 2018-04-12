using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Lógica de interacción para DevolucionFactura.xaml
    /// </summary>
    public partial class DevolucionFactura : Page
    {

        DataTable productos;
        String iva;
        String cambio;
        Producto producto;
        String idFactura;
        String lastid;
     CRUD conexion = new CRUD();
        public List<Producto> itemsNC = new List<Producto>();
        public List<Producto> productosAmodificar = new List<Producto>();
        public DevolucionFactura()
        {
            InitializeComponent();
            loadLtsFactura();
       
        }



        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            itemsNC.Clear();
            var newW = new windowAgregarNCFactura();
            newW.ShowDialog();


            try
            {

          
            if (newW.DialogResult == true)
            {
                idFactura = newW.idFactura;
                String totall = newW.txtTotal.Text;
                String subtotall = newW.txtSubtotal.Text;
                String insertNC = "INSERT INTO notacredito (total, subtotal,FK_idFactura) VALUES ( '" + totall + "', '"+ subtotall + "','" + idFactura + "')";
                conexion.operaciones(insertNC);

                string ultimoId = "Select last_insert_id()";
              lastid = conexion.ValorEnVariable(ultimoId);
           
                foreach (Producto p in newW.itemsNC)
                {
                    String nombre = p.nombre;
                    int cantidad = p.cantidad;
                    float totalp = p.total;
                    float precioUni = p.precioUnitario;
                    int idp = p.id;
    
                    Producto pr = new Producto(nombre, idp, cantidad, totalp, precioUni);
                    itemsNC.Add(p);

                   String productostNC = "INSERT INTO productos_has_notacredito (FK_idNotaCredito, FK_idProductos, cantidad, precioUnitario) VALUES ('" + lastid + "','" + idp + "', '" + cantidad + "', '" + precioUni + "')";
                    conexion.operaciones(productostNC);

                }

                dgvProductosNC.Items.Refresh();
                foreach (var producto in newW.itemsFact)
                {
                    String sql = "UPDATE productos_has_facturas SET CrNotaCredito = '" + producto.cantidad + "' where FK_idProducto = '" + producto.id + "' and FK_idfactura = '" + idFactura + "'";
                    conexion.operaciones(sql);
                }
            }
       
            

            }
            catch (NullReferenceException)
            {

            
            }

    
     
        }

        public void LoadDgvNC()
        {
            dgvProductosNC.ItemsSource = itemsNC;
        }
        public void loadLtsFactura()
        {
            String consulta = "select * from notacredito";
            conexion.Consulta(consulta, tabla: ltsNC);
            ltsNC.DisplayMemberPath = "idNotaCredito";
            ltsNC.SelectedValuePath = "idNotaCredito";
            ltsNC.SelectedIndex = 0;
        }

        public void loadLtsNC(int id)
        {
            String consulta = "select * from notacredito";
            conexion.Consulta(consulta, tabla: ltsNC);
            ltsNC.DisplayMemberPath = "idNotaCredito";
            ltsNC.SelectedValuePath = "idNotaCredito";
            ltsNC.SelectedIndex = id;
        }

        private void ltsNC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


          
      
            String productosFatura = "SELECT * FROM productos_has_notacredito WHERE FK_idNotaCredito ='" + ltsNC.SelectedValue + "'";

            productos = conexion.ConsultaParametrizada(productosFatura, ltsNC.SelectedValue);
            dgvProductosNC.ItemsSource = productos.AsDataView();

            String Fkf = "SELECT FK_idfactura FROM notacredito WHERE idNotaCredito = '" + ltsNC.SelectedValue + "' ";
            String fkfactura = conexion.ValorEnVariable(Fkf);

            String subt = "SELECT subtotal FROM notacredito WHERE idNotaCredito = '" + ltsNC.SelectedValue + "' ";
            String subtotal = conexion.ValorEnVariable(subt);


            String tot = "SELECT total FROM notacredito WHERE idNotaCredito = '" + ltsNC.SelectedValue + "' ";
            String total = conexion.ValorEnVariable(tot);

            String consulta2 = "SELECT * FROM factura f where f.idfacturas ='" + fkfactura + "'";
            DataTable OC = conexion.ConsultaParametrizada(consulta2, fkfactura);

            if (OC.Rows[0].ItemArray[4].ToString() == "0")
            {
                iva = "0";
            }
            else if (OC.Rows[0].ItemArray[4].ToString() == "1")
            {
                iva = "21";
            }
            else
            {
                iva = "10,5";
            }

            if (OC.Rows[0].ItemArray[5].ToString() == "0")
            {
                cambio = "$";
            }
            else if (OC.Rows[0].ItemArray[5].ToString() == "1")
            {
                cambio = "u$d";
            }
            else
            {
                cambio = "€";
            }

            txtSubtotal.Text = subtotal;
            txtTotal.Text = total;




            String idOC = "SELECT FK_idOC FROM factura WHERE idfacturas = '" + fkfactura + "' ";
            String idorc = conexion.ValorEnVariable(idOC);


            String idprov = "SELECT FK_idProveedor FROM ordencompra WHERE idOrdenCompra = '" + idorc + "' ";
            String idprove = conexion.ValorEnVariable(idprov);

            String nombreprove = "SELECT nombre FROM proveedor WHERE idProveedor = '" + idprove + "' ";
            String nombrepv = conexion.ValorEnVariable(nombreprove);

            txtProveedor.Text = nombrepv;



            txtIVA.Text = iva;
            txtTipoCambio.Text = cambio;

   



        }
        public void calculaTotal()
        {

            if (txtIVA.Text == "0")
            {

                txtTotal.Text = txtSubtotal.Text.ToString();
            }
            else if (txtIVA.Text == "21")
            {
                txtTotal.Text = (float.Parse(txtSubtotal.Text) * (float)1.21).ToString();

            }
            else if (txtIVA.Text == "10,5")
            {
                txtTotal.Text = (float.Parse(txtSubtotal.Text) * (float)1.105).ToString();

            }
        }

        private void btnModificarNC_Click(object sender, RoutedEventArgs e)
        {
            int idnotacredito = (int)ltsNC.SelectedValue;
            productosAmodificar.Clear();
            String iva = txtIVA.Text;
            String subtotal = txtSubtotal.Text;
            String cambio = txtTipoCambio.Text;
            String total = txtTotal.Text;

            String idf = "SELECT FK_idfactura FROM notacredito WHERE idNotaCredito = '" + ltsNC.SelectedValue + "'";
            String idFactura = conexion.ValorEnVariable(idf);

            String productosFatura = "SELECT DISTINCT t1.subtotal, t2.nombre ,t2.precioUnitario,t2.idProductos,t3.cantidad from productos_has_facturas t1, productos_has_notacredito t3 inner join productos t2 where t1.FK_idProducto = t2.idProductos and t1.FK_idfactura = '"+ idFactura + "' and  t3.FK_idNotaCredito = '"+ ltsNC.SelectedValue + "'";
            productos = conexion.ConsultaParametrizada(productosFatura, ltsNC.SelectedValue);
          

            for (int i = 0; i < productos.Rows.Count; i++)
            {
                producto = new Producto(productos.Rows[i].ItemArray[1].ToString(), (int)productos.Rows[i].ItemArray[3], (int)productos.Rows[i].ItemArray[4], (float)productos.Rows[i].ItemArray[0], (float)productos.Rows[i].ItemArray[2]);
                Console.WriteLine("nombre"+producto.nombre);
                Console.WriteLine("total" + producto.total);
                Console.WriteLine("cantidad" + producto.cantidad);
                Console.WriteLine("p unitario" + producto.precioUnitario);
                productosAmodificar.Add(producto);
          
            }



            var newW = new windowAgregarNCFactura(subtotal,total,iva,cambio,productosAmodificar, idFactura,idnotacredito);
            newW.Title = "Modificar Nota de Crédito";
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {

                String subm = newW.subtotalmodificar;
                String totalm = newW.totalmodificar;
                int idnotaCredito = newW.idnota;
                itemsNC.Clear();
                foreach (Producto p in newW.itemsNC)
                {
                    String nombre = p.nombre;
                    int cantidad = p.cantidad;
                    float totalp = p.total;
                    float precioUni = p.precioUnitario;
                    int idp = p.id;

                    Producto pr = new Producto(nombre, idp, cantidad, totalp, precioUni);
                    itemsNC.Add(p);

                    String updateNC = "UPDATE notacredito SET total =  '" + totalm + "', subtotal = '" + subm + "' WHERE idNotaCredito = '" + idnotaCredito + "'";
                    conexion.operaciones(updateNC);

                    String updateProductosNC = "UPDATE productos_has_notacredito SET FK_idNotaCredito = '" + idnotaCredito + "', FK_idProductos = '" + idp + "', cantidad = '" + cantidad + "', precioUnitario = '" + precioUni + "' WHERE FK_idNotaCredito = '" + idnotaCredito + "' AND FK_idProductos = '" + idp + "'";
                    conexion.operaciones(updateProductosNC);

                    foreach (var producto in newW.itemsFact)
                    {
                        String sql = "UPDATE productos_has_facturas SET CrNotaCredito = '" + producto.cantidad + "' where FK_idProducto = '" + producto.id + "' and FK_idfactura = '" + idFactura + "'";
                        conexion.operaciones(sql);
                    }
             

                }
            }


            }

        private void btnEliminarNC_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                MessageBoxResult dialog = MessageBox.Show("Esta seguro que desea eliminar la nota de crédito numero :" + ltsNC.SelectedValue, "Advertencia", MessageBoxButton.YesNo);


                if (dialog == MessageBoxResult.Yes)
                {
                    String idf = "SELECT FK_idfactura FROM notacredito WHERE idNotaCredito = '" + ltsNC.SelectedValue + "'";
                    String idFactura = conexion.ValorEnVariable(idf);

                    string sql2 = "DELETE  FROM notacredito WHERE idNotaCredito = '" + ltsNC.SelectedValue + "' AND FK_idfactura = '"+idFactura+"'";
                    conexion.operaciones(sql2);

                    string sql3 = " DELETE  FROM productos_has_notacredito WHERE FK_idNotaCredito =  '" + ltsNC.SelectedValue + "'";
                    conexion.operaciones(sql3);

                    txtIVA.Text = "";
                    txtSubtotal.Text = "";
                    txtTipoCambio.Text = "";
                    txtTotal.Text = "";
                    ltsNC.Items.Refresh();
                   // loadLtsFactura();
                 //   ltsNC.SelectedIndex = 0;
                }
           
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione una factura a eliminar");
            }
        }

      
    }
}
