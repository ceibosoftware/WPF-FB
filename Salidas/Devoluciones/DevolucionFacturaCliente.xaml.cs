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

namespace wpfFamiliaBlanco.Salidas.Devoluciones
{
    /// <summary>
    /// Lógica de interacción para DevolucionFacturaCliente.xaml
    /// </summary>
    public partial class DevolucionFacturaCliente : Page
    {
        DataTable productos;
        bool bandera = false;
        String iva;
        String cambio;
        Producto producto;
        String idFactura;
        String lastid;
        CRUD conexion = new CRUD();
        public List<Producto> itemsNC = new List<Producto>();
        public List<Producto> productosAmodificar = new List<Producto>();
        public DevolucionFacturaCliente()
        {
            InitializeComponent();
            loadLtsNotaCredito();
            dgvProductosNC.IsReadOnly = true;
            txtIVA.IsReadOnly = true;
            txtProveedor.IsReadOnly = true;
            txtSubtotal.IsReadOnly = true;
            txtTipoCambio.IsReadOnly = true;
            txtTotal.IsReadOnly = true;
            LoadDgvNC();
            SetearColumnas();
            ltsNC.SelectedIndex = 0;
            funcionver();

        }

        public void SetearColumnas()
        {
            dgvProductosNC.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Cantidad";
            textColumn.Binding = new Binding("cantidad");
            dgvProductosNC.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio Unitario";
            textColumn2.Binding = new Binding("precioUnitario");
            dgvProductosNC.Columns.Add(textColumn2);
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            bandera = true;
            itemsNC.Clear();
            int index = ltsNC.SelectedIndex;
            Console.WriteLine("idindex parametro  " + index);
            var newW = new windowAgregarNCFactura(1);
            newW.ShowDialog();
            DateTime hoy = DateTime.Today;


            if (newW.DialogResult == true)
            {
                idFactura = newW.idFactura;
                String totall = newW.txtTotal.Text;
                String subtotall = newW.txtSubtotal.Text;
                String insertNC = "INSERT INTO notacreditosalida (total, subtotal,FK_idFacturas, fecha) VALUES ( '" + totall + "', '" + subtotall + "','" + idFactura + "','" + hoy.ToString("yyyy/MM/dd") + "')";
                conexion.operaciones(insertNC);


                string ultimoId = "Select last_insert_id()";
                lastid = conexion.ValorEnVariable(ultimoId);

                foreach (Producto p in newW.itemsNC)
                {
                    String nombre = p.nombre;
                    int cantidad = p.cantidad;
                    float totalp = p.total;
                    float precioUni = p.precioUnitario;
                    int idp = p.id;

                    Producto pr = new Producto(nombre, idp, cantidad, totalp, precioUni);
                    itemsNC.Add(p);

                    String productostNC = "INSERT INTO productos_has_notacreditosalida (FK_idNotaCredito, FK_idProductos, cantidad, precioUnitario) VALUES ('" + lastid + "','" + idp + "', '" + cantidad + "', '" + precioUni + "')";
                    conexion.operaciones(productostNC);

                }

                foreach (var producto in newW.itemsFact)
                {
                    String sql = "UPDATE productos_has_facturassalida SET CrNotaCredito = '" + producto.cantidad + "' where FK_idProductos = '" + producto.id + "' and FK_idfacturas = '" + idFactura + "'";
                    conexion.operaciones(sql);
                }

                LoadDgvNC();
                loadLtsNotaCredito();

                bandera = false;
                ltsNC.Items.MoveCurrentToLast();
            }







        }

        public void LoadDgvNC()
        {

            dgvProductosNC.ItemsSource = itemsNC;
            dgvProductosNC.Items.Refresh();
        }
        public void loadLtsNotaCredito()
        {

            //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
            String consulta = "select * from notacreditosalida where FK_idfacturas IS NOT NULL";
            conexion.Consulta(consulta, tabla: ltsNC);
            ltsNC.DisplayMemberPath = "idNotaCredito";
            ltsNC.SelectedValuePath = "idNotaCredito";
            ltsNC.SelectedIndex = -1;
            ltsNC.SelectedIndex = 0;
        }




        private void ltsNC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {


                if (!bandera)
                {

                    //aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
                    Console.WriteLine("ID TS NOTA CREDITO" + ltsNC.SelectedValue);

                    String productosNc = "SELECT * FROM productos_has_notacreditosalida WHERE FK_idNotaCredito ='" + ltsNC.SelectedValue + "'";

                    productos = conexion.ConsultaParametrizada(productosNc, ltsNC.SelectedValue);
                    dgvProductosNC.ItemsSource = productos.AsDataView();



                    String Fkf = "SELECT * FROM notacreditosalida WHERE idNotaCredito = '" + ltsNC.SelectedValue + "' AND FK_idfacturas IS NOT NULL";

                    DataTable OC1 = conexion.ConsultaParametrizada(Fkf, ltsNC.SelectedValue);



             
                    String totalDB = OC1.Rows[0].ItemArray[1].ToString();
                    String subtotalDB = OC1.Rows[0].ItemArray[2].ToString();
                    String idfacturaDB = OC1.Rows[0].ItemArray[4].ToString();

                    String consulta2 = "SELECT * FROM facturasalida f where f.idfacturas ='" + idfacturaDB + "'";
                    DataTable OC = conexion.ConsultaParametrizada(consulta2, idfacturaDB);



                    if (OC.Rows[0].ItemArray[4].ToString() == "0")
                    {
                        iva = "0";
                    }
                    else if (OC.Rows[0].ItemArray[4].ToString() == "1")
                    {
                        iva = "21";
                    }
                    else
                    {
                        iva = "10,5";
                    }

                    if (OC.Rows[0].ItemArray[5].ToString() == "0")
                    {
                        cambio = "$";
                    }
                    else if (OC.Rows[0].ItemArray[5].ToString() == "1")
                    {
                        cambio = "u$d";
                    }
                    else
                    {
                        cambio = "€";
                    }

                    txtSubtotal.Text = subtotalDB;
                    txtTotal.Text = totalDB;




                    String idOC = "SELECT FK_idOrdenCompra FROM facturasalida WHERE idfacturas = '" + idfacturaDB + "' ";
                    String idorc = conexion.ValorEnVariable(idOC);


                    //String idprov = "SELECT FK_idProveedor FROM ordencomprasalida WHERE idOrdenCompra = '" + idorc + "' ";
                    //String idprove = conexion.ValorEnVariable(idprov);


                    ////aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
                    //String nombreprove = "SELECT nombre FROM proveedor WHERE idProveedor = '" + idprove + "' ";
                    //String nombrepv = conexion.ValorEnVariable(nombreprove);

                    //txtProveedor.Text = nombrepv;



                    txtIVA.Text = iva;
                    txtTipoCambio.Text = cambio;



                }
            }
            catch (Exception)
            {


            }

        }
        public void calculaTotal()
        {

            if (txtIVA.Text == "0")
            {

                txtTotal.Text = txtSubtotal.Text.ToString();
            }
            else if (txtIVA.Text == "21")
            {
                txtTotal.Text = (float.Parse(txtSubtotal.Text) * (float)1.21).ToString();

            }
            else if (txtIVA.Text == "10,5")
            {
                txtTotal.Text = (float.Parse(txtSubtotal.Text) * (float)1.105).ToString();

            }
        }

        private void btnModificarNC_Click(object sender, RoutedEventArgs e)
        {
            bandera = true;
            int idnotacredito = (int)ltsNC.SelectedValue;
            productosAmodificar.Clear();
            String iva = txtIVA.Text;
            String subtotal = txtSubtotal.Text;
            String cambio = txtTipoCambio.Text;
            String total = txtTotal.Text;

            String idf = "SELECT FK_idfacturas FROM notacreditosalida WHERE idNotaCredito = '" + ltsNC.SelectedValue + "'";
            String idFactura = conexion.ValorEnVariable(idf);

            String productosFatura = "SELECT DISTINCT t1.subtotal, t2.nombre ,t2.precioUnitario,t2.idProductos,t3.cantidad from productos_has_facturassalida t1, productos_has_notacreditosalida t3 inner join productos t2 where t1.FK_idProductos = t2.idProductos and t1.FK_idfacturas = '" + idFactura + "' and  t3.FK_idNotaCredito = '" + ltsNC.SelectedValue + "'";
            productos = conexion.ConsultaParametrizada(productosFatura, ltsNC.SelectedValue);


            for (int i = 0; i < productos.Rows.Count; i++)
            {
                producto = new Producto(productos.Rows[i].ItemArray[1].ToString(), (int)productos.Rows[i].ItemArray[3], (int)productos.Rows[i].ItemArray[4], (float)productos.Rows[i].ItemArray[0], (float)productos.Rows[i].ItemArray[2]);
             
                productosAmodificar.Add(producto);

            }



            var newW = new windowAgregarNCFactura(subtotal, total, iva, cambio, productosAmodificar, idFactura, idnotacredito,1);
            newW.Title = "Modificar Nota de Crédito";
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {

                String subm = newW.subtotalmodificar;
                String totalm = newW.totalmodificar;
                int idnotaCredito = newW.idnota;
                itemsNC.Clear();
                foreach (Producto p in newW.itemsNC)
                {
                    String nombre = p.nombre;
                    int cantidad = p.cantidad;
                    float totalp = p.total;
                    float precioUni = p.precioUnitario;
                    int idp = p.id;

                    Producto pr = new Producto(nombre, idp, cantidad, totalp, precioUni);
                    itemsNC.Add(p);

                    String updateNC = "UPDATE notacreditosalida SET total =  '" + totalm + "', subtotal = '" + subm + "' WHERE idNotaCredito = '" + idnotaCredito + "'";
                    conexion.operaciones(updateNC);

                    String updateProductosNC = "UPDATE productos_has_notacreditosalida SET FK_idNotaCredito = '" + idnotaCredito + "', FK_idProductos = '" + idp + "', cantidad = '" + cantidad + "', precioUnitario = '" + precioUni + "' WHERE FK_idNotaCredito = '" + idnotaCredito + "' AND FK_idProductos = '" + idp + "'";
                    conexion.operaciones(updateProductosNC);

                    foreach (var producto in newW.itemsFact)
                    {
                        String sql = "UPDATE productos_has_facturassalida SET CrNotaCredito = '" + producto.cantidad + "' where FK_idProductos = '" + producto.id + "' and FK_idfacturas = '" + idFactura + "'";
                        conexion.operaciones(sql);
                    }


                }
                LoadDgvNC();
                loadLtsNotaCredito();

                bandera = false;
                ltsNC.Items.MoveCurrentToLast();
            }


        }

        private void btnEliminarNC_Click(object sender, RoutedEventArgs e)
        {
            bandera = true;
            try
            {

                String idFactura = "";
                MessageBoxResult dialog = MessageBox.Show("Esta seguro que desea eliminar la nota de crédito numero :" + ltsNC.SelectedValue, "Advertencia", MessageBoxButton.YesNo);


                if (dialog == MessageBoxResult.Yes)
                {

                    for (int i = 0; i < productos.Rows.Count; i++)
                    {

                        String idf = "SELECT FK_idfacturas FROM notacreditosalida WHERE idNotaCredito = '" + ltsNC.SelectedValue + "'";
                        idFactura = conexion.ValorEnVariable(idf);
                        MessageBox.Show("id oc" + productos.Rows[i].ItemArray[0]);
                        MessageBox.Show("cantidad en la NC" + productos.Rows[i].ItemArray[1]);
                        MessageBox.Show("precio unitario" + productos.Rows[i].ItemArray[2]);
                        MessageBox.Show("id producto" + productos.Rows[i].ItemArray[3]);
                        MessageBox.Show("id nota credito" + productos.Rows[i].ItemArray[4]);
                
                        String consulta = "UPDATE productos_has_facturassalida SET CrNotaCredito = CrNotaCredito + '" + (int)productos.Rows[i].ItemArray[1] + "' where FK_idProductos = '" + (int)productos.Rows[i].ItemArray[3] + "' and FK_idfacturas= '" + idFactura + "'";
                        conexion.operaciones(consulta);
                    }


                    string sql2 = "DELETE  FROM notacreditosalida WHERE idNotaCredito = '" + ltsNC.SelectedValue + "' AND FK_idfacturas = '" + idFactura + "'";
                    conexion.operaciones(sql2);

                    string sql3 = " DELETE  FROM productos_has_notacreditosalida WHERE FK_idNotaCredito =  '" + ltsNC.SelectedValue + "'";
                    conexion.operaciones(sql3);

                    txtIVA.Text = "";
                    txtSubtotal.Text = "";
                    txtTipoCambio.Text = "";
                    txtTotal.Text = "";
              
          
                }

                LoadDgvNC();
                loadLtsNotaCredito();

                bandera = false;


            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione una factura a eliminar");
            }
        }

        private void dpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                String consulta = " Select * from notacreditosalida t1, factura t2  where  t2.fecha = @valor and t1.FK_idfacturas = t2.idfacturas ";
                DataTable OCFecha = conexion.ConsultaParametrizada(consulta, dpFecha.SelectedDate);
                ltsNC.ItemsSource = OCFecha.AsDataView();
                ltsNC.DisplayMemberPath = "idNotaCredito";
                ltsNC.SelectedValuePath = "idNotaCredito";
                ltsNC.SelectedIndex = 0;
            }
            catch (NullReferenceException)
            {


            }
        }

        private void btnVertodo_Click(object sender, RoutedEventArgs e)
        {
            String consulta = "select * from notacreditosalida where FK_idfacturas IS NOT NULL";
            conexion.Consulta(consulta, tabla: ltsNC);
            ltsNC.DisplayMemberPath = "idNotaCredito";
            ltsNC.SelectedValuePath = "idNotaCredito";
            ltsNC.SelectedIndex = 0;
        }

        public void funcionver()
        {
            String consulta = "select * from notacreditosalida where FK_idfacturas IS NOT NULL";
            conexion.Consulta(consulta, tabla: ltsNC);
            ltsNC.DisplayMemberPath = "idNotaCredito";
            ltsNC.SelectedValuePath = "idNotaCredito";
            ltsNC.SelectedIndex = 0;
        }
    }
}
