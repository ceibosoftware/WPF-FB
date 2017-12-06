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
       
        CRUD conexion = new CRUD();
        public windowAgregarClienteME()
        {
            InitializeComponent();
            loadListaProducto();
            
        }

        public void loadListaProducto()
        {
            String consulta = " Select * from productos ";
            conexion.Consulta(consulta, ltsProductos);
            ltsProductos.DisplayMemberPath = "nombre";
            ltsProductos.SelectedValuePath = "idProductos";

        }

        private void ltsProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            String consulta = "SELECT p.nombre , p.precioUnitario from productos p where p.idProductos  = @valor";
            DataTable productos = conexion.ConsultaParametrizada(consulta, ltsProductos.SelectedValue);
            txtNombre.Text = productos.Rows[0].ItemArray[0].ToString();
            txtPrecioUnitario.Text = productos.Rows[0].ItemArray[1].ToString();

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
    }
}
