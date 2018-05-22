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

namespace wpfFamiliaBlanco.Salidas.Devoluciones
{
    /// <summary>
    /// Lógica de interacción para DevolucionProductosClientes.xaml
    /// </summary>
    public partial class DevolucionProductosClientes : Page
    {
        public DevolucionProductosClientes()
        {
            InitializeComponent();
            frameDevoluciones.Content = new DevolucionRemitoCliente();
            btnRemito.Style = FindResource("botonTabPressed") as Style;
        }

        private void btnRemito_Click(object sender, RoutedEventArgs e)
        {
            frameDevoluciones.Content = new DevolucionRemitoCliente();
            btnFactura.Style = FindResource("botonTab") as Style;
            btnRemito.Style = FindResource("botonTabPressed") as Style;
        }

        private void btnFactura_Click(object sender, RoutedEventArgs e)
        {
            frameDevoluciones.Content = new DevolucionFacturaCliente();
            btnRemito.Style = FindResource("botonTab") as Style;
            btnFactura.Style = FindResource("botonTabPressed") as Style;
        }
    }
}
