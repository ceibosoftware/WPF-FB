using System;
using System.Collections.Generic;
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
using wpfFamiliaBlanco.Salidas;
using wpfFamiliaBlanco.Salidas.Devoluciones;
using wpfFamiliaBlanco.Salidas.Facturacion;
//using wpfFamiliaBlanco.Salidas.Ordenes;
using wpfFamiliaBlanco.Salidas.Pagos;
using wpfFamiliaBlanco.Salidas.Remitos;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for pageSalidas.xaml
    /// </summary>
    public partial class pageSalidas : Page
    {
        public pageSalidas()
        {
            InitializeComponent();
            //frameInicioSalida.Content = new OrdenesSalida();
            btnOrdenes.Style = FindResource("botonTabPressed") as Style;
            btnRemitos.Style = FindResource("botonTab") as Style;
            btnFacturas.Style = FindResource("botonTab") as Style;
            btnPagoProveedor.Style = FindResource("botonTab") as Style;
            btnDevolucionProductos.Style = FindResource("botonTab") as Style;
        }

        private void btnOrdenes_Click(object sender, RoutedEventArgs e)
        {
           // frameInicioSalida.Content = new OrdenesSalida();
            btnOrdenes.Style = FindResource("botonTabPressed") as Style;
            btnRemitos.Style = FindResource("botonTab") as Style;
            btnFacturas.Style = FindResource("botonTab") as Style;
            btnPagoProveedor.Style = FindResource("botonTab") as Style;
            btnDevolucionProductos.Style = FindResource("botonTab") as Style;
        }

        private void btnFacturas_Click(object sender, RoutedEventArgs e)
        {
            frameInicioSalida.Content = new FacturacionSalida();
            btnOrdenes.Style = FindResource("botonTab") as Style;
            btnRemitos.Style = FindResource("botonTab") as Style;
            btnFacturas.Style = FindResource("botonTabPressed") as Style;
            btnPagoProveedor.Style = FindResource("botonTab") as Style;
            btnDevolucionProductos.Style = FindResource("botonTab") as Style;
        }

        private void btnPagoProveedor_Click(object sender, RoutedEventArgs e)
        {
            frameInicioSalida.Content = new PagoClientes();
            btnOrdenes.Style = FindResource("botonTab") as Style;
            btnRemitos.Style = FindResource("botonTab") as Style;
            btnFacturas.Style = FindResource("botonTab") as Style;
            btnPagoProveedor.Style = FindResource("botonTabPressed") as Style;
            btnDevolucionProductos.Style = FindResource("botonTab") as Style;
        }

        private void btnDevolucionProductos_Click(object sender, RoutedEventArgs e)
        {
            frameInicioSalida.Content = new DevolucionProductosClientes();
            btnOrdenes.Style = FindResource("botonTab") as Style;
            btnRemitos.Style = FindResource("botonTab") as Style;
            btnFacturas.Style = FindResource("botonTab") as Style;
            btnPagoProveedor.Style = FindResource("botonTab") as Style;
            btnDevolucionProductos.Style = FindResource("botonTabPressed") as Style;
        }

        private void btnRemitos_Click(object sender, RoutedEventArgs e)
        {
            frameInicioSalida.Content = new RemitoSalida();
            btnOrdenes.Style = FindResource("botonTab") as Style;
            btnRemitos.Style = FindResource("botonTabPressed") as Style;
            btnFacturas.Style = FindResource("botonTab") as Style;
            btnPagoProveedor.Style = FindResource("botonTab") as Style;
            btnDevolucionProductos.Style = FindResource("botonTab") as Style;
        }
    }
}
