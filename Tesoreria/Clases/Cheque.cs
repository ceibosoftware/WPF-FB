using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco.Tesoreria.Clases
{
    class Cheque
    {
        CRUD conexion = new CRUD();

        private int tipo;
        private DateTime fechacobro;
        private DateTime fechaconfeccion;
        private float importe;
        private int numero;
        private string banco;

        public int Tipo { get => tipo; set => tipo = value; }
        public DateTime Fechacobro { get => fechacobro; set => fechacobro = value; }
        public DateTime Fechaconfeccion { get => fechaconfeccion; set => fechaconfeccion = value; }
        public float Importe { get => importe; set => importe = value; }
        public int Numero { get => numero; set => numero = value; }
        public string Banco { get => banco; set => banco = value; }

        public void save(int tipo)
        {
            if (tipo==0)//el cheque es tipo 0 cuando es propio y tipo 1 cuando es de terceros.
            {
                String consulta;
                consulta = "INSERT INTO carteracheque(tipo,fechacobro,fechaconfeccion,numero,importe,banco) VALUES('" + tipo + "','" + fechacobro.ToString("yyyy/MM/dd") + "','" + fechaconfeccion.ToString("yyyy/MM/dd") + "','" + numero + "','" + importe + "','" + banco + "')";
                conexion.operaciones(consulta);
            }
            else
            {
                String consulta;
                consulta = "INSERT INTO carteracheque(tipo,fechacobro,fechaconfeccion,numero,importe,banco) VALUES('" + tipo + "','" + fechacobro.ToString("yyyy/MM/dd") + "','" + fechaconfeccion.ToString("yyyy/MM/dd") + "','" + numero + "','" + importe + "','" + banco + "')";
                conexion.operaciones(consulta);
            }
            
        }

        

        public string numerocheque(int id)
        {
            String ultimonumero;
            String consulta;
            consulta="SELECT numero from carteracheque where tipo='"+0+"'AND idCheque='"+id+";";
            ultimonumero = conexion.ValorEnVariable(consulta);
            int correlativo = int.Parse(ultimonumero);
            correlativo = correlativo + 1;
            ultimonumero = correlativo.ToString();

            return ultimonumero;
        }

        public void setDatos(string id, string tipo)
        {
            DataTable cheque;
            String consulta;
            consulta = "Select * from carteracheque where idCheque='" + id + ";'";
            cheque = conexion.coleccion(consulta);


            Tipo = (int)cheque.Rows[0].ItemArray[1];
            Fechacobro = (DateTime)cheque.Rows[0].ItemArray[2];
            Fechaconfeccion = (DateTime)cheque.Rows[0].ItemArray[3];
            Importe = (float)cheque.Rows[0].ItemArray[4];
            Numero = (int)cheque.Rows[0].ItemArray[5];
            Banco = (String)cheque.Rows[0].ItemArray[6];



        }

        public static DataTable getCheque()
        {
            CRUD conexion = new CRUD();
            String consulta;
            DataTable cheque;
            consulta = "Select * from carteracheque";
            cheque = conexion.coleccion(consulta);
            return cheque;



        }

        public static DataTable getCheque(int id)
        {
            CRUD conexion = new CRUD();
            String consulta;
            DataTable cheque;
            consulta = "Select * from carteracheque where idCheque='"+id+";'";
            cheque = conexion.coleccion(consulta);
            return cheque;



        }
    }
}
