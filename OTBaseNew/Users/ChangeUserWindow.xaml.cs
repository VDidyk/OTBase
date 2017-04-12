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

namespace OTBaseNew.Users
{
    /// <summary>
    /// Interaction logic for ChangeUserWindow.xaml
    /// </summary>
    public partial class ChangeUserWindow : Window
    {
        Requests.Request req;
        public ChangeUserWindow(Requests.Request req)
        {
            InitializeComponent();
            this.req = req;
            var tmp = User.GetAllUsers;
            for (int i = 0; i < tmp.Count;i++ )
            {
                text.Items.Add(tmp[i].FName+" "+tmp[i].LName);
                if (req.working_user_id == tmp[i].Id)
                {
                    text.SelectedIndex = i;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            if (text.SelectedIndex != -1)
            {
                User st = User.GetAllUsers[text.SelectedIndex];
                req.working_user_id = st.Id;
                req.Save();
                MainWindow.Message("Менеджер змінений!");
            }
            MainWindow.AddNewAction(req, "Менеджер змінений на " + req.GetWorking_user.FName + " " + req.GetWorking_user.LName);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
