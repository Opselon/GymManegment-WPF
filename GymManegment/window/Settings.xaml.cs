using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RegistryKey UsernameKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\GYM");
            UsernameKey.SetValue("AutoLogin", "true");
       
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RegistryKey UsernameKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\GYM");
            UsernameKey.SetValue("AutoLogin", "false");
         
     
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {




            RegistryKey UsernameKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\GYM");
            if ((string)UsernameKey.GetValue("AutoLogin") == "true")
            {
                AutoRun.IsChecked = true;
            }



            if ((string)UsernameKey.GetValue("CpuShowMain") == "True")
            {
                CpuActive.IsChecked = true;
            }
           if ((string)UsernameKey.GetValue("CpuShowMain") == "False")
            {
                CpuActive.IsChecked = false;
            }




            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if ((string)rk.GetValue("GymManegment, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null","") != "")
            {
                StartActive.IsChecked = true;

            }
            else if ((string)rk.GetValue("GymManegment, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null","") == "")
            StartDeActive.IsChecked = true;

        }
        private void SetStartup()
        {

            RegistryKey rk = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);


            if ((string)rk.GetValue(Process.GetCurrentProcess().MainModule.FileName) != "")
            {
              
                rk.SetValue(Application.Current.MainWindow.GetType().Assembly.ToString(), Process.GetCurrentProcess().MainModule.FileName);

            }

        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RegistryKey UsernameKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\GYM");
            UsernameKey.SetValue("AutomaticStartUp", "True");
            SetStartup();
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            RegistryKey UsernameKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\GYM");
            UsernameKey.SetValue("AutomaticStartUp", "False");
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rk.SetValue(Application.Current.MainWindow.GetType().Assembly.ToString(), "");
        }

        private void CpuActive_Checked(object sender, RoutedEventArgs e)
        {
            MainWin w = new MainWin();
            w.CpuShow();
            RegistryKey UsernameKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\GYM");
            UsernameKey.SetValue("CpuShowMain" , "True");
        }

        private void CpuActive_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWin w = new MainWin();
            w.CpuShow();
            RegistryKey UsernameKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\GYM");
            UsernameKey.SetValue( "CpuShowMain" , "False" );
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
