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

namespace OTBaseNew.Statuses
{
    /// <summary>
    /// Interaction logic for StatusChangeWindow.xaml
    /// </summary>
    public partial class StatusChangeWindow : Window
    {
        Requests.Request req;
        public StatusChangeWindow(Requests.Request req)
        {
            InitializeComponent();
            this.req = req;
            var tmp = Status.GetAllStatuses();
            for (int i = 0; i < tmp.Count;i++ )
            {
                text.Items.Add(tmp[i].Name);
                if (req.GetStatus.Id == tmp[i].Id)
                {
                    text.SelectedIndex = i;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Status st = Status.FindByName(text.SelectedItem.ToString());
            req.GetStatus = st;
            req.Save();
            MainWindow.Message("Статус змінено!");
            MainWindow.AddNewAction(req, "Статус змінено на " + req.GetStatus.Name);
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
