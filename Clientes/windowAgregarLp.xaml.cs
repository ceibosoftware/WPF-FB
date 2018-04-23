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

namespace wpfFamiliaBlanco.Clientes
{
    /// <summary>
    /// Lógica de interacción para windowAgregarLp.xaml
    /// </summary>
    public partial class windowAgregarLp : Window
    {
        CRUD conexion = new CRUD();

        public List<Producto> items = new List<Producto>();
        public List<Producto> itemslp = new List<Producto>();
        public Producto item;
        public int idlistadb;
        bool bandera = false;

        public windowAgregarLp()
        {
            InitializeComponent();
            loaddgvp();
            loaddgvlp();
            camplimit();
            
            
        }

        private void camplimit()
        {
            dgvLp.IsReadOnly = true;
            dgvProductos.IsReadOnly = true;
            txtNombre.MaxLength = 20;
            txtPreciolista.MaxLength = 10;
            
            
        }
        public windowAgregarLp(int id,string nombre, DataTable itemslp, String fecha)
        {
            InitializeComponent();

            this.txtNombre.Text = nombre;
            loaddgvp();
            loadlistadeprecios(itemslp);
            idlistadb = id;
            camplimit();
            
        }

        private void loadlistadeprecios(DataTable lp)
        {

            dgvLp.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvLp.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio de Lista";
            textColumn2.Binding = new Binding("preciolista");
            dgvLp.Columns.Add(textColumn2);

            for (int i = 0; i < lp.Rows.Count; i++)
            {

                item = new Producto((int)lp.Rows[i].ItemArray[2], (float)lp.Rows[i].ItemArray[1], lp.Rows[i].ItemArray[0].ToString());
                itemslp.Add(item);
            }


            dgvLp.ItemsSource = itemslp;


        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        public Boolean Validacion()
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Complete el nombre");
                return false;
            }
            
            else if (itemslp.Count <= 0)
            {
                MessageBox.Show("Agregue un producto");
                return false;
            }

            return true;
        }




        private void btnAgregarp_Click(object sender, RoutedEventArgs e)
        {

            try
            {

          
            Producto p = dgvProductos.SelectedItem as Producto;
            

            var newW = new windowAgregarProducto();

            newW.txtnombre.Text = p.nombre;
            newW.txtpreciou.Text = p.precioUnitario.ToString();

                newW.ShowDialog();


                float preciolista;
                if (newW.DialogResult == true)
                {

                
                    preciolista = float.Parse(newW.txtpreciolista.Text);
                    Producto plp = new Producto(p.id, preciolista, p.nombre);
                    itemslp.Add(plp);
                
                    dgvLp.Items.Refresh();
                    items.Remove(p);
                    dgvProductos.Items.Refresh();

                
                }
                
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un producto");
            }

        }

        private Boolean validarmodificar()
        {
             if (string.IsNullOrEmpty(txtPreciolista.Text))
            {
                MessageBox.Show("Complete el precio de lista");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void loaddgvp()
        {

            dgvProductos.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvProductos.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio Unitario";
            textColumn2.Binding = new Binding("precioUnitario");
            dgvProductos.Columns.Add(textColumn2);

            try
            {
                String consultaProducto = "SELECT * from productos";
                DataTable productos = conexion.ConsultaParametrizada(consultaProducto);
                for (int i = 0; i < productos.Rows.Count; i++)
                {
                    item = new Producto((int)productos.Rows[i].ItemArray[0], productos.Rows[i].ItemArray[1].ToString(), (float)productos.Rows[i].ItemArray[5]);
                    items.Add(item);
                }


                dgvProductos.ItemsSource = items;



            }
            catch (Exception)
            {


            }

        }

        private void loaddgvlp()
        {
            dgvLp.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvLp.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio de Lista";
            textColumn2.Binding = new Binding("preciolista");
            dgvLp.Columns.Add(textColumn2);
           
            try
            {
                String consultalp = "SELECT p.nombre, plp.preciolista from productos p,productos_has_listadeprecios plp where FK_idLista = @valor and FK_idProductos=p.idProductos";
                DataTable productos = conexion.ConsultaParametrizada(consultalp);
                for (int i = 0; i < productos.Rows.Count; i++)
                {
                    item = new Producto((int)productos.Rows[i].ItemArray[0], productos.Rows[i].ItemArray[1].ToString(), (float)productos.Rows[i].ItemArray[5]);
                    itemslp.Add(item);
                }


                dgvLp.ItemsSource = itemslp;



            }
            catch (Exception)
            {


            }

        }

        private void dgvProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (Validacion())
            {
                DialogResult = true;
            }
        }

        private void btnEliminarp_Click(object sender, RoutedEventArgs e)
        {
            bandera = true;

            Producto prod;
            prod = dgvLp.SelectedItem as Producto;
            try
            {

                String consulta = "SELECT precioUnitario from productos where idProductos = '" + prod.id + "'";
                float precio = float.Parse(conexion.ValorEnVariable(consulta));

            
           

                itemslp.Remove(prod);
                dgvLp.Items.Refresh();
                Producto p = new Producto(prod.id,prod.nombre,precio);
                items.Add(p);
                dgvProductos.Items.Refresh();

            
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un producto");
            }

            bandera = false;

        }

        private void dgvLp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!bandera)
            {

                Producto selected = dgvLp.SelectedItem as Producto;

                float precio = selected.preciolista;
                txtPreciolista.Text = precio.ToString();

            }





        }

        private void btnModpl_Click(object sender, RoutedEventArgs e)
        {

            try
            {

           
            Producto selected = dgvLp.SelectedItem as Producto;
 

            MessageBoxResult dialog = MessageBox.Show("Esta seguro que desea modificar el producto :" + selected.nombre, "Advertencia", MessageBoxButton.YesNo);

            if (dialog == MessageBoxResult.Yes)
            {
                float precio = float.Parse(txtPreciolista.Text);
                selected.preciolista = precio;
                
                dgvLp.Items.Refresh();



            }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un producto");
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtPreciolista_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}

