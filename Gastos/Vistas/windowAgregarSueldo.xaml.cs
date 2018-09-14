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

namespace wpfFamiliaBlanco.Gastos.Vistas
{
    /// <summary>
    /// Interaction logic for windowAgregarSueldo.xaml
    /// </summary>
    public partial class windowAgregarSueldo : Window
    {
        CRUD conexion = new CRUD();
        public windowAgregarSueldo()
        {
            InitializeComponent();
            LoadCMB();
            LoadCMBTIpo();
        }
        private void LoadCMB()
        {
            cmbFormadePAgo.Items.Add("Efectivo");
            cmbFormadePAgo.Items.Add("Transferencia");
            cmbFormadePAgo.SelectedIndex = 0;
        }

        private void LoadCMBTIpo()
        {
            cmbTipo.Items.Add("Ingresos Brutos");
            cmbTipo.Items.Add("IVA");
            cmbTipo.Items.Add("Otro");
            cmbTipo.SelectedIndex = 0;
        }
        private void txtMonto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Decimal" || e.Key.ToString() == "OemPeriod")
            {
                e.Handled = true;
                txtMonto.Text = txtMonto.Text + ",";
                txtMonto.SelectionStart = txtMonto.Text.Length;
            }
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (Valida())
            {
                DialogResult = true;
            }
        }

        private bool Valida()
        {
            String tieneN = "SELECT COUNT(*) FROM gasto WHERE nombre  = '" + txtNombre.Text + "'";
            String NC = conexion.ValorEnVariable(tieneN).ToString();

            if (txtMonto.Text == "")
            {
                MessageBox.Show("Ingrese el monto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            else if (NC != "0")
            {
                MessageBox.Show("El nombre ingresado ya existe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (txtNombre.Text == "")
            {
                MessageBox.Show("Ingrese el nombre", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (txtObservaciones.Text == "")
            {
                MessageBox.Show("Ingrese las observaciones", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (txtHoras.Text == "")
            {
                MessageBox.Show("Ingrese las horas trabajadas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                return true;

            }
        }
    }
}
