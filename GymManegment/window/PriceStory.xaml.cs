using Arash;
using DataModelLayer;
using GymManegment.Module;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for PriceStory.xaml
    /// </summary>
    public partial class PriceStory : Window
    {
        public PriceStory()
        {
            InitializeComponent();
        }


        gymEntities2 database = new gymEntities2();

        public string membername;
        public string memberfamily;
        public int memberid;
        string Cheaked;

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
            var query = database.Database.SqlQuery<VW_Member>("select * From VW_Member where 1 = 1 " + SearchStringForUser() + " ORDER BY MemberStartDate DESC ");
           //    MessageBox.Show(query.ToString());
            var user = query.ToList();

            DataGrid_Members.ItemsSource = user;
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

            string searchstring =  "" + Cheaked;

            try
            {
                if (txt_id.Text != "")
                {
                    searchstring += " And MemberFindID = " + txt_id.Text;
                }
            }
            catch
            {
            }

            if (factortext.Text != "")
            {
                searchstring += " And MemberFactorCode like '%" + factortext.Text + "%'";
            }

            if (Comb.SelectedIndex == 1)
            {
                searchstring += " And MemberService = 1";
            }
            if (Comb.SelectedIndex == 2)
            {
                searchstring += " And MemberService = 2";
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
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
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
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand myCommand = new SqlCommand("Select Sum(MemberCharge) from Members", con1);

            object result = myCommand.ExecuteScalar();
            test_lb.Content = Convert.ToString(result) + " تومان";
            Cal_Az.SelectedDate = PersianDate.Today.AddDays(-30);

            SqlCommand myCommands = new SqlCommand("Select Sum(MemberCharge) from Members where MemberStartDate Between '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Cal_Az.Text)) + "' And  '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(NowTime.Text)) + "'"  , con1);

            object results = myCommand.ExecuteScalar();
            test1_lb.Content = Convert.ToString(results) + " تومان";

   

            SqlCommand myCommandsst = new SqlCommand("Select Count(MemberFactorCode) from Members where MemberStartDate = '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(NowTime.Text)) + "'", con1);

            object resultsst = myCommandsst.ExecuteScalar();
            test_lb_Copy.Content = Convert.ToString(resultsst) + " عدد";

            SqlCommand myCommandsstt = new SqlCommand("Select Sum(MemberCharge) from Members where MemberStartDate = '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(NowTime.Text)) + "'", con1);

            object resultsstt = myCommandsstt.ExecuteScalar();
            test1_lb_Copy.Content = Convert.ToString(resultsstt) + " تومان";


           

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
     
        }

        private void test1_lb_Loaded(object sender, RoutedEventArgs e)
        {
         
        }

        private void Ribbon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txt_id_Copy1_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void TodayFactors_Checked(object sender, RoutedEventArgs e)
        {
            Cheaked = " And MemberStartDate = '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(PublicVar.TodayTime)) + "'";
            ShowUserInfo(searchstate);
        }

        private void TodayFactors_Unchecked(object sender, RoutedEventArgs e)
        {
            Cheaked = "";
            ShowUserInfo(searchstate);

        }

        private void DataGrid_Members_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
           
            object itemOne = DataGrid_Members.SelectedItem;
            try
            {
                var content = (DataGrid_Members.SelectedCells[2].Column.GetCellContent(itemOne) as TextBlock).Text;
              

                NamayeshTarikh.Text = String.Format("{0:yyyy/M/d}", Convert.ToDateTime(Convert.ToString(content)));
                NamayeshTarikh1.SelectedDate = PersianDate.Parse(content);



                TarikhMah.Content = NamayeshTarikh1.SelectedDate.MonthAsPersianMonth;
                TarikhRoz.Content = NamayeshTarikh1.SelectedDate.Day;
                TarikhRoozeFarsi.Content = NamayeshTarikh1.SelectedDate.PersianDayOfWeek;

            }
            catch

            {

            }
        }

        private void test1_lb_Copy_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
            Module.PNumberTString Tabdil = new Module.PNumberTString();
            string horof = Convert.ToString(test1_lb_Copy.Content);
            string  res = horof.Replace("تومان", "");
            MessageBox.Show(Tabdil.num2str(Regex.Replace(res, @"\s+", "")), "مبلغ به حروف");
        }

        private void test_lb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Module.PNumberTString Tabdil = new Module.PNumberTString();
            string horof = Convert.ToString(test_lb.Content);
            string res = horof.Replace("تومان", "");
            MessageBox.Show(Tabdil.num2str(Regex.Replace(res, @"\s+", "")), "مبلغ به حروف");
        }

        private void test1_lb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Module.PNumberTString Tabdil = new Module.PNumberTString();
            string horof = Convert.ToString(test1_lb.Content);
            string res = horof.Replace("تومان", "");
            MessageBox.Show(Tabdil.num2str(Regex.Replace(res, @"\s+", "")), "مبلغ به حروف");
        }

        private void test1_lb_Copy2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Module.PNumberTString Tabdil = new Module.PNumberTString();
            string horof = Convert.ToString(test1_lb_Copy2.Content);
            string res = horof.Replace("تومان", "");
            MessageBox.Show(Tabdil.num2str(Regex.Replace(res, @"\s+", "")), "مبلغ به حروف");
        }

        private void test1_lb_Copy_MouseMove(object sender, MouseEventArgs e)
        {
            Module.PNumberTString Tabdil = new Module.PNumberTString();
            string horof = Convert.ToString(test1_lb_Copy.Content);
            string res = horof.Replace("تومان", "");
            string number = Regex.Replace(res, @"\s+", "");
            string spot = Tabdil.num2str(number);

            test1_lb_Copy.ToolTip = spot + " تومان";

        }

        private void test_lb_MouseMove(object sender, MouseEventArgs e)
        {
            Module.PNumberTString Tabdil = new Module.PNumberTString();
            string horof = Convert.ToString(test_lb.Content);
            string res = horof.Replace("تومان", "");
            string number = Regex.Replace(res, @"\s+", "");
            string spot = Tabdil.num2str(number);


            test_lb.ToolTip = spot + " تومان";
        }

        private void test1_lb_MouseMove(object sender, MouseEventArgs e)
        {
            Module.PNumberTString Tabdil = new Module.PNumberTString();
            string horof = Convert.ToString(test1_lb.Content);
            string res = horof.Replace("تومان", "");
            string number = Regex.Replace(res, @"\s+", "");
            string spot = Tabdil.num2str(number);

            test1_lb.ToolTip = spot + " تومان";
        }
    }
   
    }

