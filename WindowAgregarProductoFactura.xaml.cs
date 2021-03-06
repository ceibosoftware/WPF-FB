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
using System.Windows.Shapes;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Lógica de interacción para WindowAgregarProductoFactura.xaml
    /// </summary>
    public partial class WindowAgregarProductoFactura : Window
    {
        public string canpararestar;
        public int can;
        public String cantidadmensaje;
        public WindowAgregarProductoFactura()
        {
            InitializeComponent();
           
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(txtCantidad.Text, out int cantidad);
       
            if (can >= cantidad &&  !string.IsNullOrEmpty(txtCantidad.Text))
            {
                DialogResult = true;
            }else if (string.IsNullOrEmpty(txtCantidad.Text))
            {
                MessageBox.Show("Ingrese una cantidad.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
         
            }
            else
            {
                MessageBox.Show("La cantidad no puede ser mayor a " + can, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            
            }
          
        }

        private void txtCantidad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void btncancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
