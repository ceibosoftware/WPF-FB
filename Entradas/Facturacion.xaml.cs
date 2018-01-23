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
        public List<Producto> itemsFactura = new List<Producto>();
        public List<Producto> itemsdb = new List<Producto>();
        public List<Producto> itemsFacturaDB = new List<Producto>();
        public List<Producto> itemsFacdb = new List<Producto>();
        public List<Producto> produ = new List<Producto>();
        public List<factura> items = new List<factura>();
        public List<Cuotas> cuotasAinsertar = new List<Cuotas>();
        factura fa;
        Decimal subtotal;
        public Facturacion()
        {
    
            InitializeComponent();
            LoadListaComboProveedor();
           LoadDgvFactura();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            cuotasAinsertar.Clear();
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


                //inserto cuotas
                foreach (Cuotas cuot in newW.todaslascuotas)
                {

                    int id2 = cuot.cuota;
                    int dias = cuot.dias;
                    DateTime fecha = cuot.fechadepago;


                    String sqlProductoHas = "INSERT INTO cuotas ( dias, fecha, FK_idFacturas) VALUES ('" + dias + "', '" + fecha.ToString("yyyy/MM/dd") + "', '" + id + "')";
                    conexion.operaciones(sqlProductoHas);


                }
                
                //INSERTO LOS PRODUCTOS DE LA FACTURA
                foreach (Producto p in newW.itemsFact)
                {
                    String nombre = p.nombre;
                    int cantidad = p.cantidad;
                    decimal totalp = p.total;
                    decimal precioUni = p.precioUnitario;
          
                    Producto pr = new Producto(nombre, 1, cantidad, totalp, precioUni);
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
            String consulta = "SELECT DISTINCT p.nombre, p.idProveedor FROM proveedor p INNER JOIN ordencompra o ON p.idProveedor = o.FK_idProveedor";
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
            ltsFactura.SelectedIndex = 0;
        }

        private void ltsFactura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

    

            try
            {
                String productosFatura = "SELECT t1.cantidad ,t1.subtotal, t2.nombre ,t2.precioUnitario ,t2.idProductos from productos_has_facturas t1 inner join productos t2 where t1.FK_idProducto = t2.idProductos and t1.FK_idfactura =  '" + ltsFactura.SelectedValue + "'";
                DataTable productos = conexion.ConsultaParametrizada(productosFatura, ltsFactura.SelectedValue);
                dgvProductosFactura.ItemsSource = productos.AsDataView();

                
                String consulta2 = "SELECT * FROM factura f where f.idfacturas ='" + ltsFactura.SelectedValue + "'";
                DataTable OC = conexion.ConsultaParametrizada(consulta2, ltsFactura.SelectedValue);

                if (OC.Rows[0].ItemArray[4].ToString() == "0")
                {
                    txtIVA.Text = "0";
                }else if (OC.Rows[0].ItemArray[4].ToString() == "1")
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
        
            

            }
            catch (Exception)
            {

          
            }

    

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

            String sql = "SELECT numeroFactura FROM factura WHERE idfacturas = '" + ltsFactura.SelectedValue + "'";
            String fa = conexion.ValorEnVariable(sql);


            MessageBoxResult dialog = MessageBox.Show("Esta seguro que desea eliminar la factura numero :" + fa, "Advertencia", MessageBoxButton.YesNo);
            if (dialog == MessageBoxResult.Yes)
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
            itemsFacturaDB.Clear();
            int numerofacturaID = (int)this.ltsFactura.SelectedValue;

            String proveedor = cmbProveedores.Text;
            
            String crfact = "SELECT * FROM factura WHERE idfacturas = '" + numerofacturaID + "'";
            DataTable OC = conexion.ConsultaParametrizada(crfact, numerofacturaID);
            DateTime fecha = (DateTime)OC.Rows[0].ItemArray[8];
            String cuotas = OC.Rows[0].ItemArray[6].ToString();
            int numf = (int)OC.Rows[0].ItemArray[2];
            int iva;
            int tipocambio;

            itemsFacdb.Clear();
            int j = 0;
            for (int i = 0; i < dgvProductosFactura.Items.Count - 1; i++)
            {

                var cantidad = (dgvProductosFactura.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
                j++;

                var total = (dgvProductosFactura.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
                j++;

                var nombre = (dgvProductosFactura.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
                j++;


                var precioU = (dgvProductosFactura.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
                j++;


                var id = (dgvProductosFactura.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
                j++;
                j = 0;

 
                subtotal = subtotal + decimal.Parse(precioU) * decimal.Parse(total);
                Producto conA = new Producto(nombre, int.Parse(id), int.Parse(cantidad), decimal.Parse(total), decimal.Parse(precioU));
                itemsFacdb.Add(conA);
                }

            Cuotas cuota;
            cuotasAinsertar.Clear();
            try
            {
                
                String sql2 = "SELECT * from cuotas c  where c.FK_idfacturas = '" + numerofacturaID+"'";

                DataTable cuotass = conexion.ConsultaParametrizada(sql2, numerofacturaID);
                for (int i = 0; i < cuotass.Rows.Count; i++)
                {
                    cuota = new Cuotas((int)cuotass.Rows[i].ItemArray[0], (int)cuotass.Rows[i].ItemArray[1], (DateTime)cuotass.Rows[i].ItemArray[2]);
                    cuotasAinsertar.Add(cuota);
                  
                }



            }
            catch (NullReferenceException)
            {


            }

            Producto producto;
            try
            {
                String sql2 = "SELECT productos.nombre, productos.idProductos, cantidad, subtotal, productos_has_ordencompra.precioUnitario  FROM productos_has_ordencompra, productos WHERE FK_idOC ='" + int.Parse(cmbordenCompra.Text) + "' AND productos.idProductos = productos_has_ordencompra.FK_idProducto";

                DataTable productos = conexion.ConsultaParametrizada(sql2, int.Parse(cmbordenCompra.Text));
                for (int i = 0; i < productos.Rows.Count; i++)
                {
                    producto = new Producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[1], (int)productos.Rows[i].ItemArray[2], (decimal)productos.Rows[i].ItemArray[3], (decimal)productos.Rows[i].ItemArray[4]);
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

            var newW = new windowAgregarFactura((int)numf, proveedor, itemsdb, itemsFacdb, fecha, int.Parse(cmbordenCompra.Text), decimal.Parse(txtSubTotal.Text), decimal.Parse(txtTotal1.Text), iva, tipocambio, subtotal, cuotas, cuotasAinsertar);
            newW.Title = "Modificar Factura";
            newW.ShowDialog();
            
            if (newW.DialogResult == true)
            {
                String idPRove2 = cmbProveedores.SelectedValue.ToString();
                decimal subtotal2 = decimal.Parse(newW.txtSubtotal.Text);
                decimal total2 = decimal.Parse(newW.txtTotal.Text);
                int numeroFact2 = int.Parse(newW.txtNroFactura.Text);
                String iva32 = newW.cmbIVA.SelectedIndex.ToString();
                String tipoCambio2 = newW.cmbTipoCambio.SelectedIndex.ToString();
                String cuotas2 = newW.cmbCuotas.Text;
                int fkOrden2 = int.Parse(newW.cmbOrden.Text);
                DateTime dtp2 = System.DateTime.Now;
                dtp2 = newW.dtFactura.SelectedDate.Value;

                try
                {
                    //UPDATE FACTURA
                    String updatefactura = "UPDATE factura SET subtotal =  '" + subtotal2 + "',numeroFactura = '" + numeroFact2 + "' ,total = '" + total2 + "',iva= '" + iva32 + "',tipocambio='" + tipoCambio2 + "' ,cuotas = '" + cuotas2 + "',FK_idOC= '" + fkOrden2 + "',fecha ='" + dtp2.ToString("yyyy/MM/dd") + "' WHERE idfacturas = '" + numerofacturaID + "'";
                    conexion.operaciones(updatefactura);
                }
                catch (Exception)
                {

                    
                }
                
                string sql2 = "DELETE  FROM cuotas WHERE FK_idfacturas = '" + numerofacturaID + "'";
                conexion.operaciones(sql2);


                //inserto cuotas en la factura
                foreach (Cuotas cc in newW.todaslascuotas)
                {
                    int id2 = cc.cuota;
                    int dias12 = cc.dias;
                    DateTime fecha21 = cc.fechadepago;


                    String insertcuotas = "INSERT INTO cuotas ( dias, fecha, FK_idFacturas) VALUES ('" + dias12 + "', '" + fecha21.ToString("yyyy/MM/dd") + "', '" + numerofacturaID + "')";
                    conexion.operaciones(insertcuotas);


                }


                String deleteproducto = "DELETE FROM productos_has_facturas WHERE FK_idfactura = '"+numerofacturaID+"'";
                conexion.operaciones(deleteproducto);

                //INSERTO LOS PRODUCTOS DE LA FACTURA
                foreach (Producto p in newW.itemsFact)
                {
                    String nombre = p.nombre;
                    int cantidad = p.cantidad;
                    decimal totalp = p.total;
                    decimal precioUni = p.precioUnitario;
                    int idProducto = p.id;

                    Producto pr = new Producto(nombre, idProducto, cantidad, totalp, precioUni);
                    itemsFacturaDB.Add(pr);

             

          
                    String sqlProductoHas = "INSERT INTO productos_has_facturas (cantidad, subtotal, FK_idProducto, FK_idFactura) VALUES ('" + cantidad + "','" + subtotal2 + "', '" + idProducto + "', '" + numerofacturaID + "')";
                    conexion.operaciones(sqlProductoHas);


                    //TRAIGO LA CANTIDAD RESTANTE DE LA FACTURA
                    String crfact3 = "SELECT CrFactura FROM productos_has_ordencompra WHERE FK_idOC = '" + fkOrden2 + "'";
                   String crfactura = conexion.ValorEnVariable(crfact3);

                    int cantidadrestanteFact = Int32.Parse(crfactura);
                   cantidadrestanteFact = cantidadrestanteFact - cantidad;
               

                    //INSERTO VALOR NUEVO DE CANTIDAD RESTANTE
                  String updateCR = "UPDATE productos_has_ordencompra SET CrFactura = '" + cantidadrestanteFact + "' WHERE FK_idOC = '" + fkOrden2 + "'";
                   conexion.operaciones(updateCR);
                    cantidadrestanteFact = 0;

                   

                }

           
              

            }

            LoadDgvFactura();
        
        }
        
    }
}
