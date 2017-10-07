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
    /// Interaction logic for windowModificarProveedor.xaml
    /// </summary>
    public partial class windowModificarProveedor : Window
    {

        CRUD conexion = new CRUD();
        private List<Categorias> items;
        public List<Categorias> Items { get => items; set => items = value; }
        public windowModificarProveedor(List<Categorias> lista)
        {
            InitializeComponent();
            CargarCMB();
            LoadListaProv(lista);
            LoadListaProveedor();
            LlenarComboFiltro();
            
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
            cmbRazonSocial.Items.Add("SA");
            cmbRazonSocial.Items.Add("Responsable Inscripto");
            cmbRazonSocial.Items.Add("MOnotributista");
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void LlenarComboFiltro()
        {

            cmbFiltro.Items.Add("Nombre");
            cmbFiltro.Items.Add("Categoria");

        }

        private void btnCatAgregar_Click(object sender, RoutedEventArgs e)
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
    }
}
