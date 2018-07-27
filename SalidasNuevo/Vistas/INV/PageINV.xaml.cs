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

        private void settxt()
        {
            txtalcohol.Text = inv.Alcohol.ToString();
            txtdensidad.Text = inv.Densidad.ToString();
            txtfecha.Text = inv.Fechaanalisis.ToString("yyyy/MM/dd");
            txtlitros.Text = inv.Litros.ToString();
            txtnombre.Text = inv.Nombrevino.ToString();

            
        }

        private void loadltsanalisis()
        {
            ltsnumanalisis.ItemsSource = Analisis.getAnalisis().AsDataView();
            ltsnumanalisis.DisplayMemberPath = "numero";
            ltsnumanalisis.SelectedValuePath = "idAnalisis";
            ltsnumanalisis.Items.Refresh();

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
        private void collapsedproduct(string tipo)
        {
            if (tipo=="0"){
                txtproducto.Visibility = Visibility.Collapsed;
                lblproducto.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtproducto.Visibility = Visibility.Visible;
                lblproducto.Visibility = Visibility.Visible;
            }
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
            loadltsanalisis(inv.Tipo.ToString());
            cmbtipo.SelectedIndex = inv.Tipo;
            ltsnumanalisis.SelectedIndex = ltsnumanalisis.Items.Count - 1;
            settxt();
        }

        private void ListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        { 
              inv.setDatos(ltsnumanalisis.SelectedValue.ToString(),cmbtipo.SelectedIndex.ToString());
              settxt();
            
            
        }

        private void cmbtipo_DropDownClosed(object sender, EventArgs e)
        {
           
                
                loadltsanalisis(cmbtipo.SelectedIndex.ToString());
                collapsedproduct(cmbtipo.SelectedIndex.ToString());
           
            
            
        }
    }
}
