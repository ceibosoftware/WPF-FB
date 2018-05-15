﻿using System;
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
using wpfFamiliaBlanco.Proveedores;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for windowAgregarProducto.xaml
    /// </summary>
    /// 
    public partial class windowAgregarProducto : Window
    {

       
        private  List<elemento> items = new List<elemento>();
        private Boolean aceptar = false;
        CRUD conexion = new CRUD();
        public bool venta;
        public List<elemento> Items { get => items; set => items = value; }
        public bool Aceptar { get => aceptar; set => aceptar = value; }

        //protected override void OnClosing(System.ComponentModel.CancelEventArgs args) //sacar desenfoque
        //{
        //    var wW = GetWindow(this).Owner.Effect;
        //    base.OnClosing(args);
        //}

        public windowAgregarProducto()
        {
            InitializeComponent();
            LoadListaComboCategoria();
            LoadListaProveedor();
            
            LlenarComboFiltro();
            LoadListaProv();
           // newW.btnAgregar.Click += new EventHandler(this.MiBoton_Click);

        }

        public void LoadListaComboCategoria()
        {
            String consulta = "SELECT * FROM categorias";
            conexion.Consulta(consulta, combo: cmbCategoria);
            cmbCategoria.DisplayMemberPath = "nombre";
            cmbCategoria.SelectedValuePath = "idCategorias";
            cmbCategoria.SelectedIndex = 0;
        }

        private void LoadListaProveedorCategoria()
        {
            String consulta = "SELECT proveedor.nombre ,proveedor.idProveedor FROM proveedor,categorias_has_proveedor  WHERE categorias_has_proveedor.FK_idCategorias =  @valor and  proveedor.idProveedor = categorias_has_proveedor.FK_idProveedor;";
            ltsProveedores.ItemsSource = conexion.ConsultaParametrizada(consulta, cmbCategoria.SelectedValue).AsDataView();
            ltsProveedores.DisplayMemberPath = "nombre";
            ltsProveedores.SelectedValuePath = "idProveedor";

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
            try
            {
#pragma warning disable CS0219 // The variable 'provIndex' is assigned but its value is never used
                int provIndex = 0;
#pragma warning restore CS0219 // The variable 'provIndex' is assigned but its value is never used
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
            catch (NullReferenceException)
            {

                MessageBox.Show("Es necesario seleccionar un proveedor a agregar");
            }
           

        }

        private void btnProvEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Items.Remove(Items.Find(item => item.id == (int)ltsProvProductos.SelectedValue));
                ltsProvProductos.Items.Refresh();
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Es necesario seleccionar un proveedor a eliminar");
            }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (validar())
            {
                if ((bool)chkVenta.IsChecked)
                {
                    venta = true;
                }
                else
                {
                    venta = false;
                }
                Aceptar = true;
                this.Close();
            }
         
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    

        public bool validar()
        {

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Falta completar campo nombre");
                return false;
            }
            else if (string.IsNullOrEmpty(txtExistenciaMinima.Text))
            {
                MessageBox.Show("falta completar campo existencias minimas");
                return false;
            }
            else if (string.IsNullOrEmpty(txtUnidad.Text))
            {
                MessageBox.Show("falta completar campo unidad");
                return false;
            }
            else if (string.IsNullOrEmpty(txtPrecioUnitario.Text))
            {
                MessageBox.Show("falta completar campo precio unitario");
                return false;
            }
            else if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                MessageBox.Show("falta completar campo descripcion");
                return false;
            }
            else if (ltsProvProductos.Items.Count == 0)
            {
                MessageBox.Show("Es necesario ingresar algun proveedor");
                return false;
            }else if(cmbCategoria.SelectedItem == null)
            {
                MessageBox.Show("Es necesario seleccionar categoria");
                return false;
            }
            else
            {
                return true;
            }
          
        }

        private void btnCatNueva_Click(object sender, RoutedEventArgs e)
        {
            windowAgregarCategoria newW = new windowAgregarCategoria();
            newW.ShowDialog();

           if(newW.DialogResult == true)
           {
                LoadListaComboCategoria();
           }
        }

        private void txtNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z-ñ]"))
            {
                e.Handled = true;
            }
        }

   


        private void btnProvNuevo_Click(object sender, RoutedEventArgs e)
        {
            windowAgregarProveedor.lista.Clear();
            String idProv;
            Proveedores.windowAgregarProveedor newW2 = new Proveedores.windowAgregarProveedor();
            newW2.ShowDialog();
            if(newW2.DialogResult == true)
            {
                String nombre = newW2.txtNombre.Text;
                String cuit = newW2.txtCuit.Text;
                String razonSocial = newW2.cmbRazonSocial.Text;
                String direccion = newW2.txtDireccion.Text;
               // String categoria = newW2.cmbCategoria.Text;
                String codigoPostal = newW2.txtCP.Text;
                String localidad = newW2.txtLocalidad.Text;


                //INSERTAR DATOS PRINCIPALES
                String sql;
                sql = "insert into proveedor(nombre, razonSocial, cuit, codigoPostal, direccion, localidad) values('" + nombre + "', '" + razonSocial + "', '" + cuit + "', '" + codigoPostal + "', '" + direccion + "', '" + localidad + "');";
                conexion.operaciones(sql);


                String sql2 = "Select idProveedor from proveedor order by idProveedor DESC LIMIT 1";
                idProv = conexion.ValorEnVariable(sql2);

                Console.WriteLine("ULTIMO ID" + idProv);

                //INSERTAR CONTACTO PROVEEDOR
                String sqlContacto;

                string ultimoId = "Select last_insert_id()";
                String id = conexion.ValorEnVariable(ultimoId);
                for (int i = 0; i < Proveedores.windowAgregarProveedor.lista.Count; i++)
                {
                    String nombreL = Proveedores.windowAgregarProveedor.lista[i].NombreContacto;
                    String telefonoL = Proveedores.windowAgregarProveedor.lista[i].NumeroTelefono;
                    String emailL = Proveedores.windowAgregarProveedor.lista[i].Email;
                    sqlContacto = "insert into contactoproveedor(telefono, email, nombreContacto, FK_idProveedor) values('" + telefonoL + "', '" + emailL + "', '" + nombreL + "', '" + idProv + "');";
                    conexion.operaciones(sqlContacto);
                }
                // loadListaProducto();

                //INSERTAR CATEGORIAS PROVEEDOR


                for (int i = 0; i < newW2.Items.Count; i++)
                {
                    int idCategoria = newW2.Items[i].id;
                    string sql3 = "INSERT INTO categorias_has_proveedor(FK_idProveedor, FK_idCategorias) VALUES('" + id + "','" + idCategoria + "' )";
                    conexion.operaciones(sql3);
                }
                
               
            }
           // LoadListaProveedor(); //
            LoadListaProveedorCategoria();
        }

        private void cmbCategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Items.Clear();
            ltsProvProductos.Items.Refresh();        
            LoadListaProveedorCategoria();
        }

        private void txtExistenciaMinima_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
            
        }

        private void txtPrecioUnitario_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtUnidad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z-ñ]"))
            {
                e.Handled = true;
            }
        }

        private void txtPrecioUnitario_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
 }

