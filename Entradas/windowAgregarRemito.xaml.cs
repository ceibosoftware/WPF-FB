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

namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Interaction logic for windowAgregarRemito.xaml
    /// </summary>
    public partial class windowAgregarRemito : Window
    {
       public int idRemito;
        bool ejecuta = true;
        bool modifica1 = false;
        bool modifica = false;
        int id;
        private List<Producto> prodRemito = new List<Producto>();
        private List<Producto> productos = new List<Producto>();
        private List<Producto> prodRemitoStock = new List<Producto>();
        CRUD conexion = new CRUD();
        public List<Producto> ProdRemito { get => prodRemito; set => prodRemito = value; }
        public List<Producto> Productos { get => productos; set => productos = value; }
        public List<Producto> ProdRemitoStock { get => prodRemitoStock; set => prodRemitoStock = value; }
        int tipo;
        public windowAgregarRemito()
        {
            InitializeComponent();
            RbExterno.Visibility = Visibility.Collapsed;
            RbInterno.Visibility = Visibility.Collapsed;
            loadcmbProveedores();
            loadDgvProd();
            loadDgvProdRemito();
            dtRemito.SelectedDate = DateTime.Now;
            txtNroRemito.MaxLength = 18;
            dgvProductosOC.IsReadOnly = true;
            dgvProductosRemito.IsReadOnly = true;

        }
        //AGREGAR REMITO DESDE SALIDAS
        public windowAgregarRemito(int tipo1)
        {
            InitializeComponent();
            RbExterno.Visibility = Visibility.Visible;
            RbInterno.Visibility = Visibility.Visible;
            RbInterno.IsChecked = true;
            tipo = tipo1;
            loadcmbProveedoresSalida();
            loadDgvProd();
            loadDgvProdRemito();
          //  loadCmbOrdenesSalida();
            dtRemito.SelectedDate = DateTime.Now;
            txtNroRemito.MaxLength = 10;
            dgvProductosOC.IsReadOnly = true;
            dgvProductosRemito.IsReadOnly = true;

        }
        //Agregar remito desde OC
        public windowAgregarRemito(int proveedor, int numeroOC)
        {
            InitializeComponent();
            RbExterno.Visibility = Visibility.Collapsed;
            RbInterno.Visibility = Visibility.Collapsed;
            loadcmbProveedores();
            loadDgvProd();
            loadDgvProdRemito();
     
            loadcmbProveedores(proveedor);
            loadCmbOrdenes(numeroOC);
            dtRemito.SelectedDate = DateTime.Now;
            txtNroRemito.MaxLength = 18;
            dgvProductosOC.IsReadOnly = true;
            dgvProductosRemito.IsReadOnly = true;

        }
        //MODIFICAR
        public windowAgregarRemito(int proveedor, int numeroOC, List<Producto> productosRemito,DateTime fecha, string numeroRemito, int idRemito)
        {

            InitializeComponent();
            modifica1 = true;
            RbExterno.Visibility = Visibility.Collapsed;
            RbInterno.Visibility = Visibility.Collapsed;
            loadCmbOrdenes(numeroOC);
            loadDgvProd();
            prodRemito = productosRemito;
            backupStock(productosRemito);
            loadDgvProdRemito(prodRemito);
            loadProductosOC(numeroOC);
            loadFechaEmision();
            dtRemito.SelectedDate =fecha ;
            txtNroRemito.Text = numeroRemito;
            this.idRemito = idRemito;      
            cmbProveedores.IsEnabled = false;
            cmbOrden.IsEnabled = false;
            txtFiltro.IsEnabled = false;
            cmbFechas.IsEnabled = false;
            ejecuta = false;
            loadcmbProveedores();
            ejecuta = true;
            dgvProductosOC.IsReadOnly = true;
            dgvProductosRemito.IsReadOnly = true;
            modifica = true;
            //cambios diseño batta
            lblWindowTitle.Content = "Modificar Remito";

        }

        //MODIFICAR DESDE SALIDAS
        public windowAgregarRemito(int proveedor, int numeroOC, List<Producto> productosRemito, DateTime fecha, string numeroRemito, int idRemito, int tipo6)
        {
            InitializeComponent();
            tipo = tipo6;
            if (tipo == 1)
            {
                RbInterno.IsChecked = true;
            }
            else
            {
                RbExterno.IsChecked = true;
            }
  
            loadCmbOrdenesSalida(numeroOC);
            loadDgvProd();
            prodRemito = productosRemito;
            backupStock(productosRemito);
            loadDgvProdRemito(prodRemito);
            loadProductosOCSalida(numeroOC);
            loadFechaEmisionSalida();
            dtRemito.SelectedDate = fecha;
            txtNroRemito.Text = numeroRemito;
            this.idRemito = idRemito;
            cmbProveedores.IsEnabled = false;
            cmbOrden.IsEnabled = false;
            txtFiltro.IsEnabled = false;
            cmbFechas.IsEnabled = false;
            ejecuta = false;
            loadcmbProveedoresSalida();
            ejecuta = true;
            dgvProductosOC.IsReadOnly = true;
            dgvProductosRemito.IsReadOnly = true;
            RbInterno.IsEnabled = false;
            RbExterno.IsEnabled = false;
            //cambios diseño batta
            lblWindowTitle.Content = "Modificar Remito";

        }
        private void backupStock(List<Producto> productosRemito)
        {
            foreach (Producto producto in productosRemito)
            {
                prodRemitoStock.Add(producto);
            }
        }
        /*public void fechas()
        {
            String consulta = "Select distinct DATE_FORMAT(fecha, '%d-%m-%Y') AS fecha from ordencompra";
            conexion.Consulta(consulta, combo: cmbFechas);
            cmbFechas.DisplayMemberPath = "fecha";
            cmbFechas.SelectedValue = "fecha";
            cmbFechas.SelectedIndex = 0;
        }*/
        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ejecuta)
            {
                prodRemito.Clear();
                dgvProductosRemito.Items.Refresh();

                if (RbInterno.IsChecked == true)
                {
                    try
                    {

                        ejecuta = false;
                        loadCmbOrdenesSalida();
                       // loadFechaEmision();
                        ejecuta = true;
                    }
                    catch (Exception)
                    {


                    }
                }
                else if(RbExterno.IsChecked == true)
                {
                    try
                    {

                        ejecuta = false;
                        loadCmbOrdenesSalida();
                       // loadFechaEmision();
                        ejecuta = true;
                    }
                    catch (Exception)
                    {


                    }
                }
                else
                {
                    try
                    {

                        ejecuta = false;
                        loadCmbOrdenes();
                        loadFechaEmision();
                        ejecuta = true;
                    }
                    catch (Exception)
                    {


                    }
                }
               
            }
        }
        public void loadDgvProd() {
            dgvProductosOC.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvProductosOC.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Cantidad";
            textColumn2.Binding = new Binding("cantidad");
            dgvProductosOC.Columns.Add(textColumn2);
            dgvProductosOC.ItemsSource = productos;
        }
        public void loadDgvProdRemito()
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
            dgvProductosRemito.ItemsSource = prodRemito;
            
        }
        public void loadDgvProdRemito(List<Producto> prodRemito)
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
            dgvProductosRemito.ItemsSource = prodRemito;
            //dgvProductosRemito.Columns[1].Visibility = Visibility.Collapsed;
        }

        public void loadcmbProveedores()
        {

            String consulta = "SELECT DISTINCT p.nombre, p.idProveedor FROM proveedor p inner join ordencompra o where o.FK_idProveedor = p.idProveedor ";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
            cmbProveedores.SelectedIndex = 0;
            

        }

        public void loadcmbProveedoresSalida()
        {

            if (RbInterno.IsChecked == true)
            {
                String consulta = "SELECT DISTINCT p.nombre, p.idClientemi FROM clientesmi p inner join ordencomprasalida o where o.FK_idClientemi = p.idClientemi ";
                conexion.Consulta(consulta, combo: cmbProveedores);
                cmbProveedores.DisplayMemberPath = "nombre";
                cmbProveedores.SelectedValuePath = "idClientemi";
                cmbProveedores.SelectedIndex = 0;
                loadCmbOrdenesSalida();
            }
            else
            {
                String consulta = "SELECT DISTINCT p.nombre, p.idClienteme FROM clientesme p inner join ordencomprasalida o where o.FK_idClienteme = p.idClienteme";
                conexion.Consulta(consulta, combo: cmbProveedores);
                cmbProveedores.DisplayMemberPath = "nombre";
                cmbProveedores.SelectedValuePath = "idClienteme";
                cmbProveedores.SelectedIndex = 0;
                loadCmbOrdenesSalida();
            }



        }

        public void loadcmbProveedores(int proveedor)
        {
            ejecuta = false;
            String consulta = "SELECT DISTINCT p.nombre, p.idProveedor FROM proveedor p inner join ordencompra o where o.FK_idProveedor = p.idProveedor ";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
            cmbProveedores.SelectedValue= proveedor;

            ejecuta = true;
        }

        private void cmbFechas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ejecuta)
            {
                String consulta = " Select * from ordencompra t1 where t1.fecha = @valor and FK_idProveedor = '" + cmbProveedores.SelectedValue + "'";
                DateTime fecha = new DateTime();
                DateTime.TryParse(cmbFechas.SelectedValue.ToString(), out fecha);
                fecha.ToString("yyyy-MM-dd");
                DataTable OCFecha = conexion.ConsultaParametrizada(consulta, fecha);
                cmbOrden.ItemsSource = OCFecha.AsDataView();
                cmbOrden.DisplayMemberPath = "idOrdenCompra";
                cmbOrden.SelectedValuePath = "idOrdenCompra";
                cmbOrden.SelectedIndex = 0;
            }
        }

        private void cmbOrden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RbInterno.IsChecked == true)
            {
                loadProductosOCSalida();
          
            }
            else if (RbExterno.IsChecked == true)
            {
                loadProductosOCSalida();
             
            }
            else
            {
                loadProductosOC();
          
            }
 

        }

        private void btnProdAgregar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                bool existe = false;
                Producto prod = dgvProductosOC.SelectedItem as Producto;
                if (prod.cantidad > 0)
                {
                    var newW = new WindowAgregarProductoFactura();
                    for (int i = 0; i < prodRemito.Count; i++)
                    {
                        if (prodRemito[i].nombre == prod.nombre)
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
                        MessageBox.Show("El producto ya se agrego", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                    }


                    if (newW.DialogResult == true)
                    {
                        if (int.Parse(newW.txtCantidad.Text) > 0)
                        {
                            Producto productoremito = new Producto(prod.nombre, prod.id, int.Parse(newW.txtCantidad.Text));
                            prodRemito.Add(productoremito);
                            dgvProductosRemito.Items.Refresh();
                            prod.cantidad = prod.cantidad - int.Parse(newW.txtCantidad.Text);
                            dgvProductosOC.Items.Refresh();
                        }
                        else
                        {
                            MessageBox.Show("La cantidad no puede ser cero", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                          
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ya se entregaron todos los remitos de este producto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                  
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un producto para agregar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
     
                
            }
          
        }

        private void btnProdEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Producto prod = dgvProductosRemito.SelectedItem as Producto;
                if (modifica)
                {
                  
                    String en = "SELECT fecha FROM remito WHERE idremitos = '" + idRemito + "'";
                    DateTime fechaRemito = DateTime.Parse(conexion.ValorEnVariable(en));

                    String s = "SELECT ums FROM productos WHERE idProductos = '" + prod.id + "'";
                    DateTime fechaUMS = DateTime.Parse(conexion.ValorEnVariable(s));

                    TimeSpan ts = fechaRemito - fechaUMS;

                    // Difference in days.
                    int differenceInDays = ts.Days;
                    






















                     
                    if (differenceInDays <= 0)
                    {
                        MessageBox.Show("No se puede modificar el remito porque ya se ha realizado una venta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                  
                    }
                    else
                    {
                        for (int i = 0; i < productos.Count; i++)
                        {
                            if (productos[i].nombre == prod.nombre)
                            {
                                productos[i].cantidad += prod.cantidad;
                            }

                        }
                        prodRemito.Remove(prod);
                        dgvProductosRemito.Items.Refresh();
                        dgvProductosOC.Items.Refresh();
                    }
                }
                else
                {
                    for (int i = 0; i < productos.Count; i++)
                    {
                        if (productos[i].nombre == prod.nombre)
                        {
                            productos[i].cantidad += prod.cantidad;
                        }

                    }
                    prodRemito.Remove(prod);
                    dgvProductosRemito.Items.Refresh();
                    dgvProductosOC.Items.Refresh();
                }
            }
               
            
            
           
              
            
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un producto para eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        

            }

        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Busquedas de proveedor.
            DataTable productos = new DataTable();
            String consulta;
            consulta = "SELECT  DISTINCT p.nombre, p.idProveedor FROM proveedor p inner join ordencompra o where p.nombre LIKE '%' @valor '%' and o.FK_idProveedor = p.idProveedor ";
            productos = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            cmbProveedores.ItemsSource = productos.AsDataView();
            cmbProveedores.SelectedIndex = 0;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (validar()) {
                DialogResult = true;
            }
        
        }

        public void loadCmbOrdenes()
        {
            String consulta = " Select * from ordencompra t1 where t1.FK_idProveedor = @valor ";
            DataTable OCProveedor = conexion.ConsultaParametrizada(consulta, cmbProveedores.SelectedValue);
            cmbOrden.ItemsSource = OCProveedor.AsDataView();
            cmbOrden.DisplayMemberPath = "idOrdenCompra";
            cmbOrden.SelectedValuePath = "idOrdenCompra";
            cmbOrden.SelectedIndex = 0;
        }


        public void loadCmbOrdenesSalida()
        {

            if (RbInterno.IsChecked == true)
            {
                String consulta = " Select * from ordencomprasalida t1 where t1.FK_idClientemi = @valor ";
                DataTable OCProveedor = conexion.ConsultaParametrizada(consulta, cmbProveedores.SelectedValue);
                cmbOrden.ItemsSource = OCProveedor.AsDataView();
                cmbOrden.DisplayMemberPath = "idOrdenCompra";
                cmbOrden.SelectedValuePath = "idOrdenCompra";
                cmbOrden.SelectedIndex = -1;
                cmbOrden.SelectedIndex = 0;
            }
            else
            {
                String consulta = " Select * from ordencomprasalida t1 where t1.FK_idClienteme = @valor ";
                DataTable OCProveedor = conexion.ConsultaParametrizada(consulta, cmbProveedores.SelectedValue);
                cmbOrden.ItemsSource = OCProveedor.AsDataView();
                cmbOrden.DisplayMemberPath = "idOrdenCompra";
                cmbOrden.SelectedValuePath = "idOrdenCompra";
                cmbOrden.SelectedIndex = -1;
                cmbOrden.SelectedIndex = 0;
            }
      
        }
        public void loadCmbOrdenes(int orden)
        {
            String consulta = " Select idOrdenCompra from ordencompra ";
            conexion.Consulta(consulta, combo: cmbOrden);
            cmbOrden.DisplayMemberPath = "idOrdenCompra";
            cmbOrden.SelectedValuePath = "idOrdenCompra";
  
            for (int i = 0; i < cmbOrden.Items.Count; i++)
            {
                cmbOrden.SelectedIndex = i;

                if (cmbOrden.SelectedValue.ToString() == orden.ToString())
                {
                    break;
                }
            }
               
        }

        public void loadCmbOrdenesSalida(int orden)
        {
            String consulta = " Select idOrdenCompra from ordencomprasalida ";
            conexion.Consulta(consulta, combo: cmbOrden);
            cmbOrden.DisplayMemberPath = "idOrdenCompra";
            cmbOrden.SelectedValuePath = "idOrdenCompra";

            for (int i = 0; i < cmbOrden.Items.Count; i++)
            {
                cmbOrden.SelectedIndex = i;

                if (cmbOrden.SelectedValue.ToString() == orden.ToString())
                {
                    break;
                }
            }

        }
        public void loadFechaEmision()
        {
            String sql = "   Select distinct DATE_FORMAT(t1.fecha, '%d-%m-%Y') AS fecha from ordencompra t1 where t1.FK_idProveedor = @valor ";
            DataTable fechas = conexion.ConsultaParametrizada(sql, cmbProveedores.SelectedValue);
            cmbFechas.ItemsSource = fechas.AsDataView();
            cmbFechas.DisplayMemberPath = "fecha";
            cmbFechas.SelectedValuePath = "fecha";
            cmbFechas.SelectedIndex = 0;
        }

        public void loadFechaEmisionSalida()
        {

            if (RbInterno.IsChecked == true)
            {
                String sql = "   Select distinct DATE_FORMAT(t1.fecha, '%d-%m-%Y') AS fecha from ordencomprasalida t1 where t1.FK_idClientemi = @valor ";
                DataTable fechas = conexion.ConsultaParametrizada(sql, cmbProveedores.SelectedValue);
                cmbFechas.ItemsSource = fechas.AsDataView();
                cmbFechas.DisplayMemberPath = "fecha";
                cmbFechas.SelectedValuePath = "fecha";
                cmbFechas.SelectedIndex = 0;
            }
            else
            {
                String sql = "   Select distinct DATE_FORMAT(t1.fecha, '%d-%m-%Y') AS fecha from ordencomprasalida t1 where t1.FK_idClienteme = @valor ";
                DataTable fechas = conexion.ConsultaParametrizada(sql, cmbProveedores.SelectedValue);
                cmbFechas.ItemsSource = fechas.AsDataView();
                cmbFechas.DisplayMemberPath = "fecha";
                cmbFechas.SelectedValuePath = "fecha";
                cmbFechas.SelectedIndex = 0;
            }
   
        }

        public void loadProductosOC()
        {
            productos.Clear();
            string consulta = "SELECT t2.idProductos, t1.CrRemito ,t2.nombre FROM productos_has_ordencompra t1 inner join productos t2 where FK_idOC = @valor and t1.FK_idProducto = t2.idProductos";
            DataTable prod = conexion.ConsultaParametrizada(consulta, cmbOrden.SelectedValue);
            
            for (int i = 0; i < prod.Rows.Count; i++)
            {
                int idProductos = (int)prod.Rows[i].ItemArray[0];
                int cantidad = (int)prod.Rows[i].ItemArray[1];           
                string nombre = prod.Rows[i].ItemArray[2].ToString();
                Producto produc = new Producto(nombre, idProductos, cantidad);
                productos.Add(produc);
            }
 
            dgvProductosOC.Items.Refresh();
            if (modifica1 == false)
            {
                if ((int)cmbOrden.SelectedValue < 10)
                {
                    txtNroRemito.Text = "000" + cmbOrden.SelectedValue.ToString() + "-" + "0000";
                }
                else
                {
                    txtNroRemito.Text = "00" + cmbOrden.SelectedValue.ToString() + "-" + "0000";
                }
            }
       
        }

        public void loadProductosOCSalida()
        {
            productos.Clear();
            string consulta = "SELECT t2.idProductos, t1.CrRemito ,t2.nombre FROM productos_has_ordencomprasalida t1 inner join productos t2 where FK_idOrdenCompra = @valor and t1.FK_idProducto = t2.idProductos";
            DataTable prod = conexion.ConsultaParametrizada(consulta, cmbOrden.SelectedValue);

            for (int i = 0; i < prod.Rows.Count; i++)
            {
                int idProductos = (int)prod.Rows[i].ItemArray[0];
                int cantidad = (int)prod.Rows[i].ItemArray[1];
                string nombre = prod.Rows[i].ItemArray[2].ToString();
                Producto produc = new Producto(nombre, idProductos, cantidad);
                productos.Add(produc);
            }

            dgvProductosOC.Items.Refresh();
        }
        public void loadProductosOC(int Orden)
        {
            productos.Clear();
            string consulta = "SELECT t2.idProductos, t1.CrRemito ,t2.nombre FROM productos_has_ordencompra t1 inner join productos t2 where FK_idOC = @valor and t1.FK_idProducto = t2.idProductos";
            DataTable prod = conexion.ConsultaParametrizada(consulta, Orden);
            for (int i = 0; i < prod.Rows.Count; i++)
            {
                int idProductos = (int)prod.Rows[i].ItemArray[0];
                int cantidad = (int)prod.Rows[i].ItemArray[1];
                string nombre = prod.Rows[i].ItemArray[2].ToString();

                productos.Add(new Producto(nombre, idProductos, cantidad));
            }
  
          
            dgvProductosOC.Items.Refresh();
        }

        public void loadProductosOCSalida(int Orden)
        {
            productos.Clear();
            string consulta = "SELECT t2.idProductos, t1.CrRemito ,t2.nombre FROM productos_has_ordencomprasalida t1 inner join productos t2 where FK_idOrdenCompra = @valor and t1.FK_idProducto = t2.idProductos";
            DataTable prod = conexion.ConsultaParametrizada(consulta, Orden);
            for (int i = 0; i < prod.Rows.Count; i++)
            {
                int idProductos = (int)prod.Rows[i].ItemArray[0];
                int cantidad = (int)prod.Rows[i].ItemArray[1];
                string nombre = prod.Rows[i].ItemArray[2].ToString();

                productos.Add(new Producto(nombre, idProductos, cantidad));
            }


            dgvProductosOC.Items.Refresh();
        }
        public bool validar()
        {

            if (prodRemito.Count <= 0)
            {
                MessageBox.Show("Ingrese al menos un producto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
              
                return false;
            }
            else if (string.IsNullOrEmpty(dtRemito.Text))
            {
                MessageBox.Show("Seleccione una fecha", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            
                return false;
            }
            else if (string.IsNullOrEmpty(txtNroRemito.Text))
            {
                MessageBox.Show("Ingrese numero remito", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
              
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

        private void txtNroRemito_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void RbInterno_Checked(object sender, RoutedEventArgs e)
        {
            String consulta = "SELECT DISTINCT p.nombre, p.idClientemi FROM clientesmi p inner join ordencomprasalida o where o.FK_idClientemi = p.idClientemi ";
                conexion.Consulta(consulta, combo: cmbProveedores);
                cmbProveedores.DisplayMemberPath = "nombre";
                cmbProveedores.SelectedValuePath = "idClientemi";
                cmbProveedores.SelectedIndex = 0;
            
           
            
            
        }

        private void RbExterno_Checked(object sender, RoutedEventArgs e)
        {
            String consulta = "SELECT DISTINCT p.nombre, p.idClienteme FROM clientesme p inner join ordencomprasalida o where o.FK_idClienteme = p.idClienteme";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idClienteme";
            cmbProveedores.SelectedIndex = 0;
        }

        private void btnProdAgregarTodo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnProdAgregar.Visibility = Visibility.Collapsed;
                btnProdEliminar.Visibility = Visibility.Collapsed;
                bool existe = false;

                Producto prod = dgvProductosOC.SelectedItem as Producto;

                id = prod.id;
                if (prod.cantidad > 0)
                {

                    for (int i = 0; i < ProdRemito.Count; i++)
                    {
                        if (ProdRemito[i].nombre == prod.nombre)
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
                        foreach (var producto in productos )
                        {


                            Producto productoremito = new Producto(producto.nombre, producto.id, producto.cantidad);
                            prodRemito.Add(productoremito);
                            dgvProductosRemito.Items.Refresh();
                            producto.cantidad = 0;
                            dgvProductosOC.Items.Refresh();

                        }
                    }
                    else
                    {
                        MessageBox.Show("El producto ya se agrego", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }



                }
                else
                {
                    MessageBox.Show("Ya se agregaron todas las facturas de este producto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un producto a agregar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void btnProdEliminarTodo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnProdAgregar.Visibility  = Visibility.Visible;
                btnProdEliminar.Visibility = Visibility.Visible;
                Producto prod = dgvProductosRemito.SelectedItem as Producto;
                if (modifica)
                {

                    String en = "SELECT fecha FROM remito WHERE idremitos = '" + idRemito + "'";
                    DateTime fechaRemito = DateTime.Parse(conexion.ValorEnVariable(en));

                    String s = "SELECT ums FROM productos WHERE idProductos = '" + prod.id + "'";
                    DateTime fechaUMS = DateTime.Parse(conexion.ValorEnVariable(s));

                    TimeSpan ts = fechaRemito - fechaUMS;

                    // Difference in days.
                    int differenceInDays = ts.Days;

                    if (differenceInDays <= 0)
                    {
                        MessageBox.Show("No se puede modificar el remito porque ya se ha realizado una venta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                    else
                    {
                        for (int i = 0; i < productos.Count; i++)
                        {
                            if (productos[i].nombre == ProdRemito[i].nombre)
                            {
                                productos[i].cantidad += ProdRemito[i].cantidad;
                            }

                        }
                        ProdRemito.Clear();
                        dgvProductosRemito.Items.Refresh();
                        dgvProductosOC.Items.Refresh();
                    }
                }
                else
                {
                    for (int i = 0; i < productos.Count; i++)
                    {
                        if (productos[i].nombre == ProdRemito[i].nombre)
                        {
                            productos[i].cantidad += ProdRemito[i].cantidad;
                        }

                    }
                    ProdRemito.Clear();
                    dgvProductosRemito.Items.Refresh();
                    dgvProductosOC.Items.Refresh();
                }
            }






            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un producto para eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }
    }

  

}
