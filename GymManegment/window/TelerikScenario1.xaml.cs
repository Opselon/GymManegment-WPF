using DataModelLayer;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace GymManegment.window
{
    /// <summary>
    /// Interaction logic for TelerikScenario1.xaml
    /// </summary>
    public partial class TelerikScenario1
    {

        public TelerikScenario1()
        {

            InitializeComponent();

        }
        gymEntities2 database = new gymEntities2();
        private string finder;
        public void ShowUserInfo(Func<string> SearchStringForUser)
        {

            var query = database.Database.SqlQuery<ListMove>("select * From ListMove where 1 = 1 " + SearchStringForUser());
          //    MessageBox.Show(query.ToString());
            var u = query.ToList();
            try
            {


                DataGrid_User.ItemsSource = u;
            }
            catch
            {

            }

        }
        public string searchstate()
        {

            
            string searchstring = "";
            try
            {
                if (SerchText.Text != "" && SerchText.Text != "متن خود را برای جستوجو در لیست بنویسید")
                {
                    searchstring += " And ProgramName like '%" + SerchText.Text + "%'";
                }
                if (finder != "همه")
                    {
                    searchstring += " And ProgramName like '%" + finder.Trim() + "%'";
                     
                }
               
            }
            catch
            {
               
            }

         

            return searchstring;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DataGrid_User_Loaded(object sender, RoutedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }
        int i = 0 , t = 0;
        private void InsertMethod(object sender, RoutedEventArgs e)
        {

            object itemOne = DataGrid_User.SelectedItem;

            t++;
        
         
            string submiter = (DataGrid_User.SelectedCells[1].Column.GetCellContent(itemOne) as TextBlock).Text + "\n";


            TextRange rangeOfText1 = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);


            TextRange rangeOfText2 = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);

            rangeOfText1.Text +=  submiter;
        
            rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

            if (t == 1)
            {
                rangeOfText2.Text += "\n" + "تعداد : " + X1.Text + X3.Content + X2.Text ;
                rangeOfText2.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
                rangeOfText2.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                rangeOfText2.Text += "\n" + "تعداد : " + X1.Text + X3.Content + X2.Text;
                rangeOfText2.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
                rangeOfText2.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
  

            HarekatList.Text += submiter;
            HarekatList.Text += "تعداد : " + X1.Text + X3.Content + X2.Text + "\n";

        }
        private void DataGrid_User_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_User_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object itemOne = DataGrid_User.SelectedItem;
            string name = (DataGrid_User.SelectedCells[1].Column.GetCellContent(itemOne) as TextBlock).Text;
            HarekatName.Text = name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TelerikScenario2 w_n = new TelerikScenario2();
            w_n.ShowDialog();
        }

        private void X1X_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int x1 = Convert.ToInt16(X1.Text);
            x1++;
            X1.Text = x1.ToString() ;
        }

        private void X1XX_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int x1 = Convert.ToInt16(X1.Text);
            x1--;
            X1.Text = x1.ToString();
        }

        private void X1X_Copy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int x1 = Convert.ToInt16(X2.Text);
            x1++;
            X2.Text = x1.ToString();
        }

        private void X1XX_Copy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int x1 = Convert.ToInt16(X2.Text);
            x1--;
            X2.Text = x1.ToString();
        }
  
    
        int it = 0;
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
         
            string[] week = new string[6] { "جلسه اول", "جلسه دوم", "جلسه سوم", "جلسه چهار", "جلسه پنجم", "جلسه شیشم" };
            int nomore = week.Length;
            it++;


            TextRange rangeOfText1 = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
            var size = (double)rangeOfText1.GetPropertyValue(FontSizeProperty);


            switch (it)
            {
                case 0:
                    rangeOfText1.Text += week[it] + "\n";
     
                    rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
                    rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                    rangeOfText1.ApplyPropertyValue(TextElement.FontSizeProperty, size + 10);
                    HarekatList.Text += week[it] + "\n";
                break;
                case 1:
                 
                    rangeOfText1.Text += week[it] + "\n";
                    rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
                    rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                    rangeOfText1.Text += "--------------------" + "\n";
                    HarekatList.Text += week[it] + "\n";
                    break;
                case 2:
                    rangeOfText1.Text += week[it] + "\n";
                    rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
                    rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                    rangeOfText1.Text += "--------------------" + "\n";
                    HarekatList.Text += week[it] + "\n";

                    break;
                case 3:
               
                    rangeOfText1.Text += week[it] + "\n";
                    rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
                    rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                

                    rangeOfText1.Text += "--------------------" + "\n";
                    HarekatList.Text += week[it] + "\n";
                    break;
                case 4:
                  
                    rangeOfText1.Text += week[it] + "\n";
                    rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
                    rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
              

                    rangeOfText1.Text += "--------------------" + "\n";

                    HarekatList.Text += week[it] + "\n";
                    break;
                case 5:
                 
                    rangeOfText1.Text += week[it] + "\n";
                    rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
                    rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                  
                    rangeOfText1.Text += "--------------------" + "\n";
                    HarekatList.Text += week[it] + "\n";
                    break;
                case 6:
              
                    rangeOfText1.Text += week[it];
                    rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
                    rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                    rangeOfText1.Text += "--------------------" + "\n";
                    HarekatList.Text += week[it] + "\n";
                    break;
            }


        }

        private void richTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextRange rangeOfText1 = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
            rangeOfText1.Text += "جلسه اول" + "\n ";
            rangeOfText1.Text += "--------------------" + "\n";
            rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
            rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        
          
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog printDialog1 = new System.Windows.Controls.PrintDialog();
            System.Windows.Forms.PrintDialog printDocument1 = new System.Windows.Forms.PrintDialog();

            //if (printDialog1.ShowDialog() == false)
            //{
               
            //  printDialog1.ShowDialog();
            //}
            PrintCommand();
        }

        private void PrintCommand()
        {
            PrintDialog pd = new PrintDialog();
            if ((pd.ShowDialog() == true))
            {
                //use either one of the below      
              //  pd.PrintVisual(richTextBox as Visual, "printing as visual");
                pd.PrintDocument((((IDocumentPaginatorSource)richTextBox.Document).DocumentPaginator), "printing as paginator");
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PrintCommand();
   
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            colorDialog.ShowDialog();
        

        }

        private void Button_Click_3(object sender, MouseButtonEventArgs e)
        {

        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
          
        }

        private void Image_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {

        }

        private void HarekatName_Copy1_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowUserInfo(searchstate);
        }


        private void SerchText_MouseDown(object sender, MouseButtonEventArgs e)
        {
         
        }

        private void SerchText_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SerchText.Text = "";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = Convert.ToString(ComboSelector.SelectedValue);
             finder = name.Replace("System.Windows.Controls.ListBoxItem: ","");
            ShowUserInfo(searchstate);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string filePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + @"\Programs\Document.rtf";
            string newpatch = @filePath.Replace("\\GymManegment.exe", "");
            Process wordProcess = new Process();
            wordProcess.StartInfo.FileName = @newpatch;
            wordProcess.StartInfo.UseShellExecute = true;
            wordProcess.Start();

      
        }

        private void RadWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
