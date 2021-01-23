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
using Arash;
using DataModelLayer;
using GymManegment.Module;
namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for win_customer.xaml
    /// </summary>
    public partial class win_customer : Window
    {
        public win_customer()
        {
            InitializeComponent();
        
        }
        gymEntities2 database = new gymEntities2();

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            
            //var query = from u in database.VW_Users select u;
            //var user = query.ToList();
            //DataGrid_User.ItemsSource = user;
            ShowUserInfo(searchstate);
        }

        //تابع ساخت شرط برای نمایش اطلاعات در گرید ویو
        private void ShowUserInfo(Func<string> SearchStringForUser)
        {
            var query = database.Database.SqlQuery<VW_Customers>("select * From VW_Customers where 1 = 1 " + SearchStringForUser()) + "";
            //     MessageBox.Show(query.ToString());
            var u = query.ToList();
            DataGrid_Customer.ItemsSource = u;
        }
        private string searchstate()
        {

            string searchstring = " and StartDate between '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Cal_Az.Text)) + "' and '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Cal_Ta.Text)) + "'";

            if (txt_nameMoshtari.Text != "")
            {
                searchstring = " And CustomerName like '%" + txt_nameMoshtari.Text.Trim() + "%'";
            }
            if (txt_adress.Text != "")
            {
                searchstring += " And CustomerAddress like '%" + txt_adress.Text.Trim() + "%'";
            }
            if (txt_tel.Text != "")
            {
                searchstring += " And CustomerTel like '%" + txt_tel.Text.Trim() + "%'";
            }
            // select* from VW_Users where 1 = 1 And UserFamily like reza
          
            return searchstring;
        }

        private void SerCh_Click(object sender, RoutedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //      this.DragMove();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void DataGrid_User_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void calc_image_MouseMove(object sender, MouseEventArgs e)
        {
            calc_image.Visibility = Visibility.Hidden;
        }

        private void calc_image1_MouseMove(object sender, MouseEventArgs e)
        {
            calc_image1.Visibility = Visibility.Hidden;
        }

        private void txt_nameMoshtari_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void txt_adress_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void txt_tel_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            Cal_Az.SelectedDate = PersianDate.Today.AddDays(-1);
        }
    }
}
