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
using wpfFamiliaBlanco.SalidasNuevo.Clases.OrdenesPedido;

namespace wpfFamiliaBlanco.SalidasNuevo.Vistas.FacturaMI
{
    /// <summary>
    /// Interaction logic for WindowAgregarFMI.xaml
    /// </summary>
    public partial class WindowAgregarFMI : Window
    {
        Cliente cliente = new Cliente();
        OrdenPedido OP = new OrdenPedido();
        public List<ProductoOP> items = new List<ProductoOP>();
        public List<ProductoOP> itemsFact = new List<ProductoOP>();
        float Subtotal = 0;
        float total = 0;
        int id;
        public WindowAgregarFMI()
        {
            InitializeComponent();
            LoadCMBClientes();
            LoadCmbIVA();
            LoadCmbMoneda();
            ColumnasDGVProductosMuestra();
            ColumnasDGVProductosFactura();
            LoadDgvProductosFactura();
        }

        private void LoadDgvProductosFactura()
        {
            dgvProductosFactura.ItemsSource = itemsFact;
        }

        private void LoadCmbMoneda()
        {
            cmbTipoCambio.Items.Add("$");
            cmbTipoCambio.Items.Add("EUR");
            cmbTipoCambio.Items.Add("USD");
            cmbTipoCambio.SelectedIndex = 0;
        }

        private void LoadCmbIVA()
        {
            cmbIVA.Items.Add("21");
            cmbIVA.Items.Add("10.5");
            cmbIVA.Items.Add("0");
            cmbIVA.SelectedIndex = 0;
        }

        private void LoadCMBClientes()
        {
            cmbClientes.ItemsSource = cliente.getClientes().AsDataView();
            cmbClientes.DisplayMemberPath = "nombre";
            cmbClientes.SelectedValuePath = "idClientemi";
        }
        private void cmbClientes_DropDownClosed(object sender, EventArgs e)
        {
            cmbordenCompra.ItemsSource = OP.GetOrdenesCliente((int)cmbClientes.SelectedValue).AsDataView();
            cmbordenCompra.SelectedValuePath = "idOPMI";
            cmbordenCompra.DisplayMemberPath = "idOPMI";
        }

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbordenCompra_DropDownClosed(object sender, EventArgs e)
        {
            OP.GetOrdenes((int)cmbordenCompra.SelectedValue);
            dgvProductosOC.ItemsSource = OP.ProductosOP;
        }

        private void ColumnasDGVProductosMuestra()
        {

            dgvProductosOC.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("Nombre");
            dgvProductosOC.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Cantidad";
            textColumn2.Binding = new Binding("TotalBotellas");
            dgvProductosOC.Columns.Add(textColumn2);
            DataGridTextColumn textColumn3 = new DataGridTextColumn();
            textColumn3.Header = "Precio Unitario";
            textColumn3.Binding = new Binding("Precio");
            dgvProductosOC.Columns.Add(textColumn3);
            DataGridTextColumn textColumn5 = new DataGridTextColumn();
            textColumn5.Header = " % Descuento";
            textColumn5.Binding = new Binding("Descuento");
            dgvProductosOC.Columns.Add(textColumn5);

        }

        private void ColumnasDGVProductosFactura()
        {

            dgvProductosFactura.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("Nombre");
            dgvProductosFactura.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Cantidad";
            textColumn2.Binding = new Binding("TotalBotellas");
            dgvProductosFactura.Columns.Add(textColumn2);
            DataGridTextColumn textColumn3 = new DataGridTextColumn();
            textColumn3.Header = "Precio Unitario";
            textColumn3.Binding = new Binding("Precio");
            dgvProductosFactura.Columns.Add(textColumn3);
            DataGridTextColumn textColumn5 = new DataGridTextColumn();
            textColumn5.Header = " % Descuento";
            textColumn5.Binding = new Binding("Descuento");
            dgvProductosFactura.Columns.Add(textColumn5);

        }
        private void btnProdAgregarTodo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnProdAgregar.Visibility = Visibility.Collapsed;
                btnProdEliminar.Visibility = Visibility.Collapsed;
                bool existe = false;

                ProductoOP prod = dgvProductosOC.SelectedItem as ProductoOP;
                String id = prod.Getid(prod.Nombre);
                if (prod.TotalBotellas > 0)
                {

                    for (int i = 0; i < itemsFact.Count; i++)
                    {
                        if (itemsFact[i].Nombre == prod.Nombre)
                        {
                            existe = true;
                        }
                        else
                        {
                            existe = false;
                        }
                    }
                    if (prod.TotalBotellas >= 1 && !existe)
                    {
                        foreach (var producto in OP.ProductosOP)
                        {

                            ProductoOP productoFactura = new ProductoOP(int.Parse(id), prod.TotalBotellas, prod.Descuento, prod.Precio);
                            itemsFact.Add(productoFactura);
                            Subtotal = Subtotal + (productoFactura.TotalBotellas * productoFactura.Precio);

                            dgvProductosFactura.Items.Refresh();
                            // float.TryParse(txtSubtotal.Text, out subtotal);


                            txtSubtotal.Text = (Subtotal).ToString();
                            producto.TotalBotellas = 0;
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

        private void btnProdAgregar_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                bool existe = false;

                ProductoOP prod = dgvProductosOC.SelectedItem as ProductoOP;
                String id = prod.Getid(prod.Nombre);
  
             
                if (prod.TotalBotellas > 0)
                {
                    var newW = new WindowAgregarProductoFactura();
                    for (int i = 0; i < itemsFact.Count; i++)
                    {
                        if (itemsFact[i].Nombre == prod.Nombre)
                        {
                            existe = true;
                        }
                        else
                        {
                            existe = false;
                        }
                    }
                    if (prod.TotalBotellas >= 1 && !existe)
                    {

                        newW.txtCantidad.Text = prod.TotalBotellas.ToString();
                        newW.can = prod.TotalBotellas;
                        newW.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("El producto ya se agrego", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }


                    if (newW.DialogResult == true)
                    {
                        if (int.Parse(newW.txtCantidad.Text) > 0)
                        {

                            ProductoOP productoFactura = new ProductoOP(int.Parse(id), int.Parse(newW.txtCantidad.Text), prod.Descuento, prod.Precio);
                            itemsFact.Add(productoFactura);
                            Subtotal = Subtotal + (int.Parse(newW.txtCantidad.Text) * productoFactura.Precio);

                            dgvProductosFactura.Items.Refresh();
                            //float.TryParse(txtSubtotal.Text, out subtotal);


                            txtSubtotal.Text = (Subtotal).ToString();
                            prod.TotalBotellas = prod.TotalBotellas - int.Parse(newW.txtCantidad.Text);
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

                ProductoOP prod = dgvProductosFactura.SelectedItem as ProductoOP;
                int cantidadAsumar = 0;

                cantidadAsumar = prod.TotalBotellas;
              
                for (int i = 0; i < OP.ProductosOP.Count; i++)
                {
                    if (OP.ProductosOP[i].Nombre == prod.Nombre)
                    {
                        OP.ProductosOP[i].TotalBotellas += prod.TotalBotellas;
                    }

                }
                Subtotal = Subtotal - (prod.TotalBotellas * prod.Precio);
            
                txtSubtotal.Text = Subtotal.ToString();
                calculaTotal();
                dgvProductosOC.Items.Refresh();
                itemsFact.Remove(prod);
                dgvProductosFactura.Items.Refresh();
                if (dgvProductosFactura.HasItems == false)
                {
                    txtSubtotal.Text = "0";
                    txtTotal.Text = "0";

                   // todaslascuotas.Clear();
                   // DgvCuotas.Items.Refresh();
                }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un producto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnProdEliminartodo_Click(object sender, RoutedEventArgs e)
        {
            try
            {

        

                for (int i = 0; i < OP.ProductosOP.Count; i++)
                {
                    if (OP.ProductosOP[i].Nombre == itemsFact[i].Nombre)
                    {
                        OP.ProductosOP[i].TotalBotellas += itemsFact[i].TotalBotellas;

                    }
                    Subtotal = Subtotal - (itemsFact[i].TotalBotellas * itemsFact[i].Precio);
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

                   // todaslascuotas.Clear();
                   // DgvCuotas.Items.Refresh();
                }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un producto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void calculaTotal()
        {

            if (cmbIVA.SelectedIndex == 2)
            {

                txtTotal.Text = txtSubtotal.Text.ToString();
            }
            else if (cmbIVA.SelectedIndex == 0)
            {
                total = float.Parse(txtSubtotal.Text) * (float)1.21;
                txtTotal.Text = total.ToString();

            }
            else if (cmbIVA.SelectedIndex == 1)
            {
                total = float.Parse(txtSubtotal.Text) * (float)1.105;
                txtTotal.Text = total.ToString();

            }
        }

        private void cmbIVA_DropDownClosed(object sender, EventArgs e)
        {
            if (itemsFact.Count <=0)
            {
                MessageBox.Show("Primero ingrese productos a la factura", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                calculaTotal();
            }
        }
    }
}
