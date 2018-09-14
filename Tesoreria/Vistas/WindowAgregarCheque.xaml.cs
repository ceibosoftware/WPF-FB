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
using wpfFamiliaBlanco.Tesoreria.Clases;

namespace wpfFamiliaBlanco.Tesoreria.Vistas
{
    /// <summary>
    /// Lógica de interacción para WindowAgregarCheque.xaml
    /// </summary>
    public partial class WindowAgregarCheque : Window
    {
        Cheque cheq;
        internal Cheque Cheq { get => cheq; set => cheq = value; }

        int tipo;

        public WindowAgregarCheque()
        {
            InitializeComponent();
          
        }

        public WindowAgregarCheque(int tipo,int numero)
        {
            this.tipo = tipo;
            
            InitializeComponent();
            collapsedproduct(tipo);
            loadcmbfinaldidad();
            loadcmbcuenta();
            

        }
       

           
        
        private void loadcmbfinaldidad()
        {
            cmbtipo.Items.Add("Cruzado");
            cmbtipo.Items.Add("Al portador");

        }

        private void loadcmbcuenta()
        {
            cmbcta.Items.Add("Cuenta 1");
            cmbcta.Items.Add("Cuenta 2");
        }
        public Boolean validacionpropio()
        {
            
            if (cmbtipo.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione la finalidad del cheque");
                return false;
            }
            else if (cmbcta.SelectedIndex==-1)
            {
                MessageBox.Show("Complete la cuenta bancaria ");
                return false;
            }
            else if (String.IsNullOrEmpty(txtimporte.Text))
            {
                MessageBox.Show("Complete el importe");
                return false;
            }
            else if (String.IsNullOrEmpty(txtnumerocheque.Text))
            {
                MessageBox.Show("Complete el numero de cheque");
                return false;

            }
            else if (String.IsNullOrEmpty(dtpcobro.SelectedDate.ToString()))
            {
                MessageBox.Show("Complete la fecha de cobro");
                return false;
            }
            else if (String.IsNullOrEmpty(dtpconfeccion.SelectedDate.ToString()))
            {
                MessageBox.Show("Complete la fecha de confeccion");
                return false;
            }
            else if (String.IsNullOrEmpty(txttitular.Text))
            {
                MessageBox.Show("complete el titular de la cuenta");
                return false;
            }
            
            return true;
        }
        public Boolean validaciontercero()
        {

            if (cmbtipo.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione la finalidad del cheque");
                return false;
            }
            else if (String.IsNullOrEmpty(txtbco.Text))
            {
                MessageBox.Show("Complete el banco");
                return false;
            }
            else if (String.IsNullOrEmpty(txtimporte.Text))
            {
                MessageBox.Show("Complete el importe");
                return false;
            }
            else if (String.IsNullOrEmpty(txtnumerocheque.Text))
            {
                MessageBox.Show("Complete el numero de cheque");
                return false;

            }
            else if (String.IsNullOrEmpty(dtpcobro.SelectedDate.ToString()))
            {
                MessageBox.Show("Complete la fecha de cobro");
                return false;
            }
            else if (String.IsNullOrEmpty(dtpconfeccion.SelectedDate.ToString()))
            {
                MessageBox.Show("Complete la fecha de confeccion");
                return false;
            }
            else if (cmbcliente.SelectedIndex==-1)
            {
                MessageBox.Show("Seleccione que cliente entrego el cheque");
                return false;
            }
           
            else if (String.IsNullOrEmpty(txtportador.Text))
            {
                MessageBox.Show("Complete con nombre de quien paga el cheque");
                return false;
            }


            return true;
        }
        private void collapsedproduct(int tipo)
        {
            if (tipo == 0)
            {
                lblorigen.Visibility = Visibility.Collapsed;
                txtbco.Visibility = Visibility.Collapsed;
                lblbco.Visibility = Visibility.Collapsed;
                lblentrego.Visibility = Visibility.Collapsed;
                cmbcliente.Visibility = Visibility.Collapsed;

            }
            else
            {
                cmbcta.Visibility = Visibility.Collapsed;
                lblcta.Visibility = Visibility.Collapsed;
                lbltitular.Visibility = Visibility.Collapsed;
                txttitular.Visibility = Visibility.Collapsed;
            }
        }


        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {

            if (tipo == 0)
            {
                if (validacionpropio())
                {
                    DialogResult = true;
                }
                

            }
            else 
            {
                if (validaciontercero()){ 
                DialogResult = true;
                }

            }
        }

        
        private void cmbProveedores_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
       
    }
}
