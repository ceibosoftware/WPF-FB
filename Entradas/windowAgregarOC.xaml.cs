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
    /// Interaction logic for windowAgregarOC.xaml
    /// </summary>
    public partial class windowAgregarOC : Window
    {

        List<producto> productos = new List<producto>() ;
        decimal subtotal;
        decimal total;
        CRUD conexion = new CRUD();
        public windowAgregarOC()
        {
            InitializeComponent();
            loadDgvProductos();
            LoadListaComboProveedor();
            LlenarCmbIVA();
            LlenarCmbTipoCambio();
            loadCmbfiltro();
                  
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
                producto p = new producto(newW.idProducto, newW.txtNombre.Text, cantidad,precioU,total);
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
            String consulta = "SELECT * FROM proveedor";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
            cmbProveedores.SelectedIndex = 0;
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
            // Busquedas de productos.
            DataTable productos = new DataTable();
            String consulta;

            if (cmbFiltro.Text == "Nombre")
            {   //Busca por nombre
                consulta = "SELECT * FROM proveedor WHERE proveedor.nombre LIKE '%' @valor '%'";
                productos = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
                
            }
            else if (cmbFiltro.Text == "Categoria" )
            {
                //busca por nombre de categoria (posibilidad de agregar combobox)
               consulta = "SELECT proveedor.nombre ,categorias.idCategorias FROM categorias , proveedor, categorias_has_proveedor WHERE  categorias.idCategorias = categorias_has_proveedor.FK_idCategorias and categorias.nombre LIKE '%' @valor '%'";
               productos = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
              
            }
            


           cmbProveedores.ItemsSource = productos.AsDataView();
            cmbProveedores.SelectedIndex = 0;
        }

        public void loadCmbfiltro()
        {
            cmbFiltro.Items.Add("Nombre");
            cmbFiltro.Items.Add("Categoria");
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

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
