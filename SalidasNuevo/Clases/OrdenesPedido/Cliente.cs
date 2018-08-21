using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpfFamiliaBlanco.SalidasNuevo.Clases.OrdenesPedido
{
    public class Cliente
    {
        private CRUD conexion = new CRUD();
        private int id;
        private string nombre;
        private string transporte;
        private string telefonoTransporte;
        private string direccionEntrega;
        private string cuit;
        private int idLista;
        
        public string Nombre { get => nombre; set => nombre = value; }
        public string Transporte { get => transporte; set => transporte = value; }
        public string TelefonoTransporte { get => telefonoTransporte; set => telefonoTransporte = value; }
        public string DireccionEntrega { get => direccionEntrega; set => direccionEntrega = value; }
        public int Id { get => id; set => id = value; }
        public string Cuit { get => cuit; set => cuit = value; }
        public int IdLista { get => idLista; set => idLista = value; }

        public DataTable getClientes()
        {
            DataTable clientes;
            string consulta = "SELECT nombre, idClientemi FROM clientesMI";
            clientes = conexion.coleccion(consulta);
            return clientes;
        }
        public DataTable getClientesOPedidos()
        {
            DataTable clientes;
            string consulta = "SELECT  DISTINCT  nombre, idClientemi FROM clientesMI c , ordenespedido op where c.idClientemi = op.FK_idClientemi ";
            clientes = conexion.coleccion(consulta);
            return clientes;
        }
        public void setDatos(string id)
        {
            
            DataTable cliente;
            string consulta = "SELECT nombre, idClientemi, transporte, teltransporte, direccionentrega, cuit, FK_idLista FROM clientesMI WHERE idClientemi = '"+id+"'";
           
            cliente = conexion.coleccion(consulta);
            Id = (int)cliente.Rows[0].ItemArray[1];
            Nombre = cliente.Rows[0].ItemArray[0].ToString();
            Transporte = cliente.Rows[0].ItemArray[2].ToString();
            TelefonoTransporte = cliente.Rows[0].ItemArray[3].ToString();
            DireccionEntrega = cliente.Rows[0].ItemArray[4].ToString();
            cuit = cliente.Rows[0].ItemArray[5].ToString();
            idLista = (int)cliente.Rows[0].ItemArray[6];
        }
        public void actualizarTransporte()
        {
            string consulta = "UPDATE clientesmi SET transporte = '"+transporte+"', teltransporte = '"+telefonoTransporte+"', direccionentrega = '"+ direccionEntrega+"' where idClientemi = '"+id+"'";
            conexion.operaciones(consulta);
        }
        public string getLista()
        {
            string consulta = "SELECT nombre FROM listadeprecios WHERE idLista = '" + idLista + "'";
            return conexion.ValorEnVariable(consulta);
        }

    }
}
