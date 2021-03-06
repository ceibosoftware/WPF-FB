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
    /// Interaction logic for windowModificarProducto.xaml
    /// </summary>
    public partial class windowModificarProducto : Window
    {

        private List<elemento> items = new List<elemento>();
        private Boolean aceptar = false;
        CRUD conexion = new CRUD();
        public bool venta;
        public bool Aceptar { get => aceptar; set => aceptar = value; }
        public List<elemento> Items { get => items; set => items = value; }

        public windowModificarProducto()
        {

            InitializeComponent();
            LoadListaComboCategoria();           
            LlenarlistaProveedores();

        }
        public windowModificarProducto(int cmbValue, string nombre, string descripcion, List<elemento> items, float existencia, String unidad , float precioUnitario, bool venta, int moneda, float costo)
        {
        
            InitializeComponent();
         
            LoadListaComboCategoria();
           
            cmbCategoria.SelectedValue = cmbValue;
            txtDescripcion.Text = descripcion;
            txtNombre.Text = nombre;
            txtExistenciaMinima.Text = existencia.ToString();
            txtPrecioUnitario.Text = precioUnitario.ToString();
            txtUnidad.Text = unidad;
            loadCotizacion();       
            LlenarCmbTipoCambio();
            cmbMoneda1.SelectedIndex = moneda;
            txtCosto.Text = costo.ToString();
            if (venta)
            {
                chkVenta.IsChecked = true;
            }
            else
            {
                chkVenta.IsChecked = false;
            }
            llenarProveedoresExistentes(items);
            LoadListaProv();
            LoadListaProveedorCategoria();
            // LlenarlistaProveedores();
        }
        private void llenarProveedoresExistentes(List<elemento> lista) {
            foreach (var item in lista)
            {
               
                this.Items.Add(item);
            }

            
        }
        private void LoadListaComboCategoria()
        {
            String consulta = "SELECT * FROM categorias ,  categorias_has_proveedor c where c.FK_idCategorias = categorias.idCategorias  ";
            conexion.Consulta(consulta, combo: cmbCategoria);
            cmbCategoria.DisplayMemberPath = "nombre";
            cmbCategoria.SelectedValuePath = "idCategorias";
            cmbCategoria.SelectedIndex = 1;
            cmbCategoria.SelectedIndex = 0;
        }

        private void txtBuscar_GotMouseCapture(object sender, MouseEventArgs e)
        {
            txtNombre.Text = "";
        }


        private void LoadListaProveedor()
        {
            String consulta = " Select * from proveedor ";
            conexion.Consulta(consulta, ltsProveedores);
            ltsProveedores.DisplayMemberPath = "nombre";
            ltsProveedores.SelectedValuePath = "idProveedor";

        }
        private void LoadListaProveedorCategoria()
        {
            String consulta = "SELECT proveedor.nombre ,proveedor.idProveedor FROM proveedor,categorias_has_proveedor  WHERE categorias_has_proveedor.FK_idCategorias =  @valor and  proveedor.idProveedor = categorias_has_proveedor.FK_idProveedor;";
            ltsProveedores.ItemsSource = conexion.ConsultaParametrizada(consulta, cmbCategoria.SelectedValue).AsDataView();
            ltsProveedores.DisplayMemberPath = "nombre";
            ltsProveedores.SelectedValuePath = "idProveedor";
           

        }


        private void LlenarlistaProveedores()
        {
            String consulta = " Select * from proveedor ";
            conexion.Consulta(consulta, ltsProveedores);
            ltsProveedores.DisplayMemberPath = "nombre";
            ltsProveedores.SelectedValuePath = "idProveedor";
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

             //Busca por nombre
                consulta = "SELECT * FROM proveedor WHERE proveedor.nombre LIKE '%' @valor '%'";
                productos = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
       
          
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
                    Console.WriteLine("cantidad " + ltsProvProductos.Items.Count);
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
                        MessageBox.Show("Ese proveedor ya fue agregado", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un producto a modificar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

                MessageBox.Show("Seleccione proveedor a eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadListaProv()
        {
            

            ltsProvProductos.ItemsSource = Items;
            ltsProvProductos.DisplayMemberPath = "nombre";
            ltsProvProductos.SelectedValuePath = "id";

           
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
                MessageBox.Show("Falta completar campo nombre", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          
                return false;
            }
            else if (string.IsNullOrEmpty(txtExistenciaMinima.Text))
            {
                MessageBox.Show("Falta completar campo existencias minimas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
  
                return false;
            }
            else if (string.IsNullOrEmpty(txtUnidad.Text))
            {
                MessageBox.Show("Falta completar campo unidad", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
 
                return false;
            }
            else if (string.IsNullOrEmpty(txtPrecioUnitario.Text))
            {
                MessageBox.Show("Falta completar campo precio unitario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
     
                return false;
            }
            else if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                MessageBox.Show("Falta completar campo descripción", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
            else if (ltsProvProductos.Items.Count == 0)
            {
                MessageBox.Show("Es necesario ingresar algun proveedor", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          
                return false;
            }
            else
            {
                return true;
            }

        }

        private void txtNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
        
        }

       

        private void btnCatNueva_Click(object sender, RoutedEventArgs e)
        {
            windowAgregarCategoria newW = new windowAgregarCategoria();
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {
                LoadListaComboCategoria();
            }
        }

        private void btnProvNuevo_Click(object sender, RoutedEventArgs e)
        {
            windowAgregarProveedor.lista.Clear();
            String idProv;
            Proveedores.windowAgregarProveedor newW2 = new Proveedores.windowAgregarProveedor();
            newW2.ShowDialog();
            if (newW2.DialogResult == true)
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
            LoadListaProveedorCategoria(); //
            LoadListaComboCategoria();
        }

        private void cmbCategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            //LoadListaProveedorCategoria();
            //Items.Clear();
            //ltsProvProductos.Items.Refresh();

        }

        private void txtUnidad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z-ñ]"))
            {
                e.Handled = true;
            }
        }

        private void txtExistenciaMinima_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
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

        private void LlenarCmbTipoCambio()
        {
            cmbMoneda1.Items.Add("$");
            cmbMoneda1.Items.Add("u$d");
            cmbMoneda1.Items.Add("€");
        }
        private void loadCotizacion()
        {
            txtCosto.Text = "";
            if (cmbMoneda1.SelectedIndex == 0)
            {
                txtCotizacion.Text = "1";
            }
            else if (cmbMoneda1.SelectedIndex == 1)
            {
                string consultaDolar = "SELECT cotizacion from cotizacion where nombre = 'dolar'";
                txtCotizacion.Text = conexion.ValorEnVariable(consultaDolar);
            }
            else
            {
                string consultaEuro = "SELECT cotizacion from cotizacion where nombre = 'euro'";
                txtCotizacion.Text = conexion.ValorEnVariable(consultaEuro);
            }



        }

        private void cmbMoneda_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
       
        }

        private void txtCosto_TextChanged(object sender, TextChangedEventArgs e)
        {
        

        }
        private void calculaPrecioUnitario()
        {
            if (txtCosto.Text != "")
            {
                txtPrecioUnitario.Text = (float.Parse(txtCosto.Text) * float.Parse(txtCotizacion.Text)).ToString();
            }
            else
            {
                txtPrecioUnitario.Text = "";
            }
        }

        private void cmbMoneda1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadCotizacion();
            calculaPrecioUnitario();
        }

        private void txtCosto_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            calculaPrecioUnitario();
        }

        private void txtCosto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
            Regex regex = new Regex("^[,][0-9]+$|^[0-9]*[,]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtCosto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Decimal" || e.Key.ToString() == "OemPeriod")
            {
                e.Handled = true;
                txtCosto.Text = txtCosto.Text + ",";
                txtCosto.SelectionStart = txtCosto.Text.Length;
            }
        }

        private void cmbCategoria_DropDownClosed(object sender, EventArgs e)
        {
            LoadListaProveedorCategoria();
            Items.Clear();
            ltsProvProductos.Items.Refresh();
        }
    }
}
