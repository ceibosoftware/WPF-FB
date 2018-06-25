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
        float totalf;
        float totalr;
        CRUD conexion = new CRUD();
        public int tipo ;
        public int tipopago= -1;
   
        public windowAgregarPagoProveedor(float totalfact)
        {
            InitializeComponent();
            totalf = totalfact;

            LoadCmbFormaPago();
            OcultarElementos();
            txttotafactura.Text = totalfact.ToString();
           

        }

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
            cmbFormaPago.SelectedIndex =0;
       
        }

        public bool ValidaDatos()
        {
            if (cmbFormaPago.Text == "Efectivo")
            {
                if (txttotafacturaApagar.Text != "")
                {
                    if ( float.Parse(txttotafacturaApagar.Text) > float.Parse(txttotafactura.Text))
                    {
                        MessageBox.Show("El monto ingresado supera el monto restante de la factura", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (txtRecibo.Text =="")
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

     
            if (cmbFormaPago.SelectedIndex ==0)
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
            else if(cmbFormaPago.SelectedIndex ==1)
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
            }
            else if(cmbFormaPago.SelectedIndex == 2)
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
            }
            else if (cmbFormaPago.SelectedIndex == -1)
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


            txttotafactura.IsEnabled = false;
        }
        //PAGO EFECTIVO

        //private void btnEfectivo_Click(object sender, RoutedEventArgs e)
        //{
        //    DialogResult = true;
        //}




        //PAGO CON CHEQUE


        //var newW = new WindowAgregarPagoProveedorCheque(totalc, this.tipo);
        //newW.ShowDialog();

        //if (newW.DialogResult == true)
        //{
        //    banco = newW.cmbBanco.SelectedItem.ToString();
        //    destinatario = newW.txtDestinatario.Text;
        //    numCheque = int.Parse(newW.txtnumeroCheque.Text);
        //    importe = float.Parse(newW.txtImporte.Text);
        //    fecha = newW.dtpFecha.SelectedDate.Value.Date;
        //    fechaCobro = newW.dtpFechaCobro.SelectedDate.Value.Date;
        //    tipo = newW.tipo;

        //    if (tipo == 1)
        //    {
        //        String sql = "INSERT INTO chequesalida (banco, importe,destinatario,numeroCheque, fecha, fechaCobro)VALUES ('" + banco + "','" + importe + "','" + destinatario + "','" + numCheque + "','" + fecha.ToString("yyyy/MM/dd") + "','" + fechaCobro.ToString("yyyy/MM/dd") + "')";
        //        conexion.operaciones(sql);
        //    }
        //    else
        //    {
        //        String sql = "INSERT INTO cheque (banco, importe,destinatario,numeroCheque, fecha, fechaCobro)VALUES ('" + banco + "','" + importe + "','" + destinatario + "','" + numCheque + "','" + fecha.ToString("yyyy/MM/dd") + "','" + fechaCobro.ToString("yyyy/MM/dd") + "')";
        //        conexion.operaciones(sql);
        //    }


        //    string ultimoId = "Select last_insert_id()";
        //    idCheque = conexion.ValorEnVariable(ultimoId);
        //    tipopago = 1;
        ////    this.Close();
        //}
    }

       
    
    
    //PAGO CON CUENTA BANCARIA

        //var newW = new WindowAgregarPagoProveedorCtaBancaria(totalc, tipo);
        //newW.ShowDialog();

        //if (newW.DialogResult == true)
        //{
        //    cbu = newW.cbuu;
        //    nombreTitular = newW.nombreT;

        //    if (tipo == 1)
        //    {
        //        String sql = "INSERT INTO cuentabancosalida (cbu, nombreTitular, montoPagado)VALUES ('" + cbu + "','" + nombreTitular + "','" + float.Parse(newW.txtMonto.Text) + "')";
        //        conexion.operaciones(sql);
        //    }
        //    else
        //    {
        //        String sql = "INSERT INTO cuentabanco (cbu, nombreTitular, montoPagado)VALUES ('" + cbu + "','" + nombreTitular + "','" + float.Parse(newW.txtMonto.Text) + "')";
        //        conexion.operaciones(sql);
        //    }


        //    string ultimoId = "Select last_insert_id()";
        //    idCtaBcria = conexion.ValorEnVariable(ultimoId);
        //    tipopago = 2;
        //    this.Close();
        //}
    
}

