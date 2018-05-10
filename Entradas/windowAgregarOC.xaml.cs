using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Interaction logic for windowAgregarOC.xaml
    /// </summary>
    public partial class windowAgregarOC : Window
    {
        int idOC;
        public bool agregado = false;
        bool modifica = false;
        int contador = 0;
        public List<Producto> productos = new List<Producto>();
        public float subtotal;
        float total;
        public DateTime fecha;
        CRUD conexion = new CRUD();
        public List<Producto> Productos { get => Productos; set => productos = value; }

        public windowAgregarOC()
        {
            loadGeneral();
            cmbProveedores.SelectedIndex = 0;
            cmbDireccion.SelectedIndex = 0;
            cmbTelefono.SelectedIndex = 0;
            dpFecha.SelectedDate = DateTime.Now;
            ColumnasDGVProductos();
        }
        public windowAgregarOC(DateTime fecha, String observaciones, float subtotal, int iva, int tipoCambio, String formaPago, int telefono, int proveedor, int direccion, List<Producto> producto, int idOC)
        {
            modifica = true;
            this.productos = producto;
            loadGeneral();
            dpFecha.SelectedDate = fecha;
            txtObservaciones.Text = observaciones;
            this.subtotal = subtotal;
            txtSubtotal.Text = subtotal.ToString();
            cmbIVA.SelectedIndex = iva;
            cmbTipoCambio.SelectedIndex = tipoCambio;
            txtFormaPago.Text = formaPago;
            cmbTelefono.SelectedValue = telefono;
            cmbDireccion.SelectedValue = direccion;
            cmbProveedores.SelectedValue = proveedor;
            calculaTotal();
            this.idOC = idOC;
            //Cambios de Diseño batta
            lblWindowTitle.Content = "Modificar Orden de Compra"; 
            lblWindowTitle.Width = 176;
            ColumnasDGVProductos();
        }


        private void lblAgregarRemito_Copy_Click(object sender, RoutedEventArgs e)
        {

            var newW = new windowAgregarRemito();
            var resultado = MessageBox.Show("Desea agregar la orden de compra? ", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes)
            {
                newW.ShowDialog();
            }


        }

        private void btAgregarFactura_Copy_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarFactura();
            var resultado = MessageBox.Show("Desea agregar la orden de compra? ", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes)
            {
                newW.ShowDialog();
            }

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowCuotas();
            newW.ShowDialog();
        }

        private void btnAgregar_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (cmbProveedores.SelectedIndex != -1)
            {
                bool existe = false;
                var newW = new windowAgregarClienteME((int)cmbProveedores.SelectedValue);
                newW.ShowDialog();
                if (newW.DialogResult == true)
                {
                    int.TryParse(newW.txtCantidad.Text, out int cantidad);
                    float.TryParse(newW.txtTotal.Text, out float total);
                    float.TryParse(newW.txtPrecioUnitario.Text, out float precioU);
                    for (int i = 0; i < productos.Count; i++)
                    {
                        if (productos[i].nombre == newW.txtNombre.Text)
                        {
                            existe = true;
                            break;
                        }
                        else
                        {
                            existe = false;
                        }
                    }
                    if (!existe)
                    {
                        Producto p = new Producto(newW.txtNombre.Text, newW.idProducto, cantidad, total, precioU);
                        productos.Add(p);
                        loadDgvProductos();
                        dgvProductos.Items.Refresh();
                        float.TryParse(txtSubtotal.Text, out subtotal);
                        subtotal += p.total;
                        txtSubtotal.Text = (subtotal).ToString();
                        calculaTotal();
                    }
                    else
                    {
                        MessageBox.Show("El producto ya fue agregado a la orden de compra", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }





            }
            else
            {
                MessageBox.Show("Es necesario seleccionar un proveedor para agregar producto");
            }
        }


        public void LoadListaComboProveedor()
        {
            String consulta = "SELECT  distinct t1.nombre , t1.idProveedor FROM proveedor t1 inner join productos_has_proveedor t2 on t1.idProveedor = t2.FK_idProveedor";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";

        }
        public void LoadListaComboTelefonos()
        {
            String consulta = "SELECT * FROM telefonocontacto";
            conexion.Consulta(consulta, combo: cmbTelefono);
            cmbTelefono.DisplayMemberPath = "telefono";
            cmbTelefono.SelectedValuePath = "idTelefono";

        }
        public void LoadListaComboDireccion()
        {
            String consulta = "SELECT * FROM direcciones";
            conexion.Consulta(consulta, combo: cmbDireccion);
            cmbDireccion.DisplayMemberPath = "direccion";
            cmbDireccion.SelectedValuePath = "idDireccion";

        }

        private void LlenarCmbIVA()
        {
            cmbIVA.Items.Add((float)0);
            cmbIVA.Items.Add((float)21);
            cmbIVA.Items.Add((float)10.5);

        }

        private void LlenarCmbTipoCambio()
        {
            cmbTipoCambio.Items.Add("$");
            cmbTipoCambio.Items.Add("u$d");
            cmbTipoCambio.Items.Add("€");
        }
        private void loadDgvProductos()
        {
            dgvProductos.ItemsSource = productos;

        }


        private void cmbIVA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calculaTotal();

        }
        public void calculaTotal()
        {
            if (cmbIVA.SelectedIndex == 0)
            {
                txtTotal.Text = subtotal.ToString();
            }
            else if (cmbIVA.SelectedIndex == 1)
            {
                total = subtotal * (float)1.21;
                txtTotal.Text = total.ToString();
            }
            else if (cmbIVA.SelectedIndex == 2)
            {
                total = subtotal * (float)1.105;
                txtTotal.Text = total.ToString();
            }
        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Busquedas de proveedor.
            DataTable productos = new DataTable();
            String consulta;
            consulta = "SELECT * FROM proveedor WHERE proveedor.nombre LIKE '%' @valor '%'";
            productos = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            cmbProveedores.ItemsSource = productos.AsDataView();
            cmbProveedores.SelectedIndex = 0;
        }



        private void btnEliminar_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Producto prod = dgvProductos.SelectedItem as Producto;
                subtotal -= prod.total;
                calculaTotal();
                txtSubtotal.Text = (subtotal).ToString();
                productos.Remove(prod);
                dgvProductos.Items.Refresh();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("No se ha seleccionado ningun producto");
            }

        }

        private void btnModificar_Copy1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool existe = false;
                Producto prod = dgvProductos.SelectedItem as Producto;
                float.TryParse(txtSubtotal.Text, out subtotal);
                subtotal -= prod.total;
                var newW = new windowAgregarClienteME((int)cmbProveedores.SelectedValue, prod.id, prod.nombre);

                newW.txtCantidad.Text = prod.cantidad.ToString();

                newW.txtPrecioUnitario.Text = prod.precioUnitario.ToString();
                newW.txtNombre.Text = prod.nombre;
                newW.CalculaTotal();
                newW.ShowDialog();

                if (newW.DialogResult == true)
                {
                    int.TryParse(newW.txtCantidad.Text, out int cantidad);
                    float.TryParse(newW.txtTotal.Text, out float total);
                    float.TryParse(newW.txtPrecioUnitario.Text, out float precioU);
                    if (prod.nombre != newW.txtNombre.Text)
                    {
                        for (int i = 0; i < productos.Count; i++)
                        {
                            if (productos[i].nombre == newW.txtNombre.Text)
                            {
                                existe = true;
                                break;
                            }
                            else
                            {
                                existe = false;
                            }
                        }
                        if (!existe)
                        {
                            prod.cantidad = cantidad;

                            prod.total = total;
                            prod.precioUnitario = precioU;
                            prod.nombre = newW.txtNombre.Text;
                            prod.id = newW.idProducto;
                            dgvProductos.Items.Refresh();
                            subtotal += prod.total;
                            txtSubtotal.Text = (subtotal).ToString();
                            calculaTotal();
                        }
                        else
                        {
                            MessageBox.Show("El producto ya fue agregado a la orden de compra", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                    else
                    {
                        prod.cantidad = cantidad;
                        prod.total = total;
                        prod.precioUnitario = precioU;
                        prod.nombre = newW.txtNombre.Text;
                        prod.id = newW.idProducto;
                        dgvProductos.Items.Refresh();
                        subtotal += prod.total;
                        txtSubtotal.Text = (subtotal).ToString();
                        calculaTotal();
                    }
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("No se ha seleccionado ningun producto");
            }



        }

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (validar())
            {
                DialogResult = true;
            }
        }

        public bool validar()
        {

            if (productos.Count <= 0)
            {
                MessageBox.Show("Es necesario ingresar al menos un producto");
                return false;
            }
            else if (string.IsNullOrEmpty(dpFecha.Text))
            {
                MessageBox.Show("Es necesario seleccionar la fecha");
                return false;
            }
            else if (string.IsNullOrEmpty(txtFormaPago.Text))
            {
                MessageBox.Show("Falta completar la forma de pago");
                return false;
            }
            else if (string.IsNullOrEmpty(txtObservaciones.Text))
            {
                MessageBox.Show("Falta completa el campo observaciones");
                return false;
            }
            else
            {
                return true;
            }

        }

        private void dpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            fecha = dpFecha.SelectedDate.Value.Date;
        }

        private void loadGeneral()
        {
            InitializeComponent();
            loadDgvProductos();
            LoadListaComboProveedor();
            LlenarCmbIVA();
            LlenarCmbTipoCambio();
            LoadListaComboDireccion();
            LoadListaComboTelefonos();


        }

        private void cmbProveedores_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (contador > 2)
            {
                productos.Clear();
                dgvProductos.Items.Refresh();
                subtotal = 0;
                txtSubtotal.Text = subtotal.ToString();
                calculaTotal();
            }
            contador++;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRemito_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialog;

            if (validar())
            {
                string ultimoId;
                if (!agregado)
                {
                    dialog = MessageBox.Show("¿Desea agregar orden de compra?", "Agregar OC", MessageBoxButton.YesNo);
                }
                else
                {
                    dialog = MessageBoxResult.Yes;
                }
                if (dialog == MessageBoxResult.Yes)
                {
                    if (!agregado){
                        this.agregaOC();
                        agregado = true;
                        ultimoId = "Select last_insert_id()";
                        int idOr = int.Parse(conexion.ValorEnVariable(ultimoId));
                      
                    }
                    else
                    {
                        string consulta = "select idOrdenCompra from ordencompra order by idOrdenCompra desc LIMIT 1 ";
                        int idOr = int.Parse(conexion.ValorEnVariable(consulta));
                       
                    }
                    int idProveedor = (int)cmbProveedores.SelectedValue;

                    var newW = new windowAgregarRemito(idProveedor,idOC);

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
                         ultimoId = "Select last_insert_id()";

                        String id = conexion.ValorEnVariable(ultimoId);
                        foreach (var producto in newW.ProdRemito)
                        {
                            String productos = "insert into productos_has_remitos(cantidad,  FK_idProducto, FK_idRemito) values( '" + producto.cantidad + "', '" + producto.id + "','" + id + "' )";
                            conexion.operaciones(productos);
                        }
                        //ACTUALIZAR CANTITAD RESTANTE REMITO DE PRODUCTO OC
                        int idOrden = (int)newW.cmbOrden.SelectedValue;
                        foreach (var producto in newW.Productos)
                        {
                            String sql = "UPDATE productos_has_ordencompra SET CrRemito = '" + producto.cantidad + "' where FK_idProducto = '" + producto.id + "' and FK_idOC = '" + idOrden + "'";
                            conexion.operaciones(sql);
                        }

                    }
                }
            }
        }

        private String agregaOC()
        {
            int Proveedor = (int)this.cmbProveedores.SelectedValue;
            Console.WriteLine(Proveedor);
            DateTime fecha = this.fecha;
            fecha = Convert.ToDateTime(fecha.ToString("yyyy/MM/dd"));
            Console.WriteLine(fecha);
            decimal.TryParse(this.txtSubtotal.Text, out decimal subtotal);
            decimal.TryParse(this.txtTotal.Text, out decimal total);
            int direccion = (int)this.cmbDireccion.SelectedValue;
            int telefono = (int)this.cmbTelefono.SelectedValue;
            String observacion = this.txtObservaciones.Text;
            String formaPago = this.txtFormaPago.Text;
            int iva = this.cmbIVA.SelectedIndex;
            int tipoCambio = this.cmbTipoCambio.SelectedIndex;
            String sql = "insert into ordencompra(fecha, observaciones, subtotal, total, iva, tipoCambio ,formaPago, FK_idContacto,FK_idDireccion,FK_idProveedor) values( '" + fecha.ToString("yyyy/MM/dd") + "', '" + observacion + "', '" + subtotal + "', '" + total + "', '" + iva + "','" + tipoCambio + "','" + formaPago + "','" + telefono + "','" + direccion + "','" + Proveedor + "');";
            conexion.operaciones(sql);

            string ultimoId = "Select last_insert_id()";
            String idOC = conexion.ValorEnVariable(ultimoId);
            foreach (var producto in this.productos)
            {
                String productos = "insert into productos_has_ordencompra(cantidad, subtotal, Crfactura, CrRemito, FK_idProducto, FK_idOC,PUPagado) values( '" + producto.cantidad + "', '" + producto.total + "', '" + producto.cantidad + "', '" + producto.cantidad + "', '" + producto.id + "','" + idOC + "','" + producto.precioUnitario + "');";
                conexion.operaciones(productos);
            }
            return idOC;
        }

        private void btnFactura_Click(object sender, RoutedEventArgs e)
        {
            if (validar())
            {
                MessageBoxResult dialog;
              
                string fkOrden;
                if (!agregado)
                {
                    dialog = MessageBox.Show("¿Desea agregar orden de compra?", "Agregar OC", MessageBoxButton.YesNo);
                }
                else
                {
                    dialog = MessageBoxResult.Yes;
                }
                if (dialog == MessageBoxResult.Yes)
                {
                    if (!agregado)
                    {
                        fkOrden = this.agregaOC();
                        agregado = true;
                   

                    }
                    else
                    {
                        string consulta = "select idOrdenCompra from ordencompra order by idOrdenCompra desc LIMIT 1 ";
                        fkOrden = conexion.ValorEnVariable(consulta);

                    }
                    int idProveedor = (int)cmbProveedores.SelectedValue;


                
                    Console.WriteLine("idPRove : " + fkOrden);
                    agregado = true;
                  
                    var newW = new windowAgregarFactura(fkOrden,cmbProveedores.Text);
                    newW.ShowDialog();
           
                    
                    //INSERTO DATOS FACTURA
                    if (newW.DialogResult == true)
                    {
                        String idPRove = newW.cmbProveedores.SelectedValue.ToString();
                        Console.WriteLine("idPRove : " + idPRove);
                        decimal subtotal = decimal.Parse(newW.txtSubtotal.Text);
                        Console.WriteLine("subtotal : " + subtotal);
                        decimal total = decimal.Parse(newW.txtTotal.Text);
                        Console.WriteLine("total : " + total);
                        int numeroFact = int.Parse(newW.txtNroFactura.Text);
                        Console.WriteLine("numeroFact : " + numeroFact);
                        String iva = newW.cmbIVA.SelectedIndex.ToString();
                        Console.WriteLine("iva : " + iva);
                        String tipoCambio = newW.cmbTipoCambio.SelectedIndex.ToString();
                        Console.WriteLine("tipoCambio : " + tipoCambio);
                        String cuotas = newW.cmbCuotas.Text;
                        Console.WriteLine("cuotas : " + cuotas);
                     
                     
                        // fk orden de compra agregado
                      
                        DateTime dtp = System.DateTime.Now;
                        dtp = newW.dtFactura.SelectedDate.Value;

                        String sqlFactura = "INSERT INTO factura ( subtotal, numeroFactura, total, iva, tipoCambio, cuotas, FK_idOC, fecha )VALUES ('" + subtotal + "','" + numeroFact + "','" + total + "','" + iva + "','" + tipoCambio + "','" + cuotas + "','" + fkOrden + "','" + dtp.ToString("yyyy/MM/dd") + "')";
                        conexion.operaciones(sqlFactura);


                        //CREAR CONSULTA PARA TRAER ID FACTURA
                        int idProducto = newW.id;
                        string idOC = "Select last_insert_id()";
                        String idOrden = conexion.ValorEnVariable(idOC);


                        //inserto cuotas
                        foreach (Cuotas cuot in newW.todaslascuotas)
                        {

                            int id2 = cuot.cuota;
                            int dias = cuot.dias;
                            DateTime fecha = cuot.fechadepago;

                            String sqlProductoHas = "INSERT INTO cuotas ( dias, fecha, FK_idFacturas) VALUES ('" + dias + "', '" + fecha.ToString("yyyy/MM/dd") + "', '" + idOrden + "')";
                            conexion.operaciones(sqlProductoHas);

                        }
                      
                        //INSERTO LOS PRODUCTOS DE LA FACTURA
                        foreach (Producto p in newW.itemsFact)
                        {
                            String nombre = p.nombre;
                            int cantidad = p.cantidad;
                            float totalp = p.total;
                            float precioUni = p.precioUnitario;

                            String sqlProductoHas = "INSERT INTO productos_has_facturas (cantidad, subtotal, FK_idProducto, FK_idFactura) VALUES ('" + cantidad + "','" + subtotal + "', '" + idProducto + "', '" + idOrden + "')";
                            conexion.operaciones(sqlProductoHas);

                            //ACTUALIZAR CANTITAD RESTANTE REMITO DE PRODUCTO OC
                         
                            foreach (var producto in newW.items)
                            {
                                String sql = "UPDATE productos_has_ordencompra SET CrFactura = '" + producto.cantidad + "' where FK_idProducto = '" + producto.id + "' and FK_idOC = '" + fkOrden + "'";
                                conexion.operaciones(sql);
                            }
                        }
                    }
                }
            }


        }

        private void ColumnasDGVProductos()
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
            DataGridTextColumn textColumn3 = new DataGridTextColumn();
            textColumn3.Header = "Precio Unitario";
            textColumn3.Binding = new Binding("precioUnitario");
            dgvProductos.Columns.Add(textColumn3);
            DataGridTextColumn textColumn4 = new DataGridTextColumn();
            textColumn4.Header = "Subtotal";
            textColumn4.Binding = new Binding("total");
            dgvProductos.Columns.Add(textColumn4);

        }
    }

}

