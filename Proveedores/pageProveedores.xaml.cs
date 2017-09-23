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
using wpfFamiliaBlanco.Proveedores;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for pageProveedores.xaml
    /// </summary>
    public partial class pageProveedores : Page
    {
        public pageProveedores()
        {
            InitializeComponent();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e) //btnModificarProveedor_Click
        {
            var newW = new windowModificarProveedor();
            newW.Show();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarProveedor();
            newW.Show();
        }
    }
}
