using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
using DataModelLayer;
using System.Drawing;
using System.Linq.Expressions;
using Microsoft.Win32;
using GymManegment.Module;

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for ControlPanel.xaml
    /// </summary>
    public partial class ControlPanel : Window
    {
        public ControlPanel()
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


                        SqlCommand Commandcmds = new SqlCommand(
                           "SELECT (UsersKomod) FROM Users WHERE UserID = " + txt_id.Text.Trim(),
                           con1);
                        object rdr11 = Commandcmds.ExecuteScalar();
                        string komodfinder = Convert.ToString(rdr11);
                        if (komodfinder != "")
                        {
                            Txt_Komod.Text = komodfinder;
                        }
                        else
                        {
                            Txt_Komod.Text = "";
                        }
                     


                

                        newuser.IsEnabled = false;

                        MyImage.Source = new BitmapImage(new Uri("/pic/users.jpg", UriKind.Relative));
                        while (rdr1.Read())
                        {
                            newuser.IsEnabled = true;
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

                        newuser.IsEnabled = true;

                    }














                    //SqlCommand myCommand = new SqlCommand("Select sum(UserBedehiRial) from Members where MemberFindID = " + txt_id.Text.Trim(), con1);
                    //object result = myCommand.ExecuteScalar();
                    //Days.Content = Convert.ToString(result) + " ریال";


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
                            //  Active.Foreground = Brushes.Green;
                            break;


                        case 2:
                            Active.Content = "غیر فعال";
                            //            Active.Foreground = Brushes.Red;
                            break;

                        default:
                            Active.Content = "کاربر یافت نشد";
                            //       Active.Foreground = Brushes.White;
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
            DataGrid_User.Opacity = 0.9;
        }

        private void newuser_Click(object sender, RoutedEventArgs e)
        {
            string time = DateTime.Now.ToString("HH:mm");
            Inv U = new Inv();
            SucFul S = new SucFul();
            ErrorPage E = new ErrorPage();


            string ClosedID = Txt_Komod.Text.ToString();


            try
            {

                U.UserID = Convert.ToInt16(txt_id.Text);
                U.InvEnterDate = time;
                U.InvOutDate = "1";
                if (Txt_Komod.Text.Trim() != "")
                {
                    U.InvCount = Convert.ToInt32(Txt_Komod.Text);
                }
                else if (Txt_Komod.Text.Trim() == "")
                {
                    U.InvCount = 1;
                }
                U.InvDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(EnterDate.Text));





                database.Inv.Add(U);
                database.SaveChanges();




                ShowUserInfo(searchstate);











            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
            finally
            {
                txt_id.Focus();
                txt_id.Text = "";
                Txt_Komod.Text = "";
            }
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

            


        }

        private void txt_id_serch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
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
                "select * From InvView where 1 = 1 " + SearchStringForUser());
            //      MessageBox.Show(query.ToString());
            var u = query.ToList();
            DataGrid_User.ItemsSource = u;
            SqlCommand calcsz =
                new SqlCommand(
                    "Select count(InvID) from Inv Where InvDate = '" +
                    String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(EnterDate.Text)) + "'"
                    , con1);
            object calc = calcsz.ExecuteScalar();
            Calcs.Content = Convert.ToString(calc) + " نفر";
        }


        private string searchstate()
        {

            string searchstring = " And InvDate = '" +
                                  String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(EnterDate.Text)) +
                                  "' And InvOutDate = '1'";




            if (txt_id_Copy.Text != "")
            {
                searchstring += " And FullName like '%" + txt_id_Copy.Text.Trim() + "%'";
            }
            if (txt_id_Copy1.Text != "")
            {
                searchstring += " And UserID like '%" + txt_id_Copy1.Text.Trim() + "'";
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
            DataGrid_User.Columns[8].Visibility = Visibility.Hidden;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            DataGrid_User.Columns[8].Visibility = Visibility.Visible;

        }

        private void DataGrid_User_MouseDown(object sender, MouseButtonEventArgs e)
        {




        }

        private void DataGrid_User_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            object itemOne = DataGrid_User.SelectedItem;

            if (DataGrid_User.SelectedItem != null)
            {
                SqlCommand myCommandss =
                    new SqlCommand(
                        "Select DISTINCT (UserName + ' ' + UserFamily) from VW_Users where UserID = " +
                        (DataGrid_User.SelectedCells[0].Column.GetCellContent(itemOne) as TextBlock).Text, con1);
                object resultss = myCommandss.ExecuteScalar();
                Name.Content = Convert.ToString(resultss) + "";


                SqlCommand GenderCommands =
                    new SqlCommand(
                        "Select DISTINCT (UserGenderFarsi) from VW_Users where UserID = " +
                        (DataGrid_User.SelectedCells[0].Column.GetCellContent(itemOne) as TextBlock).Text,
                        con1);
                object GenderRes = GenderCommands.ExecuteScalar();
                Gender.Content = Convert.ToString(GenderRes) + "";


                SqlCommand ActiveCommands =
                    new SqlCommand(
                        "Select DISTINCT (UserActive) from VW_Users where UserID = " +
                        (DataGrid_User.SelectedCells[0].Column.GetCellContent(itemOne) as TextBlock).Text, con1);
                object ActiveRes = ActiveCommands.ExecuteScalar();

                SqlCommand DaysCommands =
                    new SqlCommand(
                        "Select Max(MemberLastCharge) from VW_Member where MemberFindID  = " +
                        (DataGrid_User.SelectedCells[0].Column.GetCellContent(itemOne) as TextBlock).Text,
                        con1);
                object DaysRes = DaysCommands.ExecuteScalar();
                Days.Content = Convert.ToString(DaysRes) + "";



                int a = Convert.ToInt16(ActiveRes);

                try
                {

                    SqlCommand Commandcmd = new SqlCommand(
                        "SELECT (UserImage) FROM Users WHERE UserID = " + (DataGrid_User.SelectedCells[0].Column.GetCellContent(itemOne) as TextBlock).Text,
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




                switch (a)
                {
                    case 1:
                        Active.Content = "فعال";
                        Active.Foreground = System.Windows.Media.Brushes.Green;
                        break;


                    case 2:
                        Active.Content = "غیر فعال";
                        Active.Foreground = System.Windows.Media.Brushes.Red;
                        break;

                    default:
                        Active.Content = "کاربر یافت نشد";
                        Active.Foreground = System.Windows.Media.Brushes.White;
                        break;
                }

            }
        }

        private void txt_id_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void txt_id_Copy1_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void DataGrid_User_LoadingRow(object sender, DataGridRowEventArgs e)
        {
        }

        private void DataGrid_User_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
        
        }

        private void txt_id_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
