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
        List<Producto> productosparametro = new List<Producto>();
        List<Producto> itemsNC = new List<Producto>();
        List<Producto> itemsRemito = new List<Producto>();
        public List<Producto> productosAmodificar = new List<Producto>();
        String lastid;
        public Producto producto;
        DataTable productos;
        public DevolucionRemito()
        {
            InitializeComponent();
            loadLtsNCRemitos();
            loadDGVproductosRemitos();
            loadLtsNCRemitos();
            loadProductosNCRemitos();
            dgvProductosRemito.IsReadOnly = true;
        }

        public void loadLtsNCRemitos()
        {
            String consulta = "select * from notacredito WHERE FK_idremitos IS NOT NULL";
            conexion.Consulta(consulta, tabla: ltsRemitos);
            ltsRemitos.DisplayMemberPath = "idNotaCredito";
            ltsRemitos.SelectedValuePath = "idNotaCredito";
            ltsRemitos.SelectedIndex = 1;
            ltsRemitos.SelectedIndex = 0;
        }

        public void loadDGVproductosRemitos()
        {
            dgvProductosRemito.ItemsSource = productosparametro;
            dgvProductosRemito.Items.Refresh();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new WindowAgregarNCRemito();
            newW.ShowDialog();


            if (newW.DialogResult == true)
            {

                String insertNCR = "INSERT INTO notacredito (FK_idremitos) VALUES ('" + newW.idRemito + "')";
                conexion.operaciones(insertNCR);

                string ultimoId = "Select last_insert_id()";
                lastid = conexion.ValorEnVariable(ultimoId);

                foreach (Producto p in newW.itemsNC)
                {
                    String nombre = p.nombre;
                    int cantidad = p.cantidad;
                    float totalp = p.total;
                    float precioUni = p.precioUnitario;
                    int idp = p.id;

                    Producto pr = new Producto(nombre, idp, cantidad, totalp, precioUni);
                    itemsNC.Add(pr);

                    String productostNCR = "INSERT INTO productos_has_notacredito (FK_idNotaCredito, FK_idProductos, cantidad) VALUES ('" + lastid + "','" + idp + "', '" + cantidad + "')";
                    conexion.operaciones(productostNCR);

                }

                foreach (var producto in newW.productosparametro)
                {
                    String updateCRremito = "UPDATE productos_has_remitos SET CrNotaCredito = '" + producto.cantidad + "' where FK_idProducto = '" + producto.id + "' and FK_idRemito = '" + newW.idRemito + "'";
                    conexion.operaciones(updateCRremito);
                }
            }
            loadLtsNCRemitos();
            ltsRemitos.Items.MoveCurrentToLast();
         }

        private void ltsRemitos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            String productosFatura = "SELECT DISTINCT n.idProductoNC,n.FK_idNotaCredito,n.FK_idProductos,n.cantidad,n.precioUnitario, p.nombre FROM productos_has_notacredito n,  productos p WHERE n.FK_idNotaCredito ='"+ltsRemitos.SelectedValue+"' AND n.FK_idProductos = p.idProductos";

            productos = conexion.ConsultaParametrizada(productosFatura, ltsRemitos.SelectedValue);
            dgvProductosRemito.ItemsSource = productos.AsDataView();
            productosparametro.Clear();
            for (int i = 0; i < productos.Rows.Count; i++)
            {
                productosparametro.Add(new Producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[2], (int)productos.Rows[i].ItemArray[1]));
            }

           
        }
        private void loadProductosNCRemitos()
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
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            int idnotacredito = (int)ltsRemitos.SelectedValue;
            productosAmodificar.Clear();


            String idr = "SELECT FK_idremitos FROM notacredito WHERE idNotaCredito = '" + ltsRemitos.SelectedValue + "'";
            String idRem = conexion.ValorEnVariable(idr);

            String productosRemito = "SELECT DISTINCT  t2.nombre,t2.idProductos,t3.cantidad from productos_has_remitos t1, productos_has_notacredito t3 inner join productos t2 where t1.FK_idProducto = t2.idProductos and t1.FK_idRemito = '" + idRem + "' and  t3.FK_idNotaCredito = '" + ltsRemitos.SelectedValue + "'";
            productos = conexion.ConsultaParametrizada(productosRemito, ltsRemitos.SelectedValue);


            for (int i = 0; i < productos.Rows.Count; i++)
            {
                producto = new Producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[1], (int)productos.Rows[i].ItemArray[2]);
                productosAmodificar.Add(producto);

            }

            var newW = new WindowAgregarNCRemito(productosAmodificar, int.Parse(idRem), idnotacredito);
            newW.Title = "Modificar Nota de Crédito";
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {

         
                int idRemito = newW.idRemito;
                itemsNC.Clear();
                foreach (Producto p in newW.itemsNC)
                {
                    String nombre = p.nombre;
                    int cantidad = p.cantidad;
                    float totalp = p.total;
                    float precioUni = p.precioUnitario;
                    int idp = p.id;

                    Producto pr = new Producto(nombre, idp, cantidad, totalp, precioUni);
                    itemsNC.Add(p);

      

                    String updateProductosNC = "UPDATE productos_has_notacredito SET FK_idNotaCredito = '" + newW.idNotaCred + "', FK_idProductos = '" + idp + "', cantidad = '" + cantidad + "' WHERE FK_idNotaCredito = '" + newW.idNotaCred + "' AND FK_idProductos = '" + idp + "'";
                    conexion.operaciones(updateProductosNC);

                    foreach (var producto in newW.productosparametro)
                    {
                        String sql = "UPDATE productos_has_remitos SET CrNotaCredito = '" + producto.cantidad + "' where FK_idProducto = '" + producto.id + "' and FK_idRemito = '" + idRemito + "'";
                        conexion.operaciones(sql);
                    }


                }
                loadLtsNCRemitos();
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String idremi="";


                MessageBoxResult dialog = MessageBox.Show("Esta seguro que desea eliminar la nota de crédito numero :" + ltsRemitos.SelectedValue, "Advertencia", MessageBoxButton.YesNo);


                if (dialog == MessageBoxResult.Yes)
                {

                    for (int i = 0; i < productos.Rows.Count; i++)
                    {
                        String idre = "SELECT FK_idremitos FROM notacredito WHERE idNotaCredito = '" + ltsRemitos.SelectedValue + "'";
                         idremi = conexion.ValorEnVariable(idre);

                        String consulta = "UPDATE productos_has_remitos SET CrNotaCredito = CrNotaCredito + '" + (int)productos.Rows[i].ItemArray[3] + "' where FK_idProducto = '" + (int)productos.Rows[i].ItemArray[2] + "' and FK_idRemito = '" + idremi + "'";
                        conexion.operaciones(consulta);
                    }
          

                    string sql2 = "DELETE  FROM notacredito WHERE idNotaCredito = '" + ltsRemitos.SelectedValue + "' AND FK_idremitos = '" + idremi + "'";
                    conexion.operaciones(sql2);

                    string sql3 = " DELETE  FROM productos_has_notacredito WHERE FK_idNotaCredito =  '" + ltsRemitos.SelectedValue + "'";
                    conexion.operaciones(sql3);

         
                    ltsRemitos.Items.Refresh();
                   loadLtsNCRemitos();
                    ltsRemitos.SelectedIndex = 0;
                }

            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione una Nota de Crédito a eliminar");
            }
        }

        private void dpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                String consulta = " Select * from notacredito t1, remito t2  where  t2.fecha = @valor and t1.FK_idremitos = t2.idremitos  ";
                DataTable OCFecha = conexion.ConsultaParametrizada(consulta, dpFecha.SelectedDate);
                ltsRemitos.ItemsSource = OCFecha.AsDataView();
                ltsRemitos.DisplayMemberPath = "idNotaCredito";
                ltsRemitos.SelectedValuePath = "idNotaCredito";
                ltsRemitos.SelectedIndex = 0;
            }
            catch (NullReferenceException)
            {


            }
        }

        private void btnVertodo_Click(object sender, RoutedEventArgs e)
        {
            String consulta = "select * from notacredito  WHERE FK_idremitos IS NOT NULL";
            conexion.Consulta(consulta, tabla: ltsRemitos);
            ltsRemitos.DisplayMemberPath = "idNotaCredito";
            ltsRemitos.SelectedValuePath = "idNotaCredito";
            ltsRemitos.SelectedIndex = 0;
        }
    }
}
