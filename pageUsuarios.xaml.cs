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
        public pageUsuarios()
        {
            InitializeComponent();
        }

        public object Owner { get; private set; }

        public void btnPrueba_Click(object sender, RoutedEventArgs e)
        {

            var newW = new windowTEST();

            newW.Owner = MainWindow.GetWindow(this);

            BlurEffect myBlur = new BlurEffect();
            myBlur.Radius = 5;
            this.Effect = myBlur;

            newW.ShowDialog();







        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
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

