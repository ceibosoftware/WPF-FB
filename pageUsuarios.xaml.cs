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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using MySql.Data.MySqlClient;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for pageUsuarios.xaml
    /// </summary>
    public partial class pageUsuarios : Page
    {
       public  int var = 0;
        CRUD conexion = new CRUD();
       

        public pageUsuarios()
        {
            InitializeComponent();
            LlenarComboUsuario();
     

            
            if (windowUsuarios.tipoUsuarioDB == "super")
            {

                this.lblTexto.Visibility = Visibility.Collapsed;
                cmbTipoUsuario.SelectedIndex = 1;

            }
            if (windowUsuarios.tipoUsuarioDB == "basico")
            {

                this.btnSubmit.IsEnabled = false;
                this.txtPassword.IsEnabled = false;
                this.txtUsername.IsEnabled = false;
                this.cmbTipoUsuario.IsEnabled = false;
               
               
            }

        }



        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            String name = txtUsername.Text.ToString();
            String nombreDB = "SELECT COUNT(*) FROM usuarios WHERE usuario  = '" + name + "'";
            String nomCat = conexion.ValorEnVariable(nombreDB).ToString();


            if (txtUsername.Text != "" && txtPassword.Password.ToString() != "" && nomCat == "0")
            {

                //CREAR USUARIO
                        String NuevoUsuario = txtUsername.Text;
                        String NuevoTipo = cmbTipoUsuario.SelectedItem.ToString();


                        String sql5 = "insert into usuarios(usuario, contrasenia, tipoUsuario) values('" + NuevoUsuario + "', '" + Seguridad.Encriptar(txtPassword.Password.ToString()) + "', '" + NuevoTipo + "');";
                        conexion.operaciones(sql5);

                        MessageBox.Show("USUARIO CREADO CORRECTAMENTE");
                    
         
            }
            else if (txtUsername.Text == "")
            {
                MessageBox.Show("Debe ingresar el nombre de usuario");

            }
            else if (txtPassword.Password.ToString() == "")
            {
                MessageBox.Show("Debe ingresar una contraseña");
            }
            else if (!nomCat.Equals("0"))
            {
                MessageBox.Show("El usuario ingresado ya existe");
            }
            else if (cmbTipoUsuario.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Debe seleccionar un tipo de usuario");
            }

        }

        private void LlenarComboUsuario()
        {

            cmbTipoUsuario.Items.Add("basico");
            cmbTipoUsuario.Items.Add("super");

        }
    }
}

