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
    /// Interaction logic for ChangeSerialNumberWindow.xaml
    /// </summary>
    public partial class ChangeSerialNumberWindow : Window
    {
        Request req;
        public ChangeSerialNumberWindow(Request req)
        {
            InitializeComponent();
            this.req = req;
            text.Text = req.Serial_number;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            req.Serial_number = text.Text;
            req.Save();
            MainWindow.Message("Серійний номер змінено!");
            MainWindow.AddNewAction(req, "Серійний номер змінено на " + req.Serial_number);
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
