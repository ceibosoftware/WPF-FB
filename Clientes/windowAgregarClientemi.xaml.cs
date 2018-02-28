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
        //List<Contacto> listaContacto = new List<Contacto>();
        public List<Contacto> lista = new List<Contacto>();
        int idcliente;


        public windowAgregarClientemi()
        {
            InitializeComponent();
            lista.Clear();
            llenarcmbrs();
            LoadDGVContacto();
            

        }

       public windowAgregarClientemi(string nombre,string cuit,string trasnporte,string teltransporte,string direccionentrega,string razonsocial,List<Contacto>lista, int id)
        {
            InitializeComponent();
            txtNombre.Text = nombre;
            txtCuit.Text = cuit;
            txtTransporte.Text = trasnporte;
            txtTelt.Text = teltransporte;
            txtDireccion.Text = direccionentrega;
            cmbRs.Text = razonsocial;
            llenarcmbrs();
            this.lista = lista;
            loaddgvcontacto(this.lista);
            idcliente = id;

        }

        private void loaddgvcontacto(List<Contacto> l)
        {
            dgvContacto.ItemsSource = l;
        }

        private void llenarcmbrs()
        {

            cmbRs.Items.Add("Nombre");
            cmbRs.Items.Add("Categoria");
        }





        public Boolean Validacion()
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Complete el nombre");
                return false;
            }
            else if (string.IsNullOrEmpty(txtCuit.Text))
            {
                MessageBox.Show("Complete el CUIT");
                return false;

            }
            else if (string.IsNullOrEmpty(txtTelt.Text))
            {
                MessageBox.Show("Complete el Telefono del Transporte");
                return false;

            }
            else if (string.IsNullOrEmpty(txtTransporte.Text))
            {
                MessageBox.Show("Complete la direccion del Transporte");
                return false;

            }
            else if (string.IsNullOrEmpty(txtDireccion.Text))
            {
                MessageBox.Show("Complete la direccion");
                return false;
            }
            else if (lista.Count <= 0)
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
                DialogResult = true;
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

                MessageBox.Show("Seleccione un contacto");
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

                MessageBox.Show("Seleccione un contacto");
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
    }
}
