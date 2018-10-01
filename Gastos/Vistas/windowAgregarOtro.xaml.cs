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
    /// Interaction logic for windowAgregarOtro.xaml
    /// </summary>
    public partial class windowAgregarOtro : Window
    {
        CRUD conexion = new CRUD();
        public windowAgregarOtro()
        {
            InitializeComponent();
            LoadCMB();
            LoadCMBTIpo();
        }

        public windowAgregarOtro(int tipo, float monto, String nombre, int formap, String obs, DateTime fecha)
        {
            InitializeComponent();
            LoadCMBModificar(formap);
            LoadCMBTIpoModificar(tipo);
            txtNombre.Text = nombre;
            txtMonto.Text = monto.ToString();
            txtObservaciones.Text = obs;
            dtpFecha.SelectedDate = fecha.Date;
            
        }
        private void LoadCMBTIpoModificar(int t)
        {
            cmbTipo.Items.Add("Ingresos Brutos");
            cmbTipo.Items.Add("IVA");
            cmbTipo.Items.Add("Otro");
            if (t == 0)
            {
                cmbTipo.SelectedIndex = 0; cmbTipo.SelectedIndex = 0;
            }
            else if (t == 1)
            {
                cmbTipo.SelectedIndex = 1;

            }
            else
            {
                cmbTipo.SelectedIndex = 2;
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
            cmbTipo.Items.Add("Ingresos Brutos");
            cmbTipo.Items.Add("IVA");
            cmbTipo.Items.Add("Otro");
            cmbTipo.SelectedIndex = 0;
        }
        private void txtMonto_KeyDown(object sender, KeyEventArgs e)
        {

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
            else
            {
                return true;

            }
        }
    }
}
