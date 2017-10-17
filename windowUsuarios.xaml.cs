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
    /// Interaction logic for windowUsuarios.xaml
    /// </summary>
    public partial class windowUsuarios : Window
    {
        public static String tipoUsuarioDB;
        CRUD conexion = new CRUD();
        public windowUsuarios()
        {
            InitializeComponent();
        }


        //INICIAR SESION
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            String usuarioTxt;
            String passTxt;


         
            String usuarioDB ;
            String passDB ;

            usuarioTxt = txtUsername.Text;          
            passTxt = txtPassword.Text;

 
            String sql3 = " SELECT COUNT(*) FROM usuarios WHERE usuario = '" + usuarioTxt + "'";
            String us = conexion.ValorEnVariable(sql3).ToString();

         
            if (us == "0")
            {
                MessageBox.Show("No existe usuario con ese nombre");
            }
             else
             {
                String sql = "SELECT usuario FROM usuarios WHERE usuario = '" + usuarioTxt + "'";
                usuarioDB = conexion.ValorEnVariable(sql).ToString();
                String sql2 = "SELECT contrasenia  FROM usuarios WHERE usuario = '" + usuarioTxt + "'";
                passDB = conexion.ValorEnVariable(sql2).ToString();
                String sql4 = "SELECT tipoUsuario  FROM usuarios WHERE usuario = '" + usuarioTxt + "'";
                tipoUsuarioDB = conexion.ValorEnVariable(sql4).ToString();

                if (usuarioTxt.Equals(usuarioDB) && passTxt.Equals(passDB)){

                   
                    var newW = new MainWindow();
                    newW.Show();

                    }else if (!usuarioTxt.Equals(usuarioDB)) {

                                MessageBox.Show("usuario incorrecto");

                    }else{
                                MessageBox.Show("contraseña incorrecta");
                    }
             }
     
        }

        //CREAR USUARIO
        private void btnPrueba_Click(object sender, RoutedEventArgs e)
        {
            var newW2 = new windowCrearUsuario();
            newW2.ShowDialog();
            if (newW2.DialogResult == true)
            {

                String NuevoUsuario = newW2.txtUsername.Text;
                String NuevaContrasenia = newW2.txtPassword.Text;
                String NuevoTipo = newW2.cmbTipoUsuario.SelectedItem.ToString();

                String sql5 = "insert into usuarios(usuario, contrasenia, tipoUsuario) values('" + NuevoUsuario + "', '"+ NuevaContrasenia + "', '"+ NuevoTipo + "');";
                conexion.operaciones(sql5);

                MessageBox.Show("USUARIO CREADO CORRECTAMENTE");
            }

      


        }
    }
}
