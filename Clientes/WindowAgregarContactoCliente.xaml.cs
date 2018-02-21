using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para WindowAgregarContactoCliente.xaml
    /// </summary>
    public partial class WindowAgregarContactoCliente : Window
    {
        public WindowAgregarContactoCliente()
        {
            InitializeComponent();
            txtNombreContacto.MaxLength = 25;
            txtMailContacto.MaxLines = 1;
            txtNombreContacto.MaxLines = 1;
            txtTelefonoContacto.MaxLines = 1;
            txtMailContacto.MaxLength = 30;
            txtTelefonoContacto.MaxLength = 13;
            txtTelefonoContacto.Focus();
        }


       
        private void btnCancelar_Copy1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtTelefonoContacto_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void txtNombreContacto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z-ñ]"))
            {
                e.Handled = true;
            }
        }

        private void btnAceptar_Copy1_Click_1(object sender, RoutedEventArgs e)
        {
            if (txtMailContacto.Text == "")
            {
                MessageBox.Show("Ingrese un email");
            }
            else if (!Regex.IsMatch(txtMailContacto.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                MessageBox.Show("Ingrese un email valido");
            }

            else if (txtNombreContacto.Text == "")
            {
                MessageBox.Show("Ingrese un nombre de contacto");
            }
            else if (txtTelefonoContacto.Text == "")
            {
                MessageBox.Show("Ingrese un telefono");
            }
            //else if (!Regex.IsMatch(txtTelefonoContacto.Text, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"))
            //{
              //  MessageBox.Show("Ingrese un telefono valido");
            //}
            else
            {
                DialogResult = true;
            }

           
        }

        private void btnCancelar_Copy1_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
