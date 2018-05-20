using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WpfTest.classes;
using System.Diagnostics;




namespace WpfTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        
        
        private void Get_data_Click(object sender, RoutedEventArgs e)
        {

             

            string temp= temp_item.Text;
            Excel xl = new Excel();
            if (System.IO.File.Exists("TradesTable .xlsx"))
                xl.FileOpen("TradesTable .xlsx");
            else
            {
                xl.AddRow("id", "price", "quantitu", "side", "timestamp");
                xl.FileSave("TradesTable .xlsx");
            }
            

            string url = "https://api.hitbtc.com/api/2/public/trades/" + temp + "?sort=DESC&limit=1000";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                TradesTable[] TradesArray = JsonConvert.DeserializeObject<TradesTable[]>(responseFromServer);
                foreach (TradesTable t in TradesArray)
                {
                    xl.AddRow(t.id, t.price, t.price, t.side, t.timestamp.ToString());

                }
                xl.FileSave("TradesTable .xlsx");

                reader.Close();


                MessageBox.Show("success!");
                response.Close();
            }
            catch (WebException у )
            {
                MessageBox.Show("Error argument!"+у);
                
            }
            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists("TradesTable .xlsx"))
                Process.Start(@"TradesTable .xlsx");
            else
            {
                Excel xl = new Excel();
                xl.AddRow("id", "price", "quantitu", "side", "timestamp");
                xl.FileSave("TradesTable .xlsx");
                Process.Start(@"TradesTable .xlsx");
            } 
        }
    }
}
