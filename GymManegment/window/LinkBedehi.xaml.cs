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
using System.Data.SqlClient;
using System.Data;
using GymManegment.Module;
namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for LinkBedehi.xaml
    /// </summary>
    public partial class LinkBedehi : Window
    {
        public LinkBedehi()
        {
            InitializeComponent();
        }
        gymEntities2 database = new gymEntities2();

        public string membername;
        public string memberfamily;
        public int memberid;


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

        private void ShowUserInfo(Func<string> SearchStringForUser)
        {
         

        }
        private void Tasvie(object sender, RoutedEventArgs e)
        {
            decimal pop;
            object item = DataGrid_Members.SelectedItem;
            pop = Convert.ToInt32((DataGrid_Members.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("UPDATE Members set MemberService = 1 where MemberFindID = " + memberid + " and MemberFactorCode = " + pop, con1);

            myReader = myCommand.ExecuteReader();
            ShowUserInfo(searchstate);
        }
        //tarhaye tashvighi
        private void DelFac(object sender, RoutedEventArgs e)
        {
            decimal pop;
            object item = DataGrid_Members.SelectedItem;
            pop = Convert.ToInt32((DataGrid_Members.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);

            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("Delete from Members where MemberFactorCode = " + pop, con1);

            myReader = myCommand.ExecuteReader();
            ShowUserInfo(searchstate);
        }
        private void TasvieNashode(object sender, RoutedEventArgs e)
        {
            decimal pop;
            object item = DataGrid_Members.SelectedItem;
            pop = Convert.ToInt32((DataGrid_Members.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);

            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("UPDATE Members set MemberService = 2 where MemberFindID = " + memberid + " and MemberFactorCode = " + pop, con1);

            myReader = myCommand.ExecuteReader();
            ShowUserInfo(searchstate);
        }
        private string searchstate()
        {

            string searchstring = "";

            if (factortext.Text != "")
            {
                searchstring = " And MemberFactorCode like '%" + factortext.Text + "%'";
            }

            if (Comb.SelectedIndex == 1)
            {
                searchstring = " And MemberService = 1";
            }
            if (Comb.SelectedIndex == 2)
            {
                searchstring = " And MemberService = 2";
            }
            // select* from VW_Users where 1 = 1 And UserFamily like reza

            return searchstring;
        }



        //تابع ساخت شرط برای نمایش اطلاعات در گرید ویو


        private void SerCh_Click(object sender, RoutedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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
            txt_user.Text = membername + " " + memberfamily;
            // var query = database.Database.SqlQuery<Members>("select * From VW_Users where 1 = 1 and MemberFindID = " + memberid + " " + SearchStringForUser());
            //  var user = query.ToList();
            //   DataGrid_Members.ItemsSource = user;
            ShowUserInfo(searchstate);

        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            txt_user.Text = membername + " " + memberfamily;
            ShowUserInfo(searchstate);
        }

        private void txt_user_TextChanged(object sender, TextChangedEventArgs e)
        {
            txt_user.Text = memberfamily;
            ShowUserInfo(searchstate);
        }

        private void DataGrid_Members_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void newuser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ShowUserInfo(searchstate);
        }

        private void test_lb_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select Sum(UserBedehiRial) from Members where MemberFindID = " + memberid + " and UserBedehiRial > 0 ", con1);



            object result = myCommand.ExecuteScalar();
            test_lb.Content = Convert.ToString(result) + " تومان";
        }

        private void test1_lb_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select Sum(MemberCharge) from Members where MemberFindID = " + memberid, con1);


            object result = myCommand.ExecuteScalar();
            test1_lb.Content = Convert.ToString(result) + " تومان";
        }

        private void Ribbon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
