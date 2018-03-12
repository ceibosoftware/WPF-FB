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
    /// Lógica de interacción para DevolucionRemito.xaml
    /// </summary>
    public partial class DevolucionRemito : Page
    {

        CRUD conexion = new CRUD();
        DataTable productos;
        List<Producto> productosparametro = new List<Producto>();
        public DevolucionRemito()
        {
            InitializeComponent();
            loadLtsRemitos();
            loadProductosRemitos();
        }

        public void loadLtsRemitos()
        {
            String consulta = "select * from remito";
            conexion.Consulta(consulta, tabla: ltsRemitos);
            ltsRemitos.DisplayMemberPath = "numeroRemito";
            ltsRemitos.SelectedValuePath = "idremitos";
            ltsRemitos.SelectedIndex = 0;
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ltsRemitos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Datos Remito

            String sql = "Select  t3.nombre,t1.fecha ,t2.idOrdenCompra from remito t1 , ordencompra t2, proveedor t3 where idremitos = @valor and t1.FK_idOC = t2.idOrdenCompra and t2.FK_idProveedor = t3.idProveedor";
            DataTable datos = conexion.ConsultaParametrizada(sql, ltsRemitos.SelectedValue);
          /*  if (datos.Rows.Count > 0)
            {
                lblProvR.Content = datos.Rows[0].ItemArray[0].ToString();
                DateTime fecha = (DateTime)datos.Rows[0].ItemArray[1];
                this.fecha = (DateTime)datos.Rows[0].ItemArray[1];
                lblFechaR.Content = fecha.ToString("dd/MM/yyyy");
                lblNroOCR.Content = datos.Rows[0].ItemArray[2].ToString();
            }*/
            //consulta productos
            String consulta = "  SELECT t2.nombre , t1.cantidad, t2.idProductos from productos_has_remitos t1 inner join productos t2  on t1.FK_idProducto = t2.idProductos where t1.FK_idRemito = @valor";
            productos = conexion.ConsultaParametrizada(consulta, ltsRemitos.SelectedValue);
            productosparametro.Clear();
            for (int i = 0; i < productos.Rows.Count; i++)
            {
                productosparametro.Add(new Producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[2], (int)productos.Rows[i].ItemArray[1]));
            }

            dgvProductosRemito.Items.Refresh();

        }

        private void loadProductosRemitos()
        {
            dgvProductosRemito.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvProductosRemito.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Cantidad";
            textColumn2.Binding = new Binding("cantidad");
            dgvProductosRemito.Columns.Add(textColumn2);
            dgvProductosRemito.ItemsSource = productosparametro;
        }
    }
}
