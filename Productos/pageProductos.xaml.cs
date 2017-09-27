﻿using System;
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

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for pageProductos.xaml
    /// </summary>
    public partial class pageProductos : Page
    {
        CRUD conexion = new CRUD();

        public pageProductos()
        {

            InitializeComponent();
            loadListaProducto();
            LlenarComboFiltro();
           
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarProducto();
            newW.ShowDialog();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowModificarProducto();
            newW.ShowDialog();
        }

        private void loadListaProducto()
        {
            String consulta = " Select * from productos ";
            conexion.Consulta(consulta, ltsProductos);
            ltsProductos.DisplayMemberPath = "nombre";
            ltsProductos.SelectedValuePath = "idProductos";
        }

        private void ltsProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //consulta categoria, descripcion
                String consulta = "SELECT productos.nombre, productos.idProductos, productos.descripcion, categorias.nombre, categorias.idCategorias, productos.FK_idCategorias FROM productos , categorias WHERE idProductos = @valor AND productos.FK_idCategorias = categorias.idCategorias";
                DataTable productos = conexion.ConsultaParametrizada(consulta, ltsProductos.SelectedValue);
                txtDescripcion.Text = productos.Rows[0].ItemArray[2].ToString();
                txtCategoria.Text = productos.Rows[0].ItemArray[3].ToString();

                //consulta proveedores
                String consultaProveedores = "SELECT proveedor.nombre from proveedor , productos_has_proveedor WHERE productos_has_proveedor.FK_idProductos = @valor  AND productos_has_proveedor.FK_idProveedor = proveedor.idProveedor";
                DataTable proveedores = conexion.ConsultaParametrizada(consultaProveedores, ltsProductos.SelectedValue);
                ltsProveedores.ItemsSource = proveedores.AsDataView();
                ltsProveedores.DisplayMemberPath = "nombre";
            }
            catch
            {

            }

        }

        private void txtFiltro_GotMouseCapture(object sender, MouseEventArgs e)
        {
            txtFiltro.Text = "";
        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Busquedas de productos.
            DataTable productos = new DataTable();
            String consulta;

            if (cmbFiltro.Text == "Nombre")
            {   //Busca por nombre
                consulta = "SELECT* FROM productos WHERE productos.nombre LIKE '%' @valor '%'";
                productos = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            }
            else if (cmbFiltro.Text == "Categoria")
            {
                //busca por nombre de categoria (posibilidad de agregar combobox)
                consulta = "SELECT productos.nombre ,categorias.idCategorias FROM categorias , productos WHERE categorias.nombre LIKE  '%' @valor '%' and categorias.idCategorias = productos.FK_idCategorias";
                productos = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            }

            ltsProductos.ItemsSource = productos.AsDataView();

        }

        private void LlenarComboFiltro()
        {
             
           cmbFiltro.Items.Add("Nombre");
           cmbFiltro.Items.Add("Categoria");
        }
    }
}

