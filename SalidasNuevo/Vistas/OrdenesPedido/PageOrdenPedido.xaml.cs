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

namespace wpfFamiliaBlanco.SalidasNuevo.Vistas.OrdenesPedido
{
    /// <summary>
    /// Interaction logic for PageOrdenPedido.xaml
    /// </summary>
    public partial class PageOrdenPedido : Page
    {
        public PageOrdenPedido()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarOP();
            newW.ShowDialog();
            if(newW.DialogResult == true)
            {
                newW.OPMI.save();
            }
        }

        private void mostrarWindow(Window newW)
        {
            newW.ShowDialog();
        }

    }
}
