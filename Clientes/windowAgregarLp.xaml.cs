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

namespace wpfFamiliaBlanco.Clientes
{
    /// <summary>
    /// Lógica de interacción para windowAgregarLp.xaml
    /// </summary>
    public partial class windowAgregarLp : Window
    {
        CRUD conexion = new CRUD();
        



        public windowAgregarLp()
        {
            InitializeComponent();
            loadproductos();
        }


        private void loadproductos()
        {
            String consultap = "SELECT * from productos";
            conexion.Consulta(consultap, ltsProductos);
            ltsProductos.DisplayMemberPath = "nombre";
            ltsProductos.SelectedValuePath = "idProductos";
        }
        private void loadlista()
        {
            String consultal = "Select * from productos_has_listadeprecios";
            conexion.Consulta(consultal, ltsLista);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void btnAgregarp_Click(object sender, RoutedEventArgs e)
        {
                
                
                int p = (int)ltsProductos.SelectedValue;
                String consultap = "SELECT precioUnitario from productos where idProductos ='" + p + "';";
                String consultan = "SELECT nombre from productos where idProductos ='" + p + "';";
                var newW = new windowAgregarProducto();
                String precio = conexion.ValorEnVariable(consultap);
                String nombre = conexion.ValorEnVariable(consultan);
                newW.txtpreciou.Text = precio;
                newW.txtnombre.Text = nombre;


                newW.ShowDialog();
                
                

               
          
              
               
            


        }
    }
}
