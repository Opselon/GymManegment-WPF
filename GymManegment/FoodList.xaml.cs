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
using System.Globalization;
using System.Data;

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for FoodList.xaml
    /// </summary>
    public partial class FoodList : Window
    {
        public FoodList()
        {
            InitializeComponent();
        }
        //     Ghaad   Vazn    
        public double height, weight, age, malebmr, NewUserID; //variables
        public byte gender, exercise;

        gymEntities2 database = new gymEntities2();
        int totalcalery;


        public void needcolori()
        {
            /* 
            height = double.Parse(heightTextBox.Text);
            weight = double.Parse(weightTextBox.Text);
            age = double.Parse(ageTextBox.Text);
            */
            switch (gender)
            {
                case 0:
                    CaleryNeed.Content = (weight * 10 + height * 6.25 - age * 5 - 5);
                    break;
                case 1:
                    CaleryNeed.Content = weight * 10 + height * 6.25 - age * 5 - 161;
                    break;
            }


        }


        public void ShowUserInfo(Func<string> SearchStringForUser)
        {

            var query = database.Database.SqlQuery<VW_Foods>("select * From VW_Foods where 1 = 1 " + SearchStringForUser());
            //  MessageBox.Show(query.ToString());
            var u = query.ToList();
            DataGrid_User.ItemsSource = u;

        }
        public string searchstate()
        {

            string searchstring = "";


            if (food_name.Text != "")
            {
                searchstring += " And vegetablesName like '%" + food_name.Text.Trim() + "%'";
            }
            if (FoodFindID_txt.Text != "")
            {
                searchstring += " And vegetablesID like '%" + FoodFindID_txt.Text + "%'";
            }
            if (FoodChoice.SelectedIndex == 1)
            {
                searchstring += " And Finder = 1";
            }
            if (FoodChoice.SelectedIndex == 2)
            {
                searchstring += " And Finder = 2";
            }
            if (FoodChoice.SelectedIndex == 3)
            {
                searchstring += " And Finder = 3";
            }





            return searchstring;
        }

        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }

        private void DataGrid_User_Loaded(object sender, RoutedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void DataGrid_User_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            object itemOne = DataGrid_User.SelectedItem;

             if(itemOne != null)
            {




            string idfinder = ((DataGrid_User.SelectedCells[0].Column.GetCellContent(itemOne) as TextBlock).Text);
            string strname = (DataGrid_User.SelectedCells[1].Column.GetCellContent(itemOne) as TextBlock).Text;
            ThisName.Content = strname;
            string strcallry = (DataGrid_User.SelectedCells[3].Column.GetCellContent(itemOne) as TextBlock).Text;
            MyVitamin.Content = strcallry;
            MyDesc.Text = (DataGrid_User.SelectedCells[4].Column.GetCellContent(itemOne) as TextBlock).Text;
            CallryShow.Value = Convert.ToInt32((DataGrid_User.SelectedCells[2].Column.GetCellContent(itemOne) as TextBlock).Text);










            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            con1.Open();
            try
            {
                SqlCommand Commandcmd = new SqlCommand(
                      "SELECT (vegetablesImage) FROM vegetablesTable WHERE vegetablesID = " + idfinder,
                      con1);
                SqlDataReader rdr1 = null;
                rdr1 = Commandcmd.ExecuteReader();
                while (rdr1.Read())
                {

                    Images.Fill = new ImageBrush
                    {
                        //       ImageSource = new BitmapImage(new Uri("/pic/User.jpg", UriKind.Relative))
                    };
                    if (rdr1 != null)
                    {

                        byte[] data = (byte[])rdr1[0]; // 0 is okay if you only selecting one column
                                                       //And use:
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(data))
                        {
                            var imageSource = new BitmapImage();
                            imageSource.BeginInit();
                            imageSource.StreamSource = ms;
                            imageSource.CacheOption = BitmapCacheOption.OnLoad;
                            imageSource.EndInit();

                            // Assign the Source property of your image
                            //  MyImage.Source = imageSource;
                            Images.Fill = new ImageBrush
                            {
                                ImageSource = imageSource
                            };
                        }
                    }
                }
            }













            catch (Exception excepstion)
            {
                Images.Fill = new ImageBrush
                {
                    //  ImageSource = new BitmapImage(new Uri("/pic/download.png", UriKind.Relative))
                };
            }
                //  0.4580 x 100 = 45.80 %
            }

        }


        private void totalconverter()
        {

            object itemOne = DataGrid_User.SelectedItem;
            CaleryTotal.Content = totalcalery += Convert.ToInt32((DataGrid_User.SelectedCells[2].Column.GetCellContent(itemOne) as TextBlock).Text);
        }


        private void SobhaneClick(object sender, RoutedEventArgs e)
        {
            totalconverter();
            object itemOne = DataGrid_User.SelectedItem;
            Sobhane_text.Text += (DataGrid_User.SelectedCells[1].Column.GetCellContent(itemOne) as TextBlock).Text + " | ";
        }




        private void showinfromation()
        {
            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            DataTable dt = new DataTable();
            con1.Open();
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("select * From VW_Users where 1 = 1 and UserID = " + NewUserID, con1);
            myReader = myCommand.ExecuteReader();
            try
            {

                SqlCommand Commandcmd = new SqlCommand(
                    "SELECT (UserImage) FROM Users WHERE UserID = " + NewUserID,
                    con1);
                SqlDataReader rdr1 = null;
                rdr1 = Commandcmd.ExecuteReader();
                //  MyImage.Source = new BitmapImage(new Uri("/pic/users.jpg", UriKind.Relative));




                while (rdr1.Read())
                {
                    // MyImage.Source = new BitmapImage(new Uri("/pic/user.jpg", UriKind.Relative));
                    if (rdr1 != null)
                    {
                        byte[] data = (byte[])rdr1[0]; // 0 is okay if you only selecting one column
                                                       //And use:
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(data))
                        {
                            var imageSource = new BitmapImage();
                            imageSource.BeginInit();
                            imageSource.StreamSource = ms;
                            imageSource.CacheOption = BitmapCacheOption.OnLoad;
                            imageSource.EndInit();

                            MyImage.Fill = new ImageBrush
                            {
                                ImageSource = imageSource
                            };


                        }


                    }


                }


            }
            catch (Exception exception)
            {
                //   MyImage.Source = new BitmapImage(new Uri("/pic/User.jpg", UriKind.Relative));

            }
            while (myReader.Read())
            {


                Name_lbl.Content = (myReader["UserName"].ToString());
                Family_lbl.Content = (myReader["UserFamily"].ToString());
                Age_lbl.Content = (myReader["UserAge"].ToString());
                Ghaad_lbl.Content = (myReader["userGhaad"].ToString());
                Vazn_lbl.Content = (myReader["userSize"].ToString());

            }

            con1.Close();

        }





        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            showinfromation();

            needcolori();
        }




        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }
        private void FoodFindID_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }
        private void FoodChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {




        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            // insert to database FoodTemplates
            FoodTemplates F = new FoodTemplates()
            {
                Sobhane = Sobhane_text.Text,
                Nahar = Nahar_text.Text,
                Asrane = Asrane_text.Text,
                Sham = Sham_text.Text,
                AsraneDesc = AsraneDesc.Text,
                SobhaneDesc = SobhaneDesc.Text,
                NaharDesc = NaharDesc.Text,
                ShamDesc = ShamDesc.Text,
                AllCallory = Convert.ToInt32(CaleryTotal.Content),
                TemplateName = NameFoods.Text,
               // VitaminUsed = Vitamins.Text
        
            };
            MessageBox.Show("برنامه غذایی ذخیره شد");
              database.FoodTemplates.Add(F);
              database.SaveChanges();





        }

        private void BMI_lbl_Loaded(object sender, RoutedEventArgs e)
        {





        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            SobhaneImage.Source = new BitmapImage(new Uri("/img/LF1.png", UriKind.Relative));
        }

        private void SobhaneImage_MouseLeave(object sender, MouseEventArgs e)
        {
            SobhaneImage.Source = new BitmapImage(new Uri("/img/WhiteSnow.png", UriKind.Relative));
        }

        private void SobhaneImage_Copy_MouseEnter(object sender, MouseEventArgs e)
        {
            SobhaneImage_Copy.Source = new BitmapImage(new Uri("/img/LF2.png", UriKind.Relative));
        }

        private void SobhaneImage_Copy_MouseLeave(object sender, MouseEventArgs e)
        {
            SobhaneImage_Copy.Source = new BitmapImage(new Uri("/img/WhiteSnow.png", UriKind.Relative));
        }

        private void SobhaneImage_Copy1_MouseEnter(object sender, MouseEventArgs e)
        {
            SobhaneImage_Copy1.Source = new BitmapImage(new Uri("/img/LF3.png", UriKind.Relative));
        }

        private void SobhaneImage_Copy1_MouseLeave(object sender, MouseEventArgs e)
        {
            SobhaneImage_Copy1.Source = new BitmapImage(new Uri("/img/WhiteSnow.png", UriKind.Relative));
        }

        private void SobhaneImage_Copy2_MouseEnter(object sender, MouseEventArgs e)
        {
            SobhaneImage_Copy2.Source = new BitmapImage(new Uri("/img/LF4.jpg", UriKind.Relative));
        }

        private void SobhaneImage_Copy2_MouseLeave(object sender, MouseEventArgs e)
        {
            SobhaneImage_Copy2.Source = new BitmapImage(new Uri("/img/WhiteSnow.png", UriKind.Relative));
        }

        private void SobhaneImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            if (SobhaneDesc.Visibility == Visibility.Visible)
            {
                SobhaneDesc.Visibility = Visibility.Hidden;
            }
            else
              SobhaneDesc.Visibility = Visibility.Visible;
        }

        private void SobhaneImage_Copy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (NaharDesc.Visibility == Visibility.Visible)
            {
                NaharDesc.Visibility = Visibility.Hidden;
            }
            else
                NaharDesc.Visibility = Visibility.Visible;
        }

        private void SobhaneImage_Copy1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (AsraneDesc.Visibility == Visibility.Visible)
            {
                AsraneDesc.Visibility = Visibility.Hidden;
            }
            else
                AsraneDesc.Visibility = Visibility.Visible;
        }

        private void SobhaneImage_Copy2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ShamDesc.Visibility == Visibility.Visible)
            {
                ShamDesc.Visibility = Visibility.Hidden;
            }
            else
                ShamDesc.Visibility = Visibility.Visible;
        }

        //img/left-arrow-button (1).png

        private void NaharClick(object sender, RoutedEventArgs e)
        {
            totalconverter();
            object itemOne = DataGrid_User.SelectedItem;
            Nahar_text.Text += (DataGrid_User.SelectedCells[1].Column.GetCellContent(itemOne) as TextBlock).Text + " | ";


        }
        private void AsraneClick(object sender, RoutedEventArgs e)
        {


            totalconverter();
            object itemOne = DataGrid_User.SelectedItem;
            Asrane_text.Text += (DataGrid_User.SelectedCells[1].Column.GetCellContent(itemOne) as TextBlock).Text + " | ";


        }
        private void ShamClick(object sender, RoutedEventArgs e)
        {
            totalconverter();
            object itemOne = DataGrid_User.SelectedItem;
            Sham_text.Text += (DataGrid_User.SelectedCells[1].Column.GetCellContent(itemOne) as TextBlock).Text + " | ";


        }



        private void GroupBox_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


            object itemOne = DataGrid_User.SelectedItem;
            if (itemOne != null)
            {
                int finderID = Convert.ToInt32((DataGrid_User.SelectedCells[0].Column.GetCellContent(itemOne) as TextBlock).Text);


                // ---------------------------- LINQ Codes Update Table --------------------------------
                vegetablesTable vs = database.vegetablesTable.First(c => c.vegetablesID == finderID);
                vs.vegetablesDesc = MyDesc.Text;
                database.SaveChanges();
                MessageBox.Show("با موفقیت توضیحات به روز شد");
                ShowUserInfo(searchstate);
            
                // --------------------------------------------------------------------------------------
            }
            else
            {
                MessageBox.Show("شما هیچ چیز را انتخاب نکرده اید.");

            }
        }
    }
    
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                