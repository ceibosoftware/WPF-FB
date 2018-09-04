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

namespace wpfFamiliaBlanco.Clientes  // var LP = new LinkinPark();
{
    /// <summary>
    /// Lógica de interacción para windowAgregarCliente.xaml
    /// </summary>
    public partial class windowAgregarClienteme : Window
    {

        
        CRUD conexion = new CRUD();
        public int idlp;
        public List<Contacto> lista = new List<Contacto>();
        int idcliente;
        DataTable listadeprecios;
        Contacto info;

        public windowAgregarClienteme()
        {
            InitializeComponent();
            lista.Clear();
 
            loadcmblp();
            llenarcmbtc();
            camplimit();
            ActualizarDGVPrecios();
            LoadDGVContacto();
            CampLimit();
            loadcombopaises();
        }

        public windowAgregarClienteme(string nombre, string direccion, string pais, int terminocomercial,string cuitpais, string web, List<Contacto> lista, int id, int idlista)
        {
            InitializeComponent();
            txtNombre.Text = nombre;
            txtDireccion.Text = direccion;
            txtdireccionweb.Text = web;
            txtcp.Text = cuitpais;
            cmbtc.SelectedIndex = terminocomercial;
            
            llenarcmbtc();
            
            camplimit();
            this.lista = lista;
            
            loaddgvcontacto(this.lista);
            loadcmblp();
            loadcombopaises();
            cmbPrecios.SelectedValue = idlista;
            ActualizarDGVPrecios();
            LoadDGVContacto();
            idcliente = id;
            CampLimit();
            cmbpais.Text = pais;
            lblWindowTitle.Content = "Modificar Cliente de Mercado Externo";

        }

        private void loadcombopaises()
        {
            String consulta4 = "SELECT * FROM tablapaises";
            conexion.Consulta(consulta4, combo: cmbpais);
            cmbpais.DisplayMemberPath = "nombre";
            cmbpais.SelectedValuePath = "id";
            cmbpais.SelectedIndex = -1;
        }
      
        private void loadcmblp()
        {
            String consulta = "SELECT * from listadeprecios WHERE tipo=1";
            conexion.Consulta(consulta, combo: cmbPrecios);
            cmbPrecios.DisplayMemberPath = "nombre";
            cmbPrecios.SelectedValuePath = "idLista";
            


        }

        private void camplimit()
        {
            dgvPrecios.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvPrecios.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Precio de Lista";
            textColumn2.Binding = new Binding("preciolista");
            dgvPrecios.Columns.Add(textColumn2);
            
            

           
            
             
        }

        private void ActualizarDGVPrecios()

        {
            dgvPrecios.Items.Refresh();
            
            String consultalp = "SELECT p.nombre, plp.preciolista, plp.FK_idProductos, lp.anexo, lp.moneda from productos p,productos_has_listadeprecios plp, listadeprecios lp where FK_idLista = @valor and FK_idProductos=p.idProductos and idLista = @valor";
            listadeprecios = conexion.ConsultaParametrizada(consultalp, cmbPrecios.SelectedValue);
            

            dgvPrecios.ItemsSource = listadeprecios.AsDataView();



            if (listadeprecios.Rows.Count!=0)
            {
                
                if (listadeprecios.Rows[0].ItemArray[3].ToString()=="")
                {
                    lbltienea.Content = "No tiene anexo";
                }
                else
                {
                    lbltienea.Content = listadeprecios.Rows[0].ItemArray[3].ToString();
                }
                if (listadeprecios.Rows[0].ItemArray[4].ToString()=="1")
                {
                    lbllmoneda.Content = "USD";
                }
                else if (listadeprecios.Rows[0].ItemArray[4].ToString()=="2")
                {
                    lbllmoneda.Content = "EUROS";
                }
            }

            dgvPrecios.Items.Refresh();
            

        }
        

        private void CampLimit()
        {
            txtDireccion.MaxLength = 50;
            txtdireccionweb.MaxLength = 40;
            txtNombre.MaxLength = 30;
            dgvPrecios.IsReadOnly = true;
            txtcp.MaxLength = 11;

        }

       

        private void llenarcmbtc()
        {

            cmbtc.Items.Add("CIF");
            cmbtc.Items.Add("Ex-Works");
            cmbtc.Items.Add("FAS");
            cmbtc.Items.Add("FOB");
            cmbtc.Items.Add("FCA");
            cmbtc.Items.Add("C&F ");
            cmbtc.Items.Add("CIP");



        }

       


        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {

            if (Validacion())
            {
                if (cmbPrecios.SelectedIndex == -1)
                {
                    MessageBoxResult dialog = MessageBox.Show("¿Está seguro que desea agregar el cliente " + txtNombre.Text + " sin lista de precios?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (dialog == MessageBoxResult.Yes)
                    {

                        DialogResult = true;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    idlp = (int)cmbPrecios.SelectedValue; ;
                    DialogResult = true;
                }
            }
            }

        public Boolean Validacion()
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Complete el campo nombre.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(txtdireccionweb.Text))
            {
                MessageBox.Show("Complete el campo de la web.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            else if (string.IsNullOrEmpty(txtDireccion.Text))
            {
                MessageBox.Show("Complete el campo de la dirección.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (lista.Count<=0)
            {
                MessageBox.Show("Agregue un contacto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
         
            return true;
        }

        

        private void btnAgregarc_Click(object sender, RoutedEventArgs e)
        {
            var newW = new WindowAgregarContactoCliente();
            newW.ShowDialog();
            

            if (newW.DialogResult == true)
            {


                String telefono1 = newW.txtTelefonoContacto.Text;
                String nombreContacto1 = newW.txtNombreContacto.Text;
                String mail1 = newW.txtMailContacto.Text;
                info = new Contacto(nombreContacto1, mail1, telefono1);


                lista.Add(info);
                




                dgvContacto.Items.Refresh();


                


            }

        }
        private void loaddgvcontacto(List<Contacto> l)
        {
            dgvContacto.ItemsSource = l;
        }

        public void LoadDGVContacto()
        {
            
            this.dgvContacto.ItemsSource = lista;


        }
      
        public void EliminarDGVContacto()
        {
            this.dgvContacto.Items.Remove(lista);
            dgvContacto.Items.Refresh();

        }

        private void btnModificarc_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                Contacto contacto2 = dgvContacto.SelectedItem as Contacto;
                var newW = new WindowAgregarContactoCliente();
                String telActual = contacto2.NumeroTelefono.ToString();
                newW.txtMailContacto.Text = contacto2.Email.ToString();
                newW.txtNombreContacto.Text = contacto2.NombreContacto.ToString();
                newW.txtTelefonoContacto.Text = contacto2.NumeroTelefono.ToString();
                newW.ShowDialog();

                if (newW.DialogResult == true)
                {
                    String nombre = newW.txtNombreContacto.Text.ToString();
                    String tel = newW.txtTelefonoContacto.Text.ToString();
                    String mail = newW.txtMailContacto.Text.ToString();

                    contacto2.NombreContacto = nombre;
                    contacto2.NumeroTelefono = tel;
                    contacto2.Email = mail;

                    String update;
                    update = "update contactocliente set telefono  = '" + tel + "', email = '" + mail + "', nombreContacto = '" + nombre + "' where telefono ='" + telActual + "';";
                    conexion.operaciones(update);
                    dgvContacto.Items.Refresh();
                }

            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un contacto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnEliminarc_Click(object sender, RoutedEventArgs e)
        {
            try
            {



                Contacto contacto = dgvContacto.SelectedItem as Contacto;





                for (int i = 0; i <= lista.Count - 1; i++)
                {


                    if (contacto.NumeroTelefono.ToString().CompareTo(lista[i].NumeroTelefono.ToString()) == 0)
                    {

                        lista.Remove(lista[i]);
                        dgvContacto.Items.Refresh();
                        String update;
                        update = "DELETE FROM contactocliente WHERE telefono = '" + contacto.NumeroTelefono + "'";
                        conexion.operaciones(update);
                        MessageBox.Show("El contacto se ha eliminado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                    else
                    {


                    }


                }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un contacto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
     
        

        private void txtNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetter(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void cmbPrecios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarDGVPrecios();



           
            
           
            
        }

        private void txtcp_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void btnmlp_Click(object sender, RoutedEventArgs e)
        {
            var newW = new WindowAgregarLpme();
            string nombre;
            string anexo;
            newW.ShowDialog();


            if (newW.DialogResult == true)
            {
                nombre = newW.txtNombre.Text;
                DateTime hoy;
                hoy = DateTime.Today;


                if (newW.txtAnexo.Text == "")
                {
                    anexo = "";
                }
                else
                {
                    anexo = newW.txtAnexo.Text;
                }
                






                String sql;
                sql = "INSERT into listadeprecios(nombre, fecha,tipo,anexo) values('" + nombre + "', '" + hoy.ToString("yyyy/MM/dd") + "','"+1+"','"+anexo+"')";
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


                
                
               
                
                MessageBox.Show("Se agrego la lista de precio correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                loadcmblp();
                cmbPrecios.SelectedIndex = cmbPrecios.Items.Count - 1;
            }
            

        }
    }
}
