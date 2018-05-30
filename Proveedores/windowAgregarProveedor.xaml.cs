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
using wpfFamiliaBlanco.Proveedores;

namespace wpfFamiliaBlanco.Proveedores
{
    /// <summary>
    /// Interaction logic for windowAgregarProveedor.xaml
    /// </summary>
    public partial class windowAgregarProveedor : Window
    {
        private List<categoria> items = new List<categoria>();
        public List<categoria> Items { get => items; set => items = value; }
    
        public class categoria
        {
            public string nombre { get; set; }
            public int id { get; set; }
            public categoria(string nombre, int id)
            {
                this.nombre = nombre;
                this.id = id;
            }
        }



        CRUD conexion = new CRUD();
        public static String sqlContacto;
        public static  List<Contacto> lista = new List<Contacto>();

        public windowAgregarProveedor()
        {
            InitializeComponent();
            LlenarComboFiltro();
            EliminarDGVContacto();
            LoadListaProv();
            LoadListaProveedor();
            LimitarCampos();
        }

        private void LoadListaProveedor()
        {

            String consulta = " Select * from categorias ";
            conexion.Consulta(consulta, ltsCategorias);
            ltsCategorias.DisplayMemberPath = "nombre";
            ltsCategorias.SelectedValuePath = "idCategorias";
        }

        private void LimitarCampos()
        {
            ltsCatProveedores.SelectionMode = SelectionMode.Single;
            ltsCategorias.SelectionMode = SelectionMode.Single;
            txtBuscar.MaxLength = 20;
            txtBuscar.MaxLines = 1;
            txtCP.MaxLength = 10;
            txtCP.MaxLines = 1;
            txtCuit.MaxLength = 15;
            txtCuit.MaxLines = 1;
            txtDireccion.MaxLength = 20;
            txtDireccion.MaxLength = 20;
            txtLocalidad.MaxLength = 20;
            txtLocalidad.MaxLines = 1;
            txtNombre.MaxLength = 20;
            txtNombre.MaxLines = 1;
        }

        private void LoadListaProv()
        {
            ltsCatProveedores.ItemsSource = Items;
            ltsCatProveedores.DisplayMemberPath = "nombre";
            ltsCatProveedores.SelectedValuePath = "id";
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {

            if (Valida())
            {
                
                DialogResult = true;
            }
     
        }

        private void LlenarComboFiltro()
        {

            cmbRazonSocial.Items.Add("Responsable Inscripto");
            cmbRazonSocial.Items.Add("SA");
            cmbRazonSocial.Items.Add("SRL");
        }

        public Boolean Valida()
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Falta completar campo nombre", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
         
                  return false;
            }
            else if (string.IsNullOrEmpty(txtLocalidad.Text))
            {
                MessageBox.Show("Falta completar campo localidad", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    
                return false;
            }
            else if (string.IsNullOrEmpty(txtDireccion.Text))
            {
                MessageBox.Show("Falta completar campo dirección", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          
                return false;
            }
            else if (string.IsNullOrEmpty(txtCuit.Text))
            {
                MessageBox.Show("Falta completar campo cuit", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
               
                return false;
            }
            else if (string.IsNullOrEmpty(txtCP.Text))
            {
                MessageBox.Show("Falta completar campo código postal", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
               
                return false;
            }
            else if (ltsCatProveedores.Items.Count == 0)
            {
                MessageBox.Show("Es necesario ingresar alguna categoria del proveedor", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        
                return false;
            }
            else if (dgv.HasItems == false)
            {
                MessageBox.Show("Es necesario ingresar algun contacto al proveedor", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
               
                return false;
            }
            
            else
            {
                return true;
            }

        }

   

        private void btnNuevoContacto_Click_1(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarContactoProveedor();
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {
                Console.WriteLine("Entro");
                 
                String telefono1 = newW.txtTelefonoContacto.Text;
                String nombreContacto1 = newW.txtNombreContacto.Text;
                String mail1 = newW.txtMailContacto.Text;
                Contacto info =  new Contacto(nombreContacto1, mail1, telefono1);
                
                
                lista.Add(info);
                LoadDGVContacto();
                dgv.ItemsSource = lista;
                dgv.Items.Refresh();
             
                // conexion.operaciones(sql);
               
                
            }
         

        }

        public void LoadDGVContacto()
        {
            this.dgv.ItemsSource = lista;
            dgv.Items.Refresh();
          
        }

        public void EliminarDGVContacto()
        {
            this.dgv.Items.Remove(lista);
            dgv.Items.Refresh();

        }

   

        private void btnCatAgregar_Click_1(object sender, RoutedEventArgs e)
        {

            try
            {
#pragma warning disable CS0219 // The variable 'provIndex' is assigned but its value is never used
                int provIndex = 0;
#pragma warning restore CS0219 // The variable 'provIndex' is assigned but its value is never used
                Boolean existe = false;
                DataRow selectedDataRow = ((DataRowView)ltsCategorias.SelectedItem).Row;

                if (ltsCatProveedores.Items.Count <= 0)
                {
                    Items.Add(new categoria(selectedDataRow["nombre"].ToString(), (int)ltsCategorias.SelectedValue));
                    ltsCatProveedores.Items.Refresh();
                }
                else
                {
                    for (int i = 0; i < ltsCatProveedores.Items.Count; i++)
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

                        Items.Add(new categoria(selectedDataRow["nombre"].ToString(), (int)ltsCategorias.SelectedValue));
                        ltsCatProveedores.Items.Refresh();


                        Console.WriteLine("categorias" + Items.Count);



                    }
                    else
                    {
                        MessageBox.Show("Esa categoria ya fue agregada", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Debe seleccionar una categoría", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void btnCatEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Items.Remove(Items.Find(item => item.id == (int)ltsCatProveedores.SelectedValue));
                ltsCatProveedores.Items.Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show("Debe seleccionar una categoria a eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            
            }
      
        }

        private void btnEliminarContacto_Click(object sender, RoutedEventArgs e)
        {
            Contacto contacto = dgv.SelectedItem as Contacto;





            for (int i = 0; i <= lista.Count - 1; i++)
            {


                if (contacto.NumeroTelefono.ToString().CompareTo(lista[i].NumeroTelefono.ToString()) == 0)
                {

                    lista.Remove(lista[i]);
                    dgv.Items.Refresh();
#pragma warning disable CS0168 // The variable 'update' is declared but never used
                    String update;
#pragma warning restore CS0168 // The variable 'update' is declared but never used

                    MessageBox.Show("Se elimino el contacto correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                }
                else
                {
               

                }


            }
        }

        private void txtCuit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void txtCP_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void txtNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z-ñ]"))
            {
                e.Handled = true;
            }
        }

        private void txtLocalidad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z-ñ]"))
            {
                e.Handled = true;
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
           
            this.Close();

        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            string consulta;
            DataTable categorias = new DataTable();

            //Busca por nombre
            consulta = "SELECT * FROM categorias WHERE categorias.nombre LIKE '%' @valor '%'";
            categorias = conexion.ConsultaParametrizada(consulta, txtBuscar.Text);
            ltsCategorias.ItemsSource = categorias.AsDataView();
            ltsCategorias.SelectedIndex = 0;
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

          
            Contacto contacto2 = dgv.SelectedItem as Contacto;
            var newW = new windowAgregarContactoProveedor();
            String telActual = contacto2.NumeroTelefono.ToString();
            newW.txtMailContacto.Text = contacto2.Email.ToString();
            newW.txtNombreContacto.Text = contacto2.NombreContacto.ToString();
            newW.txtTelefonoContacto.Text = contacto2.NumeroTelefono.ToString();
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {
                String nombre = newW.txtNombreContacto.Text.ToString();
                String tel = newW.txtTelefonoContacto.Text.ToString();
                String mail = newW.txtMailContacto.Text.ToString();

                contacto2.NombreContacto = nombre;
                contacto2.NumeroTelefono = tel;
                contacto2.Email = mail;

                String update;
                update = "update contactoproveedor set telefono  = '" + tel + "', email = '" + mail + "', nombreContacto = '" + nombre + "' where telefono ='" + telActual + "';";
                conexion.operaciones(update);
                dgv.Items.Refresh();
            }

            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un contacto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAgregarCategoria_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarCategoria();
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {
                LoadListaComboCategoria();
            }
        }
        public void LoadListaComboCategoria()
        {
            String consulta = "SELECT * FROM categorias";
            conexion.Consulta(consulta, ltsCategorias);
            ltsCategorias.DisplayMemberPath = "nombre";
            ltsCategorias.SelectedValuePath = "idCategorias";
            ltsCategorias.SelectedIndex = 0;
        }

    }
}
