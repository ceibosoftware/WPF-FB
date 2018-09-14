using System;
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
using wpfFamiliaBlanco.Gastos.Clases;

namespace wpfFamiliaBlanco.Gastos.Vistas
{
    /// <summary>
    /// Interaction logic for windowTipoGasto.xaml
    /// </summary>
    public partial class windowTipoGasto : Window
    {

        CRUD conexion = new CRUD();
        public windowTipoGasto()
        {
            InitializeComponent();
            
        }

        private void btnImpuesto_Click(object sender, RoutedEventArgs e)
        {
            windowAgregarImpuesto newW = new windowAgregarImpuesto();
            
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {
                DateTime dtp = System.DateTime.Now;
                dtp = newW.dtpFecha.SelectedDate.Value;
                Impuesto i = new Impuesto(newW.txtNombre.Text, newW.cmbTipo.SelectedIndex, float.Parse(newW.txtMonto.Text), newW.txtObservaciones.Text, newW.cmbFormadePAgo.SelectedIndex, 0, dtp);
                i.Save();
                MessageBox.Show("El impuesto se agregó correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                this.Close();
            }
          
        }

        private void btnServicio_Click(object sender, RoutedEventArgs e)
        {
            windowAgregarServicio newW = new windowAgregarServicio();
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {
                DateTime dtp = System.DateTime.Now;
                dtp = newW.dtpFecha.SelectedDate.Value;
                Servicio i = new Servicio(newW.txtNombre.Text, newW.cmbTipo.SelectedIndex, float.Parse(newW.txtMonto.Text), newW.txtObservaciones.Text, newW.cmbFormadePAgo.SelectedIndex, 0, newW.txtUnidad.Text,dtp);
                i.Save();
                MessageBox.Show("El servicio se agregó correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                this.Close();
            }
          
        }

        private void btnSueldo_Click(object sender, RoutedEventArgs e)
        {
            windowAgregarSueldo newW = new windowAgregarSueldo();
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {
                DateTime dtp = System.DateTime.Now;
                dtp = newW.dtpFecha.SelectedDate.Value;
                Sueldo i = new Sueldo(newW.txtNombre.Text, newW.cmbTipo.SelectedIndex, float.Parse(newW.txtMonto.Text), newW.txtObservaciones.Text, newW.cmbFormadePAgo.SelectedIndex, 0, newW.txtHoras.Text,dtp);
                i.Save();
                MessageBox.Show("El sueldo se agregó correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                this.Close();
            }
        }

        private void btnOtro_Click(object sender, RoutedEventArgs e)
        {
            windowAgregarOtro newW = new windowAgregarOtro();
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {
                DateTime dtp = System.DateTime.Now;
                dtp = newW.dtpFecha.SelectedDate.Value;
                Otro i = new Otro(newW.txtNombre.Text, newW.cmbTipo.SelectedIndex, float.Parse(newW.txtMonto.Text), newW.txtObservaciones.Text, newW.cmbFormadePAgo.SelectedIndex, 0,dtp);
                i.Save();
                MessageBox.Show("Otro tipo de gasto se agregó correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                this.Close();
            }
        }
    }
}
