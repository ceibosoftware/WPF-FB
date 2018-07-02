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
    /// Interaction logic for windowMonedaOC.xaml
    /// </summary>
    public partial class windowMonedaOC : Window
    {
        public int moneda;
        public windowMonedaOC()
        {
            InitializeComponent();
        }

        private void btnARS_Click(object sender, RoutedEventArgs e)
        {
            moneda = 0;
            this.Close();
        }

        private void btnUSD_Click(object sender, RoutedEventArgs e)
        {
            moneda = 1;
            this.Close();
        }

        private void btnEUR_Click(object sender, RoutedEventArgs e)
        {
            moneda = 2;
            this.Close();
        }
    }
}
