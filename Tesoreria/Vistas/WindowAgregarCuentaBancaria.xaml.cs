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
using wpfFamiliaBlanco.Tesoreria.Clases;

namespace wpfFamiliaBlanco.Tesoreria.Vistas
{
    /// <summary>
    /// Lógica de interacción para WindowAgregarCuentaBancaria.xaml
    /// </summary>
    public partial class WindowAgregarCuentaBancaria : Window
    {
        CuentaBancaria cta;
        internal CuentaBancaria Cta { get => cta; set => cta = value; }

        public WindowAgregarCuentaBancaria()
        {
            InitializeComponent();
            loadcmbmoneda();
            loadcmbtipo();
        }

        public void llenarDatos()
        {

            int tipo = cmbtipo.SelectedIndex;
            String titular = txttitular.Text;
            string alias = txtalias.Text;
            string banco = txtbco.Text;
            String numero = txtnumero.Text;
            string cbu = txtcbu.Text;
            string obs = txtobs.Text;
            float saldo = float.Parse(txtsaldo.Text);
            int moneda = cmbmoneda.SelectedIndex;

            cta = new CuentaBancaria(tipo, titular, alias, banco, numero, cbu, obs,saldo,moneda);


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
        private Boolean validacion()
        {

            if (cmbtipo.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el tipo de cuenta");
                return false;
            }
            else if (cmbmoneda.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione la moneda");
                return false;
            }
            else if (String.IsNullOrEmpty(txtbco.Text))
            {
                MessageBox.Show("Complete el banco");
                return false;
            }
            else if (String.IsNullOrEmpty(txtsaldo.Text))
            {
                MessageBox.Show("Complete con el saldo de la cuenta");
                return false;

            }
            else if (String.IsNullOrEmpty(txtcbu.Text))
            {
                MessageBox.Show("Complete el cbu");
                return false;
            }
            else if (String.IsNullOrEmpty(txtalias.Text))
            {
                MessageBox.Show("Complete el alias");
                return false;
            }
            else if (String.IsNullOrEmpty(txttitular.Text))
            {
                MessageBox.Show("complete el titular de la cuenta");
                return false;
            }

            return true;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (validacion())
            {
                llenarDatos();
                DialogResult = true;
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
