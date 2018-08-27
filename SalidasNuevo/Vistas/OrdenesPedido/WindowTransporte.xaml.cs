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
using wpfFamiliaBlanco.SalidasNuevo.Clases.OrdenesPedido;

namespace wpfFamiliaBlanco.SalidasNuevo.Vistas.OrdenesPedido
{
    /// <summary>
    /// Interaction logic for WindowTransporte.xaml
    /// </summary>
    public partial class WindowTransporte : Window
    {
        Cliente cliente;
        public WindowTransporte(Cliente cliente)
        {
            InitializeComponent();
            this.Cliente = cliente;
            loadGerneral();
        }

        internal Cliente Cliente { get => cliente; set => cliente = value; }

        private void loadGerneral() {
            loadTXT();
        }
        private void loadTXT()
        {
            txtDireccionEntrega.Text = Cliente.DireccionEntrega;
            txtTelefonoTransporte.Text = cliente.TelefonoTransporte;
            txtTransporte.Text = cliente.Transporte;
        }
        private bool validar() { return true; }

        private void setDatos()
        {
            cliente.Transporte = txtTransporte.Text;
            cliente.TelefonoTransporte = txtTelefonoTransporte.Text;
            cliente.DireccionEntrega = txtDireccionEntrega.Text;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (validar())
            {
                setDatos();
                DialogResult = true;
            }
        }
    }
}
