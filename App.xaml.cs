using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        private const int MINIMUM_SPLASH_TIME = 1500; // milisecs
        private const int SPLASH_FADE_TIME = 500;     //milisecs

        

        protected override void OnStartup(StartupEventArgs e)
        {
            


            SplashScreen splash = new SplashScreen("ceibo_splash.png");
            
           
            splash.Show(false, true); //cargar splash screen

            Stopwatch timer = new Stopwatch();
            timer.Start(); //empezar stopwatch

            
            base.OnStartup(e);
            MainWindow main = new MainWindow(); //load/cargar ventana

            
            timer.Stop(); //timer para que dure 2 segundos en pantalla
            int remainingTimeToShowSplash = MINIMUM_SPLASH_TIME - (int)timer.ElapsedMilliseconds;
            if (remainingTimeToShowSplash > 0)
                Thread.Sleep(remainingTimeToShowSplash);

            
            splash.Close(TimeSpan.FromMilliseconds(SPLASH_FADE_TIME)); //show
            //main.Show();

            var newW = new windowUsuarios();
            newW.Show();




        }

       
    }
}
