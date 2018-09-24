using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfFamiliaBlanco.SalidasNuevo.Clases.Factura_Proforma
{
    class facturaProforma
    {
        private int id;
        private DateTime fecha;
        private int idCliente;
        private float subtotal;
        private float FSPA;
        private float total;
        private float descuento;
        private int totalCaja;
        private int totalBotella;
        private string notas;
        private List<ProductoFP> productos;
        //muestras
        private List<ProductoFP> muestra;
        private float subtotalMuestra;
        private float FSPAMuestas;
        private float totalMuestra;
        private float descuentoMuestra;
        private int totalCajasMuestra;
        private int totalBotellasMuestra;
        //anexo
        private List<ProductoFP> anexo;
        private float subtotalAnexo;
        private float FSPAanexo;
        private float TotalAnexo;
        //pallets
        private List<Pallet> packingInterno;
        
    }
}
