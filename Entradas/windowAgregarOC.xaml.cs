using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Interaction logic for windowAgregarOC.xaml
    /// </summary>
    public partial class windowAgregarOC : Window
    {
        int idOC;
        bool modifica = false;
        int contador = 0;
        public List<Producto> productos = new List<Producto>();
        public float subtotal;
        float total;
        public DateTime fecha;
        CRUD conexion = new CRUD();
        int cantidadAntigua;
        public List<Producto> Productos { get => Productos; set => productos = value; }

        public windowAgregarOC()
        {
            loadGeneral();
            cmbProveedores.SelectedIndex = 0;
            cmbDireccion.SelectedIndex = 0;
            cmbTelefono.SelectedIndex = 0;
            dpFecha.SelectedDate = DateTime.Now;

        }
        public windowAgregarOC(DateTime fecha, String observaciones, float subtotal, int iva, int tipoCambio, String formaPago, int telefono, int proveedor, int direccion, List<Producto> producto, int idOC)
        {
            modifica = true;
            this.productos = producto;
            loadGeneral();
            dpFecha.SelectedDate = fecha;
            txtObservaciones.Text = observaciones;
            this.subtotal = subtotal;
            txtSubtotal.Text = subtotal.ToString();
            cmbIVA.SelectedIndex = iva;
            cmbTipoCambio.SelectedIndex = tipoCambio;
            txtFormaPago.Text = formaPago;
            cmbTelefono.SelectedValue = telefono;
            cmbDireccion.SelectedValue = direccion;
            cmbProveedores.SelectedValue = proveedor;
            calculaTotal();
            this.idOC = idOC;
        }


        private void lblAgregarRemito_Copy_Click(object sender, RoutedEventArgs e)
        {

            var newW = new windowAgregarRemito();
            var resultado = MessageBox.Show("Desea agregar la orden de compra? ", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes)
            {
                newW.ShowDialog();
            }


        }

        private void btAgregarFactura_Copy_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarFactura();
            var resultado = MessageBox.Show("Desea agregar la orden de compra? ", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes)
            {
                newW.ShowDialog();
            }

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowCuotas();
            newW.ShowDialog();
        }

        private void btnAgregar_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (cmbProveedores.SelectedIndex != -1)
            {
                bool existe = false;
                var newW = new windowAgregarClienteME((int)cmbProveedores.SelectedValue);
                newW.ShowDialog();
                if (newW.DialogResult == true)
                {
                    int.TryParse(newW.txtCantidad.Text, out int cantidad);
                    float.TryParse(newW.txtTotal.Text, out float total);
                    float.TryParse(newW.txtPrecioUnitario.Text, out float precioU);
                    for (int i = 0; i < productos.Count; i++)
                    {
                        if (productos[i].nombre == newW.txtNombre.Text)
                        {
                            existe = true;
                            break;
                        }
                        else
                        {
                            existe = false;
                        }
                    }
                    if (!existe)
                    {
                        Producto p = new Producto(newW.txtNombre.Text, newW.idProducto, cantidad, total, precioU);
                        productos.Add(p);
                        loadDgvProductos();
                        dgvProductos.Items.Refresh();
                        float.TryParse(txtSubtotal.Text, out subtotal);
                        subtotal += p.total;
                        txtSubtotal.Text = (subtotal).ToString();
                        calculaTotal();
                    }
                    else
                    {
                        MessageBox.Show("El producto ya fue agregado a la orden de compra", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }





            }
            else
            {
                MessageBox.Show("Es necesario seleccionar un proveedor para agregar producto");
            }
        }
    

        public void LoadListaComboProveedor()
        {
            String consulta = "SELECT  distinct t1.nombre , t1.idProveedor FROM proveedor t1 inner join productos_has_proveedor t2 on t1.idProveedor = t2.FK_idProveedor";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
           
        }
        public void LoadListaComboTelefonos()
        {
            String consulta = "SELECT * FROM telefonocontacto";
            conexion.Consulta(consulta, combo: cmbTelefono);
            cmbTelefono.DisplayMemberPath = "telefono";
            cmbTelefono.SelectedValuePath = "idTelefono";
            
        }
        public void LoadListaComboDireccion()
        {
            String consulta = "SELECT * FROM direcciones";
            conexion.Consulta(consulta, combo: cmbDireccion);
            cmbDireccion.DisplayMemberPath = "direccion";
            cmbDireccion.SelectedValuePath = "idDireccion";
            
        }

        private void LlenarCmbIVA()
        {
            cmbIVA.Items.Add((float)0);
            cmbIVA.Items.Add((float)21);
            cmbIVA.Items.Add((float)10.5);
            
        }

        private void LlenarCmbTipoCambio()
        {
            cmbTipoCambio.Items.Add("$");
            cmbTipoCambio.Items.Add("u$d");
            cmbTipoCambio.Items.Add("€");
        }
        private void loadDgvProductos()
        {      
            dgvProductos.ItemsSource = productos;
            
        }
      

        private void cmbIVA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calculaTotal();
              
        }
        public void calculaTotal()
        {
            if (cmbIVA.SelectedIndex == 0)
            {
                txtTotal.Text = subtotal.ToString();
            }
            else if (cmbIVA.SelectedIndex == 1)
            {
                total = subtotal * (float)1.21;
                txtTotal.Text = total.ToString();
            }
            else if (cmbIVA.SelectedIndex == 2)
            {
                total = subtotal * (float)1.105;
                txtTotal.Text = total.ToString();
            }
        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Busquedas de proveedor.
            DataTable productos = new DataTable();
            String consulta;
            consulta = "SELECT * FROM proveedor WHERE proveedor.nombre LIKE '%' @valor '%'";
            productos = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            cmbProveedores.ItemsSource = productos.AsDataView();
            cmbProveedores.SelectedIndex = 0;
        }

    

        private void btnEliminar_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Producto prod = dgvProductos.SelectedItem as Producto;
                subtotal -= prod.total;
                calculaTotal();
                txtSubtotal.Text = (subtotal).ToString();
                productos.Remove(prod);
                dgvProductos.Items.Refresh();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("No se ha seleccionado ningun producto");
            }
           
        }

        private void btnModificar_Copy1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool existe = false;
                Producto prod = dgvProductos.SelectedItem as Producto;
                float.TryParse(txtSubtotal.Text, out subtotal);
                subtotal -= prod.total;
                var newW = new windowAgregarClienteME((int)cmbProveedores.SelectedValue, prod.id, prod.nombre);
              
                newW.txtCantidad.Text = prod.cantidad.ToString();
             
                newW.txtPrecioUnitario.Text = prod.precioUnitario.ToString();
                newW.txtNombre.Text = prod.nombre;
                newW.CalculaTotal();
                newW.ShowDialog();

                if (newW.DialogResult == true)
                {
                    int.TryParse(newW.txtCantidad.Text, out int cantidad);
                    float.TryParse(newW.txtTotal.Text, out float total);
                    float.TryParse(newW.txtPrecioUnitario.Text, out float precioU);
                    if (prod.nombre != newW.txtNombre.Text)
                    {
                        for (int i = 0; i < productos.Count; i++)
                        {
                            if (productos[i].nombre == newW.txtNombre.Text)
                            {
                                existe = true;
                                break;
                            }
                            else
                            {
                                existe = false;
                            }
                        }
                        if (!existe)
                        {
                            prod.cantidad = cantidad;
        
                            prod.total = total;
                            prod.precioUnitario = precioU;
                            prod.nombre = newW.txtNombre.Text;
                            prod.id = newW.idProducto;
                            dgvProductos.Items.Refresh();
                            subtotal += prod.total;
                            txtSubtotal.Text = (subtotal).ToString();
                            calculaTotal();
                        }
                        else
                        {
                            MessageBox.Show("El producto ya fue agregado a la orden de compra", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                    else
                    {
                        prod.cantidad = cantidad;
                        prod.total = total;
                        prod.precioUnitario = precioU;
                        prod.nombre = newW.txtNombre.Text;
                        prod.id = newW.idProducto;
                        dgvProductos.Items.Refresh();
                        subtotal += prod.total;
                        txtSubtotal.Text = (subtotal).ToString();
                        calculaTotal();
                    }
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("No se ha seleccionado ningun producto");
            }
  
        

        }

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (validar())
            {
                DialogResult = true;
            }
        }
        public bool validar()
        {

            if (productos.Count <= 0)
            {
                MessageBox.Show("Es necesario ingresar al menos un producto");
                return false;
            }
            else if (string.IsNullOrEmpty(dpFecha.Text))
            {
                MessageBox.Show("Es necesario seleccionar la fecha");
                return false;
            }
            else if (string.IsNullOrEmpty(txtFormaPago.Text))
            {
                MessageBox.Show("Falta completar la forma de pago");
                return false;
            }
            else if (string.IsNullOrEmpty(txtObservaciones.Text))
            {
                MessageBox.Show("Falta completa el campo observaciones");
                return false;
            }
            else
            {
                return true;
            }

        }

        private void dpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            fecha = dpFecha.SelectedDate.Value.Date;
        }

        private void loadGeneral()
        {
            InitializeComponent();
            loadDgvProductos();
            LoadListaComboProveedor();
            LlenarCmbIVA();
            LlenarCmbTipoCambio();
            LoadListaComboDireccion();
            LoadListaComboTelefonos();
    

        }
     
        
        private void cmbProveedores_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (contador > 2)
            {
                productos.Clear();
                dgvProductos.Items.Refresh();
                subtotal = 0;
                txtSubtotal.Text = subtotal.ToString();
                calculaTotal();
            }
            contador++;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
