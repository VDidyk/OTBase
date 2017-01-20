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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OTBaseNew
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Users.User u = new Users.User();
            u.Id = 2;
            u.FName = "Віталій";
            u.LName = "Дідик";
            u.MName = "Олегович";
            u.Login = "tailer";
            u.Password = "1234";
            u.IsAdmin = true;
            u.Bdate = Convert.ToDateTime("16.01.1996");
            u.Save();
        }
    }
}
