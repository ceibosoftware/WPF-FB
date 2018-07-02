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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace wpfFamiliaBlanco.Clientes
{
    /// <summary>
    /// Lógica de interacción para LP.xaml
    /// </summary>
    public partial class LP : Page
    {
        CRUD conexion = new CRUD();
        DataTable listadeprecios;
        List<Producto> lp=new List<Producto>();
        bool bandera = false;
        
        

        public LP()
        {
            InitializeComponent();
            loadlp();
            dgvLp.IsReadOnly = true;
            Camplimit();
            cargalp();
            ActualizaDGVlp();
            ltsLp.SelectedIndex = 0;


        }

        private void Camplimit()
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
        }

        private void ActualizaDGVlp()
        {

            if (!bandera) {
            dgvLp.Items.Refresh();

            String consultalp = "SELECT p.nombre, plp.preciolista, plp.FK_idProductos from productos p,productos_has_listadeprecios plp where FK_idLista = @valor and FK_idProductos=p.idProductos";
            listadeprecios = conexion.ConsultaParametrizada(consultalp, ltsLp.SelectedValue);

          
            dgvLp.ItemsSource = listadeprecios.AsDataView();

            }


            
            dgvLp.Items.Refresh();


        }

       
        private void cargalp()
        {
           /* dgvLp.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvLp.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio de Lista";
            textColumn2.Binding = new Binding("preciolista");
            dgvLp.Columns.Add(textColumn2);*/
        }
        private void loadlp()
        {


            String consulta = "Select * from listadeprecios WHERE tipo=1";
            conexion.Consulta(consulta, ltsLp);
            ltsLp.DisplayMemberPath = "nombre";
            ltsLp.SelectedValuePath = "idLista";
            
        }

       

        private void ltsLp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!bandera) { 
            int seleccionado = (int)ltsLp.SelectedValue;


             
            String consulta = "Select fecha from listadeprecios where idLista='" + seleccionado + "'";
            String fecha = conexion.ValorEnVariable(consulta);
            String consulta2 = "Select nombre from listadeprecios where idLista='" + seleccionado + "'";
            String nombre = conexion.ValorEnVariable(consulta2);


            lblultimam.Content = DateTime.Parse(fecha).ToString("yyyy/MM/dd");
            lblnombre.Content = nombre;
            ActualizaDGVlp();
            }

        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {

            bandera = true;
            var newW = new windowAgregarLp();
            string nombre;
           // newW.lblpreciolista.Visibility = Visibility.Hidden;
            //newW.txtPreciolista.Visibility = Visibility.Hidden;
            //newW.btnModpl.Visibility = Visibility.Hidden;
            newW.ShowDialog();

            if (newW.DialogResult==true)
            {
                nombre = newW.txtNombre.Text;
                DateTime hoy;
                hoy = DateTime.Today;


                

                
                
                String sql;
                sql = "INSERT into listadeprecios(nombre, fecha,tipo) values('" + nombre + "', '" + hoy.ToString("yyyy/MM/dd") + "','"+1+"')";
                conexion.operaciones(sql);

                string ultimoId = "Select last_insert_id()";
                String id = conexion.ValorEnVariable(ultimoId);


                String consulta;


                for (int i = 0; i < newW.itemslp.Count; i++)
                {
                    
                    int fkidp = newW.itemslp[i].id;
                    double preciolista = newW.itemslp[i].preciolista;
                    consulta = "INSERT into productos_has_listadeprecios(FK_idProductos, FK_idLista, precioLista) values('" + fkidp + "', '" + id + "', '" + preciolista + "')";
                    conexion.operaciones(consulta);




                }

                
                loadlp();
                ltsLp.SelectedIndex = ltsLp.Items.Count - 1;
                bandera = false;
                ActualizaDGVlp();
                MessageBox.Show("Se agrego la lista de precio correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }


            
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            bandera = true;
            string consulta2 = "SELECT count(*) from clientesme where FK_idLista = " + ltsLp.SelectedValue +"";
            string consulta3 = "SELECT count(*) from clientesmi where FK_idLista = " + ltsLp.SelectedValue + "";
            string cliente = conexion.ValorEnVariable(consulta2);
            string cliente2 = conexion.ValorEnVariable(consulta3);
            try
            {
                if (int.Parse(cliente2) != 0)
                {
                    MessageBox.Show("No se puede eliminar la lista de precios por que esta asignada a clientes", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    DataRow seleccionada = ((DataRowView)ltsLp.SelectedItem).Row;
                    string nombre = seleccionada["nombre"].ToString();
                    MessageBoxResult resultado = MessageBox.Show("Esta seguro que desea eliminar la Lista de precios: " + nombre, "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (resultado == MessageBoxResult.Yes)
                    {
                        int id = (int)ltsLp.SelectedValue;
                        String sqlelimancion = "DELETE FROM listadeprecios WHERE listadeprecios.idLista = '" + id + "'";
                        conexion.operaciones(sqlelimancion);

                    }
                    ltsLp.Items.Refresh();
                    ltsLp.SelectedIndex = 0;
                    loadlp();


                }               
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione una Lista de precios a eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        

            }
            ltsLp.SelectedIndex = ltsLp.Items.Count - 1;
            bandera = false;
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            bandera = true;

            try
            {

           
                int modificado;
                modificado = ltsLp.SelectedIndex;
                int idlista = (int)ltsLp.SelectedValue;
                String nombrelp;
                String fecha;

                DateTime hoy;
                hoy = DateTime.Today;

                String consulta = "SELECT nombre from listadeprecios where idLista='" + idlista + "';";
                String nombre = conexion.ValorEnVariable(consulta);

                fecha = hoy.ToString("yyyy/MM/dd");

                nombrelp = nombre;

                var newW = new windowAgregarLp(idlista,nombrelp, listadeprecios, fecha);

                for (int i = 0; i < newW.itemslp.Count; i++)
                {
                    for (int j = 0; j < newW.items.Count; j++)
                    {
                        if (newW.itemslp[i].nombre == newW.items[j].nombre)
                        {
                            newW.items.Remove(newW.items[j]);
                        }
                    }
                    
                }
              
               
                newW.ShowDialog();

            if (newW.DialogResult == true)
            {
                
                hoy = DateTime.Today;
                fecha = hoy.ToString("yyyy/MM/dd");
                nombre = newW.txtNombre.Text;

                String update = "update listadeprecios set nombre = '" + nombre + "', fecha = '" + fecha + "' where idLista = '" + idlista + "'; ";
                conexion.operaciones(update);


                String producto = "delete from productos_has_listadeprecios where FK_idLista= '" + idlista + "'";
                conexion.operaciones(producto);




                foreach (var item in newW.itemslp)
                {
                    string sql;
                    sql = "INSERT INTO productos_has_listadeprecios (FK_idLista,precioLista,FK_idProductos) values('" + idlista + "', '" + item.preciolista + "' , '" +item.id+"')";
                    conexion.operaciones(sql);
                }
                loadlp();
                ActualizaDGVlp();
                
                
            }


                ltsLp.Items.Refresh();

               
                ltsLp.SelectedIndex = modificado;
                ActualizaDGVlp();

                lblnombre.Content = nombre;
                lblultimam.Content = hoy.ToString("yyyy/MM/dd");

            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione una Lista de precios a modificar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

           
            bandera=false;
            
            
            ActualizaDGVlp();
            



        }

        private void btnPdf_Click(object sender, RoutedEventArgs e)
        {

            Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream("LP.pdf", FileMode.Create));
            doc.Open();
            var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);

            string imageURL = "C:\\Users\\Lenovo\\Source\\Repos\\CAFinal\\logo.png";
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            jpg.Alignment = Element.ALIGN_CENTER;
            //Resize image depend upon your need 
            jpg.ScaleToFit(140f, 120f);
            //Give space before image 
            jpg.SpacingBefore = 10f;
            //Give some space after the image 
            jpg.SpacingAfter = 1f;
            doc.Add(jpg);
           

         
            PdfPTable table1 = new PdfPTable(1);
            table1.AddCell("Productos");
            PdfPTable table = new PdfPTable(4);

            table.AddCell("Cantidad");
            table.AddCell("Producto");
            table.AddCell("Precio Unitario");
            table.AddCell("Total");
            PdfPTable producto = new PdfPTable(4);
            for (int i = 0; i < this.listadeprecios.Rows.Count; i++)
            {
                producto.AddCell(listadeprecios.Rows[i].ItemArray[1].ToString());
                producto.AddCell(listadeprecios.Rows[i].ItemArray[0].ToString());
                producto.AddCell(listadeprecios.Rows[i].ItemArray[2].ToString());
            }
            doc.Add(table1);
            doc.Add(table);
            doc.Add(producto);
            doc.OpenDocument();
            
        }
    }
}
