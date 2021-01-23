using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
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
using GymManegment.Module;
using System.IO;
using System.Security.Cryptography;


namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for ParametrServer.xaml
    /// </summary>
    public partial class ParametrServer : Window
    {
        public ParametrServer()
        {
            InitializeComponent();
        }

        private void ExitButn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }


        public static string BuildEntityConnection(string ADOconnectingstring)
        {
            var EntityConnection = new EntityConnectionStringBuilder
            {
                Provider = "System.Data.SqlClinet",
                ProviderConnectionString = ADOconnectingstring,
                Metadata = "res://*"
            };
            return EntityConnection.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var entityConnectingString = "Data Source =" + ServerIP.Text.Trim() +
                "; Initial catalog=" + ServerName.Text.Trim()+"; user Id =" +
                " sa;" +
                "password=" + ServerPassword.Password.Trim() + ";integrated security = false";

            try
            {
                RegistryKey ConnectionKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\GYM");
                try
                {
                    ConnectionKey.SetValue("CreateConnectiong",Decode.EncryptTextUsingUTF8(entityConnectingString));

                }
                catch
                {
                    MessageBox.Show("در ارتباط با سرور مشکلی به وجود آمده است");

                }
                finally
                {
                    ConnectionKey.Close();
                }
                MessageBox.Show("ارتباط با سرور برقرار شد");
                this.Close();
            }
            catch
            {

            }
            finally
            {

            }
        }
    }
}
