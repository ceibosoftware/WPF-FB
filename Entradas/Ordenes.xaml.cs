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
            loadlistaOC();  
            fechaActual();
            loadlistaOC();
        }

        private void loadlistaOC()
        {
            try
            {
                String consulta = " Select * from ordencompra ";
                conexion.Consulta(consulta, ltsNumeroOC);
                ltsNumeroOC.DisplayMemberPath = "idOrdenCompra";
                ltsNumeroOC.SelectedValuePath = "idOrdenCompra";
                ltsNumeroOC.SelectedIndex = 0;
            }
            catch (NullReferenceException)
            {

               
            }
              

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

    

    
       private void fechaActual()
        {

            dpFecha.SelectedDate = DateTime.Now;
        }

        private void ltsNumeroOC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //consulta productos
                String consulta = "  SELECT t2.nombre , t1.cantidad,  t1.subtotal from productos_has_ordencompra t1 inner join productos t2  on t1.FK_idProducto = t2.idProductos where t1.FK_idOC = @valor";
                DataTable productos = conexion.ConsultaParametrizada(consulta, ltsNumeroOC.SelectedValue);
                dgvProductos.ItemsSource = productos.AsDataView();
                //llenar datos de oc
                String consulta2 = "SELECT * FROM ordencompra t1 where t1.idOrdenCompra = @valor";
                DataTable OC = conexion.ConsultaParametrizada(consulta2, ltsNumeroOC.SelectedValue);
                txtSubtotal.Text = OC.Rows[0].ItemArray[3].ToString();
                txtIva.Text = OC.Rows[0].ItemArray[5].ToString();
                txtFormaPago.Text = OC.Rows[0].ItemArray[7].ToString();
                txtTipoCambio.Text = OC.Rows[0].ItemArray[6].ToString();
                txtTotal.Text = OC.Rows[0].ItemArray[4].ToString();
                txtDescripcion.Text = OC.Rows[0].ItemArray[2].ToString();
            }
            catch (Exception)
            {

               
            }
      
         
        }

        private void cmbProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                String consulta = " Select * from ordencompra t1 where t1.FK_idProveedor = @valor ";
                DataTable OCProveedor = conexion.ConsultaParametrizada(consulta,cmbProveedores.SelectedValue);
                ltsNumeroOC.ItemsSource = OCProveedor.AsDataView();
                ltsNumeroOC.DisplayMemberPath = "idOrdenCompra";
                ltsNumeroOC.SelectedValuePath = "idOrdenCompra";
                ltsNumeroOC.SelectedIndex = 0;
            }
            catch (NullReferenceException)
            {


            }

        }

        private void dpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                String consulta = " Select * from ordencompra t1 where t1.fecha = @valor ";
                DataTable OCFecha = conexion.ConsultaParametrizada(consulta, dpFecha.SelectedDate);
                ltsNumeroOC.ItemsSource = OCFecha.AsDataView();
                ltsNumeroOC.DisplayMemberPath = "idOrdenCompra";
                ltsNumeroOC.SelectedValuePath = "idOrdenCompra";
                ltsNumeroOC.SelectedIndex = 0;
            }
            catch (NullReferenceException)
            {


            }
        }
    }
}
