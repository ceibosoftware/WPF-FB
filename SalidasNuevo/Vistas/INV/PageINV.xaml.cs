using System;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpfFamiliaBlanco.SalidasNuevo.Clases.INV;

namespace wpfFamiliaBlanco.SalidasNuevo.Vistas.INV
{
    /// <summary>
    /// Lógica de interacción para PageINV.xaml
    /// </summary>
    public partial class PageINV : Page
    {
        Analisis inv=new Analisis();

        public PageINV()
        {
            InitializeComponent();
            loadGeneral();


        }
        //Metodo para definir lo que se va a mostrar en esta pantalla
        private void settxt()
        {
            
            if (inv.Tipo==0)
            {
                txttype.Text = "Libre Circulacion";
            }
            else
            {
                txttype.Text = "Aptitud de Exportacion";
            }

            txtalcohol.Text = inv.Alcohol.ToString();
            txtdensidad.Text = inv.Densidad.ToString();
            txtfecha.Text = inv.Fechaanalisis.ToString("yyyy/MM/dd");
            txtlitros.Text = inv.Litros.ToString();
            txtnombre.Text = inv.Nombrevino.ToString();
        }

        private void loadltsanalisis()
        {
            ltsnumanalisis.Items.Clear();
            ltsnumanalisis.ItemsSource = Analisis.getAnalisis().AsDataView();
            ltsnumanalisis.DisplayMemberPath = "numero";
            ltsnumanalisis.SelectedValuePath = "idAnalisis";
            ltsnumanalisis.Items.Refresh();

        }
        private void loadltsanalisis(int indexlista)
        {
            
            ltsnumanalisis.ItemsSource = Analisis.getAnalisis().AsDataView();
            ltsnumanalisis.DisplayMemberPath = "numero";
            ltsnumanalisis.SelectedValuePath = "idAnalisis";
            ltsnumanalisis.SelectedIndex = indexlista;
            inv.setDatos(ltsnumanalisis.SelectedValue.ToString());
            settxt();
           
        }
        private void loadcmbtipo()
        {
            
            cmbtipo.Items.Add("Libre Circulacion");
            cmbtipo.Items.Add("Aptitud de Exportacion");
        }

        private void loadGeneral()
        {
            loadltsanalisis();
            loadcmbtipo();
            
        }
        private void loadltsanalisis(string tipo)
        {
            
            ltsnumanalisis.ItemsSource = Analisis.getAnalisis(tipo).AsDataView();
            ltsnumanalisis.DisplayMemberPath = "numero";
            ltsnumanalisis.SelectedValuePath = "idAnalisis";
            ltsnumanalisis.Items.Refresh();
        }
        private void collapsedproduct(int tipo)
        {
            if (tipo==0){
                ltspasociado.Visibility = Visibility.Collapsed;
                lblproducto.Visibility = Visibility.Collapsed;
            }
            else
            {
                ltspasociado.Visibility = Visibility.Visible;
                lblproducto.Visibility = Visibility.Visible;
            }
        }

        private void loadltsproductos(string tipo)
        {
            
            ltspasociado.ItemsSource = Analisis.getProductoasociado(tipo).AsDataView();
            ltspasociado.DisplayMemberPath = "FK_idProductos";
            ltspasociado.SelectedValuePath = "idAnalisis";
            ltspasociado.Items.Refresh();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarINV();
            newW.ShowDialog();

            if (newW.DialogResult==true)
            {
                newW.Inv.save();
                selectlastinsert(newW.Inv);
            }
            
        }
        private void selectlastinsert(Analisis inv)
        {


            ltsnumanalisis.SelectedItem = ltsnumanalisis.Items.Count;
            loadltsanalisis(ltsnumanalisis.Items.Count);

        }

        private void ListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                inv.setDatos(ltsnumanalisis.SelectedValue.ToString());
            }
            catch (NullReferenceException)
            {

                
            }
              
              settxt();
            int id = inv.gettipo((int)ltsnumanalisis.SelectedValue);
            collapsedproduct(id);
            
            
        }

        private void cmbtipo_DropDownClosed(object sender, EventArgs e)
        {
           
                
                loadltsanalisis(cmbtipo.SelectedIndex.ToString());
                collapsedproduct(cmbtipo.SelectedIndex);
                
                



        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

            inv.delete((int)ltsnumanalisis.SelectedValue);
            ltsnumanalisis.Items.Refresh();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)ltsnumanalisis.SelectedValue;
            int indexlista = ltsnumanalisis.SelectedIndex;
            float alcohol = float.Parse(txtalcohol.Text);
            float densidad = float.Parse(txtdensidad.Text);
            float litros = float.Parse(txtlitros.Text);
            String numeroanalisis = inv.getnumero((int)ltsnumanalisis.SelectedValue);
            int tipo = inv.gettipo((int)ltsnumanalisis.SelectedValue);
            DateTime fechaanalisis = DateTime.Parse(txtfecha.Text);
            String nombrevino = txtnombre.Text;
            Analisis modified = new Analisis(id,alcohol, densidad, litros, numeroanalisis, tipo, fechaanalisis, nombrevino);
            var newW = new windowAgregarINV(modified);
            newW.ShowDialog();
            if (newW.DialogResult==true)
            {
                newW.Inv.update((int)ltsnumanalisis.SelectedValue);
            }
            else
            {
                MessageBox.Show("No se puedo modificar");
            }
            
            loadltsanalisis(indexlista);
            
            
        }

        private void ltsnumanalisis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadltsproductos(cmbtipo.SelectedIndex.ToString());
        }
    }
}
