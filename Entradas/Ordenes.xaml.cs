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
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpfFamiliaBlanco.Entradas;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for Ordenes.xaml
    /// </summary>
    public partial class Ordenes : Page
    {

        CRUD conexion = new CRUD();
        public Ordenes()
        {
            InitializeComponent();
            LoadListaComboProveedor();
            LlenarCmbIVA();
            LlenarCmbTipoCambio();
        }


        public void LoadListaComboProveedor()
        {
            String consulta = "SELECT * FROM proveedor";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
            cmbProveedores.SelectedIndex = 0;
        }


        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarOC();
            newW.ShowDialog();
        }

        private void LlenarCmbIVA()
        {
            cmbIVA.Items.Add("0");
            cmbIVA.Items.Add("21");
            cmbIVA.Items.Add("10,5");
        }

        private void LlenarCmbTipoCambio()
        {
            cmbTipoCambio_Copy.Items.Add("$");
            cmbTipoCambio_Copy.Items.Add("u$d");
            cmbTipoCambio_Copy.Items.Add("€");
        }
    }
}
