using System;
using System.Collections.Generic;
using System.Data;
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
    /// Lógica de interacción para WindowAgregarPagoFactura.xaml
    /// </summary>
    public partial class WindowAgregarPagoFactura : Window
    {

        public float totalFactura;
        CRUD conexion = new CRUD();
        public int idfactura;
        public float totalresfactura;
        public string moneda;
        DataTable porPagar;
        bool paga = false;
        public WindowAgregarPagoFactura()
        {
            InitializeComponent();
            OcultarElementos();

            if (windowUsuarios.tipoUsuarioDB == "basico")
            {
                this.btnAgregarPago.Visibility = Visibility.Collapsed;
               
            }
        }

        public void LoadLtsPagosRealizados()
        {
            String consulta = "SELECT *  FROM pago WHERE FK_idfacturas = '" + idfactura + "' ";
            conexion.Consulta(consulta, tabla: ltsPagosRealizados);
            ltsPagosRealizados.DisplayMemberPath = "idPago";
            ltsPagosRealizados.SelectedValuePath = "idPago";
            ltsPagosRealizados.SelectedIndex = 0;
        }


        public void OcultarElementos()
        {
            txtmontorealido.IsEnabled = false;
            txttotalfactura.IsEnabled = false;
            txttotalrestante.IsReadOnly = true;
            txtChequeNumero1.Visibility = Visibility.Collapsed;
            lblnrocheque.Visibility = Visibility.Collapsed;
            lblDestinatario.Visibility = Visibility.Collapsed;
            txtDestinatario.Visibility = Visibility.Collapsed;

        }

        public void RefrescarCantidadRestante()
        {
            String consulta = "SELECT totalRestante  FROM factura WHERE idfacturas = '" + idfactura + "' ";
            float actualizado = float.Parse(conexion.ValorEnVariable(consulta));

            txttotalrestante.Text = actualizado.ToString();
        }

        public WindowAgregarPagoFactura(float tf, int idfac, float restante, string moned)
        {
            InitializeComponent();
            BloquearElementos();
            AsignarValores(tf, idfac, restante, moned);
            LoadLtsPagosRealizados();
            if (windowUsuarios.tipoUsuarioDB == "basico")
            {
                this.btnAgregarPago.Visibility = Visibility.Collapsed;

            }
        }

        private void BloquearElementos()
        {
            txtmontorealido.IsReadOnly = true;
            txttotalfactura.IsReadOnly = true;
            txttotalrestante.IsReadOnly = true;
            txtForma.IsReadOnly = true;
            txtfechapago.IsReadOnly = true;
            txtRecibo.IsReadOnly = true;
        }

        private void AsignarValores(float t, int id, float r, string m)
        {
            totalFactura = t;
            idfactura = id;
            totalresfactura = r;
            moneda = m;
            txttotalfactura.Text = totalFactura.ToString();
            txttotalrestante.Text = totalresfactura.ToString();
        }
        private void ltsfacturas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!paga)
            {


                String sql = "SELECT * FROM pago WHERE FK_idfacturas = '" + idfactura + "' AND idPago = '" + ltsPagosRealizados.SelectedValue + "'";
                porPagar = conexion.ConsultaParametrizada(sql, ltsPagosRealizados.SelectedValue);

                txtmontorealido.Text = porPagar.Rows[0].ItemArray[3].ToString();
                txtfechapago.Text = porPagar.Rows[0].ItemArray[1].ToString();
                txtForma.Text = porPagar.Rows[0].ItemArray[2].ToString();
                txtRecibo.Text = porPagar.Rows[0].ItemArray[6].ToString();

                if (txtForma.Text == "Transferencia" )
                {
                    txtBanco.Visibility = Visibility.Visible;
                    lblbanco.Visibility = Visibility.Visible;
                    txtnrocta.Visibility = Visibility.Visible;
                    lblnrocta.Visibility = Visibility.Visible;
                    lblttutal.Visibility = Visibility.Visible;
                    txttitular.Visibility = Visibility.Visible;
                    txtChequeNumero1.Visibility = Visibility.Collapsed;
                    lblnrocheque.Visibility = Visibility.Collapsed;
                    lblDestinatario.Visibility = Visibility.Collapsed;
                    txtDestinatario.Visibility = Visibility.Collapsed;
                    String sql2 = "SELECT * FROM cuentabanco c , pago p WHERE p.FK_idCuentaBco = c.idCuentaBco AND p.idPago = '" + ltsPagosRealizados.SelectedValue + "'";
                    DataTable info = conexion.ConsultaParametrizada(sql2, ltsPagosRealizados.SelectedValue);
                    txtBanco.Text = info.Rows[0].ItemArray[3].ToString();
                    txtnrocta.Text = info.Rows[0].ItemArray[1].ToString();
                    txttitular.Text = info.Rows[0].ItemArray[2].ToString();
                }
                else if (txtForma.Text == "Cheque")
                {
                    txtBanco.Visibility = Visibility.Visible;
                    lblbanco.Visibility = Visibility.Visible;
                    txtChequeNumero1.Visibility = Visibility.Visible;
                    lblnrocheque.Visibility = Visibility.Visible;
                    lblDestinatario.Visibility = Visibility.Visible;
                    txtDestinatario.Visibility = Visibility.Visible;
                    txtnrocta.Visibility = Visibility.Collapsed;
                    lblnrocta.Visibility = Visibility.Collapsed;
                    String sql2 = "SELECT * FROM cheque c , pago p WHERE p.FK_idCheque = c.idCheque AND p.idPago = '" + ltsPagosRealizados.SelectedValue + "'";
                    DataTable info = conexion.ConsultaParametrizada(sql2, ltsPagosRealizados.SelectedValue);
                    txtChequeNumero1.Text = info.Rows[0].ItemArray[4].ToString();
                    txtDestinatario.Text = info.Rows[0].ItemArray[3].ToString();
                    txtBanco.Text = info.Rows[0].ItemArray[1].ToString();
                    lblttutal.Visibility = Visibility.Collapsed;
                    txttitular.Visibility = Visibility.Collapsed;

                }
                else
                {
                    txtBanco.Visibility = Visibility.Collapsed;
                    lblbanco.Visibility = Visibility.Collapsed;
                    txtChequeNumero1.Visibility = Visibility.Collapsed;
                    lblnrocheque.Visibility = Visibility.Collapsed;
                    lblDestinatario.Visibility = Visibility.Collapsed;
                    txtDestinatario.Visibility = Visibility.Collapsed;
                    txtnrocta.Visibility = Visibility.Collapsed;
                    lblnrocta.Visibility = Visibility.Collapsed;
                    lblttutal.Visibility = Visibility.Collapsed;
                    txttitular.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void btnAgregarPago_Click(object sender, RoutedEventArgs e)
        {
            

            String totalre2 = "SELECT totalRestante FROM factura WHERE idfacturas = '" + idfactura + "'";
            float totalRestant2e = float.Parse(conexion.ValorEnVariable(totalre2));
            
            if (totalRestant2e == 0)
            {
                MessageBox.Show("La factura seleccionada ya se pago en su totalidad", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var newW = new windowAgregarPagoProveedor(totalresfactura, moneda);
                newW.ShowDialog();
            

            paga = true;
            if (newW.DialogResult == true)
            {
                if (newW.cmbFormaPago.Text == "Efectivo")
                {


                    String totalre = "SELECT totalRestante FROM factura WHERE idfacturas = '" + idfactura + "'";
                    float totalRestante = float.Parse(conexion.ValorEnVariable(totalre));

                    String tipo = "SELECT tipoCambio FROM factura WHERE idfacturas = '" + idfactura + "'";
                    string tipoCambio = conexion.ValorEnVariable(tipo);
                 
                    if (newW.cmbMoneda.Text == "u$d" && tipoCambio == "1")
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text);
                    }
                    else if (newW.cmbMoneda.Text == "$" && tipoCambio == "1")
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text)/ float.Parse(newW.txtcotizacion.Text);
                    }
                    else if (newW.cmbMoneda.Text == "€" && tipoCambio == "2")
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text);
                    }
                    else if (newW.cmbMoneda.Text == "$" && tipoCambio == "2")
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text) / float.Parse(newW.txtcotizacion.Text);
                    }
                    else
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text);
                    }

                    String sq1l = "UPDATE factura SET totalRestante = '" + totalRestante + "' where idfacturas = '" + idfactura + "'";
                    conexion.operaciones(sq1l);

                  

                    String efectivo = "Efectivo";
                    String sql = "INSERT INTO pago (fecha, formaPago,efectivo ,FK_idfacturas, nroRecibo)VALUES ('" + newW.fecha.ToString("yyyy/MM/dd") + "','" + efectivo + "','" + newW.txttotafacturaApagar.Text + "','" + idfactura + "','" + newW.txtRecibo.Text + "')";
                    conexion.operaciones(sql);

                 

                    MessageBox.Show("El pago en efectivo se ha realizado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);


                    LoadLtsPagosRealizados();
                    RefrescarCantidadRestante();
                }
                else if (newW.cmbFormaPago.Text == "Cheque")
                {
                    String totalre = "SELECT totalRestante FROM factura WHERE idfacturas = '" + idfactura + "'";
                    float totalRestante = float.Parse(conexion.ValorEnVariable(totalre));

                    String tipo = "SELECT tipoCambio FROM factura WHERE idfacturas = '" + idfactura + "'";
                    string tipoCambio = conexion.ValorEnVariable(tipo);

                    if (newW.cmbMoneda.Text == "u$d" && tipoCambio == "1")
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text);
                    }
                    else if (newW.cmbMoneda.Text == "$" && tipoCambio == "1")
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text) / float.Parse(newW.txtcotizacion.Text);
                    }
                    else if (newW.cmbMoneda.Text == "€" && tipoCambio == "2")
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text);
                    }
                    else if (newW.cmbMoneda.Text == "$" && tipoCambio == "2")
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text) / float.Parse(newW.txtcotizacion.Text);
                    }
                    else
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text);
                    }

                    String sq1l = "UPDATE factura SET totalRestante = '" + totalRestante + "' where idfacturas = '" + idfactura + "'";
                    conexion.operaciones(sq1l);

                    DateTime fechaCobro = System.DateTime.Now;
                    fechaCobro = newW.dtpFechaCobro.SelectedDate.Value;
                    DateTime fecha = System.DateTime.Now;
                    fecha = newW.dtpFecha.SelectedDate.Value;
                    DateTime fechaPago = System.DateTime.Now;
                    fechaCobro = newW.dtpFechaDelPago.SelectedDate.Value;

                    String sql = "INSERT INTO cheque (banco, importe,destinatario ,numeroCheque,fecha, fechaCobro)VALUES ('" + newW.cmbBanco.Text + "','" + newW.txtImporte.Text + "','" + newW.txtDestinatario.Text + "','" + newW.txtnumeroCheque.Text + "','" + fecha.ToString("yyyy/MM/dd") + "', '" + fechaCobro.ToString("yyyy/MM/dd") + "')";
                    conexion.operaciones(sql);

                    string ultimoId = "Select last_insert_id()";
                    String id = conexion.ValorEnVariable(ultimoId);

                    String efectivo = "Cheque";
                    String sql2 = "INSERT INTO pago (fecha, formaPago,efectivo ,FK_idfacturas, nroRecibo, FK_idCheque)VALUES ('" + fecha.ToString("yyyy/MM/dd") + "','" + efectivo + "','" + newW.txttotafacturaApagar.Text + "','" + idfactura + "','" + newW.txtRecibo.Text + "', '" + id + "')";
                    conexion.operaciones(sql2);

                    MessageBox.Show("El pago con cheque se ha realizado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadLtsPagosRealizados();
                    RefrescarCantidadRestante();
                }
                else
                {
                    String totalre = "SELECT totalRestante FROM factura WHERE idfacturas = '" + idfactura + "'";
                    float totalRestante = float.Parse(conexion.ValorEnVariable(totalre));

                    String tipo = "SELECT tipoCambio FROM factura WHERE idfacturas = '" + idfactura + "'";
                   string tipoCambio = conexion.ValorEnVariable(tipo);

                    if (newW.cmbMoneda.Text == "u$d" && tipoCambio == "1")
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text);
                    }
                    else if (newW.cmbMoneda.Text == "$" && tipoCambio == "1")
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text) / float.Parse(newW.txtcotizacion.Text);
                    }
                    else if (newW.cmbMoneda.Text == "€" && tipoCambio == "2")
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text);
                    }
                    else if (newW.cmbMoneda.Text == "$" && tipoCambio == "2")
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text) / float.Parse(newW.txtcotizacion.Text);
                    }
                    else
                    {
                        totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text);
                    }

                    String sq1l = "UPDATE factura SET totalRestante = '" + totalRestante + "' where idfacturas = '" + idfactura + "'";
                    conexion.operaciones(sq1l);

                    DateTime fecha = System.DateTime.Now;
                    fecha = newW.dtpFechaDelPago.SelectedDate.Value;
                    String efectivo = "Transferencia";
                    String sql = "INSERT INTO pago (fecha, formaPago,efectivo ,FK_idfacturas, nroRecibo, FK_idCuentaBco)VALUES ('" + fecha.ToString("yyyy/MM/dd") + "','" + efectivo + "','" + newW.txttotafacturaApagar.Text + "','" + idfactura + "','" + newW.txtRecibo.Text + "', '"+newW.cmbBanco.SelectedValue+"')";
                    conexion.operaciones(sql);
                    MessageBox.Show("El pago con transferencia se ha realizado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadLtsPagosRealizados();
                    RefrescarCantidadRestante();
                }
                paga = false;
                this.Close();
            }
            }
        }

        public float ActualizaRestanteFactura(float total, float valor)
        {


            total = total - valor;


            return total;
        }
    }
}
