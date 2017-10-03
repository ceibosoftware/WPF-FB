﻿using System;
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
    /// Interaction logic for windowAgregarProducto.xaml
    /// </summary>
    /// 
    public partial class windowAgregarProducto : Window
    {
        public class elemento
        {
            public string nombre { get; set; }
            public int id { get; set; }
            public elemento(string nombre, int id)
            {
                this.nombre = nombre;
                this.id = id;
            }
        }

        public List<elemento> Items { get; set; } = new List<elemento>();
        public Boolean aceptar = false;
        CRUD conexion = new CRUD();

        public windowAgregarProducto()
        {
            InitializeComponent();
            LoadListaComboCategoria();
            LoadListaProveedor();
            cmbCategoria.SelectedIndex = 0;
            LlenarComboFiltro();
            LoadListaProv();

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
            txtNombre.Text = "";
        }

        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string consulta;
            DataTable categorias = new DataTable();

            //Busca por nombre
            consulta = "SELECT * FROM categorias WHERE categorias.nombre LIKE '%' @valor '%'";
            categorias = conexion.ConsultaParametrizada(consulta, txtBuscar.Text);
            cmbCategoria.ItemsSource = categorias.AsDataView();
            cmbCategoria.SelectedIndex = 0;
        }
        private void LoadListaProveedor()
        {

            String consulta = " Select * from proveedor ";
            conexion.Consulta(consulta, ltsProveedores);
            ltsProveedores.DisplayMemberPath = "nombre";
            ltsProveedores.SelectedValuePath = "idProveedor";
        }

        private void LoadListaProv()
        {
            ltsProvProductos.ItemsSource = Items;
            ltsProvProductos.DisplayMemberPath = "nombre";
            ltsProvProductos.SelectedValuePath = "id";
        }
        private void LlenarComboFiltro()
        {
            cmbFiltro.Items.Add("Nombre");
            cmbFiltro.Items.Add("Categoria");
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

        private void btnProvAgregar_Click(object sender, RoutedEventArgs e)
        {
            int provIndex = 0;
            Boolean existe = false;
            DataRow selectedDataRow = ((DataRowView)ltsProveedores.SelectedItem).Row;

            if (ltsProvProductos.Items.Count <= 0)
            {
                Items.Add(new elemento(selectedDataRow["nombre"].ToString(), (int)ltsProveedores.SelectedValue));
                ltsProvProductos.Items.Refresh();
            }
            else
            {
                for (int i = 0; i < ltsProvProductos.Items.Count; i++)
                {

                    if (selectedDataRow["nombre"].ToString().CompareTo(Items[i].nombre) != 0)
                    {
                        existe = false;

                    }
                    else
                    {
                        existe = true;
                        break;
                    }
                }
                if (!existe)
                {

                    Items.Add(new elemento(selectedDataRow["nombre"].ToString(), (int)ltsProveedores.SelectedValue));
                    ltsProvProductos.Items.Refresh();

                   
                        Console.WriteLine("elementos" + Items.Count);
                    


                }
                else
                {
                    MessageBox.Show("Ese proveedor ya fue agregado");
                }
            }

        }

        private void btnProvEliminar_Click(object sender, RoutedEventArgs e)
        {
            Items.Remove(Items.Find(item => item.id == (int)ltsProvProductos.SelectedValue));
            ltsProvProductos.Items.Refresh();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            aceptar = true;
            this.Close();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    
    }
}