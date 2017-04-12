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

namespace OTBaseNew.Requests
{
    /// <summary>
    /// Interaction logic for DurationChangeWindow.xaml
    /// </summary>
    public partial class DurationChangeWindow : Window
    {
        Requests.Request req;
        public DurationChangeWindow(Requests.Request req)
        {
            InitializeComponent();
            this.req = req;
            text.Text = req.From_where_to_fly;
            text1.Text = req.Where_to_fly;
            if (req.Date_to_go.Year != 1)
            {
                whenetogo.Text = req.Date_to_go.ToShortDateString();
            }
            if (req.Date_to_arrive.Year != 1)
            {
                whenetoback.Text = req.Date_to_arrive.ToShortDateString();
            }
            img.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Other\arrows.png"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            req.From_where_to_fly = text.Text;
            req.Where_to_fly = text1.Text;
            if (whenetogo.Text != "")
            {
                DateTime? tmp = Other.Utility.ConvertStringToDateTime(whenetogo.Text);
                if (tmp == null)
                {
                    MainWindow.Message("Не вірний формат вильоту. (дд.мм.рррр)");
                    return;
                }
                else
                {
                    req.Date_to_go = Convert.ToDateTime(tmp.Value.ToShortDateString());
                }
            }
            else
            {
                req.Date_to_go = Convert.ToDateTime("01.01.01");
            }
            if (whenetoback.Text != "")
            {
                DateTime? tmp1 = Other.Utility.ConvertStringToDateTime(whenetoback.Text);
                if (tmp1 == null)
                {
                    MainWindow.Message("Не вірний формат прильоту. (дд.мм.рррр)");
                    return;
                }
                else
                {
                    req.Date_to_arrive = Convert.ToDateTime(tmp1.Value.ToShortDateString());
                }
            }
            else
            {
                req.Date_to_arrive = Convert.ToDateTime("01.01.01");
            }

            req.Save();
            MainWindow.Message("Напрямок змінено!");
            MainWindow.AddNewAction(req, "Напрямок змінено!");
            DialogResult = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
