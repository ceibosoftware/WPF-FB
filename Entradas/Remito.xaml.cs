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

namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Interaction logic for Remito.xaml
    /// </summary>
    public partial class Remito : Page
    {

        CRUD conexion = new CRUD();
        public Remito()
        {
            InitializeComponent();
            LoadListaComboProveedor();
            
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarRemito();
            newW.ShowDialog();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarRemito();
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
    }

 
}
