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

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for windowAjustes.xaml
    /// </summary>
    public partial class windowAjustes : Window
    {
        CRUD conexion = new CRUD();
        public windowAjustes()
        {
            InitializeComponent();
            LoadCotizacion();
        }

        private void LoadCotizacion()
        {
            string consultaDolar = "SELECT cotizacion from cotizacion where nombre = 'dolar'";
            string consultaEuro = "SELECT cotizacion from cotizacion where nombre = 'euro'";         
            txtUSD.Text = conexion.ValorEnVariable(consultaDolar); 
            txtEUR.Text = conexion.ValorEnVariable(consultaEuro);
        }

        private void actualizarDolar()
        {
            try
            {
                string consultaDolar = "UPDATE cotizacion set cotizacion = '" + txtUSD.Text + "' where nombre = 'dolar'";
                conexion.operaciones(consultaDolar);
                MessageBox.Show("Cotizacion acualizada a :" + txtUSD.Text);
            }
            catch (Exception)
            {

                MessageBox.Show("Error al actuaizar la cotizacion");
            }
           
            
        }
        private void actualizarEuro()
        {
            try
            {
                string consultaEUR = "UPDATE cotizacion set cotizacion = '" + txtEUR.Text + "' where nombre = 'euro'";
                conexion.operaciones(consultaEUR);
                MessageBox.Show("Cotizacion acualizada a :" + txtEUR.Text);
            }
            catch (Exception)
            {

                MessageBox.Show("Error al actuaizar la cotizacion");
            }
       
        }

        private void btnActualizarUSD_Click(object sender, RoutedEventArgs e)
        {
            actualizarDolar();
        }

        private void btnActualizarEUR_Click(object sender, RoutedEventArgs e)
        {
            actualizarEuro();
        }

        private void txtUSD_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtEUR_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }
    }
}
