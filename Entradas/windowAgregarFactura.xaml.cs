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

        Decimal subtotal;
        Decimal total;
        CRUD conexion = new CRUD();
        public List<producto> items = new List<producto>();
        public  List<producto> itemsFact = new List<producto>();
        producto producto;
        public producto prod;

        public windowAgregarFactura()
        {
            InitializeComponent();
            LoadListaComboProveedor();
            LlenarComboFiltro();
            LlenarCmbIVA();
            LlenarCmbTipoCambio();
            LoadDgvProducto();
            LoadDgvFactura();
            LlenarCmbTipoCuota();
        }

        public windowAgregarFactura(int numFactura, String proveedor, List<producto> pOC, List<producto> pFA, DateTime fechafactura, int numeroOC, Decimal subtotal, Decimal total, String IVA, String tipoCambio)
        {

            this.txtNroFactura.Text = numFactura.ToString();
            this.cmbProveedores.Text = proveedor;
            dgvProductosOC.ItemsSource = pOC;
            dgvProductosFactura.ItemsSource = pFA;
            dtFactura.SelectedDate = fechafactura;
            cmbOrden.Text = numeroOC.ToString();
            txtSubtotal.Text = subtotal.ToString();
            txtTotal.Text = total.ToString();
            cmbIVA.Text = IVA.ToString();
            cmbTipoCambio.Text = tipoCambio.ToString();


        }

        public void LoadListaComboProveedor()
        {
            String consulta = "SELECT p.nombre, p.idProveedor FROM proveedor p INNER JOIN ordencompra o ON p.idProveedor = o.FK_idProveedor";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
            cmbProveedores.SelectedIndex = 1;
        }
        public void LlenarComboFiltro()
        {
            cmbFiltro.Items.Add("Proveedor");

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
        }

        private void dgvProductosFactura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                cmbOrden.SelectedIndex = 0;
            }
            catch (Exception)
            {

                MessageBox.Show("error");
            }

        }

        private void cmbOrden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            items.Clear();

            try
            {
                String sql2 = "SELECT productos.nombre, productos.idProductos, cantidad, subtotal, productos_has_ordencompra.precioUnitario  FROM productos_has_ordencompra, productos WHERE FK_idOC ='" + cmbOrden.SelectedValue.ToString() + "' AND productos.idProductos = productos_has_ordencompra.FK_idProducto";
          
                DataTable productos = conexion.ConsultaParametrizada(sql2, cmbOrden.SelectedValue);
                for (int i = 0; i < productos.Rows.Count; i++)
                {
                    producto = new producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[1],(int)productos.Rows[i].ItemArray[2], (decimal)productos.Rows[i].ItemArray[3], (decimal)productos.Rows[i].ItemArray[4]);
                    items.Add(producto);

                }

                dgvProductosOC.Items.Refresh();
            }
            catch (NullReferenceException)
            {


            }

        }
        private void LoadDgvProducto()
        {
            dgvProductosOC.ItemsSource = items;
        }

        private void LoadDgvFactura()
        {
            dgvProductosFactura.ItemsSource = itemsFact;
        }

        private void btnProdAgregar_Click(object sender, RoutedEventArgs e)
        {

            prod = dgvProductosOC.SelectedItem as producto;

            var newW = new WindowAgregarProductoFactura();
            if (prod.cantidad >=1)
            {
                
                newW.txtCantidad.Text = prod.cantidad.ToString();
                newW.can = prod.cantidad;
                newW.ShowDialog();
            }
            else
            {
                MessageBox.Show("El producto ya fue facturado");
            }
            

            if (newW.DialogResult == true)
            {
                prod.cantidad = int.Parse(newW.txtCantidad.Text);
                itemsFact.Add(prod);
                dgvProductosFactura.Items.Refresh();
                subtotal += prod.precioUnitario * prod.cantidad;
                txtSubtotal.Text = subtotal.ToString();
                calculaTotal();
            }

      
            
        }

        private void btnProdEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prod = dgvProductosFactura.SelectedItem as producto;
                subtotal = subtotal - prod.cantidad * prod.precioUnitario;
                txtSubtotal.Text = subtotal.ToString();
                calculaTotal();
                itemsFact.Remove(prod);
                dgvProductosFactura.Items.Refresh();
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un producto");
            }
            
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
           
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

        private void cmbTipoCambio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void cmbIVA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calculaTotal();
        }
    }

}
