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

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        pageProductos productos = new pageProductos();

        public MainWindow()
        {
            InitializeComponent();
            //loadTimer();
        }

        
        private void btnUsuarios_Click(object sender, RoutedEventArgs e)
        {

            frameInicio.Content = new pageUsuarios();  //frame inicio es el cuadro donde se actualizan las pantallas en la ventana
            //principal del programa
            frameUsuarios.Content = new pageTab_Usuario(); //main tab
            frameProductos.Content = new pageTabs_Blank();
            frameProveedores.Content = new pageTabs_Blank();
            frameCategorias.Content = new pageTabs_Blank();
            frameSalidas.Content = new pageTabs_Blank();
            frameEntradas.Content = new pageTabs_Blank();

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
        }

        private void btnProductos_Click(object sender, RoutedEventArgs e)
        {
            frameInicio.Content = new pageProductos();
            frameProductos.Content = new pageTab_Productos(); //main tab
            frameUsuarios.Content = new pageTabs_Blank();
            frameCategorias.Content = new pageTabs_Blank();
            frameProveedores.Content = new pageTabs_Blank();
            frameSalidas.Content = new pageTabs_Blank();
            frameEntradas.Content = new pageTabs_Blank();

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
            frameProveedores.Content = new pageTab_Proveedores(); //main tab load punto de color
            frameUsuarios.Content = new pageTabs_Blank();// unload del resto de las tabs
            frameCategorias.Content = new pageTabs_Blank();
            frameProductos.Content = new pageTabs_Blank();
            frameSalidas.Content = new pageTabs_Blank();
            frameEntradas.Content = new pageTabs_Blank();
            

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
            frameProveedores.Content = new pageTab_Proveedores(); //main tab load punto de color
            frameCategorias.Content = new pageTabs_Blank();
            frameProductos.Content = new pageTabs_Blank();
            frameSalidas.Content = new pageTabs_Blank();
            frameProveedores.Content = new pageTabs_Blank();
            frameEntradas.Content = new pageTab_Entradas();
            frameUsuarios.Content = new pageTabs_Blank();
        }

        private void btnSalidas_Click(object sender, RoutedEventArgs e)
        {
            frameInicio.Content = new pageSalidas();

           
            frameProveedores.Content = new pageTab_Proveedores(); //main tab load punto de color
            frameCategorias.Content = new pageTabs_Blank();
            frameProductos.Content = new pageTabs_Blank();
            frameSalidas.Content = new pageTab_Salidas();
            frameProveedores.Content = new pageTabs_Blank();
            frameEntradas.Content = new pageTabs_Blank();
            frameUsuarios.Content = new pageTabs_Blank();
        }

        private void btnCategorias_Click(object sender, RoutedEventArgs e)
        {
            frameInicio.Content = new pageCategorias();
            frameCategorias.Content = new pageTabs_Categorias(); //main tab
            frameProveedores.Content = new pageTabs_Blank();
            frameProductos.Content = new pageTabs_Blank();
            frameUsuarios.Content = new pageTabs_Blank();
            frameSalidas.Content = new pageTabs_Blank();
            frameEntradas.Content = new pageTabs_Blank();

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
            frameCategorias.Content = new pageTabs_Blank();
            frameProveedores.Content = new pageTabs_Blank();
            frameProductos.Content = new pageTabs_Blank();
            frameUsuarios.Content = new pageTabs_Blank();
            frameSalidas.Content = new pageTabs_Blank();
            frameEntradas.Content = new pageTabs_Blank();

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
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
            dispatcherTimer.Start();
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            productos.loadListaProducto();
            Console.WriteLine("aca estoy soy el timer"); 
           
        }

    }
}
