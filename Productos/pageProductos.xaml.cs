using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for pageProductos.xaml
    /// </summary>
    public partial class pageProductos : Page
    {
     
        CRUD conexion = new CRUD();
        public pageProductos()
        {

            InitializeComponent();
            loadListaProducto();
            LlenarComboFiltro();
            cmbFiltro.IsEnabled = false;
            LlenarCmbTipoCambio();
            if (windowUsuarios.tipoUsuarioDB == "basico")
            {
                this.btnModificar.Visibility = Visibility.Collapsed;
                this.btnEliminar.Visibility = Visibility.Collapsed;

            }

            ltsProductos.SelectedIndex = 0;
            chkVenta.IsEnabled = false;
        }
        private void LlenarCmbTipoCambio()
        {
            cmbMoneda.Items.Add("$");
            cmbMoneda.Items.Add("u$d");
            cmbMoneda.Items.Add("€");
        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarProducto();

            newW.ShowDialog();
            if (newW.Aceptar && newW.validar())
            {
                //INSERTAR DATOS EN TABLA PRODUCTOS
                String nombre = newW.txtNombre.Text;
                String descripcion = newW.txtDescripcion.Text;
                String unidad = newW.txtUnidad.Text;
                String existencias = newW.txtExistenciaMinima.Text;
                String precioUnitario = newW.txtPrecioUnitario.Text;
                String costo = newW.txtCosto.Text;
                int moneda = newW.cmbMoneda.SelectedIndex;
                int venta ;
                if (newW.venta)
                {
                    venta = 1;
                }
                else
                {
                    venta = 0;
                }
           
                int idCategoria = (int)newW.cmbCategoria.SelectedValue;
                String sql = "insert into productos(nombre, descripcion, FK_idCategorias, existenciaMinima, unidad, precioUnitario,venta, moneda, costo) values('" + nombre + "', '" + descripcion + "', '" + idCategoria + "', '" + existencias + "', '" + unidad + "','" + precioUnitario + "','" + venta + "','" + moneda + "','" + costo + "');";
                conexion.operaciones(sql);

                //INSERTAR PROVEEDORES DE PRODUCTO CARGADO                
                string ultimoId = "Select last_insert_id()";
                String id = conexion.ValorEnVariable(ultimoId);
                for (int i = 0; i < newW.Items.Count; i++)
                {
                    int idProveedor = newW.Items[i].id;
                    string sql2 = "INSERT INTO productos_has_proveedor(FK_idProductos, FK_idProveedor) VALUES('" + id + "','" + idProveedor + "' )";
                    conexion.operaciones(sql2);
                }
                loadListaProducto();
            }
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<elemento> items = new List<elemento>();
                int idProducto = (int)ltsProductos.SelectedValue;
                int index = (int)ltsProductos.SelectedIndex;
                // LLENAR DATOS PRODUCTOS.
                String consulta = "SELECT productos.nombre, productos.idProductos, productos.descripcion, categorias.nombre, categorias.idCategorias, productos.FK_idCategorias, productos.existenciaMinima, productos.unidad , productos.precioUnitario, productos.venta, productos.moneda, productos.costo FROM productos , categorias WHERE idProductos = @valor AND productos.FK_idCategorias = categorias.idCategorias"; 
                DataTable productos = conexion.ConsultaParametrizada(consulta, ltsProductos.SelectedValue);

                // LLENAR DATOS PROVEEDORES.
                String consultaProveedores = "SELECT proveedor.nombre, proveedor.idProveedor from proveedor , productos_has_proveedor WHERE productos_has_proveedor.FK_idProductos = @valor  AND productos_has_proveedor.FK_idProveedor = proveedor.idProveedor";
                DataTable proveedores = conexion.ConsultaParametrizada(consultaProveedores, ltsProductos.SelectedValue);
                for (int i = 0; i < proveedores.Rows.Count; i++)
                {
                    elemento elemento = new elemento(proveedores.Rows[i].ItemArray[0].ToString(), (int)proveedores.Rows[i].ItemArray[1]);
                    items.Add(elemento);
                }
                //CONSTRUCTOR PAGINA MODIFICAR 
                var newW = new windowModificarProducto((int)productos.Rows[0].ItemArray[4], productos.Rows[0].ItemArray[0].ToString(), productos.Rows[0].ItemArray[2].ToString(), items, (float)productos.Rows[0].ItemArray[6], productos.Rows[0].ItemArray[7].ToString(), (float)productos.Rows[0].ItemArray[8], (bool)productos.Rows[0].ItemArray[9], (int)productos.Rows[0].ItemArray[10], (float)productos.Rows[0].ItemArray[11]);

                newW.ShowDialog();
                if (newW.Aceptar)
                {
                    //ACTUALIZAR DATOS EN TABLA PRODUCTOS
                    String nombre = newW.txtNombre.Text;
                    String descripcion = newW.txtDescripcion.Text;
                    String unidad = newW.txtUnidad.Text;
                    String existencias = newW.txtExistenciaMinima.Text;
                    String precioUnitario = newW.txtPrecioUnitario.Text;
                    String costo = newW.txtCosto.Text;
                    int moneda = newW.cmbMoneda1.SelectedIndex;

                    int venta;
                    if (newW.venta)
                    {
                        venta = 1;
                    }
                    else
                    {
                        venta = 0;
                    }
                    int idCategoria = (int)newW.cmbCategoria.SelectedValue;
                    String sql = "UPDATE productos SET nombre = '" + nombre + "', descripcion = '" + descripcion + "' ,FK_idCategorias = '" + idCategoria + "',precioUnitario = '" + precioUnitario + "',unidad = '" + unidad + "',existenciaMinima = '" + existencias + "',venta = '" + venta + "', costo = '"+costo+"', moneda = '"+ moneda +"' WHERE productos.idProductos = '" + idProducto + "';";
                    conexion.operaciones(sql);
                    //ELIMINA REGISTRO DE TABLA INTERMEDIA
                    string sql2 = "delete  from productos_has_proveedor where FK_idProductos =  '" + idProducto + "'";
                    conexion.operaciones(sql2);
                    //INSERTO LOS NUEVOS DATOS DE LA TABLA INTERMEDIA                             
                    for (int i = 0; i < newW.Items.Count; i++)
                    {
                        int idProveedor = newW.Items[i].id;
                        string sql3 = "INSERT INTO productos_has_proveedor(FK_idProductos, FK_idProveedor) VALUES('" + idProducto + "','" + idProveedor + "' )";
                        conexion.operaciones(sql3);
                    }

                }              
                loadListaProducto(index);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un producto a modificar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public  void loadListaProducto()
        {
            String consulta = " Select * from productos ";
            conexion.Consulta(consulta, ltsProductos);
            ltsProductos.DisplayMemberPath = "nombre";
            ltsProductos.SelectedValuePath = "idProductos";
          
        }
        private void loadListaProducto(int index)
        {
            String consulta = " Select * from productos ";
            conexion.Consulta(consulta, ltsProductos);
            ltsProductos.DisplayMemberPath = "nombre";
            ltsProductos.SelectedValuePath = "idProductos";
            ltsProductos.SelectedIndex = index;
        }


        private void ltsProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //consulta categoria, descripcion
                String consulta = "SELECT productos.nombre, productos.idProductos, productos.descripcion, categorias.nombre, categorias.idCategorias, productos.FK_idCategorias, productos.existenciaMinima, productos.unidad , productos.precioUnitario, venta, stock, productos.moneda FROM productos , categorias WHERE idProductos = @valor AND productos.FK_idCategorias = categorias.idCategorias";
                DataTable productos = conexion.ConsultaParametrizada(consulta, ltsProductos.SelectedValue);
                txtDescripcion.Text = productos.Rows[0].ItemArray[2].ToString();
                txtCategoria.Text = productos.Rows[0].ItemArray[3].ToString();
                txtExistenciaMinima.Text = productos.Rows[0].ItemArray[6].ToString();
                txtUnidad.Text = productos.Rows[0].ItemArray[7].ToString();
                txtPrecioUnitario.Text = productos.Rows[0].ItemArray[8].ToString();
                chkVenta.IsChecked = (bool)productos.Rows[0].ItemArray[9];
                txtStock.Text = productos.Rows[0].ItemArray[10].ToString();
                cmbMoneda.SelectedIndex = (int)productos.Rows[0].ItemArray[11];
                //consulta proveedores
                String consultaProveedores = "SELECT proveedor.nombre, proveedor.idProveedor from proveedor , productos_has_proveedor WHERE productos_has_proveedor.FK_idProductos = @valor  AND productos_has_proveedor.FK_idProveedor = proveedor.idProveedor";
                DataTable proveedores = conexion.ConsultaParametrizada(consultaProveedores, ltsProductos.SelectedValue);
                ltsProveedores.ItemsSource = proveedores.AsDataView();
                ltsProveedores.DisplayMemberPath = "nombre";
            }
            catch
            {

            }

        }

        private void txtFiltro_GotMouseCapture(object sender, MouseEventArgs e)
        {
            txtFiltro.Text = "";
        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Busquedas de productos.
            DataTable productos = new DataTable();
            String consulta;

            if (cmbFiltro.Text == "Nombre")
            {   //Busca por nombre
                consulta = "SELECT* FROM productos WHERE productos.nombre LIKE '%' @valor '%'";
                productos = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            }
            else if (cmbFiltro.Text == "Categoria")
            {
                //busca por nombre de categoria (posibilidad de agregar combobox)
                consulta = "SELECT productos.nombre ,categorias.idCategorias FROM categorias , productos WHERE categorias.nombre LIKE  '%' @valor '%' and categorias.idCategorias = productos.FK_idCategorias";
                productos = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            }

            ltsProductos.ItemsSource = productos.AsDataView();

        }

        private void LlenarComboFiltro()
        {

            cmbFiltro.Items.Add("Nombre");
            cmbFiltro.Items.Add("Categoria");
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                String consulta = "SELECT count(FK_idProducto) FROM productos_has_ordencompra WHERE FK_idProducto ='"+ltsProductos.SelectedValue+"' ";
                String consulta1 = "SELECT count(FK_idProductos) FROM productos_has_listadeprecios WHERE FK_idProductos ='" + ltsProductos.SelectedValue + "' ";
                if (conexion.ValorEnVariable(consulta) != "0")
                {
                    MessageBox.Show("El producto no se puede eliminar porque pertenece a una orden de compra", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                   
                }
                else if (conexion.ValorEnVariable(consulta1) != "0")
                {
                    MessageBox.Show("El producto no se puede eliminar porque tiene una lista de precio asignada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                  
                }
                else
                {
                    DataRow selectedDataRow = ((DataRowView)ltsProductos.SelectedItem).Row;
                    string nombre = selectedDataRow["nombre"].ToString();
                    MessageBoxResult dialog = MessageBox.Show("¿Está seguro que desea eliminar el producto " + nombre+"?", "Advertencia", MessageBoxButton.YesNo,MessageBoxImage.Warning);
                    if (dialog == MessageBoxResult.Yes)
                    {
                        int idSeleccionado = (int)ltsProductos.SelectedValue;
                        string sql = "delete from productos where idProductos = '" + idSeleccionado + "'";
                        conexion.operaciones(sql);
                        loadListaProducto();

                    }
                   
                }
             
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Es necesario seleccionar un producto a eliminar", "Advertencia", MessageBoxButton.OK,MessageBoxImage.Warning);

            }

        }
        public int Darvalor(int valor)
        {
            return valor;
        }
    }
}

