using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco.Tesoreria.Clases
{
    class CuentaBancaria
    {
        CRUD conexion = new CRUD();

        private int tipo;
        private String titular;
        private String alias;
        private String banco;
        private String numero;
        private String cbu;
        private int moneda;
        private float saldo;
        private string obs;

        public int Tipo { get => tipo; set => tipo = value; }
        public string Titular { get => titular; set => titular = value; }
        public string Alias { get => alias; set => alias = value; }
        public string Banco { get => banco; set => banco = value; }
        public string Numero { get => numero; set => numero = value; }
        public string Cbu { get => cbu; set => cbu = value; }
        public int Moneda { get => moneda; set => moneda = value; }
        public float Saldo { get => saldo; set => saldo = value; }
        public string Obs { get => obs; set => obs = value; }




        public CuentaBancaria(int tipo, string titular, string alias, string banco, string numero, string cbu, string obs,float saldo, int moneda)
        {
            this.tipo = tipo;
            this.titular = titular;
            this.alias = alias;
            this.moneda = moneda;
            this.banco = banco;
            this.numero = numero;
            this.cbu = cbu;
            this.obs = obs;
            this.saldo = saldo;
        }
        public CuentaBancaria()
        {

        }

        public static DataTable getCta()
        {
            CRUD conexion = new CRUD();
            String consulta;
            DataTable ctas;
            consulta = "Select * from cuentabanco";
            ctas = conexion.coleccion(consulta);
            return ctas;



        }
        public void setDatos(string id, string tipo)
        {
            DataTable cuenta;
            String consulta;
            consulta = "Select * from cuentabanco where idCuentaBco='" + id + ";'";
            cuenta = conexion.coleccion(consulta);


            Tipo = (int)cuenta.Rows[0].ItemArray[4];
            Cbu = (String)cuenta.Rows[0].ItemArray[1];
            Alias = (String)cuenta.Rows[0].ItemArray[6];
            Titular = (String)cuenta.Rows[0].ItemArray[2];
            Banco = (String)cuenta.Rows[0].ItemArray[3];
            Saldo = (float)cuenta.Rows[0].ItemArray[5];
            Moneda = (int)cuenta.Rows[0].ItemArray[8];
            Obs = (String)cuenta.Rows[0].ItemArray[9];

        }
        public void save()
        {
            String consulta;
            consulta = "INSERT INTO cuentabanco(tipocta,cbu,alias,numerocta,nombreTitular,saldo,moneda,banco,obs) VALUES('" + tipo + "','" + cbu + "','" + alias + "','" + numero + "','" + titular + "','" + saldo + "','" + moneda + "','"+banco+"','"+obs+"')";
            conexion.operaciones(consulta);
        }

    }

   
}
