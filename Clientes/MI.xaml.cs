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
        public int idcliente;

        public MI()
        {
            InitializeComponent();
            loadListaClientes();
            ActualizaDGVContacto();
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
            cmbFiltro.Items.Add("Lista de Precios");


        }

        private void ltsClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            ActualizaDGVContacto();

            try
            {

                //consulta datosclientes
                String consulta = "SELECT * from clientesmi WHERE idClientemi = @valor";
                DataTable clientemi = conexion.ConsultaParametrizada(consulta, ltsClientes.SelectedValue);
                txtCuit.Text = clientemi.Rows[0].ItemArray[1].ToString();
                txtrs.Text = clientemi.Rows[0].ItemArray[5].ToString();
                txtDireccion.Text = clientemi.Rows[0].ItemArray[4].ToString();
                txtTelt.Text = clientemi.Rows[0].ItemArray[3].ToString();
                txtTransporte.Text = clientemi.Rows[0].ItemArray[2].ToString();

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

            DataRow selectedDataRow = ((DataRowView)ltsClientes.SelectedItem).Row;
            string nombre = selectedDataRow["nombre"].ToString();
            MessageBoxResult dialog = MessageBox.Show("Esta seguro que desea eliminar :" + nombre, "Advertencia", MessageBoxButton.YesNo);
            int idSeleccionado = (int)ltsClientes.SelectedValue;
            if (dialog == MessageBoxResult.Yes)
            {
                
                string sql = "delete from clientesmi where idClientemi = '" + idSeleccionado + "'";
                conexion.operaciones(sql);
                loadListaClientes();
                ActualizaDGVContacto();

                if (dgvContacto.Items == null)
                {

                    this.txtCuit.Text = "";
                    this.txtDireccion.Text = "";
                    this.txtrs.Text = "";
                    this.txtTelt.Text = "";
                    this.txtTransporte.Text = "";

                }

            }
            ltsClientes.SelectedIndex = 0;
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
            }
            else if (txtFiltro.Text == "Lista de Precios")
            {
                consulta = "SELECT* FROM clientesmi WHERE nombre LIKE '%' @valor '%'";
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
                String direccion = newW.txtDireccion.Text;
                String transporte = newW.txtTransporte.Text;
                int razons = newW.cmbRs.SelectedIndex;



                //INSERTAR DATOS PRINCIPALES
                String sql;
                sql = "insert into clientesmi(nombre, razonsocial, cuit, teltransporte, direccionentrega, transporte) values('" + nombre + "', '" + razons + "', '" + cuit + "', '" + telt + "', '" + direccion + "', '" + transporte + "')";
                conexion.operaciones(sql);




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

            }
            ltsClientes.SelectedIndex = ltsClientes.Items.Count - 1;
        }

        private void btnModificar_Copy_Click(object sender, RoutedEventArgs e)
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
           
            
            var newW = new windowAgregarClientemi(nombre, cuit, transporte, teltransporte, direccionentrega, razonsocial, listaContacto, idcliente);
            
            newW.ShowDialog();
            

            if (newW.DialogResult == true)
            {
                this.txtCuit.Text = newW.txtCuit.Text;
                this.txtTransporte.Text = newW.txtTransporte.Text;
                String nombreActu = newW.txtNombre.Text;
                this.txtDireccion.Text = newW.txtDireccion.Text;
                this.txtrs.Text = newW.cmbRs.Text;
                this.txtTelt.Text = newW.txtTelt.Text;

                

                String update;
                update = "update clientesmi set nombre = '" + nombreActu + "', razonsocial = '" + this.txtrs.Text + "', cuit = '" + this.txtCuit.Text + "', direccionentrega = '" + this.txtDireccion.Text + "', teltransporte = '" + this.txtTelt.Text + "', transporte = '" + this.txtTransporte.Text + "' where idClientemi ='" + idcliente + "';";
                conexion.operaciones(update);
               




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
    }
    }

