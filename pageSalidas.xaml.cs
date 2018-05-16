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
using wpfFamiliaBlanco.Salidas.Facturacion;

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
        }

        private void btnOrdenes_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
