using DataModelLayer;
using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GymManegment.window.Morabi
{
    /// <summary>
    /// Interaction logic for DashboardMain.xaml
    /// </summary>
    public partial class AddMorabi : Window
    {

        public AddMorabi()
        {
            InitializeComponent();
        }
        string ImageName = "";
        string StrName = "";
        public string MemberName = "";
        public string MemberFamily = "";

        gymEntities2 database = new gymEntities2();

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void TextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UserName.Clear();
        }

        private void NameMember_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void NameMember_TextChanged(object sender, TextChangedEventArgs e)
        {
         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


           


    











            bool Gender = true;


            if(MorabiZan.IsChecked == true)
            {
                Gender = false;
            }

            SHA256CryptoServiceProvider Sha2 = new SHA256CryptoServiceProvider();


            byte[] B1;
            byte[] B2;
            B1 = UTF8Encoding.UTF8.GetBytes(MorabiPassword.Text);
            B2 = Sha2.ComputeHash(B1);
            string UserPasswordHashed = BitConverter.ToString(B2);


            MorabiTable m = new MorabiTable();
            m.MorabiName = MorabiName.Text;
            m.MorabiFamily = MorabiFamily.Text;
            m.MorabiMeliCode = MorabiCodeMeli.Text;
            m.MorabiPhone = MorabiPhone.Text;
            m.MorabiSen = Convert.ToByte(MorabiSen.Text);
            m.MorabiUsername = UserName.Text;

            if (MorabiPassword.Text == MorabiPasswordRep.Text)

            {
                m.MorabiPassword = UserPasswordHashed;
            }

            else
            {
                
                MorabiPassword.Focus();
                MessageBox.Show("تکرار رمز عبور درست نمیباشد");
            }

            m.MorabiGender = Gender;
            if (ImageName != "")
            {
                FileStream fs = new FileStream(ImageName, FileMode.Open, FileAccess.Read);
                byte[] imgByteArr = new byte[fs.Length];
                fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                m.MorabiImage = imgByteArr;
            }
            database.MorabiTable.Add(m);
            database.SaveChanges();
        }

        private void ChoicePicture_Click(object sender, RoutedEventArgs e)
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
                    //    ImagePath.Text = ImageName.ToString();
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

        private void Button_Click_1()
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
            DashboardMain s = new DashboardMain();
            s.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
            LoginPage s = new LoginPage();
            s.Show();
        }

        private void UserName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UserName.Clear();
        }

        private void MorabiKey_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
