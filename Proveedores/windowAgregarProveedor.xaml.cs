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

namespace wpfFamiliaBlanco.Proveedores
{
    /// <summary>
    /// Interaction logic for windowAgregarProveedor.xaml
    /// </summary>
    public partial class windowAgregarProveedor : Window
    {
        public windowAgregarProveedor()
        {
            InitializeComponent();
            LlenarComboFiltro();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {

            if (Valida())
            {
                DialogResult = true;
            }
     
        }

        private void LlenarComboFiltro()
        {

            cmbRazonSocial.Items.Add("Responsable Inscripto");
            cmbRazonSocial.Items.Add("SA");
            cmbRazonSocial.Items.Add("SRL");
        }

        public Boolean Valida()
        {
            if (txtCuit.Text != "" && txtDireccion.Text != "" && txtNombre.Text != "" && txtCP.Text != "" && txtLocalidad.Text != "")
            {
                return true;
            }
            else
            {
                MessageBox.Show("Faltan campos por completar");
                return false;
            }
           
        }
    }
}
