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

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for windowModificarProducto.xaml
    /// </summary>
    public partial class windowModificarProducto : Window
    {
        CRUD conexion = new CRUD();
        public windowModificarProducto()
        {
            InitializeComponent();
            LoadListaComboCategoria();
            LoadListaProveedor();
            cmbCategoria.SelectedIndex = 0;
            LlenarComboFiltro();
        }
        private void LoadListaComboCategoria()
        {
            String consulta = "SELECT * FROM categorias";
            conexion.Consulta(consulta, combo: cmbCategoria);
            cmbCategoria.DisplayMemberPath = "nombre";
            cmbCategoria.SelectedValuePath = "idCategorias";

        }

        private void txtBuscar_GotMouseCapture(object sender, MouseEventArgs e)
        {
            txtCategoria.Text = "";
        }

        
        private void LoadListaProveedor()
        {
            String consulta = " Select * from proveedor ";
            conexion.Consulta(consulta, ltsProveedores);
            ltsProveedores.DisplayMemberPath = "nombre";
            ltsProveedores.SelectedValuePath = "idProveedor";
        }


        private void LlenarComboFiltro()
        {
            cmbFiltro.Items.Add("Nombre");
            cmbFiltro.Items.Add("Categoria");
        }

        private void txtBuscar_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            string consulta;
            DataTable categorias = new DataTable();

            //Busca por nombre
            consulta = "SELECT * FROM categorias WHERE categorias.nombre LIKE '%' @valor '%'";
            categorias = conexion.ConsultaParametrizada(consulta, txtBuscar.Text);
            cmbCategoria.ItemsSource = categorias.AsDataView();
            cmbCategoria.SelectedIndex = 0;
        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Busquedas de productos.
            DataTable productos = new DataTable();
            String consulta;

            if (cmbFiltro.Text == "Nombre")
            {   //Busca por nombre
                consulta = "SELECT * FROM proveedor WHERE proveedor.nombre LIKE '%' @valor '%'";
                productos = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            }
            else if (cmbFiltro.Text == "Categoria")
            {
                //busca por nombre de categoria (posibilidad de agregar combobox)
                consulta = "SELECT proveedor.nombre ,categorias.idCategorias FROM categorias , proveedor, categorias_has_proveedor WHERE categorias.nombre LIKE '%' @valor '%' and categorias.idCategorias = categorias_has_proveedor.FK_idCategorias";
                productos = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            }

            ltsProveedores.ItemsSource = productos.AsDataView();
           

        }
    }
}
