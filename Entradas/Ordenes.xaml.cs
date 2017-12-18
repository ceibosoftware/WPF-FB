using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for Ordenes.xaml
    /// </summary>
    public partial class Ordenes : Page
    {
        bool ejecutar = true;
        CRUD conexion = new CRUD();
        public Ordenes()
        {
            InitializeComponent();
            LoadListaComboProveedor();      
            //loadlistaOC();  
            //fechaActual();
            loadlistaOC();
            cmbProveedores.Tag = "hola";
        }

        private void loadlistaOC()
        {
            try
            {
                String consulta = " Select * from ordencompra ";
                conexion.Consulta(consulta, ltsNumeroOC);
                ltsNumeroOC.DisplayMemberPath = "idOrdenCompra";
                ltsNumeroOC.SelectedValuePath = "idOrdenCompra";
               
            }
            catch (NullReferenceException)
            {

               
            }
              

        }
        private void loadlistaOC(int index)
        {
            try
            {
                String consulta = " Select * from ordencompra ";
                conexion.Consulta(consulta, ltsNumeroOC);             
                ltsNumeroOC.DisplayMemberPath = "idOrdenCompra";
                ltsNumeroOC.SelectedValuePath = "idOrdenCompra";  
                ltsNumeroOC.SelectedIndex = index;
                
            }
            catch (NullReferenceException)
            {


            }


        }

        public void LoadListaComboProveedor()
        {
            String consulta = "SELECT DISTINCT p.nombre, p.idProveedor FROM proveedor p inner join ordencompra o where o.FK_idProveedor = p.idProveedor ";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
            cmbProveedores.SelectedIndex = 0;
        }


        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarOC();
            newW.ShowDialog();
            if (newW.DialogResult == true)
            {
                //INSERTAR OC
                int Proveedor = (int)newW.cmbProveedores.SelectedValue;
                Console.WriteLine(Proveedor);
                DateTime fecha = newW.fecha;
                fecha = Convert.ToDateTime(fecha.ToString("yyyy/MM/dd"));
                Console.WriteLine(fecha);
                decimal.TryParse(newW.txtSubtotal.Text, out decimal subtotal);
                decimal.TryParse(newW.txtTotal.Text, out decimal total);
                int direccion = (int)newW.cmbDireccion.SelectedValue;
                int telefono = (int)newW.cmbTelefono.SelectedValue;
                String observacion = newW.txtObservaciones.Text;
                String formaPago = newW.txtFormaPago.Text;
                int iva = newW.cmbIVA.SelectedIndex;
                int tipoCambio = newW.cmbTipoCambio.SelectedIndex;          
                String sql = "insert into ordencompra(fecha, observaciones, subtotal, total, iva, tipoCambio ,formaPago, FK_idContacto,FK_idDireccion,FK_idProveedor) values( '" + fecha.ToString("yyyy/MM/dd") + "', '" + observacion + "', '" + subtotal+ "', '" + total+ "', '" + iva+ "','" + tipoCambio+ "','" + formaPago + "','" + telefono + "','" + direccion + "','" + Proveedor + "');";
                conexion.operaciones(sql);

                string ultimoId = "Select last_insert_id()";
                String id = conexion.ValorEnVariable(ultimoId);
                foreach (var producto in newW.productos)
                {
                    String productos = "insert into productos_has_ordencompra(cantidad, subtotal, Crfactura, CrRemito, FK_idProducto, FK_idOC) values( '" + producto.cantidad + "', '" +producto.total + "', '" + producto.cantidad + "', '" + producto.cantidad + "', '" + producto.id + "','" + id + "');";
                    conexion.operaciones(productos);
                }
                ejecutar = false;
                loadlistaOC();
                LoadListaComboProveedor();
                ejecutar = true;
            }
            
        }

    

    
      /* private void fechaActual()
        {

            dpFecha.SelectedDate = DateTime.Now;
        }*/

        private void ltsNumeroOC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //consulta productos
                String consulta = "  SELECT t2.nombre , t1.cantidad,  t1.subtotal from productos_has_ordencompra t1 inner join productos t2  on t1.FK_idProducto = t2.idProductos where t1.FK_idOC = @valor";
                DataTable productos = conexion.ConsultaParametrizada(consulta, ltsNumeroOC.SelectedValue);
                dgvProductos.ItemsSource = productos.AsDataView();
                //llenar datos de oc
                String consulta2 = "SELECT * FROM ordencompra t1 where t1.idOrdenCompra = @valor";
                DataTable OC = conexion.ConsultaParametrizada(consulta2, ltsNumeroOC.SelectedValue);
                DateTime fecha = (DateTime)OC.Rows[0].ItemArray[1];
                lblFechaOC.Content = fecha.ToString("dd/MM/yyyy");
                txtSubtotal.Text = OC.Rows[0].ItemArray[3].ToString();
                if((int)OC.Rows[0].ItemArray[5] == 0)
                {
                    txtIva.Text = "0";
                }else if((int)OC.Rows[0].ItemArray[5] == 1)
                {
                    txtIva.Text = "21";
                }
                else
                {
                    txtIva.Text = "10,5";
                }
               
                txtFormaPago.Text = OC.Rows[0].ItemArray[7].ToString();
                //simbolo segun tipo cambio
                if((int)OC.Rows[0].ItemArray[6] == 0)
                {
                    txtTipoCambio.Text = "$";
                }
                else if((int)OC.Rows[0].ItemArray[6] == 1)
                {
                    txtTipoCambio.Text = "u$d";
                }
                else{
                    txtTipoCambio.Text = "€";
                }
              
                txtTotal.Text = OC.Rows[0].ItemArray[4].ToString();
                txtDescripcion.Text = OC.Rows[0].ItemArray[2].ToString();
            }
            catch (Exception)
            {

               
            }
      
         
        }

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ejecutar)
                {
                    String consulta = " Select * from ordencompra t1 where t1.FK_idProveedor = @valor ";
                    DataTable OCProveedor = conexion.ConsultaParametrizada(consulta, cmbProveedores.SelectedValue);
                    ltsNumeroOC.ItemsSource = OCProveedor.AsDataView();
                    ltsNumeroOC.DisplayMemberPath = "idOrdenCompra";
                    ltsNumeroOC.SelectedValuePath = "idOrdenCompra";
                    ltsNumeroOC.SelectedIndex = 0;
                    String sql = "   Select distinct DATE_FORMAT(t1.fecha, '%d-%m-%Y') AS fecha from ordencompra t1 where t1.FK_idProveedor = @valor ";
                    DataTable fechas = conexion.ConsultaParametrizada(sql, cmbProveedores.SelectedValue);
                    cmbFechas.ItemsSource = fechas.AsDataView();
                    cmbFechas.DisplayMemberPath= "fecha";
                    cmbFechas.SelectedValuePath = "fecha";
                    cmbFechas.SelectedIndex = 0;


                }
            }
            catch (NullReferenceException)
            {


            }

        }

      /*  private void dpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                String consulta = " Select * from ordencompra t1 where t1.fecha = @valor ";
                DataTable OCFecha = conexion.ConsultaParametrizada(consulta, dpFecha.SelectedDate);
                ltsNumeroOC.ItemsSource = OCFecha.AsDataView();
                ltsNumeroOC.DisplayMemberPath = "idOrdenCompra";
                ltsNumeroOC.SelectedValuePath = "idOrdenCompra";
                ltsNumeroOC.SelectedIndex = 0;
            }
            catch (NullReferenceException)
            {


            }
        }
        */
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRow selectedDataRow = ((DataRowView)ltsNumeroOC.SelectedItem).Row;
                string OC = selectedDataRow["idOrdenCompra"].ToString();
                MessageBoxResult dialog = MessageBox.Show("Esta seguro que desea eliminar la orden de compra numero : " + OC, "Advertencia", MessageBoxButton.YesNo);
                if (dialog == MessageBoxResult.Yes)
                {
                    int idSeleccionado = (int)ltsNumeroOC.SelectedValue;
                    string sql = "delete from ordencompra where idOrdenCompra = '" + idSeleccionado + "'";
                    conexion.operaciones(sql);
                    loadlistaOC();

                }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Es necesario seleccionar una orden de compra a eliminar");
            }
        }

        private void btnModificar_Copy_Click(object sender, RoutedEventArgs e)
        {
            int idOC = (int)ltsNumeroOC.SelectedValue;
            int index = (int)ltsNumeroOC.SelectedIndex;
            //VALORES NECESARIOS PARA LLENAR CONSTRUCTOR
            String consulta = "SELECT * FROM ordencompra where idOrdenCompra = @valor";
            DataTable OC = conexion.ConsultaParametrizada(consulta, ltsNumeroOC.SelectedValue);
            DateTime fecha = (DateTime)OC.Rows[0].ItemArray[1];
            String observaciones = OC.Rows[0].ItemArray[2].ToString();
            Decimal subtotal =(Decimal)OC.Rows[0].ItemArray[3];          
            int iva = (int)OC.Rows[0].ItemArray[5];
            int tipoCambio = (int)OC.Rows[0].ItemArray[6];
            String formaPago = OC.Rows[0].ItemArray[7].ToString();
            int telefono = (int)OC.Rows[0].ItemArray[8];
            int proveedor=(int)OC.Rows[0].ItemArray[10];
            int direccion = (int)OC.Rows[0].ItemArray[9];
            
            //PRODUCTOS DE LA ORDEN DE COMPRA
            String consultaProductos = "SELECT t2.idProductos, t1.cantidad ,t1.subtotal,t2.nombre,t2.precioUnitario FROM productos_has_ordencompra t1 inner join productos t2 where FK_idOC = @valor and t1.FK_idProducto = t2.idProductos";
            DataTable productos = conexion.ConsultaParametrizada(consultaProductos, ltsNumeroOC.SelectedValue);
            List<producto> listaProd = new List<producto>();
            Console.WriteLine(productos.Rows.Count);
          
            for (int i = 0; i < productos.Rows.Count; i++)
            {
                
                int idProducto = (int)productos.Rows[i].ItemArray[0];
                int cantitad = (int)productos.Rows[i].ItemArray[1];
                Decimal sub = (Decimal)productos.Rows[i].ItemArray[2];
                String nombre = productos.Rows[i].ItemArray[3].ToString();
                float PU = (float)productos.Rows[i].ItemArray[4];
                decimal PU2;
                decimal.TryParse(PU.ToString(), out PU2);
                listaProd.Add(new producto(nombre,idProducto,cantitad,sub,PU2));
            }
            var newW = new windowAgregarOC(fecha, observaciones, subtotal, iva, tipoCambio, formaPago, telefono, proveedor, direccion,listaProd);
            Console.WriteLine(proveedor);
            newW.Title = "Modificar OC";
            newW.ShowDialog();

            if(newW.DialogResult == true)
            {
                //INSERTAR OC
                int Proveedor = (int)newW.cmbProveedores.SelectedValue;
                fecha = newW.fecha;
                
                Console.WriteLine(fecha);
                decimal.TryParse(newW.txtSubtotal.Text, out decimal sub);
                decimal.TryParse(newW.txtTotal.Text, out decimal total);
                direccion = (int)newW.cmbDireccion.SelectedValue;
                telefono = (int)newW.cmbTelefono.SelectedValue;
                observaciones = newW.txtObservaciones.Text;
                formaPago = newW.txtFormaPago.Text;
                iva = newW.cmbIVA.SelectedIndex;
                tipoCambio = newW.cmbTipoCambio.SelectedIndex;
                String sql = "UPDATE ordencompra SET fecha = '" + fecha.ToString("yyyy/MM/dd") + "', observaciones = '" + observaciones + "' ,subtotal = '" + sub + "',total = '" + total + "',iva = '" + iva + "',tipoCambio = '" + tipoCambio + "',formaPago = '" + formaPago + "',FK_idContacto = '" + telefono + "',FK_idDireccion = '" + direccion+ "',FK_idProveedor = '" + Proveedor + "' WHERE ordencompra.idOrdenCompra = '" + idOC + "';";
                conexion.operaciones(sql);
                //ELIMINA REGISTRO DE TABLA INTERMEDIA
                string sql2 = "delete  from productos_has_ordencompra where FK_idOC =  '" + idOC + "'";
                conexion.operaciones(sql2);

             
                foreach (var producto in newW.productos)
                {
                    String productosActualizar = "insert into productos_has_ordencompra(cantidad, subtotal, Crfactura, CrRemito, FK_idProducto, FK_idOC) values( '" + producto.cantidad + "', '" + producto.total + "', '" + producto.cantidad + "', '" + producto.cantidad + "', '" + producto.id + "','" + idOC + "');";
                    conexion.operaciones(productosActualizar);
                }
                ejecutar = false;
                loadlistaOC(index);
                LoadListaComboProveedor();
                ejecutar = true;
            }
        }

        private void cmbFechas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String consulta = " Select * from ordencompra t1 where t1.fecha = @valor and FK_idProveedor = '"+cmbProveedores.SelectedValue +"'";
            DateTime fecha;
            DateTime.TryParse(cmbFechas.SelectedValue.ToString(), out fecha);                        
            fecha.ToString("yyyy-MM-dd");
            DataTable OCFecha = conexion.ConsultaParametrizada(consulta,fecha );
            ltsNumeroOC.ItemsSource = OCFecha.AsDataView();
            ltsNumeroOC.DisplayMemberPath = "idOrdenCompra";
            ltsNumeroOC.SelectedValuePath = "idOrdenCompra";
            ltsNumeroOC.SelectedIndex = 0;
        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Busquedas de productos.
            DataTable productos = new DataTable();
            String consulta;
            consulta = "SELECT * FROM proveedor WHERE proveedor.nombre LIKE '%' @valor '%'";
            productos = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            cmbProveedores.ItemsSource = productos.AsDataView();
            cmbProveedores.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loadlistaOC();
        }
    }
}
