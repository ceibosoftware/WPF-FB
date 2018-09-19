using System;
using System.Collections.Generic;
using System.Data;
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

namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Lógica de interacción para windowAgregarPagoProveedor.xaml
    /// </summary>
    public partial class windowAgregarPagoProveedor : Window
    {
        public Int64 cbu;
        public String nombreTitular;
        public String banco;
        public String destinatario;
        public int numCheque;
        public float importe;
        public DateTime fecha;
        public DateTime fechaCobro;
        public String idCheque;
        public String idCtaBcria;
        public String moneda;
        float totalf =0;
        float totalr;
        CRUD conexion = new CRUD();
        public int tipo;
        public int tipopago = -1;


        public windowAgregarPagoProveedor(float totalfact, string moned)
        {
            InitializeComponent();
            totalf = totalfact;
            moneda = moned;
            txtresultado.IsEnabled = false;
            LoadCmbFormaPago();
            OcultarElementos();
            txttotafactura.Text = totalfact.ToString();
            LoadCmbMoneda();

        }

        private void LoadCmbMoneda()
        {
            if (moneda == "0")
            {
                cmbMoneda.Items.Add("$");
                txtcotizacion.Text = "1";
                txtcotizacion.Visibility = Visibility.Collapsed;
                lblrecibo_Copy1.Visibility = Visibility.Collapsed;
            }
            else if (moneda == "1")
            {
                cmbMoneda.Items.Add("$");
                cmbMoneda.Items.Add("u$d");
            }
            else
            {
                cmbMoneda.Items.Add("$");
                cmbMoneda.Items.Add("€");
            }
          
            cmbMoneda.SelectedIndex = 0;
        }

        //private void TipoMoneda(string m)
        //{
        //    if (m == "0")
        //    {
        //        cmbMoneda.Text = "$";
        //        txtcotizacion.Text = "1";
        //    }
        //    else if(m == "1")
        //    {
        //        cmbMoneda.Text = "u$d";
        //    }
        //    else
        //    {
        //        cmbMoneda.Text = "€";
        //    }

        //}

        private void btnPagar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidaDatos())
            {
                DialogResult = true;
            }
        }

        public void LoadCmbFormaPago()
        {
            cmbFormaPago.Items.Add("Efectivo");
            cmbFormaPago.Items.Add("Cheque");
            cmbFormaPago.Items.Add("Transferencia");
            cmbFormaPago.SelectedIndex = 0;

        }

        public bool ValidaDatos()
        {
            if (cmbFormaPago.Text == "Efectivo")
            {
                if (txttotafacturaApagar.Text != "")
                {
                    if (float.Parse(txttotafacturaApagar.Text) > float.Parse(txttotafactura.Text))
                    {
                        MessageBox.Show("El monto ingresado supera el monto restante de la factura", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (txtRecibo.Text == "")
                    {
                        MessageBox.Show("Ingrese el numero de recibo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        fecha = dtpFechaDelPago.DisplayDate;
                        return true;
                    }

                }
                else
                {
                    MessageBox.Show("Ingrese el monto a pagar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else if (cmbFormaPago.Text == "Cheque")
            {
                if (txttotafacturaApagar.Text != "")
                {
                    return true;
                }
            }
            else
            {
                if (txttotafacturaApagar.Text != "")
                {
                    return true;
                }
            }
            return false;
        }


        public void VerTipoPago()
        {


            if (cmbFormaPago.SelectedIndex == 0)
            {
                pagoEfectivo();
            }
            else if (cmbFormaPago.SelectedIndex == 1)
            {
                PagoCheque();
            }
            else if (cmbFormaPago.SelectedIndex == 2)
            {
                PagoTransferencia();
            }
            else if (cmbFormaPago.SelectedIndex == -1)
            {
                pagoEfectivo();
            }
        }

        public void pagoEfectivo()
        {
            lblBanco.Visibility = Visibility.Collapsed;
            lblCheque.Visibility = Visibility.Collapsed;
            lblImporte.Visibility = Visibility.Collapsed;
            lblFecha.Visibility = Visibility.Collapsed;
            lblFechacobro.Visibility = Visibility.Collapsed;
            lblDestinatario.Visibility = Visibility.Collapsed;
            lbldatoscuentaBancaria.Visibility = Visibility.Collapsed;
            lblcbu.Visibility = Visibility.Collapsed;
            lblnombretitualr.Visibility = Visibility.Collapsed;
            lbldatoscheque.Visibility = Visibility.Collapsed;

            cmbBanco.Visibility = Visibility.Collapsed;
            txtnumeroCheque.Visibility = Visibility.Collapsed;
            txtImporte.Visibility = Visibility.Collapsed;
            dtpFecha.Visibility = Visibility.Collapsed;
            dtpFechaCobro.Visibility = Visibility.Collapsed;
            txtDestinatario.Visibility = Visibility.Collapsed;
            txtCbu.Visibility = Visibility.Collapsed;
            txtNombreTitular.Visibility = Visibility.Collapsed;
        }
        public void PagoCheque()
        {
            lblBanco.Visibility = Visibility.Visible;
            lblCheque.Visibility = Visibility.Visible;
            lblImporte.Visibility = Visibility.Visible;
            lblFecha.Visibility = Visibility.Visible;
            lblFechacobro.Visibility = Visibility.Visible;
            lblDestinatario.Visibility = Visibility.Visible;
            lbldatoscuentaBancaria.Visibility = Visibility.Collapsed;
            lblcbu.Visibility = Visibility.Collapsed;
            lblnombretitualr.Visibility = Visibility.Collapsed;
            lbldatoscheque.Visibility = Visibility.Visible;
            lbldatoscheque.Visibility = Visibility.Visible;

            cmbBanco.Visibility = Visibility.Visible;
            txtnumeroCheque.Visibility = Visibility.Visible;
            txtImporte.Visibility = Visibility.Visible;
            dtpFecha.Visibility = Visibility.Visible;
            dtpFechaCobro.Visibility = Visibility.Visible;
            txtDestinatario.Visibility = Visibility.Visible;
            txtCbu.Visibility = Visibility.Collapsed;
            txtNombreTitular.Visibility = Visibility.Collapsed;

            CargarCmbBancos();
        }
        public void PagoTransferencia()
        {
            lblBanco.Visibility = Visibility.Visible;
            lblCheque.Visibility = Visibility.Collapsed;
            lblImporte.Visibility = Visibility.Collapsed;
            lblFecha.Visibility = Visibility.Collapsed;
            lblFechacobro.Visibility = Visibility.Collapsed;
            lblDestinatario.Visibility = Visibility.Collapsed;
            lbldatoscuentaBancaria.Visibility = Visibility.Visible;
            lblcbu.Visibility = Visibility.Visible;
            lblnombretitualr.Visibility = Visibility.Visible;
            lbldatoscheque.Visibility = Visibility.Collapsed;

            cmbBanco.Visibility = Visibility.Visible;
            txtnumeroCheque.Visibility = Visibility.Collapsed;
            txtImporte.Visibility = Visibility.Collapsed;
            dtpFecha.Visibility = Visibility.Collapsed;
            dtpFechaCobro.Visibility = Visibility.Collapsed;
            txtDestinatario.Visibility = Visibility.Collapsed;
            txtCbu.Visibility = Visibility.Visible;
            txtNombreTitular.Visibility = Visibility.Visible;

            CargarCmbBancos();
            String sql = "SELECT * FROM cuentabanco WHERE idCuentaBco = '" + cmbBanco.SelectedValue + "'";
            DataTable cuentas = conexion.ConsultaParametrizada(sql, cmbBanco.SelectedValue);

            txtNombreTitular.Text = cuentas.Rows[0].ItemArray[2].ToString();
            txtCbu.Text = cuentas.Rows[0].ItemArray[1].ToString();
        }


        public void CargarCmbBancos()
        {
            String consulta = "SELECT * FROM cuentabanco";
            conexion.Consulta(consulta, combo: cmbBanco);
            cmbBanco.DisplayMemberPath = "banco";
            cmbBanco.SelectedValuePath = "idCuentaBco";
            cmbBanco.SelectedIndex = 0;
        }
        private void cmbFormaPago_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VerTipoPago();
        }

        private void txtCbu_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void txtnumeroCheque_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }


        public void OcultarElementos()
        {
            lblBanco.Visibility = Visibility.Collapsed;
            lblCheque.Visibility = Visibility.Collapsed;
            lblImporte.Visibility = Visibility.Collapsed;
            lblFecha.Visibility = Visibility.Collapsed;
            lblFechacobro.Visibility = Visibility.Collapsed;
            lblDestinatario.Visibility = Visibility.Collapsed;
            lbldatoscuentaBancaria.Visibility = Visibility.Collapsed;
            lblcbu.Visibility = Visibility.Collapsed;
            lblnombretitualr.Visibility = Visibility.Collapsed;
            lbldatoscheque.Visibility = Visibility.Collapsed;

            cmbBanco.Visibility = Visibility.Collapsed;
            txtnumeroCheque.Visibility = Visibility.Collapsed;
            txtImporte.Visibility = Visibility.Collapsed;
            dtpFecha.Visibility = Visibility.Collapsed;
            dtpFechaCobro.Visibility = Visibility.Collapsed;
            txtDestinatario.Visibility = Visibility.Collapsed;
            txtCbu.Visibility = Visibility.Collapsed;
            txtNombreTitular.Visibility = Visibility.Collapsed;


            txttotafactura.IsReadOnly =true;
        }

        private void cmbBanco_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

        
            String sql = "SELECT * FROM cuentabanco WHERE idCuentaBco = '" + cmbBanco.SelectedValue + "'";
            DataTable cuentas = conexion.ConsultaParametrizada(sql, cmbBanco.SelectedValue);

            txtNombreTitular.Text = cuentas.Rows[0].ItemArray[2].ToString();
            txtCbu.Text = cuentas.Rows[0].ItemArray[1].ToString();
            }
            catch (Exception)
            {

              
            }
        }

        public float CalculaMonto(float cot)
        {
            float total=0;

            total = float.Parse(txttotafacturaApagar.Text) / cot;


            return total;
        }
        private void txttotafacturaApagar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (txtcotizacion.Text == "")
            //{
            //    MessageBox.Show("Por favor ingrese cotización", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            CacularCotizacion();
        }

        private void cmbMoneda_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CacularCotizacion();
        }

        public void CacularCotizacion()
        {
            if (cmbMoneda.Text == "u$d")
            {
                txtresultado.Text = "";
                txtresultado.Text = txttotafacturaApagar.Text;
            }
            else if (cmbMoneda.Text == "$")
            {
                try
                {

                
                txtresultado.Text = (float.Parse(txttotafacturaApagar.Text) / float.Parse(txtcotizacion.Text)).ToString();

                }
                catch (Exception)
                {

                  
                }
            }

            if (txtcotizacion.Text == "" || txttotafacturaApagar.Text == "")
            {
                txtresultado.Text = "";
            }
        }

        private void txtcotizacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            CacularCotizacion();
        }

        private void txttotafacturaApagar_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Decimal" || e.Key.ToString() == "OemPeriod")
            {
                e.Handled = true;
                txttotafacturaApagar.Text = txttotafacturaApagar.Text + ",";
                txttotafacturaApagar.SelectionStart = txttotafacturaApagar.Text.Length;
            }
        }

        private void txttotafacturaApagar_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
            Regex regex = new Regex("^[,][0-9]+$|^[0-9]*[,]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }
    }
    
}

