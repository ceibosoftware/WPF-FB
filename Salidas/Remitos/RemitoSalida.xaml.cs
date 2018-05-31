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

namespace wpfFamiliaBlanco.Salidas.Remitos
{
    /// <summary>
    /// Lógica de interacción para RemitoSalida.xaml
    /// </summary>
    public partial class RemitoSalida : Page
    {
        List<Producto> productosparametro = new List<Producto>();
        //para guardar valores en el modificar y asi manejar el stock
        List<Producto> productosStock = new List<Producto>();
        DataTable productos;
        bool ejecutar = true;
        DateTime fecha;
        CRUD conexion = new CRUD();
        public RemitoSalida()
        {
            InitializeComponent();
            RbInterno.IsChecked = true;
            seleccioneParaFiltrar();
         //   LoadListaComboProveedor();
            loadProductosRemitos();
           
            loadLtsRemitos();
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
            var newW = new windowAgregarRemito(1);
            newW.ShowDialog();
            if (newW.DialogResult == true)
            {
                //DATOS REMITO.
                int numeroRemito = int.Parse(newW.txtNroRemito.Text);
                DateTime fecha = newW.dtRemito.SelectedDate.Value.Date;

                int idOC = (int)newW.cmbOrden.SelectedValue;
              
                string consulta = "insert into remitosalidas( numeroRemito, fecha, FK_idOrdenCompra) values( '" + numeroRemito + "', '" + fecha.ToString("yyyy/MM/dd") + "','" + idOC + "')";
                conexion.operaciones(consulta);

                //PRODUCTOS REMITO
                string ultimoId = "Select last_insert_id()";

                String id = conexion.ValorEnVariable(ultimoId);
                foreach (var producto in newW.ProdRemito)
                {
                   
                    String productos = "insert into productos_has_remitossalida(cantidad, CrNotaCredito,  FK_idProducto, FK_idRemito) values( '" + producto.cantidad + "', '" + producto.cantidad + "','" + producto.id + "','" + id + "' )";
                    conexion.operaciones(productos);
                }
                //ACTUALIZAR CANTITAD RESTANTE REMITO DE PRODUCTO OC
                int idOrden = (int)newW.cmbOrden.SelectedValue;
                foreach (var producto in newW.Productos)
                {
                    String sql = "UPDATE productos_has_ordencomprasalida SET CrRemito = '" + producto.cantidad + "' where FK_idProducto = '" + producto.id + "' and FK_idOrdenCompra = '" + idOrden + "'";
                    conexion.operaciones(sql);
                }
                //CARGAR STOCK EN PRODUCTO
                foreach (var producto in newW.ProdRemito)
                {
                    Console.WriteLine("id " + producto.id);
                    Console.WriteLine("id " + producto.cantidad);
                    String sql = "UPDATE productos SET stock = stock-'" + producto.cantidad + "', ums = '"+DateTime.Now.ToString("yyyy/MM/dd") + "' where idProductos = '" + producto.id + "' ";
                    conexion.operaciones(sql);
                }
                LoadListaComboProveedor();
                seleccioneParaFiltrar();
                loadLtsRemitos();

             
             
                ltsremitos.Items.MoveCurrentToLast();
                MessageBox.Show("Se agregó correctamente el Remito", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        public void LoadListaComboProveedor()
        {
            if (RbInterno.IsChecked == true)
            {
                String consulta = "SELECT DISTINCT p.nombre, p.idClientemi FROM clientesmi p ,ordencomprasalida o, remitosalidas r where o.FK_idClientemi = p.idClientemi and r.FK_idOrdenCompra = o.idOrdenCompra";
                conexion.Consulta(consulta, combo: cmbProveedores);
                ejecutar = false;
                cmbProveedores.DisplayMemberPath = "nombre";
                cmbProveedores.SelectedValuePath = "idClientemi";
                ejecutar = true;
                cmbProveedores.SelectedIndex = 0;
          
            }
            else
            {
                String consulta = "SELECT DISTINCT p.nombre, p.idClienteme FROM clientesme p ,ordencomprasalida o, remitosalidas r where o.FK_idClienteme = p.idClienteme and r.FK_idOrdenCompra = o.idOrdenCompra";
                conexion.Consulta(consulta, combo: cmbProveedores);
                ejecutar = false;
                cmbProveedores.DisplayMemberPath = "nombre";
                cmbProveedores.SelectedValuePath = "idClienteme";
                ejecutar = true;
                cmbProveedores.SelectedIndex = 0;
         
            }

        }

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            
            if (cmbProveedores.Text != "selccione para filtrar")
            {
                if (RbInterno.IsChecked == true)
                {
                    String consulta = " Select * from ordencomprasalida t1 where t1.FK_idClientemi = @valor ";
                    DataTable OCProveedor = conexion.ConsultaParametrizada(consulta, cmbProveedores.SelectedValue);
                    cmbOrdenes.ItemsSource = OCProveedor.AsDataView();
                    cmbOrdenes.DisplayMemberPath = "idOrdenCompra";
                    cmbOrdenes.SelectedValuePath = "idOrdenCompra";
                    cmbOrdenes.SelectedIndex = -1;

         
                    String sql = "   Select distinct DATE_FORMAT(t1.fecha, '%d-%m-%Y') AS fecha from ordencomprasalida t1 inner join remitosalidas t2 where t1.FK_idClientemi = @valor and t2.FK_idOrdenCompra = t1.idOrdenCompra";
                    DataTable fechas = conexion.ConsultaParametrizada(sql, cmbProveedores.SelectedValue);
                    cmbFechas.ItemsSource = fechas.AsDataView();
                    cmbFechas.DisplayMemberPath = "fecha";
                    cmbFechas.SelectedValuePath = "fecha";
                    cmbFechas.SelectedIndex = -1;

             
                    String consulta2 = "Select distinct idOrdenCompra from ordencomprasalida t1 inner join remitosalidas t2 where FK_idClientemi = @valor and t2.FK_idOrdenCompra = t1.idOrdenCompra ";

                    DataTable OCFecha = conexion.ConsultaParametrizada(consulta2, cmbProveedores.SelectedValue);
                    cmbOrdenes.ItemsSource = OCFecha.AsDataView();
                    cmbOrdenes.DisplayMemberPath = "idOrdenCompra";
                    cmbOrdenes.SelectedValuePath = "idOrdenCompra";
                    cmbOrdenes.SelectedIndex = -1;
                }
                else
                {
                    String consulta = " Select * from ordencomprasalida t1 where t1.FK_idClienteme = @valor ";
                    DataTable OCProveedor = conexion.ConsultaParametrizada(consulta, cmbProveedores.SelectedValue);
                    cmbOrdenes.ItemsSource = OCProveedor.AsDataView();
                    cmbOrdenes.DisplayMemberPath = "idOrdenCompra";
                    cmbOrdenes.SelectedValuePath = "idOrdenCompra";
                    cmbOrdenes.SelectedIndex = -1;


                    String sql = "   Select distinct DATE_FORMAT(t1.fecha, '%d-%m-%Y') AS fecha from ordencomprasalida t1 inner join remitosalidas t2 where t1.FK_idClienteme = @valor and t2.FK_idOrdenCompra = t1.idOrdenCompra";
                    DataTable fechas = conexion.ConsultaParametrizada(sql, cmbProveedores.SelectedValue);
                    cmbFechas.ItemsSource = fechas.AsDataView();
                    cmbFechas.DisplayMemberPath = "fecha";
                    cmbFechas.SelectedValuePath = "fecha";
                    cmbFechas.SelectedIndex = -1;


                    String consulta2 = "Select distinct idOrdenCompra from ordencomprasalida t1 inner join remitosalidas t2 where FK_idClienteme = @valor and t2.FK_idOrdenCompra = t1.idOrdenCompra ";

                    DataTable OCFecha = conexion.ConsultaParametrizada(consulta2, cmbProveedores.SelectedValue);
                    cmbOrdenes.ItemsSource = OCFecha.AsDataView();
                    cmbOrdenes.DisplayMemberPath = "idOrdenCompra";
                    cmbOrdenes.SelectedValuePath = "idOrdenCompra";
                    cmbOrdenes.SelectedIndex = -1;
                }
          

        

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

            if (RbInterno.IsChecked == true)
            {
                String consulta = "select * from remitosalidas r, ordencomprasalida o WHERE r.FK_idOrdenCompra = o.idOrdenCompra AND FK_idClientemi IS NOT NULL";
                conexion.Consulta(consulta, tabla: ltsremitos);
                ltsremitos.DisplayMemberPath = "numeroRemito";
                ltsremitos.SelectedValuePath = "idremitos";
                ltsremitos.SelectedIndex = 0;
            }
            else
            {
                String consulta = "select * from remitosalidas r, ordencomprasalida o WHERE r.FK_idOrdenCompra = o.idOrdenCompra AND FK_idClienteme IS NOT NULL";
                conexion.Consulta(consulta, tabla: ltsremitos);
                ltsremitos.DisplayMemberPath = "numeroRemito";
                ltsremitos.SelectedValuePath = "idremitos";
                ltsremitos.SelectedIndex = 0;
            }

        }
        public void loadLtsRemitos(int index)
        {
            seleccioneParaFiltrar();
            String consulta = "select * from remitosalidas";
            conexion.Consulta(consulta, tabla: ltsremitos);
            ltsremitos.DisplayMemberPath = "numeroRemito";
            ltsremitos.SelectedValuePath = "idremitos";
            ltsremitos.SelectedIndex = index;
        }

        public void loadLtsRemitosProv()
        {

            if (RbInterno.IsChecked == true)
            {
                String consulta = "select idremitos, numeroRemito from remitosalidas t1 inner join ordencomprasalida t2 where t1.FK_idOrdenCompra = t2.idOrdenCompra and t2.FK_idClientemi = @valor";
                ltsremitos.ItemsSource = conexion.ConsultaParametrizada(consulta, cmbProveedores.SelectedValue).AsDataView();
                ltsremitos.DisplayMemberPath = "numeroRemito";
                ltsremitos.SelectedValuePath = "idremitos";
                ltsremitos.SelectedIndex = 0;
            }
            else
            {
                String consulta = "select idremitos, numeroRemito from remitosalidas t1 inner join ordencomprasalida t2 where t1.FK_idOrdenCompra = t2.idOrdenCompra and t2.FK_idClienteme = @valor";
                ltsremitos.ItemsSource = conexion.ConsultaParametrizada(consulta, cmbProveedores.SelectedValue).AsDataView();
                ltsremitos.DisplayMemberPath = "numeroRemito";
                ltsremitos.SelectedValuePath = "idremitos";
                ltsremitos.SelectedIndex = 0;
            }


        }
        public void loadLtsRemitosFecha()
        {
            if (cmbFechas.SelectedIndex != -1)
            {

                if (RbInterno.IsChecked == true)
                {
                    string consulta = "select idremitos, numeroRemito from ordencomprasalida t1 inner join remitosalidas t2 where t1.idOrdenCompra = t2.FK_idOrdenCompra and t1.FK_idClientemi = '" + cmbProveedores.SelectedValue + "'  and t1.fecha = @valor";
                    DateTime fecha = new DateTime();
                    DateTime.TryParse(cmbFechas.SelectedValue.ToString(), out fecha);
                    ltsremitos.ItemsSource = conexion.ConsultaParametrizada(consulta, fecha.ToString("yyyy/MM/dd")).AsDataView();
                    ltsremitos.DisplayMemberPath = "numeroRemito";
                    ltsremitos.SelectedValuePath = "idremitos";
                    ltsremitos.SelectedIndex = 0;
                }
                else
                {
                    string consulta = "select idremitos, numeroRemito from ordencomprasalida t1 inner join remitosalidas t2 where t1.idOrdenCompra = t2.FK_idOrdenCompra and t1.FK_idClienteme = '" + cmbProveedores.SelectedValue + "'  and t1.fecha = @valor";
                    DateTime fecha = new DateTime();
                    DateTime.TryParse(cmbFechas.SelectedValue.ToString(), out fecha);
                    ltsremitos.ItemsSource = conexion.ConsultaParametrizada(consulta, fecha.ToString("yyyy/MM/dd")).AsDataView();
                    ltsremitos.DisplayMemberPath = "numeroRemito";
                    ltsremitos.SelectedValuePath = "idremitos";
                    ltsremitos.SelectedIndex = 0;
                }
        
            }
        }
        private void ltsremitos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            //Datos Remito

            if (RbInterno.IsChecked == true)
            {
                String sql = "Select  t3.nombre,t1.fecha ,t2.idOrdenCompra from remitosalidas t1 , ordencomprasalida t2, clientesmi t3 where idremitos = @valor and t1.FK_idOrdenCompra = t2.idOrdenCompra and t2.FK_idClientemi = t3.idClientemi";
                DataTable datos = conexion.ConsultaParametrizada(sql, ltsremitos.SelectedValue);
                if (datos.Rows.Count > 0)
                {
                    txtProveedor.Text = datos.Rows[0].ItemArray[0].ToString();
                    DateTime fecha = (DateTime)datos.Rows[0].ItemArray[1];
                    this.fecha = (DateTime)datos.Rows[0].ItemArray[1];
                    txtFecha.Text = fecha.ToString("dd/MM/yyyy");
                    txtOC.Text = datos.Rows[0].ItemArray[2].ToString();
                }
            }
            else
            {
                String sql = "Select  t3.nombre,t1.fecha ,t2.idOrdenCompra from remitosalidas t1 , ordencomprasalida t2, clientesme t3 where idremitos = @valor and t1.FK_idOrdenCompra = t2.idOrdenCompra and t2.FK_idClienteme = t3.idClienteme";
                DataTable datos = conexion.ConsultaParametrizada(sql, ltsremitos.SelectedValue);
                if (datos.Rows.Count > 0)
                {
                    txtProveedor.Text = datos.Rows[0].ItemArray[0].ToString();
                    DateTime fecha = (DateTime)datos.Rows[0].ItemArray[1];
                    this.fecha = (DateTime)datos.Rows[0].ItemArray[1];
                    txtFecha.Text = fecha.ToString("dd/MM/yyyy");
                    txtOC.Text = datos.Rows[0].ItemArray[2].ToString();
                }
            }
       
            //consulta productos
  
            String consulta = "  SELECT t2.nombre , t1.cantidad, t2.idProductos from productos_has_remitossalida t1 inner join productos t2  on t1.FK_idProducto = t2.idProductos where t1.FK_idRemito = @valor";
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

            String consulta = "SELECT distinct idremitos, numeroRemito from remitosalidas inner join ordencomprasalida where FK_idOrdenCompra = @valor ";
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

                string consulta1 = "SELECT count(FK_idremitos) FROM notacreditosalida where FK_idremitos = " + ltsremitos.SelectedValue + "";

                if (conexion.ValorEnVariable(consulta1) == "0")
                {
                    DataRow selectedDataRow = ((DataRowView)ltsremitos.SelectedItem).Row;
                    string numeroRemito = selectedDataRow["numeroRemito"].ToString();
                    MessageBoxResult dialog = MessageBox.Show("¿Esta seguro que desea eliminar el remito numero " + numeroRemito, "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (dialog == MessageBoxResult.Yes)
                    {
                        int idSeleccionado = (int)ltsremitos.SelectedValue;
                        for (int i = 0; i < productos.Rows.Count; i++)
                        {
                            String consulta = "UPDATE productos_has_ordencomprasalida SET CrRemito = CrRemito + '" + (int)productos.Rows[i].ItemArray[1] + "' where FK_idProducto = '" + productos.Rows[i].ItemArray[2] + "' and FK_idOrdenCompra = '" + txtOC.Text.ToString() + "'";
                            conexion.operaciones(consulta);

                            //Console.Write();
                            String sql2 = "UPDATE productos SET stock = stock+'" + (int)productos.Rows[i].ItemArray[1] + "' where idProductos = '" + productos.Rows[i].ItemArray[2] + "' ";
                            conexion.operaciones(sql2);
                        }
                        string sql = "delete from remitosalidas where idremitos = '" + idSeleccionado + "'";
                        conexion.operaciones(sql);

                        if (ltsremitos.Items.Count <= 0)
                        {
                            txtProveedor.Text = "";
                            txtOC.Text = "";
                            txtFecha.Text = "";
                        }
                        seleccioneParaFiltrar();
                        loadLtsRemitos();
                        LoadListaComboProveedor();
                        MessageBox.Show("Se eliminó correctamente el Remito", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("El remito no se puede eliminar porque tiene notas de crédito", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un remito a eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void btnModificar_Copy_Click(object sender, RoutedEventArgs e)
        {
            int tipocliente;
            try
            {


                string consulta1 = "SELECT count(FK_idremitos) FROM notacreditosalida where FK_idremitos = " + ltsremitos.SelectedValue + "";

                if (conexion.ValorEnVariable(consulta1) == "0")
                {
                    string numeroR = (((DataRowView)ltsremitos.SelectedItem).Row[1]).ToString();
                    DataTable idprov;
                    int OC;
                    int index;
                    if (RbInterno.IsChecked == true)
                    {
                        string consulta = "select t2.FK_idClientemi from  ordencomprasalida t2 where idOrdenCompra = @valor";
                        int.TryParse(txtOC.Text.ToString(), out OC);
                        idprov = conexion.ConsultaParametrizada(consulta, OC);
                        index = ltsremitos.SelectedIndex;
                        tipocliente = 1;
                    }
                    else
                    {
                        string consulta = "select t2.FK_idClienteme from  ordencomprasalida t2 where idOrdenCompra = @valor";
                        int.TryParse(txtOC.Text.ToString(), out OC);
                        idprov = conexion.ConsultaParametrizada(consulta, OC);
                        index = ltsremitos.SelectedIndex;
                        tipocliente = 2;
                       
                    }

                    var newW = new windowAgregarRemito((int)idprov.Rows[0].ItemArray[0], OC, productosparametro, fecha, numeroR, (int)ltsremitos.SelectedValue, tipocliente);
                    newW.ShowDialog();




                    if (newW.DialogResult == true)
                    {
                        //DATOS REMITO.

                        int numeroRemito = int.Parse(newW.txtNroRemito.Text);
                        DateTime fecha = newW.dtRemito.SelectedDate.Value.Date;
                        int idOC = (int)newW.cmbOrden.SelectedValue;
                        int idRemito = newW.idRemito;

                        string consultasql = "UPDATE  remitosalidas SET numeroRemito = '" + numeroRemito + "', fecha ='" + fecha.ToString("yyyy/MM/dd") + "', FK_idOrdenCompra ='" + idOC + "' where idremitos ='" + idRemito + "' ";
                        conexion.operaciones(consultasql);

                        //ELIMINAR PRODUCTOS

                        String sqlElim = "delete from productos_has_remitossalida where FK_idRemito = '" + idRemito + "'";
                        conexion.operaciones(sqlElim);
                        //PRODUCTOS REMITO  

                        foreach (var producto in newW.ProdRemito)
                        {
                            Console.Write("stock nuevo :" + producto.cantidad);
                            String productos = "insert into productos_has_remitossalida(cantidad,  FK_idProducto, FK_idRemito) values( '" + producto.cantidad + "', '" + producto.id + "','" + idRemito + "' )";
                            conexion.operaciones(productos);
                        }
                        //ACTUALIZAR CANTITAD RESTANTE REMITO DE PRODUCTO OC
                        int idOrden = (int)newW.cmbOrden.SelectedValue;
                        foreach (var producto in newW.Productos)
                        {
                            String sql = "UPDATE productos_has_ordencomprasalida SET CrRemito = '" + producto.cantidad + "' where FK_idProducto = '" + producto.id + "' and FK_idOrdenCompra = '" + idOrden + "'";
                            conexion.operaciones(sql);
                        }
                        //RESTO CANTIDAD STOCK VIEJA
                        foreach (var producto in newW.ProdRemitoStock)
                        {
                            //MessageBox.Show("stock viejo :" + producto.cantidad);

                            String sql = "UPDATE productos SET stock = stock+'" + producto.cantidad + "' where idProductos = '" + producto.id + "' ";
                            conexion.operaciones(sql);
                        }
                        //SUMO CANTIDAD NUEVA STOCK EN PRODUCTO
                        foreach (var producto in newW.ProdRemito)
                        {
                            //MessageBox.Show("stock nuevo :" + producto.cantidad);
                            String sql = "UPDATE productos SET stock = stock-'" + producto.cantidad + "' where idProductos = '" + producto.id + "' ";
                            conexion.operaciones(sql);
                        }

                        loadLtsRemitos(index);
                        MessageBox.Show("Se modificó correctamente el Remito", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("El remito no se puede modificar porque tiene notas de crédito", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un remito a modificar");
            }


        }

        private void RbInterno_Checked(object sender, RoutedEventArgs e)
        {
            LoadListaComboProveedor();         
         
      
            seleccioneParaFiltrar();
            loadLtsRemitos();
        }

        private void RbExterno_Checked(object sender, RoutedEventArgs e)
        {
            LoadListaComboProveedor();
         
         
            seleccioneParaFiltrar();
            loadLtsRemitos();
        }
    }

}



