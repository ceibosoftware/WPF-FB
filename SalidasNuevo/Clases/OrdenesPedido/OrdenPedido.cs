
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpfFamiliaBlanco.SalidasNuevo.Clases.OrdenesPedido
{
    
    class OrdenPedido
    {
        private CRUD conexion = new CRUD();
        private int id;
        private int numeroOrden;
        private DateTime fecha;
        private int idCliente;
        private float subtotal;
        private int tipoIva;
        private float total;
        private int totalCajas;
        private int totalBotellas;
        private float descuento;
        private string notas;
        private List<ProductoOP> productosOP = new List<ProductoOP>();
        private List<ProductoOP> productosMuestra = new List<ProductoOP>();
        
        // muestras
        private float subtotalMuestra;
        private int tipoIvaMuestra;
        private float totalMuestra;
        private int totalCajasMuestra;
        private int totalBotellasMuestra;
        private float descuentoMuestra;

        public float Subtotal { get => subtotal; set => subtotal = value; }
        internal List<ProductoOP> ProductosOP { get => productosOP; set => productosOP = value; }
        public int TotalCajas { get => totalCajas; set => totalCajas = value; }
        public int TotalBotellas { get => totalBotellas; set => totalBotellas = value; }
        public float Total { get => total; set => total = value; }
        public float Descuento { get => descuento; set => descuento = value; }
        internal List<ProductoOP> ProductosMuestra { get => productosMuestra; set => productosMuestra = value; }
        public float SubtotalMuestra { get => subtotalMuestra; set => subtotalMuestra = value; }
        public int TipoIvaMuestra { get => tipoIvaMuestra; set => tipoIvaMuestra = value; }
        public float TotalMuestra { get => totalMuestra; set => totalMuestra = value; }
        public int TotalCajasMuestra { get => totalCajasMuestra; set => totalCajasMuestra = value; }
        public int TotalBotellasMuestra { get => totalBotellasMuestra; set => totalBotellasMuestra = value; }
        public float DescuentoMuestra { get => descuentoMuestra; set => descuentoMuestra = value; }
        public int NumeroOrden { get => numeroOrden; set => numeroOrden = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public int TipoIva { get => tipoIva; set => tipoIva = value; }
        public string Notas { get => notas; set => notas = value; }
        public int IdCliente { get => idCliente; set => idCliente = value; }
        public int Id { get => id; set => id = value; }
        internal CRUD Conexion { get => conexion; set => conexion = value; }


        public bool Valida()
        {
            if (string.IsNullOrEmpty(fecha.ToString()))
            {
                MessageBox.Show("Es necesario seleccionar una fecha");
                return false;
            }else if(idCliente < 0)
            {
                MessageBox.Show("Es necesario seleccionar una cliente");
                return false;
            }else if(ProductosOP.Count() <= 0)
            {
                MessageBox.Show("Es necesario agregar al menos un producto");
                return false;
            }else if(tipoIva < 0)
            {
                MessageBox.Show("Es necesario seleccionar un tipo de iva");
                return false;
            }
                return true;

        }
        public void save()
        {

            // insertar datos OP

          
            String consulta = "INSERT INTO ordenespedido (fecha, subtotal, tipoIva, totalCajas, totalBotellas, descuento, notas, subtotalMuestra, tipoIvaMuestra, totalMuestra, totalCajasMuestra,totalBotellasMuestra, descuentoMuestra, FK_idClientemi, total)  VALUES ('"+fecha.ToString("dd/MM/yy")+"','"+subtotal+ "','"+tipoIva+ "','"+totalCajas+ "', '"+totalBotellas+ "','"+descuento+ "','"+notas+ "','"+subtotalMuestra+ "','"+tipoIvaMuestra+ "', '"+totalMuestra+ "', " + "'"+totalCajasMuestra+ "', '"+TotalBotellasMuestra+ "', '"+descuentoMuestra+ "', '"+idCliente+"', '"+total+"')";
            conexion.operaciones(consulta);
            string ultimoId = "Select last_insert_id()";
            id = int.Parse(conexion.ValorEnVariable(ultimoId));
            foreach (var producto in productosOP)
            {
                String insertarProducto = "INSERT INTO op_has_productos (FK_idOP, FK_idProductos, cajas, cajasPor, totalBotellas, descuento, descuentoPesos, importe, FK_idAnalisis, precioVenta) VALUES ('"+id+"','"+producto.Id+ "','"+producto.Cajas+ "', '"+producto.CajasPor+ "', '"+producto.TotalBotellas+ "','"+producto.Descuento+ "', '"+producto.DescuentoPesos+ "', '"+producto.Importe+"', '"+producto.IdINV+"', '"+producto.Precio+"')";
                conexion.operaciones(insertarProducto);
            }
            foreach (var producto in productosMuestra)
            {
                String insertarProducto = "INSERT INTO opmuestras_has_productos (FK_idOPMI, FK_idProductos, cajas, cajasPor, totalBotellas, descuento, descuentoPesos, importe, FK_idAnalisis, precioVenta ) VALUES ('" + id + "','" + producto.Id + "','" + producto.Cajas + "', '" + producto.CajasPor + "', '" + producto.TotalBotellas + "','" + producto.Descuento + "', '" + producto.DescuentoPesos + "', '" + producto.Importe + "', '"+ producto.IdINV +"', '"+producto.Precio +"')";
                conexion.operaciones(insertarProducto);
            }
            MessageBox.Show("Orden de pedido agregada correctamente");   
                     
           
        }
        public void AgregarProducto(ProductoOP producto)
        {
            
            productosOP.Add(producto);
        }
        public void calculaSubtotal()
        {
            subtotal = 0;
            foreach (var producto in ProductosOP)
            {
                subtotal += producto.Importe;
            }
        }
        public void calculaTotalcajas()
        {
            totalCajas = 0;
            foreach (var producto in ProductosOP)
            {
                totalCajas += producto.Cajas;
            }
        }
        public void calculaTotalBotellas()
        {
            totalBotellas = 0;
            foreach (var producto in ProductosOP)
            {
                totalBotellas += producto.TotalBotellas;
            }
        }
        public void clear()
        {
            totalBotellas = 0;
            totalCajas = 0;
            subtotal = 0;
            ProductosOP.Clear();
            total = 0;
            descuento = 0;
            //muestras
            totalBotellasMuestra = 0;
            totalCajasMuestra= 0;
            subtotalMuestra = 0;
            productosMuestra.Clear();
            totalMuestra = 0;
            descuentoMuestra = 0;
        }
        public void calculaTotal(int tipoIva)
        {
            this.tipoIva = tipoIva;
            if (tipoIva == 0)
                this.total = this.subtotal * 1f;
            else if(tipoIva == 1)
                this.total = this.subtotal * 1.21f;
            else if (tipoIva == 2)
                this.total = this.subtotal * 1.105f;
        }
        public void CalculaDescuento(float descuento)
        {
            this.descuento = descuento / 100f;
            this.total = this.total - (total * this.descuento);
        }
        public DataTable GetOrdenes()
        {
            string consulta = "select idOPMI from ordenesPedido ";
            DataTable ordenes = conexion.coleccion(consulta);
            return ordenes;
        }
        public void GetOrdenes(int idOrden)
        {
            
            string consulta = "select * from ordenesPedido where idOPMI = '"+idOrden+"' ";
            DataTable orden = conexion.coleccion(consulta);
            SetDatos(orden);
            SetProductos(idOrden);

        }
        public DataTable  GetOrdenesCliente(int idCliente)
        {

            string consulta = "select * from ordenesPedido where FK_idClientemi = '" + idCliente + "' ";
            DataTable orden = conexion.coleccion(consulta);
            return orden;
            
        }
        private void SetDatos(DataTable orden)
        {
            fecha = (DateTime)orden.Rows[0].ItemArray[1];
            subtotal = (float)orden.Rows[0].ItemArray[2];
            total = (float)orden.Rows[0].ItemArray[15];
            tipoIva = (int)orden.Rows[0].ItemArray[3];
            totalCajas = (int)orden.Rows[0].ItemArray[4];
            totalBotellas = (int)orden.Rows[0].ItemArray[5];
            Descuento = (float)orden.Rows[0].ItemArray[6];
            notas = orden.Rows[0].ItemArray[7].ToString();
            idCliente = (int)orden.Rows[0].ItemArray[14];
            Id = (int)orden.Rows[0].ItemArray[0];
            // muestra

            subtotalMuestra = (float)orden.Rows[0].ItemArray[8];
            tipoIvaMuestra = (int)orden.Rows[0].ItemArray[9];
            totalMuestra = (float)orden.Rows[0].ItemArray[10];
            totalCajasMuestra = (int)orden.Rows[0].ItemArray[11];
            totalBotellasMuestra = (int)orden.Rows[0].ItemArray[12];
            descuentoMuestra = (float)orden.Rows[0].ItemArray[13];


            
        }
        private void SetProductos(int idOrden)
        {
            //Limpiamos listas
            productosOP.Clear();
            ProductosMuestra.Clear();

            string consulta = "select * from op_has_productos where FK_idOP = '" + idOrden + "'";
            DataTable productos = conexion.coleccion(consulta);
            for (int i = 0; i < productos.Rows.Count; i++)
            {
                int idProducto = (int)productos.Rows[i].ItemArray[1];
                int Cajas = (int)productos.Rows[i].ItemArray[2] ;
                int CajasPor = (int)productos.Rows[i].ItemArray[3];
                int TotalBotellas = (int)productos.Rows[i].ItemArray[4];
                float Descuento = (float)productos.Rows[i].ItemArray[5];
                float DescuentoPesos = (float)productos.Rows[i].ItemArray[6];
                float Importe = (float)productos.Rows[i].ItemArray[7];
                int  idAnalisis = (int)productos.Rows[i].ItemArray[9];
                float precio = (float)productos.Rows[i].ItemArray[10];
                productosOP.Add(new ProductoOP(idProducto,Cajas,CajasPor,TotalBotellas,Descuento,DescuentoPesos,Importe,idAnalisis,precio));
            }
            // muestra
            string consultaMuestra = "select * from opmuestras_has_productos where FK_idOPMI = '" + idOrden + "'";
            DataTable productosMuestra = conexion.coleccion(consultaMuestra);
            for (int i = 0; i < productosMuestra.Rows.Count; i++)
            {
                int idProducto = (int)productosMuestra.Rows[i].ItemArray[1];
                int Cajas = (int)productosMuestra.Rows[i].ItemArray[2];
                int CajasPor = (int)productosMuestra.Rows[i].ItemArray[3];
                int TotalBotellas = (int)productosMuestra.Rows[i].ItemArray[4];
                float Descuento = (float)productosMuestra.Rows[i].ItemArray[5];
                float DescuentoPesos = (float)productosMuestra.Rows[i].ItemArray[6];
                float Importe = (float)productosMuestra.Rows[i].ItemArray[7];
                int idAnalisis = (int)productosMuestra.Rows[i].ItemArray[10];
                float precio = (float)productosMuestra.Rows[i].ItemArray[9];
                ProductosMuestra.Add(new ProductoOP(idProducto, Cajas, CajasPor, TotalBotellas, Descuento, DescuentoPesos, Importe, 1, precio));
            }
        }
        //metodos muestra
        public void CalculaDescuentoMuestra(float descuento)
        {
            this.descuentoMuestra = descuento / 100f;
            this.totalMuestra = this.totalMuestra - (totalMuestra * this.descuentoMuestra);
        }
        public void AgregarProductoMuestra(ProductoOP producto)
        {

            productosMuestra.Add(producto);
        }
        public void calculaSubtotalMuestra()
        {
            subtotalMuestra = 0;
            foreach (var producto in ProductosMuestra)
            {
                subtotalMuestra += producto.Importe;
            }
        }
        public void calculaTotalcajasMuestra()
        {
            totalCajasMuestra = 0;
            foreach (var producto in ProductosMuestra)
            {
                totalCajasMuestra += producto.Cajas;
            }
        }
        public void calculaTotalBotellasMuestra()
        {
            totalBotellasMuestra = 0;
            foreach (var producto in ProductosMuestra)
            {
                totalBotellasMuestra += producto.TotalBotellas;
            }
        }
        public void calculaTotalMuestra(int tipoIva)
        {
            this.tipoIvaMuestra = tipoIva;
            if (tipoIvaMuestra == 0)
                this.totalMuestra = this.subtotalMuestra * 1f;
            else if (tipoIvaMuestra == 1)
                this.totalMuestra = this.subtotalMuestra * 1.21f;
            else if (tipoIvaMuestra == 2)
                this.totalMuestra = this.subtotalMuestra * 1.105f;
        }

    }
}
