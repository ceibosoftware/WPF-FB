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

namespace wpfFamiliaBlanco.Clientes
{
    /// <summary>
    /// Interaction logic for MI.xaml
    /// </summary>
    public partial class MI : Page
    {
        public static String idclientmi;
        CRUD conexion = new CRUD();
        List<Contacto> listaContacto = new List<Contacto>();
        DataTable listadeprecios;
        public int idcliente;

        public MI()
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
            dgvContacto.IsReadOnly = true;
            txtCuit.IsReadOnly = true;
            txtDireccion.IsReadOnly = true;
            txtrs.IsReadOnly = true;
            txtTelt.IsReadOnly = true;
            txtTransporte.IsReadOnly = true;
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
        private void loadListaClientes()
        {
            String consulta = "SELECT * FROM clientesmi";
            conexion.Consulta(consulta, ltsClientes);
            ltsClientes.DisplayMemberPath = "nombre";
            ltsClientes.SelectedValuePath = "idClientemi";
            

        }


        public void ActualizaDGVContacto()
        {
            try
            {
            String consultaContacto = "SELECT contactocliente.telefono, contactocliente.email, contactocliente.nombrecontacto from contactocliente WHERE FK_idClientemi=@valor";
            DataTable contacto = conexion.ConsultaParametrizada(consultaContacto, ltsClientes.SelectedValue);
            listaContacto.Clear();
            for (int i = 0; i < contacto.Rows.Count; i++)
            {
                listaContacto.Add(new Contacto(contacto.Rows[i].ItemArray[2].ToString(), contacto.Rows[i].ItemArray[1].ToString(), contacto.Rows[i].ItemArray[0].ToString()));
            }
            
            dgvContacto.ItemsSource = listaContacto;
            dgvContacto.Items.Refresh();
            }
            catch (Exception)
            {

               
            }

        }

        private void LlenarComboFiltro()
        {

            cmbFiltro.Items.Add("Nombre");
            cmbFiltro.Items.Add("Provincia");


        }
        private void ActualizaDGVPrecios()
        {
            dgvPrecios.Items.Refresh();

            String consultalp = "Select lp.nombre,p.nombre,plp.preciolista, c.idclientemi FROM listadeprecios lp, productos_has_listadeprecios plp, clientesmi c, productos p WHERE c.FK_idLista = plp.FK_idLista AND lp.idLista = plp.FK_idLista AND p.idProductos = plp.FK_idProductos AND c.idClientemi = @valor ";


            listadeprecios = conexion.ConsultaParametrizada(consultalp,ltsClientes.SelectedValue);


            dgvPrecios.ItemsSource = listadeprecios.AsDataView();





            dgvPrecios.Items.Refresh();


        }



        private void ltsClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            ActualizaDGVContacto();
            ActualizaDGVPrecios();

            try
            {

                //consulta datosclientes
                String consulta = "SELECT * from clientesmi WHERE idClientemi = @valor";
                DataTable clientemi = conexion.ConsultaParametrizada(consulta, ltsClientes.SelectedValue);
                txtCuit.Text = clientemi.Rows[0].ItemArray[1].ToString();
                txtDireccion.Text = clientemi.Rows[0].ItemArray[4].ToString();
                txtTelt.Text = clientemi.Rows[0].ItemArray[3].ToString();
                txtTransporte.Text = clientemi.Rows[0].ItemArray[2].ToString();
                txtProvincia.Text = clientemi.Rows[0].ItemArray[8].ToString();
            
           


                if (clientemi.Rows[0].ItemArray[5].ToString() == "0")
                {
                    txtrs.Text = "Excento";
                }
                else if (clientemi.Rows[0].ItemArray[5].ToString() == "1")
                {
                    txtrs.Text = "Responsable Inscripto";
                }
                else if (clientemi.Rows[0].ItemArray[5].ToString() == "2")
                {
                    txtrs.Text = "Monotribustista";
                }


                if (clientemi.Rows[0].ItemArray[7].ToString() == "")
                {
                    txtlp.Text = "No tiene lista de precios";
                }
                else
                {
                    int id = (int)clientemi.Rows[0].ItemArray[7];
                    String consultan = "Select nombre from listadeprecios where idLista='" + id + "'";
                    String nombre = conexion.ValorEnVariable(consultan);
                    txtlp.Text = nombre;
                }








                //consulta contacto

                //String consultaContacto = "SELECT contactoproveedor.telefono, contactoproveedor.email, contactoproveedor.nombreContacto from contactoproveedor WHERE FK_idProveedor=@valor";
                //DataTable contacto = conexion.ConsultaParametrizada(consultaContacto, ltsProveedores.SelectedValue);
                //dgvContacto.ItemsSource = contacto.AsDataView();

                //consulta categorias
                //String consultaCategoria = "SELECT categorias.nombre from categorias , categorias_has_proveedor WHERE categorias_has_proveedor.FK_idProveedor = @valor AND categorias_has_proveedor.FK_idCategorias=categorias.idCategorias";
                //DataTable categorias = conexion.ConsultaParametrizada(consultaCategoria, ltsProveedores.SelectedValue);
                //ltsCategorias.ItemsSource = categorias.AsDataView();
                //ltsCategorias.DisplayMemberPath = "nombre";


            }
            catch
            {

            }
            
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string consulta = "SELECT count(*) from ordencomprasalida where FK_idClientemi = " + ltsClientes.SelectedValue + "";
                if (conexion.ValorEnVariable(consulta) == "0") { 
                    DataRow selectedDataRow = ((DataRowView)ltsClientes.SelectedItem).Row;
                    string nombre = selectedDataRow["nombre"].ToString();
                    MessageBoxResult dialog = MessageBox.Show("Esta seguro que desea eliminar al cliente " + nombre+". Se eliminarán todos sus datos", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            int idSeleccionado = (int)ltsClientes.SelectedValue;
            if (dialog == MessageBoxResult.Yes)
            {
                
                string sql = "delete from clientesmi where idClientemi = '" + idSeleccionado + "'";
                conexion.operaciones(sql);
                loadListaClientes();
                ActualizaDGVContacto();
                ActualizaDGVPrecios();
                if (dgvContacto.Items == null)
                {

                    this.txtCuit.Text = "";
                    this.txtDireccion.Text = "";
                    this.txtrs.Text = "";
                    this.txtTelt.Text = "";
                    this.txtTransporte.Text = "";
                    this.txtProvincia.Text = "";

                }
                    MessageBox.Show("El cliente se ha eliminado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
   
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







        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Busquedas de clientes.
            DataTable clientes = new DataTable();
            String consulta;

            if (cmbFiltro.Text == "Nombre")
            {   //Busca por nombre
                consulta = "SELECT* FROM clientesmi WHERE nombre LIKE '%' @valor '%'";
                clientes = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            }else if (cmbFiltro.Text=="Provincia")
            {
                consulta = "SELECT * FROM clientesmi WHERE provincia LIKE '%' @valor '%'";
                clientes = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            }
            

            ltsClientes.ItemsSource = clientes.AsDataView();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarClientemi();
            
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {

                String nombre = newW.txtNombre.Text;
                String cuit = newW.txtCuit.Text;
                String telt = newW.txtTelt.Text;
                String provincia = newW.cmbP.Text;
                String direccion = newW.txtDireccion.Text;
                String transporte = newW.txtTransporte.Text;
               
                
                int razons = newW.cmbRs.SelectedIndex;



                //INSERTAR DATOS PRINCIPALES

                if (newW.cmbPrecios.Text == "") 
                {
                    String sql;
                    sql = "insert into clientesmi(nombre, razonsocial, cuit, teltransporte, direccionentrega, transporte, provincia) values('" + nombre + "', '" + razons + "', '" + cuit + "', '" + telt + "', '" + direccion + "', '" + transporte + "','"+provincia+"')";
                    conexion.operaciones(sql);

                }
                else
                {
                    String sql;
                    sql = "insert into clientesmi(nombre, razonsocial, cuit, teltransporte, direccionentrega, transporte, FK_idLista,provincia) values('" + nombre + "', '" + razons + "', '" + cuit + "', '" + telt + "', '" + direccion + "', '" + transporte + "','" + newW.idlp + "','"+provincia+"')";
                    conexion.operaciones(sql);
                }
               




                String sql2 = "Select idClientemi from clientesmi order by idClientemi DESC LIMIT 1";
                idclientmi = conexion.ValorEnVariable(sql2);

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
                    sqlContacto = "insert into contactocliente(telefono, email, nombreContacto, FK_idClientemi) values('" + telefonoL + "', '" + emailL + "', '" + nombreL + "', '" + idclientmi + "')";
                    conexion.operaciones(sqlContacto);
                }
                // loadListaProducto();

                //INSERTAR CATEGORIAS PROVEEDOR


                loadListaClientes();
                this.ltsClientes.Items.Refresh();
                InitializeComponent();
                loadListaClientes();
                MessageBox.Show("El cliente se agregó correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            ltsClientes.SelectedIndex = ltsClientes.Items.Count - 1;
        }

        private void btnModificar_Copy_Click(object sender, RoutedEventArgs e)
        {

            try
            {

           
            int modificado;
            modificado = ltsClientes.SelectedIndex;
            idcliente = (int)ltsClientes.SelectedValue;
            String consulta = "SELECT * FROM clientesmi where idclientemi=@valor";
            DataTable cliente = conexion.ConsultaParametrizada(consulta, ltsClientes.SelectedValue);
            string nombre = cliente.Rows[0].ItemArray[6].ToString();
            string cuit = cliente.Rows[0].ItemArray[1].ToString();
            string transporte = cliente.Rows[0].ItemArray[2].ToString();
            string teltransporte = cliente.Rows[0].ItemArray[3].ToString();
            string direccionentrega = cliente.Rows[0].ItemArray[4].ToString();
            string razonsocial = cliente.Rows[0].ItemArray[5].ToString();
            string provincia = cliente.Rows[0].ItemArray[8].ToString();
            int listadeprecios;
            if (cliente.Rows[0].ItemArray[7].ToString() == "")
            {
                listadeprecios = 0;
            }
            else
            {
                listadeprecios = (int)cliente.Rows[0].ItemArray[7];
               
            }



            var newW = new windowAgregarClientemi(nombre, cuit, transporte, teltransporte, direccionentrega, razonsocial, listaContacto, idcliente, listadeprecios,provincia);
            
            newW.ShowDialog();
            

            if (newW.DialogResult == true)
            {
                this.txtCuit.Text = newW.txtCuit.Text;
                this.txtTransporte.Text = newW.txtTransporte.Text;
                String nombreActu = newW.txtNombre.Text;
                this.txtDireccion.Text = newW.txtDireccion.Text;
                this.txtrs.Text = newW.cmbRs.Text;
                this.txtTelt.Text = newW.txtTelt.Text;
                String province = newW.cmbP.Text;
                

                

              


                if (newW.cmbPrecios.Text == "")
                {
                    String update;
                    update = "update clientesmi set nombre = '" + nombreActu + "', razonsocial = '" + this.txtrs.Text + "', cuit = '" + this.txtCuit.Text + "', direccionentrega = '" + this.txtDireccion.Text + "', teltransporte = '" + this.txtTelt.Text + "', transporte = '" + this.txtTransporte.Text + "', provincia='"+province+"' where idClientemi ='" + idcliente + "';";
                    conexion.operaciones(update);
                }
                else
                {
                    String update;
                    update = "update clientesmi set nombre = '" + nombreActu + "', razonsocial = '" + this.txtrs.Text + "', cuit = '" + this.txtCuit.Text + "', direccionentrega = '" + this.txtDireccion.Text + "', teltransporte = '" + this.txtTelt.Text + "', transporte = '" + this.txtTransporte.Text + "', FK_idLista = '"+newW.idlp+ "', provincia='" + province + "' where idClientemi ='" + idcliente + "';";
                    conexion.operaciones(update);
                }



                String contact = "delete from contactocliente where FK_idClientemi= '" + idcliente + "'";
                conexion.operaciones(contact);


                

                foreach (var contacto in newW.lista)
                {
                    string sql;
                    sql = "INSERT INTO contactocliente (telefono,email,nombrecontacto,FK_idClientemi) values('" + contacto.NumeroTelefono + "', '" + contacto.Email + "', '" + contacto.NombreContacto + "', '" + idcliente + "')";
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

       
    }
    }

