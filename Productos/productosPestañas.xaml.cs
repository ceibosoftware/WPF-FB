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
using wpfFamiliaBlanco.SalidasNuevo.Vistas.INV;

namespace wpfFamiliaBlanco.Productos
{
    /// <summary>
    /// Interaction logic for productosPestañas.xaml
    /// </summary>
    public partial class productosPestañas : Page
    {
        public productosPestañas()
        {
            InitializeComponent();
            btnProductos.Style = FindResource("botonTabPressed") as Style;
            frameInicioProductos.Content = new pageProductos();
        }

        private void btnProductos_Click(object sender, RoutedEventArgs e)
        {
            frameInicioProductos.Content = new pageProductos();
            btnProductos.Style = FindResource("botonTabPressed") as Style;
            btnINV.Style = FindResource("botonTab") as Style;
            btnClientes.Style = FindResource("botonTab") as Style;
            lblProductosClientes.Content = "Productos";
        }

        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            frameInicioProductos.Content = new pageCategorias();
            btnProductos.Style = FindResource("botonTab") as Style;
            btnINV.Style = FindResource("botonTab") as Style;
            btnClientes.Style = FindResource("botonTabPressed") as Style;
            lblProductosClientes.Content = "Clientes";
        
        }

        private void btnINV_Click(object sender, RoutedEventArgs e)
        {
            frameInicioProductos.Content = new PageINV();
            btnProductos.Style = FindResource("botonTab") as Style;
            btnClientes.Style = FindResource("botonTab") as Style;
            btnINV.Style = FindResource("botonTabPressed") as Style;
            lblProductosClientes.Content = "INV";
        }
    }
}
