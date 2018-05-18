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

namespace wpfFamiliaBlanco.Entradas
{
    /// <summary>
    /// Lógica de interacción para WindowAgregarPagoProveedorCtaBancaria.xaml
    /// </summary>
    public partial class WindowAgregarPagoProveedorCtaBancaria : Window
    {

        public long cbuu;
        public String nombreT;
        public float toc;
        public int tipo ;
        public WindowAgregarPagoProveedorCtaBancaria(float totalc, int tipo4)
        {
            InitializeComponent();
            toc = totalc;
            txtMonto.Text = toc.ToString();
            txtMonto.IsEnabled = false;
            txtNombreTitular.MaxLines = 1;
            txtNombreTitular.MaxLength = 25;
            txtCbu.MaxLines = 1;
            txtCbu.MaxLength = 22;
            tipo = tipo4;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (txtCbu.Text == "")
            {
                MessageBox.Show("Ingrese CBU");
            }
            if (txtNombreTitular.Text=="")
            {
                MessageBox.Show("Ingrese Nombre del titular");
            }
            else if (txtCbu.Text != "" && txtNombreTitular.Text != "")
            {
                cbuu = long.Parse(txtCbu.Text);
                nombreT = txtNombreTitular.Text;
                DialogResult = true;
            }
          
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtCbu_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}
