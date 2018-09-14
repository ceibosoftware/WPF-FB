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

namespace wpfFamiliaBlanco.Tesoreria.Vistas
{
    /// <summary>
    /// Lógica de interacción para WindowAgregarCuentaBancaria.xaml
    /// </summary>
    public partial class WindowAgregarCuentaBancaria : Window
    {
        public WindowAgregarCuentaBancaria()
        {
            InitializeComponent();
            loadcmbmoneda();
            loadcmbtipo();
        }
        
        private void loadcmbmoneda()
        {
            cmbmoneda.Items.Add("EURO");
            cmbmoneda.Items.Add("USD");
            cmbmoneda.Items.Add("PESO");
        }
        private void loadcmbtipo()
        {
            cmbtipo.Items.Add("CA");
            cmbtipo.Items.Add("CC");
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
