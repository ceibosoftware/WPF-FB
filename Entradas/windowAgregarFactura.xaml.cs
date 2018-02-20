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
    /// Interaction logic for windowAgregarFactura.xaml
    /// </summary>
    public partial class windowAgregarFactura : Window
    {
        float subtotal;
        float subtotali =0;
        float total;
        CRUD conexion = new CRUD();
        public List<Producto> items = new List<Producto>();
        public List<Producto> itemsFact = new List<Producto>();
        public List<Cuotas> todaslascuotas = new List<Cuotas>();
        Producto producto;
        public Producto prod;
        bool bandera = false;
        DateTime dt = DateTime.Now;
        public int id = 0;
        public windowAgregarFactura()
        {
            itemsFact.Clear();
            items.Clear();
            todaslascuotas.Clear();
            InitializeComponent();
            LoadListaComboProveedor();
            LlenarCmbIVA();
            LlenarCmbTipoCambio();
            LoadDgvProducto();
            LoadDgvFactura();
            loadDGVCuotas();
            LlenarCmbTipoCuota();
            txtNroFactura.MaxLines = 1;
            txtNroFactura.MaxLength = 10;
            txtTotal.IsReadOnly = true;
            txtSubtotal.IsReadOnly = true;
            dgvProductosFactura.IsReadOnly = true;
            dgvProductosOC.IsReadOnly = true;
            seleccionefecha();
            dtFactura.SelectedDate = DateTime.Now;

            bandera = true;
        }

        public windowAgregarFactura(int numFactura, String proveedor, List<Producto> pOC, List<Producto> pFA, DateTime fechafactura, int numeroOC, float subtotal, float total, int IVA, int tipoCambio, float subtotal2, String cuotas, List<Cuotas> lCU)
        {
            InitializeComponent();
           // try
            //{

                txtNroFactura.MaxLines = 1;
                txtNroFactura.MaxLength = 10;
                itemsFact.Clear();
                items.Clear();
                todaslascuotas.Clear();
                LoadListaComboProveedor();
                LlenarCmbIVA();
                LlenarCmbTipoCambio();
                LoadDgvProducto(pOC);
                LoadDgvFactura(pFA);
                LlenarCmbTipoCuota();
                loadDGVCuotas(lCU);
                cmbProveedores.IsEnabled = false;
                cmbCuotas.IsEnabled = false;
                txtFiltro.IsEnabled = false;

            this.txtNroFactura.Text = numFactura.ToString();
                this.cmbProveedores.Text = proveedor;
                this.items = pOC;
                this.cmbCuotas.Text = cuotas;
                dtFactura.SelectedDate = fechafactura;
                cmbOrden.Text = numeroOC.ToString();
                this.subtotali = subtotal;
                txtSubtotal.Text = subtotal.ToString();
                cmbIVA.SelectedIndex = IVA;
                cmbTipoCambio.SelectedIndex = tipoCambio;
                txtTotal.Text = total.ToString();
                dt = fechafactura.Date;
                this.itemsFact = pFA;
                this.todaslascuotas = lCU;
       
                bandera = true;

                txtTotal.IsReadOnly = true;
                txtSubtotal.IsReadOnly = true;
                dgvProductosFactura.IsReadOnly = true;
                dgvProductosOC.IsReadOnly = true;
          
            //}
            //catch (Exception)
          //  {

            //}
        }
        private void seleccionefecha()
        {
            cmbCuotas.Text = "--Seleccione fecha factura--";

        }
        public void LoadListaComboProveedor()
        {
            String consulta = "SELECT DISTINCT p.nombre, p.idProveedor FROM proveedor p INNER JOIN ordencompra o ON p.idProveedor = o.FK_idProveedor";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
            cmbProveedores.SelectedIndex = 0;
        }

    

        private void LlenarCmbIVA()
        {
            cmbIVA.Items.Add("0");
            cmbIVA.Items.Add("21");
            cmbIVA.Items.Add("10,5");
        }

        private void LlenarCmbTipoCambio()
        {
            cmbTipoCambio.Items.Add("$");
            cmbTipoCambio.Items.Add("u$d");
            cmbTipoCambio.Items.Add("€");
        }

        private void LlenarCmbTipoCuota()
        {
            cmbCuotas.Items.Add("1");
            cmbCuotas.Items.Add("2");
            cmbCuotas.Items.Add("3");
            cmbCuotas.Items.Add("4");
            cmbCuotas.Items.Add("5");
            cmbCuotas.Items.Add("6");
            cmbCuotas.Items.Add("7");
            cmbCuotas.Items.Add("8");
            cmbCuotas.Items.Add("9");
            cmbCuotas.Items.Add("10");
            cmbCuotas.Items.Add("11");
            cmbCuotas.Items.Add("12");
        }

        private void dgvProductosFactura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            itemsFact.Clear();
            dgvProductosFactura.Items.Refresh();
            try
            {
                String id = cmbProveedores.SelectedValue.ToString();
                String nombreProv = cmbProveedores.Text;

                String sql = "SELECT * FROM ordencompra WHERE FK_idProveedor =  '" + id + "'";
                conexion.Consulta(sql, combo: cmbOrden);
                cmbOrden.DisplayMemberPath = "idOrdenCompra";
                cmbOrden.SelectedValuePath = "idOrdenCompra";
                cmbOrden.SelectedIndex = 0;
            }
            catch (NullReferenceException)
            {

           
           }

        }

        private void cmbOrden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            items.Clear();

         //   try
           // {
                String sql2 = "SELECT productos.nombre, productos.idProductos,productos_has_ordencompra.CrFactura, subtotal, productos_has_ordencompra.PUPagado  FROM productos_has_ordencompra, productos WHERE FK_idOC = @valor AND productos.idProductos = productos_has_ordencompra.FK_idProducto";

                DataTable productos = conexion.ConsultaParametrizada(sql2, cmbOrden.SelectedValue);
                for (int i = 0; i < productos.Rows.Count; i++)
                {
                    producto = new Producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[1], (int)productos.Rows[i].ItemArray[2], (float)productos.Rows[i].ItemArray[3], (float)productos.Rows[i].ItemArray[4]);
                    items.Add(producto);

                }

                dgvProductosOC.Items.Refresh();
                
            }
           // catch (NullReferenceException)
            //{


          //  }

        

        private void LoadDgvProducto(List<Producto> pOC)
        {
            dgvProductosOC.ItemsSource = pOC;
        }
        private void LoadDgvProducto()
        {
            dgvProductosOC.ItemsSource = items;
        }
        private void LoadDgvFactura()
        {
            dgvProductosFactura.ItemsSource = itemsFact;
            dgvProductosFactura.SelectedIndex = 0;
        }

        private void LoadDgvFactura(List<Producto> listproductosfactura)
        {
            dgvProductosFactura.ItemsSource = listproductosfactura;
        }

        private void btnProdAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool existe = false;
                Producto prod = dgvProductosOC.SelectedItem as Producto;
               
                id = prod.id;
                if (prod.cantidad > 0)
                {
                    var newW = new WindowAgregarProductoFactura();
                    for (int i = 0; i < itemsFact.Count; i++)
                    {
                        if (itemsFact[i].nombre == prod.nombre)
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
                        if (int.Parse(newW.txtCantidad.Text) > 0 )
                        {
                            Producto productoFactura = new Producto(prod.nombre, prod.id, int.Parse(newW.txtCantidad.Text), prod.total, prod.precioUnitario);
                            itemsFact.Add(productoFactura);
                            dgvProductosFactura.Items.Refresh();
                            float.TryParse(txtSubtotal.Text, out subtotal);
                            subtotal += productoFactura.total;
                            txtSubtotal.Text = (productoFactura.cantidad * productoFactura.precioUnitario).ToString();                          
                            prod.cantidad = prod.cantidad - int.Parse(newW.txtCantidad.Text);
                            dgvProductosOC.Items.Refresh();
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
                    MessageBox.Show("Ya se entregaron todos las facturas de este producto");
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un producto para agregar");

            }
            /* try
             {
                 prod = dgvProductosOC.SelectedItem as Producto;

                 var newW = new WindowAgregarProductoFactura();
                 if (prod.cantidad >= 1)
                 {

                     newW.txtCantidad.Text = prod.cantidad.ToString();
                     newW.can = prod.cantidad;
                     newW.ShowDialog();
                 }
                 else
                 {
                     MessageBox.Show("El producto ya fue facturado");
                 }


                 if (newW.DialogResult == true)
                 {

                     int cantidadactual = 0;
                     cantidadactual = int.Parse(newW.canpararestar);

                     prod.cantidad = prod.cantidad- int.Parse(newW.txtCantidad.Text);
                     Producto productoAfacturar = new Producto(prod.nombre, prod.id,cantidadactual, prod.total, prod.precioUnitario);
                     itemsFact.Add(productoAfacturar);

                     dgvProductosFactura.Items.Refresh();
                     dgvProductosOC.Items.Refresh();
                     subtotali += prod.precioUnitario * prod.cantidad;
                     txtSubtotal.Text = subtotali.ToString();
                     calculaTotal();
                 }

             }
             catch (NullReferenceException)
             {

                 MessageBox.Show("Seleccione un producto a agregar");
             }
             */

        }

        private void btnProdEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              
                prod = dgvProductosFactura.SelectedItem as Producto;
                int cantidadAsumar=0;  

                cantidadAsumar = prod.cantidad;

                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].nombre == prod.nombre)
                    {
                        items[i].cantidad += prod.cantidad;
                        /*
                        String updateCantidad = "UPDATE productos_has_ordencompra SET CrFactura = '" + items[i].cantidad + "' WHERE FK_idOC = '" + cmbOrden.Text + "' AND FK_idProducto = '"+items[i].id+"'";
                        conexion.operaciones(updateCantidad);*/
                    }

                }
                subtotali = subtotali - prod.cantidad * prod.precioUnitario;
                txtSubtotal.Text = subtotali.ToString();
                calculaTotal();
                dgvProductosOC.Items.Refresh();
                itemsFact.Remove(prod);
                dgvProductosFactura.Items.Refresh();
                if (dgvProductosFactura.HasItems == false)
                {
                    txtSubtotal.Text = "0";
                    txtTotal.Text = "0";
                    subtotali = 0;
                }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un producto");
            }

        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            bandera = false;
            if (Valida())
            {
                DialogResult = true;
            }
        }

        public void calculaTotal()
        {

            if (cmbIVA.SelectedIndex == 0)
            {
        
                txtTotal.Text = txtSubtotal.Text.ToString();
            }
            else if (cmbIVA.SelectedIndex == 1)
            {
                total = float.Parse(txtSubtotal.Text) * (float)1.21;
                txtTotal.Text = total.ToString();
            }
            else if (cmbIVA.SelectedIndex == 2)
            {
                total = float.Parse(txtSubtotal.Text) * (float)1.105;
                txtTotal.Text = total.ToString();
            }
        }

        private void cmbTipoCambio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbIVA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calculaTotal();
        }

       

        private void dgvProductosFactura_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           
        }

        private void dgvProductosFactura_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void cmbCuotas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
            int cuotass = cmbCuotas.SelectedIndex+1;
      
            if (bandera == true)
            {

                todaslascuotas.Clear();
        
                var newW = new windowCuotas(cuotass, dt);
           
                newW.ShowDialog();
                if (newW.DialogResult == true)
                {
                    todaslascuotas.Clear();
                    foreach (Cuotas cuot in newW.listacuotas)
                    {
                 
                        int id = cuot.cuota;
                        int dias = cuot.dias;
                        DateTime fecha = cuot.fechadepago;
              
                        Cuotas cu = new Cuotas(id, dias, fecha);
                        todaslascuotas.Add(cu);

                    }
                    loadDGVCuotas();
                    bandera = false;
                }
            }
            
        }

        public void loadDGVCuotas()
        {

            DgvCuotas.ItemsSource = todaslascuotas;
            DgvCuotas.Items.Refresh();
        }

        public void loadDGVCuotas(List<Cuotas>l)
        {

            DgvCuotas.ItemsSource = l;
            DgvCuotas.Items.Refresh();
        }
        public Boolean Valida()
        {

            String nombreDB = "SELECT COUNT(*) FROM factura WHERE numeroFactura  = '" + txtNroFactura.Text + "'";
            String nomCat = conexion.ValorEnVariable(nombreDB).ToString();
            if (dtFactura.SelectedDate == null )
            {
                MessageBox.Show("Ingrese fecha de la factura");
                return false;
            }
           
            else if (dgvProductosFactura.HasItems == false)
            {
                MessageBox.Show("Es necesario ingresar productos a la factura");
                return false;
            } else if (cmbCuotas.Text == "") {

                MessageBox.Show("Selecciona cantidad de cuotas");
                return false;
            }
            else if (cmbIVA.Text == "")
            {

                MessageBox.Show("Seleccione IVA");
                return false;
            }
            else if (cmbTipoCambio.Text == "")
            {

                MessageBox.Show("Seleccione tipo de cambio");
                return false;
            }
            else if (cmbProveedores.Text == "")
            {

                MessageBox.Show("Seleccione un proveedor");
                return false;
            }
            else if (bandera == true && nomCat !="0")
            {
               
                    MessageBox.Show("El numero de factura ya existe");
                    return false;
                
            

            }
         
            else if (txtNroFactura.Text == "")
            {
                MessageBox.Show("Ingrese numero de factura");
                return false;

            }
            else
            {
                return true;
            }

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtNroFactura_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Busquedas de productos.
            DataTable facturas = new DataTable();
            String consulta;
            consulta = "SELECT DISTINCT p.nombre, p.idProveedor FROM proveedor p INNER JOIN ordencompra o ON p.idProveedor = o.FK_idProveedor AND p.nombre LIKE '%' @valor '%' ";
            facturas = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            cmbProveedores.ItemsSource = facturas.AsDataView();
            cmbProveedores.SelectedIndex = 0;
        }

        private void dtFactura_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
       
            
                    todaslascuotas.Clear();
                    DgvCuotas.Items.Refresh();
                cmbCuotas.Text = "Seleccione cantidad de cuotas";
            bandera = true;
        }
    }

}
