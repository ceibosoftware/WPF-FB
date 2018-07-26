using System;
using System.Data;
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
using wpfFamiliaBlanco.SalidasNuevo.Clases.INV;

namespace wpfFamiliaBlanco.SalidasNuevo.Vistas.INV
{
    /// <summary>
    /// Lógica de interacción para windowAgregarINV.xaml
    /// </summary>
    public partial class windowAgregarINV : Window
    {
        Analisis inv;
        String tipo;
        

        internal Analisis Inv { get => inv; set => inv = value; }

        public windowAgregarINV()
        {
            InitializeComponent();
           
            llenarcmbtipo();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            
            if (validacion())
            {
                llenarDatos();
                DialogResult = true;
                
            }
        }

        public Boolean validacion()
        {

            if (cmbtipo.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el tipo de analisis");
                return false;
            }
            else if (String.IsNullOrEmpty(txtalcohol.Text))
            {
                MessageBox.Show("Complete el contenido alcoholico");
                return false;
            }
            else if (String.IsNullOrEmpty(txtdensidad.Text)){
                MessageBox.Show("Complete la densidad del vino");
                return false;
            } else if (String.IsNullOrEmpty(txtlitros.Text))
            {
                MessageBox.Show("Complete los litros del analisis");
                return false;

            } else if (String.IsNullOrEmpty(txtnumero.Text))
            {
                MessageBox.Show("Complete el numero de analisis");
                return false;
            } else if (String.IsNullOrEmpty(dpFecha.SelectedDate.ToString()))
            {
                MessageBox.Show("Complete la fecha de analisis");
                return false;
            }
            return true;
        }

        public void llenarDatos()
        {
            
            int tipo = cmbtipo.SelectedIndex;
            float densidad = float.Parse(txtdensidad.Text);
            float alcohol = float.Parse(txtalcohol.Text);
            String numeroAnalisis = txtnumero.Text;
            String nombrevino = txtnombrevino.Text;
            float litros = float.Parse(txtlitros.Text);
            DateTime fecha = DateTime.Today;
            
            inv = new Analisis(alcohol, densidad, litros, numeroAnalisis, tipo, fecha, nombrevino);


        }

        public void llenarcmbtipo()
        {
            cmbtipo.Items.Add("Libre Circulacion");
            cmbtipo.Items.Add("Aptitud de exportacion");
        }

        private void txtdensidad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
        private void txtalcohol_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
        private void txtnumero_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
        private void txtlitros_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

       
    }
}
