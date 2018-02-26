﻿using System;
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

        public ME()
        {
            InitializeComponent();
            
            loadListaClientes();
            ActualizaDGVContacto();
            ltsClientes.SelectedIndex = 0;
            LlenarComboFiltro();
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
            }
            catch (NullReferenceException)
            {

                
            }
    

        }


        private void ltsClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizaDGVContacto();


            try
            {
                //consulta datosclientes
                String consulta = "SELECT * from clientesme WHERE idClienteme = @valor";
                DataTable clienteme = conexion.ConsultaParametrizada(consulta, ltsClientes.SelectedValue);
                txtPais.Text = clienteme.Rows[0].ItemArray[2].ToString();
                txtt.Text = clienteme.Rows[0].ItemArray[5].ToString();
                txtDireccion.Text = clienteme.Rows[0].ItemArray[1].ToString();
                txtweb.Text = clienteme.Rows[0].ItemArray[3].ToString();

            }
            catch (Exception)
            {

            }
                

            







        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            DataRow selectedDataRow = ((DataRowView)ltsClientes.SelectedItem).Row;
            string nombre = selectedDataRow["nombre"].ToString();
            MessageBoxResult dialog = MessageBox.Show("Esta seguro que desea eliminar :" + nombre, "Advertencia", MessageBoxButton.YesNo);
            
            if (dialog == MessageBoxResult.Yes)
            {
                int idSeleccionado = (int)ltsClientes.SelectedValue;
                string sql = "delete from clientesme where idClienteme = '" + idSeleccionado + "'";
                conexion.operaciones(sql);
                loadListaClientes();
                ActualizaDGVContacto();

                if (dgvContacto.Items == null)
                {

                    this.txtt.Text = "";
                    this.txtDireccion.Text = "";
                    this.txtPais.Text = "";
                    this.txtweb.Text = "";


                }
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
                int tc = newW.cmbtc.SelectedIndex;




                //INSERTAR DATOS PRINCIPALES
                String sql;
                sql = "insert into clientesme(nombre, web, pais, direccion, terminocomercial) values('" + nombre + "', '" + direccionweb + "', '" + pais + "', '" + direccion + "', '" + tc + "')";
                conexion.operaciones(sql);




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

            }
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            idcliente = (int)ltsClientes.SelectedValue;
            String consulta = "SELECT * FROM clientesme where idclienteme=@valor";
            DataTable cliente = conexion.ConsultaParametrizada(consulta, ltsClientes.SelectedValue);
            string direccion = cliente.Rows[0].ItemArray[1].ToString();
            string  pais = cliente.Rows[0].ItemArray[2].ToString();
            string web = cliente.Rows[0].ItemArray[3].ToString();
            string nombre = cliente.Rows[0].ItemArray[4].ToString();
            int terminocomercial = (int)cliente.Rows[0].ItemArray[5];
            int idCliente = (int)ltsClientes.SelectedValue;

            int j = 0;

            listaContacto.Clear();
            for (int i = 0; i < dgvContacto.Items.Count - 1; i++)
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

            var newW = new windowAgregarClienteme(nombre, direccion, pais, terminocomercial, web, listaContacto,idcliente);

            newW.ShowDialog();
            


            if (newW.DialogResult == true)
            {
                int id = (int)ltsClientes.SelectedValue;
                int termino = newW.cmbtc.SelectedIndex;
                String country = newW.cmbpais.Text;
                String nombreActu = newW.txtNombre.Text;
                String address = newW.txtDireccion.Text;
                String webpage = newW.txtdireccionweb.Text;
                
                this.dgvContacto.ItemsSource = newW.dgvContacto.ItemsSource;
                

                String update;
                update = "update clientesme set nombre = '" + nombreActu + "', direccion = '" + address + "', pais = '" + country + "', web = '" + webpage + "', terminocomercial = '"  + termino + "' where idClienteme ='" + ltsClientes.SelectedValue + "';";
                conexion.operaciones(update);



                loadListaClientes();








                String contact = "delete  from contactocliente where FK_idClienteme= '" + id  + "'";
                conexion.operaciones(contact);

               

               
                    foreach (var contacto in newW.lista)
                    {

             
                    string sql;
                        sql= "INSERT INTO contactocliente (telefono,email,nombrecontacto,FK_idClienteme) values('"+contacto.NumeroTelefono+"', '"+contacto.Email+"', '"+contacto.NombreContacto+"', '"+id+"')";
                        conexion.operaciones(sql);
                    }

                    

    

            }
        }

        private void LlenarComboFiltro()
        {

            cmbFiltro.Items.Add("Nombre");
            cmbFiltro.Items.Add("Lista de Precios");


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
            else if (txtFiltro.Text == "Lista de Precios")
            {
                consulta = "SELECT* FROM clientesme WHERE nombre LIKE '%' @valor '%'";
                clientes = conexion.ConsultaParametrizada(consulta, txtFiltro.Text);
            }

            ltsClientes.ItemsSource = clientes.AsDataView();
        }
    }
}