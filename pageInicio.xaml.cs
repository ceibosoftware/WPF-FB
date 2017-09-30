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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;





namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for pageInicio.xaml
    /// </summary>
    public partial class pageInicio : Page
    {
        public pageInicio()
        {
            InitializeComponent();
            
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
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
            Storyboard.SetTargetName(animation, canvasfblanco.Name);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Control.OpacityProperty));
            // + mainwindow a storyboard
            storyboard.Children.Add(animation);

            //start storyboard
            storyboard.Begin(this);

            #endregion
        }
    }
}


