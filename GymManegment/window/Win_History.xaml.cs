using Arash;
using DataModelLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GymManegment.Module;
namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for Win_History.xaml
    /// </summary>
    public partial class Win_History : Window
    {
        public Win_History()
        {
            InitializeComponent();
        }

        gymEntities2 database = new gymEntities2();

        SqlConnection con1 =
            new SqlConnection(
                PublicVar.ConnectionString);



















        private void OutPut(object sender, RoutedEventArgs e)
        {
            string time = DateTime.Now.ToString("HH:mm");


            object itemOne = DataGrid_User.SelectedItem;
            SqlConnection con1 =
                new SqlConnection(
                    PublicVar.ConnectionString);
            con1.Open();








            if (DataGrid_User.SelectedItem != null)
            {
                SqlCommand myCommandss =
                    new SqlCommand(
                        "UPDATE Inv set InvOutDate = '" + time + "' Where InvID = " +
                        (DataGrid_User.SelectedCells[6].Column.GetCellContent(itemOne) as TextBlock).Text, con1);
                myCommandss.ExecuteScalar();

                ShowUserInfo(searchstate);
            }



        }




        private void txt_name_TextChanged(object sender, TextChangedEventArgs e)
        {



            if (txt_id.Text != "")
            {


                try
                {

                    DataTable dt = new DataTable();
                    try
                    {
                        SqlCommand Commandcmd = new SqlCommand(
                            "SELECT (UserImage) FROM Users WHERE UserID = " + txt_id.Text.Trim(),
                            con1);
                        SqlDataReader rdr1 = null;
                        rdr1 = Commandcmd.ExecuteReader();
                        MyImage.Source = new BitmapImage(new Uri("/pic/users.jpg", UriKind.Relative));




                        while (rdr1.Read())
                        {
                            MyImage.Source = new BitmapImage(new Uri("/pic/user.jpg", UriKind.Relative));
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
                                    MyImage.Source = imageSource;

                                }


                            }


                        }


                    }
                    catch (Exception exception)
                    {
                        MyImage.Source = new BitmapImage(new Uri("/pic/User.jpg", UriKind.Relative));

                    }














                    //SqlCommand myCommand = new SqlCommand("Select sum(UserBedehiRial) from Members where MemberFindID = " + txt_id.Text.Trim(), con1);
                    //object result = myCommand.ExecuteScalar();
                    //Days.Content = Convert.ToString(result) + " تومان";


                    SqlCommand myCommandss =
                        new SqlCommand(
                            "Select DISTINCT (UserName + ' ' + UserFamily) from VW_Users where UserID = " +
                            txt_id.Text.Trim(), con1);
                    object resultss = myCommandss.ExecuteScalar();
                    Name.Content = Convert.ToString(resultss) + "";





                    SqlCommand GenderCommands =
                        new SqlCommand(
                            "Select DISTINCT (UserGenderFarsi) from VW_Users where UserID = " + txt_id.Text.Trim(),
                            con1);
                    object GenderRes = GenderCommands.ExecuteScalar();
                    Gender.Content = Convert.ToString(GenderRes) + "";


                    SqlCommand ActiveCommands =
                        new SqlCommand(
                            "Select DISTINCT (UserActive) from VW_Users where UserID = " + txt_id.Text.Trim(), con1);
                    object ActiveRes = ActiveCommands.ExecuteScalar();

                    SqlCommand DaysCommands =
                        new SqlCommand(
                            "Select Max(MemberLastCharge) from VW_Member where MemberFindID  = " + txt_id.Text.Trim(),
                            con1);
                    object DaysRes = DaysCommands.ExecuteScalar();
                    Days.Content = Convert.ToString(DaysRes) + "";



                    int a = Convert.ToInt16(ActiveRes);





                    switch (a)
                    {
                        case 1:
                            Active.Content = "فعال";
                            Active.Foreground = Brushes.Green;
                            break;


                        case 2:
                            Active.Content = "غیر فعال";
                            Active.Foreground = Brushes.Red;
                            break;

                        default:
                            Active.Content = "کاربر یافت نشد";
                            Active.Foreground = Brushes.White;
                            break;
                    }




                }
                catch (Exception exception)
                {

                    MessageBox.Show(exception.ToString());
                }
            }



        }


        private void DataGrid_User_MouseMove(object sender, MouseEventArgs e)
        {
            DataGrid_User.Opacity = 1;
        }

        private void DataGrid_User_MouseLeave(object sender, MouseEventArgs e)
        {
            DataGrid_User.Opacity = 0.8;
        }

        private void newuser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void txt_id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            DragMove();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }






        private void newuser_MouseDown(object sender, MouseButtonEventArgs e)
        {

            string time = DateTime.Now.ToString("HH:mm");
            Inv U = new Inv();
            SucFul S = new SucFul();
            ErrorPage E = new ErrorPage();


        }

        private void txt_id_serch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Cal_Az.SelectedDate = PersianDate.Today.AddDays(-30);
            ShowUserInfo(searchstate);
            Days_C.Content = EnterDate.Text;
        }

        private void Txt_Komod_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        string ImageName = "";
        string StrName = "";

        private void MyImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {



        }

        private void ShowUserInfo(Func<string> SearchStringForUser)
        {
            var query = database.Database.SqlQuery<InvView>(
                "select * From InvView where 1 = 1 " + SearchStringForUser() + "   And InvOutDate != '1' ORDER BY InvDate");
            //       MessageBox.Show(query.ToString());
            var count = database.Database.SqlQuery<InvView>(
                "select Count(UserID) From InvView where 1 = 1 " + SearchStringForUser() + "   And InvOutDate != '1' ORDER BY InvDate");
            //   MessageBox.Show(query.ToString());
            var u = query.ToList();
            DataGrid_User.ItemsSource = u;

            //SqlCommand calcsz =
            //    new SqlCommand(
            //        "Select count(InvID) from Inv Where InvDate = '" +
            //        String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(EnterDate.Text)) + "'"
            //        , con1);
            //object calc = calcsz.ExecuteScalar();
            //Calcs.Content = Convert.ToString(calc) + " نفر";
            SerchLine();
        }



        private string searchstate()
        {

            string searchstring = " and InvDate between '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Cal_Az.Text)) + "' and '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Cal_Ta.Text)) + "'";
            if (txt_id_Copy.Text != "")
            {
                searchstring += " And FullName like '%" + txt_id_Copy.Text.Trim() + "%'";
            }
            if (txt_id_Copy1.Text != "")
            {
                searchstring += " And UserID like '%" + txt_id_Copy1.Text.Trim() + "%'";
                SerchLine();
            }

            if (txt_id_Copy3.Text != "")
            {
                searchstring += " And InvCount = " + txt_id_Copy3.Text.Trim() + "";
                SerchLine();
            }
            if (GenderChoicer.SelectedIndex == 0)
            {
                searchstring += "";
                SerchLine();
            }
            if (GenderChoicer.SelectedIndex == 1)
            {
                searchstring += " And UserGender = 1 ";
                SerchLine();
            }
            if (GenderChoicer.SelectedIndex == 2)
            {
                searchstring += " And UserGender = 2 ";
                SerchLine();
            }

            return searchstring;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            con1.Open();



            ShowUserInfo(searchstate);













        }

        private void newuser_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {


        }

        private void newuser_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void newuser_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DataGrid_User.Columns[9].Visibility = Visibility.Hidden;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            DataGrid_User.Columns[9].Visibility = Visibility.Visible;

        }

        private void DataGrid_User_MouseDown(object sender, MouseButtonEventArgs e)
        {




        }

        private void DataGrid_User_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            
        }

        private void txt_id_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
            SerchLine();
        }

        private void txt_id_Copy1_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
            SerchLine();
        }

        private void Image_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            Win_History p = new Win_History();
            Close();
            p.Show();
        }

        private void Cal_Az_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            ShowUserInfo(searchstate);
            SerchLine();
        }

        private void GenderChoicer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
            SerchLine();
        }

        private void Cal_Ta_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            SerchLine();
            ShowUserInfo(searchstate);
        }
        private void SerchLine()
        {
            for (int i = 0; i < DataGrid_User.Items.Count; ++i) // Only display the 3 column added in the code behind.
                Calcs.Content = i;
        }
        private void Calcs_Loaded(object sender, RoutedEventArgs e)
        {
            SerchLine();
        }

        private void DataGrid_User_AutoGeneratedColumns(object sender, EventArgs e)
        {
            SerchLine();
        }
    }
}