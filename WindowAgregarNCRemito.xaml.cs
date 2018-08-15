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
using System.Windows.Shapes;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Lógica de interacción para WindowAgregarNCRemito.xaml
    /// </summary>
    public partial class WindowAgregarNCRemito : Window
    {

        CRUD conexion = new CRUD();
        DataTable productos;
        public List<Producto> productosparametro = new List<Producto>();
        public List<Producto> itemsNC = new List<Producto>();
        public List<Producto> itemsNCAntiguos = new List<Producto>();
        public int idRemito;
        public int idNotaCred;
        public int id;
        public String idOC;
        Boolean bandera = false;
        int tipo;
        public WindowAgregarNCRemito()
        {
            InitializeComponent();
            RbInterno.Visibility = Visibility.Collapsed;
            RbExterno.Visibility = Visibility.Collapsed;
            loadProductosRemitos();
            loadProductosNCRemitos();
            LoadDgvNCRemito();
            dgvProductosNCRemito.IsReadOnly = true;
            DgvProductosRemitos.IsReadOnly = true;
            LoadComboProveedor();
            seleccioneParaFiltrar();
            bandera = true;
            loadLtsRemitos();

        }

        //NC remitos desde salidas
        public WindowAgregarNCRemito(int tipo7)
        {
            InitializeComponent();
            RbInterno.Visibility = Visibility.Visible;
            RbExterno.Visibility = Visibility.Visible;
            tipo = tipo7;
     
            RbInterno.IsChecked = true;
            loadProductosRemitos();
            loadProductosNCRemitos();
            LoadDgvNCRemito();
            dgvProductosNCRemito.IsReadOnly = true;
            DgvProductosRemitos.IsReadOnly = true;
            LoadComboProveedorSalida();
            seleccioneParaFiltrar();
            bandera = true;
            loadLtsRemitosSalida();

        }

        public WindowAgregarNCRemito(List<Producto> ProdAmodificar, int idremito, int idNotaCredito)
        {
            InitializeComponent();
        
            loadProductosRemitos();
            itemsNC = ProdAmodificar;
            backupproductos(ProdAmodificar);
            LoadDgvNCRemito();
            ltsRemitos.IsEnabled = false;
            idNotaCred = idNotaCredito;
            cmbProveedores1.IsEnabled = false;
            txtnroremito.IsEnabled = false;
            loadProductosNCRemitos();
            dgvProductosNCRemito.IsReadOnly = true;
            DgvProductosRemitos.IsReadOnly = true;
            bandera = true;
            loadLtsRemitos(idremito);
            lblWindowTitle.Content = "Modificar Nota de Crédito";
        }

        private void backupproductos(List<Producto> productosNC)
        {
            foreach (Producto producto in productosNC)
            {
                itemsNCAntiguos.Add(producto);
            }
        }
        public WindowAgregarNCRemito(List<Producto> ProdAmodificar, int idremito, int idNotaCredito, int tipo8)
        {
            InitializeComponent();
            tipo = tipo8;
            if (tipo == 1)
            {
                RbInterno.IsChecked = true;
            }
            else
            {
                RbExterno.IsChecked = true;
            }
            loadProductosRemitos();
            itemsNC = ProdAmodificar;
            LoadDgvNCRemito();
            ltsRemitos.IsEnabled = false;
            idNotaCred = idNotaCredito;
            cmbProveedores1.IsEnabled = false;
            txtnroremito.IsEnabled = false;
            loadProductosNCRemitos();
            dgvProductosNCRemito.IsReadOnly = true;
            DgvProductosRemitos.IsReadOnly = true;
            bandera = true;
            loadLtsRemitosSalida(idremito);
            RbInterno.IsEnabled = false;
            RbExterno.IsEnabled = false;
            lblWindowTitle.Content = "Modificar Nota de Crédito";
            backupproductos(ProdAmodificar);
            RbInterno.Visibility = Visibility.Collapsed;
            RbExterno.Visibility = Visibility.Collapsed;
        }
        private void LoadComboProveedor()
        {
            String consulta4 = "SELECT nombre, idProveedor FROM proveedor";
            conexion.Consulta(consulta4, combo: cmbProveedores1);
            cmbProveedores1.DisplayMemberPath = "nombre";
            cmbProveedores1.SelectedValuePath = "idProveedor";
            cmbProveedores1.SelectedIndex = -1;

        }

        private void LoadComboProveedorSalida()
        {

            if (RbInterno.IsChecked == true)
            {
                String consulta4 = "SELECT nombre, idClientemi FROM clientesmi";
                conexion.Consulta(consulta4, combo: cmbProveedores1);
                cmbProveedores1.DisplayMemberPath = "nombre";
                cmbProveedores1.SelectedValuePath = "idClientemi";
                cmbProveedores1.SelectedIndex = -1;
            }
            else
            {
                String consulta4 = "SELECT nombre, idClienteme FROM clientesme";
                conexion.Consulta(consulta4, combo: cmbProveedores1);
                cmbProveedores1.DisplayMemberPath = "nombre";
                cmbProveedores1.SelectedValuePath = "idClienteme";
                cmbProveedores1.SelectedIndex = -1;
            }
        

        }
        public void loadLtsRemitos()
        {
            String consulta = "select * from remito";
            conexion.Consulta(consulta, tabla: ltsRemitos);
            ltsRemitos.DisplayMemberPath = "numeroRemito";
            ltsRemitos.SelectedValuePath = "idremitos";
            ltsRemitos.SelectedIndex = 0;
        }

        public void loadLtsRemitosSalida()
        {
            if (RbInterno.IsChecked == true)
            {
                String consulta = "select * from remitosalidas r, ordencomprasalida o WHERE r.FK_idOrdenCompra = o.idOrdenCompra AND o.FK_idClientemi IS NOT NULL";
                conexion.Consulta(consulta, tabla: ltsRemitos);
                ltsRemitos.DisplayMemberPath = "numeroRemito";
                ltsRemitos.SelectedValuePath = "idremitos";
                ltsRemitos.SelectedIndex = 0;
            }
            else
            {
                String consulta = "select * from remitosalidas r, ordencomprasalida o WHERE r.FK_idOrdenCompra = o.idOrdenCompra AND o.FK_idClienteme IS NOT NULL";
                conexion.Consulta(consulta, tabla: ltsRemitos);
                ltsRemitos.DisplayMemberPath = "numeroRemito";
                ltsRemitos.SelectedValuePath = "idremitos";
                ltsRemitos.SelectedIndex = 0;
            }
      
        }

        public void loadLtsRemitosSalida(int index)
        {
            String consulta = "select * from remitosalidas WHERE idremitos = '" + index + "' ";
            conexion.Consulta(consulta, tabla: ltsRemitos);
            ltsRemitos.DisplayMemberPath = "numeroRemito";
            ltsRemitos.SelectedValuePath = "idremitos";
            ltsRemitos.SelectedIndex = 0;
        }
        public void loadLtsRemitos(int idr)
        {
            String consulta = "select * from remito  WHERE idremitos = '" + idr + "' ";
            conexion.Consulta(consulta, tabla: ltsRemitos);
            ltsRemitos.DisplayMemberPath = "numeroRemito";
            ltsRemitos.SelectedValuePath = "idremitos";
            ltsRemitos.SelectedIndex = 0;
        }
        private void ltsRemitos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bandera == true)
            {
                if (RbInterno.IsChecked == true)
                {
                    //Datos Remito

                    String sql = "Select t3.nombre,t1.fecha ,t2.idOrdenCompra from remitosalidas t1, ordencomprasalida t2, clientesmi t3 where idremitos = '" + ltsRemitos.SelectedValue + "' and t1.FK_idOrdenCompra = t2.idOrdenCompra and t2.FK_idClientemi = t3.idClientemi";
                    DataTable datos = conexion.ConsultaParametrizada(sql, ltsRemitos.SelectedValue);

                    String sq = "Select fecha from remitosalidas where idremitos = '" + ltsRemitos.SelectedValue + "'";
                    String fecha = conexion.ValorEnVariable(sq).ToString();


                    txtfecha.Text = fecha;

                    //consulta productos
                    String consulta = "  SELECT t2.nombre , t1.CrNotaCredito, t2.idProductos from productos_has_remitossalida t1 inner join productos t2  on t1.FK_idProducto = t2.idProductos where t1.FK_idRemito = @valor";
                    productos = conexion.ConsultaParametrizada(consulta, ltsRemitos.SelectedValue);
                    productosparametro.Clear();
                    for (int i = 0; i < productos.Rows.Count; i++)
                    {
                        productosparametro.Add(new Producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[2], (int)productos.Rows[i].ItemArray[1]));
                    }

                    DgvProductosRemitos.Items.Refresh();
                } else if (RbExterno.IsChecked == true)
                {
                    //Datos Remito
                    
                    String sql = "Select t3.nombre,t1.fecha ,t2.idOrdenCompra from remitosalidas t1, ordencomprasalida t2, clientesme t3 where idremitos = '" + ltsRemitos.SelectedValue + "' and t1.FK_idOrdenCompra = t2.idOrdenCompra and t2.FK_idClienteme = t3.idClienteme";
                    DataTable datos = conexion.ConsultaParametrizada(sql, ltsRemitos.SelectedValue);


                    String sq = "Select fecha from remitosalidas where idremitos = '" + ltsRemitos.SelectedValue + "'";
                    String fecha = conexion.ValorEnVariable(sq).ToString();


                    txtfecha.Text = fecha;

                    //consulta productos
                    String consulta = "  SELECT t2.nombre , t1.CrNotaCredito, t2.idProductos from productos_has_remitossalida t1 inner join productos t2  on t1.FK_idProducto = t2.idProductos where t1.FK_idRemito = @valor";
                    productos = conexion.ConsultaParametrizada(consulta, ltsRemitos.SelectedValue);
                    productosparametro.Clear();
                    for (int i = 0; i < productos.Rows.Count; i++)
                    {
                        productosparametro.Add(new Producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[2], (int)productos.Rows[i].ItemArray[1]));
                    }

                    DgvProductosRemitos.Items.Refresh();
                }
                else
                {
                    //Datos Remito

                    String sql = "Select  t3.nombre,t1.fecha ,t2.idOrdenCompra from remito t1 , ordencompra t2, proveedor t3 where idremitos = '" + ltsRemitos.SelectedValue + "' and t1.FK_idOC = t2.idOrdenCompra and t2.FK_idProveedor = t3.idProveedor";
                    DataTable datos = conexion.ConsultaParametrizada(sql, ltsRemitos.SelectedValue);


                    String sq = "Select fecha from remito where idremitos = '" + ltsRemitos.SelectedValue + "'";
                    String fecha = conexion.ValorEnVariable(sq).ToString();


                    txtfecha.Text = fecha;

                    //consulta productos
                    String consulta = "  SELECT t2.nombre , t1.CrNotaCredito, t2.idProductos from productos_has_remitos t1 inner join productos t2  on t1.FK_idProducto = t2.idProductos where t1.FK_idRemito = @valor";
                    productos = conexion.ConsultaParametrizada(consulta, ltsRemitos.SelectedValue);
                    productosparametro.Clear();
                    for (int i = 0; i < productos.Rows.Count; i++)
                    {
                        productosparametro.Add(new Producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[2], (int)productos.Rows[i].ItemArray[1]));
                    }

                    DgvProductosRemitos.Items.Refresh();
                }
         
            }
        }
        private void loadProductosRemitos()
        {
            DgvProductosRemitos.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            DgvProductosRemitos.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Cantidad";
            textColumn2.Binding = new Binding("cantidad");
            DgvProductosRemitos.Columns.Add(textColumn2);
            DgvProductosRemitos.ItemsSource = productosparametro;
        }
        private void loadProductosNCRemitos()
        {
            dgvProductosNCRemito.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvProductosNCRemito.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Cantidad";
            textColumn2.Binding = new Binding("cantidad");
            dgvProductosNCRemito.Columns.Add(textColumn2);
           // dgvProductosNCRemito.ItemsSource = productosparametro;
        }
        private void btnAgregarProductoNC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool existe = false;
                Producto prod = DgvProductosRemitos.SelectedItem as Producto;

                id = prod.id;
                if (prod.cantidad > 0)
                {
                    var newW = new WindowAgregarProductoFactura();
                    for (int i = 0; i < itemsNC.Count; i++)
                    {
                        if (itemsNC[i].nombre == prod.nombre)
                        {
                            existe = true;
                        }
                        else
                        {
                            existe = false;
                        }
                    }
                    if (prod.cantidad >= 1 && !existe)
                    {

                        newW.txtCantidad.Text = prod.cantidad.ToString();
                        newW.can = prod.cantidad;
                        newW.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("E producto ya se agregó", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                    if (newW.DialogResult == true)
                    {
                        if (int.Parse(newW.txtCantidad.Text) > 0)
                        {

                            Producto productoremito = new Producto(prod.nombre, prod.id, int.Parse(newW.txtCantidad.Text));
                            itemsNC.Add(productoremito);
                            dgvProductosNCRemito.Items.Refresh();
                            prod.cantidad = prod.cantidad - int.Parse(newW.txtCantidad.Text);
                            DgvProductosRemitos.Items.Refresh();
                  
                        }
                        else
                        {
                            MessageBox.Show("La cantidad no puede ser cero", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ya se entregaron todas las ordenes de compra de este producto", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                   
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un producto para agregar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
              

            }
        }
        public void LoadDgvNCRemito()
        {
            dgvProductosNCRemito.ItemsSource = itemsNC;
        }
        public Boolean Valida()
        {

            if (dgvProductosNCRemito.HasItems == false)
            {

                MessageBox.Show("Agregue productos a la Nota de Crédito", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (DgvProductosRemitos.HasItems == false)
            {
                MessageBox.Show("Agregue productos a la Nota de Crédito", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                return true;
            }


        }
        private void btnEliminarProductoNC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Producto prod = dgvProductosNCRemito.SelectedItem as Producto;
                for (int i = 0; i < productosparametro.Count; i++)
                {
                    if (productosparametro[i].nombre == prod.nombre)
                    {
                        productosparametro[i].cantidad += prod.cantidad;
                    }

                }
                itemsNC.Remove(prod);
                dgvProductosNCRemito.Items.Refresh();
                DgvProductosRemitos.Items.Refresh();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un producto para eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    

            }
        }
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            idRemito = (int)ltsRemitos.SelectedValue;

            if (tipo == 1)
            {
                if (Valida() && VerEstadoSalida())
                {
                    DialogResult = true;
                }
            }
            else
            {
                if (Valida() && VerEstado())
                {
                    DialogResult = true;
                }
            }
            
      
        }

        public bool VerEstado()
        {

            String idOC = "SELECT FK_idOC FROM remito WHERE idremitos ='" + idRemito + "' ";
            this.idOC = conexion.ValorEnVariable(idOC);
            String estOC = "SELECT estadoNC FROM ordencompra WHERE idOrdenCompra ='" + conexion.ValorEnVariable(idOC) + "'";

            String estadoOC = conexion.ValorEnVariable(estOC);

            if (estadoOC == "2")
            {
                MessageBox.Show("No se puede agregar la Nota de Crédito porque ya tiene una Nota de Crédito de Factura.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            
                return false;
            }
            else
            {
                return true;
            }

        }
        public bool VerEstadoSalida()
        {

            String idOC = "SELECT FK_idOrdenCompra FROM remitosalidas WHERE idremitos ='" + idRemito + "' ";
            this.idOC = conexion.ValorEnVariable(idOC);
            String estOC = "SELECT estadoNC FROM ordencomprasalida WHERE idOrdenCompra ='" + conexion.ValorEnVariable(idOC) + "'";

            String estadoOC = conexion.ValorEnVariable(estOC);

            if (estadoOC == "2")
            {
                MessageBox.Show("No se puede agregar la Nota de Crédito porque ya tiene una Nota de Crédito de Factura.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
            else
            {
                return true;
            }

        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnVertodo_Click(object sender, RoutedEventArgs e)
        {
            bandera = false;

            if (tipo == 1)
            {
                String consulta = "select * from remitosalidas";
                conexion.Consulta(consulta, tabla: ltsRemitos);
                ltsRemitos.DisplayMemberPath = "numeroRemito";
                ltsRemitos.SelectedValuePath = "idremitos";

                ltsRemitos.SelectedIndex = 0;

                seleccioneParaFiltrar();
                RbInterno.IsChecked = false;
                RbExterno.IsChecked = false;
            }
            else
            {
                String consulta = "select * from remito";
                conexion.Consulta(consulta, tabla: ltsRemitos);
                ltsRemitos.DisplayMemberPath = "numeroRemito";
                ltsRemitos.SelectedValuePath = "idremitos";

                ltsRemitos.SelectedIndex = 0;

                seleccioneParaFiltrar();
            }
        
            bandera = true;
        }
        private void seleccioneParaFiltrar()
        {
            cmbProveedores1.Text = "Seleccione para filtrar";

        }
        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            if (bandera == true)
            {

                if (RbInterno.IsChecked == true)
                {
                    //String id = cmbProveedores1.SelectedValue.ToString();
                   // MessageBox.Show("id" + id);
                    bandera = false;
                    String sql = " select distinct r.numeroRemito, r.idremitos from remitosalidas r, ordencomprasalida o, clientesmi p where   o.FK_idClientemi IS NOT NULL and o.idOrdenCompra = r.FK_idOrdenCompra ";

                    conexion.Consulta(sql, tabla: ltsRemitos);
                    ltsRemitos.DisplayMemberPath = "numeroRemito";
                    ltsRemitos.SelectedValuePath = "idremitos";
                    bandera = true;
                  //  ltsRemitos.SelectedIndex = 1;
                    ltsRemitos.SelectedIndex = 0;
                }
                else if(RbExterno.IsChecked == true)
                {

                    //String id = cmbProveedores1.SelectedValue.ToString();
                   // MessageBox.Show("id" + id);
                    bandera = false;
                    String sql = " select distinct r.numeroRemito, r.idremitos from remitosalidas r, ordencomprasalida o, clientesme p where   o.FK_idClienteme IS NOT NULL and o.idOrdenCompra = r.FK_idOrdenCompra ";

                    conexion.Consulta(sql, tabla: ltsRemitos);
                    ltsRemitos.DisplayMemberPath = "numeroRemito";
                    ltsRemitos.SelectedValuePath = "idremitos";
                    bandera = true;
            //        ltsRemitos.SelectedIndex = 1;
                    ltsRemitos.SelectedIndex = 0;
                }
                else
                {
                    String id = cmbProveedores1.SelectedValue.ToString();

                    bandera = false;
                    String sql = " select distinct r.numeroRemito, r.idremitos from remito r, ordencompra o, proveedor p where   o.FK_idProveedor = '" + id + "' and o.idOrdenCompra = r.FK_idOC ";

                    conexion.Consulta(sql, tabla: ltsRemitos);
                    ltsRemitos.DisplayMemberPath = "numeroRemito";
                    ltsRemitos.SelectedValuePath = "idremitos";
                    bandera = true;
                    ltsRemitos.SelectedIndex = 1;
                    ltsRemitos.SelectedIndex = 0;
                }

               
                // LoadDgvNCRemito();
            }
        
        }
        private void txtnroremito_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

         
            if (RbExterno.IsChecked == true || RbInterno.IsChecked == true)
            {
                // Busquedas de remitos
                DataTable remitos = new DataTable();
                String consulta = "SELECT * FROM remitosalidas WHERE numeroRemito LIKE '%' @valor '%'";
                remitos = conexion.ConsultaParametrizada(consulta, txtnroremito.Text);
              
                ltsRemitos.ItemsSource = remitos.AsDataView();
                ltsRemitos.SelectedIndex = 0;

            }
            else
            {
                // Busquedas de remitos
                DataTable remitos = new DataTable();
                String consulta = "SELECT * FROM remito WHERE numeroRemito LIKE '%' @valor '%'";
                remitos = conexion.ConsultaParametrizada(consulta, txtnroremito.Text);
            
                ltsRemitos.ItemsSource = remitos.AsDataView();
                ltsRemitos.SelectedIndex = 0;

            }
            }
            catch (NullReferenceException)
            {

            
            }
        }

        private void RbInterno_Checked(object sender, RoutedEventArgs e)
        {
            String consulta = "SELECT DISTINCT p.nombre, p.idclientemi FROM clientesmi p ,ordencomprasalida o, remitosalidas r where o.FK_idClientemi = p.idClientemi and r.FK_idOrdenCompra = o.idOrdenCompra";
            conexion.Consulta(consulta, combo: cmbProveedores1);
           // ejecutar = false;
            cmbProveedores1.DisplayMemberPath = "nombre";
            cmbProveedores1.SelectedValuePath = "idClientemi";
           // ejecutar = true;
            cmbProveedores1.SelectedIndex = 0;
            itemsNC.Clear();
            dgvProductosNCRemito.Items.Refresh();
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
            itemsNC.Clear();
            dgvProductosNCRemito.Items.Refresh();
            // LoadListaComboProveedor();

        }
    }
}
