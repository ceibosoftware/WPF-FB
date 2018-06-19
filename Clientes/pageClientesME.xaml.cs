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

namespace wpfFamiliaBlanco.Clientes
{
    /// <summary>
    /// Interaction logic for pageClientesME.xaml
    /// </summary>
    public partial class pageClientesME : Page
    {
        public pageClientesME()
        {
            InitializeComponent();
            frameInicioClientes.Content = new ME();
            btnClientes.Style = FindResource("botonTabPressed") as Style;

        }


        private void btnLP_Click(object sender, RoutedEventArgs e)
        {
            btnClientes.Style = FindResource("botonTab") as Style;
            btnLP.Style = FindResource("botonTabPressed") as Style;
            frameInicioClientes.Content = new LP();
        }

        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            btnClientes.Style = FindResource("botonTabPressed") as Style;
            btnLP.Style = FindResource("botonTab") as Style;
            frameInicioClientes.Content = new ME();




        }
    }
}
