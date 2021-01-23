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
using Arash;
using GymManegment.Module;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;

namespace GymManegment.window
{


    /// <summary>
    /// Interaction logic for profile_user.xaml
    /// </summary>
    public partial class MemberCharge : Window
    {



        public MemberCharge()
        {
            InitializeComponent();

        }
        gymEntities2 database = new gymEntities2();

        SqlConnection con1 =
            new SqlConnection(
                PublicVar.ConnectionString);
        public string MemberName = "";
        public string MemberFamily = "";
        public string memberid;
        private int Today;
        private int sal;
        private int mahv;
        private int mah;
        private string TimeOutSubmit;
        private int newdate;


        public MemberCharge(string name, string phoneNumber)
        {
            //login to handle name and phone number

        }
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //this.DragMove();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {

            this.Close();
        }
        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
        private string searchstate()
        {




            string searchstring = "";

            return searchstring;
        }




        //تابع ساخت شرط برای نمایش اطلاعات در گرید ویو
        private void ShowUserInfo(Func<string> SearchStringForUser)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select Sum(UserBedehiRial) from Members where MemberFindID = " + memberid + " and MemberService = 2", con1);

            //  myReader = myCommand.ExecuteReader();
            object result = myCommand.ExecuteScalar();
            test_lb.Content = Convert.ToString(result) + " تومان ";
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
            this.DragMove();
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



            TaView.DisplayDateStart = PersianDate.Parse(MahView.Text);
            TaView.SelectedDate = PersianDate.Today.AddDays(30);



            Random rnd = new Random();
            int card = rnd.Next(100000000, 999999999);
            TextBoxFactor.Text = Convert.ToString(card);
            Lbl_Name.Content = "" + MemberName + " " + MemberFamily;
            txt_username.Text = Convert.ToString(memberid);




            int Today;
            Today = MahView.SelectedDate.Day;


            mah = MahView.SelectedDate.Month;
            mahv = mah;

            sal = MahView.SelectedDate.Year;


            newdate = mah + 1;






            TextBoxAz.Text = sal + "/" + mah + "/" + Today;
            TextBoxTa.Text = sal + "/" + newdate + "/" + Today;
            // mah bayad 00 | 1 = 01



            switch (mah)
            {
                case 1:
                    DateView.Content = "فروردین";
                    break;
                case 2:
                    DateView.Content = "اردیبهشت";
                    break;
                case 3:
                    DateView.Content = "خرداد";
                    break;
                case 4:
                    DateView.Content = "تیر";
                    break;
                case 5:
                    DateView.Content = "مرداد";
                    break;
                case 6:
                    DateView.Content = "شهریور";
                    break;
                case 7:
                    DateView.Content = "مهر";
                    break;
                case 8:
                    DateView.Content = "آبان";
                    break;
                case 9:
                    DateView.Content = "آذر";
                    break;
                case 10:
                    DateView.Content = "دی";
                    break;
                case 11:
                    DateView.Content = "بهمن";
                    break;
                case 12:
                    DateView.Content = "اسفند";
                    break;
            }




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
            txt_username.Text = memberid.ToString();

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

        private void Label_Loaded(object sender, RoutedEventArgs e)
        {
            int time;
            time = MahView.SelectedDate.Month;
            switch (time)
            {
                case 1:
                    DateView.Content = "فروردین";
                    break;
                case 2:
                    DateView.Content = "اردیبهشت";
                    break;
                case 3:
                    DateView.Content = "خرداد";
                    break;
                case 4:
                    DateView.Content = "تیر";
                    break;
                case 5:
                    DateView.Content = "مرداد";
                    break;
                case 6:
                    DateView.Content = "شهریور";
                    break;
                case 7:
                    DateView.Content = "مهر";
                    break;
                case 8:
                    DateView.Content = "آبان";
                    break;
                case 9:
                    DateView.Content = "آذر";
                    break;
                case 10:
                    DateView.Content = "دی";
                    break;
                case 11:
                    DateView.Content = "بهمن";
                    break;
                case 12:
                    DateView.Content = "اسفند";
                    break;
            }
        }

        private void MahView_SelectedDateChanged(object sender, RoutedEventArgs e)
        {

            TaView.DisplayDateStart = PersianDate.Parse(MahView.Text);

            int time;
            time = MahView.SelectedDate.Month;
            switch (time)
            {
                case 1: DateView.Content = "فروردین"; mahv = 1; break;
                case 2: DateView.Content = "اردیبهشت"; mahv = 2; break;
                case 3: DateView.Content = "خرداد"; mahv = 3; break;
                case 4: DateView.Content = "تیر"; mahv = 4; break;

                case 5:

                    DateView.Content = "مرداد";
                    mahv = 5;
                    break;
                case 6:
                    DateView.Content = "شهریور";
                    mahv = 6;
                    break;
                case 7:
                    DateView.Content = "مهر";
                    mahv = 7;
                    break;
                case 8:
                    DateView.Content = "آبان";
                    mahv = 8;
                    break;
                case 9:
                    DateView.Content = "آذر";
                    mahv = 9;
                    break;
                case 10:
                    DateView.Content = "دی"; mahv = 10;
                    break;
                case 11:
                    mahv = 11;
                    DateView.Content = "بهمن";
                    break;
                case 12:
                    mahv = 12;
                    DateView.Content = "اسفند";
                    break;
            }

        }

        private void TaView_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            int time;
            time = TaView.SelectedDate.Month;
            switch (time)
            {
                case 1:
                    lbl_ta.Content = "فروردین";
                    newdate = 1;
                    break;
                case 2:
                    lbl_ta.Content = "اردیبهشت";
                    newdate = 2;
                    break;
                case 3:
                    lbl_ta.Content = "خرداد";
                    newdate = 3;
                    break;
                case 4:
                    lbl_ta.Content = "تیر";
                    newdate = 4;
                    break;
                case 5:
                    lbl_ta.Content = "مرداد";
                    newdate = 5;
                    break;
                case 6:
                    lbl_ta.Content = "شهریور";
                    newdate = 6;
                    break;
                case 7:
                    lbl_ta.Content = "مهر";
                    newdate = 7;
                    break;
                case 8:
                    lbl_ta.Content = "آبان";
                    newdate = 8;
                    break;
                case 9:
                    lbl_ta.Content = "آذر";
                    newdate = 9;
                    break;
                case 10:
                    lbl_ta.Content = "دی";
                    newdate = 10;
                    break;
                case 11:
                    lbl_ta.Content = "بهمن";
                    newdate = 11;
                    break;
                case 12:
                    lbl_ta.Content = "اسفند";
                    newdate = 12;
                    break;
            }
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Today = MahView.SelectedDate.Day;


            mah = MahView.SelectedDate.Month;
            mahv = mah;

            sal = MahView.SelectedDate.Year;


            newdate = mah + 1;

            switch (mah)
            {
                case 1: DateView.Content = "فروردین"; mahv = 1; break;
                case 2: DateView.Content = "اردیبهشت"; mahv = 2; break;
                case 3: DateView.Content = "خرداد"; mahv = 3; break;
                case 4: DateView.Content = "تیر"; mahv = 4; break;

                case 5:

                    DateView.Content = "مرداد";
                    mahv = 5;
                    break;
                case 6:
                    DateView.Content = "شهریور";
                    mahv = 6;
                    break;
                case 7:
                    DateView.Content = "مهر";
                    mahv = 7;
                    break;
                case 8:
                    DateView.Content = "آبان";
                    mahv = 8;
                    break;
                case 9:
                    DateView.Content = "آذر";
                    mahv = 9;
                    break;
                case 10:
                    DateView.Content = "دی"; mahv = 10;
                    break;
                case 11:
                    mahv = 11;
                    DateView.Content = "بهمن";
                    break;
                case 12:
                    mahv = 12;
                    DateView.Content = "اسفند";
                    break;
            }






            switch (newdate)
            {
                case 1:
                    lbl_ta.Content = "فروردین";
                    newdate = 1;
                    break;
                case 2:
                    lbl_ta.Content = "اردیبهشت";
                    newdate = 2;
                    break;
                case 3:
                    lbl_ta.Content = "خرداد";
                    newdate = 3;
                    break;
                case 4:
                    lbl_ta.Content = "تیر";
                    newdate = 4;
                    break;
                case 5:
                    lbl_ta.Content = "مرداد";
                    newdate = 5;
                    break;
                case 6:
                    lbl_ta.Content = "شهریور";
                    newdate = 6;
                    break;
                case 7:
                    lbl_ta.Content = "مهر";
                    newdate = 7;
                    break;
                case 8:
                    lbl_ta.Content = "آبان";
                    newdate = 8;
                    break;
                case 9:
                    lbl_ta.Content = "آذر";
                    newdate = 9;
                    break;
                case 10:
                    lbl_ta.Content = "دی";
                    newdate = 10;
                    break;
                case 11:
                    lbl_ta.Content = "بهمن";
                    newdate = 11;
                    break;
                case 12:
                    lbl_ta.Content = "اسفند";
                    newdate = 12;
                    break;
            }


            TextBoxAz.Text = sal + "/" + mah + "/" + Today;
            TextBoxTa.Text = sal + "/" + newdate + "/" + Today;


            ss.Visibility = Visibility.Hidden;
            s.Visibility = Visibility.Hidden;






            TextBoxAz.Visibility = Visibility.Visible;
            TextBoxTa.Visibility = Visibility.Visible;










            MahView.Visibility = Visibility.Hidden;
            TaView.Visibility = Visibility.Hidden;




        }

        private void Cheaking_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            s.Visibility = Visibility.Hidden;
        }

        private void Image_MouseMove_1(object sender, MouseEventArgs e)
        {
            ss.Visibility = Visibility.Hidden;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Random rnd = new Random();
            int card = rnd.Next(100000000, 999999999);
            TextBoxFactor.Text = Convert.ToString(card);
            Lbl_Name.Content = "" + MemberName + " " + MemberFamily;
            txt_username.Text = Convert.ToString(memberid);
        }



        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Random rnd = new Random();
            int card = rnd.Next(100000000, 999999999);
            TextBoxFactor.Text = Convert.ToString(card);
            Lbl_Name.Content = "" + MemberName + " " + MemberFamily;
            txt_username.Text = Convert.ToString(memberid);
        }






        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            BedehiText.Visibility = Visibility.Visible;
            BedehiDescText.Visibility = Visibility.Visible;
            mablq.Visibility = Visibility.Visible;
        }

        private void TextBoxAz_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void newuser_Click(object sender, RoutedEventArgs e)
        {





            // ShowUserInfo(searchstate);
















            try
            {
                Members U = new Members();
                Submit Sub = new Submit();
                // text box ro be sorate automatic bebarim ye mah bad va age motaqaier b on rooz resid bege qeir faal an
                int bedehs;
                string descs;

                LinkBedehi l = new LinkBedehi();


                if (BedehiDescText.Text == " را اینجا وارد کنید")
                {
                    descs = "توضیح این بدهی وجود ندارد";
                }
                else
                {
                    descs = BedehiDescText.Text;
                }
                if (Convert.ToDecimal(BedehiText.Text) == 50)
                {
                    bedehs = 0;
                }
                else
                {
                    bedehs = 2;
                }



                U.MemberFactorCode = Convert.ToInt64(TextBoxFactor.Text.Trim());
                U.MemberFindID = Convert.ToInt32(txt_username.Text.Trim());
                U.MemberStartDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(MahView.Text));
                U.MemberLastCharge = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(TaView.Text));
               TimeOutSubmit = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(TaView.Text));
                U.MemberMounth = Convert.ToByte(mahv);
                U.MemberMountTwo = Convert.ToByte(newdate);

                U.MemberCharge = Convert.ToDecimal(UserCharge.Text.Trim());
                U.UserBedehiRial = Convert.ToDecimal(BedehiText.Text.Trim()) == 50 ? 0 : Convert.ToDecimal(BedehiText.Text.Trim());
                U.BedehKariDesc = BedehiDescText.Text == "توضیحات وجود ندارد." ? "شارژ و تمدید حساب" : BedehiDescText.Text.Trim();
                U.MemberService = Convert.ToByte(bedehs);
                


                database.Members.Add(U);
                database.SaveChanges();
                MessageBox.Show("ثبت شد");
           

            }
            catch
            {
                MessageBox.Show("مشکلی در ثبت اطلاعات به وجود امده است");

            }
            finally
            {
                this.Close();
            }


            int estakhr, bime, khososi, barnameqazaiy;
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

            if (KhososiCheakBox.IsChecked == true)
            {
                khososi = 1;
            }
            else
            {
                khososi = 0;
            }
            if (BarnameQazaiCheakBox.IsChecked == true)
            {
                barnameqazaiy = 1;
            }
            else
            {
                barnameqazaiy = 0;
            }




            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            con1.Open();
            SqlCommand Commandcmdsz = new SqlCommand("update VW_TimeOut set UserActive = 1 Where UserFindID = " + memberid , con1);
            SqlCommand Commandcmdszz = new SqlCommand("update VW_TimeOut set  UserEndDate = '" + TimeOutSubmit + "' where UserFindID = " + memberid, con1);
            SqlCommand Commandcmdszzz = new SqlCommand("update Users set  UserEstakhr = " + estakhr + " , UserBime = " + bime + "," +
    " UserKhososi = " + khososi + " , UserBarnameQazaiy = " + barnameqazaiy + " Where UserID = " + memberid, con1);


            Commandcmdsz.ExecuteScalar();
            Commandcmdszz.ExecuteScalar();
            Commandcmdszzz.ExecuteScalar();
            con1.Close();
        }


        private void one_TextChanged(object sender, TextChangedEventArgs e)
        {
             
            Submit sub = new Submit();

        }

        private void BedehiText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BedehiDescText_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void BedehiText_PreviewMouseMove(object sender, MouseEventArgs e)
        {

        }

        private void BedehiText_MouseMove(object sender, MouseEventArgs e)
        {


        }

        private void BedehiText_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void BedehiText_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }



        private void BedehiDescText_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BedehiDescText.Text = "";
        }

        private void BedehiDescText_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            BedehiDescText.Text = "";
        }

        private void Label_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            LinkBedehi l = new LinkBedehi();
            l.memberid = Convert.ToInt32(txt_username.Text.Trim());
            this.Opacity = 0.2;
            l.Show();
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            LinkBedehi l = new LinkBedehi();
            l.memberid = Convert.ToInt32(txt_username.Text.Trim());
            l.memberfamily = Lbl_Name.Content.ToString();
            l.Show();
        }

        private void Image_MouseMove_2(object sender, MouseEventArgs e)
        {
            this.Opacity = 1;
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            LinkBedehi l = new LinkBedehi();
            l.memberid = Convert.ToInt32(txt_username.Text.Trim());
            l.memberfamily = Lbl_Name.Content.ToString();
            l.Show();
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            this.Opacity = 1;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void Image_DragLeave(object sender, DragEventArgs e)
        {

        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Image img = (System.Windows.Controls.Image)sender;
            DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(2));
            img.BeginAnimation(System.Windows.Controls.Image.OpacityProperty, animation);
        }

        private void Image_MouseMove_3(object sender, MouseEventArgs e)
        {


            try
            {
                SqlCommand Commandcmd = new SqlCommand(
                  "SELECT (UserImage) FROM Users WHERE UserID = " + txt_username.Text.Trim(),
                  con1);
                con1.Open();
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
            catch (Exception exception)
            {
                Images.Fill = new ImageBrush
                {
                  
                };
            }

            con1.Close();
        }

        private void UserCharge_TextChanged(object sender, TextChangedEventArgs e)
        {
            Module.PNumberTString Tabdil = new Module.PNumberTString();

            BeHorof.Content = Tabdil.num2str(UserCharge.Text) +" تومان";
        }

        private void UserCharge_TextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UserCharge_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {


                SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
                con1.Open();



                SqlCommand PriceFinder = new SqlCommand("SELECT Max(MemberCharge) FROM Members where MemberMounth > 1 and MemberFindID = " + memberid , con1);
                object PriceFinderObj = PriceFinder.ExecuteScalar();
                string PriceFinderString = Convert.ToString(PriceFinderObj);
                UserCharge.Text = string.Format("{0:0.###}", PriceFinderString);
                con1.Close();
            }
            catch
            {


            }
        }

        private void UserCharge_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TaView_SelectedDateChanged_1(object sender, RoutedEventArgs e)
        {
            lbl_ta.Content = TaView.SelectedDate.MonthAsPersianMonth;
        }

        private void MahView_SelectedDateChanged_1(object sender, RoutedEventArgs e)
        {
            DateView.Content = MahView.SelectedDate.MonthAsPersianMonth;
        }
    }
}
