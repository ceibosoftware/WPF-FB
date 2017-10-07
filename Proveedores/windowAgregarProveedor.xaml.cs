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

        public class contacto
        {

            public  String NombreContacto { get; set; }
            public  String Email { get; set; }
            public  String NumeroTelefono { get; set; }
           

            public contacto (String nomContacto, String ema, String numTelefono)
            {
                NombreContacto = nomContacto;
                Email = ema;
                NumeroTelefono = numTelefono;

            }
        }


        CRUD conexion = new CRUD();
        public static String sqlContacto;
        public static  List<contacto> lista = new List<contacto>();

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
            if (txtCuit.Text != "" && txtDireccion.Text != "" && txtNombre.Text != "" && txtCP.Text != "" && txtLocalidad.Text != "")
            {
                return true;
            }
            else
            {
                MessageBox.Show("Faltan campos por completar");
                return false;
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
                contacto info =  new contacto(nombreContacto1, mail1, telefono1);
                
                
                lista.Add(info);
                LoadDGVContacto();
               
               

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
    }
}
