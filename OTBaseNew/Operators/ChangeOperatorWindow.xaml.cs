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

namespace OTBaseNew.Operators
{
    /// <summary>
    /// Interaction logic for ChangeOperatorWindow.xaml
    /// </summary>
    public partial class ChangeOperatorWindow : Window
    {
        Requests.Request req;
        public ChangeOperatorWindow(Requests.Request req)
        {
            InitializeComponent();
            this.req = req;
            var tmp = Operator.GetAllOperators;
            for (int i = 0; i < tmp.Count;i++ )
            {
                text.Items.Add(tmp[i].Name);
                if (req.GetOperator.Id == tmp[i].Id)
                {
                    text.SelectedIndex = i;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (text.SelectedIndex != -1)
            {
                Operator st = Operator.FindByName(text.SelectedItem.ToString());
                req.GetOperator = st;
                req.Save();
                MainWindow.Message("Оператор змінений!");
                MainWindow.AddNewAction(req, "Готель змінено на " + req.Hotel);
            }
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
