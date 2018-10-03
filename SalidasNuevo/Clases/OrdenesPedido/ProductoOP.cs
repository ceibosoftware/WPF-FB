using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpfFamiliaBlanco.SalidasNuevo.Clases.OrdenesPedido
{
   public class ProductoOP
    {

        public ProductoOP()
        {

        }
        public ProductoOP(int idProducto, int cajas,int cajasPor, int totalBotellas, float descuento,float descuentoPesos, float importe,int idAnalisis, float precio)
        {
            nombre = GetNombre(idProducto);
            Cajas = cajas;
            CajasPor = cajasPor;
            TotalBotellas = totalBotellas;
            Descuento = descuento;
            DescuentoPesos = descuentoPesos;
            Importe = importe;
            codigoINV = GetCodigoINV(idAnalisis);
            Precio = precio;
        }

        public ProductoOP(int idProducto, int cantidad, float descuento, float precio)
        {
            nombre = GetNombre(idProducto);
            Id =int.Parse( Getid(nombre));
            TotalBotellas = cantidad;
            Descuento = descuento;
            Precio = precio;
           
        }

        private CRUD conexion = new CRUD();
        private int id;
        private string nombre;
        private float precio;
        private int stock;
        private int cajas;
        private int cajasPor;
        private int totalBotellas;
        private float descuento = 0;
        private float descuentoPesos;
        private float importe;
        private int idINV;
        private string codigoINV;

        public string Nombre { get => nombre; set => nombre = value; }
        public float Precio { get => precio; set => precio = value; }
        public int Stock { get => stock; set => stock = value; }
        public int Cajas { get => cajas; set => cajas = value; }
        public int CajasPor { get => cajasPor; set => cajasPor = value; }
        public int TotalBotellas { get => totalBotellas; set => totalBotellas = value; }
        public float Descuento  { get => descuento; set => descuento = value; }
        public float DescuentoPesos { get => descuentoPesos; set => descuentoPesos = value; }
        public float Importe { get => importe; set => importe = value; }
        public int IdINV { get => idINV; set => idINV = value; }
        public int Id { get => id; set => id = value; }
        public string CodigoINV { get => codigoINV; set => codigoINV = value; }
        public void SetStock(int idProducto)
        {
            string consulta = "SELECT stock FROM productos where idProductos = '"+idProducto+"'";
            this.stock = int.Parse(conexion.ValorEnVariable(consulta));            
        }
        public void CalculaTotalBotellas(int cajas, int cajasPor)
        {
            this.Cajas = cajas;
            this.CajasPor = cajasPor;
            this.totalBotellas = Cajas * CajasPor;
        }
        public void CalculaImporte(float Precio, int TotalBotellas)
        {
            Importe = (Precio * TotalBotellas);
        }
        public void CalculaDescuentoPesos(float descuento, float importe)
        {
            this.Descuento = descuento ;
            DescuentoPesos = importe * (this.Descuento / 100f);
        }
        public void aplicaDescuento()
        {
            Importe = Importe - DescuentoPesos;
        }
        public void clear()
        {
            cajas = 0;
            CajasPor = 0;
            totalBotellas = 0;
            descuento = 0;
            DescuentoPesos = 0;
            importe = 0;

        }
        private String GetNombre(int idProducto)
        {
            string consulta = "SELECT nombre FROM productos where idProductos = '"+idProducto+"'";
            return conexion.ValorEnVariable(consulta);
        }

        public String Getid(String  n)
        {
            string consulta = "SELECT idProductos FROM productos where nombre = '" + n + "'";
            return conexion.ValorEnVariable(consulta);
        }
        private String GetCodigoINV(int idAnalisis)
        {
            string consulta = "SELECT numero FROM analisisinv where idAnalisis = '" + idAnalisis + "'";
            return conexion.ValorEnVariable(consulta);
        }

    }
}
