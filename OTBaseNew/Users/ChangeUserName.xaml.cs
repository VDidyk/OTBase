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
    public partial class ChangeUserName : Window
    {
        User u;
        public ChangeUserName(User u)
        {
            InitializeComponent();
            this.u = u;
            fname.Text = u.FName;
            lname.Text = u.LName;
            mname.Text = u.MName;
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if(lname.Text!="")
            {
                u.FName = fname.Text;
                u.LName = lname.Text;
                u.MName = mname.Text;
                u.Save();
                this.Close();
            }
            else
            {
                MainWindow.Message("Прізвище не може бути пустим!");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
