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

namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Lógica de interacción para DevolucionFactura.xaml
    /// </summary>
    public partial class DevolucionFactura : Page
    {

        DataTable productos;

        CRUD conexion = new CRUD();
        public DevolucionFactura()
        {
            InitializeComponent();
            loadLtsFactura();
           
        }

       

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarNCFactura();
            newW.ShowDialog();
        }

        public void loadLtsFactura()
        {
            String consulta = "select * from factura";
            conexion.Consulta(consulta, tabla: ltsfacturas);
            ltsfacturas.DisplayMemberPath = "numeroFactura";
            ltsfacturas.SelectedValuePath = "idfacturas";
            ltsfacturas.SelectedIndex = 0;
        }

        private void ltsfacturas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
   
                String consulta2 = "SELECT * FROM factura f where f.idfacturas ='" + ltsfacturas.SelectedValue + "'";
                DataTable OC = conexion.ConsultaParametrizada(consulta2, ltsfacturas.SelectedValue);

                String idOC = "SELECT FK_idOC FROM factura WHERE idfacturas = '" + ltsfacturas.SelectedValue + "' ";
                String idorc = conexion.ValorEnVariable(idOC);


                String idprov = "SELECT FK_idProveedor FROM ordencompra WHERE idOrdenCompra = '" + idorc + "' ";
                String idprove = conexion.ValorEnVariable(idprov);

                String nombreprove = "SELECT nombre FROM proveedor WHERE idProveedor = '" + idprove + "' ";
                String nombrepv = conexion.ValorEnVariable(nombreprove);

                txtProveedor.Text = nombrepv;





            }
            catch (Exception)
            {


            }
        }
    }
}
