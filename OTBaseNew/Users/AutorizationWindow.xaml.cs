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
    /// Interaction logic for AutorizationWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : Window
    {
        public AutorizationWindow()
        {
            InitializeComponent();
        }

        private void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            Login.Background = Brushes.White;
            if (Login.Text != "")
            {
                char sumbol = Login.Text[Login.Text.Length - 1];
                if (sumbol == ' ')
                {
                    Login.Text = Login.Text = Login.Text.Substring(0, Login.Text.Length - 1);
                    Login.SelectionStart = Login.Text.Length;
                }
            }
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            ResultLable.Content = "";
            if(Login.Text=="")
            {
                Login.Background = Brushes.Pink;
                return;
            }
            if (Password.Password == "")
            {
                Password.Background = Brushes.Pink;
                return;
            }
            Users.User user=Users.User.FindByLogin(Login.Text);
            if(user==null)
            {
                ResultLable.Content = "Не вірний логін";
            }
            else
            {
                if(Password.Password!=user.Password)
                {
                    ResultLable.Content = "Не вірний пароль";
                }
                else
                {
                    MainWindow.Logined = user;
                    this.Close();
                }
            }
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password.Background = Brushes.White;
        }
    }

}
