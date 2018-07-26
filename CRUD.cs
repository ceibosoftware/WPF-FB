using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace wpfFamiliaBlanco
{
    class CRUD
    {
        MessageBoxButton buttons = MessageBoxButton.OK;

        //MySqlConnection conexion = new MySqlConnection("Server=batta.ddns.net; Port=8889; Database=familiablanco; Uid=root; Pwd=''");
       // MySqlConnection conexion = new MySqlConnection("Server=192.168.0.3; Database=familiablanco; Uid=mariano; Pwd=1234");
       MySqlConnection conexion = new MySqlConnection("Server=localhost; Database=familiablanco; Uid=root; Pwd=''");

        public MySqlConnection Conexion1 { get => conexion; set => conexion = value; }

        public void Conexion()
        {
            try
            {
                conexion.Open();
                var result = MessageBox.Show("Conexion exitosa", "", buttons);
                conexion.Close();
            }
            catch
            {
                var result = MessageBox.Show("Conexion fallida", "", buttons);
            }
        }

        public MySqlTransaction iniciarTransaccion()
        {
            conexion.Open();
            MySqlTransaction tr = conexion.BeginTransaction();
            return tr;
        }
        // consulta para llenar con todos los datos una listBox , vamos a ir agregando elementos a llenar, (combo y tabla son parametros opcionales por eso estan como null) 
        public void Consulta(String sql, ListBox tabla = null, ComboBox combo = null)
        {
            try
            {
                //Objeto data adapter: realiza conexion y solicitud del usuario.
                MySqlDataAdapter DA = new MySqlDataAdapter(sql, conexion);
                // Data table recibe la informacion del dataAdapter (DA).
                DataTable DT = new DataTable();
                //Pasamos la informacion del dataAdapter (DA) al data (DT) 
                DA.Fill(DT);
                if (tabla != null)
                {
                    tabla.ItemsSource = DT.AsDataView();
                }
                else if (combo != null)
                {
                    combo.ItemsSource = DT.AsDataView();
                }


            }
            catch
            {

                var result = MessageBox.Show("No se pudo realizar la consulta", "Error", buttons);
            }
        }
        // consulta parametrizada depende de el valor de parametro .
        public DataTable ConsultaParametrizada(String sql, Object parametro = null, String parametro2 = null)
        {
            try
            {
                //Objeto data adapter: realiza conexion y solicitud del usuario.
                MySqlDataAdapter DA = new MySqlDataAdapter(sql, conexion);
                if (parametro != null)
                {
                    DA.SelectCommand.Parameters.AddWithValue("valor", parametro);
                }
                else
                {
                    DA.SelectCommand.Parameters.AddWithValue("valor", parametro2);
                }

                // Data table recibe la informacion del dataAdapter (DA).
                DataTable DT = new DataTable();
                //Pasamos la informacion del dataAdapter (DA) al data (DT) 
                DA.Fill(DT);
                return DT;
            }
            catch
            {
                return null;
                var result = MessageBox.Show("No se pudo realizar la consulta", "Error", buttons);
            }
        }

        public DataTable coleccion(string sql)
        {
            
                //Objeto data adapter: realiza conexion y solicitud del usuario.
                MySqlDataAdapter DA = new MySqlDataAdapter(sql, conexion);
                // Data table recibe la informacion del dataAdapter (DA).
                DataTable DT = new DataTable();
                //Pasamos la informacion del dataAdapter (DA) al data (DT) 
                DA.Fill(DT);

                return DT;
          
        }
        // funcion que se va usar para hacer abm .
        public void operaciones(String sql)
        {
            //abrimos conexion
            conexion.Open();
          
                //objeto command almacena las instrucciones  
                MySqlCommand comando = new MySqlCommand(sql, conexion);
                //ejecutamos instruccion
                comando.ExecuteNonQuery();
            
          
            //cerramos conexion
            conexion.Close();
        }
        public string ValorEnVariable(String sql)
        {
            string valor;
            //abrimos conexion
            conexion.Open();
           
                //objeto command almacena las instrucciones  
                MySqlCommand comando = new MySqlCommand(sql, conexion);
                //ejecutamos instruccion
                valor = comando.ExecuteScalar().ToString();
         

            //cerramos conexion
            conexion.Close();
            return valor;
            

        }

        
    }
}


