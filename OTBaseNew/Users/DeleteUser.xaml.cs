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
    /// Interaction logic for ChangeUserPassword.xaml
    /// </summary>
    public partial class DeleteUser : Window
    {
        User u;
        public DeleteUser(User u)
        {
            InitializeComponent();
            this.u = u;            
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {

              if(u.Id!=MainWindow.Logined.Id)
              {
                  u.Delete();
                  this.Close();
              }
              else
              {
                  MainWindow.Message("Ви не можете видалити самого себе!");
                  this.Close();
              }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
