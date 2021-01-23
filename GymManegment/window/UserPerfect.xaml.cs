using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
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
using GymManegment.Module;
namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for UserPerfect.xaml
    /// </summary>
    public partial class UserPerfect : Window
    {
        public UserPerfect()
        {
            InitializeComponent();

        }






        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users  ", con1);
            object result = myCommand.ExecuteScalar();

            Title = "لیست اطلاعات تکمیلی ورزشکاران | تعداد کل ورزشکاران باشگاه : " + Convert.ToString(result) + "";
            SizeFix();
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Label_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users where UserActive = 1  and UserGender = 1", con1);
            object result = myCommand.ExecuteScalar();
            lbl_faal.Content = Convert.ToString(result) + " نفر فعال هستند";

        }

        private void Label_Loaded_1(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users where UserActive = 2  and UserGender = 1", con1);
            object result = myCommand.ExecuteScalar();
            lbl_qeirfaal.Content = Convert.ToString(result) + " نفر غیر فعال هستند";
        }

        private void kol_mard_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users where  UserGender = 1", con1);
            object result = myCommand.ExecuteScalar();
            kol_mard.Content = Convert.ToString(result) + " نفر ";
        }

        private void miangin_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select avg(UserAge) from Users where UserGender = 1", con1);
            object result = myCommand.ExecuteScalar();
            miangin.Content = Convert.ToString(result) + " سال ";
        }

        private void zire_20_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserAge) from Users where UserAge > 0 and UserAge < 20 and UserGender = 1", con1);
            object result = myCommand.ExecuteScalar();
            zire_20.Content = Convert.ToString(result) + " نفر ";
        }

        private void zire_30_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserAge) from Users where UserAge > 20 and UserAge < 30 and UserGender = 1", con1);
            object result = myCommand.ExecuteScalar();
            zire_30.Content = Convert.ToString(result) + " نفر ";
        }

        private void zire_40_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserAge) from Users where UserAge > 30 and UserAge < 40 and UserGender = 1", con1);
            object result = myCommand.ExecuteScalar();
            zire_40.Content = Convert.ToString(result) + " نفر ";
        }

        private void albl_faal_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users where UserActive = 1  and UserGender = 2", con1);
            object result = myCommand.ExecuteScalar();
            albl_faal.Content = Convert.ToString(result) + " نفر فعال هستند";
        }

        private void albl_qeirfaal_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users where UserActive = 2  and UserGender = 2", con1);
            object result = myCommand.ExecuteScalar();
            albl_qeirfaal.Content = Convert.ToString(result) + " نفر غیر فعال هستند";
        }

        private void Alkol_mard_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users where  UserGender = 2", con1);
            object result = myCommand.ExecuteScalar();
            Alkol_mard.Content = Convert.ToString(result) + " نفر ";
        }

        private void ALabelmiangin_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select avg(UserAge) from Users where UserGender = 2 ", con1);
            object result = myCommand.ExecuteScalar();
            ALabelmiangin.Content = Convert.ToString(result) + " سال ";
        }

        private void azire_20_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserAge) from Users where UserAge > 0 and UserAge < 20 and UserGender = 2", con1);
            object result = myCommand.ExecuteScalar();
            azire_20.Content = Convert.ToString(result) + " نفر ";
        }

        private void azire_30_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserAge) from Users where UserAge > 20 and UserAge < 30 and UserGender = 2", con1);
            object result = myCommand.ExecuteScalar();
            azire_30.Content = Convert.ToString(result) + " نفر ";
        }

        private void azire_40_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserAge) from Users where UserAge > 30 and UserAge < 40 and UserGender = 2", con1);
            object result = myCommand.ExecuteScalar();
            azire_40.Content = Convert.ToString(result) + " نفر ";
        }

        private void N1_Loaded(object sender, RoutedEventArgs e)
        {
            int mount = Taghvim.SelectedDate.Month;
            int day = Taghvim.SelectedDate.Day;
            int year = Taghvim.SelectedDate.Year;
            string Year1 = year + "/" + mount + "/" + day;
            string Year2 = year + "/" + (mount - 1) + "/" + day;


            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users where UserRegDate between '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Year2)) + "' And '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Year1)) + "'", con1);
            object result = myCommand.ExecuteScalar();
            N1.Content = Convert.ToString(result) + " نفر ";

        }

        private void N2_Loaded(object sender, RoutedEventArgs e)
        {

            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users ", con1);
            object result = myCommand.ExecuteScalar();
            N2.Content = Convert.ToString(result) + " نفر ";

        }

        private void Label_Loaded_2(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users Where UserActive = 2", con1);
            object result = myCommand.ExecuteScalar();
            N3.Content = Convert.ToString(result) + " نفر ";
        }

        private void N4_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users Where UserGender = 1", con1);
            object result = myCommand.ExecuteScalar();
            N4.Content = Convert.ToString(result) + " نفر ";
        }

        private void N5_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users Where UserGender = 2", con1);
            object result = myCommand.ExecuteScalar();
            N5.Content = Convert.ToString(result) + " نفر ";
        }

        private void N6_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select avg(UserAge) from Users", con1);
            object result = myCommand.ExecuteScalar();
            N6.Content = Convert.ToString(result) + " نفر ";
        }

        private void N7_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users Where UserActive = 1", con1);
            object result = myCommand.ExecuteScalar();
            N7.Content = Convert.ToString(result) + " نفر ";
        }

        private void N8_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users Where UserActive = 2", con1);
            object result = myCommand.ExecuteScalar();
            N8.Content = Convert.ToString(result) + " نفر ";
        }

        private void N11_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users Where UserGender = 1", con1);
            object result = myCommand.ExecuteScalar();
            N11.Content = Convert.ToString(result) + " نفر ";
        }

        private void N12_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select count(UserID) from Users Where UserGender = 2", con1);
            object result = myCommand.ExecuteScalar();
            N12.Content = Convert.ToString(result) + " نفر ";
        }
        private void SizeFix()
        {

            this.MaxHeight = 354;
            this.MinHeight = 354;
            this.MaxWidth = 780;
            this.MinWidth = 780;
        }
    }
}
