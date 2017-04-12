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
    /// Interaction logic for ChangeVisaWindow.xaml
    /// </summary>
    public partial class ChangeVisaWindow : Window
    {
        Request req;
        public ChangeVisaWindow(Request req)
        {
            InitializeComponent();
            this.req = req;

            if(req.Visa_is_important)
            {
                important.IsChecked = true;
                if(req.Get_visa==true)
                {
                    Get.IsChecked = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (important.IsChecked == true)
            {
                req.Visa_is_important = true;
                if (Get.IsChecked == true)
                {
                    req.Get_visa = true;
                }
                else
                {
                    req.Get_visa = false;
                }
            }
            else
            {
                req.Visa_is_important = false;
                req.Get_visa = false;
            }
            req.Save();
            MainWindow.Message("Інформація про візу змінена!");
            MainWindow.AddNewAction(req, "Інформація про візу змінена!");
            DialogResult = true;
            this.Close();
        }

        private void important_Checked(object sender, RoutedEventArgs e)
        {
           
        }

        private void important_Click(object sender, RoutedEventArgs e)
        {
            if (important.IsChecked == true)
            {
                Get.IsChecked = false;
                Get.IsEnabled = true;
            }
            else
            {
                Get.IsChecked = false;
                Get.IsEnabled = false;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
