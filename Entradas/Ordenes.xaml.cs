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
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpfFamiliaBlanco.Entradas;
//PARA PDF
using HandlebarsDotNet;
using IronPdf.Forms;
using IronPdf;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for Ordenes.xaml
    /// </summary>
    public partial class Ordenes : Page
    {


        DataTable productos;
        bool ejecutar = true;
        CRUD conexion = new CRUD();
        public Ordenes()
        {

            InitializeComponent();
            loadlistaOC();
            ejecutar = false;
            LoadListaComboProveedor();
            ejecutar = true;
            seleccioneParaFiltrar();
            ColumnasDGVProductos();


        }

        private void loadlistaOC()
        {
            String consulta = " Select * from ordencompra ";
            conexion.Consulta(consulta, tabla: ltsNumeroOC);
            ltsNumeroOC.DisplayMemberPath = "idOrdenCompra";
            ltsNumeroOC.SelectedValuePath = "idOrdenCompra";
            ltsNumeroOC.SelectedIndex = 0;

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
            cmbProveedores.SelectedIndex = -1;
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
            textColumn3.Binding = new Binding("PUPagado");
            dgvProductos.Columns.Add(textColumn3);
            DataGridTextColumn textColumn4 = new DataGridTextColumn();
            textColumn4.Header = "Subtotal";
            textColumn4.Binding = new Binding("subtotal");
            dgvProductos.Columns.Add(textColumn4);

        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {

            var moneda = new windowMonedaOC();
            moneda.ShowDialog();
            if (moneda.DialogResult == true)
            {
                var newW = new windowAgregarOC(moneda.moneda);
                if (newW.existeProveedor)
                {
                    newW.ShowDialog();



                    if (newW.DialogResult == true && !newW.agregado)
                    {
                        //INSERTAR OC
                        int Proveedor = (int)newW.cmbProveedores.SelectedValue;
                        Console.WriteLine(Proveedor);
                        DateTime fecha = newW.fecha;
                        fecha = Convert.ToDateTime(fecha.ToString("yyyy/MM/dd"));
                        Console.WriteLine(fecha);
                        String subtotal = newW.txtSubtotal.Text;
                        String total = newW.txtTotal.Text;
                        int direccion = (int)newW.cmbDireccion.SelectedValue;
                        int telefono = (int)newW.cmbTelefono.SelectedValue;
                        String observacion = newW.txtObservaciones.Text;
                        String formaPago = newW.txtFormaPago.Text;
                        int iva = newW.cmbIVA.SelectedIndex;
                        int tipoCambio = newW.cmbTipoCambio.SelectedIndex;
                        String cotizacion = newW.txtCotizacion.Text;
                        String sql = "insert into ordencompra(fecha, observaciones, subtotal, total, iva, tipoCambio ,formaPago, FK_idContacto,FK_idDireccion,FK_idProveedor,cotizacion) values( '" + fecha.ToString("yyyy/MM/dd") + "', '" + observacion + "', '" + subtotal + "', '" + total + "', '" + iva + "','" + tipoCambio + "','" + formaPago + "','" + telefono + "','" + direccion + "','" + Proveedor + "','" + cotizacion + "')";
                        conexion.operaciones(sql);
                        string ultimoId = "Select last_insert_id()";
                        String id = conexion.ValorEnVariable(ultimoId);
                        foreach (var producto in newW.productos)
                        {
                            String productos = "insert into productos_has_ordencompra(cantidad, subtotal, Crfactura, CrRemito, FK_idProducto, FK_idOC,PUPagado) values( '" + producto.cantidad + "', '" + producto.total + "', '" + producto.cantidad + "', '" + producto.cantidad + "', '" + producto.id + "','" + id + "','" + producto.precioUnitario + "');";
                            conexion.operaciones(productos);
                        }
                        MessageBox.Show("Se agregó la orden de compra correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    ejecutar = false;
                    loadlistaOC();
                    LoadListaComboProveedor();
                    ltsNumeroOC.Items.MoveCurrentToLast();
                    ejecutar = true;
                    seleccioneParaFiltrar();
                }
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
                //consulta s
                String consulta = "  SELECT t2.nombre , t1.cantidad,  t1.subtotal , t1.PUPagado from productos_has_ordencompra t1 inner join productos t2  on t1.FK_idProducto = t2.idProductos where t1.FK_idOC = @valor";
                productos = conexion.ConsultaParametrizada(consulta, ltsNumeroOC.SelectedValue);
                dgvProductos.ItemsSource = productos.AsDataView();
                //llenar datos de oc
                String consulta2 = "SELECT * FROM ordencompra t1 inner join proveedor where t1.idOrdenCompra = @valor and FK_idProveedor = idProveedor";
                DataTable OC = conexion.ConsultaParametrizada(consulta2, ltsNumeroOC.SelectedValue);
                DateTime fecha = (DateTime)OC.Rows[0].ItemArray[1];
                txtFecha.Text = fecha.ToString("dd/MM/yyyy");
                txtSubtotal.Text = OC.Rows[0].ItemArray[3].ToString();
                txtProveedor.Text = OC.Rows[0].ItemArray[14].ToString();
                if ((int)OC.Rows[0].ItemArray[5] == 0)
                {
                    txtIva.Text = "0";
                }
                else if ((int)OC.Rows[0].ItemArray[5] == 1)
                {
                    txtIva.Text = "21";
                }
                else
                {
                    txtIva.Text = "10,5";
                }

                txtFormaPago.Text = OC.Rows[0].ItemArray[7].ToString();
                //simbolo segun tipo cambio
                if ((int)OC.Rows[0].ItemArray[6] == 0)
                {
                    txtTipoCambio.Text = "$";
                }
                else if ((int)OC.Rows[0].ItemArray[6] == 1)
                {
                    txtTipoCambio.Text = "u$d";
                }
                else
                {
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
                    ejecutar = false;
                    String sql = "   Select distinct DATE_FORMAT(t1.fecha, '%d-%m-%Y') AS fecha from ordencompra t1 where t1.FK_idProveedor = @valor ";
                    DataTable fechas = conexion.ConsultaParametrizada(sql, cmbProveedores.SelectedValue);
                    cmbFechas.ItemsSource = fechas.AsDataView();
                    cmbFechas.DisplayMemberPath = "fecha";
                    cmbFechas.SelectedValuePath = "fecha";
                    cmbFechas.SelectedIndex = 0;
                    ejecutar = true;

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
        private void eliminarOC()
        {
            int idSeleccionado = (int)ltsNumeroOC.SelectedValue;
            string sql = "delete from ordencompra where idOrdenCompra = '" + idSeleccionado + "'";
            conexion.operaciones(sql);
            loadlistaOC();
        }
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRow selectedDataRow = ((DataRowView)ltsNumeroOC.SelectedItem).Row;
                string OC = selectedDataRow["idOrdenCompra"].ToString();
                MessageBoxResult dialog = MessageBox.Show("Esta seguro que desea eliminar la orden de compra número " + OC, "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                string existeRemito = "select count(idremitos) from remito where FK_idOC = " + OC + " ";
                string existeFactura = "select count(idFacturas) from factura where FK_idOC = " + OC + " ";
                if (dialog == MessageBoxResult.Yes)
                {
                    if (conexion.ValorEnVariable(existeRemito) != "0" && conexion.ValorEnVariable(existeFactura) != "0")
                    {
                        MessageBox.Show("No se puede eliminar la orden  tiene remitos y facturas asociados", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                    else if (conexion.ValorEnVariable(existeRemito) != "0")
                    {
                        MessageBox.Show("No se puede eliminar la orden  tiene remitos asociados", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                    else if (conexion.ValorEnVariable(existeFactura) != "0")
                    {
                        MessageBox.Show("No se puede eliminar la orden  tiene facturas asociadas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);


                    }
                    else
                    {
                        eliminarOC();
                        MessageBox.Show("Se elimino correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }


                if (ltsNumeroOC.Items.Count <= 0)
                {
                    txtDescripcion.Text = "";
                    txtFiltro.Text = "";
                    txtFormaPago.Text = "";
                    txtIva.Text = "";
                    txtSubtotal.Text = "";
                    txtTipoCambio.Text = "";
                    txtTotal.Text = "";
                    lblFechaOC.Content = "";
                }

            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione una orden de compra a eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnModificar_Copy_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                int idOC = (int)ltsNumeroOC.SelectedValue;
                int index = (int)ltsNumeroOC.SelectedIndex;
                string existeRemito = "select count(idremitos) from remito where FK_idOC = " + idOC + " ";
                string existeFactura = "select count(idFacturas) from factura where FK_idOC = " + idOC + " ";

                if (conexion.ValorEnVariable(existeRemito) == "0" && conexion.ValorEnVariable(existeFactura) == "0")
                {
                    //VALORES NECESARIOS PARA LLENAR CONSTRUCTOR
                    String consulta = "SELECT * FROM ordencompra where idOrdenCompra = @valor";
                    DataTable OC = conexion.ConsultaParametrizada(consulta, ltsNumeroOC.SelectedValue);
                    DateTime fecha = (DateTime)OC.Rows[0].ItemArray[1];
                    String observaciones = OC.Rows[0].ItemArray[2].ToString();
                    float subtotal = (float)OC.Rows[0].ItemArray[3];
                    int iva = (int)OC.Rows[0].ItemArray[5];
                    int tipoCambio = (int)OC.Rows[0].ItemArray[6];
                    String formaPago = OC.Rows[0].ItemArray[7].ToString();
                    int telefono = (int)OC.Rows[0].ItemArray[8];
                    int proveedor = (int)OC.Rows[0].ItemArray[10];
                    int direccion = (int)OC.Rows[0].ItemArray[9];
                    float cotizacion = (float)OC.Rows[0].ItemArray[12];

                    //PRODUCTOS DE LA ORDEN DE COMPRA
                    String consultaProductos = "SELECT t2.idProductos, t1.cantidad ,t1.subtotal,t2.nombre,t1.PUPagado FROM productos_has_ordencompra t1 inner join productos t2 where FK_idOC = @valor and t1.FK_idProducto = t2.idProductos";
                    DataTable productos = conexion.ConsultaParametrizada(consultaProductos, ltsNumeroOC.SelectedValue);
                    List<Producto> listaProd = new List<Producto>();


                    for (int i = 0; i < productos.Rows.Count; i++)
                    {

                        int idProducto = (int)productos.Rows[i].ItemArray[0];
                        int cantitad = (int)productos.Rows[i].ItemArray[1];
                        float sub = (float)productos.Rows[i].ItemArray[2];
                        String nombre = productos.Rows[i].ItemArray[3].ToString();
                        float PU = (float)productos.Rows[i].ItemArray[4];
                        listaProd.Add(new Producto(nombre, idProducto, cantitad, sub, PU));
                    }
                    var newW = new windowAgregarOC(fecha, observaciones, subtotal, iva, tipoCambio, formaPago, telefono, proveedor, direccion, listaProd, idOC, cotizacion);

                    newW.Title = "Modificar OC";
                    newW.ShowDialog();

                    if (newW.DialogResult == true)
                    {
                        //INSERTAR OC
                        int Proveedor = (int)newW.cmbProveedores.SelectedValue;
                        fecha = newW.fecha;
                        Console.WriteLine(fecha);
                        String sub = newW.txtSubtotal.Text;
                        String total = newW.txtTotal.Text;
                        direccion = (int)newW.cmbDireccion.SelectedValue;
                        telefono = (int)newW.cmbTelefono.SelectedValue;
                        observaciones = newW.txtObservaciones.Text;
                        formaPago = newW.txtFormaPago.Text;
                        iva = newW.cmbIVA.SelectedIndex;
                        tipoCambio = newW.cmbTipoCambio.SelectedIndex;
                        String sql = "UPDATE ordencompra SET fecha = '" + fecha.ToString("yyyy/MM/dd") + "', observaciones = '" + observaciones + "' ,subtotal = '" + sub + "',total = '" + total + "',iva = '" + iva + "',tipoCambio = '" + tipoCambio + "',formaPago = '" + formaPago + "',FK_idContacto = '" + telefono + "',FK_idDireccion = '" + direccion + "',FK_idProveedor = '" + Proveedor + "' WHERE ordencompra.idOrdenCompra = '" + idOC + "';";
                        conexion.operaciones(sql);

                        //ELIMINA REGISTRO DE TABLA INTERMEDIA
                        string sql2 = "delete  from productos_has_ordencompra where FK_idOC =  '" + idOC + "'";
                        conexion.operaciones(sql2);


                        foreach (var producto in newW.productos)
                        {
                            //string CantidadAntigua = "select cantidad from productos_has_ordencompra where FK_idOC = '" + idOC + "' and FK_idProducto =  '" + producto.id + "'";
                            //int.TryParse(conexion.ValorEnVariable(CrRemito), out int CRR);
                            //CRR = producto.cantidad - CRR;
                            String productosActualizar = "insert into productos_has_ordencompra(cantidad, subtotal, Crfactura, CrRemito, FK_idProducto, FK_idOC, PUPagado) values( '" + producto.cantidad + "', '" + producto.total + "', '" + producto.cantidad + "', '" + producto.cantidad + "', '" + producto.id + "','" + idOC + "','" + producto.precioUnitario + "');";
                            conexion.operaciones(productosActualizar);
                        }
                        ejecutar = false;
                        loadlistaOC(index);
                        LoadListaComboProveedor();
                        ejecutar = true;
                    }
                }
                else
                {
                    MessageBox.Show("No se puede modificar una orden que tiene remitos o facturas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione una orden a modificar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void cmbFechas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ejecutar)
            {

                String consulta = " Select * from ordencompra t1 where t1.fecha = @valor and FK_idProveedor = '" + cmbProveedores.SelectedValue + "'";
                DateTime fecha;
                DateTime.TryParse(cmbFechas.SelectedValue.ToString(), out fecha);
                fecha.ToString("yyyy-MM-dd");
                DataTable OCFecha = conexion.ConsultaParametrizada(consulta, fecha);
                ltsNumeroOC.ItemsSource = OCFecha.AsDataView();
                ltsNumeroOC.DisplayMemberPath = "idOrdenCompra";
                ltsNumeroOC.SelectedValuePath = "idOrdenCompra";
                ltsNumeroOC.SelectedIndex = 0;
            }
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
            cmbProveedores.SelectedIndex = -1;
            cmbFechas.SelectedIndex = -1;
            loadlistaOC();
            seleccioneParaFiltrar();

        }
        private void seleccioneParaFiltrar()
        {
            cmbProveedores.Text = "Seleccione para filtrar";
            cmbFechas.Text = "Seleccione para filtrar";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream("OC.pdf", FileMode.Create));
            doc.Open();
            var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);

            string imageURL = "D:\\Repositorio familia blanco\\WPF-FB\\logo.png";
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            jpg.Alignment = Element.ALIGN_CENTER;
            //Resize image depend upon your need
            jpg.ScaleToFit(140f, 120f);
            //Give space before image
            jpg.SpacingBefore = 10f;
            //Give some space after the image
            jpg.SpacingAfter = 1f;
            doc.Add(jpg);
            Paragraph proveedor = new Paragraph("Proveedor: " + txtProveedor.Text.ToString());
            Paragraph fecha = new Paragraph("Fecha: " + lblFecha.Content.ToString());
            Paragraph telefono = new Paragraph("Numero de contacto: 0303456 ");
            Paragraph Direccion = new Paragraph("Direccion de entrega: Guardia vieja 2314 ");   //buena mari
            Paragraph prod = new Paragraph("Productos de la orden \n \n");

            
            doc.Add(proveedor);
            doc.Add(fecha);
            doc.Add(telefono);
            doc.Add(Direccion);
            doc.Add(prod);
            PdfPTable table1 = new PdfPTable(1);
            table1.AddCell("Productos");
            PdfPTable table = new PdfPTable(4);

            table.AddCell("Cantidad");
            table.AddCell("Producto");
            table.AddCell("Precio Unitario");
            table.AddCell("Total");
            PdfPTable producto = new PdfPTable(4);
            for (int i = 0; i < this.productos.Rows.Count; i++)
            {
                producto.AddCell(productos.Rows[i].ItemArray[1].ToString());
                producto.AddCell(productos.Rows[i].ItemArray[0].ToString());
                producto.AddCell(productos.Rows[i].ItemArray[3].ToString());
                producto.AddCell(productos.Rows[i].ItemArray[2].ToString()+" "+ txtTipoCambio.Text );
            }
            doc.Add(table1);
            doc.Add(table);
            doc.Add(producto);
            Paragraph subtotal = new Paragraph("Subtotal: " + txtSubtotal.Text.ToString());
            Paragraph iva = new Paragraph("Iva: " + txtIva.Text.ToString());
            Paragraph total = new Paragraph("Total: " + txtTotal.Text.ToString());
            subtotal.IndentationLeft = 400f;
            iva.IndentationLeft = 400f;
            total.IndentationLeft = 400f;
            doc.Add(subtotal);
            doc.Add(iva);
            doc.Add(total);
            doc.Close();

    

        }
    }
}


