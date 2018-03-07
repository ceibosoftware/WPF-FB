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
    /// Lógica de interacción para windowAgregarNCFactura.xaml
    /// </summary>
    public partial class windowAgregarNCFactura : Window
    {
        CRUD conexion = new CRUD();
        DataTable productos;
        public List<Producto> itemsFact = new List<Producto>();
        public int id = 0;
        float subtotal;
        public Producto prod;
        public windowAgregarNCFactura()
        {
            InitializeComponent();
            loadLtsfactura();
            txtIVA.IsReadOnly = true;
            txtTotal.IsReadOnly = true;
            txtTipoCambio.IsReadOnly = true;
            txtSubtotal.IsReadOnly = true;
    
            
        }
      
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void loadLtsfactura()
        {
            String consulta = "SELECT idfacturas,numeroFactura FROM factura";
            conexion.Consulta(consulta, ltsfacturas);
            ltsfacturas.DisplayMemberPath = "numeroFactura";
            ltsfacturas.SelectedValuePath = "idfacturas";
            ltsfacturas.SelectedIndex = 0;
        }

        private void ltsfacturas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                String productosFatura = "SELECT DISTINCT t1.subtotal, t2.nombre ,t2.precioUnitario,t2.idProductos, t1.cantidad from productos_has_facturas t1, productos_has_ordencompra t3 inner join productos t2 where t1.FK_idProducto = t2.idProductos and t1.FK_idfactura = '" + ltsfacturas.SelectedValue + "'";

                productos = conexion.ConsultaParametrizada(productosFatura, ltsfacturas.SelectedValue);
                dgvProductosFactur.ItemsSource = productos.AsDataView();


                String consulta2 = "SELECT * FROM factura f where f.idfacturas ='" + ltsfacturas.SelectedValue + "'";
                DataTable OC = conexion.ConsultaParametrizada(consulta2, ltsfacturas.SelectedValue);

                String idOC = "SELECT FK_idOC FROM factura WHERE idfacturas = '" + ltsfacturas.SelectedValue + "' ";
                String idorc = conexion.ValorEnVariable(idOC);


                String idprov = "SELECT FK_idProveedor FROM ordencompra WHERE idOrdenCompra = '" +idorc + "' ";
                String idprove = conexion.ValorEnVariable(idprov);

                String nombreprove = "SELECT nombre FROM proveedor WHERE idProveedor = '" + idprove + "' ";
                String nombrepv = conexion.ValorEnVariable(nombreprove);

                txtProveedor.Text = nombrepv;

                /*
                if (OC.Rows[0].ItemArray[4].ToString() == "0")
                {
                    txtIVA.Text = "0";
                }
                else if (OC.Rows[0].ItemArray[4].ToString() == "1")
                {
                    txtIVA.Text = "21";
                }
                else
                {
                    txtIVA.Text = "10,5";
                }

                if (OC.Rows[0].ItemArray[5].ToString() == "0")
                {
                    txtTipoCambio.Text = "$";
                }
                else if (OC.Rows[0].ItemArray[5].ToString() == "1")
                {
                    txtTipoCambio.Text = "u$d";
                }
                else
                {
                    txtTipoCambio.Text = "€";
                }
                txtSubtotal.Text = OC.Rows[0].ItemArray[1].ToString();
                txtTotal.Text = OC.Rows[0].ItemArray[3].ToString();
                */


            }
            catch (Exception)
            {


            }

        }

        private void btnAgregarProductoNC_Click(object sender, RoutedEventArgs e)
        {
           
           
        }
          
    }

 }
