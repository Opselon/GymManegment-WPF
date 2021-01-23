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

namespace GymManegment.window.Morabi
{
    /// <summary>
    /// Interaction logic for DashboardMain.xaml
    /// </summary>
    public partial class LoginMorabi : Window
    {
        public LoginMorabi()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void TextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
         
        }

        private void NameMember_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void NameMember_TextChanged(object sender, TextChangedEventArgs e)
        {
         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            DashboardMain s = new DashboardMain();
            s.Show();
        }
    }
}
