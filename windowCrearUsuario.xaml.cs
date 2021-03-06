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
    /// Interaction logic for windowCrearUsuario.xaml
    /// </summary>
    public partial class windowCrearUsuario : Window
    {

        CRUD conexion = new CRUD();

        public windowCrearUsuario()
        {
            InitializeComponent();
            LlenarComboUsuario();
            cmbTipoUsuario.SelectedIndex = 1;

            if (windowUsuarios.tipoUsuarioDB == "basico")
            {

                this.btnSubmit.IsEnabled = false;
            }

        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs args)
        {
            this.Owner.Effect = null;
            base.OnClosing(args);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            String name = txtUsername.Text.ToString();
            String nombreDB = "SELECT COUNT(*) FROM usuarios WHERE usuario  = '" + name + "'";
            String nomCat = conexion.ValorEnVariable(nombreDB).ToString();
    

            if (txtUsername.Text != "" && txtPassword.Password.ToString() != "" && nomCat == "0" )
            {
                this.DialogResult = true;
            }
            else if (txtUsername.Text == "")
            {
                MessageBox.Show("Debe ingresar el nombre de usuario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
             

            }else if (txtPassword.Password.ToString() == "")
            {
                MessageBox.Show("Debe ingresar una contraseña", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!nomCat.Equals("0"))
            {
                MessageBox.Show("El usuario ingresado ya existe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
              
            }else if (cmbTipoUsuario.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Debe seleccionar un tipo de usuario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
         
            }
            
        }

        private void LlenarComboUsuario()
        {

            cmbTipoUsuario.Items.Add("basico");
            cmbTipoUsuario.Items.Add("super");

        }
    }
}
