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

namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Lógica de interacción para WindowAgregarPagoProveedorCheque.xaml
    /// </summary>
    public partial class WindowAgregarPagoProveedorCheque : Window
    {
        public String banco;
        public String destinatario;
        public int numCheque;
        public float importe;
        public DateTime fecha;
        public DateTime fechaCobro;
        public float totc;
        public int tipo;

        public WindowAgregarPagoProveedorCheque(float totalcu, int tipo3)
        {
            InitializeComponent();
            LlenarCmbBanco();
            totc = totalcu;
            txtImporte.Text = totc.ToString();
            txtImporte.IsEnabled = false;
            txtDestinatario.MaxLines = 1;
            txtDestinatario.MaxLength = 25;
            txtnumeroCheque.MaxLines = 1;
            txtnumeroCheque.MaxLength = 20;
            tipo = tipo3;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
        
            if (txtnumeroCheque.Text == "")
            {
                MessageBox.Show("Ingrese un numero de cheque");
            }
            if (txtDestinatario.Text == "")
            {
                MessageBox.Show("Ingrese un destinatario");
            }
            if (dtpFecha.SelectedDate == null)
            {
                MessageBox.Show("Seleccione una fecha");
            }
            if (dtpFechaCobro.SelectedDate == null)
            {
                MessageBox.Show("Seleccione una fecha de cobro");
            }
          
            else if (txtnumeroCheque.Text != "" && txtDestinatario.Text != "" && dtpFechaCobro.SelectedDate != null && dtpFecha.SelectedDate != null)
            {
                MessageBox.Show("impoirte" +txtImporte.Text);
                banco = cmbBanco.SelectedItem.ToString();
                destinatario = txtDestinatario.Text;
                numCheque = int.Parse(txtnumeroCheque.Text);
                importe = float.Parse(txtImporte.Text);
                fecha = dtpFecha.SelectedDate.Value.Date;
                fechaCobro = dtpFechaCobro.SelectedDate.Value.Date;
                
                DialogResult = true;
            }
         
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LlenarCmbBanco()
        {
            cmbBanco.Items.Add("GALICIA");
            cmbBanco.Items.Add("HSBC");
            cmbBanco.Items.Add("SANTANDER RIO");
            cmbBanco.Items.Add("PATAGONIA");
            cmbBanco.SelectedIndex = 0;
        }

        private void txtnumeroCheque_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}
