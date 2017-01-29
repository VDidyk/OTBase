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
        public static Users.User Logined;
        public static string Exepath = Environment.CurrentDirectory;
        public MainWindow()
        {
            InitializeComponent();
            Users.AutorizationWindow w = new Users.AutorizationWindow();
            w.ShowDialog();
            if (Logined == null)
            {
                MainWindow.Message("Щасливо");
                this.Close();
            }
            else
            {
                //MainWindow.Message(string.Format("Привіт, {0}", Logined.FName));
                NameLable.Content = Logined.FName;
                LoadImages();
                LoadClientsImages();
            }
        }
        public static void Message(string text)
        {
            Alarm a1 = new Alarm(text, MainWindow.Exepath);
        }

        void LoadImages()
        {
            imageBrush.ImageSource = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Other\User-Icon.png"));
            ClientsMenuImages.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Menu\Clients.png"));
            RequestsMenuImages.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Menu\Requests.png"));
            UsersMenuImages.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Menu\Users.png"));
            OperatorsMenuImages.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Menu\Operators.png"));
            ConfigMenuImages.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Menu\Config.png"));
        }
        void LoadClientsImages()
        {
            
            AddClientBtnClients.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\AddBtn.png"));
            ShowClientBtnClients.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\ShowBtn.png"));
            SearchClientBtnClients.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\SearchBtn.png"));

        }
    }
}
