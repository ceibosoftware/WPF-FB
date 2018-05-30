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

namespace wpfFamiliaBlanco.Salidas.Devoluciones
{
    /// <summary>
    /// Lógica de interacción para DevolucionRemitoCliente.xaml
    /// </summary>
    public partial class DevolucionRemitoCliente : Page
    {
        CRUD conexion = new CRUD();
        List<Producto> productosparametro = new List<Producto>();
        List<Producto> itemsNC = new List<Producto>();
        List<Producto> itemsRemito = new List<Producto>();
        public List<Producto> productosAmodificar = new List<Producto>();
        String lastid;
        public Producto producto;
        DataTable productos;
        Boolean bandera = false;
        int tipo;
        public DevolucionRemitoCliente()
        {
            InitializeComponent();
            RbInterno.IsChecked = true;
            loadLtsNCRemitos();
            loadDGVproductosRemitos();
            loadLtsNCRemitos();
            loadProductosNCRemitos();
            dgvProductosRemito.IsReadOnly = true;
            LoadComboProveedor();
            seleccioneParaFiltrar();
            bandera = true;
            ltsRemitos.SelectedIndex = 0;
            txtfecha.IsReadOnly = true;
            txtproveedor.IsReadOnly = true;
            txtremito.IsReadOnly = true;
        }

        public void loadLtsNCRemitos()
        {
           
            String consulta = "select * from notacreditosalida WHERE FK_idremitos IS NOT NULL";
            conexion.Consulta(consulta, tabla: ltsRemitos);
            ltsRemitos.DisplayMemberPath = "idNotaCredito";
            ltsRemitos.SelectedValuePath = "idNotaCredito";
            ltsRemitos.SelectedIndex = -1;


        }

        public void loadDGVproductosRemitos()
        {
            dgvProductosRemito.ItemsSource = productosparametro;
            dgvProductosRemito.Items.Refresh();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new WindowAgregarNCRemito(1);
            newW.ShowDialog();

            DateTime dtp2 = System.DateTime.Now;
            if (newW.DialogResult == true)
            {

                String insertNCR = "INSERT INTO notacreditosalida (FK_idremitos, fecha) VALUES ('" + newW.idRemito + "','" + dtp2.ToString("yyyy/MM/dd") + "')";
                conexion.operaciones(insertNCR);

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
                    itemsNC.Add(pr);

                    String productostNCR = "INSERT INTO productos_has_notacreditosalida (FK_idNotaCredito, FK_idProductos, cantidad) VALUES ('" + lastid + "','" + idp + "', '" + cantidad + "')";
                    conexion.operaciones(productostNCR);

                }

                foreach (var producto in newW.productosparametro)
                {
                    String updateCRremito = "UPDATE productos_has_remitossalida SET CrNotaCredito = '" + producto.cantidad + "' where FK_idProducto = '" + producto.id + "' and FK_idRemito = '" + newW.idRemito + "'";
                    conexion.operaciones(updateCRremito);


                }

                foreach (var item in newW.itemsNC)
                {
                    String updatestock = "UPDATE productos SET stock = stock +'" + item.cantidad + "' where idProductos = '" + item.id + "'";
                    conexion.operaciones(updatestock);
                }
                String updateestadoOC = "UPDATE ordencomprasalida SET estadoNC = '" + 1 + "' where idOrdenCompra = '" + newW.idOC + "'";
                conexion.operaciones(updateestadoOC);
                loadLtsNCRemitos();
            }



        }

        private void ltsRemitos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {


                String productosFatura = "SELECT DISTINCT n.idProductoNC,n.FK_idNotaCredito,n.FK_idProductos,n.cantidad,n.precioUnitario, p.nombre FROM productos_has_notacreditosalida n,  productos p WHERE n.FK_idNotaCredito ='" + ltsRemitos.SelectedValue + "' AND n.FK_idProductos = p.idProductos";

                productos = conexion.ConsultaParametrizada(productosFatura, ltsRemitos.SelectedValue);
                dgvProductosRemito.ItemsSource = productos.AsDataView();
                productosparametro.Clear();
                for (int i = 0; i < productos.Rows.Count; i++)
                {
                    productosparametro.Add(new Producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[2], (int)productos.Rows[i].ItemArray[1]));
                }

                String consulta2 = "SELECT * FROM notacreditosalida n, remitosalidas r where n.idNotaCredito = '"+ ltsRemitos.SelectedValue + "'and n.FK_idremitos = r.idremitos";
                DataTable OC = conexion.ConsultaParametrizada(consulta2, ltsRemitos.SelectedValue);



                DateTime fecha = (DateTime)OC.Rows[0].ItemArray[3];
                txtfecha.Text = fecha.ToString("dd/MM/yyyy");


                //if (RbInterno.IsChecked == true)
                //{
                //    String c = " select p.nombre from remitosalidas r, ordencomprasalida o, clientesmi p, notacreditosalida n where r.FK_idOrdenCompra = o.idOrdenCompra and o.FK_idClientemi = p.idClientemi and r.idremitos = n.FK_idremitos and n.idNotacredito  = '" + ltsRemitos.SelectedValue + "'";
                //    String p = conexion.ValorEnVariable(c).ToString();

                //    txtproveedor.Text = p;
                //}
                //else
                //{
                //    String c = " select p.nombre from remitosalidas r, ordencomprasalida o, clientesme p, notacreditosalida n where r.FK_idOrdenCompra = o.idOrdenCompra and o.FK_idClienteme = p.idClienteme and r.idremitos = n.FK_idremitos and n.idNotacredito  = '" + ltsRemitos.SelectedValue + "'";
                //    String p = conexion.ValorEnVariable(c).ToString();

                //    txtproveedor.Text = p;
                //}
       
              txtremito.Text= OC.Rows[0].ItemArray[7].ToString() ;


            }
            catch (Exception)
            {


            }
        }
        private void loadProductosNCRemitos()
        {
            dgvProductosRemito.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvProductosRemito.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Cantidad";
            textColumn2.Binding = new Binding("cantidad");
            dgvProductosRemito.Columns.Add(textColumn2);
            dgvProductosRemito.ItemsSource = productosparametro;
        }
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {

            try
            {


                int idnotacredito = (int)ltsRemitos.SelectedValue;

                productosAmodificar.Clear();


                String idr = "SELECT FK_idremitos FROM notacreditosalida WHERE idNotaCredito = '" + idnotacredito + "'";
                String idRem = conexion.ValorEnVariable(idr);

                String productosRemito = "SELECT DISTINCT  t2.nombre,t2.idProductos,t3.cantidad from productos_has_remitossalida t1, productos_has_notacreditosalida t3 inner join productos t2 where t1.FK_idProducto = t2.idProductos and t1.FK_idRemito = '" + idRem + "' and  t3.FK_idNotaCredito = '" + ltsRemitos.SelectedValue + "'";
                productos = conexion.ConsultaParametrizada(productosRemito, ltsRemitos.SelectedValue);


                for (int i = 0; i < productos.Rows.Count; i++)
                {
                    producto = new Producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[1], (int)productos.Rows[i].ItemArray[2]);
                    productosAmodificar.Add(producto);


                }

     
            if (RbInterno.IsChecked == true)
            {
                tipo = 1;
            }
            else
            {
                tipo = 2;
            }
            var newW = new WindowAgregarNCRemito(productosAmodificar, int.Parse(idRem), idnotacredito,tipo);
                newW.Title = "Modificar Nota de Crédito";
                newW.ShowDialog();

                if (newW.DialogResult == true)
                {


                    int idRemito = newW.idRemito;
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



                        String updateProductosNC = "UPDATE productos_has_notacreditosalida SET FK_idNotaCredito = '" + newW.idNotaCred + "', FK_idProductos = '" + idp + "', cantidad = '" + cantidad + "' WHERE FK_idNotaCredito = '" + newW.idNotaCred + "' AND FK_idProductos = '" + idp + "'";
                        conexion.operaciones(updateProductosNC);

                        foreach (var producto in newW.productosparametro)
                        {
                            String sql = "UPDATE productos_has_remitossalida SET CrNotaCredito = '" + producto.cantidad + "' where FK_idProducto = '" + producto.id + "' and FK_idRemito = '" + idRemito + "'";
                            conexion.operaciones(sql);
                        }

                        //update stock
                        foreach (var item in newW.itemsNCAntiguos)
                        {
                          
                            String updatestock = "UPDATE productos SET stock = stock-'" + item.cantidad + "' where idProductos = '" + item.id + "'";
                            conexion.operaciones(updatestock);
                        }
                        //update stock
                        foreach (var item in newW.itemsNC)
                        {
                            String updatestock = "UPDATE productos SET stock = stock+ '" + item.cantidad + "' where idProductos = '" + item.id + "'";
                            conexion.operaciones(updatestock);
                        }


                    }
                    loadLtsNCRemitos();
                    MessageBox.Show("Se modificó correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione una nota de credito a modificar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
             
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRow selectedDataRow = ((DataRowView)ltsRemitos.SelectedItem).Row;
                String idremi = "";


                MessageBoxResult dialog = MessageBox.Show("¿Esta seguro que desea eliminar la nota de crédito numero :" + ltsRemitos.SelectedValue, "Advertencia", MessageBoxButton.YesNo,MessageBoxImage.Warning);


                if (dialog == MessageBoxResult.Yes)
                {

                    for (int i = 0; i < productos.Rows.Count; i++)
                    {
                        String idre = "SELECT FK_idremitos FROM notacreditosalida WHERE idNotaCredito = '" + ltsRemitos.SelectedValue + "'";
                        idremi = conexion.ValorEnVariable(idre);

                        String consulta = "UPDATE productos_has_remitossalida SET CrNotaCredito = CrNotaCredito + '" + (int)productos.Rows[i].ItemArray[3] + "' where FK_idProducto = '" + (int)productos.Rows[i].ItemArray[2] + "' and FK_idRemito = '" + idremi + "'";
                        conexion.operaciones(consulta);


                        String updatestock = "UPDATE productos SET stock = stock -'" + (int)productos.Rows[i].ItemArray[3] + "' where idProductos = '" + (int)productos.Rows[i].ItemArray[2] + "'";
                        conexion.operaciones(updatestock);
                    }


                    string sql2 = "DELETE  FROM notacreditosalida WHERE idNotaCredito = '" + ltsRemitos.SelectedValue + "' AND FK_idremitos = '" + idremi + "'";
                    conexion.operaciones(sql2);

                    string sql3 = " DELETE  FROM productos_has_notacreditosalida WHERE FK_idNotaCredito =  '" + ltsRemitos.SelectedValue + "'";
                    conexion.operaciones(sql3);
                    String idOC = "SELECT FK_idOrdenCompra FROM remitosalidas WHERE idremitos ='" + idremi + "' ";
                    String idOrden = conexion.ValorEnVariable(idOC);
                    String sql = "SELECT COUNT(FK_idremitos) FROM notacredito WHERE FK_idfactura = '" + idremi + "' ";
                    if (conexion.ValorEnVariable(sql) == "0")
                    {
                        String updateestadoOC = "UPDATE ordencomprasalida SET estadoNC = '" + 0 + "' where idOrdenCompra = '" + idOrden + "'";
                        conexion.operaciones(updateestadoOC);
                    }
                    ltsRemitos.Items.Refresh();
                    loadLtsNCRemitos();
                    ltsRemitos.SelectedIndex = 0;
                    MessageBox.Show("SSe eliminó correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                if (ltsRemitos.Items.Count <= 0)
                {
                    txtfecha.Text = "";
                    txtnroremito.Text = "";
                    txtproveedor.Text = "";
                }

            }
            catch (NullReferenceException)
            {

          MessageBox.Show("Seleccione una Nota de Crédito a eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
               
            }
        }

        private void dpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                String consulta = " Select * from notacreditosalida t1, remitosalidas t2  where  t2.fecha = @valor and t1.FK_idremitos = t2.idremitos  ";
                DataTable OCFecha = conexion.ConsultaParametrizada(consulta, dpFecha.SelectedDate);
                ltsRemitos.ItemsSource = OCFecha.AsDataView();
                ltsRemitos.DisplayMemberPath = "idNotaCredito";
                ltsRemitos.SelectedValuePath = "idNotaCredito";
                ltsRemitos.SelectedIndex = 0;
            }
            catch (NullReferenceException)
            {


            }
        }

        private void btnVertodo_Click(object sender, RoutedEventArgs e)
        {
            RbInterno.IsChecked = false;
            RbExterno.IsChecked = false;
            String consulta = "select * from notacreditosalida  WHERE FK_idremitos IS NOT NULL";
            conexion.Consulta(consulta, tabla: ltsRemitos);
            ltsRemitos.DisplayMemberPath = "idNotaCredito";
            ltsRemitos.SelectedValuePath = "idNotaCredito";
            ltsRemitos.SelectedIndex = 0;
            bandera = false;
            seleccioneParaFiltrar();
            bandera = true;
        }

        private void LoadComboProveedor()
        {


            if (RbInterno.IsChecked == true)
            {
                String consulta4 = "SELECT nombre, idClientemi FROM clientesmi";
                conexion.Consulta(consulta4, combo: cmbProveedores1);
                cmbProveedores1.DisplayMemberPath = "nombre";
                cmbProveedores1.SelectedValuePath = "idClientemi";
                cmbProveedores1.SelectedIndex = 0;
            }
            else
            {
                String consulta4 = "SELECT nombre, idClienteme FROM clientesme";
                conexion.Consulta(consulta4, combo: cmbProveedores1);
                cmbProveedores1.DisplayMemberPath = "nombre";
                cmbProveedores1.SelectedValuePath = "idClienteme";
                cmbProveedores1.SelectedIndex = 0;
            }

          

        }

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

          
            if (bandera == true)
            {

                if (RbInterno.IsChecked == true)
                {
                    String id = cmbProveedores1.SelectedValue.ToString();

                    String sql = "select distinct n.idNotaCredito from remitosalidas r, ordencomprasalida o, clientesmi p, notacreditosalida n where   o.FK_idClientemi = '" + id + "' and o.idOrdenCompra = r.FK_idOrdenCompra   and n.FK_idremitos  = r.idremitos";
                    conexion.Consulta(sql, ltsRemitos);
                    ltsRemitos.DisplayMemberPath = "idNotaCredito";
                    ltsRemitos.SelectedValuePath = "idNotaCredito";
                    ltsRemitos.SelectedIndex = 0;
                }
                else
                {
                    String id = cmbProveedores1.SelectedValue.ToString();
                
                    String sql = "select distinct n.idNotaCredito from remitosalidas r, ordencomprasalida o, clientesme p, notacreditosalida n where   o.FK_idClienteme = '" + id + "' and o.idOrdenCompra = r.FK_idOrdenCompra   and n.FK_idremitos  = r.idremitos";
                    conexion.Consulta(sql, ltsRemitos);
                    ltsRemitos.DisplayMemberPath = "idNotaCredito";
                    ltsRemitos.SelectedValuePath = "idNotaCredito";
                    ltsRemitos.SelectedIndex = 0;
                }
         
                // loadDGVproductosRemitos();
            }

            }
            catch (NullReferenceException)
            {

                
            }

        }

        private void seleccioneParaFiltrar()
        {
            cmbProveedores1.Text = "Seleccione para filtrar";

        }

        private void txtnroremito_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Busquedas de productos.
            DataTable productos = new DataTable();
            String consulta;
            consulta = "SELECT * FROM notacreditosalida WHERE idNotaCredito LIKE '%' @valor '%' and FK_idremitos IS NOT NULL";
            productos = conexion.ConsultaParametrizada(consulta, txtnroremito.Text);
            ltsRemitos.ItemsSource = productos.AsDataView();



        }

        private void RbInterno_Checked(object sender, RoutedEventArgs e)
        {
            String consulta = "SELECT DISTINCT p.nombre, p.idClientemi FROM clientesmi p ,ordencomprasalida o, remitosalidas r where o.FK_idClientemi = p.idClientemi and r.FK_idOrdenCompra = o.idOrdenCompra";
            conexion.Consulta(consulta, combo: cmbProveedores1);
         //   ejecutar = false;
            cmbProveedores1.DisplayMemberPath = "nombre";
            cmbProveedores1.SelectedValuePath = "idClientemi";
           // ejecutar = true;
            cmbProveedores1.SelectedIndex = 0;
            // loadLtsRemitos();
            // LoadListaComboProveedor();
            Console.WriteLine("id" + cmbProveedores1.SelectedValue);
        }

        private void RbExterno_Checked(object sender, RoutedEventArgs e)
        {
            String consulta = "SELECT DISTINCT p.nombre, p.idClienteme FROM clientesme p ,ordencomprasalida o, remitosalidas r where o.FK_idClienteme = p.idClienteme and r.FK_idOrdenCompra = o.idOrdenCompra";
            conexion.Consulta(consulta, combo: cmbProveedores1);
         //   ejecutar = false;
            cmbProveedores1.DisplayMemberPath = "nombre";
            cmbProveedores1.SelectedValuePath = "idClienteme";
          //  ejecutar = true;
            cmbProveedores1.SelectedIndex = 0;
            //  LoadListaComboProveedor();
            // loadLtsRemitos();
            Console.WriteLine("id" + cmbProveedores1.SelectedValue);
        }
    }
}

