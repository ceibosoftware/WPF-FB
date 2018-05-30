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
    /// Interaction logic for windowAgregarCategoria.xaml
    /// </summary>
    public partial class windowAgregarCategoria : Window
    {
        

        CRUD conexion = new CRUD();
        public static string nombreCatLst;
        public string id;
        DataRow selectedDataRow;

        public windowAgregarCategoria()
        {
            InitializeComponent();
            loadListaCategoria();
            ltsCategorias.SelectedIndex = 1;
           // CargarTxtNombreCategoria();

        }


        //CARGAR CATEGORIAS EN EL LIST
        private void loadListaCategoria()
        {
            String consulta = " Select * from categorias ";
            conexion.Consulta(consulta, ltsCategorias);
            ltsCategorias.DisplayMemberPath = "nombre";
            ltsCategorias.SelectedValuePath = "idCategorias";

        }

        //GET NOMBRE CATEGORIA
        public string getNameLstCategorias()
        {
            return ltsCategorias.DisplayMemberPath;
        }





        //MODIFICAR CATEGORIA
        private void btnModificar_Click_1(object sender, RoutedEventArgs e)
        {


            id = ltsCategorias.SelectedValue.ToString();
            Console.WriteLine("VALOR: " + id);


            String name = textnombre.Text.ToString();

            Console.WriteLine("Nombre: " + name);
            String sql;



            sql = "update categorias set nombre='" + name + "' WHERE idCategorias ='" + id + "'";
            conexion.operaciones(sql);
            this.loadListaCategoria();


        }



        //AGREGAR CATEGORIA
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            String name = textnombre.Text.ToString();
            String sql;
            String nombreDB = "SELECT COUNT(*) FROM categorias WHERE nombre  = '" + name + "'";
            String nomCat = conexion.ValorEnVariable(nombreDB).ToString();
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAA" + nomCat);
            try
            {


                if (name == "")
                {

                    MessageBox.Show("Debe escribir el nombre de la categoria", "Advertencia", MessageBoxButton.OK,MessageBoxImage.Warning);
                }
                else if (nomCat == "0")
                {

                    sql = "insert into categorias(nombre) values('" + name + "');";
                    conexion.operaciones(sql);

                    this.loadListaCategoria();





                }
                else
                {
                    MessageBox.Show("La categoría ya existe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Ocurrió un error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //ELIMINAR CATEGORIA
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            selectedDataRow = ((DataRowView)ltsCategorias.SelectedItem).Row;
            textnombre.Text = selectedDataRow["nombre"].ToString();

            MessageBoxResult result = MessageBox.Show("¿Seguro quiere eliminar la categoría ? " + selectedDataRow["nombre"].ToString(), "Advertencia", MessageBoxButton.YesNo,MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    id = ltsCategorias.SelectedValue.ToString();
                    String name = textnombre.Text.ToString();
                    String sql;

                    sql = "delete from categorias where idCategorias='" + id + "'";
                    conexion.operaciones(sql);
                    this.loadListaCategoria();
                    break;
                case MessageBoxResult.No:

                    break;

            }

        }


        private void ltsCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {

                CargarTxtNombreCategoria();
            }
            catch
            {

            }



        }

        private void CargarTxtNombreCategoria()
        {
            try
            {

           
            String consulta = "SELECT nombre FROM categorias WHERE idCategorias = @valor";
            DataTable proveedor = conexion.ConsultaParametrizada(consulta, ltsCategorias.SelectedValue);

            textnombre.Text = proveedor.Rows[0].ItemArray[0].ToString();
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Escriba el nombre de la categoría", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


    }
}

