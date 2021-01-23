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
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : Window
    {
        
        public Users()
        {

            InitializeComponent();

       

        }

        gymEntities2 database = new gymEntities2();



        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        
        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
        
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
         
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Ext_btn.Source = new BitmapImage(new Uri("/img/exitt.png", UriKind.Relative));
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //DataGridTextColumn textColumn1 = new DataGridTextColumn();
            //textColumn1.Header = "Your header";
            //textColumn1.Binding = new Binding("UserImage");

            //var query = from u in database.VW_Users select u;
            //var user = query.ToList();
            //DataGrid_User.ItemsSource = user;
            ShowUserInfo(searchstate);
        }

        //تابع ساخت شرط برای نمایش اطلاعات در گرید ویو
        public void ShowUserInfo(Func<string> SearchStringForUser)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(PublicVar.ConnectionString))
                {
                    string OnlineMethod = ("select * From VW_Users where 1 = 1 " + SearchStringForUser());
                    SqlCommand cmd = new SqlCommand(OnlineMethod, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable("Employee");
                    sda.Fill(dt);
                    DataGrid_User.ItemsSource = dt.DefaultView;
                }
            }
            catch
            {
                var query = database.Database.SqlQuery<VW_Users>("select * From VW_Users where 1 = 1 " + SearchStringForUser());
                //  MessageBox.Show(query.ToString());
                var u = query.ToList();
                DataGrid_User.ItemsSource = u;
            }
  
        }
        public string searchstate()
        {

            string searchstring = "";

            if (txt_name.Text != "")
            {
                searchstring = " And UserName like '%" + txt_name.Text.Trim() + "%'";
            }
            if (txt_family.Text != "")
            {
                searchstring += " And UserFamily like '%" + txt_family.Text.Trim() + "%'";
            }

            if (R_Dis.IsChecked == true)
            {

                searchstring += "And UserActive = 2";
            }
            if (R_Enb.IsChecked == true)
            {
                searchstring += "And UserActive = 1";

            }
            else
                searchstring += "And UserActive > 0";

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
      
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void DataGrid_User_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            con1.Open();
            object itemOne = DataGrid_User.SelectedItem;
            string idfinder;

            try{
                idfinder = (DataGrid_User.SelectedCells[0].Column.GetCellContent(itemOne) as TextBlock).Text;
            }
            catch
            {
                idfinder = "";
            }
            try
            {
               
                SqlCommand Commandcmd = new SqlCommand(
                    "SELECT (UserImage) FROM Users WHERE UserID = " + idfinder,
                    con1);
                SqlDataReader rdr1 = null;
                rdr1 = Commandcmd.ExecuteReader();
                while (rdr1.Read())
                {
                  
                    Images.Fill = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri("/pic/User.jpg", UriKind.Relative))
                    };
                    if (rdr1 != null)
                    {
                        byte[] data = (byte[])rdr1[0]; // 0 is okay if you only selecting one column
                                                       //And use:
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(data))
                        {
                            var imageSource = new BitmapImage();
                            imageSource.BeginInit();
                            imageSource.StreamSource = ms;
                            imageSource.CacheOption = BitmapCacheOption.OnLoad;
                            imageSource.EndInit();

                            // Assign the Source property of your image
                            //  MyImage.Source = imageSource;
                            Images.Fill = new ImageBrush
                            {
                                ImageSource = imageSource
                            };
                        }

                      
                    }


                }
            }
            catch (Exception excepstion)
            {
                Images.Fill = new ImageBrush
                { 
                 //  ImageSource = new BitmapImage(new Uri("/pic/download.png", UriKind.Relative))
                };
            }

            try
            {
                SqlCommand myCommand = new SqlCommand("SELECT (UserEstakhr) FROM Users where UserID = " + idfinder, con1);
                object result = myCommand.ExecuteScalar();
                string estakhrshow = Convert.ToString(result);
                if (estakhrshow == "True")
                {

                    EstakhrIcon.Source = new BitmapImage(new Uri("/pic/newpic/Snap/1.png", UriKind.Relative));

                }
                else
                {
                    EstakhrIcon.Source = new BitmapImage(new Uri("/img/no.png", UriKind.Relative));
                }





                SqlCommand myCommands = new SqlCommand("SELECT (UserBime) FROM Users where UserID = " + idfinder, con1);
                object results = myCommands.ExecuteScalar();
                string estakhrshows = Convert.ToString(results);
                if (estakhrshows == "True")
                {

                    BimeIcon.Source = new BitmapImage(new Uri("/pic/newpic/Snap/1.png", UriKind.Relative));

                }
                else
                {
                    BimeIcon.Source = new BitmapImage(new Uri("/img/no.png", UriKind.Relative));
                }



                SqlCommand myCommandss = new SqlCommand("SELECT (UserKhososi) FROM Users where UserID = " + idfinder, con1);
                object resultss = myCommandss.ExecuteScalar();
                string estakhrshowss = Convert.ToString(resultss);
                if (estakhrshowss == "True")
                {

                    KhososiIcon.Source = new BitmapImage(new Uri("/pic/newpic/Snap/1.png", UriKind.Relative));

                }
                else
                {
                    KhososiIcon.Source = new BitmapImage(new Uri("/img/no.png", UriKind.Relative));
                }


                SqlCommand myCommandsss = new SqlCommand("SELECT (UserBarnameQazaiy) FROM Users where UserID = " + idfinder, con1);
                object resultsss = myCommandsss.ExecuteScalar();
                string estakhrshowsss = Convert.ToString(resultsss);
                if (estakhrshowsss == "True")
                {

                    BarnameIcon.Source = new BitmapImage(new Uri("/pic/newpic/Snap/1.png", UriKind.Relative));

                }
                else
                {
                    BarnameIcon.Source = new BitmapImage(new Uri("/img/no.png", UriKind.Relative));
                }

 
            }
            catch
            {

      

            }
          
            con1.Close();
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void txt_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void txt_family_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void R_Dis_Checked(object sender, RoutedEventArgs e)
        {

            ShowUserInfo(searchstate);


        }

        private void R_Enb_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void R_Enb_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void R_Enb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void R_Enb_Click(object sender, RoutedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddUser AU = new AddUser();
            this.Opacity = 0.2;
            AU.Show();
        }
        private void Show_ChargePage(object sender, RoutedEventArgs e)
        {

            object item = DataGrid_User.SelectedItem;

            PublicVar.GUserName = (DataGrid_User.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
            PublicVar.GUserFamily = (DataGrid_User.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text;
            PublicVar.GUserID = Convert.ToInt32((DataGrid_User.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);

            profile_user w_charge = new profile_user();
            MemberCharge memcharge = new MemberCharge();
            ErrorPage pageerror = new ErrorPage();
            if (DataGrid_User.SelectedItem != null)
            {
                w_charge.memberfamily = (DataGrid_User.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text;
                w_charge.memberid = Convert.ToInt32((DataGrid_User.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                w_charge.membername = (DataGrid_User.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                this.Opacity = 0.2;
                w_charge.Show();
            }
            else
                pageerror.Show();


        }
        private void Show_Entering(object sender, RoutedEventArgs e)
        {
            object itemOne = DataGrid_User.SelectedItem;
            Entering ChargePage = new Entering();
            ErrorPage pageerror = new ErrorPage();
            if (DataGrid_User.SelectedItem != null)
            {
                ChargePage.MemberName = (DataGrid_User.SelectedCells[1].Column.GetCellContent(itemOne) as TextBlock).Text;
                ChargePage.MemberFamily = (DataGrid_User.SelectedCells[2].Column.GetCellContent(itemOne) as TextBlock).Text;
                ChargePage.memberid = Convert.ToInt32((DataGrid_User.SelectedCells[0].Column.GetCellContent(itemOne) as TextBlock).Text);

                ChargePage.Show();
                this.Opacity = 0.2;
            }
            else
                pageerror.Show();
        }

        private void UserInfoShowing(object sender, RoutedEventArgs e)
        {
            object itemOne = DataGrid_User.SelectedItem;
            UserInfo info = new UserInfo();

            this.Opacity = 0.2;

            ErrorPage pageerror = new ErrorPage();
            if (DataGrid_User.SelectedItem != null)
            {
                info.MemberName = (DataGrid_User.SelectedCells[1].Column.GetCellContent(itemOne) as TextBlock).Text;
                info.MemberFamily = (DataGrid_User.SelectedCells[2].Column.GetCellContent(itemOne) as TextBlock).Text;
                info.NewUserID = Convert.ToInt32((DataGrid_User.SelectedCells[0].Column.GetCellContent(itemOne) as TextBlock).Text);

                info.Show();
                this.Opacity = 0.2;
            }
            else
                pageerror.Show();
        }

        private void Show_ReChargePage(object sender, RoutedEventArgs e)
        {

            object itemOne = DataGrid_User.SelectedItem;
          
            MemberCharge ChargePage = new MemberCharge();
            ErrorPage pageerror = new ErrorPage();
            if (DataGrid_User.SelectedItem != null)
            {
                ChargePage.MemberName = (DataGrid_User.SelectedCells[1].Column.GetCellContent(itemOne) as TextBlock).Text;
                ChargePage.MemberFamily = (DataGrid_User.SelectedCells[2].Column.GetCellContent(itemOne) as TextBlock).Text;
                ChargePage.memberid = ((DataGrid_User.SelectedCells[0].Column.GetCellContent(itemOne) as TextBlock).Text);
                ChargePage.Show();
                this.Opacity = 0.2;
            }
            else
                pageerror.Show();


        }
        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            this.Opacity = 1;
        }

      

        private void DataGrid_User_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void R_Dis_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {

            this.Opacity = 1;
        }

        private void Label_Loaded(object sender, RoutedEventArgs e)
        {



            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("SELECT COUNT(UserID) FROM Users", con1);
            object result = myCommand.ExecuteScalar();
            myReader = myCommand.ExecuteReader();
            test1_lb_Copy1.Content = Convert.ToString(result) + " نفر";
            ShowUserInfo(searchstate); con1.Close();
        }

        private void test1_lb_Copy2_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("SELECT COUNT(UserID) FROM Users where UserActive = 1", con1);
            object result = myCommand.ExecuteScalar();
            myReader = myCommand.ExecuteReader();
            test1_lb_Copy2.Content = Convert.ToString(result) + " نفر";
            ShowUserInfo(searchstate); con1.Close();

        }

        private void test1_lb_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("SELECT COUNT(UserID) FROM Users where UserActive = 2", con1);
            object result = myCommand.ExecuteScalar();
            myReader = myCommand.ExecuteReader();
            test1_lb.Content = Convert.ToString(result) + " نفر";
            ShowUserInfo(searchstate);
            con1.Close();
        }

        private void test1_lb_Copy1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UserPerfect u = new UserPerfect();
            u.Show();
        }

        private void test1_lb_Copy2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UserPerfect u = new UserPerfect();
            u.Show();
        }

        private void test1_lb_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            UserPerfect u = new UserPerfect();
            u.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (DataGrid_User.SelectedItem != null)
            {
                try
                {
                    object itemOne = DataGrid_User.SelectedItem;
                    SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
                    DataTable dt = new DataTable();
                    con1.Open();
                    SqlDataReader myReader = null;
                    SqlCommand myCommand = new SqlCommand("Delete FROM VW_Users where UserID = " + (DataGrid_User.SelectedCells[0].Column.GetCellContent(itemOne) as TextBlock).Text, con1);
                    object result = myCommand.ExecuteScalar();
                    myReader = myCommand.ExecuteReader();

                    ShowUserInfo(searchstate);
                }
                catch
                {
                    MessageBox.Show("دوباره تلاش کنید");
                }
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Users w = new Users();
            this.Close();
            w.Show();
        }

        private void Image_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

    

        private void Refresher_MouseEnter(object sender, MouseEventArgs e)
        {
            Refresher.Source = new BitmapImage(new Uri("/img/refresh-flat.png", UriKind.Relative));
        }

        private void Refresher_MouseLeave(object sender, MouseEventArgs e)
        {
            Refresher.Source = new BitmapImage(new Uri("/img/Refresh.png", UriKind.Relative));
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Ext_btn.Source = new BitmapImage(new Uri("/img/exit.png", UriKind.Relative));
        }

        private void DataGrid_User_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

          
                object itemOne = DataGrid_User.SelectedItem;
                // دریافت سن
                string GenderFinder = (DataGrid_User.SelectedCells[6].Column.GetCellContent(itemOne) as TextBlock).Text;
                FoodList f = new FoodList();
                //دریافت جنسیت
                f.age = Convert.ToDouble((DataGrid_User.SelectedCells[5].Column.GetCellContent(itemOne) as TextBlock).Text);



                if (GenderFinder == "اقا")
                {
                    f.gender = 0;
                }
                if (GenderFinder == "خانم")
                {
                    f.gender = 1;
                }




                // دریافت وزن ورزشکار

                SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
                con1.Open();
                int UserID = Convert.ToInt32((DataGrid_User.SelectedCells[0].Column.GetCellContent(itemOne) as TextBlock).Text);
                SqlCommand myCommand = new SqlCommand("select (userSize) from users where UserID = " + UserID, con1);
                object result = myCommand.ExecuteScalar();
                string weight = Convert.ToString(result);
                f.weight = int.Parse(weight);



                // دریافت قد ورزشکار


                SqlCommand myCommands = new SqlCommand("select (userSize) from users where UserID = " + UserID, con1);
                object heightresult = myCommands.ExecuteScalar();
                string height = Convert.ToString(heightresult);
                f.height = int.Parse(height);
                f.NewUserID = UserID;
                f.Show();
                con1.Close();

            



        }
    }
}
