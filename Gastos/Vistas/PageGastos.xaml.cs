using System;
using System.Collections.Generic;
using System.Data;
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
using wpfFamiliaBlanco.Gastos.Clases;

namespace wpfFamiliaBlanco.Gastos.Vistas
{
    /// <summary>
    /// Interaction logic for PageGastos.xaml
    /// </summary>
    public partial class PageGastos : Page
    {
        public List<Gasto> listagastos = new List<Gasto>();
        CRUD conexion = new CRUD();
        public PageGastos()
        {
            InitializeComponent();
            SeleccionarRadioButton();
            SetearColumnasDGVGastos();
            SetDGVGastos();
            BloquearElementos();
        }

        private void BloquearElementos()
        {
            txtfecha.IsReadOnly = true;
            txtFormaDePago.IsReadOnly = true;
            txtHoras.IsReadOnly = true;
            txtObservaciones.IsReadOnly = true;
            txtUnidad.IsReadOnly = true;
        }

        private void SetFormaPago(String var)
        {
            if (var == "0")
            {
                txtFormaDePago.Text = "Efectivo";
            }
            else if (var == "1")
            {
                txtFormaDePago.Text = "Cheque";
            }
            else
            {
                txtFormaDePago.Text = "Transferencia";
            }
        }
        private void SeleccionarDatos()
        {
            if (rbImpuesto.IsChecked == true)
            {
                var nombre = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[1].ToString();
                var monto = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[3].ToString();
                var obs = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[4].ToString();
                var formaPago = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[7].ToString();
                
                SetFormaPago(formaPago);
                txtObservaciones.Text = obs.ToString();
                var fecha = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[9].ToString();
                txtfecha.Text = fecha.ToString();
            }
            else if (RbServicios.IsChecked == true)
            {
                var nombre = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[1].ToString();
                var monto = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[3].ToString();
                var unidad = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[5].ToString();
                var obs = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[4].ToString();
                var formaPago = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[7].ToString();
                var fecha = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[9].ToString();
                txtfecha.Text = fecha.ToString();
                SetFormaPago(formaPago);
                txtObservaciones.Text = obs.ToString();
                txtUnidad.Text = unidad.ToString();

            }
            else if (RbSueldo.IsChecked == true)
            {
                var nombre = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[1].ToString();
                var monto = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[3].ToString();
                var horas = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[6].ToString();
                var obs = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[4].ToString();
                var formaPago = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[7].ToString();
                var fecha = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[9].ToString();
                txtfecha.Text = fecha.ToString();
                SetFormaPago(formaPago);
                txtObservaciones.Text = obs.ToString();
                txtHoras.Text = horas.ToString();

            }
            else
            {
                var nombre = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[1].ToString();
                var monto = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[3].ToString();
                var obs = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[4].ToString();
                var formaPago = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[7].ToString();
                var fecha = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[9].ToString();
                txtfecha.Text = fecha.ToString();
                SetFormaPago(formaPago);
                txtObservaciones.Text = obs.ToString();
            
            }
        }
        private void OcultarElementos()
        {
            if (rbImpuesto.IsChecked == true)
            {

                lblHorasT.Visibility = Visibility.Collapsed;
                txtHoras.Visibility = Visibility.Collapsed;

                lblunidad.Visibility = Visibility.Collapsed;
                txtUnidad.Visibility = Visibility.Collapsed;
                

            }
            else if (RbServicios.IsChecked == true)
            {

                lblHorasT.Visibility = Visibility.Collapsed;
                txtHoras.Visibility = Visibility.Collapsed;

                lblunidad.Visibility = Visibility.Visible;
                txtUnidad.Visibility = Visibility.Visible;
            }
            else if (RbSueldo.IsChecked == true)
            {

                lblHorasT.Visibility = Visibility.Visible;
                txtHoras.Visibility = Visibility.Visible;

                lblunidad.Visibility = Visibility.Collapsed;
                txtUnidad.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblHorasT.Visibility = Visibility.Collapsed;
                txtHoras.Visibility = Visibility.Collapsed;

                lblunidad.Visibility = Visibility.Collapsed;
                txtUnidad.Visibility = Visibility.Collapsed;
            }
        }
    

 
        private void SeleccionarRadioButton()
        {
            rbImpuesto.IsChecked = true;
            rbOtros.IsChecked = false;
            RbSueldo.IsChecked = false;
            RbServicios.IsChecked = false;
        }

        private void CambiarRB()
        {
            rbOtros.IsChecked = true;
            rbOtros.IsChecked = false;
            rbImpuesto.IsChecked = true;
        }
        private void SetearColumnasDGVGastos()
        {

            dgvGastos.AutoGenerateColumns = false;
            dgvGastos.IsReadOnly = true;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvGastos.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Monto";
            textColumn2.Binding = new Binding("monto");
            dgvGastos.Columns.Add(textColumn2);
            DataGridTextColumn textColumn3 = new DataGridTextColumn();
            textColumn3.Header = "Observaciones";
            textColumn3.Binding = new Binding("observaciones");
           // dgvGastos.Columns.Add(textColumn3);

        }


        private void SetDGVGastos()
        {
            if (rbImpuesto.IsChecked == true)
            {
                dgvGastos.ItemsSource = Gasto.GetGastos(0).AsDataView();
                
                
            }
            else if (RbServicios.IsChecked == true)
            {
                dgvGastos.ItemsSource = Gasto.GetGastos(1).AsDataView();
               
            }
            else if (RbSueldo.IsChecked == true)
            {
                dgvGastos.ItemsSource = Gasto.GetGastos(2).AsDataView();
                
            }
            else if(rbOtros.IsChecked == true)
            {
                dgvGastos.ItemsSource = Gasto.GetGastos(3).AsDataView();
               
            }
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            windowTipoGasto newW = new windowTipoGasto();
            newW.ShowDialog();
            if (newW.DialogResult == true)
            {
                CambiarRB();
            }
        }

        private void btnModificar_Copy_Click(object sender, RoutedEventArgs e)
        {
            //for (int i = 0; i < dgvGastos.Items.Count - 1; i++)
            //{

            //    var telefono = (dgvGastos.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
            //    j++;

            //    var email = (dgvGastos.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
            //    j++;
            //    var nombre2 = (dgvGastos.Items[i] as System.Data.DataRowView).Row.ItemArray[j].ToString();
            //    j++;

            //    j = 0;

            //    Impuesto imp = new Impuesto(nombre2, email, telefono);
            //    listagastos.Add(conA);


            //}
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbImpuesto_Checked(object sender, RoutedEventArgs e)
        {
            SetDGVGastos();
            OcultarElementos();
        }

        private void RbServicios_Checked(object sender, RoutedEventArgs e)
        {
            SetDGVGastos();
            OcultarElementos();
        }

        private void RbSueldo_Checked(object sender, RoutedEventArgs e)
        {
            SetDGVGastos();
            OcultarElementos();
        }

        private void rbOtros_Checked(object sender, RoutedEventArgs e)
        {
            SetDGVGastos();
            OcultarElementos();
        }

        private void dgvGastos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
        }

        private void dgvGastos_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OcultarElementos();
            SeleccionarDatos();
        }


    }
}
