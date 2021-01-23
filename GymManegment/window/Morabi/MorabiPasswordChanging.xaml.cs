using DataModelLayer;
using GymManegment.Module;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace GymManegment.window.Morabi
{
    /// <summary>
    /// Interaction logic for MorabiPasswordChanging.xaml
    /// </summary>
    public partial class MorabiPasswordChanging : Window
    {
        public MorabiPasswordChanging()
        {
            InitializeComponent();
        }
        public int ID;
        private string usernamefind;
        private string passwordfind;
        gymEntities2 database = new gymEntities2();
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (OldPassword.Password == passwordfind)
            {
                if (Password.Password == PasswordRetry.Password)
                {

                    MorabiTable m = database.MorabiTable.First(c => c.MorabiID == ID);
                    m.MorabiPassword = Password.Password;
                    database.SaveChanges();
                    MessageBox.Show("اطلاعات با موفقیت بروزرسانی شد.");
                }
                else
                {
                    MessageBox.Show("در به روز رسانی اطلاعات مشکلی به وجود آمد.");
                }

            }


            else
            {
                MessageBox.Show("پسورد قدیمی اشتباه است");
            }
                
            
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
            DashboardMain s = new DashboardMain();
            s.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            con1.Open();
            DataTable dt = new DataTable();
            SqlDataReader myReader = null;
        
            SqlCommand myCommand = new SqlCommand("select * From MorabiTable where 1 = 1 and MorabiID = " + ID, con1);
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                Username.Text = (myReader["MorabiUsername"].ToString());
                usernamefind = Username.Text;
                Username.IsEnabled = false;
                passwordfind = (myReader["MorabiPassword"].ToString());
            }

            con1.Close();
            

            
        }
    }
}
