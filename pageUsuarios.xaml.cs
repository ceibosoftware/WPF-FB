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
        
        CRUD conexion = new CRUD();
        MySqlConnection sqlCon = new MySqlConnection("Server = localhost; Port = 3306; Database = familiablanco; Uid = root; Pwd = ''");

        public pageUsuarios()
        {

            MySqlConnection sqlCon = new MySqlConnection("Server = localhost; Port = 3306; Database = familiablanco; Uid = root; Pwd = ''");

            InitializeComponent();
        }

        public object Owner { get; private set; }

        public void btnPrueba_Click(object sender, RoutedEventArgs e)
        {


            string usuario = txtUsername.Text;
            string pass = txtPassword.Text;

           if ( registro(usuario, pass))
            {
                MessageBox.Show($"Usuario {usuario} fue creado");
            }
            else
            {
                MessageBox.Show($"Usuario {usuario} no creado");
            }



           /* var newW = new windowTEST();

            newW.Owner = MainWindow.GetWindow(this);

            BlurEffect myBlur = new BlurEffect();
            myBlur.Radius = 5;
            this.Effect = myBlur;

            newW.ShowDialog();


            */




        }

        public bool login(string usuario, string contra)
        {
            string query = $"SELECT * FROM usuarios WHERE usuario = '{usuario}' AND pass='{contra}';";


            try
            {
                if (nuevaconexion())
                {
                    MySqlCommand cmd = new MySqlCommand(query, sqlCon);
                    MySqlDataReader lector = cmd.ExecuteReader();

                    if(lector.Read())
                    {
                        lector.Close();
                        sqlCon.Close();
                        return true;
                    }
                    else
                    {
                        lector.Close();
                        sqlCon.Close();
                        return false;
                    }
                }
                else
                {
                    sqlCon.Close();
                        return false;
                }
            }
            catch(Exception ex)
            {
                sqlCon.Close();
                return false;
            }

        }

        public bool registro (string usuario, string contra)
        {


            string query = $"INSERT INTO usuarios (idUsuarios, usuario,pass) VALUES ('','{usuario}','{contra}');";

            try
            {
                if (nuevaconexion())
                {
                    MySqlCommand cmd = new MySqlCommand(query, sqlCon);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch(Exception ex)
                    {
                        return false;
                    }
                }
                else
                {
                    sqlCon.Close();
                    return false;
                }
            }
            catch(Exception ex)
            {
                sqlCon.Close();
                return false;
            }


        }

        private bool nuevaconexion()
        {

            try
            {
                sqlCon.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                { 
                    case 0:
                        MessageBox.Show("Fallo la conexion");
                        break;
                    case 1045:
                        MessageBox.Show("Usuario o contraseña incorrecto");
                        break;
                    
                }
                return false;
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {


            string usuario = txtUsername.Text;
            string pass = txtUsername.Text;

            if (login(usuario, pass))
            {
                MessageBox.Show($"Ingreso el usuario {usuario}");
            }
            else
            {
                MessageBox.Show($"{usuario} no ingreso, usuario o contraseña incorrecto ");
            }




            /* MySqlConnection sqlCon = new MySqlConnection("Server = batta.ddns.net; Port = 8889; Database = familiablanco; Uid = root; Pwd = ''");


             try
             {
                 if (sqlCon.State == MySqlConnection.Closed)
                     sqlCon.Open();
                 String query = "SELECT COUNT(1) FROM tblUser WHERE Username=@Username AND Password=@Password";
                 MySqlCommand sqlCmd = new MySqlCommand(query, sqlCon);
                 sqlCmd.CommandType = CommandType.Text;
                 sqlCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                 sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Password);
                 int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                 if (count == 1)
                 {
                     MainWindow dashboard = new MainWindow();
                     dashboard.Show();
                     this.Close();
                 }
                 else
                 {
                     MessageBox.Show("Username or password is incorrect.");
                 }
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }
             finally
             {
                 sqlCon.Close();
             }
             */
        }
    }
}

