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
using wpfFamiliaBlanco.Tesoreria.Clases;

namespace wpfFamiliaBlanco.Tesoreria.Vistas
{
    /// <summary>
    /// Lógica de interacción para PageCheque.xaml
    /// </summary>
    
    public partial class PageCheque : Page
    {
        Cheque cheq = new Cheque();

        public PageCheque()
        {
            InitializeComponent();
            loadGeneral();
            

        }



        private void loadltscheques()
        {
            ltsCheques.ItemsSource = Cheque.getCheque().AsDataView();
            ltsCheques.DisplayMemberPath = "numero";
            ltsCheques.SelectedValuePath = "idCheque";
            ltsCheques.Items.Refresh();

       
        }
       

        private void settxt()
        {
            if (rbpropio.IsChecked==true)
            {
                
                txtimporte.Text = cheq.Importe.ToString();
                txtnumero.Text = cheq.Numero.ToString();
                txtFechacobro.Text = cheq.Fechacobro.ToString("yyyy/MM/dd");
                txtfechaconfeccion.Text = cheq.Fechaconfeccion.ToString();
                txtbanco.Text = cheq.Banco.ToString();
            }
            else
            {
                
            }
           


        }
        private void loadGeneral()
        {
            loadltscheques();
            

        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            int tipo = 0;

            var newW = new WindowAgregarCheque(tipo,ltsCheques.Items.Count);

            newW.ShowDialog();
            if (newW.DialogResult == true)
            {
                newW.Cheq.save(tipo);
                
            }

        }
        private void btnAgregart_Click(object sender, RoutedEventArgs e)
        {
            int tipo = 1;

            var newW = new WindowAgregarCheque(tipo,ltsCheques.Items.Count);
            newW.ShowDialog();
            if (newW.DialogResult == true)
            {
                newW.Cheq.save(tipo);

            }
        }

        private void btnModificar_Copy_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

        }

       

        private void ltsFactura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ltsCheques_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            cheq.setDatos(ltsCheques.SelectedValue.ToString(), "0");
            settxt();
        }



        private void rbtercero_Checked(object sender, RoutedEventArgs e)
        {
            lblentrego.Visibility = Visibility.Visible;
            txtfirmante.Visibility = Visibility.Collapsed;
            txtcliente.Visibility = Visibility.Visible;
            lblentrego.Content = "Entrego";
        }

        private void rbpropio_Unchecked(object sender, RoutedEventArgs e)
        {
            lblentrego.Visibility = Visibility.Visible;
            txtcliente.Visibility = Visibility.Visible;
            txtfirmante.Visibility = Visibility.Collapsed;
            lblentrego.Content = "Entrego";
        }

        private void rbpropio_Checked(object sender, RoutedEventArgs e)
        {
            txtcliente.Visibility = Visibility.Collapsed;
            lblentrego.Content = "Firmante";
            txtfirmante.Visibility = Visibility.Visible;
        }

        private void rbtercero_Unchecked(object sender, RoutedEventArgs e)
        {
            txtcliente.Visibility = Visibility.Collapsed;
            txtfirmante.Visibility = Visibility.Visible;
            lblentrego.Content = "Firmante";
        }
    }
}
