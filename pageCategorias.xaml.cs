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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for pageCategorias.xaml
    /// </summary>
    public partial class pageCategorias : Page
    {

        CRUD conexion = new CRUD();
        public static string nombreCatLst;
        public string id;
        DataRow selectedDataRow;

        public pageCategorias()
        {
            InitializeComponent();
            loadListaCategoria();
            ltsCategorias.SelectedIndex = 1;
          CargarTxtNombreCategoria();

            if (windowUsuarios.tipoUsuarioDB == "basico")
            {
                this.btnModificar.Visibility = Visibility.Collapsed;
                this.btnEliminar.Visibility = Visibility.Collapsed;

            }

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


            String name = textnombre.Text.ToString();
            String sql;
            String nombreDB = "SELECT COUNT(*) FROM categorias WHERE nombre  = '" + name + "'";
            String nomCat = conexion.ValorEnVariable(nombreDB).ToString();
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAA" + nomCat);
            try
            {

           
                if (name == "")
                {

                   MessageBox.Show("Debe escribir el nombre de la categoria" , "Alerta", MessageBoxButton.OK);
                }
                else if (nomCat == "0")
                {

                      sql = "insert into categorias(nombre) values('" + name + "');";
                        conexion.operaciones(sql);

                        this.loadListaCategoria();





                }
                else
                {
                    MessageBox.Show("La categoria ya existe");
                }
            }
             catch
                {
                    MessageBox.Show("Ocurrio un error");
                }
        }


        //ELIMINAR CATEGORIA
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
           

                selectedDataRow = ((DataRowView)ltsCategorias.SelectedItem).Row;
                textnombre.Text =selectedDataRow["nombre"].ToString();
        
               MessageBoxResult result = MessageBox.Show("Seguro quiere eliminar la categoría  " + selectedDataRow["nombre"].ToString() +"?", "Cuidado", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                   
                   
                        id = ltsCategorias.SelectedValue.ToString();
                        String name = textnombre.Text.ToString();
                        String sql;
                        String categoria;
                        String sql2 = " SELECT COUNT(*) FROM categorias_has_proveedor WHERE FK_idCategorias = '" + id + "'";
                        categoria = conexion.ValorEnVariable(sql2).ToString();

                    if (categoria == "0"  )
                    {
                        sql = "delete from categorias where idCategorias='" + id + "'";
                        conexion.operaciones(sql);
                        MessageBox.Show("Categoría eliminada correctamente.");
                        this.loadListaCategoria();
                        break;
                    }
                    else
                    {
                        MessageBox.Show("No se puede eliminar la categoría porque esta asignada a un proveedor.");
                        break;
                    }
                    
                
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

            String consulta = "SELECT nombre FROM categorias WHERE idCategorias = @valor";
            DataTable proveedor = conexion.ConsultaParametrizada(consulta, ltsCategorias.SelectedValue);

            textnombre.Text = proveedor.Rows[0].ItemArray[0].ToString();
          
            
        }

      
    }
}
