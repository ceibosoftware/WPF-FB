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

namespace wpfFamiliaBlanco.Tesoreria.Vistas
{
    /// <summary>
    /// Lógica de interacción para PageTesoreria.xaml
    /// </summary>
    public partial class PageTesoreria : Page
    {
        public PageTesoreria()
        {
            InitializeComponent();
        }


        private void btnEfectivo_Click(object sender, RoutedEventArgs e)
        {
            ////frameInicioEntradas.Content = new Efectivo();
            ////btnOrdenes.Style = FindResource("botonTab") as Style;
            ////btnRemitos.Style = FindResource("botonTab") as Style;
            ////btnFacturas.Style = FindResource("botonTabPressed") as Style;
            ////btnPagoProveedor.Style = FindResource("botonTab") as Style;
            ////btnDevolucionProductos.Style = FindResource("botonTab") as Style;
        }

        private void btnCuentaBancaria_Click(object sender, RoutedEventArgs e)
        {
            frameInicioTesoreria.Content = new PageCuentaBancaria();
            btnCuentaBancaria.Style = FindResource("botonTabPressed") as Style;
            btnEfectivo.Style = FindResource("botonTab") as Style;
            btnCheque.Style = FindResource("botonTab") as Style;
            
        }

        private void btnCheque_Click(object sender, RoutedEventArgs e)
        {
            frameInicioTesoreria.Content = new PageCheque();
            btnEfectivo.Style = FindResource("botonTab") as Style;
            btnCheque.Style = FindResource("botonTabPressed") as Style;
            btnCuentaBancaria.Style = FindResource("botonTab") as Style;
            
        }
    }
}
