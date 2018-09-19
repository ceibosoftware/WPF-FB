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
    /// Interaction logic for windowAgregarFactura.xaml
    /// </summary>
    public partial class windowAgregarFactura : Window
    {


        float total = 0;
        CRUD conexion = new CRUD();
        public List<Producto> items = new List<Producto>();
        public List<Producto> itemsFact = new List<Producto>();
        public List<Cuotas> todaslascuotas = new List<Cuotas>();
        public List<Producto> productosOC = new List<Producto>();
        Producto producto;
        public Producto prod;
        bool bandera = false;
        bool modificafactura = false;
        DateTime dt = DateTime.Now;
      public  float Subtotal = 0;
        public int id = 0;
        bool modifica = false;
        public windowAgregarFactura()
        {
            itemsFact.Clear();
            items.Clear();
            todaslascuotas.Clear();
            InitializeComponent();
            LoadListaComboProveedor();
            LlenarCmbIVA();
            LlenarCmbTipoCambio();
            LoadDgvProducto();
            LoadDgvFactura();
            loadDGVCuotas();
            LlenarCmbTipoCuota();
            txtNroFactura.MaxLines = 1;
            txtNroFactura.MaxLength = 18;
            txtTotal.IsReadOnly = true;
            txtSubtotal.IsReadOnly = true;
            dgvProductosFactura.IsReadOnly = true;
            dgvProductosOC.IsReadOnly = true;
            seleccionefecha();
            dtFactura.SelectedDate = DateTime.Now;
            //ANTONITO
            bandera = true;
            SetearColumnas();
            SetearColumnas2();
            cmbTipoCambio.IsEnabled = false;
            txtNroFactura.Focus();
            txtNroFactura.MaxLength = 8;
           
        }

        public windowAgregarFactura(String numFactura, String proveedor, List<Producto> pOC, List<Producto> pFA, DateTime fechafactura, int numeroOC, float subtotal, float total, int IVA, int tipoCambio, float subtotal2, String cuotas, List<Cuotas> lCU, String cotizacion)
        {
            InitializeComponent();
        
            modifica = true;
            modificafactura = true;
                txtNroFactura.MaxLines = 1;
                txtNroFactura.MaxLength = 18;
                itemsFact.Clear();
                items.Clear();
                todaslascuotas.Clear();
                LoadListaComboProveedor();
                LlenarCmbIVA();
                LlenarCmbTipoCambio();
                LoadDgvProducto(pOC);
                LoadDgvFactura(pFA);
                LlenarCmbTipoCuota();
                loadDGVCuotas(lCU);
                cmbProveedores.IsEnabled = false;
                cmbOrden.IsEnabled = false;
                txtFiltro.IsEnabled = false;
            cmbTipoCambio.IsEnabled = false;
                this.txtNroFactura.Text = numFactura.ToString();
                this.cmbProveedores.Text = proveedor;
                this.items = pOC;
                this.cmbCuotas.Text = cuotas;
                dtFactura.SelectedDate = fechafactura;
                cmbOrden.Text = numeroOC.ToString();
                this.Subtotal = subtotal;
                txtSubtotal.Text = subtotal.ToString();
                cmbIVA.SelectedIndex = IVA;
                cmbTipoCambio.SelectedIndex = tipoCambio;
            if (tipoCambio == 0)
            {
                txtCotizacion.Visibility = Visibility.Collapsed;
                txtTotalPesos.Visibility = Visibility.Collapsed;
                lblCotizacion.Visibility = Visibility.Collapsed;
                lblTotalPesos.Visibility = Visibility.Collapsed;
            }
            txtCotizacion.Text = cotizacion;
            
                txtTotal.Text = total.ToString();
                dt = fechafactura.Date;
                this.itemsFact = pFA;
                this.todaslascuotas = lCU;
              
                
            bandera = true;

                txtTotal.IsReadOnly = true;
                txtSubtotal.IsReadOnly = true;
                dgvProductosFactura.IsReadOnly = true;
                dgvProductosOC.IsReadOnly = true;

           
            SetearColumnas();
            calculaTotalPesos();
            //cambios diseño batta
            lblWindowTitle.Content = "Modificar Factura";
        }
        public windowAgregarFactura(string idOC, String proveedor)
        {
            InitializeComponent();
            LoadListaComboProveedor();
            LlenarCmbIVA();
            LlenarCmbTipoCambio();
            LoadDgvProducto();
            LoadDgvFactura();
            LlenarCmbTipoCuota();
            loadDGVCuotas();
            cmbOrden.Text = idOC;
            cmbProveedores.Text = proveedor;
            txtTotal.IsReadOnly = true;
            txtSubtotal.IsReadOnly = true;
            dgvProductosFactura.IsReadOnly = true;
            dgvProductosOC.IsReadOnly = true;
            seleccionefecha();
            dtFactura.SelectedDate = DateTime.Now;
            SetearColumnas();
            loadDatosOC();
        }

        public void SetearColumnas()
        {

            //DGV FACTURA
            dgvProductosFactura.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvProductosFactura.Columns.Add(textColumn);

            DataGridTextColumn textColumn1 = new DataGridTextColumn();
            textColumn1.Header = "Cantidad";
            textColumn1.Binding = new Binding("cantidad");
            dgvProductosFactura.Columns.Add(textColumn1);

            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio Unitario";
            textColumn2.Binding = new Binding("precioUnitario");
            dgvProductosFactura.Columns.Add(textColumn2);

          /*  DataGridTextColumn textColumn3 = new DataGridTextColumn();
            textColumn3.Header = "Subtotal";
            textColumn3.Binding = new Binding("subtotal");
            dgvProductosFactura.Columns.Add(textColumn3);*/

                       //DGV OC
            dgvProductosOC.AutoGenerateColumns = false;
            DataGridTextColumn textColumn4 = new DataGridTextColumn();
            textColumn4.Header = "Nombre";
            textColumn4.Binding = new Binding("nombre");
            dgvProductosOC.Columns.Add(textColumn4);

            DataGridTextColumn textColumn14 = new DataGridTextColumn();
            textColumn14.Header = "Cantidad";
            textColumn14.Binding = new Binding("cantidad");
            dgvProductosOC.Columns.Add(textColumn14);

            DataGridTextColumn textColumn24 = new DataGridTextColumn();
            textColumn24.Header = "Precio Unitario";
            textColumn24.Binding = new Binding("precioUnitario");
            dgvProductosOC.Columns.Add(textColumn24);

          /*  DataGridTextColumn textColumn34 = new DataGridTextColumn();
            textColumn34.Header = "Subtotal";
            textColumn34.Binding = new Binding("subtotal");
            dgvProductosOC.Columns.Add(textColumn34);*/
        }
        private void SetearColumnas2()
        {

        }
        private void seleccionefecha()
        {
            cmbCuotas.Text = "--Seleccione fecha factura--";

        }
        public void LoadListaComboProveedor()
        {
            String consulta = "SELECT DISTINCT p.nombre, p.idProveedor FROM proveedor p INNER JOIN ordencompra o ON p.idProveedor = o.FK_idProveedor";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
            cmbProveedores.SelectedIndex = 0;
        }

    

        private void LlenarCmbIVA()
        {
            cmbIVA.Items.Add("0");
            cmbIVA.Items.Add("21");
            cmbIVA.Items.Add("10,5");
        }

        private void LlenarCmbTipoCambio()
        {
            cmbTipoCambio.Items.Add("$");
            cmbTipoCambio.Items.Add("u$d");
            cmbTipoCambio.Items.Add("€");
        }

        private void LlenarCmbTipoCuota()
        {
            cmbCuotas.Items.Add("1");
            cmbCuotas.Items.Add("2");
            cmbCuotas.Items.Add("3");
            cmbCuotas.Items.Add("4");
            cmbCuotas.Items.Add("5");
            cmbCuotas.Items.Add("6");
            cmbCuotas.Items.Add("7");
            cmbCuotas.Items.Add("8");
            cmbCuotas.Items.Add("9");
            cmbCuotas.Items.Add("10");
            cmbCuotas.Items.Add("11");
            cmbCuotas.Items.Add("12");
        }

        private void dgvProductosFactura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            itemsFact.Clear();
            productosOC.Clear();
            dgvProductosOC.Items.Refresh();
            dgvProductosFactura.Items.Refresh();
            try
            {
                String id = cmbProveedores.SelectedValue.ToString();
                String nombreProv = cmbProveedores.Text;
             
                    String sql = "SELECT * FROM ordencompra WHERE FK_idProveedor =  '" + id + "'";
                    conexion.Consulta(sql, combo: cmbOrden);
                    cmbOrden.DisplayMemberPath = "idOrdenCompra";
                    cmbOrden.SelectedValuePath = "idOrdenCompra";
             
                    cmbOrden.SelectedIndex = -1;
                
               
            }
            catch (NullReferenceException)
            {

           
           }

        }

        private void cmbOrden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           

        }
     

        

        private void LoadDgvProducto(List<Producto> pOC)
        {
            foreach (var item in pOC)
            {
                productosOC.Add(item);
            }
            dgvProductosOC.ItemsSource = productosOC;
        }
        private void LoadDgvProducto()
        {
            dgvProductosOC.ItemsSource = items;
        }
        private void LoadDgvFactura()
        {
            dgvProductosFactura.ItemsSource = itemsFact;
            dgvProductosFactura.SelectedIndex = 0;
        }

        private void LoadDgvFactura(List<Producto> listproductosfactura)
        {
            dgvProductosFactura.ItemsSource = listproductosfactura;
        }

        private void btnProdAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            
                bool existe = false;
                Producto prod = dgvProductosOC.SelectedItem as Producto;
               
                id = prod.id;
                if (prod.cantidad > 0)
                {
                    var newW = new WindowAgregarProductoFactura();
                    for (int i = 0; i < itemsFact.Count; i++)
                    {
                        if (itemsFact[i].nombre == prod.nombre)
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
                        MessageBox.Show("El producto ya se agrego", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }


                    if (newW.DialogResult == true)
                    {
                        if (int.Parse(newW.txtCantidad.Text) > 0 )
                        {
                            
                            Producto productoFactura = new Producto(prod.nombre, prod.id, int.Parse(newW.txtCantidad.Text), prod.total, prod.precioUnitario);
                            itemsFact.Add(productoFactura);
                            Subtotal = Subtotal + (productoFactura.cantidad * productoFactura.precioUnitario);
                          
                            dgvProductosFactura.Items.Refresh();
                            // float.TryParse(txtSubtotal.Text, out subtotal);

                            
                            txtSubtotal.Text = (Subtotal).ToString();                          
                            prod.cantidad = prod.cantidad - int.Parse(newW.txtCantidad.Text);
                            dgvProductosOC.Items.Refresh();
                            calculaTotal();
                        }
                        else
                        {
                            MessageBox.Show("La cantidad no puede ser cero", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
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

        private void btnProdEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              
                prod = dgvProductosFactura.SelectedItem as Producto;
                int cantidadAsumar=0;  

                cantidadAsumar = prod.cantidad;

                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].nombre == prod.nombre)
                    {
                        items[i].cantidad += prod.cantidad;
                
                    }

                }
                Subtotal = Subtotal - (prod.cantidad * prod.precioUnitario);
                MessageBox.Show("Total: " + total);
                MessageBox.Show("subtotal: " + Subtotal);
                txtSubtotal.Text = Subtotal.ToString();
                calculaTotal();
                dgvProductosOC.Items.Refresh();
                itemsFact.Remove(prod);
                dgvProductosFactura.Items.Refresh();
                if (dgvProductosFactura.HasItems == false)
                {
                    txtSubtotal.Text = "0";
                    txtTotal.Text = "0";
               
                    todaslascuotas.Clear();
                    DgvCuotas.Items.Refresh();
                }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un producto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
         
            bandera = false;
            if (Valida())
            {
                if (modificafactura)
                {

                }
                else
                {

                    if ((int)cmbOrden.SelectedValue < 10)
                    {
                        String numero = txtNroFactura.Text.ToString();

                        String numerofinal = "OC " + "000" + cmbOrden.SelectedValue.ToString() + "-" + numero;

                        txtNroFactura.Text = numerofinal;
                    } else if ((int)cmbOrden.SelectedValue >= 100)
                    {
                        String numero = txtNroFactura.Text.ToString();
                        String numerofinal = "OC " + "0" + cmbOrden.SelectedValue.ToString() + "-" + numero;

                        txtNroFactura.Text = numerofinal;
                    }
                    else if ((int)cmbOrden.SelectedValue >= 1000)
                    {
                        String numero = txtNroFactura.Text.ToString();
                        String numerofinal = "OC " + cmbOrden.SelectedValue.ToString() + "-" + numero;

                        txtNroFactura.Text = numerofinal;
                    }

                    else
                    {
                        String numero = txtNroFactura.Text.ToString();
                        String numerofinal = "OC " + "00" + cmbOrden.SelectedValue.ToString() + "-" + numero;

                        txtNroFactura.Text = numerofinal;
                    }

                }
                DialogResult = true;
            }
        }

        public void calculaTotal()
        {

            if (cmbIVA.SelectedIndex == 0)
            {
        
                txtTotal.Text = txtSubtotal.Text.ToString();
            }
            else if (cmbIVA.SelectedIndex == 1)
            {
                total = float.Parse(txtSubtotal.Text) * (float)1.21;
                txtTotal.Text = total.ToString();
               
            }
            else if (cmbIVA.SelectedIndex == 2)
            {
                total = float.Parse(txtSubtotal.Text) * (float)1.105;
                txtTotal.Text = total.ToString();
                
            }
        }

        private void cmbTipoCambio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        
        }

        private void cmbIVA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calculaTotal();
            calculaTotalPesos();
            todaslascuotas.Clear();
            DgvCuotas.Items.Refresh();
            cmbCuotas.SelectedIndex = -1;
            cmbCuotas.Text = "Seleccione cantidad de cuotas";
        }

       

        private void dgvProductosFactura_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           
        }

        private void dgvProductosFactura_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

  

        public void loadDGVCuotas()
        {

            DgvCuotas.ItemsSource = todaslascuotas;
            DgvCuotas.Items.Refresh();
        }

        public void loadDGVCuotas(List<Cuotas>l)
        {

            DgvCuotas.ItemsSource = l;
            DgvCuotas.Items.Refresh();
        }
        public Boolean Valida()
        {

            String nombreDB = "SELECT COUNT(*) FROM factura WHERE numeroFactura  = '" + txtNroFactura.Text + "'";
            String nomCat = conexion.ValorEnVariable(nombreDB).ToString();
            if (dtFactura.SelectedDate == null )
            {
                MessageBox.Show("Ingrese fecha de la factura", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
           
            else if (dgvProductosFactura.HasItems == false)
            {
                MessageBox.Show("Ingrese productos a la factura", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            } else if (cmbCuotas.Text == "") {

                MessageBox.Show("Seleccione cantidad de cuotas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (cmbIVA.Text == "")
            {

                MessageBox.Show("Seleccione IVA", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (cmbTipoCambio.Text == "")
            {

                MessageBox.Show("Seleccione tipo de cambio", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (cmbProveedores.Text == "")
            {

                MessageBox.Show("Seleccione un Proveedor", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (cmbTipoCambio.Text != "$" && txtCotizacion.Text == "")
            {
             
                    MessageBox.Show("Ingrese cotización", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                
            }
            else if(!DgvCuotas.HasItems)
            {
                MessageBox.Show("Seleccione cantidad de cuotas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (bandera == true && nomCat !="0")
            {

                MessageBox.Show("El número de la factura ya existe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
                
            

            }
         
            else if (txtNroFactura.Text == "")
            {
                MessageBox.Show("Ingrese número de la factura", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            else if (txtNroFactura.Text == "000"+cmbOrden.SelectedValue.ToString()+"-"+"0000" || txtNroFactura.Text == "00" + cmbOrden.SelectedValue.ToString()+"-" + "0000")
            {
                MessageBox.Show("Complete el número de la factura", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void txtNroFactura_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            items.Clear();
            productosOC.Clear();
            Subtotal = 0;
            txtSubtotal.Text = "0";
            calculaTotal();
            dgvProductosOC.Items.Refresh();
            // Busquedas de productos.
            DataTable facturas = new DataTable();
            String consulta;
            consulta = "SELECT DISTINCT p.nombre, p.idProveedor FROM proveedor p INNER JOIN ordencompra o ON p.idProveedor = o.FK_idProveedor AND p.nombre LIKE '%' @valor '%' ";
            facturas = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            cmbProveedores.ItemsSource = facturas.AsDataView();
            cmbProveedores.SelectedIndex = 0;
        }

        private void dtFactura_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
       
            
                    todaslascuotas.Clear();
                    DgvCuotas.Items.Refresh();
                cmbCuotas.Text = "Seleccione cantidad de cuotas";
            bandera = true;
        }

        private void txtNroFactura_TextChanged(object sender, TextChangedEventArgs e)
        {

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
                   
                    for (int i = 0; i < itemsFact.Count; i++)
                    {
                        if (itemsFact[i].nombre == prod.nombre)
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
                        foreach (var producto in items)
                        {

                       
                        Producto productoFactura = new Producto(producto.nombre, producto.id, producto.cantidad, producto.total, producto.precioUnitario);
                        itemsFact.Add(productoFactura);
                        Subtotal = Subtotal + (productoFactura.cantidad * productoFactura.precioUnitario);

                        dgvProductosFactura.Items.Refresh();
                        // float.TryParse(txtSubtotal.Text, out subtotal);


                        txtSubtotal.Text = (Subtotal).ToString();
                        producto.cantidad = 0;
                        dgvProductosOC.Items.Refresh();
                        calculaTotal();

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

        private void btnProdEliminartodo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                prod = dgvProductosFactura.SelectedItem as Producto;
                int cantidadAsumar = 0;

                cantidadAsumar = prod.cantidad;

                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].nombre == itemsFact[i].nombre)
                    {
                        items[i].cantidad += itemsFact[i].cantidad;

                    }
                    Subtotal = Subtotal - (itemsFact[i].cantidad * itemsFact[i].precioUnitario);
                }
               // Subtotal = Subtotal - (prod.cantidad * prod.precioUnitario);

                txtSubtotal.Text = Subtotal.ToString();
                calculaTotal();
                dgvProductosOC.Items.Refresh();
                itemsFact.Clear();
                dgvProductosFactura.Items.Refresh();
                btnProdAgregar.Visibility = Visibility.Visible;
                btnProdEliminar.Visibility = Visibility.Visible;
                if (dgvProductosFactura.HasItems == false)
                {
                    txtSubtotal.Text = "0";
                    txtTotal.Text = "0";

                    todaslascuotas.Clear();
                    DgvCuotas.Items.Refresh();
                }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un producto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtCotizacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculaTotalPesos();
        }

        private  void calculaTotalPesos()
        {
            try
            {

            
            if (txtTotal.Text != "" && txtCotizacion.Text != "")
                txtTotalPesos.Text = (float.Parse(txtTotal.Text) * float.Parse(txtCotizacion.Text)).ToString();
            if (txtCotizacion.Text == "")
                txtTotalPesos.Text = "";
            if(itemsFact.Count == 0)            
                txtTotalPesos.Text = "";
            }
            catch (Exception)
            {

               
            }
        }

        private void cmbCuotas_DropDownClosed(object sender, EventArgs e)
        {
            int cuotass = cmbCuotas.SelectedIndex + 1;

           

                todaslascuotas.Clear();
                if (dgvProductosFactura.Items.Count == 0)
                {
                    MessageBox.Show("Primero cargue productos a la factura", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    var newW = new windowCuotas(cuotass, dt, float.Parse(txtTotal.Text));
                    newW.Title = "Agregar Cuotas";
                    newW.ShowDialog();

                    if (newW.DialogResult == true)
                    {
                        todaslascuotas.Clear();
                        foreach (Cuotas cuot in newW.listacuotas)
                        {

                            int id = cuot.cuota;
                            int dias = cuot.dias;
                            DateTime fecha = cuot.fechadepago;
                            float totalPagar = cuot.montoCuota;
                            int cuota = cuot.cuota;
                            Cuotas cu = new Cuotas(id, dias, fecha, totalPagar, cuota);
                            todaslascuotas.Add(cu);

                        }
                        loadDGVCuotas();

                    }
                    else
                    {
                        bandera = false;
                        cmbCuotas.SelectedIndex = -1;
                        bandera = true;
                    }

                }

            
        }

        private void cmbProveedores_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

            itemsFact.Clear();
        
            dgvProductosFactura.Items.Refresh();
            try
            {
                String id = cmbProveedores.SelectedValue.ToString();
                String nombreProv = cmbProveedores.Text;

                String sql = "SELECT * FROM ordencompra WHERE FK_idProveedor =  '" + id + "'";
                conexion.Consulta(sql, combo: cmbOrden);
                cmbOrden.DisplayMemberPath = "idOrdenCompra";
                cmbOrden.SelectedValuePath = "idOrdenCompra";

                cmbOrden.SelectedIndex = -1;
                cmbOrden.Text = "Seleccione OC";
                items.Clear();
                dgvProductosOC.Items.Refresh();

            }
            catch (NullReferenceException)
            {


            }
        }

        private void cmbOrden_DropDownClosed(object sender, EventArgs e)
        {



            loadDatosOC();

            
        }

        private void loadDatosOC()
        {
            items.Clear();
            txtSubtotal.Text = "0";
            calculaTotal();
            Subtotal = 0;

            String sql2 = "SELECT productos.nombre, productos.idProductos,productos_has_ordencompra.CrFactura, subtotal, productos_has_ordencompra.PUPagado  FROM productos_has_ordencompra, productos WHERE FK_idOC = @valor AND productos.idProductos = productos_has_ordencompra.FK_idProducto";

            DataTable productos = conexion.ConsultaParametrizada(sql2, cmbOrden.SelectedValue);
            for (int i = 0; i < productos.Rows.Count; i++)
            {
                producto = new Producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[1], (int)productos.Rows[i].ItemArray[2], (float)productos.Rows[i].ItemArray[3], (float)productos.Rows[i].ItemArray[4]);
                items.Add(producto);

            }

            dgvProductosOC.Items.Refresh();
            todaslascuotas.Clear();
            DgvCuotas.Items.Refresh();
            itemsFact.Clear();
            dgvProductosFactura.Items.Refresh();

            string consultamoneda = "select tipoCambio from ordencompra where idOrdenCompra = " + cmbOrden.SelectedValue + "";
            cmbTipoCambio.SelectedIndex = int.Parse(conexion.ValorEnVariable(consultamoneda));

            if (int.Parse(conexion.ValorEnVariable(consultamoneda)) == 0)
            {
                txtCotizacion.Visibility = Visibility.Collapsed;
                txtTotalPesos.Visibility = Visibility.Collapsed;
                lblCotizacion.Visibility = Visibility.Collapsed;
                lblTotalPesos.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtCotizacion.Visibility = Visibility.Visible;
                txtTotalPesos.Visibility = Visibility.Visible;
                lblCotizacion.Visibility = Visibility.Visible;
                lblTotalPesos.Visibility = Visibility.Visible;
            }
        }
    }

}
