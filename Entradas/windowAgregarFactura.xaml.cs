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
using System.Windows.Shapes;

namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Interaction logic for windowAgregarFactura.xaml
    /// </summary>
    public partial class windowAgregarFactura : Window
    {

        CRUD conexion = new CRUD();
        public windowAgregarFactura()
        {
            InitializeComponent();
            LoadListaComboProveedor();
            LlenarComboFiltro();
            LlenarCmbIVA();
            LlenarCmbTipoCambio();
        }

        public void LoadListaComboProveedor()
        {
            String consulta = "SELECT p.nombre, p.idProveedor FROM proveedor p INNER JOIN ordencompra o ON p.idProveedor = o.FK_idProveedor";
            conexion.Consulta(consulta, combo: cmbProveedores);
            cmbProveedores.DisplayMemberPath = "nombre";
            cmbProveedores.SelectedValuePath = "idProveedor";
            cmbProveedores.SelectedIndex = 1;
        }
    public void LlenarComboFiltro()
        {
            cmbFiltro.Items.Add("Proveedor");

        }

        private void LlenarCmbIVA()
        {
            cmbIVA.Items.Add("0");
            cmbIVA.Items.Add("21");
            cmbIVA.Items.Add("10,5");
        }

        private void LlenarCmbTipoCambio()
        {
            cmbTipoCambio.Items.Add("$");
            cmbTipoCambio.Items.Add("u$d");
            cmbTipoCambio.Items.Add("€");
        }

        private void dgvProductosFactura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

       
            try
            {
                String id = cmbProveedores.SelectedValue.ToString();
                String nombreProv = cmbProveedores.Text;

                String sql = "SELECT * FROM productos_has_ordencompra, proveedor WHERE ordencompra.FK_idProveedor = proveedor.idProveedor";

          
            }
            catch (Exception)
            {

                
            }
          
        }
    }
}
