using System;
using System.Collections.Generic;
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
    public partial class windowAgregarClienteme : Window
    {

        
        CRUD conexion = new CRUD();
        public List<Contacto> lista = new List<Contacto>();
        int idcliente;
        Contacto info;

        public windowAgregarClienteme()
        {
            InitializeComponent();
            lista.Clear();
            llenarcmbpais();
            llenarcmbtc();
            LoadDGVContacto();
            CampLimit();
        }

        public windowAgregarClienteme(string nombre, string direccion, string pais, int terminocomercial, string web, List<Contacto> lista, int id)
        {
            InitializeComponent();
            txtNombre.Text = nombre;
            txtDireccion.Text = direccion;
            txtdireccionweb.Text = web;
            cmbtc.SelectedIndex = terminocomercial;
            cmbpais.SelectedValue = pais;
            llenarcmbtc();
            llenarcmbpais();
            this.lista = lista;
            loaddgvcontacto(this.lista);
            LoadDGVContacto();
            idcliente = id;
            CampLimit();


        }

        private void CampLimit()
        {
            txtDireccion.MaxLength = 50;
            txtdireccionweb.MaxLength = 40;
            txtNombre.MaxLength = 30;
           

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

       

        private void llenarcmbpais()
        {

            cmbpais.Items.Add("USA");
            
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            
                if (Validacion())
                {
                    DialogResult = true;
                }
            
        }

        public Boolean Validacion()
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Complete el nombre");
                return false;
            }
            else if (string.IsNullOrEmpty(txtdireccionweb.Text))
            {
                MessageBox.Show("Complete la web");
                return false;

            }
            else if (string.IsNullOrEmpty(txtDireccion.Text))
            {
                MessageBox.Show("Complete la direccion");
                return false;
            }
            else if (lista.Count<=0)
            {
                MessageBox.Show("Agregue un contacto");
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

                MessageBox.Show("Seleccione un contacto");
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
                        MessageBox.Show("Se ha eliminado el contacto");
                        break;
                    }
                    else
                    {


                    }


                }
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleecione un contacto");
            }

        }

        private void txtNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetter(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}
