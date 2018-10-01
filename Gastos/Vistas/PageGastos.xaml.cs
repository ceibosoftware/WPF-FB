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
        String tipoRb;
        public PageGastos()
        {
            InitializeComponent();
            SeleccionarRadioButton();
            SetearColumnasDGVGastos();
            SetDGVGastos();
            BloquearElementos();
        }

        
        private void Modificar()
        {
            if (rbImpuesto.IsChecked == true)
            {
                var id = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[0].ToString();
                var nombre = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[1].ToString();
                var monto = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[3].ToString();
                var obs = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[4].ToString();
                var formaPago = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[7].ToString();
                var tipo = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[2].ToString();
                var fecha = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[9].ToString();
                var fechav = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[11].ToString();

                windowAgregarImpuesto newW= new windowAgregarImpuesto(nombre,int.Parse(tipo),float.Parse(monto),obs,int.Parse(formaPago),DateTime.Parse(fecha),DateTime.Parse(fechav));
                newW.Title = "Modificar Impuesto";
                newW.ShowDialog();

                if (newW.DialogResult == true)
                {
                    DateTime dtpv = System.DateTime.Now;
                    dtpv = newW.dtpV.SelectedDate.Value;
                    DateTime dtp = System.DateTime.Now;
                    dtp = newW.dtpFecha.SelectedDate.Value;

                    Impuesto imp = new Impuesto(newW.txtNombre.Text,tipo,float.Parse(monto),newW.txtObservaciones.Text, newW.cmbFormadePAgo.SelectedIndex,0,dtp,dtpv );
                    imp.Update(int.Parse(id));
                    MessageBox.Show("Impuesto modificado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (RbServicios.IsChecked == true)
            {
                var id = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[0].ToString();
                var tipo = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[2].ToString();
                var nombre = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[1].ToString();
                var monto = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[3].ToString();
                var unidad = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[5].ToString();
                var obs = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[4].ToString();
                var formaPago = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[7].ToString();
                var fecha = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[9].ToString();
                var proveedor = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[10].ToString();
                var fechav = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[11].ToString();
               
                windowAgregarServicio newW = new windowAgregarServicio(nombre, int.Parse(tipo), float.Parse(monto), obs, int.Parse(formaPago), DateTime.Parse(fecha), DateTime.Parse(fechav),proveedor, unidad);
                newW.Title = "Modificar Servicio";
                newW.ShowDialog();

                if (newW.DialogResult == true)
                {
                    DateTime dtpv = System.DateTime.Now;
                    dtpv = newW.dtpV.SelectedDate.Value;
                    DateTime dtp = System.DateTime.Now;
                    dtp = newW.dtpFecha.SelectedDate.Value;

                    Servicio ser = new Servicio(newW.txtNombre.Text, tipo, float.Parse(monto), newW.txtObservaciones.Text, newW.cmbFormadePAgo.SelectedIndex, 0, newW.txtUnidad.Text,dtp, newW.txtProveedor.Text, dtpv);
                    ser.Update(int.Parse(id));
                    MessageBox.Show("Impuesto modificado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (RbSueldo.IsChecked == true)
            {
                
                var id = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[0].ToString();
                var tipo = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[2].ToString();
                var nombre = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[1].ToString();
                var monto = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[3].ToString();
                var horast = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[6].ToString();
                var obs = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[4].ToString();
                var formaPago = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[7].ToString();
                var fecha = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[9].ToString();
                var viatico = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[12].ToString();
                

                windowAgregarSueldo newW = new windowAgregarSueldo(int.Parse(tipo),nombre, float.Parse(monto), int.Parse(horast), obs, int.Parse(formaPago), DateTime.Parse(fecha),float.Parse(viatico));
                newW.Title = "Modificar Sueldo";
                newW.ShowDialog();

                if (newW.DialogResult == true)
                {
                  
                    DateTime dtp = System.DateTime.Now;
                    dtp = newW.dtpFecha.SelectedDate.Value;

                    Sueldo i = new Sueldo(newW.txtNombre.Text, tipo, float.Parse(newW.txtMonto.Text), newW.txtObservaciones.Text, newW.cmbFormadePAgo.SelectedIndex, 0, newW.txtHoras.Text, dtp, float.Parse(newW.txtViaticos.Text));
                    i.Update(int.Parse(id));
                    MessageBox.Show("Sueldo modificado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                var id = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[0].ToString();
                var tipo = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[2].ToString();
                var nombre = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[1].ToString();
                var monto = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[3].ToString();
                var obs = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[4].ToString();
                var formaPago = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[7].ToString();
                var fecha = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[9].ToString();
                


                windowAgregarOtro newW = new windowAgregarOtro(int.Parse(tipo), float.Parse(monto), nombre,  int.Parse(formaPago), obs, DateTime.Parse(fecha));
                newW.Title = "Modificar otro";
                newW.ShowDialog();

                if (newW.DialogResult == true)
                {

                    DateTime dtp = System.DateTime.Now;
                    dtp = newW.dtpFecha.SelectedDate.Value;
                    Otro i = new Otro(newW.txtNombre.Text, tipo, float.Parse(newW.txtMonto.Text), newW.txtObservaciones.Text, newW.cmbFormadePAgo.SelectedIndex, 0, dtp);
                    i.Update(int.Parse(id));
                    MessageBox.Show("Otro modificado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void BloquearElementos()
        {
            txtfecha.IsReadOnly = true;
            txtFormaDePago.IsReadOnly = true;
            txtHoras.IsReadOnly = true;
            txtObservaciones.IsReadOnly = true;
            txtProveedor.IsReadOnly = true;
            txtfechaVencimiento.IsReadOnly = true;
            txtViat.IsReadOnly = true;
        }
        private void ParaMensajeEliminar()
        {
            if (rbImpuesto.IsChecked == true)
            {
                tipoRb = "Impuesto";
            }
            else if (RbServicios.IsChecked == true)
            {
                tipoRb = "Servicio";
            }
            else if (RbSueldo.IsChecked == true)
            {
                tipoRb = "Sueldo";
            }
            else
            {
                tipoRb = "Otro";
            }
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
                try
                {

                var nombre = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[1].ToString();
                var monto = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[3].ToString();
                var obs = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[4].ToString();
                var formaPago = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[7].ToString();
                var tipo = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[2].ToString();
                var fecha = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[9].ToString();
                var fechav = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[11].ToString();
                SetFormaPago(formaPago);
                setTipo(tipo);
                txtObservaciones.Text = obs.ToString();
                txtfecha.Text = fecha.ToString();
                txtfechaVencimiento.Text = fechav.ToString();

                }
                catch (NullReferenceException)
                {

                    MessageBox.Show("Seleccione un impuesto", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else if (RbServicios.IsChecked == true)
            {
                try
                {

         
                var tipo = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[2].ToString();
                var nombre = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[1].ToString();
                var monto = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[3].ToString();
                var unidad = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[5].ToString();
                var obs = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[4].ToString();
                var formaPago = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[7].ToString();
                var fecha = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[9].ToString();
                var proveedor = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[10].ToString();
                var fechav = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[11].ToString();
                txtfecha.Text = fecha.ToString();
                txtfechaVencimiento.Text = fechav.ToString();
                txtProveedor.Text = proveedor.ToString();
                SetFormaPago(formaPago);
                txtObservaciones.Text = obs.ToString();
                txtUnidad.Text = unidad.ToString();
                setTipo(tipo);
                }
                catch (NullReferenceException)
                {

                    MessageBox.Show("Seleccione un servicio", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else if (RbSueldo.IsChecked == true)
            {
                try
                {

             
                var tipo = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[2].ToString();
                var nombre = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[1].ToString();
                var monto = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[3].ToString();
                var horas = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[6].ToString();
                var obs = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[4].ToString();
                var formaPago = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[7].ToString();
                var fecha = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[9].ToString();
                var viatico = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[12].ToString();
                txtViat.Text = viatico.ToString();
                txtfecha.Text = fecha.ToString();
                SetFormaPago(formaPago);
                txtObservaciones.Text = obs.ToString();
                txtHoras.Text = horas.ToString();
                setTipo(tipo);
                }
                catch (NullReferenceException)
                {

                    MessageBox.Show("Seleccione un sueldo", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                try
                {


                    var tipo = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[2].ToString();
                    var nombre = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[1].ToString();
                    var monto = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[3].ToString();
                    var obs = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[4].ToString();
                    var formaPago = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[7].ToString();
                    var fecha = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[9].ToString();
                    txtfecha.Text = fecha.ToString();
                    SetFormaPago(formaPago);
                    txtObservaciones.Text = obs.ToString();
                    setTipo(tipo);
                 }
                 catch (NullReferenceException)
                 {
                    MessageBox.Show("Seleccione un elemento", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                 }
            }
        }
        private void cambiarRb()
        {
            if (rbImpuesto.IsChecked == true)
            {
                rbImpuesto.IsChecked = false;
                RbSueldo.IsChecked = true;
                rbImpuesto.IsChecked = true;
            }
            else if (RbServicios.IsChecked == true)
            {
                RbServicios.IsChecked = true;
                RbSueldo.IsChecked = true;
                RbServicios.IsChecked = true;
            }
            else if (RbSueldo.IsChecked == true)
            {
                RbSueldo.IsChecked = false;
                RbServicios.IsChecked = true;
                RbSueldo.IsChecked = true;
            }
            else
            {
                rbOtros.IsChecked = false;
                RbSueldo.IsChecked = true;
                rbOtros.IsChecked = true;
            }
        }
        private void setTipo(String t)
        {
            if (t == "0")
            {
                txtTipo.Text = "Ingresos Brutos";
            }
            else if (t == "1")
            {
                txtTipo.Text = "IVA";
            }
            else if (t == "2")
            {
                txtTipo.Text = "Otro";
            }
            else if (t == "7")
            {
                txtTipo.Text = "Directo";
            }
            else if (t == "8")
            {
                txtTipo.Text = "Indirecto";
            }
            else if (t == "4")
            {
                txtTipo.Text = "Jornal";
            }
            else if (t == "5")
            {
                txtTipo.Text = "Mensual";
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

                lblProveedor.Visibility = Visibility.Collapsed;
                txtProveedor.Visibility = Visibility.Collapsed;

                lblFechaven.Visibility = Visibility.Visible;
                txtfechaVencimiento.Visibility = Visibility.Visible;

                lblviat.Visibility = Visibility.Collapsed;
                txtViat.Visibility = Visibility.Collapsed;
            }
            else if (RbServicios.IsChecked == true)
            {

                lblHorasT.Visibility = Visibility.Collapsed;
                txtHoras.Visibility = Visibility.Collapsed;

                lblunidad.Visibility = Visibility.Visible;
                txtUnidad.Visibility = Visibility.Visible;

                lblProveedor.Visibility = Visibility.Visible;
                txtProveedor.Visibility = Visibility.Visible;

                lblFechaven.Visibility = Visibility.Visible;
                txtfechaVencimiento.Visibility = Visibility.Visible;


                lblviat.Visibility = Visibility.Collapsed;
                txtViat.Visibility = Visibility.Collapsed;
            }
            else if (RbSueldo.IsChecked == true)
            {

                lblHorasT.Visibility = Visibility.Visible;
                txtHoras.Visibility = Visibility.Visible;

                lblunidad.Visibility = Visibility.Collapsed;
                txtUnidad.Visibility = Visibility.Collapsed;

                lblProveedor.Visibility = Visibility.Collapsed;
                txtProveedor.Visibility = Visibility.Collapsed;

                lblFechaven.Visibility = Visibility.Collapsed;
                txtfechaVencimiento.Visibility = Visibility.Collapsed;


                lblviat.Visibility = Visibility.Visible;
                txtViat.Visibility = Visibility.Visible;
            }
            else
            {
                lblHorasT.Visibility = Visibility.Collapsed;
                txtHoras.Visibility = Visibility.Collapsed;

                lblunidad.Visibility = Visibility.Collapsed;
                txtUnidad.Visibility = Visibility.Collapsed;

                lblProveedor.Visibility = Visibility.Collapsed;
                txtProveedor.Visibility = Visibility.Collapsed;

                lblFechaven.Visibility = Visibility.Collapsed;
                txtfechaVencimiento.Visibility = Visibility.Collapsed;

                lblviat.Visibility = Visibility.Collapsed;
                txtViat.Visibility = Visibility.Collapsed;
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
            Modificar();
            CambiarRB();
        }
        

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            ParaMensajeEliminar();
            MessageBoxResult dialog = MessageBox.Show("Esta seguro que desea eliminar el " + tipoRb + "?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);


            if (dialog == MessageBoxResult.Yes)
            {
                var id = (dgvGastos.SelectedItem as System.Data.DataRowView).Row[0].ToString();
                Impuesto i = new Impuesto();
                i.Delete(id);
                MessageBox.Show("" + tipoRb + " Eliminado correctamente", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Information);
                cambiarRb();
            }

         
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
