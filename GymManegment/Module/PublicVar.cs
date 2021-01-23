using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManegment.Module
{
    public static class PublicVar
    {
       
        public static string PasswordLogin = "1";
        public static string GUserName = "";
        public static string GUserFamily = "";
        public static int GUserID;
        public static string TodayTime = "";
    //    public static string ConnectionStrings = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\gym.mdf;  initial catalog = gym; user id = sa ;  password=re110121; Integrated Security=True;Connect Timeout=30";
        public static string ConnectionString = "Data Source=tcp:46.105.154.162;initial catalog = gym ;User ID=sa;Password=re110121!";
     //   public static string ConnectionString = "data source =.; initial catalog = gym; user id = sa; password=re110121;MultipleActiveResultSets=True;App=EntityFramework&quot;";
    }
}
