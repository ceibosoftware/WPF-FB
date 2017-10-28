using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using wpfFamiliaBlanco.Proveedores;

namespace wpfFamiliaBlanco
{

    /// <summary>
    /// Interaction logic for pageProveedores.xaml
    /// </summary>
    public partial class pageProveedores : Page
    {
        public static String idProv2;
        public static String idProv;
        CRUD conexion = new CRUD();
        List<Contacto> listaContacto = new List<Contacto>();

        public pageProveedores()
        {
            InitializeComponent();
            loadListaProveedores();
            LlenarComboFiltro();

            if (windowUsuarios.tipoUsuarioDB == "basico")
            {
                this.btnModificar.Visibility = Visibility.Collapsed;
                this.btnEliminar.Visibility = Visibility.Collapsed;
                this.dgvContacto.IsEnabled = false;
            }

        }
   
        private void btnModificar_Click(object sender, RoutedEventArgs e) //btnModificarProveedor_Click
        {
            int j=0;

            listaContacto.Clear();
          for (int i = 0; i < dgvContacto.Items.Count -1 ; i++)
            {

                var telefono = (dgvContacto.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
                j++;

                var email = (dgvContacto.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
                j++;
                var nombre2 = (dgvContacto.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
                j++;

                j = 0;

                Contacto conA = new Contacto(nombre2, email, telefono);
                listaContacto.Add(conA);

     
            }



            idProv2 = ltsProveedores.SelectedValue.ToString();
            List<Categorias> items = new List<Categorias>();
            String selectedValue = Convert.ToString(ltsProveedores.SelectedValue);
            String sql = "SELECT nombre from proveedor WHERE idProveedor = '" + selectedValue + "'";
            String nombre = conexion.ValorEnVariable(sql);
            
            // LLENAR DATOS PROVEEDORES.
            String consultaProveedores = "SELECT categorias.nombre, categorias.idCategorias from categorias , categorias_has_proveedor WHERE categorias_has_proveedor.FK_idProveedor = @valor  AND categorias_has_proveedor.FK_idCategorias = categorias.idCategorias";
            DataTable proveedores = conexion.ConsultaParametrizada(consultaProveedores, ltsProveedores.SelectedValue);
            for (int i = 0; i < proveedores.Rows.Count; i++)
            {
                Categorias categoria = new Categorias(proveedores.Rows[i].ItemArray[0].ToString(), (int)proveedores.Rows[i].ItemArray[1]);
                items.Add(categoria);
            }
           
        


            var newW = new windowModificarProveedor(items, listaContacto);
            newW.txtCuit.Text = this.txtCuit.Text;
            newW.txtCP.Text = this.txtCP.Text;
            newW.txtCategoria.Text = nombre.ToString();
            newW.txtDireccion.Text = this.txtDireccion.Text;
            newW.cmbRazonSocial.Text = this.txtRazonSocial.Text;
            newW.txtLocalidad.Text = this.txtLocalidad.Text;
     
            

            newW.ShowDialog();

            if (newW.DialogResult == true)
            {
                this.txtCuit.Text = newW.txtCuit.Text;
                this.txtCP.Text = newW.txtCP.Text;
                String nombreActu = newW.txtCategoria.Text;
                this.txtDireccion.Text = newW.txtDireccion.Text;
                this.txtRazonSocial.Text = newW.cmbRazonSocial.Text;
                this.txtLocalidad.Text = newW.txtLocalidad.Text;
                this.dgvContacto.ItemsSource = newW.dgvContactom.ItemsSource;
                

                String update;
                update = "update proveedor set nombre = '" + nombreActu + "', razonSocial = '" + this.txtRazonSocial.Text + "', cuit = '" + this.txtCuit.Text + "', codigoPostal = '" + this.txtCP.Text + "', direccion = '" + this.txtDireccion.Text + "', localidad = '" + this.txtLocalidad.Text + "' where idProveedor ='" + selectedValue + "';";
                conexion.operaciones(update);
                
     
                String sqlContacto3;

     
                String telefonoDB = "SELECT telefono from contactoproveedor WHERE FK_idProveedor = '" + selectedValue + "'";
                String TelDb = conexion.ValorEnVariable(telefonoDB).ToString();

                int kj = 0;

                try
                {

               
                for (int i = 0; i < dgvContacto.Items.Count - 1; i++)
                    {
                 
                        var telefono = (dgvContacto.Items[i] as System.Data.DataRowView).Row.ItemArray[kj];
                        kj++;
                    
                        var email = (dgvContacto.Items[i] as System.Data.DataRowView).Row.ItemArray[kj];
                        kj++;
                        var nombre2 = (dgvContacto.Items[i] as System.Data.DataRowView).Row.ItemArray[kj];
                        kj++;

                        kj = 0;

                    if (TelDb.Equals(telefono))
                    {
                        sqlContacto3 = "insert into contactoproveedor(telefono, email, nombreContacto, FK_idProveedor) values('" + telefono + "', '" + email + "', '" + nombre2 + "', '" + selectedValue + "');";
                        conexion.operaciones(sqlContacto3);
                        loadListaProveedores();
                    }
                    loadListaProveedores();
                }

                }
                catch (Exception)
                {

              
                }

                //ELIMINA REGISTRO DE TABLA INTERMEDIA
                string sql2 = "delete  from categorias_has_proveedor where FK_idProveedor =  '" + selectedValue + "'";
                    conexion.operaciones(sql2);
                    //INSERTO LOS NUEVOS DATOS DE LA TABLA INTERMEDIA                             
                    for (int i = 0; i < newW.Items.Count; i++)
                    {
                        int idCategoria = newW.Items[i].id;
                        string sql3 = "INSERT INTO categorias_has_proveedor(FK_idCategorias, FK_idProveedor) VALUES('" + idCategoria + "','" + selectedValue + "' )";
                        conexion.operaciones(sql3);
                    }

                
            }
         }
        
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            windowAgregarProveedor.lista.Clear();
            var newW2 = new windowAgregarProveedor();
            newW2.ShowDialog();

            if (newW2.DialogResult == true)
            {

                String nombre = newW2.txtNombre.Text;
                String cuit = newW2.txtCuit.Text;
                String razonSocial = newW2.cmbRazonSocial.Text;
                String direccion = newW2.txtDireccion.Text;
                String codigoPostal = newW2.txtCP.Text;
                String localidad = newW2.txtLocalidad.Text;

                
                //INSERTAR DATOS PRINCIPALES
                String sql;
                sql = "insert into proveedor(nombre, razonSocial, cuit, codigoPostal, direccion, localidad) values('" + nombre + "', '" + razonSocial + "', '" + cuit + "', '" + codigoPostal + "', '" + direccion + "', '" + localidad + "');";
                conexion.operaciones(sql);


                String sql2 = "Select idProveedor from proveedor order by idProveedor DESC LIMIT 1";
                 idProv = conexion.ValorEnVariable(sql2);

                Console.WriteLine("ULTIMO ID" + idProv);

                //INSERTAR CONTACTO PROVEEDOR
                String sqlContacto;

                string ultimoId = "Select last_insert_id()";
                  String id = conexion.ValorEnVariable(ultimoId);
                  for (int i = 0; i < windowAgregarProveedor.lista.Count; i++)
                  {
                      String nombreL = windowAgregarProveedor.lista[i].NombreContacto;
                      String telefonoL = windowAgregarProveedor.lista[i].NumeroTelefono;
                      String emailL = windowAgregarProveedor.lista[i].Email;
                      sqlContacto = "insert into contactoproveedor(telefono, email, nombreContacto, FK_idProveedor) values('" + telefonoL + "', '" + emailL + "', '" + nombreL + "', '" + idProv + "');";
                      conexion.operaciones(sqlContacto);
                  }
                // loadListaProducto();

                //INSERTAR CATEGORIAS PROVEEDOR
              
           
                for (int i = 0; i < newW2.Items.Count; i++)
                {
                    int idCategoria = newW2.Items[i].id;
                    string sql3 = "INSERT INTO categorias_has_proveedor(FK_idProveedor, FK_idCategorias) VALUES('" + id+ "','" + idCategoria + "' )";
                    conexion.operaciones(sql3);
                }
                loadListaProveedores();
                this.ltsCategorias.Items.Refresh();
            }
        }

        
        private void loadListaProveedores()
        {
            String consulta = "SELECT * FROM Proveedor";
            conexion.Consulta(consulta, ltsProveedores);
            ltsProveedores.DisplayMemberPath = "nombre";
            ltsProveedores.SelectedValuePath = "idProveedor";
            ltsProveedores.SelectedIndex = 1;

        }


        private void loadListaCategorias()
        {
            String consulta = "SELECT * FROM categorias";
            conexion.Consulta(consulta, ltsCategorias);
            ltsProveedores.DisplayMemberPath = "nombre";
            ltsProveedores.SelectedValuePath = "idProveedor";

        }

        private void ltsProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                //consulta datosproveedor
                String consulta = "SELECT * from proveedor WHERE idProveedor = @valor";
                DataTable proveedor = conexion.ConsultaParametrizada(consulta, ltsProveedores.SelectedValue);
                txtCuit.Text = proveedor.Rows[0].ItemArray[3].ToString();
                txtDireccion.Text = proveedor.Rows[0].ItemArray[5].ToString();
                txtRazonSocial.Text = proveedor.Rows[0].ItemArray[2].ToString();
                txtLocalidad.Text = proveedor.Rows[0].ItemArray[6].ToString();
                txtCP.Text = proveedor.Rows[0].ItemArray[4].ToString();

                //consulta contacto
                String consultaContacto = "SELECT contactoproveedor.telefono, contactoproveedor.email, contactoproveedor.nombreContacto from contactoproveedor WHERE FK_idProveedor=@valor";
                DataTable contacto = conexion.ConsultaParametrizada(consultaContacto, ltsProveedores.SelectedValue);
                dgvContacto.ItemsSource = contacto.AsDataView();

                //consulta categorias
                String consultaCategoria = "SELECT categorias.nombre from categorias , categorias_has_proveedor WHERE categorias_has_proveedor.FK_idProveedor = @valor AND categorias_has_proveedor.FK_idCategorias=categorias.idCategorias";
                DataTable categorias = conexion.ConsultaParametrizada(consultaCategoria, ltsProveedores.SelectedValue);
                ltsCategorias.ItemsSource = categorias.AsDataView();
                ltsCategorias.DisplayMemberPath = "nombre";


            }
            catch
            {

            }
        }



        private void LlenarComboFiltro()
        {

            cmbFiltro.Items.Add("Nombre");
            cmbFiltro.Items.Add("Categoria");
           
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            DataRow selectedDataRow = ((DataRowView)ltsProveedores.SelectedItem).Row;
            string nombre = selectedDataRow["nombre"].ToString();
            MessageBoxResult dialog = MessageBox.Show("Esta seguro que desea eliminar :" + nombre, "Advertencia", MessageBoxButton.YesNo);
            if (dialog == MessageBoxResult.Yes)
            {
                int idSeleccionado = (int)ltsProveedores.SelectedValue;
                string sql = "delete from proveedor where idProveedor = '" + idSeleccionado + "'";
                conexion.operaciones(sql);
                loadListaProveedores();
                ActualizaDGVContacto();

                if(dgvContacto.Items == null)
                {
                    this.txtCuit.Text = "";
                    this.txtCP.Text = "";   
                    this.txtDireccion.Text = "";
                    this.txtRazonSocial.Text = "";
                    this.txtLocalidad.Text = "";
        
                }

            }
        }

        public void ActualizaDGVContacto()
        {
            String consultaContacto = "SELECT contactoproveedor.telefono, contactoproveedor.email, contactoproveedor.nombreContacto from contactoproveedor WHERE FK_idProveedor=@valor";
            DataTable contacto = conexion.ConsultaParametrizada(consultaContacto, ltsProveedores.SelectedValue);
            dgvContacto.ItemsSource = contacto.AsDataView();
         
        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {

            
            // Busquedas de productos.
            DataTable proveedores = new DataTable();
            String consulta;

            if (cmbFiltro.Text == "Nombre")
            {   //Busca por nombre
                consulta = "SELECT* FROM proveedor WHERE nombre LIKE '%' @valor '%'";
                proveedores = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            }
            else if (cmbFiltro.Text == "Categoria")
            {
                //busca por nombre de categoria (posibilidad de agregar combobox)
                consulta = " select categorias.idCategorias,categorias_has_proveedor.FK_idProveedor,proveedor.nombre	from  categorias,categorias_has_proveedor,proveedor where categorias.nombre like @valor  and categorias.idCategorias = categorias_has_proveedor.FK_idCategorias and proveedor.idProveedor = categorias_has_proveedor.FK_idProveedor";
                proveedores = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
                if (txtFiltro.Text == "")
                {
                    consulta = "SELECT* FROM proveedor WHERE nombre LIKE '%' @valor '%'";
                    proveedores = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
                }
            }

            ltsProveedores.ItemsSource = proveedores.AsDataView();
        }
    }

   
}
