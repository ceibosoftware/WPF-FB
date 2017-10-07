using System;
using System.Collections.Generic;
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
    /// Interaction logic for windowModificarProveedor.xaml
    /// </summary>
    public partial class windowModificarProveedor : Window
    {
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
        private List<categoria> items = new List<categoria>();
        public List<categoria> Items { get => items; set => items = value; }
        public windowModificarProveedor()
        {
            InitializeComponent();
            CargarCMB();
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

        public void CargarCMB()
        {
            cmbRazonSocial.Items.Add("SA");
            cmbRazonSocial.Items.Add("Responsable Inscripto");
            cmbRazonSocial.Items.Add("MOnotributista");
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
