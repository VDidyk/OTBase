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
    /// Interaction logic for ChangeHotelWindow.xaml
    /// </summary>
    public partial class ChangeHotelWindow : Window
    {
        Request req;
        public ChangeHotelWindow(Request req)
        {
            InitializeComponent();
            this.req = req;
            text.Text = req.Hotel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (text.Text != "")
            {
                req.Hotel = text.Text;
                req.Save();
                MainWindow.Message("Готель номер змінено!");
            }
            this.Close();
        }
    }

}
