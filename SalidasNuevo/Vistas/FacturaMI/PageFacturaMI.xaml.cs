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

namespace wpfFamiliaBlanco.SalidasNuevo.Vistas.FacturaMI
{
    /// <summary>
    /// Interaction logic for PageFacturaMI.xaml
    /// </summary>
    public partial class PageFacturaMI : Page
    {

        Cliente cliente = new Cliente();
        OrdenPedido OP = new OrdenPedido();
        public PageFacturaMI()
        {
            InitializeComponent();
            LoadCMBClientes();
        }

        private void LoadCMBClientes()
        {
            cmbClientes.ItemsSource = cliente.getClientes().AsDataView();
            cmbClientes.DisplayMemberPath = "nombre";
            cmbClientes.SelectedValuePath = "idClientemi";
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            WindowAgregarFMI newW = new WindowAgregarFMI();
            newW.ShowDialog();
        }

        private void btnModificar_Copy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnVertodo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbClientes_DropDownClosed(object sender, EventArgs e)
        {
            cmbordenCompra.ItemsSource = OP.GetOrdenesCliente((int)cmbClientes.SelectedValue).AsDataView();
            cmbordenCompra.SelectedValuePath = "idOPMI";
            cmbordenCompra.DisplayMemberPath = "idOPMI";
        }
    }
}
