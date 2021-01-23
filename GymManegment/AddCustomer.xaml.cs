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
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {



        public AddCustomer()
        {
            InitializeComponent();
        }



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        private void Username_txt_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Username_txt.Text == "نام کاربری")
                Username_txt.Text = "";

        }
        private void Username_txt_MouseLeave(object sender, MouseEventArgs e)
        {
          
            if (Username_txt.Text == "")
                Username_txt.Text = "نام کاربری";


        }

        private void Password_txt_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Password_txt.Text == "رمز عبور")
                Password_txt.Text = "";
        }

        private void Password_txt_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Password_txt.Text == "")
                Password_txt.Text = "رمز عبور";
        }

        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Adress_txt.Text == "شهر")
                Adress_txt.Text = "";
        }

        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Adress_txt.Text == "")
                Adress_txt.Text = "شهر";
        }




        private void Phone_txt_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Phone_txt.Text == "شماره تماس")
                Phone_txt.Text = "";
        }

        private void Phone_txt_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Phone_txt.Text == "")
                Phone_txt.Text = "شماره تماس";
        }
        private void RegKey_txt_MouseEnter(object sender, MouseEventArgs e)
        {
            if (RegKey_txt.Text == "کد فعال سازی")
                RegKey_txt.Text = "";
        }
        private void RegKey_txt_MouseLeave(object sender, MouseEventArgs e)
        {
            if (RegKey_txt.Text == "")
                RegKey_txt.Text = "کد فعال سازی";
        }

        private void Username_txt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        object package;




        private void Comb_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {




        }

        private void Apk_Loaded(object sender, RoutedEventArgs e)
        {

    
        }
    }
}
