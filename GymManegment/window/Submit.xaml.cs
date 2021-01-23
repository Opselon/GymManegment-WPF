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
using DataModelLayer;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Navigation;

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for Submit.xaml
    /// </summary>
    public partial class Submit : Window
    {
        public decimal sss;
        public string ss;
        public Submit()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public static class ControlID
        {
            public static string TextData { get; set; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MemberCharge m = new MemberCharge();
            aa.Focus();
            ID.Text = m.txt_username.Text;
        }

        private void aaa_Click(object sender, RoutedEventArgs e)
        {
            SucFul s = new SucFul();
            ErrorPage E = new ErrorPage();
            MemberCharge m = new MemberCharge();
            
            try
            {
                
                ControlID.TextData = a.Text;


              
            }
            catch
            {
                E.Error_Lable.Content = "در ثبت بدهی مشتری مشکلی به وجود آمده است";
            }
            finally
            {
                this.Hide();
            }
          

        }
    }
}
