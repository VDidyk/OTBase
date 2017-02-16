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
        public Clients.Client ClientToShow;
        public static Users.User Logined;
        StackPanel ActualEditClientPanel;
        public List<Grid> grids = new List<Grid>();
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
                grids.Add(ClientsGrid);
                ClientToShow = Clients.Client.FindById(2);
                LoadShowClient(ClientToShow);
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
        void LookClient(object sender, MouseButtonEventArgs e)
        {
            Border b = (Border)sender;
            StackPanel sp = (StackPanel)b.Child;
            Label id = (Label)sp.Children[0];
            ClientToShow = Clients.Client.FindById(Convert.ToInt32(id.Content));
            LoadShowClient(ClientToShow);
        }
        //-----------------        
        #region Сетка Клиенты
        #region Сетка добавить клиента
        void LoadGridAddClient()
        {
            foreach (var i in Users.User.GetAllUsers)
            {
                AddManagerToWrapPanelInAddClients(i);
            }
            foreach (var i in Regions.Region.GetAllRegions)
            {
                ChooseRegionComboboxAddClient.Items.Add(i.Name);
            }
            foreach (var i in Resourses.Resourse.GetAllResourses)
            {
                ChooseResourseComboboxAddClient.Items.Add(i.Name);
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
            if (ChooseRegionComboboxAddClient.SelectedIndex != -1)
            {
                ChooseCityComboboxAddClient.Items.Clear();
                foreach (var i in Regions.Region.FindByName(ChooseRegionComboboxAddClient.Items[ChooseRegionComboboxAddClient.SelectedIndex].ToString()).GetCities)
                {
                    ChooseCityComboboxAddClient.Items.Add(i.Name);
                }
            }
        }
        private void SaveAddClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LnameAddClient.Text == "")
                {
                    MainWindow.Message("Введіть прізвище");
                    return;
                }
                List<Phones.Phone> phones = new List<Phones.Phone>();
                List<Emails.Email> emails = new List<Emails.Email>();
                Clients.Client c = new Clients.Client();
                c.FName = FnameAddClient.Text;
                c.LName = LnameAddClient.Text;
                c.MName = MnameAddClient.Text;
                if (BDayAddClient.Text != "")
                {
                    DateTime? d = Other.Utility.ConvertStringToDateTime(BDayAddClient.Text);
                    if (d == null)
                    {
                        MainWindow.Message("Дата народження заповнена не вірно. Формат: дд.мм.рррр");
                        return;
                    }
                    c.Bday = Convert.ToDateTime(d.Value.ToString());
                }

                //Добавляет телефоны вытаскивая со списка
                foreach (var i in AddClientTelephonsTextboxStack.Children)
                {
                    try
                    {
                        Phones.Phone phone = new Phones.Phone();
                        string tmp = ((TextBox)i).Text;
                        string number = Other.Utility.ConvertStringToPhoneString(tmp);
                        phone.number = number;
                        phones.Add(phone);
                    }
                    catch
                    {

                    }
                }
                //Добавляет телефоны вытаскивая со списка
                foreach (var i in AddClientEmailTextboxStack.Children)
                {
                    try
                    {
                        Emails.Email mail = new Emails.Email();
                        string tmp = ((TextBox)i).Text;
                        mail.name = tmp;
                        emails.Add(mail);
                    }
                    catch
                    {

                    }
                }
                #region Создает адресс
                Addresses.Address a = new Addresses.Address();
                if (ChooseCityComboboxAddClient.SelectedIndex != -1)
                {
                    a.city_id = Cities.City.FindByName(ChooseCityComboboxAddClient.SelectedItem.ToString()).Id;
                }
                else
                {
                    if (AddNewCityTextBoxAddClient.Text != "")
                    {
                        Cities.City city = new Cities.City();
                        city.Name = AddNewCityTextBoxAddClient.Text;
                        if (ChooseRegionComboboxAddClient.SelectedIndex != -1 || AddNewRegionTextBoxAddClient.Text != "")
                        {
                            MainWindow.Message("Оберіть або створіть нову область!");
                            return;
                        }
                        else
                        {
                            if (ChooseRegionComboboxAddClient.SelectedIndex != -1)
                            {
                                try
                                {
                                    Regions.Region r = Regions.Region.FindByName(ChooseRegionComboboxAddClient.SelectedItem.ToString());
                                    city.Region_id = r.Id;
                                    city.Save();
                                }
                                catch
                                {
                                    Cities.City ci = Cities.City.FindByName(city.Name);
                                    a.city_id = ci.Id;
                                }
                            }
                            else
                            {
                                Regions.Region r = new Regions.Region();
                                try
                                {
                                    r.Name = AddNewRegionTextBoxAddClient.Text;
                                    r.Save();
                                }
                                catch
                                {
                                    r = Regions.Region.FindByName(r.Name);
                                }
                                try
                                {
                                    city.Region_id = r.Id;
                                    city.Save();
                                }
                                catch
                                {
                                    Cities.City ci = Cities.City.FindByName(city.Name);
                                    a.city_id = ci.Id;
                                }
                            }
                        }
                    }
                }
                a.address = AddressAddClient.Text;
                if (a.address != "" || a.city_id != 0)
                {
                    a.Save();
                    c.Address_id = a.id;
                }
                #endregion
                #region Создает пасспорт
                Passports.Passport p = new Passports.Passport();
                p.Fname = FnamePassportAddClient.Text;
                p.Lname = LnamePassportAddClient.Text;
                p.series = SerialPassportAddClient.Text;
                p.given_by = GivenByPassportAddClient.Text;
                if (GivenWhenPassportAddClient.Text != "")
                {
                    DateTime? tmpdate = Other.Utility.ConvertStringToDateTime(GivenWhenPassportAddClient.Text);
                    if (tmpdate == null)
                    {
                        MainWindow.Message("Дата видачі паспорту заповнена не вірно. Формат: дд.мм.рррр");
                        return;
                    }
                    else
                    {
                        p.given_when = Convert.ToDateTime(tmpdate.Value.ToString());
                    }
                }
                if (GivenTheTimePassportAddClient.Text != "")
                {
                    DateTime? tmpdate1 = Other.Utility.ConvertStringToDateTime(GivenTheTimePassportAddClient.Text);
                    if (tmpdate1 == null)
                    {
                        MainWindow.Message("Термін дії паспорту заповнений не вірно. Формат: дд.мм.рррр");
                        return;
                    }
                    else
                    {
                        p.given_the_time = Convert.ToDateTime(tmpdate1.Value.ToString());
                    }
                }
                if (p.Fname != "" || p.Lname != "" || p.series != "" || p.given_by != "" || p.given_the_time.Year != 1 || p.given_when.Year != 1)
                {
                    p.Save();
                    c.Passport_id = p.id;
                }
                #endregion
                if (selected_manager_for_client_in_add_client == null)
                {
                    c.Working_user_id = Logined.Id;
                }
                else
                {
                    StackPanel sp = selected_manager_for_client_in_add_client.Child as StackPanel;
                    Label lab = sp.Children[0] as Label;
                    c.Working_user_id = Convert.ToInt32(lab.Content);
                }
                c.Notice = NoticeAddClient.Text;
                c.Created_user_id = Logined.Id;
                foreach (var i in phones)
                {
                    if (i.number != "")
                    {
                        if (Phones.Phone.FindByNumber(i.number) == null)
                        {
                            i.Save();
                            c.Phones_Ides.Add(i.Id);
                        }
                        else
                        {
                            c.Phones_Ides.Add(Phones.Phone.FindByNumber(i.number).Id);
                        }
                    }
                }
                foreach (var i in emails)
                {
                    if (i.name != "")
                    {
                        if (Emails.Email.FindByName(i.name) == null)
                        {
                            i.Save();
                            c.Emails_Ides.Add(i.Id);
                        }
                        else
                        {
                            c.Emails_Ides.Add(Emails.Email.FindByName(i.name).Id);
                        }
                    }
                }
                if (ChooseResourseComboboxAddClient.SelectedIndex != -1)
                {
                    c.Resourse_id = Resourses.Resourse.FindByName(ChooseResourseComboboxAddClient.SelectedItem.ToString()).Id;
                }
                Clients.ShortClientShow sh = new Clients.ShortClientShow(c);
                sh.Owner = this;
                sh.ShowDialog();
                MainWindow.Message("Клієнт створений!");
                CleareAddClientFilesd();
                TurnGridBack();
            }
            catch (Exception error)
            {
                MainWindow.Message(error.Message);
            }

        }
        void CleareAddClientFilesd()
        {
            FnameAddClient.Text = "";
            LnameAddClient.Text = "";
            MnameAddClient.Text = "";
            BDayAddClient.Text = "";
            AddClientTelephonsTextboxStack.Children.Clear();
            AddClientTelephonsBtnStack.Children.Clear();
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
            AddPhonesAddClientScroll.ScrollToBottom();
            AddClientEmailTextboxStack.Children.Clear();
            AddClientEmailBtnStack.Children.Clear();
            TextBox t1 = new TextBox();
            t1.Style = Resources["TexBoxStyle"] as Style;
            t1.Margin = new Thickness(0, 10, 0, 10);
            Button b1 = new Button();
            b1.Content = "+";
            b1.Style = Resources["ButtonStyle"] as Style;
            b1.Margin = new Thickness(5, 10, 5, 10);
            b1.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            b1.FontSize = 30;
            b1.Click += AddEmailFieldAddClients_Click;
            AddClientEmailTextboxStack.Children.Add(t1);
            AddClientEmailBtnStack.Children.Add(b1);
            AddPhonesAddClientScroll.ScrollToBottom();
            AddressAddClient.Text = "";
            ChooseRegionComboboxAddClient.SelectedIndex = -1;
            ChooseCityComboboxAddClient.SelectedIndex = -1;
            FnamePassportAddClient.Text = "";
            LnamePassportAddClient.Text = "";
            SerialPassportAddClient.Text = "";
            GivenByPassportAddClient.Text = "";
            GivenWhenPassportAddClient.Text = "";
            GivenTheTimePassportAddClient.Text = "";
            ChooseResourseComboboxAddClient.SelectedIndex = -1;
            if (selected_manager_for_client_in_add_client != null)
                selected_manager_for_client_in_add_client.Style = Resources["ClientBorderStyle"] as Style;
            selected_manager_for_client_in_add_client = null;
            NoticeAddClient.Text = "";
            ManagersWrapPenelInAddClients.Children.Clear();
            ChooseRegionComboboxAddClient.Items.Clear();
            ChooseResourseComboboxAddClient.Items.Clear();
        }
        private void CancelAddClient_Click(object sender, RoutedEventArgs e)
        {
            TurnGridBack();
            CleareAddClientFilesd();
        }
        #endregion
        #region Сетка поиск клиента
        void AddFindedClientsFindClient(string word)
        {
            ClientsPanelFindClient.Children.Clear();
            List<Clients.Client> clients = Clients.Client.FindByWord(word);
            if (clients == null)
            {
                MainWindow.Message("Дуже багато результату з таким словом. Вкажіть конкретніше слово, або скористайтесь фільтром у списку клієнтів!");
                return;
            }
            for (int i = 0; i < clients.Count; i++)
            {
                Border b = new Border();
                b.Width = 300;
                b.Style = Resources["ClientBorderStyle"] as Style;
                StackPanel sp = new StackPanel();
                b.Child = sp;
                Label lid = new Label();
                lid.Content = clients[i].Id.ToString();
                lid.Visibility = System.Windows.Visibility.Hidden;
                Label l = new Label();
                l.Style = Resources["LabelStyle"] as Style;
                l.Content = clients[i].FName + " " + clients[i].LName;
                l.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                Image im = new Image();
                im.Height = 150;
                im.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\client.png"));
                Label l2 = new Label();
                l2.Style = Resources["LabelStyle"] as Style;
                l2.Content = clients[i].Created.ToShortDateString();
                l2.FontWeight = FontWeights.Normal;
                l2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                sp.Children.Add(lid);
                sp.Children.Add(l);
                sp.Children.Add(im);
                sp.Children.Add(l2);
                ClientsPanelFindClient.Children.Add(b);
            }
        }
        private void FindClientBtnFindClient_Click(object sender, RoutedEventArgs e)
        {
            if (FindKeyWordFindClient.Text != "")
            {
                AddFindedClientsFindClient(FindKeyWordFindClient.Text);
            }
        }
        private void CancelFindClient_Click(object sender, RoutedEventArgs e)
        {
            ClientsPanelFindClient.Children.Clear();
            TurnGridBack();
        }
        #endregion
        #region Сетка показать клиентов
        void LoadShowClientsGrid()
        {
            foreach (var i in Users.User.GetAllUsers)
            {
                CreatedManagersComboBoxShowClientsGrid.Items.Add(i.FName + " " + i.LName);
                WorkingManagersComboBoxShowClientsGrid.Items.Add(i.FName + " " + i.LName);
            }
            DateBeforeShowClientsGrid.Text = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)).ToShortDateString();
            DateAfterShowClientsGrid.Text = (DateTime.Now.AddDays(1)).ToString();
            TurnGridNext(ShowClients);
        }
        private void ShowClientsBtnShowClientsGrid_Click(object sender, RoutedEventArgs e)
        {
            string date = "";
            string working = "";
            string created = "";
            date = string.Format("created between '{0}' and '{1}'", MySqlWorker.DataBase.ConvertDateToMySqlString(Convert.ToDateTime(DateBeforeShowClientsGrid.Text)), MySqlWorker.DataBase.ConvertDateToMySqlString(Convert.ToDateTime(DateAfterShowClientsGrid.Text)));

            if (WorkingManagersComboBoxShowClientsGrid.SelectedIndex != -1)
                working = string.Format("&& working_user_id={0}", Users.User.GetAllUsers[WorkingManagersComboBoxShowClientsGrid.SelectedIndex].Id.ToString());
            if (CreatedManagersComboBoxShowClientsGrid.SelectedIndex != -1)
                created = string.Format("&& created_user_id={0}", Users.User.GetAllUsers[CreatedManagersComboBoxShowClientsGrid.SelectedIndex].Id.ToString());
            string query = "select * from Clients where " + date + " " + working + " " + created;
            //База данных
            MySqlWorker.DataBase db = SQL.SqlConnect.db;
            //Создает запрос и возвращает результат
            var list = db.MakeRequest(query);
            ClientsPanelInShowClients.Children.Clear();
            foreach (var i in list)
            {
                AddClientInShowClientsGrid(Clients.Client.FindById(Convert.ToInt32(i["id"])));
            }
        }
        private void CloseShowClients_Click(object sender, RoutedEventArgs e)
        {
            TurnGridBack();
        }
        private void ShowClientsClients_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadShowClientsGrid();
        }
        void AddClientInShowClientsGrid(Clients.Client client)
        {
            Border b = new Border();
            b.Width = 300;
            b.Style = Resources["ClientBorderStyle"] as Style;
            StackPanel sp = new StackPanel();
            b.Child = sp;
            Label lid = new Label();
            lid.Content = client.Id.ToString();
            lid.Visibility = System.Windows.Visibility.Hidden;
            Label l = new Label();
            l.Style = Resources["LabelStyle"] as Style;
            l.Content = client.FName + " " + client.LName;
            l.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            Image im = new Image();
            im.Height = 150;
            im.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\client.png"));
            Label l2 = new Label();
            l2.Style = Resources["LabelStyle"] as Style;
            l2.Content = client.Created.ToShortDateString();
            l2.FontWeight = FontWeights.Normal;
            l2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            sp.Children.Add(lid);
            sp.Children.Add(l);
            sp.Children.Add(im);
            sp.Children.Add(l2);
            ClientsPanelInShowClients.Children.Add(b);
        }
        private void ClearFilterBtnShowClientsGrid_Click(object sender, RoutedEventArgs e)
        {
            DateBeforeShowClientsGrid.Text = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)).ToShortDateString();
            DateAfterShowClientsGrid.Text = (DateTime.Now.AddDays(1)).ToString();
            WorkingManagersComboBoxShowClientsGrid.SelectedIndex = -1;
            WorkingManagersComboBoxShowClientsGrid.SelectedIndex = -1;
        }
        #endregion
        #region Сетка показать клиента
        void LoadShowClient(Clients.Client client)
        {
            if (client == null)
            {
                TurnGridBack();
            }
            TurnGridNext(ShowClient);
            MainInfoImageShowClient.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\Card.png"));
            PassportImageShowClient.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\Passport.png"));
            AddressImageShowClient.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\House.png"));
            InfoImageShowClient.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\info.png"));

            //Основная информация
            FullNameShowClient.Text = client.FName + " " + client.LName + " " + client.MName;
            IdShowClient.Text = client.Id.ToString();
            if (client.Bday.Year != 1)
            {
                BDayShowClient.Text = client.Bday.ToShortDateString();
            }
            else
            {
                BDayShowClient.Text = "Не вказано";
            }
            CreatedShowClient.Text = client.Created.ToShortDateString();
            Passports.Passport p = client.GetPassport;
            if (p != null)
            {
                PassportBorderInShowUser.Visibility = System.Windows.Visibility.Visible;
                PassportBorderInShowUser.Height = 250;
                if (p.Fname != "" || p.Lname != "")
                {
                    FullNamePassportShowClient.Text = p.Fname + " " + p.Lname;
                }
                else
                {
                    FullNamePassportShowClient.Text = "Ім'я та прізвище не вказані!";
                }
                if (p.series != "")
                {
                    SerialPassportShowClient.Text = p.series;
                }
                else
                {
                    SerialPassportShowClient.Text = "Пусто";
                }
                if (p.given_by != "")
                {
                    GivenByPassportShowClient.Text = p.given_by;
                }
                else
                {
                    GivenByPassportShowClient.Text = "Пусто";
                }
                if (p.given_when.Year != 1)
                {
                    GivenWhenPassportShowClient.Text = p.given_when.ToShortDateString();
                }
                else
                {
                    GivenWhenPassportShowClient.Text = "Пусто";
                }
                if (p.given_the_time.Year != -1)
                {
                    GivenForPassportShowClient.Text = p.given_the_time.ToShortDateString();
                }
                else
                {
                    GivenForPassportShowClient.Text = "Пусто";
                }
            }
            else
            {
                PassportBorderInShowUser.Visibility = System.Windows.Visibility.Hidden;
                PassportBorderInShowUser.Height = 0;
            }

            Addresses.Address a = client.GetAddress;
            if (a != null)
            {
                AddressBorderInShowUser.Visibility = System.Windows.Visibility.Visible;
                AddressBorderInShowUser.Height = 250;
                if (a.GetCity != null)
                {
                    CityShowClient.Text = a.GetCity.Name;
                    if (a.GetCity.GetRegion != null)
                    {
                        RegionShowClient.Text = a.GetCity.GetRegion.Name;

                    }
                    else
                    {
                        RegionShowClient.Text = "Не вказано";
                    }
                }
                else
                {
                    CityShowClient.Text = "Не вказано";
                    RegionShowClient.Text = "Не вказано";
                }
                if (a.address != "")
                {
                    AddressShowClient.Text = a.address;
                }
                else
                {
                    AddressShowClient.Text = "Не вказано";
                }
            }
            else
            {
                AddressBorderInShowUser.Visibility = System.Windows.Visibility.Hidden;
                AddressBorderInShowUser.Height = 0;
            }

            Users.User cu = client.GetCreatedUser;
            WhoCreatedShowClient.Text = cu.LName + " " + cu.FName;
            Users.User wu = client.GetWorkingdUser;
            WorkingShowClient.Text = wu.LName + " " + wu.FName;
            Users.User u = client.GetEditdUser;
            if (u != null)
            {
                WhoEditedShowClient.Text = u.LName + " " + u.FName;
            }
            else
            {
                WhoEditedShowClient.Text = "Не редагувався";
            }
            if (client.Notice != "")
                NoticeShowClient.Text = client.Notice;
            else
            {
                NoticeShowClient.Text = "Пусто";
            }
            AddClientPhonesInShowClientGrid(client);
            AddClientEmailsInShowClientGrid(client);
            EditMainInfoImgInShowClientGrid.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\Card.png"));
            EditPassportImgInShowClientGrid.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\Passport.png"));
            EditAddressImgInShowClientGrid.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\House.png"));
            EditContactsImgInShowClientGrid.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\Contacts.png"));
            EditDiscountsImgInShowClientGrid.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\Discount.png"));
            AddRequestImgInShowClientGrid.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\bell.png"));
            AddToBlackListImgInShowClientGrid.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\blacklist.png"));
            DeleteImgInShowClientGrid.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\delete.png"));
            if (ActualEditClientPanel != null)
                ActualEditClientPanel.Visibility = System.Windows.Visibility.Hidden;
            EditClientBorderShowClient.Height = 0;
        }
        private void CloseShowClient_Click(object sender, RoutedEventArgs e)
        {
            TurnGridBack();
        }
        //Сейвы
        private void EditClientMainInfoSaveShowClient_Click(object sender, RoutedEventArgs e)
        {
            if (EditClientLNameShowClient.Text == "")
            {
                MainWindow.Message("Введіть прізвище!");
                return;
            }
            ClientToShow.FName = EditClientFNameShowClient.Text;
            ClientToShow.LName = EditClientLNameShowClient.Text;
            ClientToShow.MName = EditClientMNameShowClient.Text;
            if (Other.Utility.ConvertStringToDateTime(EditClientBDayShowClient.Text) != null)
            {
                ClientToShow.Bday = Convert.ToDateTime(Other.Utility.ConvertStringToDateTime(EditClientBDayShowClient.Text.ToString()));
            }
            else
            {
                ClientToShow.Bday = Convert.ToDateTime("01.01.0001");
            }
            if (EditClientResourseShowClient.SelectedIndex != -1)
            {
                ClientToShow.GetResourse = Resourses.Resourse.FindByName(EditClientResourseShowClient.Items[EditClientResourseShowClient.SelectedIndex].ToString());
            }
            else
            {
                ClientToShow.Resourse_id = 0;
            }
            if (EditClientWorkingManagerShowClient.SelectedIndex != -1)
            {
                ClientToShow.Working_user_id = Users.User.GetAllUsers.ToList()[EditClientWorkingManagerShowClient.SelectedIndex].Id;
            }
            else
            {
                MainWindow.Message("Не обраний менеджер!");
                return;
            }
            ClientToShow.Last_edit_user_id = MainWindow.Logined.Id;
            ClientToShow.Save();
            MainWindow.Message("Інформацію змінено");
            EditClientBorderShowClient.Visibility = System.Windows.Visibility.Hidden;
            EditClientBorderShowClient.Height = 0;
            LoadShowClient(ClientToShow);

        }
        private void EditClientAddressSaveShowClient_Click(object sender, RoutedEventArgs e)
        {
            Addresses.Address a = ClientToShow.GetAddress;
            if (EditClientAddressCityShowClient.SelectedIndex == -1 && EditClientAddressShowClient.Text == "")
            {
                if (a != null)
                {
                    a.Delete();
                    ClientToShow.Address_id = 0;
                }
            }
            else
            {
                if (a == null)
                {
                    a = new Addresses.Address();
                }
                if (EditClientAddressCityShowClient.SelectedIndex != -1)
                {
                    a.GetCity = Cities.City.FindByName(EditClientAddressCityShowClient.SelectedItem.ToString());
                }
                a.address = EditClientAddressShowClient.Text;
                a.Save();
                ClientToShow.Address_id = a.id;
            }
            ClientToShow.Last_edit_user_id = MainWindow.Logined.Id;
            ClientToShow.Save();
            AddressEditClientStackPanelShowClient.Visibility = System.Windows.Visibility.Hidden;
            EditClientBorderShowClient.Height = 0;
            MainWindow.Message("Адресу змінено");
            LoadShowClient(ClientToShow);

        }
        private void EditClientPassportMainInfoSaveShowClient_Click(object sender, RoutedEventArgs e)
        {
            Passports.Passport p = ClientToShow.GetPassport;
            if (EditClientPassportFNameShowClient.Text == "" && EditClientPassportLNameShowClient.Text == "" && EditClientPassportSerieShowClient.Text == "" && EditClientGivenWhenShowClient.Text == "" && EditClientGivenTheTimeShowClient.Text == "" && EditClientGivenByShowClient.Text == "")
            {
                if (p != null)
                {
                    ClientToShow.Passport_id = 0;
                    ClientToShow.Save();
                    p.Delete();
                }
            }
            else
            {
                if (p == null)
                {
                    p = new Passports.Passport();
                }
                p.Fname = EditClientPassportFNameShowClient.Text;
                p.Lname = EditClientPassportLNameShowClient.Text;
                p.series = EditClientPassportSerieShowClient.Text;
                if (Other.Utility.ConvertStringToDateTime(EditClientGivenWhenShowClient.Text) != null)
                {
                    p.given_when = Convert.ToDateTime(EditClientGivenWhenShowClient.Text);
                }
                if (Other.Utility.ConvertStringToDateTime(EditClientGivenTheTimeShowClient.Text) != null)
                {
                    p.given_the_time = Convert.ToDateTime(EditClientGivenTheTimeShowClient.Text);
                }
                p.given_by = EditClientGivenByShowClient.Text;
                p.Save();
                ClientToShow.GetPassport = p;
            }
            ClientToShow.Last_edit_user_id = MainWindow.Logined.Id;
            ClientToShow.Save();
            PassportEditClientStackPanelShowClient.Visibility = System.Windows.Visibility.Hidden;
            EditClientBorderShowClient.Height = 0;
            MainWindow.Message("Паспортні дані змінено");
            LoadShowClient(ClientToShow);
        }
        // Загрузки
        void LoadEditMainInfoShowClient(Clients.Client client)
        {
            if (ActualEditClientPanel != null)
            {
                ActualEditClientPanel.Visibility = System.Windows.Visibility.Hidden;
                ActualEditClientPanel = null;
            }
            ActualEditClientPanel = MainInfoEditClientStackPanelShowClient;
            ActualEditClientPanel.Visibility = System.Windows.Visibility.Visible;
            EditClientFNameShowClient.Text = client.FName;
            EditClientLNameShowClient.Text = client.LName;
            EditClientMNameShowClient.Text = client.MName;
            EditClientBDayShowClient.Text = client.Bday.ToShortDateString();
            var tmp = Resourses.Resourse.GetAllResourses;
            EditClientResourseShowClient.Items.Clear();
            for (int i = 0; i < tmp.Count; i++)
            {
                EditClientResourseShowClient.Items.Add(tmp[i].Name);
                if (client.Resourse_id == tmp[i].Id)
                {
                    EditClientResourseShowClient.SelectedIndex = i;
                }
            }
            var tmp1 = Users.User.GetAllUsers;
            EditClientWorkingManagerShowClient.Items.Clear();
            for (int i = 0; i < tmp1.Count; i++)
            {
                EditClientWorkingManagerShowClient.Items.Add(tmp1[i].FName + " " + tmp1[i].LName);
                if (client.Working_user_id == tmp1[i].Id)
                {
                    EditClientWorkingManagerShowClient.SelectedIndex = i;
                }
            }
            EditClientBorderShowClient.Visibility = System.Windows.Visibility.Visible;
            EditClientBorderShowClient.Height = MainInfoEditClientStackPanelShowClient.Height;
            ShowClientActionsScroll.ScrollToHome();
        }
        void LoadEditPassportShowClient(Passports.Passport passport)
        {
            if (ActualEditClientPanel != null)
            {
                ActualEditClientPanel.Visibility = System.Windows.Visibility.Hidden;
                ActualEditClientPanel = null;
            }
            ActualEditClientPanel = PassportEditClientStackPanelShowClient;
            ActualEditClientPanel.Visibility = System.Windows.Visibility.Visible;
            if (passport != null)
            {

                EditClientPassportFNameShowClient.Text = passport.Fname;
                EditClientPassportLNameShowClient.Text = passport.Lname;
                EditClientPassportSerieShowClient.Text = passport.series;
                if (passport.given_when.Year != 1)
                {
                    EditClientGivenWhenShowClient.Text = passport.given_when.ToShortDateString();
                }
                if (passport.given_the_time.Year != 1)
                {
                    EditClientGivenTheTimeShowClient.Text = passport.given_the_time.ToShortDateString();
                }
                EditClientGivenByShowClient.Text = passport.given_by;
            }
            else
            {
                EditClientPassportFNameShowClient.Text = "";
                EditClientPassportLNameShowClient.Text = "";
                EditClientPassportSerieShowClient.Text = "";
                EditClientGivenWhenShowClient.Text = "";
                EditClientGivenTheTimeShowClient.Text = "";
                EditClientGivenByShowClient.Text = "";
            }
            EditClientBorderShowClient.Visibility = System.Windows.Visibility.Visible;
            EditClientBorderShowClient.Height = PassportEditClientStackPanelShowClient.Height;
            ShowClientActionsScroll.ScrollToHome();
        }
        void LoadEditAddressShowClient(Addresses.Address address)
        {
            if (ActualEditClientPanel != null)
            {
                ActualEditClientPanel.Visibility = System.Windows.Visibility.Hidden;
                ActualEditClientPanel = null;
            }
            ActualEditClientPanel = AddressEditClientStackPanelShowClient;
            ActualEditClientPanel.Visibility = System.Windows.Visibility.Visible;
            if (address != null)
            {
                List<Regions.Region> regions = Regions.Region.GetAllRegions;
                EditClientAddressRegionShowClient.Items.Clear();
                for (int i = 0; i < regions.Count; i++)
                {
                    EditClientAddressRegionShowClient.Items.Add(regions[i].Name);
                    if (address.GetCity != null && address.GetCity.GetRegion != null)
                    {
                        if (address.GetCity.GetRegion.Id == regions[i].Id)
                        {
                            EditClientAddressRegionShowClient.SelectedIndex = i;
                        }
                    }
                }
                if (EditClientAddressRegionShowClient.SelectedIndex != -1)
                {
                    List<Cities.City> cities = Regions.Region.FindByName(EditClientAddressRegionShowClient.SelectedItem.ToString()).GetCities;
                    for (int i = 0; i < cities.Count; i++)
                    {
                        EditClientAddressCityShowClient.Items.Add(cities[i].Name);
                        if (address.GetCity.Id == cities[i].Id)
                        {
                            EditClientAddressCityShowClient.SelectedIndex = i;
                        }
                    }
                }
                EditClientAddressShowClient.Text = address.address;
            }
            else
            {
                List<Regions.Region> regions = Regions.Region.GetAllRegions;
                EditClientAddressRegionShowClient.Items.Clear();
                EditClientAddressCityShowClient.Items.Clear();
                for (int i = 0; i < regions.Count; i++)
                {
                    EditClientAddressRegionShowClient.Items.Add(regions[i].Name);
                }
                EditClientAddressRegionShowClient.SelectedIndex = -1;
                EditClientAddressCityShowClient.SelectedIndex = -1;
                EditClientAddressShowClient.Text = "";
            }
            EditClientBorderShowClient.Visibility = System.Windows.Visibility.Visible;
            EditClientBorderShowClient.Height = AddressEditClientStackPanelShowClient.Height;
            ShowClientActionsScroll.ScrollToHome();
        }
        // Конец загрузки
        //Операции
        private void EditAddressInShowClientGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadEditAddressShowClient(ClientToShow.GetAddress);
        }
        private void EditMainInfoInShowClientGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadEditMainInfoShowClient(ClientToShow);
        }
        private void EditPassportInShowClientGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadEditPassportShowClient(ClientToShow.GetPassport);
        }
        private void EditClientMainInfoCancelShowClient_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            ((StackPanel)b.Parent).Visibility = System.Windows.Visibility.Hidden;
            EditClientBorderShowClient.Height = 0;
            if (ActualEditClientPanel != null)
            {
                ActualEditClientPanel.Visibility = System.Windows.Visibility.Hidden;
                ActualEditClientPanel = null;
            }
        }
        private void EditClientAddressRegionShowClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditClientAddressCityShowClient.Items.Clear();
            if (EditClientAddressRegionShowClient.SelectedIndex != -1)
            {
                List<Cities.City> cities = Regions.Region.FindByName(EditClientAddressRegionShowClient.SelectedItem.ToString()).GetCities;
                for (int i = 0; i < cities.Count; i++)
                {
                    EditClientAddressCityShowClient.Items.Add(cities[i].Name);
                }
            }
        }
        #endregion
        void LoadClientsGrid()
        {
            LoadClientsImages();
            LoadFiveLastClients();
        }
        void LoadClientsImages()
        {

            AddClientBtnClients.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\AddBtn.png"));
            ShowClientBtnClients.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\ShowBtn.png"));
            SearchClientBtnClients.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\SearchBtn.png"));

        }
        void AddClientPhonesInShowClientGrid(Clients.Client client)
        {
            PhonesListInShowClient.Children.Clear();
            foreach (var i in client.GetPhones)
            {
                TextBox t = new TextBox();
                t.Style = Resources["TexBoxStyle"] as Style;
                t.IsReadOnly = true;
                t.Margin = new Thickness(10);
                t.Text = i.number;
                PhonesListInShowClient.Children.Add(t);
            }
        }
        void AddClientEmailsInShowClientGrid(Clients.Client client)
        {
            EmailsListInShowClient.Children.Clear();
            foreach (var i in client.GetEmails)
            {
                TextBox t = new TextBox();
                t.Style = Resources["TexBoxStyle"] as Style;
                t.IsReadOnly = true;
                t.Margin = new Thickness(10);
                t.Text = i.name;
                EmailsListInShowClient.Children.Add(t);
            }
        }
        void LoadFiveLastClients()
        {
            List<Clients.Client> clients = Clients.Client.GetFiveLastClients();
            FiveLastClientsGridClientGrid.Children.Clear();
            for (int i = 0; i < clients.Count; i++)
            {
                Border b = new Border();
                b.MouseLeftButtonDown += LookClient;
                b.Style = Resources["ClientBorderStyle"] as Style;
                StackPanel sp = new StackPanel();
                b.Child = sp;
                Label lid = new Label();
                lid.Content = clients[i].Id.ToString();
                lid.Visibility = System.Windows.Visibility.Hidden;
                Label l = new Label();
                l.Style = Resources["LabelStyle"] as Style;
                l.Content = clients[i].FName + " " + clients[i].LName;
                l.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                Image im = new Image();
                im.Height = 150;
                im.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\client.png"));
                Label l2 = new Label();
                l2.Style = Resources["LabelStyle"] as Style;
                l2.Content = clients[i].Created.ToShortDateString();
                l2.FontWeight = FontWeights.Normal;
                l2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                sp.Children.Add(lid);
                sp.Children.Add(l);
                sp.Children.Add(im);
                sp.Children.Add(l2);
                FiveLastClientsGridClientGrid.Children.Add(b);
                b.SetValue(Grid.ColumnProperty, i);
            }
        }
        private void FindClientGridClients_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TurnGridNext(FindClient);
        }
        private void AddClientClients_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadGridAddClient();
            TurnGridNext(ClientAddGrid);
        }
        #endregion
        //-----------------
        #region Сетка Пользователи
        void LoadUsersGrid()
        {
            AddNewUserImageInUsers.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Other\add.png"));
            AddAllUsersInUsersGrid();

        }
        void AddAllUsersInUsersGrid()
        {
            Border b = UsersPanelInUsers.Children[0] as Border;
            UsersPanelInUsers.Children.Clear();
            UsersPanelInUsers.Children.Add(b);
            foreach (var i in Users.User.GetAllUsers)
            {
                AddUserUsersGrid(i);
            }
        }
        void AddUserUsersGrid(Users.User user)
        {
            Border b = new Border();
            b.Width = 250;
            b.Style = Resources["ClientBorderStyle"] as Style;
            StackPanel sp = new StackPanel();
            b.Child = sp;
            Label lid = new Label();
            lid.Content = user.Id.ToString();
            lid.Visibility = System.Windows.Visibility.Hidden;
            Label l = new Label();
            l.Style = Resources["LabelStyle"] as Style;
            l.Content = user.FName + " " + user.LName;
            l.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            Image im = new Image();
            im.Height = 150;
            im.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\client.png"));
            Label l2 = new Label();
            l2.Style = Resources["LabelStyle"] as Style;
            l2.Content = user.Created.ToShortDateString();
            l2.FontWeight = FontWeights.Normal;
            l2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            sp.Children.Add(lid);
            sp.Children.Add(l);
            sp.Children.Add(im);
            sp.Children.Add(l2);
            UsersPanelInUsers.Children.Add(b);
        }
        private void CreateNewUserInUsersGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.Logined.IsAdmin)
                LoadAddUserGrid();
            else
            {
                MainWindow.Message("У Вас немає прав доступу!");
            }
        }
        #region Создать пользователя
        private void AddPhonesAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (AddPhoneBoxAddUser.Text != "")
            {
                PhonesListBoxInAddUser.Items.Add(Other.Utility.ConvertStringToPhoneString(AddPhoneBoxAddUser.Text));
                AddPhoneBoxAddUser.Text = "";
            }
        }
        private void AddEmailsAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (AddEmailBoxAddUser.Text != "")
            {
                EmailsListBoxInAddUser.Items.Add(AddEmailBoxAddUser.Text);
                AddEmailBoxAddUser.Text = "";
            }
        }
        void LoadAddUserGrid()
        {
            TurnGridNext(AddUserInUsersGrid);
            foreach (var i in Positions.Position.GetAllPositions)
            {
                PositionsComboboxInAddUsersGrid.Items.Add(i.Name);
            }
        }
        void ClearAllUserValuesInAddUserGrid()
        {
            FnameAddUser.Text = "";
            LnameAddUser.Text = "";
            MnameAddUser.Text = "";
            LoginAddUser.Text = "";
            PasswordAddUser.Text = "";
            BDayAddUser.Text = "";
            PositionsComboboxInAddUsersGrid.Items.Clear();
            AdminChekBoxInAddUserGrid.IsChecked = false;
            AddPhoneBoxAddUser.Text = "";
            AddEmailBoxAddUser.Text = "";
            PhonesListBoxInAddUser.Items.Clear();
            EmailsListBoxInAddUser.Items.Clear();
        }
        private void CancelAddUser_Click(object sender, RoutedEventArgs e)
        {
            ClearAllUserValuesInAddUserGrid();
            TurnGridBack();
        }
        private void SaveAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (LoginAddUser.Text == "")
            {
                MainWindow.Message("Ведіть логін!");
                return;
            }
            if (PasswordAddUser.Text == "")
            {
                MainWindow.Message("Ведіть пароль!");
                return;
            }
            if (Users.User.FindByLogin(LoginAddUser.Text) != null)
            {
                MainWindow.Message("Такий логін вже існує!");
                return;
            }
            if (PositionsComboboxInAddUsersGrid.SelectedIndex == -1)
            {
                MainWindow.Message("Оберіть посаду!");
                return;
            }
            Users.User u = new Users.User();
            u.FName = FnameAddUser.Text;
            u.LName = LnameAddUser.Text;
            u.MName = MnameAddUser.Text;
            u.Login = LoginAddUser.Text;
            u.Password = PasswordAddUser.Text;
            if (BDayAddUser.Text != "")
            {
                DateTime? b = Other.Utility.ConvertStringToDateTime(BDayAddUser.Text);
                if (b == null)
                {
                    MainWindow.Message("Дата народження введена не вірно! (Формат: дд.мм.рррр)");
                    return;
                }
                else
                {
                    u.Bday = Convert.ToDateTime(b.Value.ToShortDateString());
                }
            }
            u.Position = Positions.Position.FindByName(PositionsComboboxInAddUsersGrid.SelectedItem.ToString());
            if (AdminChekBoxInAddUserGrid.IsChecked == true)
            {
                u.IsAdmin = true;
            }
            else
            {
                u.IsAdmin = false;
            }
            u.Save();
            foreach (var i in PhonesListBoxInAddUser.Items)
            {
                Phones.Phone p = Phones.Phone.FindByNumber(i.ToString());
                if (p == null)
                {
                    p = new Phones.Phone();
                    p.number = i.ToString();
                    p.Users_Ides.Add(u.Id);
                    p.Save();
                }
                else
                {
                    p.Users_Ides.Add(u.Id);
                    p.Save();
                }
            }
            foreach (var i in EmailsListBoxInAddUser.Items)
            {
                Emails.Email p = Emails.Email.FindByName(i.ToString());
                if (p == null)
                {
                    p = new Emails.Email();
                    p.name = i.ToString();
                    p.Users_Ides.Add(u.Id);
                    p.Save();
                }
                else
                {
                    p.Users_Ides.Add(u.Id);
                    p.Save();
                }
            }
            AddAllUsersInUsersGrid();
            ClearAllUserValuesInAddUserGrid();
            TurnGridBack();
        }
        private void LoginAddUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LoginAddUser.Text != "")
            {
                char sumbol = LoginAddUser.Text[LoginAddUser.Text.Length - 1];
                if (sumbol == ' ')
                {
                    LoginAddUser.Text = LoginAddUser.Text = LoginAddUser.Text.Substring(0, LoginAddUser.Text.Length - 1);
                    LoginAddUser.SelectionStart = LoginAddUser.Text.Length;
                }
            }
        }
        #endregion
        #endregion
        //-----------------
        #region Навигация
        void TurnGridBack()
        {
            grids[grids.Count - 1].Visibility = System.Windows.Visibility.Hidden;
            grids.RemoveAt(grids.Count - 1);
            grids[grids.Count - 1].Visibility = System.Windows.Visibility.Visible;
        }
        void TurnGridNext(Grid grid)
        {
            grids[grids.Count - 1].Visibility = System.Windows.Visibility.Hidden;
            grid.Visibility = System.Windows.Visibility.Visible;
            grids.Add(grid);
        }
        void TurnGridMain(Grid grid)
        {
            while (grids.Count > 1)
            {
                TurnGridBack();
            }
            grids[grids.Count - 1].Visibility = System.Windows.Visibility.Hidden;
            grids.RemoveAt(grids.Count - 1);
            grids.Add(grid);
            grid.Visibility = System.Windows.Visibility.Visible;
        }
        #endregion
        //-----------------
        #region События при нажатии на кнопки меню
        private void ClientsButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadClientsGrid();
            TurnGridMain(ClientsGrid);
        }
        private void UsersButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadUsersGrid();
            TurnGridMain(UsersGrid);
        }

        #endregion
        //-----------------
    }
}
