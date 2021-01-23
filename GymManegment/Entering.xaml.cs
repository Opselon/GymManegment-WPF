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
using GymManegment.Module;

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for Entering.xaml
    /// </summary>
    public partial class Entering : Window
    {
        gymEntities2 database = new gymEntities2();
        public string MemberName;
        public string MemberFamily;
        public int memberid;
        public Entering()
        {
            InitializeComponent();
        }
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {

            this.Close();
        }
       
    
        
    

        private void SerCh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void DataGrid_User_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Cal_Ta_SelectedDateChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ShowUserInfo(Func<string> SearchStringForUser)
        {
            var query = database.Database.SqlQuery<InvView>("select * From InvView where 1 = 1 And UserID = " + memberid + SearchStringForUser());
        //        MessageBox.Show(query.ToString());
            var u = query.ToList();
            EnterinUsers.ItemsSource = u;
        }
        private string searchstate()
        {

            string searchstring = " ";

            //if (txt_name.Text != "")
            //{
            //    searchstring = " And UserName like '%" + txt_name.Text.Trim() + "%'";
            //}
            //if (txt_family.Text != "")
            //{
            //    searchstring += " And UserFamily like '%" + txt_family.Text.Trim() + "%'";
            //}

            //if (R_Dis.IsChecked == true)
            //{

            //    searchstring += "And UserActive = 2";
            //}
            //else
            //{
            //    searchstring += "And UserActive = 1";
            //}
            return searchstring;
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {




        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            ShowUserInfo(searchstate);
            Lbl_Name.Content = "" + MemberName + " " + MemberFamily;
         //   txt_username.Text = MemberName + " " + MemberFamily;

        }


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            Users US = new Users();
            this.Close();
            US.Opacity = 1;
        }

        private void txt_username_TextChanged(object sender, TextChangedEventArgs e)
        {
          //  txt_username.Text = MemberName + " " + MemberFamily;

        }

        private void txt_username_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txt_username_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txt_username_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void txt_username_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void EnterinUsers_Loaded(object sender, RoutedEventArgs e)
        {

            ShowUserInfo(searchstate);
        }
    }
}

