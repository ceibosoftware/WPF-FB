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
    /// Interaction logic for windowAgregarRemito.xaml
    /// </summary>
    public partial class windowAgregarRemito : Window
    {
       public int idRemito;
        bool ejecuta = true;
        private List<Producto> prodRemito = new List<Producto>();
        private List<Producto> productos = new List<Producto>();
        CRUD conexion = new CRUD();

        public List<Producto> ProdRemito { get => prodRemito; set => prodRemito = value; }
        public List<Producto> Productos { get => productos; set => productos = value; }

        public windowAgregarRemito()
        {
            InitializeComponent();      
            loadcmbProveedores();
            loadDgvProd();
            loadDgvProdRemito();
            dtRemito.SelectedDate = DateTime.Now;
        }
        public windowAgregarRemito(int proveedor, int numeroOC, List<Producto> productosRemito,DateTime fecha, string numeroRemito, int idRemito)
        {
            InitializeComponent();

            loadCmbOrdenes(numeroOC);
            loadDgvProd();
            prodRemito = productosRemito;
            loadDgvProdRemito(prodRemito);
            loadProductosOC(numeroOC);
            loadFechaEmision();
            dtRemito.SelectedDate =fecha ;
            txtNroRemito.Text = numeroRemito;
            this.idRemito = idRemito;      
            cmbProveedores.IsEnabled = false;
            cmbOrden.IsEnabled = false;
            txtFiltro.IsEnabled = false;
            cmbFechas.IsEnabled = false;
            ejecuta = false;
            loadcmbProveedores();
            ejecuta = true;
        }
        /*public void fechas()
        {
            String consulta = "Select distinct DATE_FORMAT(fecha, '%d-%m-%Y') AS fecha from ordencompra";
            conexion.Consulta(consulta, combo: cmbFechas);
            cmbFechas.DisplayMemberPath = "fecha";
            cmbFechas.SelectedValue = "fecha";
            cmbFechas.SelectedIndex = 0;
        }*/
        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ejecuta)
            {
                prodRemito.Clear();
                dgvProductosRemito.Items.Refresh();
                try
                {
                  
                    ejecuta = false;
                      loadCmbOrdenes();
                    loadFechaEmision();
                    ejecuta = true;
                }
                catch (Exception)
                {


                }
            }
        }
        public void loadDgvProd() {
            dgvProductosOC.ItemsSource = productos;
        }
        public void loadDgvProdRemito()
        {
            dgvProductosRemito.ItemsSource = prodRemito;
        }
        public void loadDgvProdRemito(List<Producto> prodRemito)
        {
            dgvProductosRemito.ItemsSource = prodRemito;
        }

        public void loadcmbProveedores()
        {

            String consulta = "SELECT DISTINCT p.nombre, p.idProveedor FROM proveedor p inner join ordencompra o where o.FK_idProveedor = p.idProveedor ";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
            cmbProveedores.SelectedIndex = 0;
            

        }

        public void loadcmbProveedores(int proveedor)
        {
            ejecuta = false;
            String consulta = "SELECT DISTINCT p.nombre, p.idProveedor FROM proveedor p inner join ordencompra o where o.FK_idProveedor = p.idProveedor ";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
            cmbProveedores.SelectedValue= proveedor;

            ejecuta = true;
        }

        private void cmbFechas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ejecuta)
            {
                String consulta = " Select * from ordencompra t1 where t1.fecha = @valor and FK_idProveedor = '" + cmbProveedores.SelectedValue + "'";
                DateTime fecha = new DateTime();
                DateTime.TryParse(cmbFechas.SelectedValue.ToString(), out fecha);
                fecha.ToString("yyyy-MM-dd");
                DataTable OCFecha = conexion.ConsultaParametrizada(consulta, fecha);
                cmbOrden.ItemsSource = OCFecha.AsDataView();
                cmbOrden.DisplayMemberPath = "idOrdenCompra";
                cmbOrden.SelectedValuePath = "idOrdenCompra";
                cmbOrden.SelectedIndex = 0;
            }
        }

        private void cmbOrden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadProductosOC();

        }

        private void btnProdAgregar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                bool existe = false;
                Producto prod = dgvProductosOC.SelectedItem as Producto;
                if (prod.cantidad > 0)
                {
                    var newW = new WindowAgregarProductoFactura();
                    for (int i = 0; i < prodRemito.Count; i++)
                    {
                        if (prodRemito[i].nombre == prod.nombre)
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
                        if (int.Parse(newW.txtCantidad.Text) > 0)
                        {
                            Producto productoremito = new Producto(prod.nombre, prod.id, int.Parse(newW.txtCantidad.Text), prod.total, prod.precioUnitario);
                            prodRemito.Add(productoremito);
                            dgvProductosRemito.Items.Refresh();
                            prod.cantidad = prod.cantidad - int.Parse(newW.txtCantidad.Text);
                            dgvProductosOC.Items.Refresh();
                        }
                        else
                        {
                            MessageBox.Show("La cantidad no puede ser cero");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ya se entregaron todos los remitos de este producto");
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un producto para agregar");
                
            }
          
        }

        private void btnProdEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Producto prod = dgvProductosRemito.SelectedItem as Producto;
                for (int i = 0; i < productos.Count; i++)
                {
                    if (productos[i].nombre == prod.nombre)
                    {
                        productos[i].cantidad += prod.cantidad;
                    }
                    
                }
                prodRemito.Remove(prod);
                dgvProductosRemito.Items.Refresh();
                dgvProductosOC.Items.Refresh();
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un producto para eliminar");

            }

        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Busquedas de proveedor.
            DataTable productos = new DataTable();
            String consulta;
            consulta = "SELECT  DISTINCT p.nombre, p.idProveedor FROM proveedor p inner join ordencompra o where p.nombre LIKE '%' @valor '%' and o.FK_idProveedor = p.idProveedor ";
            productos = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            cmbProveedores.ItemsSource = productos.AsDataView();
            cmbProveedores.SelectedIndex = 0;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (validar()) {
                DialogResult = true;
            }
        
        }

        public void loadCmbOrdenes()
        {
            String consulta = " Select * from ordencompra t1 where t1.FK_idProveedor = @valor ";
            DataTable OCProveedor = conexion.ConsultaParametrizada(consulta, cmbProveedores.SelectedValue);
            cmbOrden.ItemsSource = OCProveedor.AsDataView();
            cmbOrden.DisplayMemberPath = "idOrdenCompra";
            cmbOrden.SelectedValuePath = "idOrdenCompra";
            cmbOrden.SelectedIndex = -1;
            cmbOrden.SelectedIndex = 0;
        }
        public void loadCmbOrdenes(int orden)
        {
            String consulta = " Select idOrdenCompra from ordencompra ";
            conexion.Consulta(consulta, combo: cmbOrden);
            cmbOrden.DisplayMemberPath = "idOrdenCompra";
            cmbOrden.SelectedValuePath = "idOrdenCompra";
            cmbOrden.SelectedValue = orden;
            /*for (int i = 0; i < cmbOrden.Items.Count; i++)
            {
                cmbOrden.SelectedIndex = i;

                if (cmbOrden.SelectedValue.ToString() == orden.ToString())
                {
                    break;
                }
            }*/
               
        }
         public void loadFechaEmision()
        {
            String sql = "   Select distinct DATE_FORMAT(t1.fecha, '%d-%m-%Y') AS fecha from ordencompra t1 where t1.FK_idProveedor = @valor ";
            DataTable fechas = conexion.ConsultaParametrizada(sql, cmbProveedores.SelectedValue);
            cmbFechas.ItemsSource = fechas.AsDataView();
            cmbFechas.DisplayMemberPath = "fecha";
            cmbFechas.SelectedValuePath = "fecha";
            cmbFechas.SelectedIndex = 0;
        }

        public void loadProductosOC()
        {
            productos.Clear();
            string consulta = "SELECT t2.idProductos, t1.CrRemito ,t1.subtotal,t2.nombre,t1.PUPagado FROM productos_has_ordencompra t1 inner join productos t2 where FK_idOC = @valor and t1.FK_idProducto = t2.idProductos";
            DataTable prod = conexion.ConsultaParametrizada(consulta, cmbOrden.SelectedValue);
            for (int i = 0; i < prod.Rows.Count; i++)
            {
                int idProductos = (int)prod.Rows[i].ItemArray[0];
                int cantidad = (int)prod.Rows[i].ItemArray[1];
                float subtotal = (float)prod.Rows[i].ItemArray[2];
                string nombre = prod.Rows[i].ItemArray[3].ToString();
                float PUpagado = (float)prod.Rows[i].ItemArray[4];
                productos.Add(new Producto(nombre, idProductos, cantidad, subtotal, PUpagado));
            }
            dgvProductosOC.Items.Refresh();
        }
        public void loadProductosOC(int Orden)
        {
            productos.Clear();
            string consulta = "SELECT t2.idProductos, t1.CrRemito ,t1.subtotal,t2.nombre,t1.PUPagado FROM productos_has_ordencompra t1 inner join productos t2 where FK_idOC = @valor and t1.FK_idProducto = t2.idProductos";
            DataTable prod = conexion.ConsultaParametrizada(consulta, Orden);
            for (int i = 0; i < prod.Rows.Count; i++)
            {
                int idProductos = (int)prod.Rows[i].ItemArray[0];
                int cantidad = (int)prod.Rows[i].ItemArray[1];
                float subtotal = (float)prod.Rows[i].ItemArray[2];
                string nombre = prod.Rows[i].ItemArray[3].ToString();
                float PUpagado = (float)prod.Rows[i].ItemArray[4];
                productos.Add(new Producto(nombre, idProductos, cantidad, subtotal, PUpagado));
            }
            dgvProductosOC.Items.Refresh();
        }
        public bool validar()
        {

            if (prodRemito.Count <= 0)
            {
                MessageBox.Show("Ingrese al menos un producto");
                return false;
            }
            else if (string.IsNullOrEmpty(dtRemito.Text))
            {
                MessageBox.Show("Seleccione una fecha");
                return false;
            }
            else if (string.IsNullOrEmpty(txtNroRemito.Text))
            {
                MessageBox.Show("Ingrese numero remito");
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

        private void txtNroRemito_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }

  

}
