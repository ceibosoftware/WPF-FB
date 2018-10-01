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

        private String SetTipo(String t)
        {
            String tipo="";
            if (t == "Directo")
            {
                tipo = "7";
            }
            else if (t == "Indirecto")
            {
                tipo = "8";
            }
            else if (t == "Ingresos Brutos")
            {
                tipo = "0";
            }
            else if (t == "IVA")
            {
                tipo = "1";
            }
            else if (t == "Otro")
            {
                tipo = "2";
            }
            else if (t == "Jornal")
            {
                tipo = "4";
            }
            else if (t == "Mensual")
            {
                tipo = "5";
            }
            return tipo;
        }
        private void btnImpuesto_Click(object sender, RoutedEventArgs e)
        {
            windowAgregarImpuesto newW = new windowAgregarImpuesto();
            
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {
                DateTime dtpv = System.DateTime.Now;
                dtpv = newW.dtpV.SelectedDate.Value;
                String tipo =SetTipo(newW.cmbTipo.SelectedValue.ToString());
                DateTime dtp = System.DateTime.Now;
                dtp = newW.dtpFecha.SelectedDate.Value;
                Impuesto i = new Impuesto(newW.txtNombre.Text, tipo, float.Parse(newW.txtMonto.Text), newW.txtObservaciones.Text, newW.cmbFormadePAgo.SelectedIndex, 0, dtp,dtpv);
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

                String tipo = SetTipo(newW.cmbTipo.SelectedValue.ToString());
                DateTime dtp = System.DateTime.Now;
                dtp = newW.dtpFecha.SelectedDate.Value;
                DateTime dtpv = System.DateTime.Now;
                dtpv = newW.dtpV.SelectedDate.Value;
                Servicio i = new Servicio(newW.txtNombre.Text, tipo, float.Parse(newW.txtMonto.Text), newW.txtObservaciones.Text, newW.cmbFormadePAgo.SelectedIndex, 0, newW.txtUnidad.Text,dtp, newW.txtProveedor.Text,dtpv);
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
                String tipo = SetTipo(newW.cmbTipo.SelectedValue.ToString());
                DateTime dtp = System.DateTime.Now;
                dtp = newW.dtpFecha.SelectedDate.Value;
                Sueldo i = new Sueldo(newW.txtNombre.Text, tipo, float.Parse(newW.txtMonto.Text), newW.txtObservaciones.Text, newW.cmbFormadePAgo.SelectedIndex, 0, newW.txtHoras.Text,dtp,float.Parse( newW.txtViaticos.Text));
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
                String tipo = SetTipo(newW.cmbTipo.SelectedValue.ToString());
                DateTime dtp = System.DateTime.Now;
                dtp = newW.dtpFecha.SelectedDate.Value;
                Otro i = new Otro(newW.txtNombre.Text, tipo, float.Parse(newW.txtMonto.Text), newW.txtObservaciones.Text, newW.cmbFormadePAgo.SelectedIndex, 0,dtp);
                i.Save();
                MessageBox.Show("Otro tipo de gasto se agregó correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                this.Close();
            }
        }
    }
}
