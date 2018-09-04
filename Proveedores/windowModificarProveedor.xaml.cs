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

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for windowModificarProveedor.xaml
    /// </summary>
    public partial class windowModificarProveedor : Window
    {

        CRUD conexion = new CRUD();
        private List<Categorias> items;
        public List<Categorias> Items { get => items; set => items = value; }
        public List<Contacto> conActual = new List<Contacto>() ;
         public string CUIT;
     
        public windowModificarProveedor(List<Categorias> lista, List<Contacto> contactoActual)
        {
            InitializeComponent();
            CargarCMB();
            LoadListaProv(lista);
            LoadListaProveedor();
            LlenarComboFiltro();
            LimitarCampos();
            conActual = contactoActual;
            dgvContactom.ItemsSource = conActual;

            ltsCatProveedores.SelectionMode = SelectionMode.Single;
            ltsCategorias.SelectionMode = SelectionMode.Single;
            this.dgvContactom.IsReadOnly = true;
            
           

        }



        private void LimitarCampos()
        {
            ltsCatProveedores.SelectionMode = SelectionMode.Single;
            ltsCategorias.SelectionMode = SelectionMode.Single;
            txtFiltro.MaxLength = 10;
            txtFiltro.MaxLines = 1;
            txtCP.MaxLength = 10;
            txtCP.MaxLines = 1;
            txtCuit.MaxLength = 15;
            txtCuit.MaxLines = 1;
            txtDireccion.MaxLength = 40;
            txtLocalidad.MaxLength = 20;
            txtLocalidad.MaxLines = 1;
            txtCategoria.MaxLength = 20;
            txtCategoria.MaxLines = 1;
        }
        private void LoadListaProveedor()
        {

            String consulta = " Select * from categorias ";
            conexion.Consulta(consulta, ltsCategorias);
            ltsCategorias.DisplayMemberPath = "nombre";
            ltsCategorias.SelectedValuePath = "idCategorias";
        }

        private void LoadListaProv(List<Categorias> lista)
        {
            items = lista;
            ltsCatProveedores.ItemsSource = items;
            ltsCatProveedores.DisplayMemberPath = "nombre";
            ltsCatProveedores.SelectedValuePath = "id";
        }

        public void CargarCMB()
        {
            cmbRazonSocial.Items.Add("Responsable Inscripto");
            cmbRazonSocial.Items.Add("Monotributista");
            cmbRazonSocial.Items.Add("Exento");
            cmbRazonSocial.Items.Add("Consumidor final");
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCategoria.Text))
            {
                MessageBox.Show("Falta completar campo nombre");

            }
            else if (string.IsNullOrEmpty(txtLocalidad.Text))
            {
                MessageBox.Show("falta completar campo localidad");

            }
            else if (string.IsNullOrEmpty(txtDireccion.Text))
            {
                MessageBox.Show("falta completar campo dirección");

            }
            else if (string.IsNullOrEmpty(txtCuit.Text))
            {
                MessageBox.Show("falta completar campo cuit");

            }
            else if (string.IsNullOrEmpty(txtCP.Text))
            {
                MessageBox.Show("falta completar campo código postal");

            }
            else if (ltsCatProveedores.Items.Count == 0)
            {
                MessageBox.Show("Es necesario ingresar alguna categoria del proveedor");

            }
            else if (!string.IsNullOrEmpty(txtCuit.Text) && int.Parse(conexion.ValorEnVariable("select count(cuit) from proveedor where cuit = '" + txtCuit.Text + "'")) == 1 && String.Compare(CUIT,txtCuit.Text) != 0)
            {

                MessageBox.Show("El cuit ingresado ya existe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
               

            }
            else if (dgvContactom.HasItems == false)
            {
                MessageBox.Show("Es necesario ingresar algun contacto al proveedor");

            }
            else
            {
                DialogResult = true;
            }
        }

        private void LlenarComboFiltro()
        {


        }

        private void btnCatAgregar_Click(object sender, RoutedEventArgs e)
        {

            try
            {

           

            Boolean existe = false;
            DataRow selectedDataRow = ((DataRowView)ltsCategorias.SelectedItem).Row;

            if (ltsCatProveedores.Items.Count <= 0)
            {
                items.Add(new Categorias(selectedDataRow["nombre"].ToString(), (int)ltsCategorias.SelectedValue));
                ltsCatProveedores.Items.Refresh();
            }
            else
            {
                for (int i = 0; i < ltsCatProveedores.Items.Count; i++)
                {

                    if (selectedDataRow["nombre"].ToString().CompareTo(items[i].nombre) != 0)
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

                    items.Add(new Categorias(selectedDataRow["nombre"].ToString(), (int)ltsCategorias.SelectedValue));
                    ltsCatProveedores.Items.Refresh();


                    Console.WriteLine("categorias" + Items.Count);



                }
                else
                {
                    MessageBox.Show("Esa categoria ya fue agregada");
                }
            }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione una categoria");
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
                MessageBox.Show("Seleccione una categoría");
            }
        }

        private void btnNuevoContacto_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarContactoProveedor();
            String idpr = pageProveedores.idProv2;
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {
               
                String telefono12 = newW.txtTelefonoContacto.Text;
                String nombreContacto12 = newW.txtNombreContacto.Text;
                String mail12 = newW.txtMailContacto.Text;
                Contacto con = new Contacto(nombreContacto12, mail12, telefono12);
                conActual.Add(con);
                String sqlContacto;

                sqlContacto = "insert into contactoproveedor(telefono, email, nombreContacto, FK_idProveedor) values('" + telefono12 + "', '" + mail12 + "', '" + nombreContacto12 + "', '" + idpr + "');";
                conexion.operaciones(sqlContacto);
                LoadListaProveedor();
                dgvContactom.ItemsSource = conActual;
                dgvContactom.Items.Refresh();
                

            }
                
        }


        private void btnEliminarContacto_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

          

            Contacto contacto = dgvContactom.SelectedItem as Contacto;

          



               for (int i = 0; i <= conActual.Count -1; i++)
               {
                
              
                if (contacto.NumeroTelefono.ToString().CompareTo(conActual[i].NumeroTelefono.ToString()) == 0)
                {

                    conActual.Remove(conActual[i]);
                    dgvContactom.Items.Refresh();
                    String update;
                    update = "DELETE FROM contactoproveedor WHERE telefono = '"+contacto.NumeroTelefono+"'";
                    conexion.operaciones(update);
                    MessageBox.Show("Se ha eliminado el contacto");
                    break;
                } else
                 {
                    
                            
                 }
                    

               }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleecione un contacto");
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

        private void txtCategoria_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
         
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

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {

            try
            {

            
            Contacto contacto2 = dgvContactom.SelectedItem as Contacto;
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
                dgvContactom.Items.Refresh();
            }

            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un contacto");
            }
        }

        private void cmbFiltro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            string consulta;
            DataTable categorias = new DataTable();

            //Busca por nombre
            consulta = "SELECT * FROM categorias WHERE categorias.nombre LIKE '%' @valor '%'";
            categorias = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            ltsCategorias.ItemsSource = categorias.AsDataView();
            ltsCategorias.SelectedIndex = 0;
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

        private void btnNuevoContacto_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
