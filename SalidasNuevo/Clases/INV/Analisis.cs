using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco.SalidasNuevo.Clases.INV
{
    
    class Analisis
    {
        CRUD conexion = new CRUD();


        private string numeroanalisis;
        private int tipo;
        private DateTime fechaanalisis;
        private float alcohol;
        private float densidad;
        private float litros;
        private string nombrevino;

        public string Numeroanalisis { get => numeroanalisis; set => numeroanalisis = value; }
        public int Tipo { get => tipo; set => tipo = value; }
        public float Alcohol { get => alcohol; set => alcohol = value; }
        public float Densidad { get => densidad; set => densidad = value; }
        public string Nombrevino{ get => nombrevino; set => nombrevino = value; }
        public float Litros { get => litros; set => litros = value; }
        public DateTime Fechaanalisis { get => fechaanalisis; set => fechaanalisis = value; }

        public Analisis(){ }

        public Analisis(float alcohol,float densidad, float litros, String numeroanalisis,int tipo, DateTime fechaanalisis,String nombrevino)
        {
            this.densidad = densidad;
            this.litros = litros;
            this.numeroanalisis = numeroanalisis;
            this.tipo = tipo;
            this.alcohol = alcohol;
            this.fechaanalisis = fechaanalisis;
            this.nombrevino = nombrevino;
        }

        public void save()
        {
            String consulta;
            consulta = "INSERT INTO analisisinv(tipo,alcohol,densidad,numero,fecha,litros,nombrevino) VALUES('" + tipo + "','"+alcohol+"','"+densidad+"','"+numeroanalisis+"','"+ fechaanalisis.ToString("yyyy/MM/dd") + "','"+litros+"','"+nombrevino+"')";
            conexion.operaciones(consulta);
        }
        
        public void setDatos(string id, string tipo)
        {
            DataTable analisisINV;
            String consulta;
            consulta = "Select * from analisisinv where idAnalisis='" + id + ";'";
            analisisINV=conexion.coleccion(consulta);


            Alcohol = (float)analisisINV.Rows[0].ItemArray[4];
            Densidad = (float)analisisINV.Rows[0].ItemArray[5];
            Litros = (float)analisisINV.Rows[0].ItemArray[3];
            Tipo = (int)analisisINV.Rows[0].ItemArray[6];
            Nombrevino = analisisINV.Rows[0].ItemArray[7].ToString();
            Numeroanalisis = (String)analisisINV.Rows[0].ItemArray[1];
            Fechaanalisis = (DateTime)analisisINV.Rows[0].ItemArray[2];



        }

        public static DataTable getAnalisis()
        {
            CRUD conexion = new CRUD();
            String consulta;
            DataTable analisisINV;
            consulta = "Select * from analisisinv";
            analisisINV=conexion.coleccion(consulta);
            return analisisINV;


            
        }
        public static DataTable getAnalisis(string tipo)
        {
            CRUD conexion = new CRUD();
            String consulta;
            DataTable analisisINV;
            consulta = "Select * from analisisinv where tipo='"+tipo+";'";
            analisisINV = conexion.coleccion(consulta);
            return analisisINV;



        }
       
    }
}
