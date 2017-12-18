﻿using System;
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
        int contador = 0;
       public List<producto> productos = new List<producto>() ;
       public decimal subtotal;
        decimal total;
        public DateTime fecha;
        CRUD conexion = new CRUD();

        public List<producto> Productos { get => productos; set => productos = value; }

        public windowAgregarOC()
        {
            loadGeneral();
            cmbProveedores.SelectedIndex = 0;
            cmbDireccion.SelectedIndex = 0;
            cmbTelefono.SelectedIndex = 0;
            dpFecha.SelectedDate = DateTime.Now; 
                  
        }
        public windowAgregarOC(DateTime fecha, String observaciones, Decimal subtotal,   int iva , int tipoCambio, String formaPago , int telefono, int proveedor, int direccion, List<producto> producto)
        {
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

        }

       
        private void lblAgregarRemito_Copy_Click(object sender, RoutedEventArgs e)
        {
            
            var newW = new windowAgregarRemito();
           var resultado = MessageBox.Show("Desea agregar la orden de compra? ","Advertencia",MessageBoxButton.YesNo,MessageBoxImage.Question);
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
            var newW = new windowAgregarClienteME((int)cmbProveedores.SelectedValue);
            newW.ShowDialog();
           
            if (newW.DialogResult == true)
            {
                int.TryParse(newW.txtCantidad.Text, out int cantidad);
                decimal.TryParse(newW.txtTotal.Text, out decimal total);
                decimal.TryParse(newW.txtPrecioUnitario.Text, out decimal precioU);
                producto p = new producto( newW.txtNombre.Text, newW.idProducto, cantidad, total,precioU);
                productos.Add(p);
                loadDgvProductos();
                dgvProductos.Items.Refresh();
                decimal.TryParse(txtSubtotal.Text, out  subtotal);
                subtotal += p.total;
                txtSubtotal.Text = (subtotal).ToString() ;
                calculaTotal();
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
            cmbIVA.Items.Add((decimal)0);
            cmbIVA.Items.Add((decimal)21);
            cmbIVA.Items.Add((decimal)10.5);
            
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
                total = subtotal * (decimal)1.21;
                txtTotal.Text = total.ToString();
            }
            else if (cmbIVA.SelectedIndex == 2)
            {
                total = subtotal * (decimal)1.105;
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
            producto prod = dgvProductos.SelectedItem as producto;
            subtotal -= prod.total;
            calculaTotal();
            txtSubtotal.Text = (subtotal).ToString();
            productos.Remove(prod);
            dgvProductos.Items.Refresh();
        }

        private void btnModificar_Copy1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                producto prod = dgvProductos.SelectedItem as producto;
                decimal.TryParse(txtSubtotal.Text, out subtotal);
                subtotal -= prod.total;
                var newW = new windowAgregarClienteME((int)cmbProveedores.SelectedValue, prod.id);
                newW.txtCantidad.Text = prod.cantidad.ToString();
                newW.txtPrecioUnitario.Text = prod.precioUnitario.ToString();
                newW.ShowDialog();

                if (newW.DialogResult == true)
                {
                    int.TryParse(newW.txtCantidad.Text, out int cantidad);
                    decimal.TryParse(newW.txtTotal.Text, out decimal total);
                    decimal.TryParse(newW.txtPrecioUnitario.Text, out decimal precioU);
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
