using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpfFamiliaBlanco.SalidasNuevo.Clases.INV
{
    
    public class Analisis
    {
        CRUD conexion = new CRUD();


        private string numeroanalisis;
        private int tipo;
        private DateTime fechaanalisis;
        private float alcohol;
        private float densidad;
        private float litros;
        private string nombrevino;
        private int id;

        public string Numeroanalisis { get => numeroanalisis; set => numeroanalisis = value; }
        public int Tipo { get => tipo; set => tipo = value; }
        public int Id { get => id; set => id = value; }
        public float Alcohol { get => alcohol; set => alcohol = value; }
        public float Densidad { get => densidad; set => densidad = value; }
        public string Nombrevino{ get => nombrevino; set => nombrevino = value; }
        public float Litros { get => litros; set => litros = value; }
        public DateTime Fechaanalisis { get => fechaanalisis; set => fechaanalisis = value; }

        public Analisis(){ }

        public Analisis(int id,float alcohol,float densidad, float litros, String numeroanalisis,int tipo, DateTime fechaanalisis,String nombrevino)
        {
            this.id = id;
            this.densidad = densidad;
            this.litros = litros;
            this.numeroanalisis = numeroanalisis;
            this.tipo = tipo;
            this.alcohol = alcohol;
            this.fechaanalisis = fechaanalisis;
            this.nombrevino = nombrevino;
        }
        public Analisis( float alcohol, float densidad, float litros, String numeroanalisis, int tipo, DateTime fechaanalisis, String nombrevino)
        {
            
            this.densidad = densidad;
            this.litros = litros;
            this.numeroanalisis = numeroanalisis;
            this.tipo = tipo;
            this.alcohol = alcohol;
            this.fechaanalisis = fechaanalisis;
            this.nombrevino = nombrevino;
        }


        // Metodos CRUD

        public void save()
        {
            String consulta;
            consulta = "INSERT INTO analisisinv(tipo,alcohol,densidad,numero,fecha,litros,nombrevino) VALUES('" + tipo + "','"+alcohol+"','"+densidad+"','"+numeroanalisis+"','"+ fechaanalisis.ToString("yyyy/MM/dd") + "','"+litros+"','"+nombrevino+"')";
            conexion.operaciones(consulta);
        }

        public void delete(int id)
        {
            

            if (verifydelete(id))
            {
                String consulta;
                consulta = "DELETE FROM analisisinv WHERE idAnalisis = '" + id + "'";
                conexion.operaciones(consulta);

            }
            
            

        }
        public void update(int id)
        {
            if (verifyupdate(id))
            {
                String consulta;
                consulta = "UPDATE analisisinv set tipo = '" + tipo + "', fecha = '" + fechaanalisis.ToString("yyyy/MM/dd") + "', numero= '" + numeroanalisis + "', densidad='" + densidad  + "', litros='" + litros + "', nombrevino='" + nombrevino + "', alcohol='"+alcohol+"' where idAnalisis = '" + id + "'; ";
                conexion.operaciones(consulta);
            }
        }

        // Funciones que chequean los datos antes de actulizar o elimnar el registro

        public bool verifyupdate(int id)
        {
            if (checkdata(id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool verifydelete(int id)
        {
            if (checkdata(id))
            {
                MessageBoxResult resultado;
                resultado = MessageBox.Show("Esta seguro que desea eliminar la Lista de precios: " + numeroanalisis, "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (resultado==MessageBoxResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        //Funcion que devuelve el tipo de analisis. Libre Circulacion o Aptitud de Exportacion

        public int gettipo(int idAnalisis)
        {
            String consulta;
            consulta = "SELECT tipo from analisisinv where idAnalisis='" + idAnalisis + "'";
            int tipo;
            String type = conexion.ValorEnVariable(consulta);
            tipo = int.Parse(type);
            return tipo;

        }

        //Funcion que devuelve el numero de analisis.

        public string getnumero(int idAnalisis)
        {
            String consulta;
            consulta = "SELECT numero from analisisinv where idAnalisis='" + idAnalisis + "'";
            return conexion.ValorEnVariable(consulta);

        }
        public bool checkdata(int id)
        {
            String consulta;
            consulta = "SELECT count(FK_idAnalisis) from op_has_productos where FK_idAnalisis = " + id + "";
            String count;
            count = conexion.ValorEnVariable(consulta);
            

            if (int.Parse(count) <= 0) {
                return true;
            }
            else
            {
                MessageBox.Show("No se puede eliminar/modificar el analisis por que existen Ordenes de Pedido asociadas.");

                return false;
            }

        }
        
        //Metodo que setea (uso de setter) los datos del analisis.

        public void setDatos(string id)
        {
            DataTable analisisINV;
            String consulta;
            consulta = "SELECT * FROM analisisinv WHERE idAnalisis='" + id + ";'";
            analisisINV=conexion.coleccion(consulta);


            Alcohol = (float)analisisINV.Rows[0].ItemArray[4];
            Densidad = (float)analisisINV.Rows[0].ItemArray[5];
            Litros = (float)analisisINV.Rows[0].ItemArray[3];
            Tipo = (int)analisisINV.Rows[0].ItemArray[6];
            Nombrevino = analisisINV.Rows[0].ItemArray[7].ToString();
            Numeroanalisis = (String)analisisINV.Rows[0].ItemArray[1];
            Fechaanalisis = (DateTime)analisisINV.Rows[0].ItemArray[2];



        }

        //Funcion que obtiene todos los analisis

        public static DataTable getAnalisis()
        {
            CRUD conexion = new CRUD();
            String consulta;
            DataTable analisisINV;
            consulta = "Select * from analisisinv";
            analisisINV=conexion.coleccion(consulta);
            return analisisINV;


            
        }

        //Funcion que obtiene los analsis segun tipo

        public static DataTable getAnalisis(string tipo)
        {
            CRUD conexion = new CRUD();
            String consulta;
            DataTable analisisINV;
            consulta = "Select * from analisisinv where tipo='"+tipo+";'";
            analisisINV = conexion.coleccion(consulta);
            return analisisINV;



        }
        public static DataTable getProductoasociado(string tipo)
        {
            CRUD conexion = new CRUD();
            String consulta;
            DataTable analisisINV;
            consulta = "Select * from analisisinv where tipo='" + tipo + ";'";
            analisisINV = conexion.coleccion(consulta);
            return analisisINV;



        }
    }
}
