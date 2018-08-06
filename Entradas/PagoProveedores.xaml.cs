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


namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Lógica de interacción para PagoProveedores.xaml
    /// </summary>
    public partial class PagoProveedores : Page
    {
        CRUD conexion = new CRUD();
        public List<Producto> ProximosPagos = new List<Producto>();
        DataTable porPagar;
        bool bandera = false;
        pageEntradas pe = new pageEntradas();
        String moneda;
        float restante = 0;
        public PagoProveedores()
        {
            InitializeComponent();
            LoadltsFacturas();
            txtMoneda.IsReadOnly = true;
            txtfecha.IsReadOnly = true;
            txtNumeroCuota.IsReadOnly = true;
            txtnumerocuotapaga.IsReadOnly = true;
            txtRestanteFactura.IsReadOnly = true;
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
            moneda = txtMoneda.Text;
            TipoMoneda(moneda);
     
            var newW = new WindowAgregarPagoFactura(float.Parse(txtTotalFactura.Text), (int) ltsfacturas.SelectedValue, (float)restante, moneda);
           
            newW.ShowDialog();

        }


        private void TipoMoneda(string m)
        {
            if (m == "$")
            {
                moneda = "0";
            }
            else if (m == "u$d")
            {
              moneda = "1";
            }
            else
            {
                moneda = "2";
            }

        }

        public void LoadltsFacturas()
        {
            String consulta = "SELECT *  FROM factura ";
            conexion.Consulta(consulta, tabla: ltsfacturas);
            ltsfacturas.DisplayMemberPath = "numeroFactura";
            ltsfacturas.SelectedValuePath = "idfacturas";
            ltsfacturas.SelectedIndex = 0;



        }

        public void loadLtsPagosRealizadosMOve()
        {


        }

        private void ltsProximospagos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //txtNumeroCuota.Visibility = Visibility.Visible;
            //txtProveedor.Visibility = Visibility.Visible;
            //txtNumeroFactura.Visibility = Visibility.Visible;
            //txtTotalCuota.Visibility = Visibility.Visible;
            //txtTotalFactura.Visibility = Visibility.Visible;
            //btnAgregarPago.Visibility = Visibility.Visible;
            //lbl1.Visibility = Visibility.Visible;
            //lbl2.Visibility = Visibility.Visible;
            //lbl3.Visibility = Visibility.Visible;
            //lbl4.Visibility = Visibility.Visible;
            //lbl5.Visibility = Visibility.Visible;

            //try
            //{

            //    if (!bandera)
            //    {
            //        btnEliminarPago.Visibility = Visibility.Collapsed;
                 
    
            //    String ifFactura = "SELECT FK_idfacturas FROM cuotas WHERE idCuota = '" + ltsProximospagos.SelectedValue + "'";        
            //    String idfac = conexion.ValorEnVariable(ifFactura);
            //    Console.WriteLine("ID " + idfac);
            //    String sql = "SELECT  c.montoCuota,c.numCuota, f.total, f.numeroFactura, c.fecha, c.estado FROM cuotas c, factura f WHERE  c.idCuota = '"+ltsProximospagos.SelectedValue+"' AND f.idfacturas = '"+idfac+"'";
            //     porPagar = conexion.ConsultaParametrizada(sql, ltsProximospagos.SelectedValue);

            //    txtTotalCuota.Text = porPagar.Rows[0].ItemArray[0].ToString();
            //    txtNumeroCuota.Text =porPagar.Rows[0].ItemArray[1].ToString();
            //    txtTotalFactura.Text = porPagar.Rows[0].ItemArray[2].ToString();
            //    txtNumeroFactura.Text = porPagar.Rows[0].ItemArray[3].ToString();
    

            //    fecha = (DateTime)porPagar.Rows[0].ItemArray[4];
            //    DateTime hoy = DateTime.Now;

            //    TimeSpan ts = fecha - hoy;

            //    // Difference in days.
            //    int differenceInDays = ts.Days;

            //    Console.WriteLine("diferencia de dias " + differenceInDays);
             
            //    if (differenceInDays<=0)
            //    {
            //        Console.WriteLine("DIAS "+differenceInDays + " VENCIDO");
            //        ltsProximospagos.Background = Brushes.Red;
            //        txtTotalPAgo.BorderBrush = Brushes.Red;
            //        txtTipoPago.BorderBrush = Brushes.Red;
            //        txtfecha.BorderBrush = Brushes.Red;
            //        txtnumerocuotapaga.BorderBrush = Brushes.Red;
            //        txtEstado.BorderBrush = Brushes.Red;
            //            lbldias.Content = "Venció hace " + differenceInDays*-1 + " días";
            //        }
            //    else if (differenceInDays <= 10)
            //    {
            //        Console.WriteLine("DIAS " + differenceInDays + " PROXIMO A VENCER");
            //        ltsProximospagos.Background = Brushes.Orange;
            //        txtTotalPAgo.BorderBrush = Brushes.Orange;
            //        txtTipoPago.BorderBrush = Brushes.Orange;
            //        txtfecha.BorderBrush = Brushes.Orange;
            //        txtnumerocuotapaga.BorderBrush = Brushes.Orange;
            //        txtEstado.BorderBrush = Brushes.Orange;
            //            lbldias.Content = "Faltan " + differenceInDays + " días para su vencimiento";
            //        }

            //    else if(differenceInDays>=11 )
            //    {
            //        Console.WriteLine("DIAS " + differenceInDays + " POR PAGAR");
            //        ltsProximospagos.Background = Brushes.Yellow;
            //        txtTotalPAgo.BorderBrush = Brushes.Yellow;
            //        txtTipoPago.BorderBrush = Brushes.Yellow;
            //        txtfecha.BorderBrush = Brushes.Yellow;
            //        txtnumerocuotapaga.BorderBrush = Brushes.Yellow;
            //        txtEstado.BorderBrush = Brushes.Yellow;
            //        lbldias.Content = "Faltan " + differenceInDays + " días para su vencimiento";
            //        }
               

            //    String estado = porPagar.Rows[0].ItemArray[5].ToString();
                 
            //    if (estado == "False")
            //    {
            //        txtEstado.Text = "NO PAGADO";
            //        txtEstado.BorderBrush = Brushes.Red;
            //    }
            //    else
            //    {
            //        txtEstado.Text = "PAGADO";
            //        txtEstado.BorderBrush = Brushes.Green;
            //    }

            //    String nombreProveedor = "SELECT DISTINCT p.nombre FROM cuotas c, factura f, ordencompra oc, proveedor p WHERE c.FK_idfacturas = f.idfacturas AND f.FK_idOC = oc.idOrdenCompra AND oc.FK_idProveedor = p.idProveedor AND idCuota = '"+ltsProximospagos.SelectedValue+"'";
            //    String nombrep = conexion.ValorEnVariable(nombreProveedor);
            //    txtProveedor.Text = nombrep;

            //    txtTotalPAgo.Text = "El pago no se ha realizado";
            //    txtTipoPago.Text = "El pago no se ha realizado";
            //    txtfecha.Text = "El pago no se ha realizado";
            //    txtnumerocuotapaga.Text = "El pago no se ha realizado";
                 
            //}
            //    ltsPagosReaizados.SelectedIndex = -1;
            //}
            //catch (Exception)
            //{


            //}
            
        }

        private void ltsPagosReaizados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
       

        }

        private void btnEliminarPago_Click(object sender, RoutedEventArgs e)
        {
            //bandera = true;
            //String tipo = txtTipoPago.Text;
            //MessageBoxResult dialog = MessageBox.Show("Esta seguro que desea eliminar el pago número " + ltsPagosReaizados.SelectedValue, "Advertencia", MessageBoxButton.YesNo,MessageBoxImage.Warning);


            //if (dialog == MessageBoxResult.Yes)
            //{
            //    String id = "SELECT FK_idCuota FROM pago WHERE idPago = '" + ltsPagosReaizados.SelectedValue + "'";
            //    String idcuot = conexion.ValorEnVariable(id);

            //    String consulta = "UPDATE cuotas SET estado =  '" + 0 + "' where idCuota = '" + idcuot + "'";
            //    conexion.operaciones(consulta);

            //    if (tipo == "Efectivo")
            //    {
            //        string sql2 = "DELETE  FROM pago WHERE idPago = '" + ltsPagosReaizados.SelectedValue + "' AND FK_idCuota = '" + idcuot + "'";
            //        conexion.operaciones(sql2);
            //    }else if (tipo == "Cheque")
            //    {
            //        String idc = "SELECT FK_idCheque FROM pago WHERE idPago = '" + ltsPagosReaizados.SelectedValue + "'";
            //        String idch = conexion.ValorEnVariable(idc);

            //        string sql2 = "DELETE  FROM pago WHERE idPago = '" + ltsPagosReaizados.SelectedValue + "' AND FK_idCuota = '" + idcuot + "'";
            //        conexion.operaciones(sql2);

            //        string sql3 = "DELETE  FROM cheque WHERE idCheque = '" + idch + "'";
            //        conexion.operaciones(sql3);
            //    }
            //    else if (tipo == "Cuenta Bancaria")
            //    {
            //        String idc = "SELECT FK_idCuentaBco FROM pago WHERE idPago = '" + ltsPagosReaizados.SelectedValue + "'";
            //        String idbco = conexion.ValorEnVariable(idc);

            //        string sql2 = "DELETE  FROM pago WHERE idPago = '" + ltsPagosReaizados.SelectedValue + "' AND FK_idCuota = '" + idcuot + "'";
            //        conexion.operaciones(sql2);

            //        string sql3 = "DELETE  FROM cuentabanco WHERE idCuentaBco = '" + idbco + "'";
            //        conexion.operaciones(sql3);
            //    }


            //   // loadLtsProximosPagos();
            //   // loadLtsPagosRealizados();
            //    bandera = false;
            //    MessageBox.Show("El pago se eliminó correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
           // }
        }

        public void ColapsarElementos()
        {
            btnEliminarPago.Visibility = Visibility.Collapsed;

            txtNumeroCuota.Visibility = Visibility.Visible;
            // txtProveedor.Visibility = Visibility.Visible;
            txtRestanteFactura.Visibility = Visibility.Visible;
            txtTotalCuota.Visibility = Visibility.Visible;
            txtTotalFactura.Visibility = Visibility.Visible;
            btnAgregarPago.Visibility = Visibility.Visible;
            lbl1.Visibility = Visibility.Visible;
            // lbl2.Visibility = Visibility.Visible;
            lbl3.Visibility = Visibility.Visible;
            lbl4.Visibility = Visibility.Visible;
            lbl5.Visibility = Visibility.Visible;

            txtTotalPAgo.Visibility = Visibility.Collapsed;
            //txtEstado.Visibility = Visibility.Collapsed;
            txtTipoPago.Visibility = Visibility.Collapsed;
            txtfecha.Visibility = Visibility.Collapsed;
            txtnumerocuotapaga.Visibility = Visibility.Collapsed;
            lblestado.Visibility = Visibility.Collapsed;
            lbltp.Visibility = Visibility.Collapsed;
            lblfp.Visibility = Visibility.Collapsed;
            fec.Visibility = Visibility.Collapsed;
            lblnumcuotapaga.Visibility = Visibility.Collapsed;
        }
        private void ltsfacturas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                ColapsarElementos();

            String sql = "SELECT * FROM cuotas c, factura f WHERE f.idfacturas = '" + ltsfacturas.SelectedValue + "' AND c.FK_idfacturas = '" + ltsfacturas.SelectedValue + "'";
            porPagar = conexion.ConsultaParametrizada(sql, ltsfacturas.SelectedValue);

            txtTotalCuota.Text = porPagar.Rows[0].ItemArray[4].ToString();
            txtNumeroCuota.Text = porPagar.Rows[0].ItemArray[5].ToString();
            txtTotalFactura.Text = porPagar.Rows[0].ItemArray[10].ToString();

                String tipoCambio = porPagar.Rows[0].ItemArray[12].ToString();
                if (tipoCambio == "0")
                {
                    txtMoneda.Text = "$";
                }
                else if (tipoCambio == "1")
                {
                    txtMoneda.Text = "u$d";
                }
                else
                {
                    txtMoneda.Text = "€";
                }
                

                restante = 0;
            restante = float.Parse( porPagar.Rows[0].ItemArray[17].ToString());
                txtRestanteFactura.Text = restante.ToString();
                if (!bandera)
            {
                    
                String consultaProveedores = "SELECT fecha, DATE_FORMAT(fecha,'%d%m%Y') FROM cuotas WHERE FK_idfacturas = '" + ltsfacturas.SelectedValue + "'";
                DataTable cuotas = conexion.ConsultaParametrizada(consultaProveedores, ltsfacturas.SelectedValue);
                ltscuotas.ItemsSource = cuotas.AsDataView();
                    ltscuotas.DisplayMemberPath = "fecha";

            }
            }
            catch
            {


            }

        }

        private void txtfiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Busquedas de productos.
            DataTable proveedores = new DataTable();
            String consulta;

      
                consulta = "SELECT DISTINCT f.idfacturas, f.subtotal, f.numeroFactura, f.total, f.iva, f.tipoCambio, f.cuotas, f.FK_idOC, f.fecha, f.totalRestante FROM factura f, ordencompra o, proveedor p, cuotas c WHERE f.FK_idOC = o.idOrdenCompra AND o.FK_idProveedor  = p.idProveedor AND p.nombre LIKE '%' @valor '%'";
                proveedores = conexion.ConsultaParametrizada(consulta, txtfiltro.Text);
   
                if (txtfiltro.Text == "")
                {
                    consulta = "SELECT * FROM factura";
                    proveedores = conexion.ConsultaParametrizada(consulta, txtfiltro.Text);
                }
            

            ltsfacturas.ItemsSource = proveedores.AsDataView();
            ltsfacturas.SelectedIndex = 0;
        }
    }
}
