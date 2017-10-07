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
        public static String idProv;
        CRUD conexion = new CRUD();
        private DataTable dt;
        public pageProveedores()
        {
            InitializeComponent();
            loadListaProveedores();
            LlenarComboFiltro();

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e) //btnModificarProveedor_Click
        {
            var newW = new windowModificarProveedor();
            
            String selectedValue = Convert.ToString(ltsProveedores.SelectedValue);
            String sql = "SELECT nombre from proveedor WHERE idProveedor = '" + selectedValue + "'";
            String nombre = conexion.ValorEnVariable(sql);
            newW.txtCuit.Text = this.txtCuit.Text;
            newW.txtCP.Text = this.txtCP.Text;
            newW.txtCategoria.Text = nombre.ToString();
            newW.txtDireccion.Text = this.txtDireccion.Text;
            newW.cmbRazonSocial.Text = this.txtRazonSocial.Text;
            newW.txtLocalidad.Text = this.txtLocalidad.Text;
            newW.dgvContactom.ItemsSource = this.dgvContacto.ItemsSource;
            
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
                
                String sqlContacto2;
                String sqlContacto3;

                sqlContacto2 = "DELETE  from contactoproveedor WHERE FK_idProveedor = '" + selectedValue + "'";
                conexion.operaciones(sqlContacto2);

                    int j = 0;
                    for (int i = 0; i < dgvContacto.Items.Count - 1; i++)
                    {
                        var telefono = (dgvContacto.Items[i] as System.Data.DataRowView).Row.ItemArray[j];
                        j++;
                        var email = (dgvContacto.Items[i] as System.Data.DataRowView).Row.ItemArray[j];
                        j++;
                        var nombre2 = (dgvContacto.Items[i] as System.Data.DataRowView).Row.ItemArray[j];
                        j++;
                        
                        j = 0;
                       sqlContacto3 = "insert into contactoproveedor(telefono, email, nombreContacto, FK_idProveedor) values('" + telefono + "', '" + email + "', '" + nombre2 + "', '" + selectedValue + "');";
                       conexion.operaciones(sqlContacto3);
                         loadListaProveedores();
                }

            }
         }
        
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var newW2 = new windowAgregarProveedor();
            newW2.ShowDialog();

            if (newW2.DialogResult == true)
            {

                String nombre = newW2.txtNombre.Text;
                String cuit = newW2.txtCuit.Text;
                String razonSocial = newW2.cmbRazonSocial.Text;
                String direccion = newW2.txtDireccion.Text;
                String categoria = newW2.cmbCategoria.Text;
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

            }
        }

        
        private void loadListaProveedores()
        {
            String consulta = "SELECT * FROM Proveedor";
            conexion.Consulta(consulta, ltsProveedores);
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

            }
        }

        public void ActualizaDGVContacto()
        {
            String consultaContacto = "SELECT contactoproveedor.telefono, contactoproveedor.email, contactoproveedor.nombreContacto from contactoproveedor WHERE FK_idProveedor=@valor";
            DataTable contacto = conexion.ConsultaParametrizada(consultaContacto, ltsProveedores.SelectedValue);
            dgvContacto.ItemsSource = contacto.AsDataView();
        }

      
    }

   
}
