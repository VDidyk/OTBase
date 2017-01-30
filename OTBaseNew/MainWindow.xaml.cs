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
                LoadGridAddClient();
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

        #region Сетка добавить клиента
        void LoadGridAddClient()
        {
            ManagersWrapPenelInAddClients.Children.Clear();
            foreach (var i in Users.User.GetAllUsers)
            {
                AddManagerToWrapPanelInAddClients(i);
            }
            foreach (var i in Regions.Region.GetAllRegions)
            {
                ChooseRegionComboboxAddClient.Items.Add(i.Name);
            }
        }
        Border selected_manager_for_client_in_add_client;
        private void AddPhoneFieldAddClients_Click(object sender, RoutedEventArgs e)
        {
            TextBox t = new TextBox();
            t.Style = Resources["TexBoxStyle"] as Style;
            t.Margin = new Thickness(0, 10, 0, 10);
            Button b = new Button();
            b.Content = "+";
            b.Style = Resources["ButtonStyle"] as Style;
            b.Margin = new Thickness(5, 10, 5, 10);
            b.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            b.FontSize = 30;
            b.Click += AddPhoneFieldAddClients_Click;
            AddClientTelephonsTextboxStack.Children.Add(t);
            AddClientTelephonsBtnStack.Children.Add(b);
            ((Button)sender).Visibility = System.Windows.Visibility.Hidden;
            AddPhonesAddClientScroll.ScrollToBottom();
        }
        private void AddEmailFieldAddClients_Click(object sender, RoutedEventArgs e)
        {
            TextBox t = new TextBox();
            t.Style = Resources["TexBoxStyle"] as Style;
            t.Margin = new Thickness(0, 10, 0, 10);
            Button b = new Button();
            b.Content = "+";
            b.Style = Resources["ButtonStyle"] as Style;
            b.Margin = new Thickness(5, 10, 5, 10);
            b.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            b.FontSize = 30;
            b.Click += AddEmailFieldAddClients_Click;
            AddClientEmailTextboxStack.Children.Add(t);
            AddClientEmailBtnStack.Children.Add(b);
            ((Button)sender).Visibility = System.Windows.Visibility.Hidden;
            AddPhonesAddClientScroll.ScrollToBottom();
        }
        private void AddNewRegionBtnAddClient_Click(object sender, RoutedEventArgs e)
        {
            if (AddNewRegionTextBoxAddClient.Visibility == System.Windows.Visibility.Hidden)
            {
                AddNewRegionTextBoxAddClient.Visibility = System.Windows.Visibility.Visible;
                AddNewRegionBtnAddClient.Visibility = System.Windows.Visibility.Hidden;
                AddNewRegionTextBoxAddClient.Focus();
                ChooseRegionComboboxAddClient.IsEnabled = false;
            }
        }
        private void AddNewRegionTextBoxAddClient_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
            {
                AddNewRegionTextBoxAddClient.Visibility = System.Windows.Visibility.Hidden;
                AddNewRegionBtnAddClient.Visibility = System.Windows.Visibility.Visible;
                ChooseRegionComboboxAddClient.IsEnabled = true;
                ChooseRegionComboboxAddClient.SelectedIndex = -1;
            }
        }
        private void AddNewCityBtnAddClient_Click(object sender, RoutedEventArgs e)
        {
            if (ChooseRegionComboboxAddClient.SelectedIndex == -1 && AddNewRegionTextBoxAddClient.Text == "")
            {
                MainWindow.Message("Оберіть область!");
            }
            else
            {
                if (AddNewCityTextBoxAddClient.Visibility == System.Windows.Visibility.Hidden)
                {
                    AddNewCityTextBoxAddClient.Visibility = System.Windows.Visibility.Visible;
                    AddNewCityBtnAddClient.Visibility = System.Windows.Visibility.Hidden;
                    AddNewCityTextBoxAddClient.Focus();
                    ChooseCityComboboxAddClient.IsEnabled = false;
                }
            }
        }

        private void AddNewCityTextBoxAddClient_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
            {
                AddNewCityTextBoxAddClient.Visibility = System.Windows.Visibility.Hidden;
                AddNewCityBtnAddClient.Visibility = System.Windows.Visibility.Visible;
                ChooseCityComboboxAddClient.IsEnabled = true;
            }
        }
        private void ManagerBorderInAddClient_Mouse_Down(object sender, MouseButtonEventArgs e)
        {
            if (selected_manager_for_client_in_add_client != null)
            {
                selected_manager_for_client_in_add_client.Style = Resources["ClientBorderStyle"] as Style;
            }
            selected_manager_for_client_in_add_client = (Border)sender;
            selected_manager_for_client_in_add_client.Style = Resources["ClientSelectedBorderStyle"] as Style;
        }
        void AddManagerToWrapPanelInAddClients(Users.User user)
        {

            Border b = new Border();
            b.MouseLeftButtonDown += ManagerBorderInAddClient_Mouse_Down;
            b.Width = 250;
            b.Height = 250;
            b.Style = Resources["ClientBorderStyle"] as Style;
            StackPanel st = new StackPanel();
            st.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            st.Margin = new Thickness(0, -25, 0, 0);
            b.Child = st;
            Label id = new Label();
            id.Content = user.Id;
            id.Visibility = System.Windows.Visibility.Hidden;
            TextBlock name = new TextBlock();
            name.Style = Resources["TextBlockStyle"] as Style;
            name.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            name.Text = user.LName + " " + user.FName;
            name.TextWrapping = TextWrapping.Wrap;
            st.Children.Add(id);
            st.Children.Add(name);
            Image im = new Image();
            im.Height = 150;
            im.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\client.png"));
            st.Children.Add(im);
            ManagersWrapPenelInAddClients.Children.Add(b);
        }
        private void ChooseRegionComboboxAddClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                AddNewCityTextBoxAddClient.Visibility = System.Windows.Visibility.Hidden;
                AddNewCityTextBoxAddClient.Text = "";
                AddNewCityBtnAddClient.Visibility = System.Windows.Visibility.Visible;
                ChooseCityComboboxAddClient.IsEnabled = true;
            foreach (var i in Regions.Region.FindByName(ChooseRegionComboboxAddClient.Items[ChooseRegionComboboxAddClient.SelectedIndex].ToString()).GetCities)
            {
                ChooseCityComboboxAddClient.Items.Add(i.Name);
            }
        }
        #endregion













    }
}
