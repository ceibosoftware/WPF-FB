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
using System.Windows.Shapes;

namespace wpfFamiliaBlanco.Proveedores
{
    /// <summary>
    /// Interaction logic for windowAgregarContactoProveedor.xaml
    /// </summary>
    public partial class windowAgregarContactoProveedor : Window
    {
        public windowAgregarContactoProveedor()
        {
            InitializeComponent();
        }

        private void btnAceptar_Copy1_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
