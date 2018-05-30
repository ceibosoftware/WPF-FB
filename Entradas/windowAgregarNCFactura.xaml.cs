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
        public List<Producto> itemsNC = new List<Producto>();
        public List<Producto> itemsFact = new List<Producto>();
        public List<Producto> itemsAmodificar = new List<Producto>();
        public List<Producto> itemsNCAntiguos = new List<Producto>();
        public int id = 0;
        float subtotal=0;
        public Producto prod;
        public Producto producto;
        public String idFactura;
        String iva;
        String cambio;
        public float subtotali ;
        public String subtotalmodificar;
        public int idnota;
        public String totalmodificar;
        public int tipo;
        Boolean modifica;
        public String idOC;
        public windowAgregarNCFactura()
        {
            InitializeComponent();
            loadLtsfactura();
            txtIVA.IsReadOnly = true;
          //  txtTotal.IsReadOnly = true;
            txtTipoCambio.IsReadOnly = true;
            txtSubtotal.IsReadOnly = true;
            LoadDgvNC();
            dgvProductosNC.IsReadOnly = true;
            DgvProductosFactur.IsReadOnly = true;
            itemsFact.Clear();
            txtTotal.IsReadOnly = true;
            LoadDgvProdFactura();
            txtProveedor.IsReadOnly = true;

        }

        public windowAgregarNCFactura(int tipo1)
        {
            InitializeComponent();
            tipo = tipo1;
            loadLtsfacturaSalida();
            txtIVA.IsReadOnly = true;
            //  txtTotal.IsReadOnly = true;
            txtTipoCambio.IsReadOnly = true;
            txtSubtotal.IsReadOnly = true;
            LoadDgvNC();
            dgvProductosNC.IsReadOnly = true;
            DgvProductosFactur.IsReadOnly = true;
            itemsFact.Clear();
            txtTotal.IsReadOnly = true;
            LoadDgvProdFactura();
            txtProveedor.IsReadOnly = true;
        

        }

        public windowAgregarNCFactura(String subtotal, String total, String iva, String cambio, List<Producto>ProdAmodificar, String idfactura1, int idnotac)
        {
            InitializeComponent();
            // loadLtsfactura();
            
            txtIVA.IsReadOnly = true;
            txtTotal.IsReadOnly = true;
            txtTipoCambio.IsReadOnly = true;
            txtSubtotal.IsReadOnly = true;
            loadLtsfactura(idfactura1);
            ltsfacturas.IsEnabled = false;
            idnota = idnotac;
            itemsNC = ProdAmodificar;
            txtSubtotal.Text = subtotal;
            txtTotal.Text = total;
            txtIVA.Text = iva;
            txtTipoCambio.Text = cambio;
            dgvProductosNC.IsReadOnly = true;
            DgvProductosFactur.IsReadOnly = true;
            txtProveedor.IsReadOnly = true;
            LoadDgvNC(itemsNC);
            LoadDgvProdFactura();
            modifica = true;
            lblWindowTitle.Content = "Modificar Nota de Crédito";
            backupproductos(ProdAmodificar);
            modifica = false;

        }

        private void backupproductos(List<Producto> productosNC)
        {
            foreach (Producto producto in productosNC)
            {
                itemsNCAntiguos.Add(producto);
            }
        }
        //desde salidas
        public windowAgregarNCFactura(String subtotal, String total, String iva, String cambio, List<Producto> ProdAmodificar, String idfactura1, int idnotac, int tipo3)
        {
            InitializeComponent();
            tipo = tipo3;
            MessageBox.Show("id factura" + idfactura1);
           // loadLtsfacturaSalida();
            loadLtsfacturaSalida(idfactura1);
            txtIVA.IsReadOnly = true;
            txtTotal.IsReadOnly = true;
            txtTipoCambio.IsReadOnly = true;
            txtSubtotal.IsReadOnly = true;
   //         loadLtsfacturaSalida(idfactura1);
            ltsfacturas.IsEnabled = false;
            idnota = idnotac;
            itemsNC = ProdAmodificar;
            txtSubtotal.Text = subtotal;
            txtTotal.Text = total;
            txtIVA.Text = iva;
            txtTipoCambio.Text = cambio;
            dgvProductosNC.IsReadOnly = true;
            DgvProductosFactur.IsReadOnly = true;
            txtProveedor.IsReadOnly = true;
            LoadDgvNC(itemsNC);
            LoadDgvProdFactura();
            lblWindowTitle.Content = "Modificar Nota de Crédito";

        }


        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void LoadDgvNC()
        {
            dgvProductosNC.AutoGenerateColumns = false;
            DataGridTextColumn textColum1n = new DataGridTextColumn();
            textColum1n.Header = "Nombre";
            textColum1n.Binding = new Binding("nombre");
            dgvProductosNC.Columns.Add(textColum1n);
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Cantidad";
            textColumn.Binding = new Binding("cantidad");
            dgvProductosNC.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio Unitario";
            textColumn2.Binding = new Binding("precioUnitario");
            dgvProductosNC.Columns.Add(textColumn2);
            dgvProductosNC.ItemsSource = itemsNC;
        }


        public void LoadDgvProdFactura()
        {
            DgvProductosFactur.AutoGenerateColumns = false;
            DataGridTextColumn textColum1n = new DataGridTextColumn();
            textColum1n.Header = "Nombre";
            textColum1n.Binding = new Binding("nombre");
            DgvProductosFactur.Columns.Add(textColum1n);
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Cantidad";
            textColumn.Binding = new Binding("cantidad");
            DgvProductosFactur.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio Unitario";
            textColumn2.Binding = new Binding("precioUnitario");
            DgvProductosFactur.Columns.Add(textColumn2);
           
        }

        public void LoadDgvNC(List<Producto> p)
        {
            dgvProductosNC.AutoGenerateColumns = false;
            DataGridTextColumn textColum1n = new DataGridTextColumn();
            textColum1n.Header = "Nombre";
            textColum1n.Binding = new Binding("nombre");
            dgvProductosNC.Columns.Add(textColum1n);
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Cantidad";
            textColumn.Binding = new Binding("cantidad");
            dgvProductosNC.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio Unitario";
            textColumn2.Binding = new Binding("precioUnitario");
            dgvProductosNC.Columns.Add(textColumn2);
            dgvProductosNC.ItemsSource = p;
        }
        public void loadLtsfactura()
        {
            String consulta = "SELECT idfacturas,numeroFactura FROM factura  ";
            conexion.Consulta(consulta, ltsfacturas);
            ltsfacturas.DisplayMemberPath = "numeroFactura";
            ltsfacturas.SelectedValuePath = "idfacturas";
            ltsfacturas.SelectedIndex = -1;
            ltsfacturas.SelectedIndex = 0;
            ltsfacturas.SelectedIndex = 1;
            ltsfacturas.SelectedIndex = 0;
        }

        public void loadLtsfacturaSalida()
        {
            String consulta = "SELECT idfacturas,numeroFactura FROM facturasalida  ";
            conexion.Consulta(consulta, ltsfacturas);
            ltsfacturas.DisplayMemberPath = "numeroFactura";
            ltsfacturas.SelectedValuePath = "idfacturas";
            ltsfacturas.SelectedIndex = 0;
        }

        public void loadLtsfactura(String idfacturas)
        {

            String consulta = "SELECT idfacturas,numeroFactura FROM factura WHERE idfacturas = '"+idfacturas+"'  ";
            conexion.Consulta(consulta, ltsfacturas);
            ltsfacturas.DisplayMemberPath = "numeroFactura";
            ltsfacturas.SelectedValuePath = "idfacturas";
            ltsfacturas.SelectedIndex = 0;
           
        }
        public void loadLtsfacturaSalida(String idfac)
        {
            MessageBox.Show("asdsa" + idfac);
            String consulta2 = "SELECT idfacturas, numeroFactura FROM facturasalida WHERE idfacturas = '" + idfac + "'";
            conexion.Consulta(consulta2, ltsfacturas);
            ltsfacturas.DisplayMemberPath = "numeroFactura";
            ltsfacturas.SelectedValuePath = "idfacturas";
            ltsfacturas.SelectedIndex = 0;

        }

        public void calculaTotal()
        {
            if (txtIVA.Text == "0")
            {
                txtTotal.Text = txtSubtotal.Text;
            }
            else if (txtIVA.Text == "21")
            {
                txtTotal.Text =( float.Parse(txtSubtotal.Text) * (float)1.21).ToString();
           
            }
            else
            {
                txtTotal.Text = (float.Parse(txtSubtotal.Text) * (float)1.105).ToString();
            }
    
        }
        private void ltsfacturas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //try
            //{
            if (!modifica)
            {

            
                itemsFact.Clear();
                itemsNC.Clear();
                dgvProductosNC.Items.Refresh();
                iva = "";
                cambio = "";

                if (tipo == 1)
                {
                    String productosFatura = "SELECT DISTINCT t1.subtotal, t2.nombre ,t2.precioUnitario,t2.idProductos,t1.CrNotaCredito from productos_has_facturassalida t1, productos_has_ordencomprasalida t3 inner join productos t2 where t1.FK_idProductos = t2.idProductos and t1.FK_idfacturas = '" + ltsfacturas.SelectedValue + "'";

                    productos = conexion.ConsultaParametrizada(productosFatura, ltsfacturas.SelectedValue);
                    DgvProductosFactur.ItemsSource = productos.AsDataView();

                    for (int i = 0; i < productos.Rows.Count; i++)
                    {
                        producto = new Producto(productos.Rows[i].ItemArray[1].ToString(), (int)productos.Rows[i].ItemArray[3], (int)productos.Rows[i].ItemArray[4], (float)productos.Rows[i].ItemArray[0], (float)productos.Rows[i].ItemArray[2]);

                        itemsFact.Add(producto);

                    }

                    DgvProductosFactur.ItemsSource = itemsFact;
                    DgvProductosFactur.Items.Refresh();

                    String consulta2 = "SELECT * FROM facturasalida f where f.idfacturas ='" + ltsfacturas.SelectedValue + "'";
                    DataTable OC = conexion.ConsultaParametrizada(consulta2, ltsfacturas.SelectedValue);

                    String idOC = "SELECT FK_idOrdenCompra FROM facturasalida WHERE idfacturas = '" + ltsfacturas.SelectedValue + "' ";
                    String idorc = conexion.ValorEnVariable(idOC);


                    //String idprov = "SELECT FK_idProveedor FROM ordencomprasalida WHERE idOrdenCompra = '" + idorc + "' ";
                    //String idprove = conexion.ValorEnVariable(idprov);

                    //String nombreprove = "SELECT nombre FROM proveedor WHERE idProveedor = '" + idprove + "' ";
                    //String nombrepv = conexion.ValorEnVariable(nombreprove);

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

                    //txtProveedor.Text = nombrepv;

                    DgvProductosFactur.SelectedIndex = 0;

                    txtIVA.Text = iva;
                    txtTipoCambio.Text = cambio;
                    txtSubtotal.Text = "0";
                    txtTotal.Text = "0";



                }
                else
                {
                    String productosFatura = "SELECT DISTINCT t1.subtotal, t2.nombre ,t2.precioUnitario,t2.idProductos,t1.CrNotaCredito from productos_has_facturas t1, productos_has_ordencompra t3 inner join productos t2 where t1.FK_idProducto = t2.idProductos and t1.FK_idfactura = '" + ltsfacturas.SelectedValue + "'";

                    productos = conexion.ConsultaParametrizada(productosFatura, ltsfacturas.SelectedValue);
                    //DgvProductosFactur.ItemsSource = productos.AsDataView();

                    for (int i = 0; i < productos.Rows.Count; i++)
                    {
                        producto = new Producto(productos.Rows[i].ItemArray[1].ToString(), (int)productos.Rows[i].ItemArray[3], (int)productos.Rows[i].ItemArray[4], (float)productos.Rows[i].ItemArray[0], (float)productos.Rows[i].ItemArray[2]);

                        itemsFact.Add(producto);

                    }

                    DgvProductosFactur.ItemsSource = itemsFact;
                    DgvProductosFactur.Items.Refresh();

                    String consulta2 = "SELECT * FROM factura f where f.idfacturas ='" + ltsfacturas.SelectedValue + "'";
                    DataTable OC = conexion.ConsultaParametrizada(consulta2, ltsfacturas.SelectedValue);

                    String idOC = "SELECT FK_idOC FROM factura WHERE idfacturas = '" + ltsfacturas.SelectedValue + "' ";
                    String idorc = conexion.ValorEnVariable(idOC);


                    String idprov = "SELECT FK_idProveedor FROM ordencompra WHERE idOrdenCompra = '" + idorc + "' ";
                    String idprove = conexion.ValorEnVariable(idprov);

                    String nombreprove = "SELECT nombre FROM proveedor WHERE idProveedor = '" + idprove + "' ";
                    String nombrepv = conexion.ValorEnVariable(nombreprove);

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

                    txtProveedor.Text = nombrepv;

                    DgvProductosFactur.SelectedIndex = 0;

                    txtIVA.Text = iva;
                    txtTipoCambio.Text = cambio;
                    txtSubtotal.Text = "0";
                    txtTotal.Text = "0";



                }






                //}
                //catch (Exception)
                //{


                //}

            }

        }

        private void btnAgregarProductoNC_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                bool existe = false;
                Producto prod = DgvProductosFactur.SelectedItem as Producto;

                id = prod.id;

                if (prod.cantidad > 0)
                {
                    var newW = new WindowAgregarProductoFactura();
                    for (int i = 0; i < itemsNC.Count; i++)
                    {
                        if (itemsNC[i].nombre == prod.nombre)
                        {
                            existe = true;
                        }
                        else
                        {
                            existe = false;
                        }
                    }
                    if (prod.cantidad >= 1 && !existe)
                    {

                        newW.txtCantidad.Text = prod.cantidad.ToString();
                        newW.can = prod.cantidad;
                        newW.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("El producto ya se agrego");
                    }


                    if (newW.DialogResult == true)
                    {
                        string consulta = "SELECT stock from productos where idProductos = " + prod.id + " ";
                        if (tipo != 1)
                        {
                            if (int.Parse(conexion.ValorEnVariable(consulta)) >= int.Parse(newW.txtCantidad.Text))
                            {
                                if (int.Parse(newW.txtCantidad.Text) > 0)
                                {

                                    Producto productoFactura = new Producto(prod.nombre, prod.id, int.Parse(newW.txtCantidad.Text), int.Parse(newW.txtCantidad.Text) * prod.precioUnitario, prod.precioUnitario);
                                    itemsNC.Add(productoFactura);
                                    dgvProductosNC.Items.Refresh();
                                    float.TryParse(txtSubtotal.Text, out subtotal);
                                    subtotal += productoFactura.total;
                                    txtSubtotal.Text = (subtotal).ToString();
                                    prod.cantidad = prod.cantidad - int.Parse(newW.txtCantidad.Text);
                                    DgvProductosFactur.Items.Refresh();
                                    calculaTotal();

                                }
                                else
                                {
                                    MessageBox.Show("La cantidad no puede ser cero");
                                }
                            }
                            else
                            {
                                MessageBox.Show("La cantidad ingresada supera al stock del producto");
                            }
                        }

                        else
                        {
                            if (int.Parse(newW.txtCantidad.Text) > 0)
                            {

                                Producto productoFactura = new Producto(prod.nombre, prod.id, int.Parse(newW.txtCantidad.Text), int.Parse(newW.txtCantidad.Text) * prod.precioUnitario, prod.precioUnitario);
                                itemsNC.Add(productoFactura);
                                dgvProductosNC.Items.Refresh();
                                float.TryParse(txtSubtotal.Text, out subtotal);
                                subtotal += productoFactura.total;
                                txtSubtotal.Text = (subtotal).ToString();
                                prod.cantidad = prod.cantidad - int.Parse(newW.txtCantidad.Text);
                                DgvProductosFactur.Items.Refresh();
                                calculaTotal();

                            }
                            else
                            {
                                MessageBox.Show("La cantidad no puede ser cero");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ya se entregaron todas las ordenes de compra de este producto");
                    }
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un producto para agregar");

            }
        }

       

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            idFactura = ltsfacturas.SelectedValue.ToString();
            subtotalmodificar = txtSubtotal.Text;
            totalmodificar = txtTotal.Text;

            if (tipo == 1)
            {
                if (Valida() && VerEstadoSalida())
                {
                    DialogResult = true;
                }
            }
            else
            {
                if (Valida() && VerEstado())
                {
                    DialogResult = true;
                }
            }
            
         
           
 
        }
        
        public bool VerEstado()
        {
            String idOC = "SELECT FK_idOC FROM factura WHERE idfacturas ='" + idFactura + "' ";
           this.idOC = conexion.ValorEnVariable(idOC);
            String estOC = "SELECT estadoNC FROM ordencompra WHERE idOrdenCompra ='" + conexion.ValorEnVariable(idOC) + "'";

            String estadoOC = conexion.ValorEnVariable(estOC);

            if (estadoOC =="1")
            {
                MessageBox.Show("No se puede agregar la Nota de Crédito porque ya tiene una Nota de Crédito de Remito.");

                return false;
            }
            else
            {
                return true;
            }

        }
        public bool VerEstadoSalida()
        {
            String idOC = "SELECT FK_idOrdenCompra FROM facturasalida WHERE idfacturas ='" + idFactura + "' ";
            this.idOC = conexion.ValorEnVariable(idOC);
            String estOC = "SELECT estadoNC FROM ordencomprasalida WHERE idOrdenCompra ='" + conexion.ValorEnVariable(idOC) + "'";

            String estadoOC = conexion.ValorEnVariable(estOC);

            if (estadoOC == "1")
            {
                MessageBox.Show("No se puede agregar la Nota de Crédito porque ya tiene una Nota de Crédito de Remito.");

                return false;
            }
            else
            {
                return true;
            }

        }
        public Boolean Valida()
        {

            if (DgvProductosFactur.HasItems == false)
            {
                MessageBox.Show("Agregue productos a la Nota de Crédito");
                return false;
            }
            else if (dgvProductosNC.HasItems == false)
            {
                MessageBox.Show("Agregue productos a la Nota de Crédito");
                return false;
            }
            else
            {
                return true;
            }


        }
        private void btnEliminarProductoNC_Click(object sender, RoutedEventArgs e)
        {
         
            try
            {

                prod = dgvProductosNC.SelectedItem as Producto;
                int cantidadAsumar = 0;

                cantidadAsumar = prod.cantidad;

                for (int i = 0; i < itemsFact.Count; i++)
                {
                    if (itemsFact[i].nombre == prod.nombre)
                    {
                        itemsFact[i].cantidad += prod.cantidad;

                    }

                }
                subtotali = subtotali - prod.cantidad * prod.precioUnitario;
                txtSubtotal.Text = subtotali.ToString();
                calculaTotal();
                DgvProductosFactur.Items.Refresh();
                itemsNC.Remove(prod);
                itemsAmodificar.Remove(prod);
                dgvProductosNC.Items.Refresh();
           
        
                if (dgvProductosNC.HasItems == false)
                {
                    txtSubtotal.Text = "0";
                    txtTotal.Text = "0";
                    subtotali = 0;
                    dgvProductosNC.Items.Refresh();
                }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un producto");
            }

        }

        private void DgvProductosFactur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
          
    }

 
