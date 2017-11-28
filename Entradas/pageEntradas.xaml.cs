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
using wpfFamiliaBlanco.Entradas;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for pageEntradas.xaml
    /// </summary>
    public partial class pageEntradas : Page
    {
        public pageEntradas()
        {
            InitializeComponent();
            frameInicioEntradas.Content = new Ordenes();
        }

        private void btnFacturas_Click(object sender, RoutedEventArgs e)
        {
            frameInicioEntradas.Content = new Facturacion();
            btnOrdenes.Style = FindResource("botonTab") as Style;
            btnRemitos.Style = FindResource("botonTab") as Style;
            btnFacturas.Style = FindResource("botonTabPressed") as Style;
            btnPagoProveedor.Style = FindResource("botonTab") as Style;
        }

        private void btnOrdenes_Click(object sender, RoutedEventArgs e)
        {
            frameInicioEntradas.Content = new Ordenes();
            btnOrdenes.Style = FindResource("botonTabPressed") as Style;
            btnRemitos.Style = FindResource("botonTab") as Style;
            btnFacturas.Style = FindResource("botonTab") as Style;
            btnPagoProveedor.Style = FindResource("botonTab") as Style;
        }

        private void btnRemitos_Click(object sender, RoutedEventArgs e)
        {
            frameInicioEntradas.Content = new Remito();
            btnOrdenes.Style = FindResource("botonTab") as Style;
            btnRemitos.Style = FindResource("botonTabPressed") as Style;
            btnFacturas.Style = FindResource("botonTab") as Style;
            btnPagoProveedor.Style = FindResource("botonTab") as Style;
        }
    }
}
