using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
using GymManegment.Module;

namespace GymManegment.window.Morabi
{
    /// <summary>
    /// Interaction logic for DashboardMain.xaml
    /// </summary>
    public partial class DashboardMain : Window
    {

        gymEntities2 database = new gymEntities2();

        public DashboardMain()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            AddMorabi a = new AddMorabi();
            a.Show();
        }
        public void ShowUserInfo(Func<string> SearchStringForUser)
        {

            var query = database.Database.SqlQuery<MorabiTable_View>("select * From MorabiTable_View where 1 = 1 " + SearchStringForUser());
            //  MessageBox.Show(query.ToString());
            var u = query.ToList();
            DataGrid_User.ItemsSource = u;

        }

        public string searchstate()
        {

            string searchstring = "";
            return searchstring;
        }

        private void DataGrid_User_Loaded(object sender, RoutedEventArgs e)
        {
            ShowUserInfo(searchstate);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            #region Data loading on my tables
            ///
            /// Load Data for loading into Clickedbase 
            ///
            Load1.Text = Convert.ToString(Rate1.Content);
            Load2.Text = Convert.ToString(Rate2.Content);
            Load3.Text = Convert.ToString(Rate3.Content);
            Load4.Text = Convert.ToString(Rate4.Content);
            #endregion



            #region Loading Images into Rectange

            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            con1.Open();

            ShowUserInfo(searchstate);
            string Finder = Rate1.Content + " , " + Rate2.Content + " , " + Rate3.Content + " , " + Rate4.Content;

            SqlCommand Commandcmd = new SqlCommand(
                "SELECT (MorabiImage) FROM MorabiTable where MorabiID = " + Load1.Text, con1);
            SqlDataReader rdr1 = null;
            rdr1 = Commandcmd.ExecuteReader();
            while (rdr1.Read())
            {
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
                        Image1.Fill = new ImageBrush
                        {
                            ImageSource = imageSource
                        };
                    }

                }
                SqlCommand Rect2 = new SqlCommand(
               "SELECT (MorabiImage) FROM MorabiTable where MorabiID = " + Load2.Text, con1);
                SqlDataReader rdr11 = null;
                rdr11 = Rect2.ExecuteReader();
                while (rdr11.Read())
                {
                    if (rdr11 != null)
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
                            Image2.Fill = new ImageBrush
                            {
                                ImageSource = imageSource
                            };
                        }

                    }
                    SqlCommand Rect3 = new SqlCommand(
               "SELECT (MorabiImage) FROM MorabiTable where MorabiID = " + Load3.Text, con1);
                    SqlDataReader rdr3 = null;
                    rdr3 = Rect3.ExecuteReader();
                    while (rdr3.Read())
                    {
                        if (rdr1 != null)
                        {
                            byte[] data = (byte[])rdr3[0]; // 0 is okay if you only selecting one column
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
                                Image3.Fill = new ImageBrush
                                {
                                    ImageSource = imageSource
                                };
                            }

                        }
                        SqlCommand Rect4 = new SqlCommand(
               "SELECT (MorabiImage) FROM MorabiTable where MorabiID = " + Load4.Text, con1);
                        SqlDataReader rdr4 = null;
                        rdr4 = Rect4.ExecuteReader();
                        while (rdr4.Read())
                        {
                            if (rdr4 != null)
                            {
                                byte[] data = (byte[])rdr4[0]; // 0 is okay if you only selecting one column
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
                                    Image4.Fill = new ImageBrush
                                    {
                                        ImageSource = imageSource
                                    };
                                }

                            }
                        }
                    }
                }
            }
            if (DataGrid_User.SelectedItem != null)
            {
                var itemOne = DataGrid_User.SelectedItem;

            }
            #endregion



        }

        private void DataGrid_User_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            TextBlock MorabiName1 = DataGrid_User.Columns[1].GetCellContent(DataGrid_User.Items[0]) as TextBlock;
            TextBlock Family1 = DataGrid_User.Columns[2].GetCellContent(DataGrid_User.Items[0]) as TextBlock;

            TextBlock MorabiName2 = DataGrid_User.Columns[1].GetCellContent(DataGrid_User.Items[1]) as TextBlock;
            TextBlock Family2 = DataGrid_User.Columns[2].GetCellContent(DataGrid_User.Items[1]) as TextBlock;

            //     TextBlock MorabiName3 = DataGrid_User.Columns[1].GetCellContent(DataGrid_User.Items[0]) as TextBlock;
            //     TextBlock Family3 = DataGrid_User.Columns[2].GetCellContent(DataGrid_User.Items[0]) as TextBlock;

        }

        private void DataGrid_User_Loaded_1(object sender, RoutedEventArgs e)
        {

        }


        private void DataGrid_User_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            #region Load Morabi Information


            #region Table 1
            ///
            /// Find fist Morabi from table [1]
            ///



            MorabiNameRate_1.Content = DataGrid_User.Columns[1].GetCellContent(DataGrid_User.Items[0]);
            GymName1.Content = DataGrid_User.Columns[3].GetCellContent(DataGrid_User.Items[0]);
            Rate1.Content = DataGrid_User.Columns[0].GetCellContent(DataGrid_User.Items[0]);

            #endregion


            #region Table 2
            ///
            /// Find Secend morabi From table [2]
            ///

            MorabiNameRate_2.Content = DataGrid_User.Columns[1].GetCellContent(DataGrid_User.Items[1]);
            GymName2.Content = DataGrid_User.Columns[3].GetCellContent(DataGrid_User.Items[1]);
            Rate2.Content = DataGrid_User.Columns[0].GetCellContent(DataGrid_User.Items[1]);
            #endregion


            #region Table 3

            ///
            /// Find Thired morabi From table[3]
            ///

            MorabiNameRate_3.Content = DataGrid_User.Columns[1].GetCellContent(DataGrid_User.Items[2]);
            GymName3.Content = DataGrid_User.Columns[3].GetCellContent(DataGrid_User.Items[2]);
            Rate3.Content = DataGrid_User.Columns[0].GetCellContent(DataGrid_User.Items[2]);

            #endregion


            #region Table 4
            ///
            /// Find Last morabi from table[4]
            ///

            MorabiNameRate_4.Content = DataGrid_User.Columns[1].GetCellContent(DataGrid_User.Items[3]);
            GymName4.Content = DataGrid_User.Columns[3].GetCellContent(DataGrid_User.Items[3]);
            Rate4.Content = DataGrid_User.Columns[0].GetCellContent(DataGrid_User.Items[3]);
            #endregion 

            #endregion

        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            #region Rectangle 1 




            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            con1.Open();




            #region Find Morabi Name And Morabi Family

            SqlCommand myCommand = new SqlCommand("Select (MorabiName + ' ' + MorabiFamily) from MorabiTable where MorabiID = " + Load1.Text, con1);
            object result = myCommand.ExecuteScalar();
            MorabiNameTXT.Content = Convert.ToString(result).Trim();

            #endregion
            #region Find Morabi Colloctions 
            SqlCommand MorabiFindCommand = new SqlCommand("Select Sum(MorabiFind) from Users where MorabiFind = " + Load1.Text, con1);
            object MorabiFindresult = MorabiFindCommand.ExecuteScalar();
            if (Convert.ToString(MorabiFindresult) != "")
            {
                ZirMajmoeTXT.Content = Convert.ToString(MorabiFindresult).Trim();
            }
            else
            {
                ZirMajmoeTXT.Content = "0";
            }
            MorabiIDFound.Content = Load1.Text;
            #endregion
            #region Load Image
            SqlCommand CommandImage = new SqlCommand(
               "SELECT (MorabiImage) FROM MorabiTable where MorabiID = " + Load1.Text, con1);
            SqlDataReader rdr1 = null;
            rdr1 = CommandImage.ExecuteReader();
            while (rdr1.Read())
            {


                if (rdr1 != null)
                {
                    byte[] data = (byte[])rdr1[0];

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
            #endregion





            #endregion
            con1.Close();

        }


        private void Image2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            #region Rectangle 2 




            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            con1.Open();




            #region Find Morabi Name And Morabi Family

            SqlCommand myCommand = new SqlCommand("Select (MorabiName + ' ' + MorabiFamily) from MorabiTable where MorabiID = " + Load2.Text, con1);
            object result = myCommand.ExecuteScalar();
            MorabiNameTXT.Content = Convert.ToString(result).Trim();

            #endregion
            #region Find Morabi Colloctions 
            SqlCommand MorabiFindCommand = new SqlCommand("Select Sum(MorabiFind) from Users where MorabiFind = " + Load2.Text, con1);
            object MorabiFindresult = MorabiFindCommand.ExecuteScalar();
            if (Convert.ToString(MorabiFindresult) != "")
            {
                ZirMajmoeTXT.Content = Convert.ToString(MorabiFindresult).Trim();
            }
            else
            {
                ZirMajmoeTXT.Content = "0";
            }
            MorabiIDFound.Content = Load2.Text;
            #endregion
            #region Load Image
            SqlCommand CommandImage = new SqlCommand(
               "SELECT (MorabiImage) FROM MorabiTable where MorabiID = " + Load2.Text, con1);
            SqlDataReader rdr1 = null;
            rdr1 = CommandImage.ExecuteReader();
            while (rdr1.Read())
            {


                if (rdr1 != null)
                {
                    byte[] data = (byte[])rdr1[0];

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
            #endregion





            #endregion
            con1.Close();
        }

        private void Image3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            #region Rectangle 3 




            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            con1.Open();




            #region Find Morabi Name And Morabi Family

            SqlCommand myCommand = new SqlCommand("Select (MorabiName + ' ' + MorabiFamily) from MorabiTable where MorabiID = " + Load3.Text, con1);
            object result = myCommand.ExecuteScalar();
            MorabiNameTXT.Content = Convert.ToString(result).Trim();

            #endregion
            #region Find Morabi Colloctions 
            SqlCommand MorabiFindCommand = new SqlCommand("Select Sum(MorabiFind) from Users where MorabiFind = " + Load3.Text, con1);
            object MorabiFindresult = MorabiFindCommand.ExecuteScalar();
            if (Convert.ToString(MorabiFindresult) != "")
            {
                ZirMajmoeTXT.Content = Convert.ToString(MorabiFindresult).Trim();
            }
            else
            {
                ZirMajmoeTXT.Content = "0";
            }
            MorabiIDFound.Content = Load3.Text;
            #endregion
            #region Load Image
            SqlCommand CommandImage = new SqlCommand(
               "SELECT (MorabiImage) FROM MorabiTable where MorabiID = " + Load3.Text, con1);
            SqlDataReader rdr1 = null;
            rdr1 = CommandImage.ExecuteReader();
            while (rdr1.Read())
            {


                if (rdr1 != null)
                {
                    byte[] data = (byte[])rdr1[0];

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
            #endregion





            #endregion
            con1.Close();
        }

        private void Image4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            #region Rectangle 4 




            SqlConnection con1 = new SqlConnection(PublicVar.ConnectionString);
            con1.Open();




            #region Find Morabi Name And Morabi Family

            SqlCommand myCommand = new SqlCommand("Select (MorabiName + ' ' + MorabiFamily) from MorabiTable where MorabiID = " + Load4.Text, con1);
            object result = myCommand.ExecuteScalar();
            MorabiNameTXT.Content = Convert.ToString(result).Trim();

            #endregion
            #region Find Morabi Colloctions 
            SqlCommand MorabiFindCommand = new SqlCommand("Select Count(MorabiFind) from Users where MorabiFind = " + Load4.Text, con1);
            object MorabiFindresult = MorabiFindCommand.ExecuteScalar();
            if (Convert.ToString(MorabiFindresult) != "")
            {
                ZirMajmoeTXT.Content = Convert.ToString(MorabiFindresult).Trim();
            }
            else
            {
                ZirMajmoeTXT.Content = "0";
            }
            MorabiIDFound.Content = Load4.Text;
            #endregion
            #region Load Image
            SqlCommand CommandImage = new SqlCommand(
               "SELECT (MorabiImage) FROM MorabiTable where MorabiID = " + Load4.Text, con1);
            SqlDataReader rdr1 = null;
            rdr1 = CommandImage.ExecuteReader();
            while (rdr1.Read())
            {


                if (rdr1 != null)
                {
                    byte[] data = (byte[])rdr1[0];

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
            #endregion





            #endregion
            con1.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            LoginPage w = new LoginPage();
            w.Show();
        }

    }
}
