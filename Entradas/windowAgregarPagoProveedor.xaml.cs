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
        public long cbu;
        public String nombreTitular;
        public String banco;
        public String destinatario;
        public int numCheque;
        public float importe;
        public DateTime fecha;
        public DateTime fechaCobro;
        public String idCheque;
        public String idCtaBcria;
        float totalc;
        CRUD conexion = new CRUD();
        public int tipo ;
        public int tipopago= -1;
        public windowAgregarPagoProveedor(float totalcuot, int tipo2)
        {
            InitializeComponent();
            totalc = totalcuot;
            tipo = tipo2;
        }

        private void btnEfectivo_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnCheque_Click(object sender, RoutedEventArgs e)
        {
            var newW = new WindowAgregarPagoProveedorCheque(totalc, this.tipo);
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {
                banco = newW.cmbBanco.SelectedItem.ToString();
                destinatario = newW.txtDestinatario.Text;
                numCheque = int.Parse(newW.txtnumeroCheque.Text);
                importe = float.Parse(newW.txtImporte.Text);
                fecha = newW.dtpFecha.SelectedDate.Value.Date;
                fechaCobro = newW.dtpFechaCobro.SelectedDate.Value.Date;
                tipo = newW.tipo;

                if (tipo == 1)
                {
                    String sql = "INSERT INTO chequesalida (banco, importe,destinatario,numeroCheque, fecha, fechaCobro)VALUES ('" + banco + "','" + importe + "','" + destinatario + "','" + numCheque + "','" + fecha.ToString("yyyy/MM/dd") + "','" + fechaCobro.ToString("yyyy/MM/dd") + "')";
                    conexion.operaciones(sql);
                }
                else
                {
                    String sql = "INSERT INTO cheque (banco, importe,destinatario,numeroCheque, fecha, fechaCobro)VALUES ('" + banco + "','" + importe + "','" + destinatario + "','" + numCheque + "','" + fecha.ToString("yyyy/MM/dd") + "','" + fechaCobro.ToString("yyyy/MM/dd") + "')";
                    conexion.operaciones(sql);
                }
              

                string ultimoId = "Select last_insert_id()";
                idCheque = conexion.ValorEnVariable(ultimoId);
                tipopago = 1;
                this.Close();
            }
        }

        private void btnCtaBancaria_Click(object sender, RoutedEventArgs e)
        {
            var newW = new WindowAgregarPagoProveedorCtaBancaria(totalc, tipo);
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {
                cbu = newW.cbuu;
                nombreTitular = newW.nombreT;

                if (tipo == 1)
                {
                    String sql = "INSERT INTO cuentabancosalida (cbu, nombreTitular, montoPagado)VALUES ('" + cbu + "','" + nombreTitular + "','" + float.Parse( newW.txtMonto.Text )+ "')";
                    conexion.operaciones(sql);
                }
                else {
                    String sql = "INSERT INTO cuentabanco (cbu, nombreTitular, montoPagado)VALUES ('" + cbu + "','" + nombreTitular + "','" + float.Parse(newW.txtMonto.Text) + "')";
                    conexion.operaciones(sql);
                }
            

                string ultimoId = "Select last_insert_id()";
                idCtaBcria = conexion.ValorEnVariable(ultimoId);
                tipopago = 2;
                this.Close();
            }
        }
    }
}
