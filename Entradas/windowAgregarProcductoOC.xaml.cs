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

namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Interaction logic for windowAgregarClienteME.xaml
    /// </summary>
    public partial class windowAgregarClienteME : Window
    {
        int idOC;
        bool modifica = false;
        public int idProducto;
        CRUD conexion = new CRUD();
        public windowAgregarClienteME(int idProveedor, int moneda)
        {
            InitializeComponent();
            loadListaProducto(idProveedor, moneda);

        }
        public windowAgregarClienteME(int idProveedor, int idProducto, string nombre, int idOC,int moneda, bool modifica)
        {
            this.modifica = modifica;
            InitializeComponent();
            loadListaProducto(idProveedor, idProducto,nombre,moneda);
            this.idOC = idOC;
            if(modifica)
                lblPrecioUnitario.Content = "Precio unitario pagado";
        }
        public windowAgregarClienteME(int idProveedor, int idProducto,  int moneda, string nombre)
        {
           
            InitializeComponent();
            loadListaProducto(idProveedor, idProducto, nombre,moneda);
            
        }

        public void loadListaProducto(int idProveedor,int moneda)
        {
            String consulta = " Select p.nombre , p.idProductos from productos p inner join productos_has_proveedor t2 where t2.FK_idProveedor =" + idProveedor.ToString() + " and p.idProductos = t2.FK_idProductos and p.moneda = '"+moneda+"'";
            conexion.Consulta(consulta, ltsProductos);
            ltsProductos.DisplayMemberPath = "nombre";
            ltsProductos.SelectedValuePath = "idProductos";
            ltsProductos.SelectedIndex = 0;

        }
        public void loadListaProducto(int idProveedor, int idProducto,string nombre,int moneda)
        {
            String consulta = " Select p.nombre , p.idProductos from productos p inner join productos_has_proveedor t2 where t2.FK_idProveedor = @valor  and p.idProductos = t2.FK_idProductos and p.moneda ='"+moneda+"' ";
            ltsProductos.ItemsSource= conexion.ConsultaParametrizada(consulta, idProveedor).AsDataView() ;
            ltsProductos.DisplayMemberPath = "nombre";
            ltsProductos.SelectedValuePath = "idProductos";
            //BUSCAR ITEM EN LISTBOX
            for (int i = 0; i < ltsProductos.Items.Count; i++)
            {
                ltsProductos.SelectedIndex = i;
              
                if(ltsProductos.SelectedValue.ToString() == idProducto.ToString())
                {
                    break;
                }
            }
           
        }
           


   



        private void ltsProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            String consulta = "SELECT p.nombre , p.costo , p.idProductos from productos p where p.idProductos  = @valor";
           
          
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
                MessageBox.Show("No puede ingresar un número tan grande", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            
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
                MessageBox.Show("Total demasiado grande, vuelva a ingresar la cantidad","Error" ,MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            
        }
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
     
            if (validar())
            {
                compararPrecioUnitario();
                DialogResult = true;
            }
           
            
        }
        private void compararPrecioUnitario()
        {
            var resultado = MessageBoxResult.No;
            String consulta;
            if (modifica == false)
                consulta = "select p.costo from productos p where p.idProductos = " + ltsProductos.SelectedValue + " ";
            else
                consulta = "SELECT t1.PUPagado FROM productos_has_ordencompra t1 where FK_idOC = " + idOC + " and t1.FK_idProducto = " + ltsProductos.SelectedValue + " ";
          
            String valor = conexion.ValorEnVariable(consulta);
            float.TryParse(txtPrecioUnitario.Text, out float PU);
            float.TryParse(valor, out float PUOriginal);
            if (PUOriginal != PU)
            {
                if (!modifica)
                    resultado = MessageBox.Show("¿Quiere actualizar el precio unitario en el producto?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                else
                    resultado = MessageBoxResult.Yes;

                if (resultado == MessageBoxResult.Yes)
                {
                    if (modifica != true)
                        consulta = "UPDATE productos SET costo = '" + PU.ToString().Replace(",",".") + "' where idProductos = " + ltsProductos.SelectedValue + " ";
                    else
                        consulta = "UPDATE productos_has_ordencompra SET PUPagado = '" + PU.ToString().Replace(",", ".") + "' where FK_idOC = " + idOC + " and FK_idProducto = " + ltsProductos.SelectedValue + " ";

                    conexion.operaciones(consulta);
                }
            }
        }
        public bool validar()
        {
            int.TryParse(txtCantidad.Text, out int cantidad);
            Decimal.TryParse(txtPrecioUnitario.Text, out decimal PU);
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Falta completar campo nombre", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
              
                return false;
            }
            else if (string.IsNullOrEmpty(txtCantidad.Text) )
            {
                MessageBox.Show("falta completar campo cantidad", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            
                return false;
            }
            else if (string.IsNullOrEmpty(txtPrecioUnitario.Text))
            {
                MessageBox.Show("falta completar precio unitario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
             
                return false;
            }

            else if (string.IsNullOrEmpty(txtTotal.Text))
            {
                MessageBox.Show("falta completar campo total", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          
                return false;
            }
            else if (cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a cero", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
               
                return false;
            }
            else if (PU <= 0)
            {
                MessageBox.Show("El precio unitario no puede ser cero", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            
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

            Regex regex = new Regex("^[,][0-9]+$|^[0-9]*[,]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtPrecioUnitario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Decimal" || e.Key.ToString() == "OemPeriod")
            {
                e.Handled = true;
                txtPrecioUnitario.Text = txtPrecioUnitario.Text + ",";
                txtPrecioUnitario.SelectionStart = txtPrecioUnitario.Text.Length;
            }
        }
    }
}
