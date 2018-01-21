using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for windowCuotas.xaml
    /// </summary>
    public partial class windowCuotas : Window
    {
        int cantidadCuota;
        DateTime fechafactura;
        public List<cuotas> listacuotas = new List<cuotas>();
       
        public windowCuotas()
        {
            InitializeComponent();

        }

        public windowCuotas(int cantidadcuotas, DateTime fechaFactura)
        {
            listacuotas.Clear();
            InitializeComponent();
            ocutarElementos();
            this.cantidadCuota = cantidadcuotas;
            this.fechafactura = fechaFactura;

            if (cantidadCuota == 1)
            {
                lbl1.Visibility = Visibility.Visible;
                dias1.Visibility = Visibility.Visible;
                txt1.Visibility = Visibility.Visible;
            }
            else if (cantidadCuota == 2)
            {
                lbl1.Visibility = Visibility.Visible;
                dias1.Visibility = Visibility.Visible;
                txt1.Visibility = Visibility.Visible;
                lbl2.Visibility = Visibility.Visible;
                dias2.Visibility = Visibility.Visible;
                txt2.Visibility = Visibility.Visible;
            }
            else if (cantidadCuota == 3)
            {
                lbl1.Visibility = Visibility.Visible;
                dias1.Visibility = Visibility.Visible;
                txt1.Visibility = Visibility.Visible;
                lbl2.Visibility = Visibility.Visible;
                dias2.Visibility = Visibility.Visible;
                txt2.Visibility = Visibility.Visible;
                lbl3.Visibility = Visibility.Visible;
                dias3.Visibility = Visibility.Visible;
                txt3.Visibility = Visibility.Visible;
            }
            else if (cantidadCuota == 4)
            {
                lbl1.Visibility = Visibility.Visible;
                dias1.Visibility = Visibility.Visible;
                txt1.Visibility = Visibility.Visible;
                lbl2.Visibility = Visibility.Visible;
                dias2.Visibility = Visibility.Visible;
                txt2.Visibility = Visibility.Visible;
                lbl3.Visibility = Visibility.Visible;
                dias3.Visibility = Visibility.Visible;
                txt3.Visibility = Visibility.Visible;
                lbl4.Visibility = Visibility.Visible;
                dias4.Visibility = Visibility.Visible;
                txt4.Visibility = Visibility.Visible;
            }
            else if (cantidadCuota == 5)
            {
                lbl1.Visibility = Visibility.Visible;
                dias1.Visibility = Visibility.Visible;
                txt1.Visibility = Visibility.Visible;
                lbl2.Visibility = Visibility.Visible;
                dias2.Visibility = Visibility.Visible;
                txt2.Visibility = Visibility.Visible;
                lbl3.Visibility = Visibility.Visible;
                dias3.Visibility = Visibility.Visible;
                txt3.Visibility = Visibility.Visible;
                lbl4.Visibility = Visibility.Visible;
                dias4.Visibility = Visibility.Visible;
                txt4.Visibility = Visibility.Visible;
                lbl5.Visibility = Visibility.Visible;
                dias5.Visibility = Visibility.Visible;
                txt5.Visibility = Visibility.Visible;
            }
            else if (cantidadCuota == 6)
            {
                lbl1.Visibility = Visibility.Visible;
                dias1.Visibility = Visibility.Visible;
                txt1.Visibility = Visibility.Visible;
                lbl2.Visibility = Visibility.Visible;
                dias2.Visibility = Visibility.Visible;
                txt2.Visibility = Visibility.Visible;
                lbl3.Visibility = Visibility.Visible;
                dias3.Visibility = Visibility.Visible;
                txt3.Visibility = Visibility.Visible;
                lbl4.Visibility = Visibility.Visible;
                dias4.Visibility = Visibility.Visible;
                txt4.Visibility = Visibility.Visible;
                lbl5.Visibility = Visibility.Visible;
                dias5.Visibility = Visibility.Visible;
                txt5.Visibility = Visibility.Visible;
                lbl6.Visibility = Visibility.Visible;
                dias6.Visibility = Visibility.Visible;
                txt6.Visibility = Visibility.Visible;
            }
            else if (cantidadCuota == 7)
            {
                lbl1.Visibility = Visibility.Visible;
                dias1.Visibility = Visibility.Visible;
                txt1.Visibility = Visibility.Visible;
                lbl2.Visibility = Visibility.Visible;
                dias2.Visibility = Visibility.Visible;
                txt2.Visibility = Visibility.Visible;
                lbl3.Visibility = Visibility.Visible;
                dias3.Visibility = Visibility.Visible;
                txt3.Visibility = Visibility.Visible;
                lbl4.Visibility = Visibility.Visible;
                dias4.Visibility = Visibility.Visible;
                txt4.Visibility = Visibility.Visible;
                lbl5.Visibility = Visibility.Visible;
                dias5.Visibility = Visibility.Visible;
                txt5.Visibility = Visibility.Visible;
                lbl6.Visibility = Visibility.Visible;
                dias6.Visibility = Visibility.Visible;
                txt6.Visibility = Visibility.Visible;
                lbl7.Visibility = Visibility.Visible;
                dias7.Visibility = Visibility.Visible;
                txt7.Visibility = Visibility.Visible;
            }
            else if (cantidadCuota == 8)
            {
                lbl1.Visibility = Visibility.Visible;
                dias1.Visibility = Visibility.Visible;
                txt1.Visibility = Visibility.Visible;
                lbl2.Visibility = Visibility.Visible;
                dias2.Visibility = Visibility.Visible;
                txt2.Visibility = Visibility.Visible;
                lbl3.Visibility = Visibility.Visible;
                dias3.Visibility = Visibility.Visible;
                txt3.Visibility = Visibility.Visible;
                lbl4.Visibility = Visibility.Visible;
                dias4.Visibility = Visibility.Visible;
                txt4.Visibility = Visibility.Visible;
                lbl5.Visibility = Visibility.Visible;
                dias5.Visibility = Visibility.Visible;
                txt5.Visibility = Visibility.Visible;
                lbl6.Visibility = Visibility.Visible;
                dias6.Visibility = Visibility.Visible;
                txt6.Visibility = Visibility.Visible;
                lbl7.Visibility = Visibility.Visible;
                dias7.Visibility = Visibility.Visible;
                txt7.Visibility = Visibility.Visible;
                lbl8.Visibility = Visibility.Visible;
                dias8.Visibility = Visibility.Visible;
                txt8.Visibility = Visibility.Visible;
            }
            else if (cantidadCuota == 9)
            {
                lbl1.Visibility = Visibility.Visible;
                dias1.Visibility = Visibility.Visible;
                txt1.Visibility = Visibility.Visible;
                lbl2.Visibility = Visibility.Visible;
                dias2.Visibility = Visibility.Visible;
                txt2.Visibility = Visibility.Visible;
                lbl3.Visibility = Visibility.Visible;
                dias3.Visibility = Visibility.Visible;
                txt3.Visibility = Visibility.Visible;
                lbl4.Visibility = Visibility.Visible;
                dias4.Visibility = Visibility.Visible;
                txt4.Visibility = Visibility.Visible;
                lbl5.Visibility = Visibility.Visible;
                dias5.Visibility = Visibility.Visible;
                txt5.Visibility = Visibility.Visible;
                lbl6.Visibility = Visibility.Visible;
                dias6.Visibility = Visibility.Visible;
                txt6.Visibility = Visibility.Visible;
                lbl7.Visibility = Visibility.Visible;
                dias7.Visibility = Visibility.Visible;
                txt7.Visibility = Visibility.Visible;
                lbl8.Visibility = Visibility.Visible;
                dias8.Visibility = Visibility.Visible;
                lbl8.Visibility = Visibility.Visible;
                dias9.Visibility = Visibility.Visible;
                txt9.Visibility = Visibility.Visible;
                lbl9.Visibility = Visibility.Visible;
            }
            else if (cantidadCuota == 10)
            {
                lbl1.Visibility = Visibility.Visible;
                dias1.Visibility = Visibility.Visible;
                txt1.Visibility = Visibility.Visible;
                lbl2.Visibility = Visibility.Visible;
                dias2.Visibility = Visibility.Visible;
                txt2.Visibility = Visibility.Visible;
                lbl3.Visibility = Visibility.Visible;
                dias3.Visibility = Visibility.Visible;
                txt3.Visibility = Visibility.Visible;
                lbl4.Visibility = Visibility.Visible;
                dias4.Visibility = Visibility.Visible;
                txt4.Visibility = Visibility.Visible;
                lbl5.Visibility = Visibility.Visible;
                dias5.Visibility = Visibility.Visible;
                txt5.Visibility = Visibility.Visible;
                lbl6.Visibility = Visibility.Visible;
                dias6.Visibility = Visibility.Visible;
                txt6.Visibility = Visibility.Visible;
                lbl7.Visibility = Visibility.Visible;
                dias7.Visibility = Visibility.Visible;
                txt7.Visibility = Visibility.Visible;
                lbl8.Visibility = Visibility.Visible;
                dias8.Visibility = Visibility.Visible;
                lbl8.Visibility = Visibility.Visible;
                dias9.Visibility = Visibility.Visible;
                txt9.Visibility = Visibility.Visible;
                txt9.Visibility = Visibility.Visible;
                dias10.Visibility = Visibility.Visible;
                txt10.Visibility = Visibility.Visible;
                lbl10.Visibility = Visibility.Visible;
            }
            else if (cantidadCuota == 11)
            {
                lbl1.Visibility = Visibility.Visible;
                dias1.Visibility = Visibility.Visible;
                txt1.Visibility = Visibility.Visible;
                lbl2.Visibility = Visibility.Visible;
                dias2.Visibility = Visibility.Visible;
                txt2.Visibility = Visibility.Visible;
                lbl3.Visibility = Visibility.Visible;
                dias3.Visibility = Visibility.Visible;
                txt3.Visibility = Visibility.Visible;
                lbl4.Visibility = Visibility.Visible;
                dias4.Visibility = Visibility.Visible;
                txt4.Visibility = Visibility.Visible;
                lbl5.Visibility = Visibility.Visible;
                dias5.Visibility = Visibility.Visible;
                txt5.Visibility = Visibility.Visible;
                lbl6.Visibility = Visibility.Visible;
                dias6.Visibility = Visibility.Visible;
                txt6.Visibility = Visibility.Visible;
                lbl7.Visibility = Visibility.Visible;
                dias7.Visibility = Visibility.Visible;
                txt7.Visibility = Visibility.Visible;
                lbl8.Visibility = Visibility.Visible;
                dias8.Visibility = Visibility.Visible;
                lbl8.Visibility = Visibility.Visible;
                dias9.Visibility = Visibility.Visible;
                txt9.Visibility = Visibility.Visible;
                txt9.Visibility = Visibility.Visible;
                dias10.Visibility = Visibility.Visible;
                txt10.Visibility = Visibility.Visible;
                txt10.Visibility = Visibility.Visible;
                dias11.Visibility = Visibility.Visible;
                txt11.Visibility = Visibility.Visible;
                lbl11.Visibility = Visibility.Visible;
            }
            else
            {
                lbl1.Visibility = Visibility.Visible;
                dias1.Visibility = Visibility.Visible;
                txt1.Visibility = Visibility.Visible;
                lbl2.Visibility = Visibility.Visible;
                dias2.Visibility = Visibility.Visible;
                txt2.Visibility = Visibility.Visible;
                lbl3.Visibility = Visibility.Visible;
                dias3.Visibility = Visibility.Visible;
                txt3.Visibility = Visibility.Visible;
                lbl4.Visibility = Visibility.Visible;
                dias4.Visibility = Visibility.Visible;
                txt4.Visibility = Visibility.Visible;
                lbl5.Visibility = Visibility.Visible;
                dias5.Visibility = Visibility.Visible;
                txt5.Visibility = Visibility.Visible;
                lbl6.Visibility = Visibility.Visible;
                dias6.Visibility = Visibility.Visible;
                txt6.Visibility = Visibility.Visible;
                lbl7.Visibility = Visibility.Visible;
                dias7.Visibility = Visibility.Visible;
                txt7.Visibility = Visibility.Visible;
                lbl8.Visibility = Visibility.Visible;
                dias8.Visibility = Visibility.Visible;
                lbl8.Visibility = Visibility.Visible;
                dias9.Visibility = Visibility.Visible;
                txt9.Visibility = Visibility.Visible;
                txt9.Visibility = Visibility.Visible;
                dias10.Visibility = Visibility.Visible;
                txt10.Visibility = Visibility.Visible;
                txt10.Visibility = Visibility.Visible;
                dias11.Visibility = Visibility.Visible;
                txt11.Visibility = Visibility.Visible;
                txt11.Visibility = Visibility.Visible;
                dias12.Visibility = Visibility.Visible;
                txt12.Visibility = Visibility.Visible;
                lbl12.Visibility = Visibility.Visible;
            }
      
        }
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            listacuotas.Clear();
            if (cantidadCuota == 1)
            {
           
                int dias = int.Parse(txt1.Text);
                DateTime answer = fechafactura.AddDays(dias); 
                cuotas cuota = new cuotas(1, dias, answer);          
                listacuotas.Add(cuota);             
                DialogResult = true;
            }
            else if (cantidadCuota == 2)
            {
                int dias = int.Parse(txt1.Text);
                int dias2 = int.Parse(txt2.Text);
                DateTime answer = fechafactura.AddDays(dias);
                DateTime answer2 = fechafactura.AddDays(dias2);
               
                cuotas cuota = new cuotas(1, dias, answer);
                cuotas cuota2 = new cuotas(2, dias2, answer2);
             
                listacuotas.Add(cuota);
                listacuotas.Add(cuota2);
                DialogResult = true;
            }
            else if (cantidadCuota == 3)
      {
                int dias = int.Parse(txt1.Text);
                int dias2 = int.Parse(txt2.Text);
                int dias3 = int.Parse(txt3.Text);
                DateTime answer = fechafactura.AddDays(dias);
                DateTime answer2 = fechafactura.AddDays(dias2);
                DateTime answer3 = fechafactura.AddDays(dias3);
     
                cuotas cuota = new cuotas(1, dias, answer);
                cuotas cuota2 = new cuotas(2, dias2, answer2);
                cuotas cuota3 = new cuotas(3, dias3, answer3);
               
                listacuotas.Add(cuota);
                listacuotas.Add(cuota2);
                listacuotas.Add(cuota3);
                DialogResult = true;
            }
            else if (cantidadCuota == 4)
            {
                int dias = int.Parse(txt1.Text);
                int dias2 = int.Parse(txt2.Text);
                int dias3 = int.Parse(txt3.Text);
                int dias4 = int.Parse(txt4.Text);
                DateTime answer = fechafactura.AddDays(dias);
                DateTime answer2 = fechafactura.AddDays(dias2);
                DateTime answer3 = fechafactura.AddDays(dias3);
                DateTime answer4 = fechafactura.AddDays(dias4);
           
                cuotas cuota = new cuotas(1, dias, answer);
                cuotas cuota2 = new cuotas(2, dias2, answer2);
                cuotas cuota3 = new cuotas(3, dias3, answer3);
                cuotas cuota4 = new cuotas(4, dias4, answer4);
         
                listacuotas.Add(cuota);
                listacuotas.Add(cuota2);
                listacuotas.Add(cuota3);
                listacuotas.Add(cuota4);
                DialogResult = true;
            }
            else if (cantidadCuota == 5)
            {

                int dias = int.Parse(txt1.Text);
                int dias2 = int.Parse(txt2.Text);
                int dias3 = int.Parse(txt3.Text);
                int dias4 = int.Parse(txt4.Text);
                int dias5 = int.Parse(txt5.Text);
                DateTime answer = fechafactura.AddDays(dias);
                DateTime answer2 = fechafactura.AddDays(dias2);
                DateTime answer3 = fechafactura.AddDays(dias3);
                DateTime answer4 = fechafactura.AddDays(dias4);
                DateTime answer5 = fechafactura.AddDays(dias5);
      
                cuotas cuota = new cuotas(1, dias, answer);
                cuotas cuota2 = new cuotas(2, dias2, answer2);
                cuotas cuota3 = new cuotas(3, dias3, answer3);
                cuotas cuota4 = new cuotas(4, dias4, answer4);
                cuotas cuota5 = new cuotas(5, dias5, answer5);
         
                listacuotas.Add(cuota);
                listacuotas.Add(cuota2);
                listacuotas.Add(cuota3);
                listacuotas.Add(cuota4);
                listacuotas.Add(cuota5);
                DialogResult = true;
            }
            else if (cantidadCuota == 6)
            {

                int dias = int.Parse(txt1.Text);
                int dias2 = int.Parse(txt2.Text);
                int dias3 = int.Parse(txt3.Text);
                int dias4 = int.Parse(txt4.Text);
                int dias5 = int.Parse(txt5.Text);
                int dias6 = int.Parse(txt6.Text);
                DateTime answer = fechafactura.AddDays(dias);
                DateTime answer2 = fechafactura.AddDays(dias2);
                DateTime answer3 = fechafactura.AddDays(dias3);
                DateTime answer4 = fechafactura.AddDays(dias4);
                DateTime answer5 = fechafactura.AddDays(dias5);
                DateTime answer6 = fechafactura.AddDays(dias6);
   
                cuotas cuota = new cuotas(1, dias, answer);
                cuotas cuota2 = new cuotas(2, dias2, answer2);
                cuotas cuota3 = new cuotas(3, dias3, answer3);
                cuotas cuota4 = new cuotas(4, dias4, answer4);
                cuotas cuota5 = new cuotas(5, dias5, answer5);
                cuotas cuota6 = new cuotas(6, dias6, answer6);
             
                listacuotas.Add(cuota);
                listacuotas.Add(cuota2);
                listacuotas.Add(cuota3);
                listacuotas.Add(cuota4);
                listacuotas.Add(cuota5);
                listacuotas.Add(cuota6);
                DialogResult = true;

            }
            else if (cantidadCuota == 7)
            {
                int dias = int.Parse(txt1.Text);
                int dias2 = int.Parse(txt2.Text);
                int dias3 = int.Parse(txt3.Text);
                int dias4 = int.Parse(txt4.Text);
                int dias5 = int.Parse(txt5.Text);
                int dias6 = int.Parse(txt6.Text);
                int dias7 = int.Parse(txt7.Text);
                DateTime answer = fechafactura.AddDays(dias);
                DateTime answer2 = fechafactura.AddDays(dias2);
                DateTime answer3 = fechafactura.AddDays(dias3);
                DateTime answer4 = fechafactura.AddDays(dias4);
                DateTime answer5 = fechafactura.AddDays(dias5);
                DateTime answer6 = fechafactura.AddDays(dias6);
                DateTime answer7 = fechafactura.AddDays(dias7);
              
                cuotas cuota = new cuotas(1, dias, answer);
                cuotas cuota2 = new cuotas(2, dias2, answer2);
                cuotas cuota3 = new cuotas(3, dias3, answer3);
                cuotas cuota4 = new cuotas(4, dias4, answer4);
                cuotas cuota5 = new cuotas(5, dias5, answer5);
                cuotas cuota6 = new cuotas(6, dias6, answer6);
                cuotas cuota7 = new cuotas(7, dias7, answer7);

                listacuotas.Add(cuota);
                listacuotas.Add(cuota2);
                listacuotas.Add(cuota3);
                listacuotas.Add(cuota4);
                listacuotas.Add(cuota5);
                listacuotas.Add(cuota6);
                listacuotas.Add(cuota7);
                DialogResult = true;
            }
            else if (cantidadCuota == 8)
            {

                int dias = int.Parse(txt1.Text);
                int dias2 = int.Parse(txt2.Text);
                int dias3 = int.Parse(txt3.Text);
                int dias4 = int.Parse(txt4.Text);
                int dias5 = int.Parse(txt5.Text);
                int dias6 = int.Parse(txt6.Text);
                int dias7 = int.Parse(txt7.Text);
                int dias8 = int.Parse(txt8.Text);
                DateTime answer = fechafactura.AddDays(dias);
                DateTime answer2 = fechafactura.AddDays(dias2);
                DateTime answer3 = fechafactura.AddDays(dias3);
                DateTime answer4 = fechafactura.AddDays(dias4);
                DateTime answer5 = fechafactura.AddDays(dias5);
                DateTime answer6 = fechafactura.AddDays(dias6);
                DateTime answer7 = fechafactura.AddDays(dias7);
                DateTime answer8 = fechafactura.AddDays(dias8);
                cuotas cuota = new cuotas(1, dias, answer);
                cuotas cuota2 = new cuotas(2, dias2, answer2);
                cuotas cuota3 = new cuotas(3, dias3, answer3);
                cuotas cuota4 = new cuotas(4, dias4, answer4);
                cuotas cuota5 = new cuotas(5, dias5, answer5);
                cuotas cuota6 = new cuotas(6, dias6, answer6);
                cuotas cuota7 = new cuotas(7, dias7, answer7);
                cuotas cuota8 = new cuotas(8, dias8, answer8);
                listacuotas.Add(cuota);
                listacuotas.Add(cuota2);
                listacuotas.Add(cuota3);
                listacuotas.Add(cuota4);
                listacuotas.Add(cuota5);
                listacuotas.Add(cuota6);
                listacuotas.Add(cuota7);
                listacuotas.Add(cuota8);
                DialogResult = true;
            }
            else if (cantidadCuota == 9)
            {

                int dias = int.Parse(txt1.Text);
                int dias2 = int.Parse(txt2.Text);
                int dias3 = int.Parse(txt3.Text);
                int dias4 = int.Parse(txt4.Text);
                int dias5 = int.Parse(txt5.Text);
                int dias6 = int.Parse(txt6.Text);
                int dias7 = int.Parse(txt7.Text);
                int dias8 = int.Parse(txt8.Text);
                int dias9 = int.Parse(txt9.Text);
                DateTime answer = fechafactura.AddDays(dias);
                DateTime answer2 = fechafactura.AddDays(dias2);
                DateTime answer3 = fechafactura.AddDays(dias3);
                DateTime answer4 = fechafactura.AddDays(dias4);
                DateTime answer5 = fechafactura.AddDays(dias5);
                DateTime answer6 = fechafactura.AddDays(dias6);
                DateTime answer7 = fechafactura.AddDays(dias7);
                DateTime answer8 = fechafactura.AddDays(dias8);
                DateTime answer9 = fechafactura.AddDays(dias9);
                cuotas cuota = new cuotas(1, dias, answer);
                cuotas cuota2 = new cuotas(2, dias2, answer2);
                cuotas cuota3 = new cuotas(3, dias3, answer3);
                cuotas cuota4 = new cuotas(4, dias4, answer4);
                cuotas cuota5 = new cuotas(5, dias5, answer5);
                cuotas cuota6 = new cuotas(6, dias6, answer6);
                cuotas cuota7 = new cuotas(7, dias7, answer7);
                cuotas cuota8 = new cuotas(8, dias8, answer8);
                cuotas cuota9 = new cuotas(9, dias9, answer9);
                listacuotas.Add(cuota);
                listacuotas.Add(cuota2);
                listacuotas.Add(cuota3);
                listacuotas.Add(cuota4);
                listacuotas.Add(cuota5);
                listacuotas.Add(cuota6);
                listacuotas.Add(cuota7);
                listacuotas.Add(cuota8);
                listacuotas.Add(cuota9);
                DialogResult = true;
            }
            else if (cantidadCuota == 10)
            {
                int dias = int.Parse(txt1.Text);
                int dias2 = int.Parse(txt2.Text);
                int dias3 = int.Parse(txt3.Text);
                int dias4 = int.Parse(txt4.Text);
                int dias5 = int.Parse(txt5.Text);
                int dias6 = int.Parse(txt6.Text);
                int dias7 = int.Parse(txt7.Text);
                int dias8 = int.Parse(txt8.Text);
                int dias9 = int.Parse(txt9.Text);
                int dias10 = int.Parse(txt10.Text);
                DateTime answer = fechafactura.AddDays(dias);
                DateTime answer2 = fechafactura.AddDays(dias2);
                DateTime answer3 = fechafactura.AddDays(dias3);
                DateTime answer4 = fechafactura.AddDays(dias4);
                DateTime answer5 = fechafactura.AddDays(dias5);
                DateTime answer6 = fechafactura.AddDays(dias6);
                DateTime answer7 = fechafactura.AddDays(dias7);
                DateTime answer8 = fechafactura.AddDays(dias8);
                DateTime answer9 = fechafactura.AddDays(dias9);
                DateTime answer10 = fechafactura.AddDays(dias10);
                cuotas cuota = new cuotas(1, dias, answer);
                cuotas cuota2 = new cuotas(2, dias2, answer2);
                cuotas cuota3 = new cuotas(3, dias3, answer3);
                cuotas cuota4 = new cuotas(4, dias4, answer4);
                cuotas cuota5 = new cuotas(5, dias5, answer5);
                cuotas cuota6 = new cuotas(6, dias6, answer6);
                cuotas cuota7 = new cuotas(7, dias7, answer7);
                cuotas cuota8 = new cuotas(8, dias8, answer8);
                cuotas cuota9 = new cuotas(9, dias9, answer9);
                cuotas cuota10 = new cuotas(10, dias10, answer10);
                listacuotas.Add(cuota);
                listacuotas.Add(cuota2);
                listacuotas.Add(cuota3);
                listacuotas.Add(cuota4);
                listacuotas.Add(cuota5);
                listacuotas.Add(cuota6);
                listacuotas.Add(cuota7);
                listacuotas.Add(cuota8);
                listacuotas.Add(cuota9);
                listacuotas.Add(cuota10);
                DialogResult = true;
            }
            else if (cantidadCuota == 11)
            {

                int dias = int.Parse(txt1.Text);
                int dias2 = int.Parse(txt2.Text);
                int dias3 = int.Parse(txt3.Text);
                int dias4 = int.Parse(txt4.Text);
                int dias5 = int.Parse(txt5.Text);
                int dias6 = int.Parse(txt6.Text);
                int dias7 = int.Parse(txt7.Text);
                int dias8 = int.Parse(txt8.Text);
                int dias9 = int.Parse(txt9.Text);
                int dias10 = int.Parse(txt10.Text);
                int dias11 = int.Parse(txt11.Text);
                DateTime answer = fechafactura.AddDays(dias);
                DateTime answer2 = fechafactura.AddDays(dias2);
                DateTime answer3 = fechafactura.AddDays(dias3);
                DateTime answer4 = fechafactura.AddDays(dias4);
                DateTime answer5 = fechafactura.AddDays(dias5);
                DateTime answer6 = fechafactura.AddDays(dias6);
                DateTime answer7 = fechafactura.AddDays(dias7);
                DateTime answer8 = fechafactura.AddDays(dias8);
                DateTime answer9 = fechafactura.AddDays(dias9);
                DateTime answer10 = fechafactura.AddDays(dias10);
                DateTime answer11 = fechafactura.AddDays(dias11);
                cuotas cuota = new cuotas(1, dias, answer);
                cuotas cuota2 = new cuotas(2, dias2, answer2);
                cuotas cuota3 = new cuotas(3, dias3, answer3);
                cuotas cuota4 = new cuotas(4, dias4, answer4);
                cuotas cuota5 = new cuotas(5, dias5, answer5);
                cuotas cuota6 = new cuotas(6, dias6, answer6);
                cuotas cuota7 = new cuotas(7, dias7, answer7);
                cuotas cuota8 = new cuotas(8, dias8, answer8);
                cuotas cuota9 = new cuotas(9, dias9, answer9);
                cuotas cuota10 = new cuotas(10, dias10, answer10);
                cuotas cuota11 = new cuotas(11, dias11, answer11);
                listacuotas.Add(cuota);
                listacuotas.Add(cuota2);
                listacuotas.Add(cuota3);
                listacuotas.Add(cuota4);
                listacuotas.Add(cuota5);
                listacuotas.Add(cuota6);
                listacuotas.Add(cuota7);
                listacuotas.Add(cuota8);
                listacuotas.Add(cuota9);
                listacuotas.Add(cuota10);
                listacuotas.Add(cuota11);
                DialogResult = true;
            }
            else
            {
                int dias = int.Parse(txt1.Text);
                int dias2 = int.Parse(txt2.Text);
                int dias3 = int.Parse(txt3.Text);
                int dias4 = int.Parse(txt4.Text);
                int dias5 = int.Parse(txt5.Text);
                int dias6 = int.Parse(txt6.Text);
                int dias7 = int.Parse(txt7.Text);
                int dias8 = int.Parse(txt8.Text);
                int dias9 = int.Parse(txt9.Text);
                int dias10 = int.Parse(txt10.Text);
                int dias11 = int.Parse(txt11.Text);
                int dias12 = int.Parse(txt12.Text);
                DateTime answer = fechafactura.AddDays(dias);
                DateTime answer2 = fechafactura.AddDays(dias2);
                DateTime answer3 = fechafactura.AddDays(dias3);
                DateTime answer4 = fechafactura.AddDays(dias4);
                DateTime answer5 = fechafactura.AddDays(dias5);
                DateTime answer6 = fechafactura.AddDays(dias6);
                DateTime answer7 = fechafactura.AddDays(dias7);
                DateTime answer8 = fechafactura.AddDays(dias8);
                DateTime answer9 = fechafactura.AddDays(dias9);
                DateTime answer10 = fechafactura.AddDays(dias10);
                DateTime answer11 = fechafactura.AddDays(dias11);
                DateTime answer12 = fechafactura.AddDays(dias12);
                cuotas cuota = new cuotas(1, dias, answer);
                cuotas cuota2 = new cuotas(2, dias2, answer2);
                cuotas cuota3 = new cuotas(3, dias3, answer3);
                cuotas cuota4 = new cuotas(4, dias4, answer4);
                cuotas cuota5 = new cuotas(5, dias5, answer5);
                cuotas cuota6 = new cuotas(6, dias6, answer6);
                cuotas cuota7 = new cuotas(7, dias7, answer7);
                cuotas cuota8 = new cuotas(8, dias8, answer8);
                cuotas cuota9 = new cuotas(9, dias9, answer9);
                cuotas cuota10 = new cuotas(10, dias10, answer10);
                cuotas cuota11 = new cuotas(11, dias11, answer11);
                cuotas cuota12 = new cuotas(12, dias12, answer12);
                listacuotas.Add(cuota);
                listacuotas.Add(cuota2);
                listacuotas.Add(cuota3);
                listacuotas.Add(cuota4);
                listacuotas.Add(cuota5);
                listacuotas.Add(cuota6);
                listacuotas.Add(cuota7);
                listacuotas.Add(cuota8);
                listacuotas.Add(cuota9);
                listacuotas.Add(cuota10);
                listacuotas.Add(cuota11);
                listacuotas.Add(cuota12);
                DialogResult = true;
            }
        }

    
    
        

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
           
            this.Close();
        }

        private void ocutarElementos()
        {
            lbl1.Visibility = Visibility.Collapsed;
            lbl2.Visibility = Visibility.Collapsed;
            lbl3.Visibility = Visibility.Collapsed;
            lbl4.Visibility = Visibility.Collapsed;
            lbl5.Visibility = Visibility.Collapsed;
            lbl6.Visibility = Visibility.Collapsed;
            lbl7.Visibility = Visibility.Collapsed;
            lbl8.Visibility = Visibility.Collapsed;
            lbl9.Visibility = Visibility.Collapsed;
            lbl10.Visibility = Visibility.Collapsed;
            lbl11.Visibility = Visibility.Collapsed;
            lbl12.Visibility = Visibility.Collapsed;

            txt1.Visibility = Visibility.Collapsed;
            txt2.Visibility = Visibility.Collapsed;
            txt3.Visibility = Visibility.Collapsed;
            txt4.Visibility = Visibility.Collapsed;
            txt5.Visibility = Visibility.Collapsed;
            txt6.Visibility = Visibility.Collapsed;
            txt7.Visibility = Visibility.Collapsed;
            txt8.Visibility = Visibility.Collapsed;
            txt9.Visibility = Visibility.Collapsed;
            txt10.Visibility = Visibility.Collapsed;
            txt11.Visibility = Visibility.Collapsed;
            txt12.Visibility = Visibility.Collapsed;

            dias1.Visibility = Visibility.Collapsed;
            dias2.Visibility = Visibility.Collapsed;
            dias3.Visibility = Visibility.Collapsed;
            dias4.Visibility = Visibility.Collapsed;
            dias5.Visibility = Visibility.Collapsed;
            dias6.Visibility = Visibility.Collapsed;
            dias7.Visibility = Visibility.Collapsed;
            dias8.Visibility = Visibility.Collapsed;
            dias9.Visibility = Visibility.Collapsed;
            dias10.Visibility = Visibility.Collapsed;
            dias11.Visibility = Visibility.Collapsed;
            dias12.Visibility = Visibility.Collapsed;
   


        }
    }
}
