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

namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Interaction logic for windowAgregarOC.xaml
    /// </summary>
    public partial class windowAgregarOC : Window
    {
        public windowAgregarOC()
        {
            InitializeComponent();
            
            cmbCuotas.Items.Add("1");
            cmbCuotas.Items.Add("2");
            cmbCuotas.Items.Add("3");
        }

        private void cmbCuotas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var newW = new windowCuotas();
            newW.ShowDialog();
        }

        private void lblAgregarRemito_Copy_Click(object sender, RoutedEventArgs e)
        {
            
            var newW = new windowAgregarRemito();
           var resultado = MessageBox.Show("Desea agregar la orden de compra? ","Advertencia",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes)
            {
                newW.ShowDialog();
            }
            

        }

        private void btAgregarFactura_Copy_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarFactura();
            var resultado = MessageBox.Show("Desea agregar la orden de compra? ", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes)
            {
                newW.ShowDialog();
            }
           
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowCuotas();
            newW.ShowDialog();
        }

        private void btnAgregar_Copy_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarClienteME();
            newW.ShowDialog();
        }
    }
}
