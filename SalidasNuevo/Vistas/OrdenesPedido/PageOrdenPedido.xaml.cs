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
using wpfFamiliaBlanco.SalidasNuevo.Clases.OrdenesPedido;

namespace wpfFamiliaBlanco.SalidasNuevo.Vistas.OrdenesPedido
{
    /// <summary>
    /// Interaction logic for PageOrdenPedido.xaml
    /// </summary>
    public partial class PageOrdenPedido : Page
    {
        OrdenPedido OP = new OrdenPedido();
        Cliente cliente = new Cliente();
        public PageOrdenPedido()
        {
            InitializeComponent();
            LoadGeneral();
        }


        //Metodos
        private void LoadGeneral()
        {
            LoadListOredenes();
            ColumnasDGVProductos();
            ColumnasDGVProductosMuestra();
            loadCmbIva();
            loadcmbCliente();
        }
        private void loadcmbCliente()
        {
            cmbCliente.ItemsSource = cliente.getClientesOPedidos().AsDataView();
            cmbCliente.DisplayMemberPath = "nombre";
            cmbCliente.SelectedValuePath = "idClientemi";
            cmbCliente.SelectedIndex = -1;
            seleccioneParaFiltrar();

        }
        private void seleccioneParaFiltrar()
        {
            cmbCliente.Text = "Seleccione para filtrar";
        }
        private void LoadListOredenes()
        {

            ltsOrdenes.ItemsSource = OP.GetOrdenes().AsDataView();
            ltsOrdenes.DisplayMemberPath = "idOPMI";
            ltsOrdenes.SelectedValuePath = "idOPMI";
        }
        private void LoadListOredenesCliente(int idCliente)
        {

            ltsOrdenes.ItemsSource = OP.GetOrdenesCliente(idCliente).AsDataView();
            ltsOrdenes.DisplayMemberPath = "idOPMI";
            ltsOrdenes.SelectedValuePath = "idOPMI";
        }
        private void llenarElementos()
        {

            txtSubtotal.Text = OP.Subtotal.ToString();
            txtTotal.Text = OP.Total.ToString();
            txtTotalCajas.Text = OP.TotalCajas.ToString();
            cmbTipoIVA.SelectedIndex = OP.TipoIva;
            txtTotalBotellas.Text = OP.TotalBotellas.ToString();
            txtDescuento.Text = OP.Descuento.ToString();
            
            //muestra
            txtSubtotalMuestra.Text = OP.SubtotalMuestra.ToString();
            txtTotalMuestra.Text = OP.TotalMuestra.ToString();
            txtTotalCajasMuestra.Text = OP.TotalCajasMuestra.ToString();
            cmbTipoIVAMuestra.SelectedIndex = OP.TipoIvaMuestra;
            txtTotalBotellasMuestra.Text = OP.TotalBotellasMuestra.ToString();
            txtDescuento.Text = OP.DescuentoMuestra.ToString();

            //Productos
            loadDgvProductos();
            loadDgvProductosMuestra();
            dgvProductos.Items.Refresh();
            dgvProductosMuestra.Items.Refresh();

        }
        private void ColumnasDGVProductos()
        {

            dgvProductos.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("Nombre");
            dgvProductos.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio";
            textColumn2.Binding = new Binding("Precio");
            dgvProductos.Columns.Add(textColumn2);
            DataGridTextColumn textColumn3 = new DataGridTextColumn();
            textColumn3.Header = "Cajas";
            textColumn3.Binding = new Binding("Cajas");
            dgvProductos.Columns.Add(textColumn3);
            DataGridTextColumn textColumn4 = new DataGridTextColumn();
            textColumn4.Header = "Cajas por";
            textColumn4.Binding = new Binding("CajasPor");
            dgvProductos.Columns.Add(textColumn4);
            DataGridTextColumn textColumn5 = new DataGridTextColumn();
            textColumn5.Header = " % Descuento";
            textColumn5.Binding = new Binding("Descuento");
            dgvProductos.Columns.Add(textColumn5);
            DataGridTextColumn textColumn6 = new DataGridTextColumn();
            textColumn6.Header = "Descuento en Pesos";
            textColumn6.Binding = new Binding("DescuentoPesos");
            dgvProductos.Columns.Add(textColumn6);
            DataGridTextColumn textColumn8 = new DataGridTextColumn();
            textColumn8.Header = "Importe";
            textColumn8.Binding = new Binding("Importe");
            dgvProductos.Columns.Add(textColumn8);
            DataGridTextColumn textColumn9 = new DataGridTextColumn();
            textColumn9.Header = "INV";
            textColumn9.Binding = new Binding("CodigoINV");
            dgvProductos.Columns.Add(textColumn9);

        }
        private void ColumnasDGVProductosMuestra()
        {

            dgvProductosMuestra.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("Nombre");
            dgvProductosMuestra.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio";
            textColumn2.Binding = new Binding("Precio");
            dgvProductosMuestra.Columns.Add(textColumn2);
            DataGridTextColumn textColumn3 = new DataGridTextColumn();
            textColumn3.Header = "Cajas";
            textColumn3.Binding = new Binding("Cajas");
            dgvProductosMuestra.Columns.Add(textColumn3);
            DataGridTextColumn textColumn4 = new DataGridTextColumn();
            textColumn4.Header = "Cajas por";
            textColumn4.Binding = new Binding("CajasPor");
            dgvProductosMuestra.Columns.Add(textColumn4);
            DataGridTextColumn textColumn5 = new DataGridTextColumn();
            textColumn5.Header = " % Descuento";
            textColumn5.Binding = new Binding("Descuento");
            dgvProductosMuestra.Columns.Add(textColumn5);
            DataGridTextColumn textColumn6 = new DataGridTextColumn();
            textColumn6.Header = "Descuento Pesos";
            textColumn6.Binding = new Binding("DescuentoPesos");
            dgvProductosMuestra.Columns.Add(textColumn6);
            DataGridTextColumn textColumn8 = new DataGridTextColumn();
            textColumn8.Header = "Importe";
            textColumn8.Binding = new Binding("Importe");
            dgvProductosMuestra.Columns.Add(textColumn8);
            DataGridTextColumn textColumn9 = new DataGridTextColumn();
            textColumn9.Header = "INV ";
            textColumn9.Binding = new Binding("CodigoINV");
            dgvProductosMuestra.Columns.Add(textColumn9);

        }
        private void loadDgvProductos()
        {
            dgvProductos.ItemsSource = OP.ProductosOP;

        }
        private void loadDgvProductosMuestra()
        {
            dgvProductosMuestra.ItemsSource = OP.ProductosMuestra;

        }
        private void loadCmbIva()
        {

            cmbTipoIVA.Items.Add((float)0);
            cmbTipoIVA.Items.Add((float)21);
            cmbTipoIVA.Items.Add((float)10.5);
            //Muestra
            cmbTipoIVAMuestra.Items.Add((float)0);
            cmbTipoIVAMuestra.Items.Add((float)21);
            cmbTipoIVAMuestra.Items.Add((float)10.5);

        }
        //Eventos
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarOP();
            newW.ShowDialog();
            if(newW.DialogResult == true)
            {
                newW.OPMI.save();
                LoadListOredenes();
                ltsOrdenes.SelectedIndex = ltsOrdenes.Items.Count - 1 ;
                OP.GetOrdenes((int)ltsOrdenes.SelectedValue);
                llenarElementos();
            }
        }       
        private void ltsOrdenes_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OP.GetOrdenes((int)ltsOrdenes.SelectedValue);
            llenarElementos();
        }
        private void cmbCliente_DropDownClosed(object sender, EventArgs e)
        {
            LoadListOredenesCliente((int)cmbCliente.SelectedValue);
        }
        private void btnVerTodo_Click(object sender, RoutedEventArgs e)
        {
            LoadListOredenes();
            seleccioneParaFiltrar();
        }
    }
}
