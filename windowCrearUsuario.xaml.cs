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

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for windowCrearUsuario.xaml
    /// </summary>
    public partial class windowCrearUsuario : Window
    {

        CRUD conexion = new CRUD();

        public windowCrearUsuario()
        {
            InitializeComponent();
            LlenarComboUsuario();
            cmbTipoUsuario.SelectedIndex = 1;
        
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            String name = txtUsername.Text.ToString();
            String nombreDB = "SELECT COUNT(*) FROM usuarios WHERE usuario  = '" + name + "'";
            String nomCat = conexion.ValorEnVariable(nombreDB).ToString();
            MessageBox.Show("usuario: " + nomCat);

            if (txtUsername.Text != "" && txtPassword.Text != "" && nomCat == "0" )
            {
                this.DialogResult = true;
            }
            else if (txtUsername.Text == "")
            {
                MessageBox.Show("Debe ingresar el nombre de usuario");

            }else if (txtPassword.Text == "")
            {
                MessageBox.Show("Debe ingresar una contraseña");
            }else if (!nomCat.Equals("0"))
            {
                MessageBox.Show("El usuario ingresado ya existe");
            }else if (cmbTipoUsuario.SelectedItem.ToString() == "")
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
