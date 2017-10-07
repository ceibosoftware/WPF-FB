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

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for windowModificarProveedor.xaml
    /// </summary>
    public partial class windowModificarProveedor : Window
    {
        public windowModificarProveedor()
        {
            InitializeComponent();
            CargarCMB();
        }

        public windowModificarProveedor(String cuit, String nombre, String razonSocial, String direccion, String cp, String localidad, List list)
        {
           
        }

        public void CargarCMB()
        {
            cmbRazonSocial.Items.Add("SA");
            cmbRazonSocial.Items.Add("Responsable Inscripto");
            cmbRazonSocial.Items.Add("MOnotributista");
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
