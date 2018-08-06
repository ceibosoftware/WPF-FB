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
    /// Lógica de interacción para LPME.xaml
    /// </summary>
    public partial class LPME : Page
    {

        CRUD conexion = new CRUD();
        List<Producto> lp = new List<Producto>();
        DataTable listadeprecios;
        bool bandera = false;


        public LPME()
        {
            InitializeComponent();
            loadlp();
            dgvLpme.IsReadOnly = true;
            Camplimit();
            ActualizaDGVlp();
            ltsLpme.SelectedIndex = 0;
        }


        private void loadlp()
        {
            String consulta = "Select * from listadeprecios WHERE tipo=1";
            conexion.Consulta(consulta, ltsLpme);
            ltsLpme.DisplayMemberPath = "nombre";
            ltsLpme.SelectedValuePath = "idLista";

        }

        private void Camplimit()
        {
            dgvLpme.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvLpme.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio de Lista";
            textColumn2.Binding = new Binding("preciolista");
            dgvLpme.Columns.Add(textColumn2);
        }

        private void ActualizaDGVlp()
        {

            if (!bandera)
            {
                dgvLpme.Items.Refresh();

                String consultalp = "SELECT p.nombre, plp.preciolista, plp.FK_idProductos from productos p,productos_has_listadeprecios plp where FK_idLista = @valor and FK_idProductos=p.idProductos";
                listadeprecios = conexion.ConsultaParametrizada(consultalp, ltsLpme.SelectedValue);


                dgvLpme.ItemsSource = listadeprecios.AsDataView();

            }



            dgvLpme.Items.Refresh();


        }

        private void ltsLpme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!bandera)
            {
                int seleccionado = (int)ltsLpme.SelectedValue;



                String consulta = "Select fecha from listadeprecios where idLista='" + seleccionado + "'";
                String fecha = conexion.ValorEnVariable(consulta);
                String consulta2 = "Select nombre from listadeprecios where idLista='" + seleccionado + "'";
                String nombre = conexion.ValorEnVariable(consulta2);
                String consulta3 = "Select anexo from listadeprecios where idLista='" + seleccionado + "'";
                String anexo = conexion.ValorEnVariable(consulta3);
                String consulta4 = "Select moneda from listadeprecios where idLista='" + seleccionado + "'";
                String moneda = conexion.ValorEnVariable(consulta4);


                lblultimam.Content = DateTime.Parse(fecha).ToString("yyyy/MM/dd");
                lblnombre.Content = nombre;
                if (moneda=="1")
                {
                    lblcoin.Content = "USD";
                }
                else
                {
                    lblcoin.Content = "EUROS";
                }
                if (anexo=="")
                {
                    lbltienea.Content = "No tiene anexo";
                }
                else
                {
                    lbltienea.Content = anexo;
                }
                
                ActualizaDGVlp();
            }
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {


            bandera = true;
            var newW = new WindowAgregarLpme();
            string nombre;
            string anexo;
            newW.lblpreciolista.Visibility = Visibility.Collapsed;
            newW.txtPreciolista.Visibility = Visibility.Collapsed;
            newW.btnModpl.Visibility = Visibility.Collapsed;
            newW.ShowDialog();
            int moneda;

            if (newW.DialogResult == true)
            {
                nombre = newW.txtNombre.Text;
                DateTime hoy;
                hoy = DateTime.Today;
                if (newW.txtAnexo.Text=="")
                {
                    anexo = "";
                }
                else
                {
                    anexo = newW.txtAnexo.Text;
                }


                if (newW.cmbMoneda.Text=="USD")
                {
                    moneda = 1;
                }
                else
                {
                    moneda = 2;
                }
               



                if (newW.txtAnexo.Text=="")
                {
                    String sql;
                    sql = "INSERT into listadeprecios(nombre, fecha,tipo,moneda) values('" + nombre + "', '" + hoy.ToString("yyyy/MM/dd") + "','" + 1 +"','"+moneda+"')";
                    conexion.operaciones(sql);
                }
                else
                {
                    String sql;
                    sql = "INSERT into listadeprecios(nombre, fecha,tipo,anexo,moneda) values('" + nombre + "', '" + hoy.ToString("yyyy/MM/dd") + "','" + 1 + "','" + anexo + "','"+moneda+"')";
                    conexion.operaciones(sql);
                }
               

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
                ltsLpme.SelectedIndex = ltsLpme.Items.Count - 1;
                bandera = false;
                ActualizaDGVlp();
                MessageBox.Show("Se agrego la lista de precio correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                bandera = false;
            }




        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            bandera = true;

            try
            {


                int modificado;
                
                modificado = ltsLpme.SelectedIndex;
                int idlista = (int)ltsLpme.SelectedValue;
                String nombrelp;
                String fecha;
                String anexo;

                int coin;

                DateTime hoy;
                hoy = DateTime.Today;

                String consulta = "SELECT nombre from listadeprecios where idLista='" + idlista + "';";
                String nombre = conexion.ValorEnVariable(consulta);

                fecha = hoy.ToString("yyyy/MM/dd");

                nombrelp = nombre;
                String consulta2 = "SELECT anexo from listadeprecios where idLista='" + idlista + "';";
                String anexolista = conexion.ValorEnVariable(consulta2);
                String consulta3 = "SELECT moneda from listadeprecios where idLista='" + idlista + "';";
                String moneda = conexion.ValorEnVariable(consulta3);
                coin = int.Parse(moneda);
                anexo = anexolista;
             
                var newW = new WindowAgregarLpme(idlista, lblnombre.Content.ToString(), listadeprecios, fecha,coin,lbltienea.Content.ToString());

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
                    anexo = newW.txtAnexo.Text;
                    coin = newW.cmbMoneda.SelectedIndex+1;

                    String update = "update listadeprecios set nombre = '" + nombre + "', fecha = '" + fecha + "', anexo= '"+anexo+"', moneda='"+coin+"' where idLista = '" + idlista + "'; ";
                    conexion.operaciones(update);


                    String producto = "delete from productos_has_listadeprecios where FK_idLista= '" + idlista + "'";
                    conexion.operaciones(producto);




                    foreach (var item in newW.itemslp)
                    {
                        string sql;
                        sql = "INSERT INTO productos_has_listadeprecios (FK_idLista,precioLista,FK_idProductos) values('" + idlista + "', '" + item.preciolista + "' , '" + item.id + "')";
                        conexion.operaciones(sql);
                    }
                    loadlp();
                    ActualizaDGVlp();


                }


                ltsLpme.Items.Refresh();


                ltsLpme.SelectedIndex = modificado;
                ActualizaDGVlp();

                lblnombre.Content = nombre;
                lblultimam.Content = hoy.ToString("yyyy/MM/dd");
                lbltienea.Content = anexo;

                if (coin==1)
                {
                    lblcoin.Content = "USD";
                }else if (coin == 2)
                {
                    lblcoin.Content = "EUROS";
                }
               

            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione una Lista de precios a modificar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            bandera = false;


            ActualizaDGVlp();


        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            bandera = true;
            string consulta2 = "SELECT count(*) from clientesme where FK_idLista = " + ltsLpme.SelectedValue + "";
            string consulta3 = "SELECT count(*) from clientesmi where FK_idLista = " + ltsLpme.SelectedValue + "";
            string cliente = conexion.ValorEnVariable(consulta2);
            string cliente2 = conexion.ValorEnVariable(consulta3);
            try
            {
                if (int.Parse(cliente) != 0)
                {
                    MessageBox.Show("No se puede eliminar la lista de precios por que esta asignada a clientes", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    DataRow seleccionada = ((DataRowView)ltsLpme.SelectedItem).Row;
                    string nombre = seleccionada["nombre"].ToString();
                    MessageBoxResult resultado = MessageBox.Show("Esta seguro que desea eliminar la Lista de precios: " + nombre, "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (resultado == MessageBoxResult.Yes)
                    {
                        int id = (int)ltsLpme.SelectedValue;
                        String sqlelimancion = "DELETE FROM listadeprecios WHERE listadeprecios.idLista = '" + id + "'";
                        conexion.operaciones(sqlelimancion);

                    }
                    ltsLpme.Items.Refresh();
                    ltsLpme.SelectedIndex = 0;
                    loadlp();


                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione una Lista de precios a eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);


            }
            ltsLpme.SelectedIndex = ltsLpme.Items.Count - 1;
            bandera = false;
        }

        private void btnPdf_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
