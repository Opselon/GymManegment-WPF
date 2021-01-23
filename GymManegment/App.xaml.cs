using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GymManegment.window;
using System.Threading;
using System.Globalization;

namespace GymManegment
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fa-IR");
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
        }

        private void App_StartUp(object sender, StartupEventArgs e)
        {
            Win_Login w_n = new Win_Login();
            w_n.ShowDialog();

           
        }

      
    }
}
 