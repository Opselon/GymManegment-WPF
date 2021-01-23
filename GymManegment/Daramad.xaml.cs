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

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for Daramad.xaml
    /// </summary>
    public partial class Daramad : Window
    {
        public Daramad()
        {
            InitializeComponent();
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            Srch.Source = new BitmapImage(new Uri("/1/4/searching.png", UriKind.Relative));


        }

        private void MLeave(object sender, MouseEventArgs e)
        {
            Srch.Source = new BitmapImage(new Uri("/1/4/9983812.png", UriKind.Relative));

        }
        private void RB(object sender, MouseEventArgs e)
        {
            Gridet.Margin = new Thickness(-26, 0, -38, 54);


        }
        private void LB(object sender, MouseEventArgs e)
        {
            Gridet.Margin = new Thickness(-26, 0, -38, 54);


        }
        private void Image_MouseDown_2(object sender, MouseEventArgs e)
        {
           Close();


        }
        private void SizeFix()
        {
  
        
        }

    }
}