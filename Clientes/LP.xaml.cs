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

namespace wpfFamiliaBlanco.Clientes
{
    /// <summary>
    /// Lógica de interacción para LP.xaml
    /// </summary>
    public partial class LP : Page
    {

        CRUD conexion = new CRUD();

        public LP()
        {
            InitializeComponent();
            loadlp();
            
            
        }

        private void ActualizaDGVlp()
        {
            String consultalp = "SELECT p.nombre, plp.preciolista from productos p,productos_has_listadeprecios plp where FK_idLista = @valor and FK_idProductos=p.idProductos";
            DataTable productos = conexion.ConsultaParametrizada(consultalp, ltsLp.SelectedValue);
            dgvLp.ItemsSource = productos.AsDataView();
            dgvLp.Items.Refresh();
            

        }


        private void loadlp()
        {
            String consulta = "Select * from listadeprecios";
            conexion.Consulta(consulta, ltsLp);
            ltsLp.DisplayMemberPath = "nombre";
            ltsLp.SelectedValuePath = "idLista";

        }

        private void ltsLp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String consulta = "SELECT * from listadeprecios WHERE idLista = @valor";
            DataTable listadeprecios = conexion.ConsultaParametrizada(consulta, ltsLp.SelectedValue);
            ActualizaDGVlp();

            lblfecha.Content = listadeprecios.Rows[0].ItemArray[2].ToString();
            lblnlp.Content = listadeprecios.Rows[0].ItemArray[1].ToString();

           

        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarLp();
            newW.ShowDialog();


            
        }
    }
}
