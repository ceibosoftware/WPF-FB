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
        DataTable porPagar;
        bool paga = false;
        public WindowAgregarPagoFactura()
        {
            InitializeComponent();
            txtmontorealido.IsEnabled = false;
            txttotalfactura.IsEnabled = false;
            txttotalrestante.IsReadOnly = true;
        }

        public void LoadLtsPagosRealizados()
        {
            String consulta = "SELECT *  FROM pago WHERE FK_idfacturas = '" + idfactura + "' ";
            conexion.Consulta(consulta, tabla: ltsPagosRealizados);
            ltsPagosRealizados.DisplayMemberPath = "idPago";
            ltsPagosRealizados.SelectedValuePath = "idPago";
            ltsPagosRealizados.SelectedIndex = 0;
        }

        public void RefrescarCantidadRestante()
        {
            String consulta = "SELECT totalRestante  FROM factura WHERE idfacturas = '" + idfactura + "' ";
            float actualizado = float.Parse(conexion.ValorEnVariable(consulta));

            txttotalrestante.Text = actualizado.ToString();
        }

        public WindowAgregarPagoFactura(float tf, int idfac, float restante)
        {
            InitializeComponent();
            BloquearElementos();
            AsignarValores(tf, idfac, restante);
            LoadLtsPagosRealizados();
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

        private void AsignarValores(float t, int id, float r)
        {
            totalFactura = t;
            idfactura = id;
            totalresfactura = r;
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
                txtRecibo.Text = porPagar.Rows[0].ItemArray[7].ToString();
            }
        }

        private void btnAgregarPago_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarPagoProveedor(totalresfactura);

            String totalre2 = "SELECT totalRestante FROM factura WHERE idfacturas = '" + idfactura + "'";
            float totalRestant2e = float.Parse(conexion.ValorEnVariable(totalre2));
            if (totalRestant2e == 0)
            {
                MessageBox.Show("La factura seleccionada ya se pago en su totalidad", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                newW.ShowDialog();
            }

            paga = true;
            if (newW.DialogResult == true)
            {
                if (newW.cmbFormaPago.Text == "Efectivo")
                {


                    String totalre = "SELECT totalRestante FROM factura WHERE idfacturas = '" + idfactura + "'";
                    float totalRestante = float.Parse(conexion.ValorEnVariable(totalre));

                    totalRestante = totalRestante - float.Parse(newW.txttotafacturaApagar.Text);

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

                }
                else
                {

                }
                paga = false;
            }
        }

        public float ActualizaRestanteFactura(float total, float valor)
        {


            total = total - valor;


            return total;
        }
    }
}
