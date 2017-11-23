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

namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Interaction logic for windowAgregarOC.xaml
    /// </summary>
    public partial class windowAgregarOC : Window
    {


        CRUD conexion = new CRUD();
        public windowAgregarOC()
        {
            InitializeComponent();

            AgregarCuotasCmbCuotas();
            LoadListaComboProveedor();
            LlenarCmbIVA();
            LlenarCmbTipoCambio();
                  
        }

        private void cmbCuotas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var newW = new windowCuotas();
            if (cmbCuotas.SelectedItem.Equals("1"))
            {
                newW.lblCuota2.Visibility = Visibility.Collapsed;
                newW.txtCuota2.Visibility = Visibility.Collapsed;

                newW.lblCuota3.Visibility = Visibility.Collapsed;
                newW.txtCuota3.Visibility = Visibility.Collapsed;
            }else if (cmbCuotas.SelectedItem.Equals("2"))
            {
                newW.lblCuota3.Visibility = Visibility.Collapsed;
                newW.txtCuota3.Visibility = Visibility.Collapsed;
            }
            newW.ShowDialog();
        }

        public void AgregarCuotasCmbCuotas()
        {
            cmbCuotas.Items.Add("1");
            cmbCuotas.Items.Add("2");
            cmbCuotas.Items.Add("3");
        }
        private void lblAgregarRemito_Copy_Click(object sender, RoutedEventArgs e)
        {
            
            var newW = new windowAgregarRemito();
           var resultado = MessageBox.Show("Desea agregar la orden de compra? ","Advertencia",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes)
            {
                newW.ShowDialog();
            }
            

        }

        private void btAgregarFactura_Copy_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarFactura();
            var resultado = MessageBox.Show("Desea agregar la orden de compra? ", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes)
            {
                newW.ShowDialog();
            }
           
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowCuotas();
            newW.ShowDialog();
        }

        private void btnAgregar_Copy_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarClienteME();
            newW.ShowDialog();
        }


        public void LoadListaComboProveedor()
        {
            String consulta = "SELECT * FROM proveedor";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
            cmbProveedores.SelectedIndex = 0;
        }

        private void LlenarCmbIVA()
        {
            cmbIVA_Copy.Items.Add("0");
            cmbIVA_Copy.Items.Add("21");
            cmbIVA_Copy.Items.Add("10,5");
        }

        private void LlenarCmbTipoCambio()
        {
            cmbTipoCambio_Copy.Items.Add("$");
            cmbTipoCambio_Copy.Items.Add("u$d");
            cmbTipoCambio_Copy.Items.Add("€");
        }
    }
}
