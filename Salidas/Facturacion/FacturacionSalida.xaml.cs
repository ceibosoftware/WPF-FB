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

namespace wpfFamiliaBlanco.Salidas.Facturacion
{
    /// <summary>
    /// Lógica de interacción para FacturacionSalida.xaml
    /// </summary>
    public partial class FacturacionSalida : Page
    {
        CRUD conexion = new CRUD();
        public List<Producto> itemsFactura = new List<Producto>();
        public List<Producto> itemsdb = new List<Producto>();
        public List<Producto> itemsFacturaDB = new List<Producto>();
        public List<Producto> itemsFacdb = new List<Producto>();
        public List<Producto> produ = new List<Producto>();
        public List<factura> items = new List<factura>();
        public List<Cuotas> cuotasAinsertar = new List<Cuotas>();
        float subtotal;
        int fkOrden;
        String FKoc;
        string idOC;
        String fkidproveedor;
        String idproveed;
        DataTable productos;

        public FacturacionSalida()
        {

            InitializeComponent();
            rbExterno.IsChecked = true;
            rbInterno.IsChecked = false;
            rbInterno.IsChecked = true;
            rbExterno.IsChecked = false;

            lblNC.Visibility = Visibility.Collapsed;
            // LoadListaComboProveedor();

            LoadDgvFactura();
            txtSubTotal.IsReadOnly = true;
            txtTotal1.IsReadOnly = true;
            txtIVA.IsReadOnly = true;
            txtTipoCambio.IsReadOnly = true;
            dgvProductosFactura.IsReadOnly = true;
            LoadDgvFactura();
            LoadListfactura();
            //cmbordenCompra.SelectedIndex = -1;
            //  seleccioneParaFiltrar();

            if (windowUsuarios.tipoUsuarioDB == "basico")
            {
                this.btnModificar_Copy.Visibility = Visibility.Collapsed;
                this.btnEliminar.Visibility = Visibility.Collapsed;

            }

            ltsFactura.SelectedIndex = 0;
            SetearColumnas();
            txtfecha.IsEnabled = false;
            txtproveedor.IsEnabled = false;
            txtoc.IsEnabled = false;


        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            cuotasAinsertar.Clear();
            var newW = new windowAgregarFacturaSalidas();
            newW.ShowDialog();


            //INSERTO DATOS FACTURA
            if (newW.DialogResult == true)
            {
                String idPRove = newW.cmbCliente.SelectedValue.ToString();
                decimal subtotal = decimal.Parse(newW.txtSubtotal.Text);
                decimal total = decimal.Parse(newW.txtTotal.Text);
                Int64 numeroFact = Int64.Parse(newW.txtNroFactura.Text);
                String iva = newW.cmbIVA.SelectedIndex.ToString();
                String tipoCambio = newW.cmbTipoCambio.SelectedIndex.ToString();
                String cuotas = newW.cmbCuotas.Text;
                fkOrden = int.Parse(newW.cmbOrden.Text);
                DateTime dtp = System.DateTime.Now;
                dtp = newW.dtFactura.SelectedDate.Value;

                Console.WriteLine("idCliente " + idPRove);
                Console.WriteLine("subtotal " + subtotal);
                Console.WriteLine("total " + total);
                Console.WriteLine("numeroFact " + numeroFact);
                Console.WriteLine("iva " + iva);
                Console.WriteLine("cuotas " + cuotas);
                Console.WriteLine("fkOrden " + fkOrden);
                Console.WriteLine("dtp " + dtp);

                String sqlFactura = "INSERT INTO facturasalida ( subtotal, numeroFactura, total, iva, tipoCambio, cuotas, FK_idOrdenCompra, fecha )VALUES ('" + subtotal + "','" + numeroFact + "','" + total + "','" + iva + "','" + tipoCambio + "','" + cuotas + "','" + fkOrden + "','" + dtp.ToString("yyyy/MM/dd") + "')";
                conexion.operaciones(sqlFactura);


                //CREAR CONSULTA PARA TRAER ID FACTURA
                int idProducto = newW.id;
                string ultimoId = "Select last_insert_id()";
                String id = conexion.ValorEnVariable(ultimoId);

                Console.WriteLine("idfactura " + id);
                //inserto cuotas
                foreach (Cuotas cuot in newW.todaslascuotas)
                {

                    int id2 = cuot.cuota;
                    int dias = cuot.dias;
                    DateTime fecha = cuot.fechadepago;
                    float montocuota = cuot.montoCuota;
                    int cuota = cuot.cuota;

                    String sqlProductoHas = "INSERT INTO cuotassalida ( dias, fecha, montoCuota ,FK_idFacturas,numCuota,estado) VALUES ('" + dias + "', '" + fecha.ToString("yyyy/MM/dd") + "','" + montocuota + "' ,'" + id + "' ,'" + cuota + "','" + 0 + "')";
                    conexion.operaciones(sqlProductoHas);

                }

                //INSERTO LOS PRODUCTOS DE LA FACTURA
                foreach (Producto p in newW.itemsFact)
                {
                    String nombre = p.nombre;
                    int cantidad = p.cantidad;
                    float totalp = p.total;
                    float precioUni = p.precioUnitario;
                    int idp = p.id;
                    Producto pr = new Producto(nombre, idp, cantidad, totalp, precioUni);

                    itemsFacturaDB.Add(p);


                    String sqlProductoHas = "INSERT INTO productos_has_facturassalida (cantidad, subtotal,CrNotaCredito ,FK_idProductos, FK_idFacturas) VALUES ('" + cantidad + "','" + subtotal + "', '" + cantidad + "','" + idp + "', '" + id + "')";
                    conexion.operaciones(sqlProductoHas);


                    // TRAIGO LA CANTIDAD RESTANTE DE LA FACTURA
                    /* String crfact = "SELECT CrFactura FROM productos_has_ordencompra WHERE FK_idOC = '"+fkOrden+ "' AND FK_idProducto = '" + idProducto + "'";
                     String crfactura =conexion.ValorEnVariable(crfact);

                     int cantidadrestanteFact = Int32.Parse(crfactura);
                     cantidadrestanteFact = cantidadrestanteFact - cantidad;
                 */

                    //ACTUALIZAR CANTITAD RESTANTE REMITO DE PRODUCTO OC
                    int idOrden = (int)newW.cmbOrden.SelectedValue;
                    foreach (var producto in newW.items)
                    {
                        String sql = "UPDATE productos_has_ordencomprasalida SET CrFactura = '" + producto.cantidad + "' where FK_idProducto = '" + producto.id + "' and FK_idOrdenCompra = '" + idOrden + "'";
                        conexion.operaciones(sql);
                    }


                }

                MessageBox.Show("La factura se agregó correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            LoadListfactura();
            ltsFactura.Items.Refresh();
            ltsFactura.Items.MoveCurrentToLast();
        }

        public void LoadListaCMI()
        {
            String consulta = "SELECT * FROM clientesmi";
            conexion.Consulta(consulta, combo: cmbCliente);
            cmbCliente.DisplayMemberPath = "nombre";
            cmbCliente.SelectedValuePath = "idClientemi";


        }

        public void LoadListaCME()
        {
            String consulta = "SELECT * FROM clientesme";
            conexion.Consulta(consulta, combo: cmbCliente);
            cmbCliente.DisplayMemberPath = "nombre";
            cmbCliente.SelectedValuePath = "idClienteme";
            cmbCliente.SelectedIndex = 0;

        }
        public void SetearColumnas()
        {
            dgvProductosFactura.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvProductosFactura.Columns.Add(textColumn);

            DataGridTextColumn textColumn1 = new DataGridTextColumn();
            textColumn1.Header = "Cantidad";
            textColumn1.Binding = new Binding("cantidad");
            dgvProductosFactura.Columns.Add(textColumn1);

            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio Unitario";
            textColumn2.Binding = new Binding("precioUnitario");
            dgvProductosFactura.Columns.Add(textColumn2);

            DataGridTextColumn textColumn3 = new DataGridTextColumn();
            textColumn3.Header = "Subtotal";
            textColumn3.Binding = new Binding("subtotal");
            dgvProductosFactura.Columns.Add(textColumn3);
        }
        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                String id = cmbCliente.SelectedValue.ToString();
                String nombreCliente = cmbCliente.Text;

                if (rbInterno.IsChecked == true)
                {
                    String sql = "SELECT * FROM ordencomprasalida WHERE FK_idClientemi =  '" + id + "'";
                    conexion.Consulta(sql, combo: cmbordenCompra);
                    cmbordenCompra.DisplayMemberPath = "idOrdenCompra";
                    cmbordenCompra.SelectedValuePath = "idOrdenCompra";
                    cmbordenCompra.SelectedIndex = 0;
                    ltsFactura.SelectedIndex = 0;
                    LoadDgvFactura();
                }
                else
                {
                    String sql = "SELECT * FROM ordencomprasalida WHERE FK_idClienteme =  '" + id + "'";
                    conexion.Consulta(sql, combo: cmbordenCompra);
                    cmbordenCompra.DisplayMemberPath = "idOrdenCompra";
                    cmbordenCompra.SelectedValuePath = "idOrdenCompra";
                    cmbordenCompra.SelectedIndex = 0;
                    ltsFactura.SelectedIndex = 0;
                    LoadDgvFactura();
                }




            }
            catch (Exception)
            {


            }

        }


        private void LoadDgvFactura()
        {

            String consulta = "SELECT * FROM facturasalida WHERE FK_idOrdenCompra = '" + cmbordenCompra.SelectedValue + "' ";
            conexion.Consulta(consulta, ltsFactura);
            ltsFactura.DisplayMemberPath = "numeroFactura";
            ltsFactura.SelectedValuePath = "idfacturas";
            ltsFactura.SelectedIndex = 0;


        }


        private void Loadltsfactura()
        {
            if (rbInterno.IsChecked == true)
            {
                String consulta = "SELECT * FROM facturasalida f, ordencomprasalida o WHERE f.FK_idOrdenCompra = o.idOrdenCompra AND FK_idClientemi IS NOT NULL";
                conexion.Consulta(consulta, ltsFactura);
                ltsFactura.DisplayMemberPath = "numeroFactura";
                ltsFactura.SelectedValuePath = "idfacturas";
                ltsFactura.SelectedIndex = 0;
            }
            else
            {
                String consulta = "SELECT * FROM facturasalida f, ordencomprasalida o WHERE f.FK_idOrdenCompra = o.idOrdenCompra AND FK_idClienteme IS NOT NULL";
                conexion.Consulta(consulta, ltsFactura);
                ltsFactura.DisplayMemberPath = "numeroFactura";
                ltsFactura.SelectedValuePath = "idfacturas";
                ltsFactura.SelectedIndex = 0;
            }



        }


        private void dgvFactura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbordenCompra_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String consulta = "SELECT idfacturas,numeroFactura FROM facturasalida WHERE FK_idOrdenCompra = '" + cmbordenCompra.SelectedValue + "' ";
            conexion.Consulta(consulta, ltsFactura);
            ltsFactura.DisplayMemberPath = "numeroFactura";
            ltsFactura.SelectedValuePath = "idfacturas";
            ltsFactura.SelectedIndex = 0;
        }

        private void ltsFactura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            lblNC.Visibility = Visibility.Collapsed;

            try
            {

                String productosFatura = "SELECT DISTINCT t1.subtotal, t2.nombre ,t2.precioUnitario,t2.idProductos, t1.cantidad from productos_has_facturassalida t1, productos_has_ordencomprasalida t3 inner join productos t2 where t1.FK_idProductos = t2.idProductos and t1.FK_idfacturas = '" + ltsFactura.SelectedValue + "'";

                productos = conexion.ConsultaParametrizada(productosFatura, ltsFactura.SelectedValue);
                dgvProductosFactura.ItemsSource = productos.AsDataView();


                String consulta2 = "SELECT * FROM facturasalida f where f.idfacturas ='" + ltsFactura.SelectedValue + "'";
                DataTable OC = conexion.ConsultaParametrizada(consulta2, ltsFactura.SelectedValue);

                DateTime fecha = (DateTime)OC.Rows[0].ItemArray[7];
                txtfecha.Text = fecha.ToString("dd/MM/yyyy");
                txtoc.Text = OC.Rows[0].ItemArray[8].ToString();



                if (rbInterno.IsChecked == true)
                {
                    String pr = "select p.nombre from facturasalida f, ordencomprasalida o, clientesmi p where f.FK_idOrdenCompra = o.idOrdenCompra and o.FK_idClientemi = p.idClientemi and f.idfacturas  ='" + ltsFactura.SelectedValue + "'";
                    String p = conexion.ValorEnVariable(pr).ToString();
                    txtproveedor.Text = p;
                }
                else
                {
                    String pr = "select p.nombre from facturasalida f, ordencomprasalida o, clientesme p where f.FK_idOrdenCompra = o.idOrdenCompra and o.FK_idClienteme = p.idClienteme and f.idfacturas  = '" + ltsFactura.SelectedValue + "'";
                    String p = conexion.ValorEnVariable(pr).ToString();
                    txtproveedor.Text = p;
                }



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
                txtSubTotal.Text = OC.Rows[0].ItemArray[1].ToString();
                txtTotal1.Text = OC.Rows[0].ItemArray[3].ToString();


                String tieneNC = "SELECT COUNT(*) FROM notacredito WHERE FK_idFactura  = '" + OC.Rows[0].ItemArray[0] + "'";
                String NC = conexion.ValorEnVariable(tieneNC).ToString();

                if (NC != "0")
                {
                    lblNC.Visibility = Visibility.Visible;
                }

            }
            catch (Exception)
            {


            }



        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

            try
            {


                String tieneNC = "SELECT COUNT(*) FROM notacreditosalida WHERE FK_idFacturas  = '" + ltsFactura.SelectedValue + "'";
                String NC = conexion.ValorEnVariable(tieneNC).ToString();

                String idf = "   SELECT idCuota FROM cuotassalida WHERE FK_idfacturas =  '" + ltsFactura.SelectedValue + "'";
                String pagosi = conexion.ValorEnVariable(idf).ToString();

                String pag = "SELECT COUNT(*) FROM pagosalida WHERE FK_idCuota  = '" + pagosi + "'";
                String tienep = conexion.ValorEnVariable(pag).ToString();



                if (NC != "0" && tienep != "0")
                {
                    MessageBox.Show("La factura no se puede eliminar porque tiene un pago realizado y una nota de credito", "Error", MessageBoxButton.OK, MessageBoxImage.Error);


                }
                else if (tienep != "0")
                {
                    MessageBox.Show("La factura no se puede eliminar porque tiene un pago realizado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else if (NC != "0")
                {
                    MessageBox.Show("La factura no se puede eliminar porque tiene una nota de credito", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    DataRow selectedDataRow = ((DataRowView)ltsFactura.SelectedItem).Row;
                string numeroFactura = selectedDataRow["numeroFactura"].ToString();
                string idFactura = selectedDataRow["idfacturas"].ToString();
                MessageBoxResult dialog = MessageBox.Show("¿Esta seguro que desea eliminar la factura numero " + numeroFactura, "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);


                if (dialog == MessageBoxResult.Yes)
                {

                        int idSeleccionado = (int)ltsFactura.SelectedValue;
                        for (int i = 0; i < productos.Rows.Count; i++)
                        {
                            String orden = "SELECT FK_idOrdenCompra FROM facturasalida WHERE idfacturas = '" + idFactura + "' ";
                            String valororden = conexion.ValorEnVariable(orden);

                            String consulta = "UPDATE productos_has_ordencomprasalida SET CrFactura = CrFactura + '" + (int)productos.Rows[i].ItemArray[4] + "' where FK_idProducto = '" + productos.Rows[i].ItemArray[3] + "' and FK_idOrdenCompra = '" + valororden + "'";
                            conexion.operaciones(consulta);
                        }
                        string sql2 = "DELETE  FROM facturasalida WHERE idfacturas = '" + ltsFactura.SelectedValue + "'";
                        conexion.operaciones(sql2);

                        string sql3 = " DELETE  FROM productos_has_facturassalida WHERE FK_idfacturas =  '" + ltsFactura.SelectedValue + "'";
                        conexion.operaciones(sql3);

                        LoadDgvFactura();
                        txtIVA.Text = "";
                        txtSubTotal.Text = "";
                        txtTipoCambio.Text = "";
                        txtTotal1.Text = "";
                        MessageBox.Show("La factura se eliminó correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
                Vertodo();
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione una factura a eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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
            try
            {

                String tieneNC = "SELECT COUNT(*) FROM notacreditosalida WHERE FK_idFacturas  = '" + ltsFactura.SelectedValue + "'";
                String NC = conexion.ValorEnVariable(tieneNC).ToString();

                String idf = "   SELECT idCuota FROM cuotassalida WHERE FK_idfacturas =  '" + ltsFactura.SelectedValue + "'";
                String pagosi = conexion.ValorEnVariable(idf).ToString();

                String pag = "SELECT COUNT(*) FROM pagosalida WHERE FK_idCuota  = '" + pagosi + "'";
                String tienep = conexion.ValorEnVariable(pag).ToString();



                if (NC != "0" && tienep != "0")
                {
                    MessageBox.Show("La factura no se puede modificar porque tiene un pago realizado y una nota de credito", "Error", MessageBoxButton.OK, MessageBoxImage.Error);


                }
                else if (tienep != "0")
                {
                    MessageBox.Show("La factura no se puede modificar porque tiene un pago realizado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else if (NC != "0")
                {
                    MessageBox.Show("La factura no se puede modificar porque tiene una nota de credito", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    itemsFacturaDB.Clear();
                    itemsFacdb.Clear();
                    itemsdb.Clear();
                    int index = ltsFactura.SelectedIndex;
                    int numerofacturaID = (int)this.ltsFactura.SelectedValue;
                    Console.WriteLine("ID FACTURA " + numerofacturaID);
                    DataRow selectedDataRow = ((DataRowView)ltsFactura.SelectedItem).Row;
                    idOC = selectedDataRow["numeroFactura"].ToString();

                    String oc = "SELECT FK_idOrdenCompra FROM facturasalida WHERE idfacturas = '" + numerofacturaID + "'";
                    FKoc = conexion.ValorEnVariable(oc);

                    if (rbInterno.IsChecked == true)
                    {
                        String fkidprov = "SELECT FK_idClientemi FROM ordencomprasalida WHERE idOrdenCompra = '" + FKoc + "'";
                        fkidproveedor = conexion.ValorEnVariable(fkidprov);

                        String fkidprov1 = "   select nombre from clientesmi  where idClientemi =  '" + fkidproveedor + "'";
                        idproveed = conexion.ValorEnVariable(fkidprov1);
                    }
                    else
                    {
                        String fkidprov = "SELECT FK_idClienteme FROM ordencomprasalida WHERE idOrdenCompra = '" + FKoc + "'";
                        fkidproveedor = conexion.ValorEnVariable(fkidprov);

                        String fkidprov1 = "  select nombre from clientesme  where idClienteme =  '" + fkidproveedor + "'";
                        idproveed = conexion.ValorEnVariable(fkidprov1);
                    }





                    String proveedor = idproveed;

                    String crfact = "SELECT * FROM facturasalida WHERE idfacturas = '" + numerofacturaID + "'";
                    DataTable OC = conexion.ConsultaParametrizada(crfact, numerofacturaID);
                    DateTime fecha = (DateTime)OC.Rows[0].ItemArray[7];
                    String cuotas = OC.Rows[0].ItemArray[6].ToString();
                    Int64 numf = (Int64)OC.Rows[0].ItemArray[2];
                    int iva;
                    int tipocambio;

                    int j = 0;
                    for (int i = 0; i < dgvProductosFactura.Items.Count; i++)
                    {

                        var total = (dgvProductosFactura.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
                        j++;

                        var nombre = (dgvProductosFactura.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
                        j++;

                        var precioU = (dgvProductosFactura.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
                        j++;


                        var id = (dgvProductosFactura.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
                        j++;


                        var cantidadrestante = (dgvProductosFactura.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
                        j++;
                        j = 0;


                        subtotal = subtotal + float.Parse(precioU) * float.Parse(total);
                        Producto conA = new Producto(nombre, int.Parse(id), int.Parse(cantidadrestante), float.Parse(total), float.Parse(precioU));
                        itemsFacdb.Add(conA);

                    }

                    Cuotas cuota;
                    cuotasAinsertar.Clear();
                    try
                    {

                        String sql2 = "SELECT * from cuotassalida c  where c.FK_idfacturas = '" + numerofacturaID + "'";

                        DataTable cuotass = conexion.ConsultaParametrizada(sql2, numerofacturaID);
                        for (int i = 0; i < cuotass.Rows.Count; i++)
                        {

                            cuota = new Cuotas((int)cuotass.Rows[i].ItemArray[0], (int)cuotass.Rows[i].ItemArray[1], (DateTime)cuotass.Rows[i].ItemArray[2], (float)cuotass.Rows[i].ItemArray[3], (int)cuotass.Rows[i].ItemArray[4]);
                            cuotasAinsertar.Add(cuota);

                        }

                    }
                    catch (NullReferenceException)
                    {


                    }

                    Producto producto;
                    itemsdb.Clear();

                    try
                    {


                        String sql2 = "SELECT productos.nombre, productos.idProductos, productos_has_ordencomprasalida.CrFactura, subtotal, productos_has_ordencomprasalida.PUPagado  FROM productos_has_ordencomprasalida, productos WHERE FK_idOrdenCompra ='" + FKoc + "' AND productos.idProductos = productos_has_ordencomprasalida.FK_idProducto";

                        DataTable productos = conexion.ConsultaParametrizada(sql2, Int64.Parse(idOC));
                        for (int i = 0; i < productos.Rows.Count; i++)
                        {
                            producto = new Producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[1], (int)productos.Rows[i].ItemArray[2], (float)productos.Rows[i].ItemArray[3], (float)productos.Rows[i].ItemArray[4]);
                            itemsdb.Add(producto);

                        }


                    }
                    catch (NullReferenceException)
                    {


                    }

                    if (txtIVA.Text == "0")
                    {
                        iva = 0;
                    }
                    else if (txtIVA.Text == "21")
                    {
                        iva = 1;
                    }
                    else
                    {
                        iva = 2;
                    }

                    if (txtTipoCambio.Text == "$")
                    {
                        tipocambio = 0;
                    }
                    else if (txtTipoCambio.Text == "u$d")
                    {
                        tipocambio = 1;
                    }
                    else
                    {
                        tipocambio = 2;
                    }

                    int tipoCliente;
                    if (rbInterno.IsChecked == true)
                    {
                        tipoCliente = 1;
                    }
                    else
                    {
                        tipoCliente = 2;
                    }

                    var newW = new windowAgregarFacturaSalidas((Int64)numf, proveedor, itemsdb, itemsFacdb, fecha, int.Parse(txtoc.Text), float.Parse(txtSubTotal.Text), float.Parse(txtTotal1.Text), iva, tipocambio, subtotal, cuotas, cuotasAinsertar, tipoCliente);
                    newW.Title = "Modificar Factura";

                    newW.ShowDialog();

                    if (newW.DialogResult == true)
                    {

                        String idPR = newW.txtcliente.Text.ToString();

                        if (rbInterno.IsChecked == true)
                        {
                            string sql32 = "SELECT idClientemi FROM clientesmi WHERE nombre = '" + idPR + "'";
                            string idProve2 = conexion.ValorEnVariable(sql32);
                        }
                        else
                        {
                            string sql32 = "SELECT idClienteme FROM clientesme WHERE nombre = '" + idPR + "'";
                            string idProve2 = conexion.ValorEnVariable(sql32);
                        }



                        decimal subtotal2 = decimal.Parse(newW.txtSubtotal.Text);
                        decimal total2 = decimal.Parse(newW.txtTotal.Text);
                        Int64 numeroFact2 = Int64.Parse(newW.txtNroFactura.Text);
                        String iva32 = newW.cmbIVA.SelectedIndex.ToString();
                        String tipoCambio2 = newW.cmbTipoCambio.SelectedIndex.ToString();
                        String cuotas2 = newW.cmbCuotas.Text;
                        int fkOrden2 = int.Parse(newW.txtordenn.Text);
                        DateTime dtp2 = System.DateTime.Now;
                        dtp2 = newW.dtFactura.SelectedDate.Value;

                        try
                        {
                            //UPDATE FACTURA
                            String updatefactura = "UPDATE facturasalida SET subtotal =  '" + subtotal2 + "',numeroFactura = '" + numeroFact2 + "' ,total = '" + total2 + "',iva= '" + iva32 + "',tipocambio='" + tipoCambio2 + "' ,cuotas = '" + cuotas2 + "',FK_idOrdenCompra= '" + fkOrden2 + "',fecha ='" + dtp2.ToString("yyyy/MM/dd") + "' WHERE idfacturas = '" + numerofacturaID + "'";
                            conexion.operaciones(updatefactura);
                        }
                        catch (Exception)
                        {


                        }

                        string sql2 = "DELETE  FROM cuotassalida WHERE FK_idfacturas = '" + numerofacturaID + "'";
                        conexion.operaciones(sql2);


                        //inserto cuotas en la factura
                        foreach (Cuotas cc in newW.todaslascuotas)
                        {
                            int id2 = cc.cuota;
                            int dias12 = cc.dias;
                            DateTime fecha21 = cc.fechadepago;
                            float montoCuota = cc.montoCuota;


                            String insertcuotas = "INSERT INTO cuotassalida ( dias, fecha, montoCuota, FK_idFacturas, estado) VALUES ('" + dias12 + "', '" + fecha21.ToString("yyyy/MM/dd") + "','" + montoCuota + "', '" + numerofacturaID + "', '" + 0 + "')";
                            conexion.operaciones(insertcuotas);


                        }


                        String deleteproducto = "DELETE FROM productos_has_facturassalida WHERE FK_idfacturas = '" + numerofacturaID + "'";
                        conexion.operaciones(deleteproducto);



                        //INSERTO LOS PRODUCTOS DE LA FACTURA
                        itemsFacturaDB.Clear();
                        foreach (Producto p in newW.itemsFact)
                        {
                            String nombre = p.nombre;
                            int cantidad = p.cantidad;
                            float totalp = p.total;
                            float precioUni = p.precioUnitario;
                            int idProducto = p.id;

                            Producto pr = new Producto(nombre, idProducto, cantidad, totalp, precioUni);
                            itemsFacturaDB.Add(pr);


                            String sqlProductoHas = "INSERT INTO productos_has_facturassalida (cantidad, subtotal, FK_idProductos, FK_idFacturas) VALUES ('" + cantidad + "','" + subtotal2 + "', '" + idProducto + "', '" + numerofacturaID + "')";
                            conexion.operaciones(sqlProductoHas);


                        }

                        int idOrden = int.Parse(newW.txtordenn.Text);
                        foreach (var producto1 in newW.items)
                        {
                            String sql = "UPDATE productos_has_ordencomprasalida SET CrFactura = '" + producto1.cantidad + "' where FK_idProducto = '" + producto1.id + "' and FK_idOrdenCompra = '" + idOrden + "'";
                            conexion.operaciones(sql);
                        }
                        MessageBox.Show("La factura se modificó correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);

                    }



                    LoadDgvFactura();
                    Vertodo(index);
                   
                }
               
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione una factura a modificar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
       

            }
        }
        private void btnVertodo_Click(object sender, RoutedEventArgs e)
        {

            if (rbInterno.IsChecked == true)
            {
                cmbCliente.SelectedIndex = -1;
                cmbordenCompra.SelectedIndex = -1;
                seleccioneParaFiltrar();
                String consulta = "select * from facturasalida f, ordencomprasalida o WHERE f.FK_idOrdenCompra = o.idOrdenCompra AND FK_idClientemi IS NOT NULL ";
                conexion.Consulta(consulta, tabla: ltsFactura);
                ltsFactura.DisplayMemberPath = "numeroFactura";
                ltsFactura.SelectedValuePath = "idfacturas";
                ltsFactura.SelectedIndex = 0;

            }
            else
            {
                cmbCliente.SelectedIndex = -1;
                cmbordenCompra.SelectedIndex = -1;
                seleccioneParaFiltrar();
                String consulta = "select * from facturasalida f, ordencomprasalida o WHERE f.FK_idOrdenCompra = o.idOrdenCompra AND FK_idClienteme IS NOT NULL";
                conexion.Consulta(consulta, tabla: ltsFactura);
                ltsFactura.DisplayMemberPath = "numeroFactura";
                ltsFactura.SelectedValuePath = "idfacturas";
                ltsFactura.SelectedIndex = 0;

            }

        }

        private void LoadListfactura()
        {
            if (rbInterno.IsChecked == true)
            {
                String consulta = "select idfacturas, numeroFactura from facturasalida f, ordencomprasalida o WHERE f.FK_idOrdenCompra = o.idOrdenCompra AND FK_idClientemi IS NOT NULL ";
                conexion.Consulta(consulta, ltsFactura);
                ltsFactura.DisplayMemberPath = "numeroFactura";
                ltsFactura.SelectedValuePath = "idfacturas";
                ltsFactura.SelectedIndex = 0;
            }
            else
            {
                String consulta = "select idfacturas, numeroFactura from facturasalida f, ordencomprasalida o WHERE f.FK_idOrdenCompra = o.idOrdenCompra AND FK_idClienteme IS NOT NULL";
                conexion.Consulta(consulta, ltsFactura);
                ltsFactura.DisplayMemberPath = "numeroFactura";
                ltsFactura.SelectedValuePath = "idfacturas";
                ltsFactura.SelectedIndex = 0;
            }

       

        }

        private void seleccioneParaFiltrar()
        {
           cmbCliente.Text = "Seleccione para filtrar";
           cmbordenCompra.Text = "Seleccione para filtrar";
        }

        private void Vertodo(int index)
        {

            if (rbInterno.IsChecked == true)
            {
                cmbCliente.SelectedIndex = -1;
                cmbordenCompra.SelectedIndex = -1;
                seleccioneParaFiltrar();
                String consulta = "select * from facturasalida f, ordencomprasalida o WHERE f.FK_idOrdenCompra = o.idOrdenCompra AND FK_idClientemi IS NOT NULL ";
                conexion.Consulta(consulta, tabla: ltsFactura);
                ltsFactura.DisplayMemberPath = "numeroFactura";
                ltsFactura.SelectedValuePath = "idfacturas";
                ltsFactura.SelectedIndex = 0;

            }
            else
            {
                cmbCliente.SelectedIndex = -1;
                cmbordenCompra.SelectedIndex = -1;
                seleccioneParaFiltrar();
                String consulta = "select * from facturasalida f, ordencomprasalida o WHERE f.FK_idOrdenCompra = o.idOrdenCompra AND FK_idClienteme IS NOT NULL";
                conexion.Consulta(consulta, tabla: ltsFactura);
                ltsFactura.DisplayMemberPath = "numeroFactura";
                ltsFactura.SelectedValuePath = "idfacturas";
                ltsFactura.SelectedIndex = 0;

            }

        }

        private void Vertodo()
        {

            if (rbInterno.IsChecked == true)
            {
                cmbCliente.SelectedIndex = -1;
                cmbordenCompra.SelectedIndex = -1;
                seleccioneParaFiltrar();
                String consulta = "select * from facturasalida f, ordencomprasalida o WHERE f.FK_idOrdenCompra = o.idOrdenCompra AND FK_idClientemi IS NOT NULL ";
                conexion.Consulta(consulta, tabla: ltsFactura);
                ltsFactura.DisplayMemberPath = "numeroFactura";
                ltsFactura.SelectedValuePath = "idfacturas";
                ltsFactura.SelectedIndex = 0;

            }
            else
            {
                cmbCliente.SelectedIndex = -1;
                cmbordenCompra.SelectedIndex = -1;
                seleccioneParaFiltrar();
                String consulta = "select * from facturasalida f, ordencomprasalida o WHERE f.FK_idOrdenCompra = o.idOrdenCompra AND FK_idClienteme IS NOT NULL";
                conexion.Consulta(consulta, tabla: ltsFactura);
                ltsFactura.DisplayMemberPath = "numeroFactura";
                ltsFactura.SelectedValuePath = "idfacturas";
                ltsFactura.SelectedIndex = 0;

            }

        }

        private void rbExterno_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("ME");
            LoadListaCME();
            seleccioneParaFiltrar();
            Loadltsfactura();
          
        }
        
        

        private void rbInterno_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("MI");
            LoadListaCMI();
            ltsFactura.SelectedIndex = -1;
            seleccioneParaFiltrar();
            Loadltsfactura();
        }
    }
}

