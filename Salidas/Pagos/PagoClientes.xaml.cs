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
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpfFamiliaBlanco.Entradas;

namespace wpfFamiliaBlanco.Salidas.Pagos
{
    /// <summary>
    /// Lógica de interacción para PagoClientes.xaml
    /// </summary>
    public partial class PagoClientes : Page
    {
        CRUD conexion = new CRUD();
        public List<Producto> ProximosPagos = new List<Producto>();
        DataTable porPagar;
        DataTable pagado;
        DataTable Pagadocheque;
        DateTime fecha;
        bool bandera = false;
        pageEntradas pe = new pageEntradas();
        public PagoClientes()
        {
            InitializeComponent();
          //  rbInterno.IsChecked = true;
            loadLtsProximosPagos();
            loadLtsPagosRealizados();
            txtEstado.IsReadOnly = true;
            txtfecha.IsReadOnly = true;
            txtNumeroCuota.IsReadOnly = true;
            txtnumerocuotapaga.IsReadOnly = true;
            txtNumeroFactura.IsReadOnly = true;
            txtProveedor.IsReadOnly = true;
            txtTipoPago.IsReadOnly = true;
            txtTotalCuota.IsReadOnly = true;
            txtTotalFactura.IsReadOnly = true;
            txtTotalPAgo.IsReadOnly = true;

    

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAgregarPago_Click(object sender, RoutedEventArgs e)
        {
            bandera = true;
            int idcuota = (int)ltsProximospagos.SelectedValue;
            String totalCuota = txtTotalCuota.Text;

            String id = "SELECT FK_idfacturas FROM cuotassalida WHERE idCuota = '" + ltsProximospagos.SelectedValue + "'";

            String idfac = conexion.ValorEnVariable(id);

            var newW = new windowAgregarPagoProveedor(float.Parse(txtTotalCuota.Text),  1);
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {

                String sql = "INSERT INTO pagosalida (fecha, formaPago,efectivo ,FK_idCuota)VALUES ('" + fecha.ToString("yyyy/MM/dd") + "','" + 0 + "','" + totalCuota + "','" + idcuota + "')";
                conexion.operaciones(sql);

                String sq2l = "UPDATE cuotassalida SET estado = '" + 1 + "' where idCuota = '" + idcuota + "' and FK_idfacturas = '" + idfac + "'";
                conexion.operaciones(sq2l);


                MessageBox.Show("El pago se ha realizado correctamente", "Información", MessageBoxButton.OK,MessageBoxImage.Information);
                pageEntradas pe = new pageEntradas();




            }
            else if (newW.tipopago == 1)
            {

      
                String sql = "INSERT INTO pagosalida (fecha, formaPago,FK_idCheque ,FK_idCuota)VALUES ('" + fecha.ToString("yyyy/MM/dd") + "','" + 1 + "','" + newW.idCheque + "','" + idcuota + "')";
                conexion.operaciones(sql);

                String sq2l = "UPDATE cuotassalida SET estado = '" + 1 + "' where idCuota = '" + idcuota + "' and FK_idfacturas = '" + idfac + "'";
                conexion.operaciones(sq2l);
                MessageBox.Show("El pago se ha realizado correctamente", "Información", MessageBoxButton.OK,MessageBoxImage.Information);
            }
            else if (newW.tipopago == 2)
            {
                String sql = "INSERT INTO pagosalida (fecha, formaPago,FK_idCuentaBco ,FK_idCuota)VALUES ('" + fecha.ToString("yyyy/MM/dd") + "','" + 2 + "','" + newW.idCtaBcria + "','" + idcuota + "')";
                conexion.operaciones(sql);

                String sq2l = "UPDATE cuotassalida SET estado = '" + 1 + "' where idCuota = '" + idcuota + "' and FK_idfacturas = '" + idfac + "'";
                conexion.operaciones(sq2l);
                MessageBox.Show("El pago se ha realizado correctamente", "Información", MessageBoxButton.OK,MessageBoxImage.Information);
            }

            loadLtsProximosPagos();
            loadLtsPagosRealizadosMOve();
            bandera = false;
            //MainWindow.notificaciones();
        }

        public void loadLtsProximosPagos()
        {
           
            String consulta = "SELECT * FROM cuotassalida WHERE estado = '" + 0 + "' ORDER BY fecha ASC ";
            conexion.Consulta(consulta, tabla: ltsProximospagos);
            ltsProximospagos.DisplayMemberPath = "fecha";
            ltsProximospagos.SelectedValuePath = "idCuota";
            ltsProximospagos.SelectedIndex = 0;
        }

        public void loadLtsPagosRealizados()
        {
            String consulta = "SELECT *  FROM pagosalida ";
            conexion.Consulta(consulta, tabla: ltsPagosReaizados);
            ltsPagosReaizados.DisplayMemberPath = "idPago";
            ltsPagosReaizados.SelectedValuePath = "idPago";



        }

        public void loadLtsPagosRealizadosMOve()
        {
            String consulta = "SELECT *  FROM pagosalida ";
            conexion.Consulta(consulta, tabla: ltsPagosReaizados);
            ltsPagosReaizados.DisplayMemberPath = "idPago";
            ltsPagosReaizados.SelectedValuePath = "idPago";


        }

        private void ltsProximospagos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtNumeroCuota.Visibility = Visibility.Visible;
            txtProveedor.Visibility = Visibility.Visible;
            txtNumeroFactura.Visibility = Visibility.Visible;
            txtTotalCuota.Visibility = Visibility.Visible;
            txtTotalFactura.Visibility = Visibility.Visible;
            btnAgregarPago.Visibility = Visibility.Visible;
            lbl1.Visibility = Visibility.Visible;
            lbl2.Visibility = Visibility.Visible;
            lbl3.Visibility = Visibility.Visible;
            lbl4.Visibility = Visibility.Visible;
            lbl5.Visibility = Visibility.Visible;

            //try
            //{

                if (!bandera)
                {
                    btnEliminarPago.Visibility = Visibility.Collapsed;


                    String ifFactura = "SELECT FK_idfacturas FROM cuotassalida WHERE idCuota = '" + ltsProximospagos.SelectedValue + "'";
                    String idfac = conexion.ValorEnVariable(ifFactura);
                    Console.WriteLine("ID " + idfac);
                    String sql = "SELECT  c.montoCuota,c.numCuota, f.total, f.numeroFactura, c.fecha, c.estado FROM cuotassalida c, facturasalida f WHERE  c.idCuota = '" + ltsProximospagos.SelectedValue + "' AND f.idfacturas = '" + idfac + "'";
                    porPagar = conexion.ConsultaParametrizada(sql, ltsProximospagos.SelectedValue);

                    txtTotalCuota.Text = porPagar.Rows[0].ItemArray[0].ToString();
                    txtNumeroCuota.Text = porPagar.Rows[0].ItemArray[1].ToString();
                    txtTotalFactura.Text = porPagar.Rows[0].ItemArray[2].ToString();
                    txtNumeroFactura.Text = porPagar.Rows[0].ItemArray[3].ToString();


                    fecha = (DateTime)porPagar.Rows[0].ItemArray[4];
                    DateTime hoy = DateTime.Now;

                    TimeSpan ts = fecha - hoy;

                    // Difference in days.
                    int differenceInDays = ts.Days;

                    Console.WriteLine("diferencia de dias " + differenceInDays);

                    if (differenceInDays <= 0)
                    {
                        Console.WriteLine("DIAS " + differenceInDays + " VENCIDO");
                        ltsProximospagos.Background = Brushes.Red;
                        txtTotalPAgo.BorderBrush = Brushes.Red;
                        txtTipoPago.BorderBrush = Brushes.Red;
                        txtfecha.BorderBrush = Brushes.Red;
                        txtnumerocuotapaga.BorderBrush = Brushes.Red;
                        txtEstado.BorderBrush = Brushes.Red;
                        lbldias.Content = "Venció hace " + differenceInDays * -1 + " días";
                    }
                    else if (differenceInDays <= 10)
                    {
                        Console.WriteLine("DIAS " + differenceInDays + " PROXIMO A VENCER");
                        ltsProximospagos.Background = Brushes.Orange;
                        txtTotalPAgo.BorderBrush = Brushes.Orange;
                        txtTipoPago.BorderBrush = Brushes.Orange;
                        txtfecha.BorderBrush = Brushes.Orange;
                        txtnumerocuotapaga.BorderBrush = Brushes.Orange;
                        txtEstado.BorderBrush = Brushes.Orange;
                        lbldias.Content = "Faltan " + differenceInDays + " días para su vencimiento";
                    }

                    else if (differenceInDays >= 11)
                    {
                        Console.WriteLine("DIAS " + differenceInDays + " POR PAGAR");
                        ltsProximospagos.Background = Brushes.Yellow;
                        txtTotalPAgo.BorderBrush = Brushes.Yellow;
                        txtTipoPago.BorderBrush = Brushes.Yellow;
                        txtfecha.BorderBrush = Brushes.Yellow;
                        txtnumerocuotapaga.BorderBrush = Brushes.Yellow;
                        txtEstado.BorderBrush = Brushes.Yellow;
                        lbldias.Content = "Faltan " + differenceInDays + " días para su vencimiento";
                    }


                    String estado = porPagar.Rows[0].ItemArray[5].ToString();

                    if (estado == "False")
                    {
                        txtEstado.Text = "NO PAGADO";
                        txtEstado.BorderBrush = Brushes.Red;
                    }
                    else
                    {
                        txtEstado.Text = "PAGADO";
                        txtEstado.BorderBrush = Brushes.Green;
                    }

             

                    //if (rbInterno.IsChecked == true)
                    //{
                    //    String nombreProveedor = "SELECT DISTINCT p.nombre FROM cuotassalida c, facturasalida f, ordencomprasalida oc, clientesmi p WHERE c.FK_idfacturas = f.idfacturas AND f.FK_idOrdenCompra = oc.idOrdenCompra AND oc.FK_idClientemi = p.idClientemi AND idCuota = '" + ltsProximospagos.SelectedValue + "'";
                    //    String nombrep = conexion.ValorEnVariable(nombreProveedor);
                    //    txtProveedor.Text = nombrep;
                    //}
                    //else
                    //{
                    //    String nombreProveedor = "SELECT DISTINCT p.nombre FROM cuotassalida c, facturasalida f, ordencomprasalida oc, clientesme p WHERE c.FK_idfacturas = f.idfacturas AND f.FK_idOrdenCompra = oc.idOrdenCompra AND oc.FK_idClienteme = p.idClienteme AND idCuota = '" + ltsProximospagos.SelectedValue + "'";
                    //    String nombrep = conexion.ValorEnVariable(nombreProveedor);
                    //    txtProveedor.Text = nombrep;
                    //}


                    txtTotalPAgo.Text = "El pago no se ha realizado";
                    txtTipoPago.Text = "El pago no se ha realizado";
                    txtfecha.Text = "El pago no se ha realizado";
                    txtnumerocuotapaga.Text = "El pago no se ha realizado";

                }
               // ltsPagosReaizados.SelectedIndex = -1;
            //}
            //catch (Exception)
            //{


            //}

        }

        private void ltsPagosReaizados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEliminarPago.Visibility = Visibility.Visible;

            btnAgregarPago.Visibility = Visibility.Collapsed;
            txtNumeroCuota.Visibility = Visibility.Collapsed;
            txtProveedor.Visibility = Visibility.Collapsed;
            txtNumeroFactura.Visibility = Visibility.Collapsed;
            txtTotalCuota.Visibility = Visibility.Collapsed;
            txtTotalFactura.Visibility = Visibility.Collapsed;
            lbl1.Visibility = Visibility.Collapsed;
            lbl2.Visibility = Visibility.Collapsed;
            lbl3.Visibility = Visibility.Collapsed;
            lbl4.Visibility = Visibility.Collapsed;
            lbl5.Visibility = Visibility.Collapsed;
            if (!bandera)
            {

                String sql = "SELECT DISTINCT p.FK_idcheque ,p.FK_idCuentaBco,p.fecha,c.idCuota,c.estado,p.efectivo,formaPago FROM pagosalida p , cuotassalida c  WHERE p.idPago = @valor AND p.FK_idCuota = c.idCuota";
                pagado = conexion.ConsultaParametrizada(sql, ltsPagosReaizados.SelectedValue);

                String FK_idCheque = pagado.Rows[0].ItemArray[0].ToString();
                String FK_ctaBco = pagado.Rows[0].ItemArray[1].ToString();
                txtfecha.Text = pagado.Rows[0].ItemArray[2].ToString();
                txtnumerocuotapaga.Text = pagado.Rows[0].ItemArray[3].ToString();
                String estado1 = pagado.Rows[0].ItemArray[4].ToString();


                if (FK_idCheque != "")
                {
                    String importe = "SELECT importe FROM chequesalida WHERE idCheque = '" + FK_idCheque + "'";
                    String importeCheque = conexion.ValorEnVariable(importe);
                    txtTipoPago.Text = "Cheque";
                    txtTotalPAgo.Text = importeCheque;

                }
                else if (FK_ctaBco != "")
                {
                    String importe = "SELECT montoPagado FROM cuentabancosalida WHERE idCuentaBco = '" + FK_ctaBco + "'";
                    String importebco = conexion.ValorEnVariable(importe);
                    txtTipoPago.Text = "Cuenta Bancaria";
                    txtTotalPAgo.Text = importebco;
                }
                else
                {
                    txtTotalPAgo.Text = pagado.Rows[0].ItemArray[5].ToString();
                }

                if (pagado.Rows[0].ItemArray[6].ToString() == "0")
                {
                    txtTipoPago.Text = "Efectivo";
                }

                if (estado1 == "True")
                {

                    txtEstado.Text = "PAGADO";
                    txtEstado.BorderBrush = Brushes.Green;
                }


                txtTotalPAgo.BorderBrush = Brushes.Green;
                txtTipoPago.BorderBrush = Brushes.Green;
                txtfecha.BorderBrush = Brushes.Green;
                txtnumerocuotapaga.BorderBrush = Brushes.Green;

            }

        }

        private void btnEliminarPago_Click(object sender, RoutedEventArgs e)
        {
            bandera = true;
            String tipo = txtTipoPago.Text;
            MessageBoxResult dialog = MessageBox.Show("¿Esta seguro que desea eliminar el pago número " + ltsPagosReaizados.SelectedValue, "Advertencia", MessageBoxButton.YesNo,MessageBoxImage.Warning);


            if (dialog == MessageBoxResult.Yes)
            {
                String id = "SELECT FK_idCuota FROM pagosalida WHERE idPago = '" + ltsPagosReaizados.SelectedValue + "'";
                String idcuot = conexion.ValorEnVariable(id);

                String consulta = "UPDATE cuotassalida SET estado =  '" + 0 + "' where idCuota = '" + idcuot + "'";
                conexion.operaciones(consulta);

                if (tipo == "Efectivo")
                {
                    string sql2 = "DELETE  FROM pagosalida WHERE idPago = '" + ltsPagosReaizados.SelectedValue + "' AND FK_idCuota = '" + idcuot + "'";
                    conexion.operaciones(sql2);
                }
                else if (tipo == "Cheque")
                {
                    String idc = "SELECT FK_idCheque FROM pagosalida WHERE idPago = '" + ltsPagosReaizados.SelectedValue + "'";
                    String idch = conexion.ValorEnVariable(idc);

                    string sql2 = "DELETE  FROM pagosalida WHERE idPago = '" + ltsPagosReaizados.SelectedValue + "' AND FK_idCuota = '" + idcuot + "'";
                    conexion.operaciones(sql2);

                    string sql3 = "DELETE  FROM chequesalida WHERE idCheque = '" + idch + "'";
                    conexion.operaciones(sql3);
                }
                else if (tipo == "Cuenta Bancaria")
                {
                    String idc = "SELECT FK_idCuentaBco FROM pagosalida WHERE idPago = '" + ltsPagosReaizados.SelectedValue + "'";
                    String idbco = conexion.ValorEnVariable(idc);

                    string sql2 = "DELETE  FROM pagosalida WHERE idPago = '" + ltsPagosReaizados.SelectedValue + "' AND FK_idCuota = '" + idcuot + "'";
                    conexion.operaciones(sql2);

                    string sql3 = "DELETE  FROM cuentabancosalida WHERE idCuentaBco = '" + idbco + "'";
                    conexion.operaciones(sql3);
                }


                loadLtsProximosPagos();
                loadLtsPagosRealizados();
                bandera = false;
                MessageBox.Show("El pago se eliminó correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

