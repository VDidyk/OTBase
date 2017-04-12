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
    /// Interaction logic for ChangeFinancesWindow.xaml
    /// </summary>
    public partial class ChangeFinancesWindow : Window
    {
        Request req;
        public ChangeFinancesWindow(Request req)
        {
            InitializeComponent();
            this.req = req;
            sum.Text = req.Price_of_tour.ToString();
            sum1.Text = req.Price_of_client.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sum.Text == "")
            {
                req.Price_of_tour = 0;
            }
            else
            {
                try
                {
                    req.Price_of_tour = Convert.ToDouble(sum.Text);
                }
                catch
                {
                    MainWindow.Message("Не вірний формат грошей (гг.кк)");
                }
            }
            if (sum1.Text == "")
            {
                req.Price_of_client = 0;
            }
            else
            {
                try
                {
                    req.Price_of_client = Convert.ToDouble(sum1.Text);
                }
                catch
                {
                    MainWindow.Message("Не вірний формат грошей (гг.кк)");
                }
            }
            req.Save();
            DialogResult = true;
            MainWindow.AddNewAction(req, "Інформація про фінанси змінена!");
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
