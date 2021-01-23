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
using GymManegment.Module;
using System.Text.RegularExpressions;

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for ChangePasswords.xaml
    /// </summary>
    public partial class ChangePasswords : Window
    {
        public ChangePasswords()
        {
            InitializeComponent();
        }
        gymEntities2 database = new gymEntities2();
      
        public string Username;
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UsernameID.Text = Username;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswords w = new ChangePasswords();
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            con1.Open();

            SqlCommand myCommand = new SqlCommand("Select (Password) from LockTable where Username = '" + UsernameID.Text.Trim() + "'", con1);
            object result = myCommand.ExecuteScalar();
            string OldPassword = Regex.Replace(Convert.ToString(result), @"\s+", "");



         

            if (PasswordID.Text.Trim() != "" && PasswordID2.Text.Trim() != "" && CurrentPassword.Text.Trim() != "")
            {
        
                if (CurrentPassword.Text.Trim() == OldPassword) // true
                {
                
                    if (PasswordID.Text.Trim() == PasswordID2.Text.Trim()) // true
                    {
                       
                     
                        string newpassword = PasswordID.Text.Trim();
                        SqlCommand Commandcmdsz = new SqlCommand("update LockTable set Password = '" + newpassword + "' Where Username = '" + UsernameID.Text.Trim() + "'", con1);
                        Commandcmdsz.ExecuteScalar();
                        MessageBox.Show("رمز عبور شما با موفقیت تغیر کرد", "اطلاعات اکانت");
                        con1.Close();
                        Close();

                    }
                    else
                    {
                        MessageBox.Show("رمز های عبور یکسان نیستند");
                        PasswordID.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("رمز عبور قبلی شما درست نیست", "اطلاعات اکانت");
                    CurrentPassword.Focus();
                }
            }
            else
            {
                MessageBox.Show("قسمت های خالی را پر کنید");
                PasswordID.Focus();
                PasswordID2.BorderBrush = Brushes.Red;
                PasswordID.BorderBrush = Brushes.Red;
            }
            con1.Close();

        }

        private void PasswordID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
