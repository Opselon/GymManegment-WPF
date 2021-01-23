using DataModelLayer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for SendFood.xaml
    /// </summary>
    public partial class SendFood : Window
    {
        string StrName;
        string ImageName = "";
        string nums;
        public SendFood()
        {
            InitializeComponent();
        }
        string port = "";
        gymEntities2 database = new gymEntities2();

        private bool Cheakable()
        {
            if (FruitName.Text == "")
            {
                MessageBox.Show("نام نباید خالی بماند");
                return false;
            }
            else if (FrutCalorie.Text == "")
            {
                MessageBox.Show("مقدار کالری نباید خالی باشد");
                return false;
            }

            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ImageName != "")
            {
                FileStream fs = new FileStream(ImageName, FileMode.Open, FileAccess.Read);
                byte[] imgByteArr = new byte[fs.Length];
                fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
                fs.Close();


                database.Sp_vegetables(FruitName.Text, Convert.ToInt32(FrutCalorie.Text), FruitDesc.Text, "1", imgByteArr, Sec.Text, FoodChoice.SelectedIndex + 1);
                database.SaveChanges();

            }
            else
                database.Sp_vegetables(FruitName.Text, Convert.ToInt32(FrutCalorie.Text), FruitDesc.Text, "1", null, Sec.Text, FoodChoice.SelectedIndex + 1);
            database.SaveChanges();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                OpenFileDialog dialog = new OpenFileDialog
                {
                    CheckFileExists = true,
                    Multiselect = false,
                    Filter = "Images (*.jpg,*.png)|*.jpg;*.png|All Files(*.*)|*.*"
                };

                dialog.ShowDialog();
                {
                    StrName = dialog.SafeFileName;
                    ImageName = dialog.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    if (ImageName != "")
                    {
                        MyImage.SetValue(System.Windows.Controls.Image.SourceProperty, isc.ConvertFromString(ImageName));
                        //  ImagePath.Text = ImageName.ToString();
                    }
                }
            }
            catch
            {
                ErrorPage Er = new ErrorPage();
                Er.Show();
                Er.Error_Lable.Content = "مشکلی در سیستم به وجود آمده است";
            }
        }
        private void FoodChoice_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e, Func<string> SearchStringForUser)
        {


        }

        private void FoodChoice_Copy_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {


            String sValue = FoodChoice_Copy.SelectedItem.ToString();
            nums = sValue.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            port = port + sValue.Replace("System.Windows.Controls.ComboBoxItem: ", "") + ",";

        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            Sec.Text = port;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {


            //try
            //{


            //    if (Sec.Text != "")
            //    {
            //        if (nums.Length == 1)
            //        {
            //            Sec.Text = port.Remove(port.Length - 2);
            //            port = Sec.Text;
            //        }



            //        if (nums.Length == 2)
            //        {
            //            Sec.Text = port.Remove(port.Length - 3);
            //            port = Sec.Text;
            //        }


            //    }
            //}
            //catch
            //{


            Sec.Text = "";
            // }

            port = "";
        }
    }
}
