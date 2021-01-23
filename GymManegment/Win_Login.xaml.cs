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
using GymManegment.window;
using Microsoft.Win32;
using DataModelLayer;
using System.Data.SqlClient;
using Arash;
using System.Text.RegularExpressions;
using GymManegment.Module;
using System.Diagnostics;

namespace GymManegment
{
   
    /// <summary>
    /// Interaction logic for Win_Login.xaml
    /// </summary>
    public partial class Win_Login : Window
    {
        public Win_Login()
        {
            InitializeComponent();
        }
    
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
         
        }
        bool login  = false;
        public async Task GetAccounts()
        {

            
            int total1 = 0;
            using (SqlConnection connection = new SqlConnection(PublicVar.ConnectionString))
            {
                gymEntities2 database = new gymEntities2();
                SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
                PublicVar.TodayTime = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(TimeNow.Text));
                con1.Open();


                SqlCommand Actives = new SqlCommand("Select DISTINCT (LockEndDate) from LockTable Where Username = '" + txt_username.Text + "' and Password = '" + txt_password.Password + "'", con1);
                object Active = Actives.ExecuteScalar();
                string SystemActive = Convert.ToString(Active);
                MainWin w = new MainWin();

                //   SqlCommand Commandcmds = new SqlCommand("update VW_TimeOut set UserActive = 2 where UserEndDate < '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(TimeNow.Text)) + "'", con1);
                //   Commandcmds.ExecuteScalar();


                SqlCommand Commandcmd = new SqlCommand("SELECT COUNT(*) FROM LockTable Where Username = '" + txt_username.Text + "' and Password = '" + txt_password.Password + "' and LockEndDate between '" + String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Lock.Text)) + "' And '" + SystemActive + "'", con1);
                int userCount = (int)Commandcmd.ExecuteScalar();


                if (userCount > 0)
                {
                    try
                    {
                        RegistryKey UsernameKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\GYM");





                        if (CheakRem.IsChecked == true)
                            if ((string)UsernameKey.GetValue("UserNameRegister") != "")
                            {
                                UsernameKey.SetValue("UserNameRegister", txt_username.Text.Trim());
                                UsernameKey.SetValue("PasswordRegister", Module.Decode.EncryptTextUsingUTF8(txt_password.Password.Trim()));
                            }

                        login = true;

                    }

                    catch
                    {
                        MessageBox.Show("مشکلی در ورود کاربر به وجود آمد");
                        w.Username = null;
                        w.Password = null;
                    }


                }
                else
                {
                    ErrorPage pageerror = new ErrorPage();
                    pageerror.Show();
                    pageerror.Error_Lable.Content = "زمان اکانت شما به پایان رسیده است";
                    con1.Close();
                    w.Username = null;
                    w.Password = null;
                }
                con1.Close();

                
            }
         
        }




        private async void btn_join_Click(object sender, RoutedEventArgs e)
        {
            MainWin w = new MainWin();
            await GetAccounts();
            if (login == true)
            {
                w.Username = txt_username.Text;
                w.Password = txt_password.Password;
                w.Show();
                this.Close();
            }
        

        }

        private void lbl_url_MouseMove(object sender, MouseEventArgs e)
        {
           // lbl_url.Foreground = Brushes.Yellow;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //دستورات ریجستری
            
            RegistryKey masterKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\GYM");
            txt_username.Text = (string)masterKey.GetValue("UserNameRegister");
            
            //دستورات تاریخ
            SetDate();
            txt_password.Focus();

           
            if ((string)masterKey.GetValue("UserNameRegister") != "")
            {
                CheakRem.IsChecked = true;
            }

        }

        private void SetDate()
            {
            day_lbl.Content = Cal.SelectedDate.PersianDayOfWeek;
            DayNumber_lbl.Content = Cal.SelectedDate.Day;
            Typical_lbl.Content = Cal.SelectedDate.MonthAsPersianMonth;
            YearNumber.Content = Cal.SelectedDate.Year;
         // Typical_lbl_Copy.Content = DateTime.Today.PersianMonthName();
            }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void txt_username_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
          
        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            ImgOzviat.Source = new BitmapImage(new Uri("/1/2122.png", UriKind.Relative));
          
      
        }

        private void Rectangle_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            ImgOzviat.Source = new BitmapImage(new Uri("/1/Untitled-12131231231231231231221312312ییظط312.png", UriKind.Relative));
        }

        private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Image_MouseMove_1(object sender, MouseEventArgs e)
        {
            ImgOzviat.Source = new BitmapImage(new Uri("/1/Untitled-12131231231231231231221312312ییظط312.png", UriKind.Relative));
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
         //   ImgOzviat.Source = new BitmapImage(new Uri("/1/Untitled-12131231231231231231221312312ییظط312.png", UriKind.Relative));
        }

        private void ImgOzviat_MouseLeave(object sender, MouseEventArgs e)
        {
            ImgOzviat.Source = new BitmapImage(new Uri("/1/Untitled-12131231231231231231221312312ییظط312.png", UriKind.Relative));
        }

        private void Image_MouseMove_2(object sender, MouseEventArgs e)
        {
            ExitButn.Source = new BitmapImage(new Uri("img/exit.png", UriKind.Relative));
        }

        private void ExitButn_MouseLeave(object sender, MouseEventArgs e)
        {
            ExitButn.Source = new BitmapImage(new Uri("/1/276363.png", UriKind.Relative));

        }

        private void RibbonCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ImgOzviat_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ImgOzviat_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Regs reg = new Regs();
            reg.Show();
        }

        private void ImgOzviat_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            // add tabitem 
            Regs reg = new Regs();
            reg.Show();
            this.Hide();
        }

        private void Label_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
           AddCustomer s = new AddCustomer();
            s.Show();
        }

        private void Parametr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ParametrServer s = new ParametrServer();
            s.Show();
        }
    

          private void SetStartup()
          {
            RegistryKey UsernameKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\GYM");
            string get = (string)  UsernameKey.GetValue("AutomaticStartUp");
            if ((string)UsernameKey.GetValue("AutomaticStartUp") != "True" && (string)UsernameKey.GetValue("AutomaticStartUp") != "False")
            {
                UsernameKey.SetValue("AutomaticStartUp", "");
            }
            Settings s = new Settings();
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if ((string)rk.GetValue(Process.GetCurrentProcess().MainModule.FileName) != "" && (string)UsernameKey.GetValue("AutomaticStartUp") == "")
            {
               rk.SetValue(Application.Current.MainWindow.GetType().Assembly.ToString(), Process.GetCurrentProcess().MainModule.FileName);
            }
          }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            RegistryKey UsernameKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\GYM");
            SetStartup();
            String thisprocessname = Process.GetCurrentProcess().ProcessName;

            if ((string)UsernameKey.GetValue("AutoLogin") == "true")
            {
                txt_password.Password = Module.Decode.DecryptTextUsingUTF8((string)UsernameKey.GetValue("PasswordRegister"));
            }


           else if ((string)UsernameKey.GetValue("AutoLogin") == "false")
            {
                txt_password.Password = "";
            }


            if (Process.GetProcesses().Count(p => p.ProcessName == thisprocessname) > 1)
                return;
            btn_join.IsDefault = true;
        }

        private void ExitButn_MouseEnter(object sender, MouseEventArgs e)
        {
    
        }
    }
}
