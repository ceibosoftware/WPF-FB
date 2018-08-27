using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco.SalidasNuevo.Clases.FMI
{
    class FacturaMi
    {
        CRUD conexion = new CRUD();
        Impuesto impuestoActual = new Impuesto();
        private int id;
        private String tipoFactura;
        private DateTime fecha;
        private DateTime fechaVencimiento;
        private String numeroFactura;
        private int diasVencimiento;

        private int idOP;
        private int idCLiente;
        private List<Impuesto> impuestos = new List<Impuesto>();

        public int Id { get => id; set => id = value; }
        public string TipoFactura { get => tipoFactura; set => tipoFactura = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public DateTime FechaVencimiento { get => fechaVencimiento; set => fechaVencimiento = value; }
        public string NumeroFactura { get => numeroFactura; set => numeroFactura = value; }
        public int DiasVencimiento { get => diasVencimiento; set => diasVencimiento = value; }
        public int IdOP { get => idOP; set => idOP = value; }
        public int IdCLiente { get => idCLiente; set => idCLiente = value; }
        internal List<Impuesto> ImpuestosF { get => impuestos; set => impuestos = value; }

        


        public void Insertar()
        {
            String insertarDatos = "INSERT INTO facturami (numeroFactura, fecha, diasVencimiento, tipoFactura, fechaVencimiento, FK_idOPMI) VALUES ('" + numeroFactura + "', '" + fecha.ToString("yyyy/MM/dd") + "', '" + diasVencimiento + "', '" + tipoFactura + "', '"+fecha.ToString("yyyy/MM/dd") + "', '"+idOP+"')";
            conexion.operaciones(insertarDatos);

            foreach (var item in ImpuestosF)
            {
                item.Insertar();
            }
            
        }

        public void Eliminar()
        {
            String ELiminarFactura = "DELETE FROM facturami WHERE id = '"+this.id+"'";
            conexion.operaciones(ELiminarFactura);
        }

    }
}
