﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpfFamiliaBlanco.Entradas;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for pageEntradas.xaml
    /// </summary>
    public partial class pageEntradas : Page
    {
    
         CRUD conexion = new CRUD();
        MainWindow mw = new MainWindow();
        public pageEntradas()
        {
            InitializeComponent();
            frameInicioEntradas.Content = new Ordenes();
            btnOrdenes.Style = FindResource("botonTabPressed") as Style;
            notificaciones();
        }

      
        public void refrescarpagina()
        {
          

        }
        public void notificaciones()
        {
            String valor = "SELECT COUNT(*) FROM cuotas WHERE estado = '" + 0 + "'";
            String CantidadPagos = "";

            CantidadPagos = this.conexion.ValorEnVariable(valor).ToString();

            if (CantidadPagos == "")
            {
                lblbotificaciones.Content = 0;
            }
            else if (CantidadPagos != "")
            {
                lblbotificaciones.Visibility = Visibility.Visible;
                lblbotificaciones.Content = "";
                lblbotificaciones.Content = CantidadPagos;

                ToolTip tt = new ToolTip();
                tt.FontSize = 15;

                tt.Content = CantidadPagos + " pagos próximos a vencer.";
                lblbotificaciones.ToolTip = tt;
            }
        }

        private void btnFacturas_Click(object sender, RoutedEventArgs e)
        {
            frameInicioEntradas.Content = new Facturacion();
            btnOrdenes.Style = FindResource("botonTab") as Style;
            btnRemitos.Style = FindResource("botonTab") as Style;
            btnFacturas.Style = FindResource("botonTabPressed") as Style;
            btnPagoProveedor.Style = FindResource("botonTab") as Style;
            btnDevolucionProductos.Style = FindResource("botonTab") as Style;
        }

        private void btnOrdenes_Click(object sender, RoutedEventArgs e)
        {
            frameInicioEntradas.Content = new Ordenes();
            btnOrdenes.Style = FindResource("botonTabPressed") as Style;
            btnRemitos.Style = FindResource("botonTab") as Style;
            btnFacturas.Style = FindResource("botonTab") as Style;
            btnPagoProveedor.Style = FindResource("botonTab") as Style;
            btnDevolucionProductos.Style = FindResource("botonTab") as Style;
        }

        private void btnRemitos_Click(object sender, RoutedEventArgs e)
        {
            frameInicioEntradas.Content = new Remito();
            btnOrdenes.Style = FindResource("botonTab") as Style;
            btnRemitos.Style = FindResource("botonTabPressed") as Style;
            btnFacturas.Style = FindResource("botonTab") as Style;
            btnDevolucionProductos.Style = FindResource("botonTab") as Style;
            btnPagoProveedor.Style = FindResource("botonTab") as Style;
        }

        private void btnPagoProveedor_Click(object sender, RoutedEventArgs e)
        {
   
        }

        private void btnPagoProveedor_Click_1(object sender, RoutedEventArgs e)
        {
            frameInicioEntradas.Content = new PagoProveedores();
            btnOrdenes.Style = FindResource("botonTab") as Style;
            btnRemitos.Style = FindResource("botonTab") as Style;
            btnFacturas.Style = FindResource("botonTab") as Style;
            btnDevolucionProductos.Style = FindResource("botonTab") as Style;
            btnPagoProveedor.Style = FindResource("botonTabPressed") as Style;
        }

        private void btnDevolucionProductos_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnDevolucion_Click_1(object sender, RoutedEventArgs e)
        {
           

            frameInicioEntradas.Content = new DevolucionProductos();
            btnOrdenes.Style = FindResource("botonTab") as Style;
            btnRemitos.Style = FindResource("botonTab") as Style;
            btnFacturas.Style = FindResource("botonTab") as Style;
            btnPagoProveedor.Style = FindResource("botonTab") as Style;
            btnDevolucionProductos.Style = FindResource("botonTabPressed") as Style;
            
          
        }
    }
}
