using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using wpfFamiliaBlanco.Proveedores;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for pageProveedores.xaml
    /// </summary>
    public partial class pageProveedores : Page
    {

        CRUD conexion = new CRUD();

        public pageProveedores()
        {
            InitializeComponent();
            loadListaProveedores();

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e) //btnModificarProveedor_Click
        {
            var newW = new windowModificarProveedor();
            newW.ShowDialog();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarProveedor();
            newW.ShowDialog();
        }

        
        private void loadListaProveedores()
        {
            String consulta = "SELECT * FROM Proveedor";
            conexion.Consulta(consulta, ltsProveedores);
            ltsProveedores.DisplayMemberPath = "nombre";
            ltsProveedores.SelectedValuePath = "idProveedor";

        }

        private void ltsProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                //consulta datosproveedor
                String consulta = "SELECT * from proveedor WHERE idProveedor = @valor";
                DataTable proveedor = conexion.ConsultaParametrizada(consulta, ltsProveedores.SelectedValue);
                txtCuit.Text = proveedor.Rows[0].ItemArray[3].ToString();
                txtDireccion.Text = proveedor.Rows[0].ItemArray[5].ToString();
                txtRazonSocial.Text = proveedor.Rows[0].ItemArray[2].ToString();
                txtLocalidad.Text = proveedor.Rows[0].ItemArray[6].ToString();
                txtCP.Text = proveedor.Rows[0].ItemArray[4].ToString();

                //consulta contacto
                String consultaContacto = "SELECT contactoproveedor.telefono, contactoproveedor.email, contactoproveedor.nombreContacto from contactoproveedor WHERE FK_idProveedor=@valor";
                DataTable contacto = conexion.ConsultaParametrizada(consultaContacto, ltsProveedores.SelectedValue);
                dgvContacto.ItemsSource = contacto.AsDataView();

                //consulta categorias
                String consultaCategoria = "SELECT categorias.nombre from categorias , categorias_has_proveedor WHERE categorias_has_proveedor.FK_idProveedor = @valor AND categorias_has_proveedor.FK_idCategorias=categorias.idCategorias";
                DataTable categorias = conexion.ConsultaParametrizada(consultaCategoria, ltsProveedores.SelectedValue);
                ltsCategorias.ItemsSource = categorias.AsDataView();
                ltsCategorias.DisplayMemberPath = "nombre";


            }
            catch
            {

            }
        }
    }
}
