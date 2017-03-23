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
using System.Windows.Threading;
using Microsoft.Win32;
namespace OTBaseNew
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Проверки
        bool CheckToLastFiveClients = false;
        #endregion
        public DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
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
                timer.Tick += new EventHandler(dispatcherTimer_Tick);
                timer.Interval = new TimeSpan(0, 0, 10);
                timer.Start();


                //MainWindow.Message(string.Format("Привіт, {0}", Logined.FName));
                NameLable.Content = Logined.FName;
                LoadImages();
                grids.Add(ClientsGrid);
                LoadAddRequest();
            }
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (CheckToLastFiveClients)
                CheckLastFiveAddedClients();
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
        void CleareAllCheckes()
        {
            CheckToLastFiveClients = false;
        }
        //-----------------        
        #region Сетка Клиенты
        bool selectclient = false;
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
                c.OnClientAdd += c_OnClientAdd;
                sh.ShowDialog();
                CleareAddClientFilesd();
                TurnGridBack();
            }
            catch (Exception error)
            {
                MainWindow.Message(error.Message);
            }
        }
        void c_OnClientAdd(object sender, EventArgs e)
        {
            CheckLastFiveAddedClients();
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

                Border b = CreateClientBorder(clients[i]);
                if (selectclient)
                {
                    for (int j = 0; j < selectedclientsincreaterequest.Count; j++)
                    {
                        if (clients[i].Id == selectedclientsincreaterequest[j].Id)
                        {
                            b.Style = (Style)Resources["ClientSelectedBorderStyle"];
                            break;
                        }
                    }
                }
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
            if (selectclient)
            {
                selectclient = false;
            }
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
            Border b = CreateClientBorder(client);
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
            DiscountsImageShowClient.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Clients\Discount.png"));
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

            DicsountsWrapPanelShowClient.Children.Clear();
            List<Discounts.Discount> discounts = client.GetDiscounts;
            foreach (var i in discounts)
            {
                TextBox t = new TextBox();
                t.Style = Resources["TexBoxStyle"] as Style;
                t.Margin = new Thickness(10, 0, 0, 0);
                t.IsReadOnly = true;
                t.Text = i.Tour + ": " + i.discount;
                DicsountsWrapPanelShowClient.Children.Add(t);
            }
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
        private void EditClientContactsSaveShowClient_Click(object sender, RoutedEventArgs e)
        {
            List<Phones.Phone> phones = new List<Phones.Phone>();
            foreach (var i in PhonesStackPanelInShowClient.Children)
            {
                TextBox t = i as TextBox;
                string number = Other.Utility.ConvertStringToPhoneString(t.Text);
                if (number != "")
                {
                    Phones.Phone p = Phones.Phone.FindByNumber(number);
                    if (p == null)
                    {
                        p = new Phones.Phone();
                        p.number = number;
                        p.Save();
                    }
                    phones.Add(p);
                }
            }
            List<Phones.Phone> uphones = ClientToShow.GetPhones;

            foreach (var i in phones)
            {
                bool exist = false;
                foreach (var j in uphones)
                {
                    if (j.Id == i.Id)
                    {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                {
                    ClientToShow.Phones_Ides.Add(i.Id);
                }
            }
            uphones = ClientToShow.GetPhones;
            foreach (var i in uphones)
            {
                bool exist = false;
                foreach (var j in phones)
                {
                    if (i.Id == j.Id)
                    {
                        exist = true;
                    }
                }
                if (!exist)
                {
                    ClientToShow.Phones_Ides.Remove(i.Id);
                }
            }



            List<Emails.Email> emails = new List<Emails.Email>();
            foreach (var i in EmailsStackPanelInShowClient.Children)
            {
                TextBox t = i as TextBox;
                string number = t.Text;
                if (number != "")
                {
                    Emails.Email p = Emails.Email.FindByName(number);
                    if (p == null)
                    {
                        p = new Emails.Email();
                        p.name = number;
                        p.Save();
                    }
                    emails.Add(p);
                }
            }
            List<Emails.Email> uemails = ClientToShow.GetEmails;

            foreach (var i in emails)
            {
                bool exist = false;
                foreach (var j in uemails)
                {
                    if (j.Id == i.Id)
                    {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                {
                    ClientToShow.Emails_Ides.Add(i.Id);
                }
            }
            uemails = ClientToShow.GetEmails;
            foreach (var i in uemails)
            {
                bool exist = false;
                foreach (var j in emails)
                {
                    if (i.Id == j.Id)
                    {
                        exist = true;
                    }
                }
                if (!exist)
                {
                    ClientToShow.Emails_Ides.Remove(i.Id);
                }
            }

            ClientToShow.Last_edit_user_id = MainWindow.Logined.Id;
            ClientToShow.Save();
            PassportEditClientStackPanelShowClient.Visibility = System.Windows.Visibility.Hidden;
            EditClientBorderShowClient.Height = 0;
            MainWindow.Message("Контактні дані змінено");
            LoadShowClient(ClientToShow);
        }
        private void EditClientDiscountssSaveShowClient_Click(object sender, RoutedEventArgs e)
        {
            List<Discounts.Discount> discounts = ClientToShow.GetDiscounts;
            foreach (var i in discounts)
            {
                i.Delete();
            }
            ClientToShow.Discounts_Ides.Clear();
            for (int i = 0; i < EditDicsountDurationStackPanelInShowClient.Children.Count; i++)
            {
                TextBox t1 = EditDicsountDurationStackPanelInShowClient.Children[i] as TextBox;
                TextBox t2 = EditDicsountValueStackPanelInShowClient.Children[i] as TextBox;
                ComboBox c = EditDicsountChouseOperetorStackPanelInShowClient.Children[i] as ComboBox;
                if (t1.Text == "" && t2.Text == "")
                {

                }
                else
                {
                    if (t1.Text == "" || t2.Text == "")
                    {
                        MainWindow.Message("Всі поля мають бути заповнені!");
                        return;
                    }
                    else
                    {
                        Discounts.Discount d = new Discounts.Discount();
                        d.Tour = t1.Text;
                        d.discount = t2.Text;
                        if (c.SelectedIndex != -1)
                        {
                            Operators.Operator o = Operators.Operator.FindByName(c.SelectedItem.ToString());
                            if (o != null)
                            {
                                d.Operator_id = o.Id;
                            }
                        }
                        d.Client_id = ClientToShow.Id;
                        d.Save();
                        ClientToShow.Discounts_Ides.Add(d.Id);
                    }
                }

            }
            ClientToShow.Last_edit_user_id = MainWindow.Logined.Id;
            ClientToShow.Save();
            PassportEditClientStackPanelShowClient.Visibility = System.Windows.Visibility.Hidden;
            EditClientBorderShowClient.Height = 0;
            MainWindow.Message("Знижки змінено");
            LoadShowClient(ClientToShow);
        }
        private void EditClientDeleteShowClient_Click(object sender, RoutedEventArgs e)
        {
            ClientToShow.OnClientRemove += ClientToShow_OnClientRemove;
            ClientToShow.Delete();
            MainWindow.Message("Клієнт був видалений");
            TurnGridBack();
        }
        void ClientToShow_OnClientRemove(object sender, EventArgs e)
        {
            CheckLastFiveAddedClients();
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
        void LoadEditContactsShowClient(Clients.Client client)
        {
            if (ActualEditClientPanel != null)
            {
                ActualEditClientPanel.Visibility = System.Windows.Visibility.Hidden;
                ActualEditClientPanel = null;
            }
            ActualEditClientPanel = ContactsEditClientStackPanelShowClient;
            ActualEditClientPanel.Visibility = System.Windows.Visibility.Visible;
            List<Phones.Phone> phones = client.GetPhones;
            List<Emails.Email> emails = client.GetEmails;
            PhonesStackPanelInShowClient.Children.Clear();
            for (int i = 0; i < phones.Count; i++)
            {
                TextBox t = new TextBox();
                t.Text = phones[i].number;
                t.Margin = new Thickness(10);
                t.Style = Resources["TexBoxStyle"] as Style;
                PhonesStackPanelInShowClient.Children.Add(t);
            }
            EmailsStackPanelInShowClient.Children.Clear();
            for (int i = 0; i < emails.Count; i++)
            {
                TextBox t = new TextBox();
                t.Text = emails[i].name;
                t.Margin = new Thickness(10);
                t.Style = Resources["TexBoxStyle"] as Style;
                EmailsStackPanelInShowClient.Children.Add(t);
            }
            EditClientBorderShowClient.Visibility = System.Windows.Visibility.Visible;
            EditClientBorderShowClient.Height = AddressEditClientStackPanelShowClient.Height;
            ShowClientActionsScroll.ScrollToHome();
        }
        void LoadEditDiscountsShowClient(Clients.Client client)
        {
            if (ActualEditClientPanel != null)
            {
                ActualEditClientPanel.Visibility = System.Windows.Visibility.Hidden;
                ActualEditClientPanel = null;
            }
            ActualEditClientPanel = DiscountsEditClientStackPanelShowClient;
            ActualEditClientPanel.Visibility = System.Windows.Visibility.Visible;
            List<Discounts.Discount> discountrs = client.GetDiscounts;
            EditDicsountChouseOperetorStackPanelInShowClient.Children.Clear();
            EditDicsountValueStackPanelInShowClient.Children.Clear();
            EditDicsountDurationStackPanelInShowClient.Children.Clear();
            foreach (var i in discountrs)
            {
                TextBox t = new TextBox();
                t.Style = Resources["TexBoxStyle"] as Style;
                t.Margin = new Thickness(10);
                t.Text = i.Tour;
                EditDicsountDurationStackPanelInShowClient.Children.Add(t);
                t = new TextBox();
                t.Style = Resources["TexBoxStyle"] as Style;
                t.Margin = new Thickness(10);
                t.Text = i.discount;
                EditDicsountValueStackPanelInShowClient.Children.Add(t);
                ComboBox c = new ComboBox();
                c.Margin = new Thickness(10);
                c.Style = Resources["ComboBoxStyle"] as Style;
                List<Operators.Operator> operators = Operators.Operator.GetAllOperators;
                for (int j = 0; j < operators.Count; j++)
                {
                    c.Items.Add(operators[j].Name);
                    if (i.Operator_id == operators[j].Id)
                    {
                        c.SelectedIndex = j;
                    }
                }
                EditDicsountChouseOperetorStackPanelInShowClient.Children.Add(c);
            }
            EditClientBorderShowClient.Visibility = System.Windows.Visibility.Visible;
            EditClientBorderShowClient.Height = AddressEditClientStackPanelShowClient.Height;
            ShowClientActionsScroll.ScrollToHome();
        }
        void LoadDeleteShowClient()
        {
            if (ActualEditClientPanel != null)
            {
                ActualEditClientPanel.Visibility = System.Windows.Visibility.Hidden;
                ActualEditClientPanel = null;
            }
            ActualEditClientPanel = DeleteClientStackPanelShowClient;
            ActualEditClientPanel.Visibility = System.Windows.Visibility.Visible;
            EditClientBorderShowClient.Visibility = System.Windows.Visibility.Visible;
            EditClientBorderShowClient.Height = AddressEditClientStackPanelShowClient.Height;
            ShowClientActionsScroll.ScrollToHome();
        }
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
        private void EditDiscountsInShowClientGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadEditDiscountsShowClient(ClientToShow);
        }
        private void DeleteInShowClientGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadDeleteShowClient();
        }
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadEditContactsShowClient(ClientToShow);
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
        private void AddPhoneInShowClient_Click(object sender, RoutedEventArgs e)
        {
            TextBox t = new TextBox();
            t.Margin = new Thickness(10);
            t.Style = Resources["TexBoxStyle"] as Style;
            PhonesStackPanelInShowClient.Children.Add(t);
        }
        private void AddEmailInShowClient_Click(object sender, RoutedEventArgs e)
        {
            TextBox t = new TextBox();
            t.Margin = new Thickness(10);
            t.Style = Resources["TexBoxStyle"] as Style;
            EmailsStackPanelInShowClient.Children.Add(t);
        }
        private void AddValueAndDurationFieldInEditSidcountsInShowCLient_Click(object sender, RoutedEventArgs e)
        {
            TextBox t = new TextBox();
            t.Style = Resources["TexBoxStyle"] as Style;
            t.Margin = new Thickness(10);
            EditDicsountDurationStackPanelInShowClient.Children.Add(t);
            t = new TextBox();
            t.Style = Resources["TexBoxStyle"] as Style;
            t.Margin = new Thickness(10);
            EditDicsountValueStackPanelInShowClient.Children.Add(t);
            ComboBox c = new ComboBox();
            c.Margin = new Thickness(10);
            c.Style = Resources["ComboBoxStyle"] as Style;
            List<Operators.Operator> operators = Operators.Operator.GetAllOperators;
            foreach (var i in operators)
            {
                c.Items.Add(i.Name);
            }
            EditDicsountChouseOperetorStackPanelInShowClient.Children.Add(c);
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
                Border b = CreateClientBorder(clients[i]);
                b.Width = double.NaN;
                b.SetValue(Grid.ColumnProperty, i);
                FiveLastClientsGridClientGrid.Children.Add(b);
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
        public Border CreateClientBorder(Clients.Client client)
        {
            Border b = new Border();
            b.Width = 300;
            if (!selectclient)
            {
                b.MouseLeftButtonDown += LookClient;
            }
            if (selectclient)
            {
                b.MouseLeftButtonDown += selectclienttolist_MouseLeftButtonDown;
            }
            b.Style = Resources["ClientBorderStyle"] as Style;
            StackPanel sp = new StackPanel();
            b.Child = sp;
            Label lid = new Label();
            lid.Height = 0;
            lid.Content = client.Id.ToString();
            lid.Visibility = System.Windows.Visibility.Hidden;
            //----------
            Button but = new Button();
            but.Height = 0;
            but.Width = double.NaN;
            but.Margin = new Thickness(10);
            but.Content = "X";
            but.Visibility = System.Windows.Visibility.Hidden;
            //----------
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
            sp.Children.Add(but);
            sp.Children.Add(l);
            sp.Children.Add(im);
            sp.Children.Add(l2);
            return b;
        }
        #endregion
        //-----------------
        #region Сетка Пользователи
        void LoadUsersGrid()
        {
            AddNewUserImageInUsers.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Other\add.png"));
            AddAllUsersInUsersGrid();

        }
        void LoadShowUserGrid(Users.User user)
        {
            TurnGridNext(ShowUserInUserGrid);
            FullNameInShowUserGrid.Content = user.FName + " " + user.LName + " " + user.MName;
            IdInShowUserGrid.Text = user.Id.ToString();
            LoginInShowUserGrid.Text = user.Login;
            BDayInShowUserGrid.Text = user.Bday.ToShortDateString();
            CreatedDateInShowUserGrid.Text = user.Created.ToShortDateString();
            PositionInShowUserGrid.Text = user.Position.Name;
            ChangePasswordLabelInShowUserGrid.Visibility = System.Windows.Visibility.Visible;
            ChangePasswordLabelInShowUserGrid.Height = double.NaN;
            ChangePhotoLabelInShowUserGrid.Visibility = System.Windows.Visibility.Visible;
            ChangePhotoLabelInShowUserGrid.Height = double.NaN;
            ChangeNameLabelInShowUserGrid.Visibility = System.Windows.Visibility.Visible;
            ChangeNameLabelInShowUserGrid.Height = double.NaN;
            ChangePositionLabelInShowUserGrid.Visibility = System.Windows.Visibility.Visible;
            ChangePositionLabelInShowUserGrid.Height = double.NaN;
            DeleteLabelInShowUserGrid.Visibility = System.Windows.Visibility.Visible;
            DeleteLabelInShowUserGrid.Height = double.NaN;
            if (MainWindow.Logined.Id != user.Id && !MainWindow.Logined.IsAdmin)
            {
                ChangePasswordLabelInShowUserGrid.Visibility = System.Windows.Visibility.Hidden;
                ChangePasswordLabelInShowUserGrid.Height = 0;
                ChangePhotoLabelInShowUserGrid.Visibility = System.Windows.Visibility.Hidden;
                ChangePhotoLabelInShowUserGrid.Height = 0;
            }
            if (!MainWindow.Logined.IsAdmin)
            {
                ChangeNameLabelInShowUserGrid.Visibility = System.Windows.Visibility.Hidden;
                ChangeNameLabelInShowUserGrid.Height = 0;
                ChangePositionLabelInShowUserGrid.Visibility = System.Windows.Visibility.Hidden;
                ChangePositionLabelInShowUserGrid.Height = 0;
                DeleteLabelInShowUserGrid.Visibility = System.Windows.Visibility.Hidden;
                DeleteLabelInShowUserGrid.Height = 0;
            }
        }
        void AddAllUsersInUsersGrid()
        {
            Border b = UsersPanelInUsers.Children[0] as Border;
            UsersPanelInUsers.Children.Clear();
            UsersPanelInUsers.Children.Add(b);
            foreach (var i in Users.User.GetAllUsers)
            {
                UsersPanelInUsers.Children.Add(AddUserUsersGrid(i));
            }
        }
        Border AddUserUsersGrid(Users.User user)
        {
            Border b = new Border();
            b.MouseLeftButtonDown += UserBorder_MouseLeftButtonDown;
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
            return b;
        }

        void UserBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            StackPanel sp = b.Child as StackPanel;
            Label l = sp.Children[0] as Label;
            int id = Convert.ToInt32(l.Content);
            Users.User u = Users.User.FindById(id);
            if (u != null)
                LoadShowUserGrid(u);
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
        private void ChangePasswordLabelInShowUserGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Users.ChangeUserPassword up = new Users.ChangeUserPassword(Users.User.FindById(Convert.ToInt32(IdInShowUserGrid.Text)));
            up.Owner = this;
            up.ShowDialog();
        }
        private void ChangeNameLabelInShowUserGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Users.ChangeUserName up = new Users.ChangeUserName(Users.User.FindById(Convert.ToInt32(IdInShowUserGrid.Text)));
            up.Owner = this;
            up.ShowDialog();
            LoadShowUserGrid(Users.User.FindById(Convert.ToInt32(IdInShowUserGrid.Text)));
        }
        private void CloseShowUserGridBtn_Click(object sender, RoutedEventArgs e)
        {
            TurnGridBack();
        }
        private void ChangePositionLabelInShowUserGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Users.ChangePosition up = new Users.ChangePosition(Users.User.FindById(Convert.ToInt32(IdInShowUserGrid.Text)));
            up.Owner = this;
            up.ShowDialog();
            LoadShowUserGrid(Users.User.FindById(Convert.ToInt32(IdInShowUserGrid.Text)));
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
        #region Сетка Операторы
        int operatoridforedithim = 0;
        void LoadOperatorGrid()
        {
            CreateOperatorNameInOperatorGrid.Text = "";
            CreateOperatorSiteInOperatorGrid.Text = "";
            DocumentsPanelInOperatorsGrid.Children.Clear();
            Label l = new Label();
            l.Content = "Оберіть оператора";
            l.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            l.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            l.FontSize = 30;
            l.Foreground = Brushes.Gray;
            OperatorInfoInOperatorsGrid.Child = l;
            OperatorsPanelInOperatorGrid.Children.Clear();
            foreach (var i in Operators.Operator.GetAllOperators)
            {
                OperatorsPanelInOperatorGrid.Children.Add(CreateOperatorGrid(i));
            }
            operatoridforedithim = 0;
        }
        Border CreateDocumentBorder(string path)
        {
            Border b = new Border();
            b.MouseLeftButtonDown += DocumentBorder_MouseLeftButtonDown;
            b.Style = Resources["DocumentBorder"] as Style;
            StackPanel sp = new StackPanel();
            sp.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            b.Child = sp;
            Label l = new Label();
            l.Visibility = System.Windows.Visibility.Hidden;
            l.Content = path;
            sp.Children.Add(l);
            Image im = new Image();
            im.Width = 120;
            im.Height = 120;
            im.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            im.Margin = new Thickness(0, 10, 0, 0);
            im.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Other\document.png"));
            sp.Children.Add(im);
            TextBlock t = new TextBlock();
            t.Style = Resources["TextBlockStyle"] as Style;
            t.TextAlignment = TextAlignment.Center;
            t.Margin = new Thickness(0, 10, 0, 0);
            t.Text = System.IO.Path.GetFileName(path);
            sp.Children.Add(t);
            return b;
        }
        Border CreateDocumentBorder(Documents.Document doc)
        {
            Border b = new Border();
            b.MouseLeftButtonDown += BorderDocumentDownloadLeftMouseDown;
            b.Style = Resources["DocumentBorder"] as Style;
            StackPanel sp = new StackPanel();
            sp.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            b.Child = sp;
            Label id = new Label();
            id.Content = doc.Id;
            id.Visibility = System.Windows.Visibility.Hidden;
            sp.Children.Add(id);
            Label l = new Label();
            l.Visibility = System.Windows.Visibility.Hidden;
            l.Content = doc.Name + doc.Extension;
            sp.Children.Add(l);
            Image im = new Image();
            im.Width = 100;
            im.Height = 100;
            im.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            im.Margin = new Thickness(0, -40, 0, 0);
            im.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Other\document.png"));
            sp.Children.Add(im);
            TextBlock t = new TextBlock();
            t.Style = Resources["TextBlockStyle"] as Style;
            t.TextAlignment = TextAlignment.Center;
            t.Margin = new Thickness(0, 10, 0, 0);
            string name;
            if ((doc.Name + doc.Extension).Length > 10)
            {
                name = (doc.Name + doc.Extension).Substring(0, 10) + "...";
            }
            else
            {
                name = doc.Name + doc.Extension;
            }
            t.Text = name;
            t.TextWrapping = TextWrapping.Wrap;
            t.ToolTip = doc.Name + doc.Extension;
            sp.Children.Add(t);
            return b;
        }
        void BorderDocumentDownloadLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            Label l = ((StackPanel)b.Child).Children[0] as Label;
            int id = Convert.ToInt32(l.Content);
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Documents.Document doc = Documents.Document.FindById(id);
                    doc.WriteDocument(dialog.SelectedPath);
                }
            }

        }
        private void CreateOperatorDocumentsInOperatorGrid_Click(object sender, RoutedEventArgs e)
        {
            if (CreateOperatorDocumentsInOperatorGrid.Background == Brushes.LightGreen)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == true)
                {
                    string path = ofd.FileName;
                    Border b = CreateDocumentBorder(path);
                    DocumentsPanelInOperatorsGrid.Children.Add(b);
                }
            }
            else
            {
                List<Border> BordersForDelete = new List<Border>();
                foreach (var i in DocumentsPanelInOperatorsGrid.Children)
                {
                    Border bo = i as Border;
                    if (bo.Style == (Style)Resources["SelectedDocumentBorder"])
                    {
                        BordersForDelete.Add(bo);
                    }
                }
                foreach (var i in BordersForDelete)
                {
                    DocumentsPanelInOperatorsGrid.Children.Remove(i);
                }
                CreateOperatorDocumentsInOperatorGrid.Background = Brushes.LightGreen;
                CreateOperatorDocumentsInOperatorGrid.Content = "Додати файл";
            }
        }
        private void DocumentBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border b = (Border)sender;
            if (b.Style == (Style)Resources["DocumentBorder"])
            {
                b.Style = Resources["SelectedDocumentBorder"] as Style;
            }
            else
            {
                b.Style = Resources["DocumentBorder"] as Style;
            }
            bool existselected = false;
            foreach (var i in DocumentsPanelInOperatorsGrid.Children)
            {
                Border bo = i as Border;
                if (bo.Style == (Style)Resources["SelectedDocumentBorder"])
                {
                    existselected = true;
                    break;
                }
            }
            if (existselected)
            {
                CreateOperatorDocumentsInOperatorGrid.Background = Brushes.Red;
                CreateOperatorDocumentsInOperatorGrid.Content = "Видалити обрані файли";
            }
            else
            {
                CreateOperatorDocumentsInOperatorGrid.Background = Brushes.LightGreen;
                CreateOperatorDocumentsInOperatorGrid.Content = "Додати файл";
            }
        }
        private void CreateOperatorInOperatorGrid_Click(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.Logined.IsAdmin)
            {
                MainWindow.Message("У Вас немає права на створення оператора!");
                return;
            }
            if (CreateOperatorNameInOperatorGrid.Text == "")
            {
                MainWindow.Message("Назва не може бути пуста!");
                return;
            }
            Operators.Operator o = new Operators.Operator();
            o.Name = CreateOperatorNameInOperatorGrid.Text;
            o.Site = CreateOperatorSiteInOperatorGrid.Text;
            o.Save();
            foreach (var i in DocumentsPanelInOperatorsGrid.Children)
            {
                Documents.Document d = new Documents.Document();
                Border b = i as Border;
                StackPanel s = b.Child as StackPanel;
                Label l = s.Children[0] as Label;
                string path = l.Content.ToString();
                d.Bytes = Documents.Document.GetBytesFromFile(path);
                d.Name = System.IO.Path.GetFileNameWithoutExtension(path);
                d.Extension = System.IO.Path.GetExtension(path);
                d.Operator_id = o.Id;
                d.Save();
            }
            MainWindow.Message("Оператор створений!");
            LoadOperatorGrid();
        }
        Border CreateOperatorGrid(Operators.Operator oper)
        {
            Border border = new Border();
            border.MouseLeftButtonDown += OperatorBorderLeftMouseDown;
            border.Style = Resources["OperatorBorder"] as Style;
            StackPanel sp = new StackPanel();
            border.Child = sp;
            Label l1 = new Label();
            l1.Content = oper.Id;
            l1.Visibility = System.Windows.Visibility.Hidden;
            sp.Children.Add(l1);
            Image img = new Image();
            img.Height = 120;
            img.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Operators\operator.png"));
            sp.Children.Add(img);
            Label l = new Label();
            l.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            l.FontSize = 20;
            l.FontWeight = FontWeights.Bold;
            l.Content = oper.Name;
            sp.Children.Add(l);
            return border;
        }
        void OperatorBorderLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            StackPanel st = b.Child as StackPanel;
            Label l = st.Children[0] as Label;
            int id = Convert.ToInt32(l.Content);
            operatoridforedithim = id;
            ShowOperatorInOperatorsGrid(Operators.Operator.FindById(id));
        }
        void ShowOperatorInOperatorsGrid(Operators.Operator oper)
        {
            StackPanel sp = new StackPanel();
            OperatorInfoInOperatorsGrid.Child = sp;
            Label l1 = new Label();
            l1.Content = oper.Id;
            l1.Visibility = System.Windows.Visibility.Hidden;
            sp.Children.Add(l1);
            Label l = new Label();
            l.Style = Resources["LabelStyle"] as Style;
            l.FontSize = 25;
            l.Content = "Назва";
            l.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            sp.Children.Add(l);
            TextBox tb = new TextBox();
            tb.IsReadOnly = true;
            tb.FontSize = 25;
            tb.Text = oper.Name;
            tb.TextAlignment = TextAlignment.Center;
            tb.BorderThickness = new Thickness(0, 0, 0, 1);
            tb.BorderBrush = Brushes.Gray;
            tb.Margin = new Thickness(10, 0, 10, 0);
            sp.Children.Add(tb);
            l = new Label();
            l.Style = Resources["LabelStyle"] as Style;
            l.FontSize = 25;
            l.Content = "Сайт";
            l.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            sp.Children.Add(l);
            tb = new TextBox();
            tb.IsReadOnly = true;
            tb.FontSize = 25;
            tb.Text = oper.Site;
            tb.TextAlignment = TextAlignment.Center;
            tb.BorderThickness = new Thickness(0, 0, 0, 1);
            tb.BorderBrush = Brushes.Gray;
            tb.Margin = new Thickness(10, 0, 10, 0);
            sp.Children.Add(tb);
            ScrollViewer sv = new ScrollViewer();
            sv.Height = 250;
            sv.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            sp.Children.Add(sv);
            WrapPanel wp = new WrapPanel();
            sv.Content = wp;
            wp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            foreach (var i in oper.GetDocuments())
            {
                Documents.Document d = i;
                wp.Children.Add(CreateDocumentBorder(d));
            }

            Button b1 = new Button();
            b1.Click += EditOperatorInOperatorGrid_Click;
            b1.Style = Resources["ButtonStyle"] as Style;
            b1.FontSize = 20;
            b1.Content = "Редагувати";
            b1.Margin = new Thickness(10, 10, 10, 0);
            sp.Children.Add(b1);
            Button b = new Button();
            b.Click += deleteoperatorinoperatorsgrid_Click;
            b.Style = Resources["ButtonStyle"] as Style;
            b.FontSize = 20;
            b.Content = "Видалити";
            b.Margin = new Thickness(10, 10, 10, 0);
            sp.Children.Add(b);
        }

        void deleteoperatorinoperatorsgrid_Click(object sender, RoutedEventArgs e)
        {
            if (operatoridforedithim != 0)
            {
                Operators.Operator op = Operators.Operator.FindById(operatoridforedithim);
                if (op != null)
                {
                    op.Delete();
                    LoadOperatorGrid();
                }
            }
        }
        private void EditOperatorInOperatorGrid_Click(object sender, RoutedEventArgs e)
        {
            if (operatoridforedithim != 0)
            {
                Operators.Operator op = Operators.Operator.FindById(operatoridforedithim);
                if (op != null)
                {
                    Operators.EditOperatorWindow eo = new Operators.EditOperatorWindow(op);
                    eo.Owner = this;
                    eo.ShowDialog();
                    LoadOperatorGrid();
                }
            }
        }
        #endregion
        //-----------------
        #region Сетка Заявки
        Border userinrequestadd;
        List<Clients.Client> selectedclientsincreaterequest = new List<Clients.Client>();
        void LoadShowRequests()
        {

        }
        void LoadAddRequest()
        {
            AddClientInAddRequestGridImage.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Other\add.png"));
            selectedclientsincreaterequest = new List<Clients.Client>();
            OperatorsComboInCreateRequestGrid.Items.Clear();
            var operators = Operators.Operator.GetAllOperators;
            foreach (var i in operators)
            {
                OperatorsComboInCreateRequestGrid.Items.Add(i.Name);
            }
            ManagersgridinCreateRequestGrid.Children.Clear();
            var users = Users.User.GetAllUsers;
            foreach (var i in users)
            {
                Border grid = AddUserUsersGrid(i);
                grid.MouseLeftButtonDown -= UserBorder_MouseLeftButtonDown;
                grid.MouseLeftButtonDown += gridinaddrequest_MouseLeftButtonDown;
                ManagersgridinCreateRequestGrid.Children.Add(grid);
            }
        }

        void selectclienttolist_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border grid = sender as Border;
            if (grid.Style == (Style)Resources["ClientBorderStyle"])
            {
                grid.Style = (Style)Resources["ClientSelectedBorderStyle"];
                Clients.Client client = Clients.Client.FindById(Convert.ToInt32(((Label)((StackPanel)((Border)sender).Child).Children[0]).Content));
                if (client != null)
                    selectedclientsincreaterequest.Add(client);
                Border b = ClientsInCreateRequestGrid.Children[0] as Border;
                ClientsInCreateRequestGrid.Children.Clear();
                ClientsInCreateRequestGrid.Children.Add(b);
                selectclient = false;
                foreach (var i in selectedclientsincreaterequest)
                {
                    //<Button HorizontalAlignment="Right" VerticalAlignment="Top" Height="30" Width="30" Margin="10"/>

                    Border bor = CreateClientBorder(i);
                    StackPanel sp = ((StackPanel)bor.Child);
                    Button but = ((Button)sp.Children[1]);
                    but.Visibility = System.Windows.Visibility.Visible;
                    but.Height = 30;
                    but.Click += clientclosebtninrequestcreategrid_Click;
                    but.Style = (Style)Resources["ButtonStyle"];
                    ClientsInCreateRequestGrid.Children.Add(bor);
                }
                selectclient = true;
            }
            else
            {
                grid.Style = (Style)Resources["ClientBorderStyle"];
                Clients.Client client = Clients.Client.FindById(Convert.ToInt32(((Label)((StackPanel)((Border)sender).Child).Children[0]).Content));
                for (int i = 0; i < selectedclientsincreaterequest.Count; i++)
                {
                    if (selectedclientsincreaterequest[i].Id == client.Id)
                    {
                        selectedclientsincreaterequest.RemoveAt(i);
                    }
                }
                Border b = ClientsInCreateRequestGrid.Children[0] as Border;
                ClientsInCreateRequestGrid.Children.Clear();
                ClientsInCreateRequestGrid.Children.Add(b);
                selectclient = false;
                foreach (var i in selectedclientsincreaterequest)
                {
                    ClientsInCreateRequestGrid.Children.Add(CreateClientBorder(i));
                }
                selectclient = true;
            }
        }

        void clientclosebtninrequestcreategrid_Click(object sender, RoutedEventArgs e)
        {
            StackPanel sp = (StackPanel)((Button)sender).Parent;
            int id = Convert.ToInt32(((Label)sp.Children[0]).Content);
            for (int i = 0; i < selectedclientsincreaterequest.Count; i++)
            {
                if (selectedclientsincreaterequest[i].Id == id)
                {
                    selectedclientsincreaterequest.RemoveAt(i);
                }
            }
            Border b = ClientsInCreateRequestGrid.Children[0] as Border;
            ClientsInCreateRequestGrid.Children.Clear();
            ClientsInCreateRequestGrid.Children.Add(b);
            foreach (var i in selectedclientsincreaterequest)
            {
                Border bor = CreateClientBorder(i);
                StackPanel sp1 = ((StackPanel)bor.Child);
                Button but = ((Button)sp1.Children[1]);
                but.Visibility = System.Windows.Visibility.Visible;
                but.Height = 30;
                but.Click += clientclosebtninrequestcreategrid_Click;
                but.Style = (Style)Resources["ButtonStyle"];
                ClientsInCreateRequestGrid.Children.Add(bor);
            }
        }
        void gridinaddrequest_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (userinrequestadd != null)
            {
                userinrequestadd.Style = Resources["ClientBorderStyle"] as Style;
            }
            Border b = (Border)sender;
            b.Style = Resources["ClientSelectedBorderStyle"] as Style;
            userinrequestadd = b;
        }
        private void AddClientInAddRequestGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectclient = true;
            TurnGridNext(FindClient);
        }
        private void CreateRequestLabelInShowRequests_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadAddRequest();
            TurnGridNext(RequestsCreateGrid);
        }
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
            CleareAllCheckes();
            CheckToLastFiveClients = true;
        }
        private void UsersButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadUsersGrid();
            TurnGridMain(UsersGrid);
            CleareAllCheckes();
        }
        private void OperatorsButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadOperatorGrid();
            TurnGridMain(OperatorsGrid);
            CleareAllCheckes();
        }
        private void RequestsButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadShowRequests();
            TurnGridMain(RequestsGrid);
            CleareAllCheckes();
        }
        #endregion
        //-----------------
        #region Проверки в фоновом потоке
        void CheckLastFiveAddedClients()
        {
            List<int> ides = new List<int>();
            foreach (var i in FiveLastClientsGridClientGrid.Children)
            {
                Border b = i as Border;
                StackPanel sp = b.Child as StackPanel;
                int id = Convert.ToInt32(((Label)sp.Children[0]).Content);
                ides.Add(id);
            }
            List<Clients.Client> clients = Clients.Client.GetFiveLastClients();
            if (clients.Count != ides.Count)
            {
                LoadFiveLastClients();
            }
            else
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i].Id != ides[i])
                    {
                        LoadFiveLastClients();
                    }
                }
            }
        }
        #endregion















    }
}
