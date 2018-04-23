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

namespace wpfFamiliaBlanco.Clientes
{
    /// <summary>
    /// Lógica de interacción para windowAgregarProducto.xaml
    /// </summary>
    public partial class windowAgregarProducto : Window
    {
        public windowAgregarProducto()
        {
            InitializeComponent();
            txtnombre.IsReadOnly = true;
            txtpreciou.IsReadOnly = true;
        }

        private Boolean validacion() {


            
            if (string.IsNullOrEmpty(txtpreciolista.Text))
            {
                MessageBox.Show("Complete el precio de lista");
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
    }
}
