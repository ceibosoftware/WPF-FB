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
using wpfFamiliaBlanco.Clientes;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for pageClientes.xaml
    /// </summary>
    public partial class pageClientes : Page
    {
        public pageClientes()
        {
            InitializeComponent();
            btnClientes.Style = FindResource("botonTabPressed") as Style;
            frameInicioClientes.Content = new MI();
           
        }

    


        private void btnLP_Click(object sender, RoutedEventArgs e)
        {
            btnClientes.Style = FindResource("botonTab") as Style;
            btnLP.Style = FindResource("botonTabPressed") as Style;
            frameInicioClientes.Content = new LP();
        }

        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            btnClientes.Style = FindResource("botonTabPressed") as Style;
            btnLP.Style = FindResource("botonTab") as Style;
            frameInicioClientes.Content = new MI();
            

            

        }
    }
}
