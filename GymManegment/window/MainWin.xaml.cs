using Arash;
using GymManegment.Module;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for MainWin.xaml
    /// </summary>
    public partial class MainWin : Window
    {

        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        protected string FarsiDaramad, MainDaramad, LastSubmiterString, FactorCodeString, SubmitersString, DeActivesString, PriceFinderString;
        int b;
        public int i;
        public int cpu;
        public MainWin()
        {

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            InitializeComponent();



            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += timer_Tick;
            timer.Start();


            DispatcherTimer timerr = new DispatcherTimer();








            timerr.Interval = TimeSpan.FromMilliseconds(800);
            timerr.Tick += timerr_Tick;
            timerr.Start();
            DispatcherTimer timerrr = new DispatcherTimer();

            timerrr.Interval = TimeSpan.FromSeconds(5);
            timerrr.Tick += timerrr_Tick;
            timerrr.Start();
         
            App.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            InitializeComponent();


        }

        public float getCurrentCpuUsagee()
        {

            return cpuCounter.NextValue();
        }
        int iaz = 1;
        void timerrr_Tick(object sender, EventArgs e)
        
        { 
           if (iaz == 1)
            {
               randomString();
               iaz = 2;
           }
            news.Content = RandomShowText();
        }
        void timerr_Tick(object sender, EventArgs e)
        {




            progressBar1.Value = (int)getCurrentCpuUsagee();


            MoveTest_Copy.Content = progressBar1.Value + "%";
            MoveTest_Copy1.Content = getAvailableRAM();


        }

        void timer_Tick(object sender, EventArgs e)
        {










            MoveTest.Content = DateTime.Now.ToString("HH:mm:ss");




        }


        public string getCurrentCpuUsage()
        {

            return cpuCounter.NextValue() + "%";
        }



        public string getAvailableRAM()
        {
            return ramCounter.NextValue() + " MB";
        }


        public string Username;
        public string Password;

        private string randomString()
        {

            


            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlCommand MainDaramadCMD = new SqlCommand("Select Sum(MemberCharge) from Members where MemberStartDate = '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(NowTime.Text)) + "'", con1);
            object resultsstt = MainDaramadCMD.ExecuteScalar();
            MainDaramad = Convert.ToString(resultsstt) + " : درامد امروز شما";

            SqlCommand LastSubmiter = new SqlCommand("Select (UserName + ' ' + UserFamily) from Users where UserID = (SELECT MAX(UserID) FROM Users)", con1);
            object LastSubmiterObj = LastSubmiter.ExecuteScalar();
            LastSubmiterString = "آخرین نفر ثبت نام کننده : " + Convert.ToString(LastSubmiterObj);
            Cal_Mount.SelectedDate = PersianDate.Today.AddDays(-30);

            SqlCommand FactorCode = new SqlCommand("Select Count(MemberFactorCode) from Members where MemberStartDate = '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(NowTime.Text)) + "'", con1);
            object FactorCodeObj = FactorCode.ExecuteScalar();
             FactorCodeString = Convert.ToString(FactorCodeObj) + " : تعداد فاکتور های ثبت شده ی امروز ";

            SqlCommand Submiters = new SqlCommand("Select count(UserID) from Users where UserRegDate between '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Cal_Mount.Text)) + "' And '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(NowTime.Text)) + "'", con1);
            object SubmitersObj = Submiters.ExecuteScalar();
             SubmitersString = Convert.ToString(SubmitersObj) + " : تعداد افرادی که این ماه ثبت نام کرده اند";


            SqlCommand DeActives = new SqlCommand("Select count(UserID) from Users where UserActive = 2", con1);
            object DeActivesObj = DeActives.ExecuteScalar();
             DeActivesString = Convert.ToString(DeActivesObj) + " : افراد غیر فعال  ";

            SqlCommand PriceFinder = new SqlCommand("Select Sum(MemberCharge) from Members", con1);
            object PriceFinderObj = PriceFinder.ExecuteScalar();
            string PriceFinderString = Convert.ToString(PriceFinderObj) + " : کل درامد شما   ";
            Module.PNumberTString farsi = new PNumberTString();
            SqlCommand PriceFinders = new SqlCommand("Select Sum(MemberCharge) from Members where MemberStartDate = '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(NowTime.Text)) + "'", con1);
            object PriceFinderObjs = PriceFinders.ExecuteScalar();
             FarsiDaramad = "درامد امروز شما : " + farsi.num2str(PriceFinderObjs.ToString()) + " تومان ";



            var names = new List<string> { FarsiDaramad, MainDaramad, LastSubmiterString, FactorCodeString, SubmitersString, DeActivesString, PriceFinderString };

            Random random = new Random();
            int index = random.Next(names.Count);
            var name = names[index];
            names.RemoveAt(index);

            return name;

        }

        private string RandomShowText()
        {
            var names = new List<string> { FarsiDaramad, MainDaramad, LastSubmiterString, FactorCodeString, SubmitersString, DeActivesString, PriceFinderString };

            Random random = new Random();
            int index = random.Next(names.Count);
            var name = names[index];
            names.RemoveAt(index);

            return name;

        }

            private void Ribbon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn_exit_click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void ribbon_Loaded(object sender, RoutedEventArgs e)
        {
            //   SizeFix();

        }

        private void MoshtariView_Win(object sender, RoutedEventArgs e)
        {
            try
            {


                Users w_user = new Users();
                w_user.Show();

            }

            catch
            {

                ErrorPage ER = new ErrorPage();
                ER.Show();
                ER.Error_Lable.Content = "مشکل در ارتباط با پایگاه ";

            }

        }

        private void btn_salamati(object sender, RoutedEventArgs e)
        {

            Form2 F2 = new Form2();
            F2.Show();

        }
        private void ShowFactors(object sender, RoutedEventArgs e)
        {

            PriceStory s = new PriceStory();
            s.Show();

        }

        private void btn_profile_user(object sender, RoutedEventArgs e)
        {
            //       MemberCharge w_user = new MemberCharge();


        }

        private void AxeBedehkaran(object sender, RoutedEventArgs e)
        {
            BedehiImage b = new BedehiImage();

            try
            {

                b.Show();
            }
            catch
            {

                throw;
            }

        }

        private void Btn_customerShow_click(object sender, RoutedEventArgs e)
        {
            win_customer w_customer = new win_customer();
            w_customer.Show();

        }
        private void Daramads(object sender, RoutedEventArgs e)
        {
            Daramad D = new Daramad();
            D.Show();

        }
        private void History(object sender, RoutedEventArgs e)
        {
            Win_History D = new Win_History();
            D.Show();

        }
        private void VarzeshkarView_win(object sender, RoutedEventArgs e)
        {
            AddUser w_customer = new AddUser();
            w_customer.Show();

        }

        private void FactorSubmits(object sender, RoutedEventArgs e)
        {
            SubmitFactor s = new SubmitFactor();
            s.Show();
        }
        private void VoroduKhoruj(object sender, RoutedEventArgs e)
        {
            ControlPanel W = new ControlPanel();
            W.Show();
        }
        private void Barname_win(object sender, RoutedEventArgs e)
        {
            win_tamrin t = new win_tamrin();
            t.Show();
        }
        private void TamrinatShow(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            ControlPanel W = new ControlPanel();
            W.Show();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

        }
        private void SizeFix()
        {
            //       this.MaxHeight = 639.74;
            //    this.MinHeight = 639.74;
            //    this.MaxWidth = 1127.503;
            //    this.MinWidth = 1127.503;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            SetDates.Content = Cal.SelectedDate.PersianDayOfWeek + " , " + Cal.SelectedDate.Day + " " + Cal.SelectedDate.MonthAsPersianMonth + " " + Cal.SelectedDate.Year;
        }


        private void lbl_name_Loaded(object sender, RoutedEventArgs e)
        {


            lbl_name.Content = Username;
        }
        public void CpuShow()
        {       //CPU Settings ---------------------  Show - hidden

            RegistryKey UsernameKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\GYM");
            if ((string)UsernameKey.GetValue("CpuShowMain") == "False")
            {

                new Thread((ThreadStart)delegate
                {
                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        progressBar1.Visibility = Visibility.Hidden;


                    });

                }).Start();

                new Thread((ThreadStart)delegate
                {

                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        MoveTest_Copy.Visibility = Visibility.Hidden;

                    });

                }).Start();

                new Thread((ThreadStart)delegate
                {

                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        MoveTest_Copy1.Visibility = Visibility.Hidden;
                    });

                }).Start();
                new Thread((ThreadStart)delegate
                {

                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        //   CPURR.Visibility = Visibility.Hidden;
                    });
                }).Start();
            }
            else if ((string)UsernameKey.GetValue("CpuShowMain") == "True")
            {





                new Thread((ThreadStart)delegate
                {
                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        progressBar1.Visibility = Visibility.Visible;


                    });

                }).Start();

                new Thread((ThreadStart)delegate
                {

                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        MoveTest_Copy.Visibility = Visibility.Visible;

                    });

                }).Start();

                new Thread((ThreadStart)delegate
                {

                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        MoveTest_Copy1.Visibility = Visibility.Visible;
                    });

                }).Start();
                new Thread((ThreadStart)delegate
                {

                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        //CPURR.Visibility = Visibility.Visible;
                    });
                }).Start();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CpuShow();
            try
            {
                SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
                con1.Open();
                SqlCommand Commandcmd = new SqlCommand("SELECT (UserImage) FROM Users WHERE UserID = (SELECT MAX(UserID) FROM Users)", con1);
                SqlDataReader rdr1 = null;
                rdr1 = Commandcmd.ExecuteReader();



                if (Username == null || Password == null)
                {
                    System.Environment.Exit(0);
                }
            }
            catch (Exception x)
            {

            }
        }

        private void lbl_name_Loaded_1(object sender, RoutedEventArgs e)
        {
            lbl_name.Content = Username;
        }
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show(string.Format("An error occured: {0}", e.Exception.Message), "Error");
            e.Handled = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //       App.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
            System.Environment.Exit(0);
        }





        private void MyImage_MouseEnter(object sender, MouseEventArgs e)
        {


            Image img = (Image)sender;
            DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(2));
            img.BeginAnimation(Image.OpacityProperty, animation);
        }

        private void MyImage_MouseLeave(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(2));
            img.BeginAnimation(Image.OpacityProperty, animation);
        }


        private void news_MouseDown(object sender, MouseButtonEventArgs e)
        {
            news.Content = randomString();
        }

        private void Rectangle_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void TextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void TextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BlurImage_MouseEnter(object sender, MouseEventArgs e)
        {


        }

        private void progressBar1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (progressBar1.Value >= 80)
            {
                progressBar1.Foreground = Brushes.Red;
            }
            else
            {

            }


        }


        private void AfradeShahrie(object sender, RoutedEventArgs e)
        {
            WindowShahrie s = new WindowShahrie();
            s.Show();
        }

        private void Settings_Show(object sender, RoutedEventArgs e)
        {
            Settings s = new Settings();
            s.Show();
        }


        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            ChangePasswords p = new ChangePasswords();
            p.Username = Convert.ToString(lbl_name.Content);
            p.Show();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            randomString();
        }
    }
}
