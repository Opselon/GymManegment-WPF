using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DataModelLayer;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.IO;
using GymManegment.Module;
using System.Data.SqlClient;
using Arash;

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public AddUser()
        {
            InitializeComponent();
        }
        gymEntities2 database = new gymEntities2();
        
        string ImageName = "";
        string StrName= "";
        public string MemberName = "";
        public string MemberFamily = "";
   


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
            //   var query = database.Database.SqlQuery<MemberInfo>("select * From MemberInfo where 1 = 1 and MemberFindID = " + memberid + " " + SearchStringPrice());
            // MessageBox.Show(query.ToString());
            //     var user = query.ToList();

            //DataGrid_Members.ItemsSource = user;

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

  

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {

            //cAccCreditDate.SelectedDateTime = DateTime.Now; 
            Username.Focus();
            Random rnd = new Random();
            int card = rnd.Next(100000000, 999999999);
            UserFactor.Text = Convert.ToString(card);
        }


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

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
            if (Kilid.IsChecked == true)
            {
                Komod.Visibility = Visibility.Visible;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Kilid_Click(object sender, RoutedEventArgs e)
        {
            if (Kilid.IsChecked == false)
            {
                Komod.Visibility = Visibility.Hidden;
                Komod.Text = "";
            }
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
                var bc = new BrushConverter();
                Username.Background = System.Windows.Media.Brushes.Red;
                Username.Focus();
                return false;
            }
            if (UserFamily.Text == "")
            {
                var bc = new BrushConverter();
                UserFamily.Background = System.Windows.Media.Brushes.Red;
                UserFamily.Focus();
                return false;
            }
            if (UserTel.Text == "")
            {
                var bc = new BrushConverter();
                UserTel.BorderBrush = System.Windows.Media.Brushes.Red;
                UserTel.Focus();
                return false;
            }
           
            if (UserAge.Text == "")
            {
                var bc = new BrushConverter();
                UserAge.Background = System.Windows.Media.Brushes.Red;
                UserAge.Focus();
                return false;
            }
            if (UserLocation.Text == "")
            {
                var bc = new BrushConverter();
                UserLocation.Background = System.Windows.Media.Brushes.Red;



                UserLocation.Focus();
                return false;
            }


            return true;
        }





        private void newuser_Click(object sender, RoutedEventArgs e)
        {
            Form2 F1 = new Form2();
            if (!Cheakable())
            {
                return;
            }
            try
            {

                int mount = Taghvim.SelectedDate.Month;
                int day = Taghvim.SelectedDate.Day;
                int year = Taghvim.SelectedDate.Year;

                string Year2 = year + "/" + mount + "/" + day;


                if (Hanis.SelectedIndex == 0)
                {
                    PayaneCharge.SelectedDate = PersianDate.Today.AddDays(30);
                   
                    Year2 = year + "/" + (mount + 1) + "/" + day;
                }
                else if (Hanis.SelectedIndex == 1)
                {
                    PayaneCharge.SelectedDate = PersianDate.Today.AddDays(60);
                    Year2 = year + "/" + (mount + 2) + "/" + day;
                }
                else if (Hanis.SelectedIndex == 2)
                {
                    PayaneCharge.SelectedDate = PersianDate.Today.AddDays(90);
                    Year2 = year + "/" + (mount + 3) + "/" + day;
                }
                else if (Hanis.SelectedIndex == 3)
                {
                    PayaneCharge.SelectedDate = PersianDate.Today.AddDays(120);
                
                    Year2 = year + "/" + (mount + 4) + "/" + day;
                }
                else if (Hanis.SelectedIndex == 4)
                {
                    Year2 = year + "/" + (mount + 6) + "/" + day;
                    PayaneCharge.SelectedDate = PersianDate.Today.AddDays(180);
                }
                else if (Hanis.SelectedIndex == 5)
                {
                    Year2 = (year + 1) + "/" + mount + "/" + day;
                    PayaneCharge.SelectedDate = PersianDate.Today.AddDays(360);
                }






                SHA256CryptoServiceProvider Sha2 = new SHA256CryptoServiceProvider();


                byte[] B1;
                byte[] B2;
                B1 = UTF8Encoding.UTF8.GetBytes(UserPassword.Text);
                B2 = Sha2.ComputeHash(B1);
                string UserPasswordHashed = BitConverter.ToString(B2);
      
                VW_Users U = new VW_Users();
                byte estakhr, bime, khososi, barnameqazaiy;
                if (EstakhrCheakBox.IsChecked == true)
                {
                    estakhr = 1;
                }
                else
                {
                    estakhr = 0;
                }

                if (BimeCheakBox.IsChecked == true)
                {
                    bime = 1;
                }
                else
                {
                    bime = 0;
                }

                if(KhososiCheakBox.IsChecked == true)
                {
                    khososi = 1;
                }
                else
                {
                    khososi = 0;
                }
                if(BarnameQazaiCheakBox.IsChecked == true)
                {
                    barnameqazaiy = 1;
                }
                else
                {
                    barnameqazaiy = 0;
                }

                if (NoeVarzesh.SelectedIndex == 0)
                {
                    U.UserKarKard = 1;
                }
                else
                    U.UserKarKard = 2;
                if (UserTamrin.SelectedIndex == 0)
                {
                    U.UserTamrin = 1;
                }
                else
                {
                    U.UserKarKard = 2;
                }
                if (rdb_man.IsChecked == true)
                {
                    U.UserGender = 1;
                }
                else
                {

                    U.UserGender = 2;
                }
                if (ImageName != "")
                {
                    FileStream fs = new FileStream(ImageName, FileMode.Open, FileAccess.Read);
                    byte[] imgByteArr = new byte[fs.Length];
                    fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    database.Sp_gym(Username.Text.Trim(), UserFamily.Text.Trim(), UserTel.Text.Trim(), UserUsername.Text.Trim(),
                        UserPasswordHashed.ToString(), Convert.ToByte(UserAge.Text.Trim()), 1, U.UserGender, UserCode.Text.Trim(), UserLocation.Text.Trim(), UserHomePhone.Text.Trim(),
                        UserDoreSine.Text.Trim(), UserBazu.Text.Trim(), UserKamar.Text.Trim(), UserGhad.Text.Trim(), UserVazn.Text.Trim(), U.UserTamrin, U.UserKarKard, U.UserShahrie, String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Year2)),
                        " ", UserCodePost.Text.Trim(), /*cAccCreditDate.Text.ToString()*/null, UserCharge.Text.Trim(), Convert.ToDecimal(UserCharge.Text.Trim()), 1, Komod.Text, imgByteArr,Convert.ToBoolean(estakhr), Convert.ToBoolean(bime), Convert.ToBoolean(khososi), Convert.ToBoolean(barnameqazaiy));
     


                    database.SaveChanges();
                    SucFul ER = new SucFul();
                    ER.Show();
                    ER.Suclbl.Content = "کاربر جدید ثبت شد";






                }

                else
                {
                    
                    
                    database.Sp_gym(Username.Text.Trim(), UserFamily.Text.Trim(), UserTel.Text.Trim(), UserUsername.Text.Trim(),
                        UserPasswordHashed.ToString(), Convert.ToByte(UserAge.Text.Trim()), 1, U.UserGender, UserCode.Text.Trim(), UserLocation.Text.Trim(), UserHomePhone.Text.Trim(),
                        UserDoreSine.Text.Trim(), UserBazu.Text.Trim(), UserKamar.Text.Trim(), UserGhad.Text.Trim(), UserVazn.Text.Trim(), U.UserTamrin, U.UserKarKard, U.UserShahrie, String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Year2)),
                        " ", UserCodePost.Text.Trim(), /*cAccCreditDate.Text.ToString()*/null, UserCharge.Text.Trim(), Convert.ToDecimal(UserCharge.Text.Trim()), 1, Komod.Text.Trim(),  null, Convert.ToBoolean(estakhr), Convert.ToBoolean(bime), Convert.ToBoolean(khososi), Convert.ToBoolean(barnameqazaiy));
                    database.SaveChanges();
                  
                    SucFul ER = new SucFul();
                    ER.Show();
                    ER.Suclbl.Content = "کاربر جدید ثبت شد";
                }



              
                    Members Us = new Members();
                    Submit Sub = new Submit();


                    LinkBedehi l = new LinkBedehi();


                SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
                con1.Open();
                SqlCommand FactorCode = new SqlCommand("select MAX(UserID) FROM Users", con1);
                object FactorCodeObj = FactorCode.ExecuteScalar();
                string FactorCodeString = Convert.ToString(FactorCodeObj);

                   Us.MemberFactorCode = Convert.ToInt64(UserFactor.Text.Trim());
                    Us.MemberFindID = Convert.ToInt32(FactorCodeString);
                    Us.MemberStartDate = PublicVar.TodayTime;
                    Us.MemberLastCharge = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Year2));









                    int timeInFactor = DateTimeHidden.SelectedDate.Month;
                    int payanTime = PayaneCharge.SelectedDate.Month;
                    Us.MemberMounth = Convert.ToByte(timeInFactor);
                    Us.MemberMountTwo = Convert.ToByte(payanTime);



                    Us.MemberCharge = Convert.ToDecimal(UserCharge.Text.Trim());
                    Us.UserBedehiRial = 0;
                     Us.BedehKariDesc = "ثبت نام";
                   // Us.MemberService = Convert.ToByte(bedehs);



                    database.Members.Add(Us);
               


                UsersTimeOut T = new UsersTimeOut();
               
                T.UserFindID = Convert.ToInt32(FactorCodeString);
                T.UserEndDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Year2));

                database.UsersTimeOut.Add(T);
                database.SaveChanges();
            }  


            catch (Exception ex)
            {
                ErrorPage ER = new ErrorPage();
                ER.Show();
                ER.Error_Lable.Content = ex.ToString();

            }
            finally
            {
                this.Close();
                ShowUserInfo(searchstate);
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
                        MyImage.SetValue(System.Windows.Controls.Image.SourceProperty, isc.ConvertFromString(ImageName));
                        ImagePath.Text = ImageName.ToString();
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

        private void UserLocation_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cAccCreditDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Random rnd = new Random();
            int card = rnd.Next(100000000, 999999999);
            UserFactor.Text = Convert.ToString(card);
        }

        private void TextBoxFactor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
       

            Random rnd = new Random();
            int card = rnd.Next(100000000, 999999999);
            UserFactor.Text = Convert.ToString(card);
        }

        private void Hanis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

        }

        private void UserCharge_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
             

                SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
                con1.Open();



                SqlCommand PriceFinder = new SqlCommand("SELECT (UserCharge) FROM Users WHERE UserID = (SELECT MAX(UserID) FROM Users)", con1);
                object PriceFinderObj = PriceFinder.ExecuteScalar();
                string PriceFinderString = Convert.ToString(PriceFinderObj);
                UserCharge.Text = string.Format("{0:0.###}",PriceFinderString);
                con1.Close(); 
            }
            catch
            {
            
            
            }
            }

        private void UserCharge_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void UserCharge_TextChanged(object sender, TextChangedEventArgs e)
        {
            Module.PNumberTString Tabdil = new Module.PNumberTString();

            BeHorof.Content = Tabdil.num2str(UserCharge.Text) + " تومن";
        }

        private void UserCharge_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
              int a, b, c, f;
            a = Convert.ToInt32(UserCharge.Text);
            b = Convert.ToInt32(DarsadeTakhfif_TXT.Text);
            c = ((Math.Abs(a) * Math.Abs(b)) / 100);
            f = a - c;
            UserCharge.Text = Convert.ToString(f);
      
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
       
        }
    }
    }
