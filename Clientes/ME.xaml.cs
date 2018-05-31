using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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

namespace wpfFamiliaBlanco.Clientes
{
    /// <summary>
    /// Interaction logic for ME.xaml
    /// </summary>
    public partial class ME : Page
    {

        public static String idclientme;
        CRUD conexion = new CRUD();
        public List<Contacto> listaContacto = new List<Contacto>();
        public int idcliente;
        DataTable listadeprecios;

        public ME()
        {
            InitializeComponent();
            
            loadListaClientes();
            ActualizaDGVContacto();
            ActualizaDGVPrecios();
            ltsClientes.SelectedIndex = 0;
            LlenarComboFiltro();
            CampLimit();
        }

    

        private void CampLimit()
        {
            txtcp.IsReadOnly = true;
            txtDireccion.IsReadOnly = true;
            txtPais.IsReadOnly = true;
            txtt.IsReadOnly = true;
            txtweb.IsReadOnly = true;
            dgvContacto.IsReadOnly = true;
            dgvPrecios.IsReadOnly = true;
            txtlp.IsReadOnly = true;

            dgvPrecios.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre1");
            dgvPrecios.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio de Lista";
            textColumn2.Binding = new Binding("preciolista");
            dgvPrecios.Columns.Add(textColumn2);
        }


        private void ActualizaDGVPrecios()
        {
            dgvPrecios.Items.Refresh();

            String consultalp = "Select lp.nombre,p.nombre,plp.preciolista, c.idclienteme FROM listadeprecios lp, productos_has_listadeprecios plp, clientesme c, productos p WHERE c.FK_idLista = plp.FK_idLista AND lp.idLista = plp.FK_idLista AND p.idProductos = plp.FK_idProductos AND c.idClienteme = @valor ";


            listadeprecios = conexion.ConsultaParametrizada(consultalp, ltsClientes.SelectedValue);


            dgvPrecios.ItemsSource = listadeprecios.AsDataView();





            dgvPrecios.Items.Refresh();


        }


        private void loadListaClientes()
        {
            try
            {
            String consult = "SELECT * FROM clientesme";
            conexion.Consulta(consult, ltsClientes);
            ltsClientes.DisplayMemberPath = "nombre";
            ltsClientes.SelectedValuePath = "idClienteme";
            }
            catch (NullReferenceException)
            {

              
            }

        }

        public void ActualizaDGVContacto()
        {
            try
            {
                String consultaContacto = "SELECT contactocliente.telefono, contactocliente.email, contactocliente.nombrecontacto from contactocliente WHERE FK_idClienteme=@valor";
                DataTable contacto = conexion.ConsultaParametrizada(consultaContacto, ltsClientes.SelectedValue);
                dgvContacto.ItemsSource = contacto.AsDataView();
                listaContacto.Clear();
                for (int i = 0; i < contacto.Rows.Count; i++)
                {
                    listaContacto.Add(new Contacto(contacto.Rows[i].ItemArray[2].ToString(), contacto.Rows[i].ItemArray[1].ToString(), contacto.Rows[i].ItemArray[0].ToString()));
                }

                dgvContacto.ItemsSource = listaContacto;
                dgvContacto.Items.Refresh();
            
            }

            catch (NullReferenceException)
            {

                
            }
    

        }


        private void ltsClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizaDGVContacto();
            ActualizaDGVPrecios();

            try
            {
                //consulta datosclientes
                String consulta = "SELECT * from clientesme WHERE idClienteme = @valor";
                DataTable clienteme = conexion.ConsultaParametrizada(consulta, ltsClientes.SelectedValue);
                txtPais.Text = clienteme.Rows[0].ItemArray[2].ToString();
                txtcp.Text = clienteme.Rows[0].ItemArray[7].ToString();

                if (clienteme.Rows[0].ItemArray[5].ToString()=="0")
                {
                    txtt.Text = "CIF";
                }else if (clienteme.Rows[0].ItemArray[5].ToString()=="1")
                {
                    txtt.Text = "Ex-Works";
                }else if (clienteme.Rows[0].ItemArray[5].ToString()=="2")
                {
                    txtt.Text = "FAS";
                }else if (clienteme.Rows[0].ItemArray[5].ToString() == "3")
                {
                    txtt.Text = "FOB";
                }else if (clienteme.Rows[0].ItemArray[5].ToString() == "4")
                {
                    txtt.Text = "FCA";
                }else if (clienteme.Rows[0].ItemArray[5].ToString() == "5")
                {
                    txtt.Text = "C&F";
                }else if (clienteme.Rows[0].ItemArray[5].ToString() == "6")
                {
                    txtt.Text = "CIP";
                }

                

                txtDireccion.Text = clienteme.Rows[0].ItemArray[1].ToString();
                txtweb.Text = clienteme.Rows[0].ItemArray[3].ToString();


                if (clienteme.Rows[0].ItemArray[6].ToString() == "")
                {
                    txtlp.Text = "No tiene lista de precios";
                }
                else
                {
                    int id = (int)clienteme.Rows[0].ItemArray[6];
                    String consultan = "Select nombre from listadeprecios where idLista='" + id + "'";
                    String nombre = conexion.ValorEnVariable(consultan);
                    txtlp.Text = nombre;
                }

            }
            catch (Exception)
            {

            }
                

            







        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                string consulta = "SELECT count(*) from ordencomprasalida where FK_idClienteme = " + ltsClientes.SelectedValue + "";
                if (conexion.ValorEnVariable(consulta) == "0")
                {
                    DataRow selectedDataRow = ((DataRowView)ltsClientes.SelectedItem).Row;
                    string nombre = selectedDataRow["nombre"].ToString();
                    MessageBoxResult dialog = MessageBox.Show("¿Está seguro que desea eliminar al cliente " + nombre + ", se eliminarán todos sus datos ", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (dialog == MessageBoxResult.Yes)
                    {

                        int idSeleccionado = (int)ltsClientes.SelectedValue;
                        string sql = "delete from clientesme where idClienteme = '" + idSeleccionado + "'";
                        conexion.operaciones(sql);
                        loadListaClientes();
                        ActualizaDGVContacto();
                        ActualizaDGVPrecios();

                        if (dgvContacto.Items == null)
                        {

                            this.txtt.Text = "";
                            this.txtDireccion.Text = "";
                            this.txtPais.Text = "";
                            this.txtweb.Text = "";
                            this.txtcp.Text = "";


                        }
                    }
                    MessageBox.Show("Se elimino correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    ltsClientes.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("El cliente no se puede eliminar porque tiene ordenes de compra", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un cliente a eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarClienteme();
            
            newW.ShowDialog();
            

            if (newW.DialogResult == true)
            {

                String nombre = newW.txtNombre.Text;
                String direccionweb = newW.txtdireccionweb.Text;
                String pais = newW.cmbpais.Text;
                String direccion = newW.txtDireccion.Text;
                String cuitpais = newW.txtcp.Text;
                int tc = newW.cmbtc.SelectedIndex;




                //INSERTAR DATOS PRINCIPALES
                
                if (newW.cmbPrecios.Text == "")
                {
                    String sql;
                    sql = "insert into clientesme(nombre, web, pais, direccion, terminocomercial, cuitpais) values('" + nombre + "', '" + direccionweb + "', '" + pais + "', '" + direccion + "', '" + tc + "','" + cuitpais + "')";
                    conexion.operaciones(sql);

                }
                else
                {
                    String sql;
                    sql = "insert into clientesme(nombre, web, pais, direccion, terminocomercial, cuitpais, FK_idLista) values('" + nombre + "', '" + direccionweb + "', '" + pais + "', '" + direccion + "', '" + tc + "','" +  cuitpais + "','" + newW.idlp + "' )";
                    conexion.operaciones(sql);
                }

                String sql2 = "Select idClienteme from clientesme order by idClienteme DESC LIMIT 1";
                idclientme = conexion.ValorEnVariable(sql2);

                //Console.WriteLine("ULTIMO ID" + idProv);

                //INSERTAR CONTACTO PROVEEDOR
                String sqlContacto;
                string ultimoId = "Select last_insert_id()";
                String id = conexion.ValorEnVariable(ultimoId);

                for (int i = 0; i < newW.lista.Count; i++)
                {


                    String nombreL = newW.lista[i].NombreContacto;
                    String telefonoL = newW.lista[i].NumeroTelefono;
                    String emailL = newW.lista[i].Email;
                    sqlContacto = "insert into contactocliente(telefono, email, nombreContacto, FK_idClienteme) values('" + telefonoL + "', '" + emailL + "', '" + nombreL + "', '" + idclientme + "')";
                    conexion.operaciones(sqlContacto);
                }
                // loadListaProducto();

                //INSERTAR CATEGORIAS PROVEEDOR


                loadListaClientes();
                this.ltsClientes.Items.Refresh();
                InitializeComponent();
                loadListaClientes();
                MessageBox.Show("Se agregó el cliente correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            ltsClientes.SelectedIndex = ltsClientes.Items.Count - 1;
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {

            try
            {

       
            
            int modificado;
            idcliente = (int)ltsClientes.SelectedValue;
            modificado = ltsClientes.SelectedIndex;
            String consulta = "SELECT * FROM clientesme where idclienteme=@valor";
            DataTable cliente = conexion.ConsultaParametrizada(consulta, ltsClientes.SelectedValue);
            string direccion = cliente.Rows[0].ItemArray[1].ToString();
            string  pais = cliente.Rows[0].ItemArray[2].ToString();
            string web = cliente.Rows[0].ItemArray[3].ToString();
            string nombre = cliente.Rows[0].ItemArray[4].ToString();
            string cuitpais = cliente.Rows[0].ItemArray[7].ToString();
            int terminocomercial = (int)cliente.Rows[0].ItemArray[5];

            int listadeprecios;
            if (cliente.Rows[0].ItemArray[6].ToString() == "")
            {
                listadeprecios = 0;
            }
            else
            {
                listadeprecios = (int)cliente.Rows[0].ItemArray[6];

            }






            var newW = new windowAgregarClienteme(nombre, direccion, pais, terminocomercial,cuitpais, web, listaContacto,idcliente,listadeprecios);

            newW.ShowDialog();
            


            if (newW.DialogResult == true)
            {
                int id = (int)ltsClientes.SelectedValue;
                int termino = newW.cmbtc.SelectedIndex;
                String country = newW.cmbpais.Text;
                String nombreActu = newW.txtNombre.Text;
                String address = newW.txtDireccion.Text;
                String webpage = newW.txtdireccionweb.Text;
                String cuitcountry = newW.txtcp.Text;
                
                this.dgvContacto.ItemsSource = newW.dgvContacto.ItemsSource;
                

              
                if (newW.cmbPrecios.Text == "")
                {
                    String update;
                    update = "update clientesme set nombre = '" + nombreActu + "', direccion = '" + address + "', pais = '" + country + "', web = '" + webpage + "', terminocomercial = '" + termino + "', cuitpais = '"+cuitcountry+ "' where idClienteme ='" + idcliente + "';";
                    conexion.operaciones(update);
                }
                else
                {
                    
                    String update;
                    update = "update clientesme set nombre = '" + nombreActu + "', direccion = '" + address + "', pais = '" + country + "', web = '" + webpage + "', terminocomercial = '" + termino + "', FK_idLista = '" + newW.idlp + "', cuitpais = '"+cuitcountry+"' where idClienteme='"+idcliente+"';";
                    conexion.operaciones(update);
                }



                String contact = "delete  from contactocliente where FK_idClienteme= '" + idcliente  + "'";
                conexion.operaciones(contact);

               

               
                    foreach (var contacto in newW.lista)
                    {
                        string sql;
                        sql= "INSERT INTO contactocliente (telefono,email,nombrecontacto,FK_idClienteme) values('"+contacto.NumeroTelefono+"', '"+contacto.Email+"', '"+contacto.NombreContacto+"', '"+idcliente+"')";
                        conexion.operaciones(sql);
                    }
                loadListaClientes();
            }
            

            ltsClientes.SelectedIndex = modificado;

            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un cliente a modificar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LlenarComboFiltro()
        {

            cmbFiltro.Items.Add("Nombre");
            


        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {

            // Busquedas de clientes.
            DataTable clientes = new DataTable();
            String consulta;

            if (cmbFiltro.Text == "Nombre")
            {   //Busca por nombre
                consulta = "SELECT* FROM clientesme WHERE nombre LIKE '%' @valor '%'";
                clientes = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            }
            

            ltsClientes.ItemsSource = clientes.AsDataView();
        }
    }
}