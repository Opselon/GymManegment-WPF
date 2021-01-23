using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DataModelLayer;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using GymManegment.Module;
using System.Text.RegularExpressions;
using MessagingToolkit.QRCode.Codec;
using Microsoft.Win32;

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for SubmitFactor.xaml
    /// </summary>
    public partial class SubmitFactor : Window
    {
        public SubmitFactor()
        {
            InitializeComponent();
        }

        private void Lbl_Name_Copy_Loaded(object sender, RoutedEventArgs e)
        {
            Lbl_Name_Copy.Content = snap.Text;
        }
        gymEntities2 database = new gymEntities2();
        public string MemberName = "";
        public string MemberFamily = "";
        public string memberid;
        private int Today;
        private int sal;
        private int mahv;
        private int mah;
        string ImageName , StrName;
        private int newdate;


       
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
    
        private string searchstate()
        {




            string searchstring = "";

            return searchstring;
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
            Random rnd = new Random();
            int card = rnd.Next(100000000, 999999999);
            TextBoxFactor.Text = Convert.ToString(card);
          


           

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



            LinkBedehi m = new LinkBedehi();

            try
            {
                


                SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
                DataTable dt = new DataTable();
                con1.Open();


                SqlCommand myCommandss = new SqlCommand("Select Sum(MemberCharge) from VW_Member where UserID = " + txt_username.Text.Trim(), con1);
                object resultss = myCommandss.ExecuteScalar();
                DaramadCount.Content = Convert.ToString(resultss) + " تومان";
                


                SqlCommand myCommands = new SqlCommand("Select DISTINCT (UserName + ' ' + UserFamily) from VW_Users where UserID = " + txt_username.Text.Trim(), con1);
                object results = myCommands.ExecuteScalar();
                test_lb_Copy.Content = Convert.ToString(results) + "";
                Lbl_Name.Content = Convert.ToString(results) + "";
                SqlCommand myCommand = new SqlCommand("Select sum(UserBedehiRial) from Members where MemberFindID = " + txt_username.Text.Trim() + " And MemberService = 2", con1);
                object result = myCommand.ExecuteScalar();
                test_lb.Content = Convert.ToString(result) + " تومان";
                try
                {
                    newuser.IsEnabled = false;
                    SqlCommand Commandcmd = new SqlCommand(
                        "SELECT (UserImage) FROM Users WHERE UserID = " + txt_username.Text.Trim(),
                        con1);
                    SqlDataReader rdr1 = null;
                    rdr1 = Commandcmd.ExecuteReader();
              




                    while (rdr1.Read())
                    {
                        newuser.IsEnabled = true;
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
                    //    ImageSource = new BitmapImage(new Uri("/pic/User.jpg", UriKind.Relative))
                    };
                }






            }
            catch (Exception exception)
            {

             
            }
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
           
            
        }

        private void MahView_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
          

        }

        private void TaView_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            Lbl_Name_Copy.Content = TaView.Text;
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
       
           



        





        }

        private void Cheaking_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
          
        }

        private void Image_MouseMove_1(object sender, MouseEventArgs e)
        {
   
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


                U.MemberDesc = TozihTextBox.Text == "توضیحات این فاکتور را بنویسید." ? "فاکتور خرید" : TozihTextBox.Text; 
              U.BedehKariDesc = TozihTextBox.Text == "توضیحات این فاکتور را بنویسید." ? "فاکتور خرید" : TozihTextBox.Text;
                U.MemberCharge = Convert.ToDecimal(UserCharge.Text.Trim());
                U.MemberCharge = Convert.ToDecimal(UserCharge.Text.Trim());
                U.UserBedehiRial = Convert.ToDecimal(BedehiText.Text.Trim()) == 50 ? 0 : Convert.ToDecimal(BedehiText.Text.Trim());
                U.MemberService = Convert.ToByte(bedehs);
                U.MemberStartDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Lbl_Name_Copy.Content));


                database.Members.Add(U);
                database.SaveChanges();
                SucFul Suc = new SucFul();

                Suc.Show();
                Suc.Suclbl.Content = "فاکتور با موفقیت ثبت شد";
               

            }
            catch
            {
                ErrorPage E = new ErrorPage();
                E.Show();
                E.Error_Lable.Content = "کد کاربری درستی وارد کنید";
                txt_username.Focus();

            }
         
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

        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

       
        }

        private void Image_MouseMove_2(object sender, MouseEventArgs e)
        {
            this.Opacity = 1;
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            PriceStory l = new PriceStory();
            ErrorPage es = new ErrorPage();

            if (test_lb_Copy.Content != "")
            {
             
                l.txt_id.Text = txt_username.Text.Trim();
                l.Show();
            }

            if (test_lb_Copy.Content == "")
            { 

                    es.Show();
                 es.Error_Lable.Content = "شما کد کاربری درستی وارد کنید.";
            }
         
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            this.Opacity = 1;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void test_lb_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TozihTextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TozihTextBox.Text = "";
        }

        private void TozihTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TozihTextBox.Text = "";
        }

        private void TozihTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void TozihTextBox_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TozihTextBox.Text = "";
        }

        private void TozihTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TozihTextBox.Text = "";
        }

        private void UserCharge_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UserCharge_TextChanged(object sender, TextChangedEventArgs e)
        {
            Module.PNumberTString Tabdil = new Module.PNumberTString();

            BeFarsi.Content = Tabdil.num2str(UserCharge.Text) + " تومن";
        }

    
        private void DarsadeTakhfif_TXT_TextChanged(object sender, TextChangedEventArgs e)
        {

           
        }

       
        private void newuser_Copy_MouseEnter(object sender, MouseEventArgs e)
        {
            int b;
            b = Convert.ToInt32(DarsadeTakhfif_TXT.Text);
            Mohasebe.ToolTip = "برای محاصبه ی (%" + b+") تخفیف کلیک کنید" ;
        }

        private void Mohasebe_Click(object sender, RoutedEventArgs e)
        {
            int a, b, c, f;
            a = Convert.ToInt32(UserCharge.Text);
            b = Convert.ToInt32(DarsadeTakhfif_TXT.Text);
            c = ((Math.Abs(a) * Math.Abs(b)) / 100);
            f = a - c;
            UserCharge.Text = Convert.ToString(f);
     

           
        }

        private void TozihTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           


        }
        string newpath;
        private void Mohasebe_Copy_Click(object sender, RoutedEventArgs e)
        {
            string text;
            text = TozihTextBox.Text;
            QRCodeEncoder enc = new QRCodeEncoder();
            Bitmap Creator = enc.Encode(text, Encoding.UTF8);
            QRImage.Source = YourUtilClass.loadBitmap(Creator);

            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("ایا میخواهید این کد  ذخیره شود ؟ ","اطلاعیه", System.Windows.Forms.MessageBoxButtons.YesNo);

            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {

                string filePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + @"\QRimages\ " + TozihTextBox.Text + " .jpg";
                string newpatch = @filePath.Replace("\\GymManegment.exe", "");
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)QRImage.Source));
                using (FileStream stream = new FileStream(newpatch, FileMode.Create))
                    encoder.Save(stream);

            }
     


          

          



        }

        private void DarsadeTakhfif_TXT_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            Mohasebe.IsEnabled = true;
        }

        Bitmap name;


        private void Mohasebe_Copy2_Click(object sender, RoutedEventArgs e)
        {

            

            OpenFileDialog dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                Filter = "Images (*.jpg,*.png)|*.jpg;*.png|All Files(*.*)|*.*"
            };
            dialog.ShowDialog();
            {
                string filePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + @"\QRimages\";
                string newpatch = @filePath.Replace("\\GymManegment.exe", "");
                dialog.InitialDirectory = "c:\\";
                 StrName = dialog.SafeFileName;
                ImageName = dialog.FileName;
                dialog.RestoreDirectory = true;
                ImageSourceConverter isc = new ImageSourceConverter();
                if (ImageName != "")
                {
                    QRImage.SetValue(System.Windows.Controls.Image.SourceProperty, isc.ConvertFromString(ImageName));

                }

            }



            QRCodeDecoder dc = new QRCodeDecoder();

            TozihTextBox.Text = dialog.SafeFileName.Replace(".jpg" , "");


    //        dc.decode(new QRCodeBitmapImage(QRImage.Source as Bitmap));
        }
        

      
        private void Mohasebe_Copy1_Click(object sender, RoutedEventArgs e)
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
                        QRImage.SetValue(System.Windows.Controls.Image.SourceProperty, isc.ConvertFromString(ImageName));
                  
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
    }
}
