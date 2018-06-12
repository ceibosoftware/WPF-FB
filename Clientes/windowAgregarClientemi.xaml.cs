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
    /// Lógica de interacción para windowAgregarCliente.xaml
    /// </summary>
    public partial class windowAgregarClientemi : Window
    {
        CRUD conexion = new CRUD();
        public int idlp;
        //List<Contacto> listaContacto = new List<Contacto>();
        public List<Contacto> lista = new List<Contacto>();
        DataTable listadeprecios;
        int idcliente;


        public windowAgregarClientemi()
        {
            InitializeComponent();
            lista.Clear();
            llenarcmbrs();
            loadcmblp();
            ActualizarDGVPrecios();
            loadcomboprovincias();
            LoadDGVContacto();
            CampLimit();

        }

       public windowAgregarClientemi(string nombre,string cuit,string trasnporte,string teltransporte,string direccionentrega,string razonsocial,List<Contacto>lista, int id, int idlista,string provincia)
        {
            InitializeComponent();
            txtNombre.Text = nombre;
            txtCuit.Text = cuit;
            txtTransporte.Text = trasnporte;
            txtTelt.Text = teltransporte;
            txtDireccion.Text = direccionentrega;
            cmbRs.Text = razonsocial;
            loadcmblp();
            loadcomboprovincias();
            cmbPrecios.SelectedValue = idlista;
            ActualizarDGVPrecios();
            llenarcmbrs();
            this.lista = lista;
            loaddgvcontacto(this.lista);
            idcliente = id;
            CampLimit();
            cmbP.Text = provincia;
            lblWindowTitle.Content = "Modificar Cliente de Mercado Interno";

        }

        private void CampLimit()
        {
            txtCuit.MaxLength = 11;
            txtDireccion.MaxLength = 40;
            txtNombre.MaxLength = 30;
            txtTelt.MaxLength = 20;
            txtTransporte.MaxLength = 20;
            dgvPrecios.IsReadOnly = true;


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

        private void loaddgvcontacto(List<Contacto> l)
        {
            dgvContacto.ItemsSource = l;
        }

        private void llenarcmbrs()
        {

            cmbRs.Items.Add("Excento");
            cmbRs.Items.Add("Responsable Inscripto");
            cmbRs.Items.Add("Monotributista");
            

        }

        private void loadcmblp()
        {
            String consulta = "SELECT * from listadeprecios";
            conexion.Consulta(consulta, combo: cmbPrecios);
            cmbPrecios.DisplayMemberPath = "nombre";
            cmbPrecios.SelectedValuePath = "idLista";

        }

        private void ActualizarDGVPrecios()
        {
            dgvPrecios.Items.Refresh();

            String consultalp = "SELECT p.nombre, plp.preciolista, plp.FK_idProductos from productos p,productos_has_listadeprecios plp where FK_idLista = @valor and FK_idProductos=p.idProductos";
            listadeprecios = conexion.ConsultaParametrizada(consultalp, cmbPrecios.SelectedValue);


            dgvPrecios.ItemsSource = listadeprecios.AsDataView();

        



        dgvPrecios.Items.Refresh();

        }

        private void loadcomboprovincias()
        {
            String consulta4 = "SELECT * FROM provincia";
            conexion.Consulta(consulta4, combo: cmbP);
            cmbP.DisplayMemberPath = "provincia_nombre";
            cmbP.SelectedValuePath = "id";
            cmbP.SelectedIndex = -1;
        }



        public Boolean Validacion()
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Complete el campo Nombre", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(txtCuit.Text))
            {
                MessageBox.Show("Complete el campo CUIT", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            else if (string.IsNullOrEmpty(txtTelt.Text))
            {
                MessageBox.Show("Complete el campo Teléfono de transporte", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            else if (string.IsNullOrEmpty(txtTransporte.Text))
            {
                MessageBox.Show("Complete la Dirección del transporte", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            else if (string.IsNullOrEmpty(txtDireccion.Text))
            {
                MessageBox.Show("Complete el campo Dirección", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (lista.Count <= 0)
            {
                MessageBox.Show("Agregue un contacto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (cmbP.SelectedIndex==-1)
            {
                MessageBox.Show("Seleccione la provinca", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                Contacto info = new Contacto(nombreContacto1, mail1, telefono1);


                lista.Add(info);

                dgvContacto.Items.Refresh();

            }






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

        private void btnAceptar_Click_1(object sender, RoutedEventArgs e)
        {
            if (Validacion())
            {
                if(cmbPrecios.SelectedIndex == -1) { 
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
                else{
                    idlp = (int)cmbPrecios.SelectedValue; ;
                    DialogResult = true;
                }


            }
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
                        MessageBox.Show("Se eliminó el contacto correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void txtCuit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void txtTelt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
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

        private void btnnlp_Click(object sender, RoutedEventArgs e)
        {
            var neww = new windowAgregarLp();
            neww.ShowDialog();
        }
    }
}
