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

namespace OTBaseNew.Actions
{
    /// <summary>
    /// Interaction logic for AddActionWindow.xaml
    /// </summary>
    public partial class AddActionWindow : Window
    {
        Requests.Request req;
        public AddActionWindow(Requests.Request req)
        {
            InitializeComponent();
            this.req = req;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(text.Text!="")
            {
                Action ac = new Action();
                ac.Request_id = req.Id;
                ac.User_id = MainWindow.Logined.Id;
                ac.Note = text.Text;
                ac.Save();
                this.Close();
            }
            else
            {
                this.Close();
            }
        }
    }
}
