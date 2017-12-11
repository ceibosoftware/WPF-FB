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
using System.Windows.Shapes;

namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Interaction logic for windowAgregarClienteME.xaml
    /// </summary>
    public partial class windowAgregarClienteME : Window
    {
        public int idProducto;
        CRUD conexion = new CRUD();
        public windowAgregarClienteME(int idProveedor)
        {
            InitializeComponent();
            loadListaProducto(idProveedor);

        }
        public windowAgregarClienteME(int idProveedor, int idProducto)
        {
            InitializeComponent();
            loadListaProducto(idProveedor, idProducto);

        }

        public void loadListaProducto(int idProveedor)
        {
            String consulta = " Select p.nombre , p.idProductos from productos p inner join productos_has_proveedor t2 where t2.FK_idProveedor =" + idProveedor.ToString() + " and p.idProductos = t2.FK_idProductos";
            conexion.Consulta(consulta, ltsProductos);
            ltsProductos.DisplayMemberPath = "nombre";
            ltsProductos.SelectedValuePath = "idProductos";
            ltsProductos.SelectedIndex = 0;

        }
        public void loadListaProducto(int idProveedor, int idProducto)
        {
            String consulta = " Select p.nombre , p.idProductos from productos p inner join productos_has_proveedor t2 where t2.FK_idProveedor =" + idProveedor.ToString() + " and p.idProductos = t2.FK_idProductos";
            conexion.Consulta(consulta, ltsProductos);
            ltsProductos.DisplayMemberPath = "nombre";
            ltsProductos.SelectedValuePath = "idProductos";
            ltsProductos.SelectedIndex = 0;
            ltsProductos.SelectedValue = idProducto;
            
        }

   



        private void ltsProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            String consulta = "SELECT p.nombre , p.precioUnitario , p.idProductos from productos p where p.idProductos  = @valor";
           
            Console.WriteLine(ltsProductos.SelectedValue.ToString());
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


                
            decimal cantidad;
            decimal precioU;
            Decimal.TryParse(txtCantidad.Text, out cantidad);
            Decimal.TryParse(txtPrecioUnitario.Text, out precioU);
            txtTotal.Text = (cantidad * precioU).ToString();

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
            else
            {
                return true;
            }

        }

    }
}
