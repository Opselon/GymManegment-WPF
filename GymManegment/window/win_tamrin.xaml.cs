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
using System.Windows.Shapes;

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for win_tamrin.xaml
    /// </summary>
    public partial class win_tamrin : Window
    {
        public win_tamrin()
        {
            InitializeComponent();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
         
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Ext_btn.Source = new BitmapImage(new Uri("/img/exit.png", UriKind.Relative));
        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Ext_btn_MouseMove(object sender, MouseEventArgs e)
        {
            Ext_btn.Source = new BitmapImage(new Uri("/img/exitt.png", UriKind.Relative));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Image_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(2));
            img.BeginAnimation(Image.OpacityProperty, animation);
        }

        private void Image_MouseEnter_1(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(2));
            img.BeginAnimation(Image.OpacityProperty, animation);
        }

        private void Image_MouseEnter_2(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(2));
            img.BeginAnimation(Image.OpacityProperty, animation);
        }

        private void Image_MouseLeave_2(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(2));
            img.BeginAnimation(Image.OpacityProperty, animation);
        }

        private void Image_MouseEnter_3(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(2));
            img.BeginAnimation(Image.OpacityProperty, animation);
        }

        private void Image_MouseLeave_3(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(2));
            img.BeginAnimation(Image.OpacityProperty, animation);
        }
    }
}
