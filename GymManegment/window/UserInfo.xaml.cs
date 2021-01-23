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
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.IO;
using System.Drawing.Imaging;
using System.Data.SqlClient;
using System.Data;
using GymManegment.Module;

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Window
    {
        public UserInfo()
        {
            InitializeComponent();
        }
        gymEntities2 database = new gymEntities2();

        public int NewUserID;
        string ImageName = "";
        string StrName = "";
        public string MemberName = "";
        public string MemberFamily = "";
        public byte cheaker;


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

        private void ShowMemberInfo(Func<string> SearchStringPrice)
        {
            var query = database.Database.SqlQuery<Members>("select * From VW_Users where 1 = 1 and UserID = " + NewUserID + " " + SearchStringPrice());
            //    MessageBox.Show(query.ToString());
            //    var user = query.ToList();
            //    DataGrid_Members.ItemsSource = user;

        }





        //تابع ساخت شرط برای نمایش اطلاعات در گرید ویو
        private void ShowUserInfo(Func<string> SearchStringForUser)
        {

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

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {



        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {


            Username.Focus();

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (cheaker == 1)
            {
                UserGhad.Focus();
            }

            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlDataReader myReader = null;


            SqlCommand myCommand = new SqlCommand("select * From VW_Users where 1 = 1 and UserID = " + NewUserID, con1);



            myReader = myCommand.ExecuteReader();
            try
            {
                newuser.IsEnabled = false;
                SqlCommand Commandcmd = new SqlCommand(
                    "SELECT (UserImage) FROM Users WHERE UserID = " + NewUserID,
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
                            newuser.IsEnabled = true;
                        }
                    }


                }


            }
            catch (Exception exception)
            {
                MyImage.Source = new BitmapImage(new Uri("/pic/User.jpg", UriKind.Relative));
                newuser.IsEnabled = true;
            }
            while (myReader.Read())
            {


                Username.Text = (myReader["UserName"].ToString());
                UserFamily.Text = (myReader["UserFamily"].ToString());
                UserTel.Text = (myReader["UserTel"].ToString());
                UserAge.Text = (myReader["UserAge"].ToString());
                UserCode.Text = (myReader["CodeMeli"].ToString());
                UserHomePhone.Text = (myReader["HomePhone"].ToString());
                UserCodePost.Text = (myReader["UserPostCode"].ToString());
                //  UserCodePost.Text = (myReader["UserActive"].ToString());


                if ((myReader["UserGenderFarsi"].ToString()) == "اقا")
                {
                    rdb_man.IsChecked = true;
                }
                if ((myReader["UserGenderFarsi"].ToString()) == "خانم")
                {
                    rdb_woman.IsChecked = true;
                }

                if ((myReader["UserTamrinFarsi"].ToString()) == "هوازي")
                {
                    NoeVarzesh.SelectedIndex = 0;
                }



                if ((myReader["UserTamrinFarsi"].ToString()) == "غير هوازي")
                {
                    NoeVarzesh.SelectedIndex = 1;
                }


                TarikhTextbox.Text = (myReader["UserRegDate"].ToString());
                UserLocation.Text = (myReader["UserAddress"].ToString());
                UserBazu.Text = (myReader["UserDoreBazo"].ToString());
                UserDoreSine.Text = (myReader["UserDoreSine"].ToString());
                UserGhad.Text = (myReader["UserGhaad"].ToString());
                UserVazn.Text = (myReader["userSize"].ToString());
                UserKamar.Text = (myReader["UserDoreKamar"].ToString());
                Komod.Text = (myReader["UsersKomod"].ToString());
                //        MyImage.Source = (myReader["UserImage"].ToString());
                //    MyImage.Source = (myReader["UserFactor"];






            }
      
            con1.Close();

        }
        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void txt_username_TextChanged(object sender, TextChangedEventArgs e)
        {
            //     txt_username.Text = MemberName + " " + MemberFamily;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void txt_tarikh_TextInput(object sender, TextCompositionEventArgs e)
        {

        }



        private void UserTel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //عبارات با قاعده برای عدد
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UserAge_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UserCode_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void UserCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UserHomePhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UserCodePost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UserHomePhone_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Kilid_Checked(object sender, RoutedEventArgs e)
        {


        }



        private void Kilid_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserDoreSine_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UserKamar_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UserGhad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UserVazn_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UserBazu_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private string searchstate()
        {

            string searchstring = "";


            return searchstring;
        }
        private bool Cheakable()
        {
            if (Username.Text == "")
            {
                ErrorPage ER = new ErrorPage();
                ER.Show();
                ER.Error_Lable.Content = "نام ورزشکار را وارد کنید";
                Username.Focus();
                return false;
            }
            if (UserFamily.Text == "")
            {
                ErrorPage ER = new ErrorPage();
                ER.Show();

                UserFamily.Focus();
                return false;
            }
            if (UserTel.Text == "")
            {
                ErrorPage ER = new ErrorPage();
                ER.Show();

                UserTel.Focus();
                return false;
            }
            if (UserFamily.Text == "")
            {
                ErrorPage ER = new ErrorPage();
                ER.Show();

                UserFamily.Focus();
                return false;
            }
            if (UserAge.Text == "")
            {
                ErrorPage ER = new ErrorPage();
                ER.Show();

                UserAge.Focus();
                return false;
            }
            if (UserLocation.Text == "")
            {
                ErrorPage ER = new ErrorPage();
                ER.Show();
                UserLocation.Focus();
                return false;
            }


            return true;
        }













        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show(e.ExceptionObject.ToString());
        }
        private void newuser_Click(object sender, RoutedEventArgs e)
        {

            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();


            if (ImageName != "")
            {
                FileStream fs = new FileStream(ImageName, FileMode.Open, FileAccess.Read);
                byte[] imgByteArr = new byte[fs.Length];
                fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                //SqlCommand myCommand = new SqlCommand("update Users set UserImage = '" + imgByteArr + "' Where UserID = " + NewUserID, con1);
                //object myReader = myCommand.ExecuteReader();

                string command =
                    "UPDATE Users SET UserImage = @img , UserName = @username , UserFamily = @userfamily , UserAge = @userage" +
                    " , UserTel = @usertel , UserGender = @usergender , UserAddress = @useraddress , CodeMeli = @codemeli, " +
                    " UsersKomod = @userskomod  , HomePhone = @homephone , UserPostCode = @userpostcode ,  userGhaad = @userghaad , userSize = @usersize where UserID = @id ";
                using (SqlConnection sqlConnectionCmdString = new SqlConnection(PublicVar.ConnectionString))
                using (SqlCommand sqlRenameCommand = new SqlCommand(command, sqlConnectionCmdString))
                {

                    sqlRenameCommand.Parameters.Add(new SqlParameter("@id", NewUserID));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@img", imgByteArr));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@username", Username.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@userfamily", UserFamily.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@userage", UserAge.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@usertel", UserTel.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@useraddress", UserLocation.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@codemeli", UserCode.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@userskomod", Komod.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@homephone", UserHomePhone.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@userpostcode", UserCodePost.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@userghaad", UserGhad.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@usersize", UserVazn.Text));
                    if (rdb_man.IsChecked == true)
                    {
                        sqlRenameCommand.Parameters.Add(new SqlParameter("@usergender", 1));
                    }
                    if (rdb_woman.IsChecked == true)
                    {
                        sqlRenameCommand.Parameters.Add(new SqlParameter("@usergender", 2));
                    }


                    sqlConnectionCmdString.Open();
                    sqlRenameCommand.ExecuteNonQuery();
                    MessageBox.Show("اطلاعات با موفقیت به روز رسانی شد", "ویرایش ورزشکار");
                    con1.Close();
                }
              
            }
            if (ImageName == "")
            {

                string command =
                    "UPDATE Users SET  UserName = @username , UserFamily = @userfamily , UserAge = @userage" +
                    " , UserTel = @usertel , UserGender = @usergender , UserAddress = @useraddress , CodeMeli = @codemeli, " +
                    " UsersKomod = @userskomod  , HomePhone = @homephone , UserPostCode = @userpostcode  , userGhaad = @userghaad , userSize = @usersize where UserID = @id ";
                using (SqlConnection sqlConnectionCmdString = new SqlConnection(PublicVar.ConnectionString))
                using (SqlCommand sqlRenameCommand = new SqlCommand(command, sqlConnectionCmdString))
                {

                    sqlRenameCommand.Parameters.Add(new SqlParameter("@id", NewUserID));
                
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@username", Username.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@userfamily", UserFamily.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@userage", UserAge.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@usertel", UserTel.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@useraddress", UserLocation.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@codemeli", UserCode.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@userskomod", Komod.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@homephone", UserHomePhone.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@userpostcode", UserCodePost.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@userghaad", UserGhad.Text));
                    sqlRenameCommand.Parameters.Add(new SqlParameter("@usersize", UserVazn.Text));
                    if (rdb_man.IsChecked == true)
                    {
                        sqlRenameCommand.Parameters.Add(new SqlParameter("@usergender", 1));
                    }
                    if (rdb_woman.IsChecked == true)
                    {
                        sqlRenameCommand.Parameters.Add(new SqlParameter("@usergender", 2));
                    }


                    sqlConnectionCmdString.Open();
                    sqlRenameCommand.ExecuteNonQuery();
                    MessageBox.Show("اطلاعات با موفقیت به روز رسانی شد", "ویرایش ورزشکار");
                    con1.Close();
                }

                if (cheaker == 1)
                {
                    System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("آیا میخواهید به صفحه ی صدور غذا بروید؟", "صدور غذا", System.Windows.Forms.MessageBoxButtons.YesNo);
                    if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        FoodList f = new FoodList();
                      
                      f.height = Convert.ToInt32(UserGhad.Text);
                       f.weight = Convert.ToInt32(UserVazn.Text);
                       f.age = Convert.ToInt32(UserAge.Text);
                        if (rdb_man.IsChecked == true)
                        {
                            f.gender = 0;
                        }
                        else if(rdb_woman.IsChecked == true)
                            f.gender = 1;
                        f.NewUserID =NewUserID;
                        f.Show();

                    }
                 
                }
                this.Close();

            }






        }

        private void Tarikh_SelectedDateChanged_1(object sender, RoutedEventArgs e)
        {

            //        txt_tarikh.Text= String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Tarikh.Text));
        }

        private void txt_tarikh_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txt_tarikh_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                OpenFileDialog dialog = new OpenFileDialog
                {
                    CheckFileExists = true,
                    Multiselect = false,
                    Filter = "Images (*.jpg,*.png)|*.jpg;*.png|All Files(*.*)|*.*"
                };

                dialog.ShowDialog();
                {
                    StrName = dialog.SafeFileName;
                    ImageName = dialog.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    if (ImageName != "")
                    {
                        MyImage.SetValue(Image.SourceProperty, isc.ConvertFromString(ImageName));

                    }
                }
            }
            catch
            {
                ErrorPage Er = new ErrorPage();
                Er.Show();
                Er.Error_Lable.Content = "مشکلی در سیستم به وجود آمده است";
            }
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {




            // Display the loaded image


        }

        private void ImagePath_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}



