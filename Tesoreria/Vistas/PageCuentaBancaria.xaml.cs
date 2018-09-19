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
    /// Lógica de interacción para PageCuentaBancaria.xaml
    /// </summary>
    public partial class PageCuentaBancaria : Page
    {
        CRUD conexion = new CRUD();
        CuentaBancaria cta = new CuentaBancaria();

        public PageCuentaBancaria()
        {
            InitializeComponent();
            loadltscheques();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new WindowAgregarCuentaBancaria();

            newW.ShowDialog();

            if (newW.DialogResult==true)
            {
                newW.Cta.save();
            }
        }

        private void loadltscheques()
        {
            ltsctas.ItemsSource = CuentaBancaria.getCta().AsDataView();
            ltsctas.DisplayMemberPath = "numerocta";
            ltsctas.SelectedValuePath = "idCuentaBco";
            ltsctas.Items.Refresh();


        }
        private void settxt()
        {
           

                txtalias.Text = cta.Alias.ToString();
                txtbco.Text = cta.Banco.ToString();
                txtmoneda.Text = cta.Moneda.ToString();
                txttipocuenta.Text = cta.Tipo.ToString();
                txttitular.Text = cta.Titular.ToString();
                txtsaldo.Text = cta.Saldo.ToString();
                txtcbu.Text = cta.Cbu.ToString();
                

        }
       


        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnModificar_Copy_Click(object sender, RoutedEventArgs e)
        {

        }
        

        private void ltsFactura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ltsctas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            cta.setDatos(ltsctas.SelectedValue.ToString(), "2");
            settxt();
        }
    }
}
