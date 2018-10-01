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

        public windowAgregarSueldo(int tipo, String nombre, float monto, int horast, String obs, int formap, DateTime fecha, float viat)
        {
            InitializeComponent();
            LoadCMBModificar(formap);
            LoadCMBTIpoModificar(tipo);
            txtHoras.Text = horast.ToString();
            txtMonto.Text = monto.ToString();
            txtNombre.Text = nombre;
            txtObservaciones.Text = obs;
            txtViaticos.Text = viat.ToString();
            dtpFecha.SelectedDate = fecha.Date;
           
        }

        private void LoadCMBTIpoModificar(int t)
        {
            cmbTipo.Items.Add("Mensual");
            cmbTipo.Items.Add("Jornal");

            if (t == 4)
            {
                cmbTipo.SelectedIndex = 1; 
            }
            else if (t == 5)
            {
                cmbTipo.SelectedIndex = 0;

            }
          
        }

        private void LoadCMBModificar(int f)
        {
            cmbFormadePAgo.Items.Add("Efectivo");
            cmbFormadePAgo.Items.Add("Transferencia");
            if (f == 0)
            {
                cmbFormadePAgo.SelectedIndex = 0;
            }
            else
            {
                cmbFormadePAgo.SelectedIndex = 1;
            }

        }

        private void LoadCMB()
        {
            cmbFormadePAgo.Items.Add("Efectivo");
            cmbFormadePAgo.Items.Add("Transferencia");
            cmbFormadePAgo.SelectedIndex = 0;
        }

        private void LoadCMBTIpo()
        {
            cmbTipo.Items.Add("Mensual");
            cmbTipo.Items.Add("Jornal");
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

        private void txtViaticos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Decimal" || e.Key.ToString() == "OemPeriod")
            {
                e.Handled = true;
                txtViaticos.Text = txtViaticos.Text + ",";
                txtViaticos.SelectionStart = txtViaticos.Text.Length;
            }
        }
    }
}
