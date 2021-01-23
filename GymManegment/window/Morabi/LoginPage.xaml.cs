using DataModelLayer;
using GymManegment.Module;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        gymEntities2 database = new gymEntities2();
        SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);



        private string findID;



        private void Button_Click(object sender, RoutedEventArgs e)
        {




            var SerialSender = from u in database.MorabiTable
                               where u.MorabiUsername == Username.Text
                               where u.MorabiPassword == Password.Password
                               select u;
            var SerialResualt = SerialSender.ToList();



            if (SerialResualt.Count > 0)
            {


                //this.Close();
                

         









                var row = database.MorabiTable.SingleOrDefault(r => r.MorabiUsername == Username.Text && r.MorabiPassword == Password.Password);
                var name = row != null ? Convert.ToString(row.MorabiID) : String.Empty;
                findID = Convert.ToString(name);

                EditMorabi E = new EditMorabi();
                E.ID = Convert.ToInt32(findID); 
                
                E.Show();









            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
            DashboardMain s = new DashboardMain();
            s.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

         
        }
    }
}
