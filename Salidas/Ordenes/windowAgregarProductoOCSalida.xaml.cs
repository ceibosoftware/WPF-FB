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

namespace wpfFamiliaBlanco.Salidas.Ordenes
{
    /// <summary>
    /// Interaction logic for windowAgregarProductoOCSalida.xaml
    /// </summary>
    public partial class windowsAgregarProductoOCSalida : Window
    {
        

        int idOC;
        bool modifica = false;
        public int idProducto;
        string idLista;
        int cliente;
        CRUD conexion = new CRUD();
        public windowsAgregarProductoOCSalida(int idProveedor, int cliente)
        {
            InitializeComponent();
            ListaCliente(idProveedor, cliente);
            loadListaProducto(idProveedor,cliente);
            

        }
        public windowsAgregarProductoOCSalida(int idProveedor, int idProducto, string nombre, int idOC, int cliente)
        {
            modifica = true;
            InitializeComponent();
            ListaCliente(idProveedor, cliente);
            loadListaProducto(idProveedor, idProducto,nombre,cliente);
            this.idOC = idOC;
            lblPrecioUnitario.Content = "Precio unitario pagado";
            modifica = false;
        }
        public windowsAgregarProductoOCSalida(int idProveedor, int idProducto, string nombre)
        {

            InitializeComponent();
            loadListaProducto(idProveedor, idProducto,nombre,cliente);

        }

        public void loadListaProducto(int idProveedor, int cliente)
        {
            String consulta;
            if (cliente == 1)
            {
               consulta = "Select p.nombre , p.idProductos from productos p , productos_has_listadeprecios t2 , clientesMI MI where MI.idClientemi = "+ idProveedor + "  and MI.FK_idLista = t2.FK_idLista and t2.FK_idProductos = p.idProductos";
            }
            else
            {
               consulta = " Select p.nombre , p.idProductos from productos p , productos_has_listadeprecios t2 , clientesME ME where ME.idClienteme = " + idProveedor + "  and ME.FK_idLista = t2.FK_idLista and t2.FK_idProductos = p.idProductos";
            }
            conexion.Consulta(consulta, ltsProductos);
            ltsProductos.DisplayMemberPath = "nombre";
            ltsProductos.SelectedValuePath = "idProductos";
            ltsProductos.SelectedIndex = 0;

        }
        private void ListaCliente(int idCliente, int tipoCliente)
        {
            string consulta;
            
            if (tipoCliente == 1)
            {
                consulta = "select FK_idLista from clientesMI where idClienteMI = "+idCliente+"";
            }
            else
            {
                consulta = "select FK_idLista from clientesME where idClienteME = " + idCliente + "";
            }
            idLista = conexion.ValorEnVariable(consulta);
           
        }
        public void loadListaProducto(int idProveedor, int idProducto, string nombre, int cliente)
        {
            MessageBox.Show(idProveedor.ToString());
            String consulta;
            if (cliente == 1)
            {
                consulta = "Select p.nombre , p.idProductos from productos p , productos_has_listadeprecios t2 , clientesMI MI where MI.idClientemi = " + idProveedor + "  and MI.FK_idLista = t2.FK_idLista and t2.FK_idProductos = p.idProductos";
            }
            else
            {
                consulta = " Select p.nombre , p.idProductos from productos p , productos_has_listadeprecios t2 , clientesME ME where ME.idClienteme = " + idProveedor + "  and ME.FK_idLista = t2.FK_idLista and t2.FK_idProductos = p.idProductos";
            }
            conexion.Consulta(consulta, ltsProductos);
            ltsProductos.DisplayMemberPath = "nombre";
            ltsProductos.SelectedValuePath = "idProductos";
            
            MessageBox.Show(ltsProductos.Items.Count.ToString());
            ////BUSCAR ITEM EN LISTBOX
            for (int i = 0; i < ltsProductos.Items.Count; i++)
            {
                ltsProductos.SelectedIndex = i;

                if (ltsProductos.SelectedValue.ToString() == idProducto.ToString())
                {
                    break;
                }
            }

        }







        private void ltsProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

                String consulta = "SELECT p.nombre, lp.precioLista , p.idProductos from productos p inner join productos_has_listadeprecios lp where p.idProductos  = @valor and lp.FK_idLista = " + idLista + " and p.idProductos = lp.FK_idProductos  ";
               

                DataTable productos = conexion.ConsultaParametrizada(consulta, ltsProductos.SelectedValue);

                txtNombre.Text = productos.Rows[0].ItemArray[0].ToString();
                txtPrecioUnitario.Text = productos.Rows[0].ItemArray[1].ToString();
                idProducto = (int)productos.Rows[0].ItemArray[2];
                if (txtCantidad.Text != "")
                {
                    txtTotal.Text = 0.ToString();
                    txtCantidad.Text = 0.ToString();
                }
            
        }

        private void txtCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                CalculaTotal();
            }
            catch (OverflowException)
            {

                MessageBox.Show("No puede ingresar un numero tan grande");
            }



        }
        public void CalculaTotal()
        {
            try
            {
                decimal cantidad;
                decimal precioU;
                decimal.TryParse(txtCantidad.Text, out cantidad);
                decimal.TryParse(txtPrecioUnitario.Text, out precioU);
                txtTotal.Text = (cantidad * precioU).ToString();
            }
            catch (OverflowException)
            {
                txtCantidad.Text = "0";
                txtCantidad.Focus();
                MessageBox.Show("Total demaciado grande , vueva a ingresar cantidad", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);

            }

        }
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {

            if (validar())
            {
                compararPrecioLista();
                DialogResult = true;
            }


        }
        private void compararPrecioLista()
        {
            //var resultado = MessageBoxResult.No;
            //String consulta;
            //if (modifica == false)
            //    consulta = "select p.precioUnitario from productos p where p.idProductos = " + ltsProductos.SelectedValue + " ";
            //else
            //    consulta = "SELECT t1.PUPagado FROM productos_has_ordencompra t1 where FK_idOC = " + idOC + " and t1.FK_idProducto = " + ltsProductos.SelectedValue + " ";           
            //String valor = conexion.ValorEnVariable(consulta);
            //float.TryParse(txtPrecioUnitario.Text, out float PU);
            //float.TryParse(valor, out float PUOriginal);
            //if (PUOriginal != PU)
            //{
            //    if (!modifica)
            //        resultado = MessageBox.Show("¿Quiere actualizar el precio unitario en el producto?", "Actualizar PU", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //    else
            //        resultado = MessageBoxResult.Yes;

            //    if (resultado == MessageBoxResult.Yes)
            //    {
            //        if (modifica != true)
            //            consulta = "UPDATE productos SET precioUnitario = '" + PU + "' where idProductos = " + ltsProductos.SelectedValue + " ";
            //        else
            //            consulta = "UPDATE productos_has_ordencompra SET PUPagado = '" + PU + "' where FK_idOC = " + idOC + " and FK_idProducto = " + ltsProductos.SelectedValue + " ";

            //        conexion.operaciones(consulta);
            //    }
            //}
        }
        public bool validar()
        {
            int.TryParse(txtCantidad.Text, out int cantidad);
            Decimal.TryParse(txtPrecioUnitario.Text, out decimal PU);
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Falta completar campo nombre");
                return false;
            }
            else if (string.IsNullOrEmpty(txtCantidad.Text))
            {
                MessageBox.Show("falta completar campo cantidad");
                return false;
            }
            else if (string.IsNullOrEmpty(txtPrecioUnitario.Text))
            {
                MessageBox.Show("falta completar precio unitario");
                return false;
            }

            else if (string.IsNullOrEmpty(txtTotal.Text))
            {
                MessageBox.Show("falta completar campo total");
                return false;
            }
            else if (cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a cero");
                return false;
            }
            else if (PU <= 0)
            {
                MessageBox.Show("El precio unitario no puede ser cero");
                return false;
            }
            else
            {
                return true;
            }

        }

        private void txtCantidad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void txtPrecioUnitario_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculaTotal();
        }

        private void txtPrecioUnitario_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }

            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

