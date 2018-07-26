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
using wpfFamiliaBlanco.SalidasNuevo.Clases.OrdenesPedido;

namespace wpfFamiliaBlanco.SalidasNuevo.Vistas.OrdenesPedido
{
    /// <summary>
    /// Interaction logic for windowAgregarOP.xaml
    /// </summary>
    public partial class windowAgregarOP : Window
    {
        Cliente cliente = new Cliente();
        OrdenPedido opmi = new OrdenPedido();

        internal OrdenPedido OPMI { get => opmi; set => opmi = value; }

        public windowAgregarOP()
        {
            InitializeComponent();
            loadGeneral();
        }

        //metodos y funciones
       

        private void loadGeneral()
        {
            loadCmbClientes();
            loadDgvProductos();
            loadCmbIva();
            ColumnasDGVProductos();
            ColumnasDGVProductosMuestra();
            loadDPfecha();
        }
        private void loadDPfecha()
        {
            dpOPFecha.SelectedDate = DateTime.Now;
        }
        private void ColumnasDGVProductos()
        {

            dgvProductos.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("Nombre");
            dgvProductos.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio";
            textColumn2.Binding = new Binding("Precio");
            dgvProductos.Columns.Add(textColumn2);
            DataGridTextColumn textColumn3 = new DataGridTextColumn();
            textColumn3.Header = "Cajas";
            textColumn3.Binding = new Binding("Cajas");
            dgvProductos.Columns.Add(textColumn3);
            DataGridTextColumn textColumn4 = new DataGridTextColumn();
            textColumn4.Header = "Cajas por";
            textColumn4.Binding = new Binding("CajasPor");
            dgvProductos.Columns.Add(textColumn4);
            DataGridTextColumn textColumn5 = new DataGridTextColumn();
            textColumn5.Header = " % Descuento";
            textColumn5.Binding = new Binding("Descuento");
            dgvProductos.Columns.Add(textColumn5);
            DataGridTextColumn textColumn6 = new DataGridTextColumn();
            textColumn6.Header = "Descuento en Pesos";
            textColumn6.Binding = new Binding("DescuentoPesos");
            dgvProductos.Columns.Add(textColumn6);
            DataGridTextColumn textColumn8 = new DataGridTextColumn();
            textColumn8.Header = "Importe";
            textColumn8.Binding = new Binding("Importe");
            dgvProductos.Columns.Add(textColumn8);
            DataGridTextColumn textColumn9 = new DataGridTextColumn();
            textColumn9.Header = "INV ";
            textColumn9.Binding = new Binding("IdINV");
            dgvProductos.Columns.Add(textColumn9);

        }
        private void ColumnasDGVProductosMuestra()
        {

            dgvProductosMuestra.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("Nombre");
            dgvProductosMuestra.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio";
            textColumn2.Binding = new Binding("Precio");
            dgvProductosMuestra.Columns.Add(textColumn2);
            DataGridTextColumn textColumn3 = new DataGridTextColumn();
            textColumn3.Header = "Cajas";
            textColumn3.Binding = new Binding("Cajas");
            dgvProductosMuestra.Columns.Add(textColumn3);
            DataGridTextColumn textColumn4 = new DataGridTextColumn();
            textColumn4.Header = "Cajas por";
            textColumn4.Binding = new Binding("CajasPor");
            dgvProductosMuestra.Columns.Add(textColumn4);
            DataGridTextColumn textColumn5 = new DataGridTextColumn();
            textColumn5.Header = " % Descuento";
            textColumn5.Binding = new Binding("Descuento");
            dgvProductosMuestra.Columns.Add(textColumn5);
            DataGridTextColumn textColumn6 = new DataGridTextColumn();
            textColumn6.Header = "Descuento Pesos";
            textColumn6.Binding = new Binding("DescuentoPesos");
            dgvProductosMuestra.Columns.Add(textColumn6);
            DataGridTextColumn textColumn8 = new DataGridTextColumn();
            textColumn8.Header = "Importe";
            textColumn8.Binding = new Binding("Importe");
            dgvProductosMuestra.Columns.Add(textColumn8);
            DataGridTextColumn textColumn9 = new DataGridTextColumn();
            textColumn9.Header = "INV ";
            textColumn9.Binding = new Binding("IdINV");
            dgvProductosMuestra.Columns.Add(textColumn9);

        }
        private void loadCmbClientes()
        {
            try
            {

                DataTable clientes = cliente.getClientes();
                cmbClientes.ItemsSource = clientes.AsDataView();
                cmbClientes.DisplayMemberPath = "nombre";
                cmbClientes.SelectedValuePath = "idClientemi";
                cmbClientes.SelectedIndex = 0;
                setDatos(cmbClientes.SelectedValue.ToString());

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("No existen clientes de mercado interno");

            }

        }
        private void loadDgvProductos()
        {
            dgvProductos.ItemsSource = opmi.ProductosOP;
            //muestra
            dgvProductosMuestra.ItemsSource = opmi.ProductosMuestra;
        }
        private void loadCmbIva()
        {
            
            cmbTipoIVA.Items.Add((float)0);
            cmbTipoIVA.Items.Add((float)21);
            cmbTipoIVA.Items.Add((float)10.5);
            //Muestra
            cmbTipoIVAMuestra.Items.Add((float)0);
            cmbTipoIVAMuestra.Items.Add((float)21);
            cmbTipoIVAMuestra.Items.Add((float)10.5);

        }
        private void setDatos(string id)
        {
            opmi.clear();
           
            cliente.setDatos(id);
            txtTransporte.Text = cliente.Transporte;
            txtTelefonoTransporte.Text = cliente.TelefonoTransporte;
            txtDireccionEntrega.Text = cliente.DireccionEntrega;
            txtCuit.Text = cliente.Cuit;
            txtLista.Text = cliente.getLista();
           
        }
        private void clearDatos() {
            dgvProductos.Items.Refresh();
            txtSubtotal.Clear();
            txtTotalCajas.Clear();
            txtTotalBotellas.Clear();
            txtTotal.Clear();

            //Muestras
            dgvProductosMuestra.Items.Refresh();
            txtSubtotalMuestra.Clear();
            txtTotalCajasMuestra.Clear();
            txtTotalBotellasMuestra.Clear();
            txtTotalMuestra.Clear();
        }
        private void insertaProducto(ProductoOP producto)
        {
            opmi.AgregarProducto(producto);
            opmi.calculaSubtotal();
            opmi.calculaTotalcajas();
            opmi.calculaTotalBotellas();
            opmi.calculaTotal(cmbTipoIVA.SelectedIndex);
           
        }
        private void setTXT()
        {
            dgvProductos.Items.Refresh();
            txtSubtotal.Text = opmi.Subtotal.ToString();
            txtTotalCajas.Text = opmi.TotalCajas.ToString();
            txtTotalBotellas.Text = opmi.TotalBotellas.ToString();
            txtTotal.Text = opmi.Total.ToString();
           

        }
        private void descuento()
        {
            if (!String.IsNullOrEmpty(txtDescuento.Text) && !String.IsNullOrEmpty(txtTotal.Text))
                if (float.Parse(txtDescuento.Text) <= 100)
                    opmi.CalculaDescuento(float.Parse(txtDescuento.Text));
                else
                {
                    MessageBox.Show("El descuento no puede ser mayor a cien porciento");
                    txtDescuento.Text = "100";
                }
                    
            else
                opmi.calculaTotal(cmbTipoIVA.SelectedIndex);


            txtTotal.Text = opmi.Total.ToString();
        }
        private void setDatosOP()
        {
            opmi.IdCliente = (int)cmbClientes.SelectedValue;
            opmi.Fecha = (DateTime)dpOPFecha.SelectedDate;
            opmi.Notas = txtNotas.Text;
        }
        //metodos y funciones Muestra
        private void descuentoMuestra()
        {
            if (!String.IsNullOrEmpty(txtDescuentoMuestra.Text) && !String.IsNullOrEmpty(txtTotalMuestra.Text))
                opmi.CalculaDescuentoMuestra(float.Parse(txtDescuentoMuestra.Text));
            else
                opmi.calculaTotalMuestra(cmbTipoIVAMuestra.SelectedIndex);


            txtTotalMuestra.Text = opmi.TotalMuestra.ToString();
        }
        private void setTXTMuestra()
        {
            dgvProductosMuestra.Items.Refresh();
            txtSubtotalMuestra.Text = opmi.SubtotalMuestra.ToString();
            txtTotalCajasMuestra.Text = opmi.TotalCajasMuestra.ToString();
            txtTotalBotellasMuestra.Text = opmi.TotalBotellasMuestra.ToString();
            txtTotalMuestra.Text = opmi.TotalMuestra.ToString();


        }
        private void insertaProductoMuestra(ProductoOP producto)
        {
            opmi.AgregarProductoMuestra(producto);
            opmi.calculaSubtotalMuestra();
            opmi.calculaTotalcajasMuestra();
            opmi.calculaTotalBotellasMuestra();
            opmi.calculaTotalMuestra(cmbTipoIVAMuestra.SelectedIndex);

        }
        // Eventos
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarProductoOP((int)cmbClientes.SelectedValue);
            newW.ShowDialog();
            if (newW.DialogResult == true)
            {
                insertaProducto(newW.Producto);
                setTXT();
                descuento();
            }
        }
        private void cmbClientes_DropDownClosed(object sender, EventArgs e)
        {
            clearDatos();
            setDatos(cmbClientes.SelectedValue.ToString());

        }
        private void cmbTipoIVA_DropDownClosed(object sender, EventArgs e)
        {
            opmi.calculaTotal(cmbTipoIVA.SelectedIndex);
            setTXT();
            descuento();
        }
        private void txtDescuento_TextChanged(object sender, TextChangedEventArgs e)
        {
            descuento();
           
        }
        //Eventos muestra
        private void btnAgregarMuestra_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarProductoOP((int)cmbClientes.SelectedValue);
            newW.ShowDialog();
            if (newW.DialogResult == true)
            {
                insertaProductoMuestra(newW.Producto);
                setTXTMuestra();
                descuentoMuestra();
            }
        }
        private void cmbTipoIVAMuestra_DropDownClosed(object sender, EventArgs e)
        {
            opmi.calculaTotalMuestra(cmbTipoIVAMuestra.SelectedIndex);
            setTXTMuestra();
            descuentoMuestra();
        }
        private void txtDescuentoMuestra_TextChanged(object sender, TextChangedEventArgs e)
        {
            descuentoMuestra();
        }
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            
            if (opmi.Valida())
            {
                setDatosOP();
                DialogResult = true;
            }
        }
        private void btnTransporte_Click(object sender, RoutedEventArgs e)
        {
            var newW = new WindowTransporte(this.cliente);
            newW.ShowDialog();
            if(newW.DialogResult == true)
            {
                
                this.cliente = newW.Cliente;
                this.cliente.actualizarTransporte();
                setDatos(cmbClientes.SelectedValue.ToString());
            }
        }

        // eventos validaciones
     
        private void txtDescuento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[,]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }
    }
}