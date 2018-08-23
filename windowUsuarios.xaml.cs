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
        bool acceso = false;
        public windowUsuarios()
        {
            InitializeComponent();
            txtUsername.Focus();
        
        }


        //INICIAR SESION
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            
            String usuarioTxt;
            String passTxt;
            String usuarioDB ;
            String passDB ;

            usuarioTxt = txtUsername.Text;          
            passTxt = txtPassword.Password.ToString();

 
            String sql3 = " SELECT COUNT(*) FROM usuarios WHERE usuario = '" + usuarioTxt + "'";
            String us = conexion.ValorEnVariable(sql3).ToString();

         
            if (us == "0")
            {
                MessageBox.Show("No existe usuario con ese nombre", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
             
            }
             else
             {
                String sql = "SELECT usuario FROM usuarios WHERE usuario = '" + usuarioTxt + "'";
                usuarioDB = conexion.ValorEnVariable(sql).ToString();
                String sql2 = "SELECT contrasenia  FROM usuarios WHERE usuario = '" + usuarioTxt + "'";
                passDB = conexion.ValorEnVariable(sql2).ToString();
                String sql4 = "SELECT tipoUsuario  FROM usuarios WHERE usuario = '" + usuarioTxt + "'";
                tipoUsuarioDB = conexion.ValorEnVariable(sql4).ToString();


                if (usuarioTxt.Equals(usuarioDB) && passTxt.Equals(Seguridad.DesEncriptar(passDB))){

                    acceso = true;
                    var newW = new MainWindow();
                    newW.Show();
                    this.Close(); 

                    }else if (!usuarioTxt.Equals(usuarioDB)) {

                    MessageBox.Show("usuario incorrecto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                 

                    }else
                {
                    MessageBox.Show("contraseña incorrecta", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                 
                    }
             }
     
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
          
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!acceso )
            {
                Environment.Exit(0);
            }
            
         }
    }
}
