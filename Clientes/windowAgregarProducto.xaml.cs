using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wpfFamiliaBlanco.Clientes
{
    /// <summary>
    /// Lógica de interacción para windowAgregarProducto.xaml
    /// </summary>
    public partial class windowAgregarProducto : Window
    {
        String dika;
        public windowAgregarProducto()
        {
            InitializeComponent();
            txtnombre.IsReadOnly = true;
            txtpreciou.IsReadOnly = true;
            txtpreciolista.MaxLines = 1;

        }

        private Boolean validacion() {


            
            if (string.IsNullOrEmpty(txtpreciolista.Text))
            {
                MessageBox.Show("Complete el campo Precio de la lista", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (validacion())
            {
                
                DialogResult = true;
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        private void txtpreciolista_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtpreciolista_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
