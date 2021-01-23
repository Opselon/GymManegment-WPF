using DataModelLayer;
using GymManegment.Module;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

namespace GymManegment.window.Morabi
{
    /// <summary>
    /// Interaction logic for EditMorabi.xaml
    /// </summary>
    public partial class EditMorabi : Window
    {
        public EditMorabi()
        {
            InitializeComponent();
        }

      
        public int ID ;
    




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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            AddMorabi a = new AddMorabi();
            a.Show();

        }
        private void NameMember_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void NameMember_TextChanged(object sender, TextChangedEventArgs e)
        {

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
            MorabiPasswordChanging s = new MorabiPasswordChanging();
            s.ID = ID;
            s.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


           
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);


            SqlCommand Commandcmd = new SqlCommand(
                    "SELECT (MorabiImage) FROM MorabiTable WHERE MorabiID = " + ID,
                    con1);
            con1.Open();
            SqlDataReader rdr1 = null;
            rdr1 = Commandcmd.ExecuteReader();

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
         








            DataTable dt = new DataTable();

            SqlDataReader myReader = null;


            SqlCommand myCommand = new SqlCommand("select * From MorabiTable where 1 = 1 and MorabiID = " + ID, con1);
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {

               MorabiName.Text = (myReader["MorabiName"].ToString());
                MorabiFamily.Text = (myReader["MorabiFamily"].ToString());
               MorabiPhone.Text = (myReader["MorabiPhone"].ToString());
                MorabiCodeMeli.Text = (myReader["MorabiMeliCode"].ToString());
                MorabiSen.Text = (myReader["MorabiSen"].ToString());
          
            }




            con1.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                MorabiTable m = database.MorabiTable.First(c => c.MorabiID == ID);
                m.MorabiName = MorabiName.Text;
                m.MorabiFamily = MorabiFamily.Text;
                m.MorabiMeliCode = MorabiCodeMeli.Text;
                m.MorabiPhone = MorabiPhone.Text;
                m.MorabiSen = Convert.ToByte(MorabiSen.Text);

                database.SaveChanges();
                MessageBox.Show("اطلاعات با موفقیت بروزرسانی شد.");
            }
            catch
            {
                MessageBox.Show("در به روز رسانی اطلاعات مشکلی به وجود آمد.");
            }
        }
    }
}
