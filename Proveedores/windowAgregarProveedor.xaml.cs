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
using System.Windows.Shapes;
using wpfFamiliaBlanco.Proveedores;

namespace wpfFamiliaBlanco.Proveedores
{
    /// <summary>
    /// Interaction logic for windowAgregarProveedor.xaml
    /// </summary>
    public partial class windowAgregarProveedor : Window
    {
  
        public class contacto
        {

            public  String NombreContacto { get; set; }
            public  String Email { get; set; }
            public  String NumeroTelefono { get; set; }
           

            public contacto (String nomContacto, String ema, String numTelefono)
            {
                NombreContacto = nomContacto;
                Email = ema;
                NumeroTelefono = numTelefono;

            }
        }


        CRUD conexion = new CRUD();
        public static String sqlContacto;
      public static  List<contacto> lista = new List<contacto>();

        public windowAgregarProveedor()
        {
            InitializeComponent();
            LlenarComboFiltro();
            EliminarDGVContacto();


        }

      

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {

            if (Valida())
            {
                
                DialogResult = true;
            }
     
        }

        private void LlenarComboFiltro()
        {

            cmbRazonSocial.Items.Add("Responsable Inscripto");
            cmbRazonSocial.Items.Add("SA");
            cmbRazonSocial.Items.Add("SRL");
        }

        public Boolean Valida()
        {
            if (txtCuit.Text != "" && txtDireccion.Text != "" && txtNombre.Text != "" && txtCP.Text != "" && txtLocalidad.Text != "")
            {
                return true;
            }
            else
            {
                MessageBox.Show("Faltan campos por completar");
                return false;
            }
           
        }

        private void btnNuevoContacto_Click(object sender, RoutedEventArgs e)
        {
            
                var newW = new windowAgregarContactoProveedor();
                newW.ShowDialog();
            
        }

        private void btnNuevoContacto_Click_1(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarContactoProveedor();
            newW.ShowDialog();

            if (newW.DialogResult == true)
            {
                Console.WriteLine("Entro");
                String telefono1 = newW.txtTelefonoContacto.Text;
                String nombreContacto1 = newW.txtNombreContacto.Text;
                String mail1 = newW.txtMailContacto.Text;
                contacto info =  new contacto(nombreContacto1, mail1, telefono1);
                
                
                lista.Add(info);
                LoadDGVContacto();
               
               

               // conexion.operaciones(sql);
               
                
            }

           
        }
        public void LoadDGVContacto()
        {
            this.dgv.ItemsSource = lista;
            dgv.Items.Refresh();
          
        }
        public void EliminarDGVContacto()
        {
            this.dgv.Items.Remove(lista);
            dgv.Items.Refresh();

        }

        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
        
        }
    }
}
