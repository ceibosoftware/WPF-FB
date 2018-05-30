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
    /// Interaction logic for Remito.xaml
    /// </summary>
    public partial class Remito : Page
    {
        List<Producto> productosparametro = new List<Producto>();
        //para guardar valores en el modificar y asi manejar el stock
        List<Producto> productosStock = new List<Producto>();
        DataTable productos;
        bool ejecutar = true;
        DateTime fecha;
        CRUD conexion = new CRUD();
        public Remito()
        {
            InitializeComponent();
            loadLtsRemitos();
            LoadListaComboProveedor();
            loadProductosRemitos();
            seleccioneParaFiltrar();
            dgvProductos.IsReadOnly = true;
            
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarRemito();
            newW.ShowDialog();
        }
        private void loadProductosRemitos()
        {
            dgvProductos.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvProductos.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Cantidad";
            textColumn2.Binding = new Binding("cantidad");
            dgvProductos.Columns.Add(textColumn2);
            dgvProductos.ItemsSource = productosparametro;
        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarRemito();
            newW.ShowDialog();
            if (newW.DialogResult == true)
            {
                //DATOS REMITO.
                int numeroRemito = int.Parse(newW.txtNroRemito.Text);
                DateTime fecha = newW.dtRemito.SelectedDate.Value.Date;

                int idOC = (int)newW.cmbOrden.SelectedValue;
                string consulta = "insert into remito( numeroRemito, fecha, FK_idOC) values( '" + numeroRemito + "', '" + fecha.ToString("yyyy/MM/dd") + "','" + idOC + "')";
                conexion.operaciones(consulta);

                //PRODUCTOS REMITO
                string ultimoId = "Select last_insert_id()";

                String id = conexion.ValorEnVariable(ultimoId);
                foreach (var producto in newW.ProdRemito)
                {
                    String productos = "insert into productos_has_remitos(cantidad, CrNotaCredito,  FK_idProducto, FK_idRemito) values( '" + producto.cantidad + "', '"+producto.cantidad+ "','" + producto.id + "','" + id + "' )";
                    conexion.operaciones(productos);
                }
                //ACTUALIZAR CANTITAD RESTANTE REMITO DE PRODUCTO OC
                int idOrden = (int)newW.cmbOrden.SelectedValue;
                foreach (var producto in newW.Productos)
                {
                    String sql = "UPDATE productos_has_ordencompra SET CrRemito = '" + producto.cantidad + "' where FK_idProducto = '" + producto.id + "' and FK_idOC = '" + idOrden + "'";
                    conexion.operaciones(sql);
                }
                //CARGAR STOCK EN PRODUCTO
                foreach (var producto in newW.ProdRemito)
                {
                    Console.WriteLine("id " + producto.id);
                    Console.WriteLine("id " + producto.cantidad);
                    String sql = "UPDATE productos SET stock = stock+'" + producto.cantidad + "', ume ='"+DateTime.Now.ToString("yyyy/MM/dd") + "' where idProductos = '" + producto.id + "' ";
                    conexion.operaciones(sql);
                }
                LoadListaComboProveedor();
                loadLtsRemitos();
            
                seleccioneParaFiltrar();
                ltsremitos.Items.MoveCurrentToLast();
                MessageBox.Show("Se agrego el remito correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        public void LoadListaComboProveedor()
        {
            String consulta = "SELECT DISTINCT p.nombre, p.idProveedor FROM proveedor p ,ordencompra o, remito r where o.FK_idProveedor = p.idProveedor and r.FK_idOC = o.idOrdenCompra";
            conexion.Consulta(consulta, combo: cmbProveedores);
            ejecutar = false;
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
            ejecutar = true;
            cmbProveedores.SelectedIndex = -1;
        }

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbProveedores.Text != "selccione para filtrar")
            {
                String consulta = " Select * from ordencompra t1 where t1.FK_idProveedor = @valor ";
                DataTable OCProveedor = conexion.ConsultaParametrizada(consulta, cmbProveedores.SelectedValue);
                cmbOrdenes.ItemsSource = OCProveedor.AsDataView();
                cmbOrdenes.DisplayMemberPath = "idOrdenCompra";
                cmbOrdenes.SelectedValuePath = "idOrdenCompra";
                cmbOrdenes.SelectedIndex = -1;

                String sql = "   Select distinct DATE_FORMAT(t1.fecha, '%d-%m-%Y') AS fecha from ordencompra t1 inner join remito t2 where t1.FK_idProveedor = @valor and t2.FK_idOC = t1.idOrdenCompra";
                DataTable fechas = conexion.ConsultaParametrizada(sql, cmbProveedores.SelectedValue);
                cmbFechas.ItemsSource = fechas.AsDataView();
                cmbFechas.DisplayMemberPath = "fecha";
                cmbFechas.SelectedValuePath = "fecha";
                cmbFechas.SelectedIndex = -1;

                String consulta2 = "Select distinct idOrdenCompra from ordencompra t1 inner join remito t2 where FK_idProveedor = @valor and t2.FK_idOC = t1.idOrdenCompra  ";

                DataTable OCFecha = conexion.ConsultaParametrizada(consulta2, cmbProveedores.SelectedValue);
                cmbOrdenes.ItemsSource = OCFecha.AsDataView();
                cmbOrdenes.DisplayMemberPath = "idOrdenCompra";
                cmbOrdenes.SelectedValuePath = "idOrdenCompra";
                cmbOrdenes.SelectedIndex = -1;

                if (ejecutar)
                {
                    loadLtsRemitosProv();
                }
                else
                {
                    loadLtsRemitos();
                }
            }
          
         

        }
         

        

        private void cmbFechas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

           loadLtsRemitosFecha();
          
        }

        public void loadLtsRemitos()
        {
            String consulta = "select * from remito";
            conexion.Consulta(consulta, tabla: ltsremitos);
            ltsremitos.DisplayMemberPath = "numeroRemito";
            ltsremitos.SelectedValuePath = "idremitos";
            //ltsremitos.SelectedIndex = 0;
        }
        public void loadLtsRemitos(int index)
        {
            seleccioneParaFiltrar();
            String consulta = "select * from remito";
            conexion.Consulta(consulta, tabla: ltsremitos);
            ltsremitos.DisplayMemberPath = "numeroRemito";
            ltsremitos.SelectedValuePath = "idremitos";
            ltsremitos.SelectedIndex = index;
        }
    
        public void loadLtsRemitosProv()
        {
           
            String consulta = "select idremitos, numeroRemito from remito t1 inner join ordencompra t2 where t1.FK_idOC = t2.idOrdenCompra and t2.FK_idProveedor = @valor";
            ltsremitos.ItemsSource = conexion.ConsultaParametrizada(consulta, cmbProveedores.SelectedValue).AsDataView() ;
            ltsremitos.DisplayMemberPath = "numeroRemito";
            ltsremitos.SelectedValuePath = "idremitos";
            ltsremitos.SelectedIndex = 0;
            
        }
        public void loadLtsRemitosFecha()
        {
            if (cmbFechas.SelectedIndex != -1)
            {
                string consulta = "select idremitos, numeroRemito from ordencompra t1 inner join remito t2 where t1.idOrdenCompra = t2.FK_idOC and t1.FK_idProveedor = '" + cmbProveedores.SelectedValue + "' and t1.fecha = @valor";
                DateTime fecha = new DateTime();
                DateTime.TryParse(cmbFechas.SelectedValue.ToString(), out fecha);
                ltsremitos.ItemsSource = conexion.ConsultaParametrizada(consulta, fecha.ToString("yyyy/MM/dd")).AsDataView();
                ltsremitos.DisplayMemberPath = "numeroRemito";
                ltsremitos.SelectedValuePath = "idremitos";
                ltsremitos.SelectedIndex = 0;
            }
        }
        private void ltsremitos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
               
                //Datos Remito

                String sql = "Select  t3.nombre,t1.fecha ,t2.idOrdenCompra from remito t1 , ordencompra t2, proveedor t3 where idremitos = @valor and t1.FK_idOC = t2.idOrdenCompra and t2.FK_idProveedor = t3.idProveedor";
                DataTable datos = conexion.ConsultaParametrizada(sql, ltsremitos.SelectedValue);
                if (datos.Rows.Count > 0)
                {
                    txtProveedor.Text = datos.Rows[0].ItemArray[0].ToString();
                    DateTime fecha = (DateTime)datos.Rows[0].ItemArray[1];
                    this.fecha = (DateTime)datos.Rows[0].ItemArray[1];
                    txtFecha.Text = fecha.ToString("dd/MM/yyyy");
                    txtOC.Text = datos.Rows[0].ItemArray[2].ToString();
                }
                //consulta productos
                String consulta = "  SELECT t2.nombre , t1.cantidad, t2.idProductos from productos_has_remitos t1 inner join productos t2  on t1.FK_idProducto = t2.idProductos where t1.FK_idRemito = @valor";
                productos = conexion.ConsultaParametrizada(consulta, ltsremitos.SelectedValue);
                productosparametro.Clear();
                for (int i = 0; i < productos.Rows.Count; i++)
                {
                    productosparametro.Add(new Producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[2], (int)productos.Rows[i].ItemArray[1]));
                    
            }
            productosStock = productosparametro;
            dgvProductos.Items.Refresh();


        }

        private void cmbOrdenes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            String consulta = "SELECT distinct idremitos, numeroRemito from remito inner join ordencompra where FK_idOC = @valor ";
            ltsremitos.ItemsSource = conexion.ConsultaParametrizada(consulta, cmbOrdenes.SelectedValue).AsDataView();
            ltsremitos.DisplayMemberPath = "numeroRemito";
            ltsremitos.SelectedValuePath = "idremitos"; 
            ltsremitos.SelectedIndex = -1;
            
        }

        private void btnVertodo_Click(object sender, RoutedEventArgs e)
        {
            /*cmbProveedores.SelectedIndex = -1;
            cmbOrdenes.SelectedIndex = -1;
            cmbFechas.SelectedIndex = -1;*/
            seleccioneParaFiltrar();
            loadLtsRemitos();
           
        }
        private void seleccioneParaFiltrar()
        {
            cmbProveedores.Text = "Seleccione para filtrar";
            cmbOrdenes.Text = "Seleccione para filtrar";
            cmbFechas.Text = "Seleccione para filtrar";
        }
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            string consulta1 = "SELECT count(FK_idremitos) FROM notacredito where FK_idremitos = "+ltsremitos.SelectedValue+"";
              
            if(conexion.ValorEnVariable(consulta1) == "0") { 
            DataRow selectedDataRow = ((DataRowView)ltsremitos.SelectedItem).Row;
            string numeroRemito = selectedDataRow["numeroRemito"].ToString();
            MessageBoxResult dialog = MessageBox.Show("Esta seguro que desea eliminar el remito número " + numeroRemito, "Advertencia", MessageBoxButton.YesNo,MessageBoxImage.Warning);
            if (dialog == MessageBoxResult.Yes)
            {
                int idSeleccionado = (int)ltsremitos.SelectedValue;
                for (int i = 0; i < productos.Rows.Count; i++)
                {
                    String consulta = "UPDATE productos_has_ordencompra SET CrRemito = CrRemito + '" + (int)productos.Rows[i].ItemArray[1] + "' where FK_idProducto = '" + productos.Rows[i].ItemArray[2] + "' and FK_idOC = '" + txtOC.Text.ToString() + "'";
                    conexion.operaciones(consulta);
                        
                        //Console.Write();
                        String sql2 = "UPDATE productos SET stock = stock-'" + (int)productos.Rows[i].ItemArray[1] + "' where idProductos = '" + productos.Rows[i].ItemArray[2] + "' ";
                        conexion.operaciones(sql2);
                    }
                string sql = "delete from remito where idremitos = '" + idSeleccionado + "'";
                conexion.operaciones(sql);
                        MessageBox.Show("Se eliminó correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        if (ltsremitos.Items.Count <= 0)
                    {
                        txtProveedor.Text = "";
                        txtOC.Text = "";
                        txtFecha.Text = "";
                    }
                    seleccioneParaFiltrar();
                    loadLtsRemitos();
                    LoadListaComboProveedor();
                    
                    }
                }
                else
                {
                    MessageBox.Show("No se puede eliminar el remito porque tiene notas de crédito", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un remito a eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnModificar_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string consulta1 = "SELECT count(FK_idremitos) FROM notacredito where FK_idremitos = " + ltsremitos.SelectedValue + "";
                
                if (conexion.ValorEnVariable(consulta1) == "0")
                {

                    string numeroR = (((DataRowView)ltsremitos.SelectedItem).Row[1]).ToString();
                        string consulta = "select t2.FK_idProveedor from  ordencompra t2 where idOrdenCompra = @valor";
                        int.TryParse(txtOC.Text.ToString(), out int OC);
                        DataTable idprov = conexion.ConsultaParametrizada(consulta, OC);
                        int index = ltsremitos.SelectedIndex;
                        var newW = new windowAgregarRemito((int)idprov.Rows[0].ItemArray[0], OC, productosparametro, fecha, numeroR, (int)ltsremitos.SelectedValue);
                        Console.WriteLine(OC);

                        newW.ShowDialog();

                    if (newW.DialogResult == true)
                    {
                        //DATOS REMITO.

                        int numeroRemito = int.Parse(newW.txtNroRemito.Text);
                        DateTime fecha = newW.dtRemito.SelectedDate.Value.Date;
                        int idOC = (int)newW.cmbOrden.SelectedValue;
                        int idRemito = newW.idRemito;

                        string consultasql = "UPDATE  remito SET numeroRemito = '" + numeroRemito + "', fecha ='" + fecha.ToString("yyyy/MM/dd") + "', FK_idOC ='" + idOC + "' where idremitos ='" + idRemito + "' ";
                        conexion.operaciones(consultasql);

                        //ELIMINAR PRODUCTOS

                        String sqlElim = "delete from productos_has_remitos where FK_idRemito = '" + idRemito + "'";
                        conexion.operaciones(sqlElim);
                        //PRODUCTOS REMITO  

                        foreach (var producto in newW.ProdRemito)
                        {
                            Console.Write("stock nuevo :" + producto.cantidad);
                            String productos = "insert into productos_has_remitos(cantidad,  FK_idProducto, FK_idRemito,CrNotaCredito) values( '" + producto.cantidad + "', '" + producto.id + "','" + idRemito + "','" + producto.cantidad + "' )";
                            conexion.operaciones(productos);
                        }
                        //ACTUALIZAR CANTITAD RESTANTE REMITO DE PRODUCTO OC
                        int idOrden = (int)newW.cmbOrden.SelectedValue;
                        foreach (var producto in newW.Productos)
                        {
                            String sql = "UPDATE productos_has_ordencompra SET CrRemito = '" + producto.cantidad + "' where FK_idProducto = '" + producto.id + "' and FK_idOC = '" + idOrden + "'";
                            conexion.operaciones(sql);
                        }
                        //RESTO CANTIDAD STOCK VIEJA
                        foreach (var producto in newW.ProdRemitoStock)
                        {
                            //MessageBox.Show("stock viejo :" + producto.cantidad);

                            String sql = "UPDATE productos SET stock = stock-'" + producto.cantidad + "' where idProductos = '" + producto.id + "' ";
                            conexion.operaciones(sql);
                        }
                        //SUMO CANTIDAD NUEVA STOCK EN PRODUCTO
                        foreach (var producto in newW.ProdRemito)
                        {
                            //MessageBox.Show("stock nuevo :" + producto.cantidad);
                            String sql = "UPDATE productos SET stock = stock+'" + producto.cantidad + "' where idProductos = '" + producto.id + "' ";
                            conexion.operaciones(sql);
                        }

                        loadLtsRemitos(index);
                    }
                    }
                    else
                    {
                    MessageBox.Show("El remito no se puede modificar porque tiene notas de crédito", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un remito a modificar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
            
        }

      
    }
