using DataModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Telerik.Windows.Controls;

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for TelerikScenario2.xaml
    /// </summary>
    public partial class TelerikScenario2
    {
        public TelerikScenario2()
        {
            InitializeComponent();
        }
        gymEntities2 database = new gymEntities2();





        public void ShowUserInfo(Func<string> SearchStringForUser)
        {

            var query = database.Database.SqlQuery<ListMove>("select * From ListMove where 1 = 1 " + SearchStringForUser());
            //  MessageBox.Show(query.ToString());
            var u = query.ToList();
            DataGrid_User.ItemsSource = u;

        }






        private void DataGrid_User_Loaded(object sender, RoutedEventArgs e )
        {

















            ShowUserInfo(searchstate);







        }
        public string searchstate()
        {

            string searchstring = "";

            return searchstring;
        }

        private void DataGrid_User_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object itemOne = DataGrid_User.SelectedItem;
            string lblid = (DataGrid_User.SelectedCells[0].Column.GetCellContent(itemOne) as TextBlock).Text;
            ID_lbl.Content = lblid;
            string tozihat = (DataGrid_User.SelectedCells[1].Column.GetCellContent(itemOne) as TextBlock).Text;
            Harekat_lbl.Text = tozihat;
        }

        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(" آیا میخواهید " + Harekat_lbl.Text + " به حرکات شما اضافه شود ؟ ", "اطلاعیه", System.Windows.Forms.MessageBoxButtons.YesNo , System.Windows.Forms.MessageBoxIcon.Question);

                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    database.Moving(0, Harekat_lbl.Text);
                    database.SaveChanges();
                    ShowUserInfo(searchstate);
                }
         
            }
            catch
            {
                MessageBox.Show("error");
            }
        }

        private void RadButton_Click_1(object sender, RoutedEventArgs e)
        {
            Harekat_lbl.Text = "";
            Harekat_lbl.Focus();
        }
    }
}
