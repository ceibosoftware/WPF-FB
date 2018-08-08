using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wpfFamiliaBlanco.SalidasNuevo.Clases.INV;
using wpfFamiliaBlanco.SalidasNuevo.Clases.OrdenesPedido;

namespace wpfFamiliaBlanco.SalidasNuevo.Vistas.OrdenesPedido
{
    /// <summary>
    /// Interaction logic for windowAgregarProductoOP.xaml
    /// </summary>
    public partial class windowAgregarProductoOP : Window
    {
        ListaPrecio listaPrecio = new ListaPrecio();
        ProductoOP producto = new ProductoOP();
        Analisis inv = new Analisis();
        private bool cambiaPrecio ;
        private bool actualizaPrecio;

        internal ProductoOP Producto { get => producto; set => producto = value; }

        public windowAgregarProductoOP(int idCliente)
        {
            InitializeComponent();
            LoadGeneral(idCliente);
        }

        //metodos y funciones
        private void LoadGeneral(int idCliente)
        {
            LoadDgvLista(idCliente);
            SetColumnasDgvProductos();
            LoadCmbAnalisis();
            SetDatosAnalisis();
        }
        private void LoadDgvLista(int idCliente)
        {
            listaPrecio.IdCliente = idCliente;
            dgvProductos.ItemsSource = listaPrecio.GetLista(idCliente);
            lblProductos.Content = listaPrecio.Nombre;
        }
        private void LoadDgvLista(int idCliente, int idProducto)
        {
            int count = 0;
            listaPrecio.IdCliente = idCliente;
            dgvProductos.ItemsSource = listaPrecio.GetLista(idCliente);
            lblProductos.Content = listaPrecio.Nombre;
            foreach (var producto in listaPrecio.Productos)
            {
                if (producto.id == idProducto)
                    break;
                count++;
            }
            dgvProductos.SelectedIndex = count;


        }
        private void LoadCmbAnalisis()
        {
            cmbAnalisis.ItemsSource = Analisis.getAnalisis().AsDataView();
            cmbAnalisis.SelectedValuePath = "idAnalisis";
            cmbAnalisis.DisplayMemberPath = "nombrevino";
            cmbAnalisis.SelectedIndex = 0;
        }
        private void SetDatosAnalisis(){
            DataTable inv  = Analisis.getAnalisisID(cmbAnalisis.SelectedValue.ToString());
            txtCodigo.Text = inv.Rows[0].ItemArray[1].ToString();
            txtLitros.Text = inv.Rows[0].ItemArray[3].ToString();
        }
        private void SetColumnasDgvProductos()
        {
            dgvProductos.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvProductos.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio";
            textColumn2.Binding = new Binding("preciolista");
            dgvProductos.Columns.Add(textColumn2);

        }
        private void SetDatosProducto()
        {

            Producto producto = SeleccionarPoductoDGV();
            this.producto.SetStock(producto.id);
            this.producto.Precio = producto.preciolista;
            this.producto.Id = producto.id;
            setTXT(producto);
        }
        private Producto SeleccionarPoductoDGV()
        {

            Producto producto = dgvProductos.SelectedItem as Producto;
            return producto;
        }
        private int calculaTotalBotellas()
        {
            if (txtCajas.Text != "" && txtCajasPOR.Text != "")
                producto.CalculaTotalBotellas(int.Parse(txtCajas.Text), int.Parse(txtCajasPOR.Text));
            else
                producto.TotalBotellas = 0;

            return producto.TotalBotellas;
        }
        private float calculaImporte()
        {
            if (!String.IsNullOrEmpty(txtPrecio.Text) && !String.IsNullOrEmpty(txtBotellas.Text))
                producto.CalculaImporte(float.Parse(txtPrecio.Text), int.Parse(txtBotellas.Text));
            return producto.Importe;
        }
        private void limpiarTXT()
        {
            this.producto.clear();
            txtCajas.Text = "";
            txtCajasPOR.Text = "";
            txtBotellas.Text = "";
            txtImporte.Text = "";

        }
        private void setTXT(Producto producto)
        {
            txtExistencias.Text = this.producto.Stock.ToString();
            Producto.Nombre = producto.nombre;
            txtNombre.Text = producto.nombre;
            txtPrecio.Text = producto.preciolista.ToString();
        }
        private void setDescuento()
        {

            if (!String.IsNullOrEmpty(txtDescuento.Text) && !String.IsNullOrEmpty(txtImporte.Text))
            {
                if (float.Parse(txtDescuento.Text) <= 100)
                {
                    producto.CalculaDescuentoPesos(float.Parse(txtDescuento.Text), calculaImporte());
                    producto.aplicaDescuento();
                    txtDesuentoPesos.Text = producto.DescuentoPesos.ToString();
                    txtImporte.Text = producto.Importe.ToString();
                }
                else
                {
                    MessageBox.Show("El descuento no puede ser mayor a cien porciento");
                    txtDescuento.Text = "100";
                }
            }
            else
            {
                producto.Descuento = 0;
                producto.DescuentoPesos = 0;
                txtImporte.Text = calculaImporte().ToString();
                txtDesuentoPesos.Text = producto.DescuentoPesos.ToString();
            }
        }      
        private void actualizarPrecio()
        {
            if (!string.IsNullOrEmpty(txtPrecio.Text))
            {
                Producto producto = SeleccionarPoductoDGV();
                listaPrecio.actualizarPrecio(producto.id, float.Parse(txtPrecio.Text));
                LoadDgvLista(listaPrecio.IdCliente, producto.id);
                SetDatosProducto();
                calculaImporte();
                setDescuento();
               
            }
            else
            {
                MessageBox.Show("Ingrese un precio");
            }
        }
        public bool valida()
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Es necesario seleccionar un producto");
                return false;
            }
            else if (string.IsNullOrEmpty(txtPrecio.Text))
            {

                MessageBox.Show("Es necesario ingresar un precio");
                return false;
            }
            else if (float.Parse(txtPrecio.Text) <= 0)
            {
                MessageBox.Show("El precio debe ser mayor a cero");
                return false;
            }
            else if (string.IsNullOrEmpty(txtCajas.Text))
            {

                MessageBox.Show("Es necesario ingresar un numero de cajas");
                return false;
            }
            else if (float.Parse(txtCajas.Text) <= 0)
            {
                MessageBox.Show("las cajas deben ser mayor a cero");
                return false;
            }
            else if (string.IsNullOrEmpty(txtCajasPOR.Text))
            {

                MessageBox.Show("Es necesario ingresar un numero de botellas por caja");
                return false;
            }
            else if (float.Parse(txtCajasPOR.Text) <= 0)
            {
                MessageBox.Show("La cantidad de botellas por caja debe ser mayor a cero");
                return false;
            }
            else if (!string.IsNullOrEmpty(txtDescuento.Text))
            {
                if (float.Parse(txtDescuento.Text) <= 0)
                {
                    MessageBox.Show("El descuento debe ser mayor a cero");
                    return false;
                }

            }else if (cambiaPrecio && !actualizaPrecio)
            {
                MessageBox.Show("Precione actualizar precio para recalcular totales");
                return false;
            }
            return true;

        }


        //Eventos
        private void dgvProductos_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            limpiarTXT();
            SetDatosProducto();
        }
        private void txtCajas_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtBotellas.Text = calculaTotalBotellas().ToString();
            setDescuento();
        }
        private void txtCajasPOR_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtBotellas.Text = calculaTotalBotellas().ToString();
            setDescuento();
        }
        private void txtBotellas_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtImporte.Text = calculaImporte().ToString();
        }
        private void txtDescuento_TextChanged(object sender, TextChangedEventArgs e)
        {
            setDescuento();
        }
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (valida())
            producto.IdINV = (int)cmbAnalisis.SelectedValue;
            DialogResult = true;
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cambiaPrecio)
            {
                actualizarPrecio();
                calculaTotalBotellas();
                calculaImporte();
                cambiaPrecio = false;
                actualizaPrecio = true;
            }
            else
            {
               
                MessageBox.Show("No se ha cambiado el precio inicial");
            }
        }
        private void txtPrecio_TextChanged(object sender, TextChangedEventArgs e)
        {
            Producto p = SeleccionarPoductoDGV();
            if (String.Compare(p.preciolista.ToString(), txtPrecio.Text) != 0)
            {
                cambiaPrecio = true;
                actualizaPrecio = false;
            }
            else
            {
                cambiaPrecio = false;
                actualizaPrecio = true;
            }
        }
        private void cmbAnalisis_DropDownClosed(object sender, EventArgs e)
        {
            SetDatosAnalisis();
        }

        // validacion eventos
        private void txtCajas_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
        private void txtCajasPOR_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
        private void txtDescuento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[,]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }
        private void txtPrecio_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[,]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

      
    }
}
