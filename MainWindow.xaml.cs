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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using wpfFamiliaBlanco.Clientes;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CRUD conexion = new CRUD();
        pageProductos productos = new pageProductos();

        public MainWindow()
        {
            InitializeComponent();
            //loadTimer();
            notificaciones();
            btnSalidasMI.Visibility = Visibility.Collapsed;
            btnSalidasME.Visibility = Visibility.Collapsed;
            btnClientesMI.Visibility = Visibility.Collapsed;
            btnClientesME.Visibility = Visibility.Collapsed;
        }

        public void notificaciones()
        {
            String valor = "SELECT COUNT(*) FROM cuotas WHERE estado = '" + 0 + "'";
            String CantidadPagos = "";

            CantidadPagos = conexion.ValorEnVariable(valor).ToString();

            if (CantidadPagos == "")
            {
                lblnotificaciones.Visibility = Visibility.Collapsed;
            }
            else if (CantidadPagos != "")
            {
                lblnotificaciones.Visibility = Visibility.Visible;
                lblnotificaciones.Content = "";
                lblnotificaciones.Content = CantidadPagos;

                ToolTip tt = new ToolTip();
                tt.FontSize = 15;

                tt.Content = CantidadPagos + " pagos próximos a vencer.";
                lblnotificaciones.ToolTip = tt;
            }
        }
                private void btnUsuarios_Click(object sender, RoutedEventArgs e)
        {

            frameInicio.Content = new pageUsuarios();  //frame inicio es el cuadro donde se actualizan las pantallas en la ventana
            //principal del programa
            btnUsuarios.Style = FindResource("BotonStylePanel") as Style;
            btnProductos.Style = FindResource("botonDock") as Style;
            btnProveedores.Style = FindResource("botonDock") as Style;
            btnEntradas.Style = FindResource("botonDock") as Style;
            btnSalidas.Style = FindResource("botonDock") as Style;
            //btnCategorias.Style = FindResource("botonDock") as Style;
            btnClientes.Style = FindResource("botonDock") as Style;
            btnSalidasMI.Visibility = Visibility.Collapsed;
            btnSalidasME.Visibility = Visibility.Collapsed;

            #region Fade in
            //animaciones
            Storyboard storyboard = new Storyboard();
            TimeSpan duration = new TimeSpan(0, 0, 1);

            // fade in
            DoubleAnimation animation = new DoubleAnimation();

            animation.From = 0.0;
            animation.To = 1.0;
            animation.Duration = new Duration(duration);
            // opacity
            Storyboard.SetTargetName(animation, frameInicio.Name);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Control.OpacityProperty));
            // + mainwindow a storyboard
            storyboard.Children.Add(animation);

            //start storyboard
            storyboard.Begin(this);

            #endregion
        }

        private void cargar_pageInicio(object sender, EventArgs e)
        {
            frameInicio.Content = new pageInicio(); // en este caso se carga la imagen bodega fblanco al inicializarse el programa
            btnSalidasMI.Visibility = Visibility.Collapsed;
            btnSalidasME.Visibility = Visibility.Collapsed;
        }

        private void btnProductos_Click(object sender, RoutedEventArgs e)
        {
            frameInicio.Content = new Productos.productosPestañas();
            btnUsuarios.Style = FindResource("botonDock") as Style;
            btnProductos.Style = FindResource("BotonStylePanel") as Style;
            btnProveedores.Style = FindResource("botonDock") as Style;
            btnEntradas.Style = FindResource("botonDock") as Style;
            btnSalidas.Style = FindResource("botonDock") as Style;
           // btnCategorias.Style = FindResource("botonDock") as Style;
            btnClientes.Style = FindResource("botonDock") as Style;
            nube.Visibility = Visibility.Visible;
            lblnotificaciones.Visibility = Visibility.Visible;
            btnSalidasMI.Visibility = Visibility.Collapsed;
            btnSalidasME.Visibility = Visibility.Collapsed;
            btnClientesMI.Visibility = Visibility.Collapsed;
            btnClientesME.Visibility = Visibility.Collapsed;

            #region Fade in
            //animaciones
            Storyboard storyboard = new Storyboard();
            TimeSpan duration = new TimeSpan(0, 0, 1);

            // fade in
            DoubleAnimation animation = new DoubleAnimation();

            animation.From = 0.0;
            animation.To = 1.0;
            animation.Duration = new Duration(duration);
            // opacity
            Storyboard.SetTargetName(animation, frameInicio.Name);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Control.OpacityProperty));
            // + mainwindow a storyboard
            storyboard.Children.Add(animation);

            //start storyboard
            storyboard.Begin(this);

            #endregion
        }

        private void btnProveedores_Click(object sender, RoutedEventArgs e)
        {
            frameInicio.Content = new pageProveedores();
            btnUsuarios.Style = FindResource("botonDock") as Style;
            btnProductos.Style = FindResource("botonDock") as Style;
            btnProveedores.Style = FindResource("BotonStylePanel") as Style;
            btnEntradas.Style = FindResource("botonDock") as Style;
            btnSalidas.Style = FindResource("botonDock") as Style;
            //btnCategorias.Style = FindResource("botonDock") as Style;
            btnClientes.Style = FindResource("botonDock") as Style;
            nube.Visibility = Visibility.Visible;
            lblnotificaciones.Visibility = Visibility.Visible;
            btnSalidasMI.Visibility = Visibility.Collapsed;
            btnSalidasME.Visibility = Visibility.Collapsed;
            btnClientesMI.Visibility = Visibility.Collapsed;
            btnClientesME.Visibility = Visibility.Collapsed;


            #region Fade in
            //animaciones
            Storyboard storyboard = new Storyboard();
            TimeSpan duration = new TimeSpan(0, 0, 1);

            // fade in
            DoubleAnimation animation = new DoubleAnimation();

            animation.From = 0.0;
            animation.To = 1.0;
            animation.Duration = new Duration(duration);
            // opacity
            Storyboard.SetTargetName(animation, frameInicio.Name);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Control.OpacityProperty));
            // + mainwindow a storyboard
            storyboard.Children.Add(animation);

            //start storyboard
            storyboard.Begin(this);

            #endregion
        }

        private void btnEntradas_Click(object sender, RoutedEventArgs e)
        {
            frameInicio.Content = new pageEntradas();
            btnUsuarios.Style = FindResource("botonDock") as Style;
            btnProductos.Style = FindResource("botonDock") as Style;
            btnProveedores.Style = FindResource("botonDock") as Style;
            btnEntradas.Style = FindResource("BotonStylePanel") as Style;
            btnSalidas.Style = FindResource("botonDock") as Style;
//btnCategorias.Style = FindResource("botonDock") as Style;
            btnClientes.Style = FindResource("botonDock") as Style;
            nube.Visibility = Visibility.Collapsed;
            lblnotificaciones.Visibility = Visibility.Collapsed;
            btnSalidasMI.Visibility = Visibility.Collapsed;
            btnSalidasME.Visibility = Visibility.Collapsed;
            btnClientesMI.Visibility = Visibility.Collapsed;
            btnClientesME.Visibility = Visibility.Collapsed;

        }

        private void btnSalidas_Click(object sender, RoutedEventArgs e)
        {
            frameInicio.Content = new pageSalidas();


            btnUsuarios.Style = FindResource("botonDock") as Style;
            btnProductos.Style = FindResource("botonDock") as Style;
            btnProveedores.Style = FindResource("botonDock") as Style;
            btnEntradas.Style = FindResource("botonDock") as Style;
            btnSalidas.Style = FindResource("BotonStylePanel") as Style;
           // btnCategorias.Style = FindResource("botonDock") as Style;
            btnClientes.Style = FindResource("botonDock") as Style;
            nube.Visibility = Visibility.Visible;
            lblnotificaciones.Visibility = Visibility.Visible;
            btnSalidasMI.Visibility = Visibility.Visible;
            btnSalidasME.Visibility = Visibility.Visible;
            btnSalidasMI.Style = FindResource("buttonSwitchStylePressed") as Style;
            btnSalidasME.Style = FindResource("ButtonStyleSwitchNotSelected") as Style;
            btnClientesMI.Visibility = Visibility.Collapsed;
            btnClientesME.Visibility = Visibility.Collapsed;
        }

        private void btnCategorias_Click(object sender, RoutedEventArgs e)
        {
            frameInicio.Content = new pageCategorias();
            btnUsuarios.Style = FindResource("botonDock") as Style;
            btnProductos.Style = FindResource("botonDock") as Style;
            btnProveedores.Style = FindResource("botonDock") as Style;
            btnEntradas.Style = FindResource("botonDock") as Style;
            btnSalidas.Style = FindResource("botonDock") as Style;
            //btnCategorias.Style = FindResource("BotonStylePanel") as Style;
            btnClientes.Style = FindResource("botonDock") as Style;
            nube.Visibility = Visibility.Visible;
            lblnotificaciones.Visibility = Visibility.Visible;
            btnSalidasMI.Visibility = Visibility.Collapsed;
            btnSalidasME.Visibility = Visibility.Collapsed;
            btnClientesMI.Visibility = Visibility.Collapsed;
            btnClientesME.Visibility = Visibility.Collapsed;
            #region Fade in
            //animaciones
            Storyboard storyboard = new Storyboard();
            TimeSpan duration = new TimeSpan(0, 0, 1);

            // fade in
            DoubleAnimation animation = new DoubleAnimation();

            animation.From = 0.0;
            animation.To = 1.0;
            animation.Duration = new Duration(duration);
            // opacity
            Storyboard.SetTargetName(animation, frameInicio.Name);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Control.OpacityProperty));
            // + mainwindow a storyboard
            storyboard.Children.Add(animation);

            //start storyboard
            storyboard.Begin(this);

            #endregion
        }

        private void btnClientes_Click_1(object sender, RoutedEventArgs e)
        {
#pragma warning disable CS0246 // The type or namespace name 'pageClientes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'pageClientes' could not be found (are you missing a using directive or an assembly reference?)
            frameInicio.Content = new pageClientes();
#pragma warning restore CS0246 // The type or namespace name 'pageClientes' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'pageClientes' could not be found (are you missing a using directive or an assembly reference?)
            btnUsuarios.Style = FindResource("botonDock") as Style;
            btnProductos.Style = FindResource("botonDock") as Style;
            btnProveedores.Style = FindResource("botonDock") as Style;
            btnEntradas.Style = FindResource("botonDock") as Style;
            btnSalidas.Style = FindResource("botonDock") as Style;
            //btnCategorias.Style = FindResource("botonDock") as Style;
            btnClientes.Style = FindResource("BotonStylePanel") as Style;
            nube.Visibility = Visibility.Visible;
            lblnotificaciones.Visibility = Visibility.Visible;
            btnSalidasMI.Visibility = Visibility.Collapsed;
            btnSalidasME.Visibility = Visibility.Collapsed;
            btnClientesMI.Visibility = Visibility.Visible;
            btnClientesME.Visibility = Visibility.Visible;
            #region Fade in
            //animaciones
            Storyboard storyboard = new Storyboard();
            TimeSpan duration = new TimeSpan(0, 0, 1);

            // fade in
            DoubleAnimation animation = new DoubleAnimation();

            animation.From = 0.0;
            animation.To = 1.0;
            animation.Duration = new Duration(duration);
            // opacity
            Storyboard.SetTargetName(animation, frameInicio.Name);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Control.OpacityProperty));
            // + mainwindow a storyboard
            storyboard.Children.Add(animation);

            //start storyboard
            storyboard.Begin(this);

            #endregion
        }

        private void btn_ImageB_Blanco_Click(object sender, RoutedEventArgs e)
        {
            frameInicio.Content = new pageInicio();
            btnSalidasMI.Visibility = Visibility.Collapsed;
            btnSalidasME.Visibility = Visibility.Collapsed;


            #region Fade in
            //animaciones
            Storyboard storyboard = new Storyboard();
            TimeSpan duration = new TimeSpan(0, 0, 1);

            // fade in
            DoubleAnimation animation = new DoubleAnimation();

            animation.From = 0.0;
            animation.To = 1.0;
            animation.Duration = new Duration(duration);
            // opacity
            Storyboard.SetTargetName(animation, frameInicio.Name);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Control.OpacityProperty));
            // + mainwindow a storyboard
            storyboard.Children.Add(animation);

            //start storyboard
            storyboard.Begin(this);

            #endregion

        }

        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCerrarSesion1_Click(object sender, RoutedEventArgs e)
        {
           
            var usuarios = new windowUsuarios();

            this.Close();
            usuarios.Show();
        }
        private void loadTimer()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 60);
            dispatcherTimer.Start();
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            productos.loadListaProducto();
            Console.WriteLine("aca estoy soy el timer"); 
           
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnSalidasMI_Click(object sender, RoutedEventArgs e)
        {
            btnSalidasMI.Style = FindResource("buttonSwitchStylePressed") as Style;
            btnSalidasME.Style = FindResource("ButtonStyleSwitchNotSelected") as Style;
            frameInicio.Content = new pageSalidas();


        }

        private void btnSalidasME_Click(object sender, RoutedEventArgs e)
        {
            btnSalidasMI.Style = FindResource("ButtonStyleSwitchNotSelected") as Style;
            btnSalidasME.Style = FindResource("buttonSwitchStylePressed") as Style;
            frameInicio.Content = new Salidas.salidasME();
        }

        private void btnClientesMI_Click(object sender, RoutedEventArgs e)
        {
            btnClientesMI.Style = FindResource("buttonSwitchStylePressed") as Style;
            btnClientesME.Style = FindResource("ButtonStyleSwitchNotSelected") as Style;
            frameInicio.Content = new pageClientes();
        }

        private void btnClientesME_Click(object sender, RoutedEventArgs e)
        {
            btnClientesMI.Style = FindResource("ButtonStyleSwitchNotSelected") as Style;
            btnClientesME.Style = FindResource("buttonSwitchStylePressed") as Style;
            frameInicio.Content = new pageClientesME();
 
        }
    }
}
