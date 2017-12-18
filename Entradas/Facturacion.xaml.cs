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
    /// Interaction logic for Facturacion.xaml
    /// </summary>
    public partial class Facturacion : Page
    {
        CRUD conexion = new CRUD();
        public List<producto> itemsFactura = new List<producto>();
        public List<producto> itemsFacturaDB = new List<producto>();
        public List<producto> produ = new List<producto>();
        public List<factura> items = new List<factura>();
        factura fa;
     
        public Facturacion()
        {
            InitializeComponent();
            LoadListaComboProveedor();
           LoadDgvFactura();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarFactura();
            newW.ShowDialog();
    

            //INSERTO DATOS FACTURA
            if (newW.DialogResult == true)
            {
                String idPRove = cmbProveedores.SelectedValue.ToString();
                   decimal subtotal = decimal.Parse(newW.txtSubtotal.Text);
                 decimal total = decimal.Parse(newW.txtTotal.Text);
                  int numeroFact = int.Parse(newW.txtNroFactura.Text);
                  String iva = newW.cmbIVA.SelectedIndex.ToString();
                   String tipoCambio = newW.cmbTipoCambio.SelectedIndex.ToString();
                   String cuotas = newW.cmbCuotas.Text;
                int fkOrden = int.Parse(newW.cmbOrden.Text);
                DateTime dtp = System.DateTime.Now;
                dtp = newW.dtFactura.SelectedDate.Value;

                String sqlFactura = "INSERT INTO factura ( subtotal, numeroFactura, total, iva, tipoCambio, cuotas, FK_idOC, fecha )VALUES ('"+subtotal+ "','" + numeroFact + "','" + total + "','" + iva + "','" + tipoCambio + "','" + cuotas + "','" + fkOrden + "','" + dtp.ToString("yyyy/MM/dd") + "')";
                conexion.operaciones(sqlFactura);

               


                //CREAR CONSULTA PARA TRAER ID FACTURA
                int idProducto = newW.prod.id;
                string ultimoId = "Select last_insert_id()";
                String id = conexion.ValorEnVariable(ultimoId);
                

                //INSERTO LOS PRODUCTOS DE LA FACTURA
                foreach (producto p in newW.itemsFact)
                {
                    String nombre = p.nombre;
                    int cantidad = p.cantidad;
                    decimal totalp = p.total;
                    decimal precioUni = p.precioUnitario;
          
                    producto pr = new producto(nombre, 1, cantidad, totalp, precioUni);
                    itemsFacturaDB.Add(p);

                    
                    String sqlProductoHas = "INSERT INTO productos_has_facturas (cantidad, subtotal, FK_idProducto, FK_idFactura) VALUES ('" + cantidad + "','" + subtotal + "', '" + idProducto + "', '" + id + "')";
                    conexion.operaciones(sqlProductoHas);


                    // TRAIGO LA CANTIDAD RESTANTE DE LA FACTURA
                    String crfact = "SELECT CrFactura FROM productos_has_ordencompra WHERE FK_idOC = '"+fkOrden+"'";
                    String crfactura =conexion.ValorEnVariable(crfact);

                    int cantidadrestanteFact = Int32.Parse(crfactura);
                    cantidadrestanteFact = cantidadrestanteFact - cantidad;
                    MessageBox.Show("cantidad restante" + cantidadrestanteFact);

                    //INSERTO VALOR NUEVO DE CANTIDAD RESTANTE
                    String updateCR = "UPDATE productos_has_ordencompra SET CrFactura = '"+cantidadrestanteFact+"' WHERE FK_idOC = '"+fkOrden+"'";
                    conexion.operaciones(updateCR);
                    cantidadrestanteFact = 0;
                  
                }

               

            }
            LoadDgvFactura(); 
        }

        public void LoadListaComboProveedor()
        {
            String consulta = "SELECT p.nombre, p.idProveedor FROM proveedor p INNER JOIN ordencompra o ON p.idProveedor = o.FK_idProveedor";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
            cmbProveedores.SelectedIndex = 1;
        }

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                

                String id = cmbProveedores.SelectedValue.ToString();
                String nombreProv = cmbProveedores.Text;

                String sql = "SELECT * FROM ordencompra WHERE FK_idProveedor =  '" + id + "'";
                conexion.Consulta(sql, combo: cmbordenCompra);
                cmbordenCompra.DisplayMemberPath = "idOrdenCompra";
                cmbordenCompra.SelectedValuePath = "idOrdenCompra";
                cmbordenCompra.SelectedIndex = 0;
            }
            catch (Exception)
            {

                MessageBox.Show("error");
            }
        }

   
        private void LoadDgvFactura()
        {
         
            String consulta = "SELECT * FROM factura WHERE FK_idOC = '"+cmbordenCompra.SelectedValue+"' ";
            conexion.Consulta(consulta, ltsFactura);
            ltsFactura.DisplayMemberPath = "numeroFactura";
            ltsFactura.SelectedValuePath = "idfacturas";
            ltsFactura.SelectedIndex = 0;
           
        }

        private void dgvFactura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void cmbordenCompra_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String consulta = "SELECT idfacturas,numeroFactura FROM factura WHERE FK_idOC = '" + cmbordenCompra.SelectedValue + "' ";
            conexion.Consulta(consulta, ltsFactura);
            ltsFactura.DisplayMemberPath = "numeroFactura";
            ltsFactura.SelectedValuePath = "idfacturas";
        }

        private void ltsFactura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // String consulta = "SELECT FK_idProducto FROM productos_has_facturas WHERE FK_idfactura =  '" + ltsFactura.SelectedValue + "' ";
            //String idProduc = conexion.ValorEnVariable(consulta);
            //MessageBox.Show("" + idProduc);

            try
            {
                String productosFatura = " SELECT t1.cantidad ,t1.subtotal, t2.nombre from productos_has_facturas t1 inner join productos t2 where t1.FK_idProducto = t2.idProductos and t1.FK_idfactura = '" + ltsFactura.SelectedValue + "'";
                DataTable contacto = conexion.ConsultaParametrizada(productosFatura, ltsFactura.SelectedValue);
                dgvProductosFactura.ItemsSource = contacto.AsDataView();


                String consulta2 = "SELECT * FROM factura f where f.idfacturas ='" + ltsFactura.SelectedValue + "'";
                DataTable OC = conexion.ConsultaParametrizada(consulta2, ltsFactura.SelectedValue);
                txtSubTotal.Text = OC.Rows[0].ItemArray[1].ToString();
                txtTotal1.Text = OC.Rows[0].ItemArray[3].ToString();
                txtTipoCambio.Text = OC.Rows[0].ItemArray[5].ToString();
                txtIVA.Text = OC.Rows[0].ItemArray[4].ToString();
            }
            catch (Exception)
            {

          
            }

    

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            string sql2 = "DELETE  FROM factura WHERE idfacturas = '" + ltsFactura.SelectedValue + "'";
            conexion.operaciones(sql2);

            string sql3 = " DELETE  FROM productos_has_facturas WHERE FK_idfactura =  '" + ltsFactura.SelectedValue + "'";
            conexion.operaciones(sql3);

            LoadDgvFactura();
            txtIVA.Text = "";
            txtSubTotal.Text = "";
            txtTipoCambio.Text = "";
            txtTotal1.Text = "";
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                String consulta = " Select * from factura t1 where t1.fecha = @valor ";
                DataTable OCFecha = conexion.ConsultaParametrizada(consulta, dtfecha.SelectedDate);
                ltsFactura.ItemsSource = OCFecha.AsDataView();
                ltsFactura.DisplayMemberPath = "numeroFactura";
                ltsFactura.SelectedValuePath = "idfacturas";
                ltsFactura.SelectedIndex = 0;
            }
            catch (NullReferenceException)
            {


            }
        }

        private void btnModificar_Copy_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
