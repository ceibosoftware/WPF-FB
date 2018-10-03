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
        public List<Producto> items = new List<Producto>();
        public List<Producto> itemsFact = new List<Producto>();
        float Subtotal = 0;
        public WindowAgregarFMI()
        {
            InitializeComponent();
            LoadCMBClientes();
            LoadCmbIVA();
            LoadCmbMoneda();
            ColumnasDGVProductosMuestra();
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

        private void btnProdAgregarTodo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnProdAgregar_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                bool existe = false;
                Producto prod = dgvProductosOC.SelectedItem as Producto;
                int id;
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
                        if (int.Parse(newW.txtCantidad.Text) > 0)
                        {

                            Producto productoFactura = new Producto(prod.nombre, prod.id, int.Parse(newW.txtCantidad.Text), prod.total, prod.precioUnitario);
                            itemsFact.Add(productoFactura);
                            Subtotal = Subtotal + (productoFactura.cantidad * productoFactura.precioUnitario);

                            dgvProductosFactura.Items.Refresh();
                            // float.TryParse(txtSubtotal.Text, out subtotal);


                            txtSubtotal.Text = (Subtotal).ToString();
                            prod.cantidad = prod.cantidad - int.Parse(newW.txtCantidad.Text);
                            dgvProductosOC.Items.Refresh();
                            //calculaTotal();
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

        }

        private void btnProdEliminartodo_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
