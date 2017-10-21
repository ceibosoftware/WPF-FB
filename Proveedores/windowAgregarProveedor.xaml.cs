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

        }

        private void LoadListaProveedor()
        {

            String consulta = " Select * from categorias ";
            conexion.Consulta(consulta, ltsCategorias);
            ltsCategorias.DisplayMemberPath = "nombre";
            ltsCategorias.SelectedValuePath = "idCategorias";
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
                MessageBox.Show("Falta completar campo nombre");
                return false;
            }
            else if (string.IsNullOrEmpty(txtLocalidad.Text))
            {
                MessageBox.Show("falta completar campo localidad");
                return false;
            }
            else if (string.IsNullOrEmpty(txtDireccion.Text))
            {
                MessageBox.Show("falta completar campo dirección");
                return false;
            }
            else if (string.IsNullOrEmpty(txtCuit.Text))
            {
                MessageBox.Show("falta completar campo cuit");
                return false;
            }
            else if (string.IsNullOrEmpty(txtCP.Text))
            {
                MessageBox.Show("falta completar campo código postal");
                return false;
            }
            else if (ltsCatProveedores.Items.Count == 0)
            {
                MessageBox.Show("Es necesario ingresar alguna categoria del proveedor");
                return false;
            }
            else if (dgv.HasItems == false)
            {
                MessageBox.Show("Es necesario ingresar algun contacto al proveedor");
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
            int provIndex = 0;
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
                    MessageBox.Show("Esa categoria ya fue agregada");
                }
            }
        }

        private void btnCatEliminar_Click(object sender, RoutedEventArgs e)
        {
      Items.Remove(Items.Find(item => item.id == (int)ltsCatProveedores.SelectedValue));
              ltsCatProveedores.Items.Refresh();
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
                    String update;
                   // update = "DELETE FROM contactoproveedor WHERE telefono = '" + contacto.NumeroTelefono + "'";
                    //conexion.operaciones(update);
                    MessageBox.Show("Se eliminio");
                    break;
                }
                else
                {
                    MessageBox.Show("conActual: " + lista[i].NumeroTelefono);
                    MessageBox.Show("No existe");

                }


            }
        }
    }
}
