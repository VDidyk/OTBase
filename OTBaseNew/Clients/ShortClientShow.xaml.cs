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

namespace OTBaseNew.Clients
{
    /// <summary>
    /// Interaction logic for ShortClientShow.xaml
    /// </summary>
    public partial class ShortClientShow : Window
    {
        Clients.Client client;
        public ShortClientShow(Clients.Client client)
        {
            InitializeComponent();
            this.client = client;
            MainInfo.Text = client.FName + " " + client.LName + " " + client.MName;
            if (client.Bday.Year != 1)
            {
                BDay.Text = client.Bday.ToShortDateString();
            }
            else
            {
                BDay.Text = "Не вказано";
            }
            if(client.GetAddress!=null)
            {
                if(client.GetAddress.GetCity!=null)
                {
                    if(client.GetAddress.GetCity.GetRegion!=null)
                    {
                        Regionandcity.Text = "Область: " + client.GetAddress.GetCity.GetRegion.Name + ". Місто: " + client.GetAddress.GetCity.Name;
                        Address.Text = client.GetAddress.address;
                    }
                    else
                    {
                        Regionandcity.Text = "Місто: " + client.GetAddress.GetCity.Name;
                        Address.Text = client.GetAddress.address;
                    }
                }
                else
                {
                    Address.Text = client.GetAddress.address;
                }
            }
            if(client.GetResourse!=null)
            {
                Resourse.Text = "Ресурс: "+client.GetResourse.Name;
            }
            else
            {
                Resourse.Text = "Ресурс: " + "Не вказано";
            }
            Manager.Text="Ведущий менеджер: "+client.GetWorkingdUser.FName+" "+client.GetWorkingdUser.LName;
            foreach(var i in client.GetPhones)
            {
                Phones.Items.Add(i.number);
            }
            foreach (var i in client.GetEmails)
            {
                Emails.Items.Add(i.name);
            }
            if(client.GetPassport!=null)
            {
                FIP.Text=client.GetPassport.Fname+" "+client.GetPassport.Lname;
                Series.Text = client.GetPassport.series;
                GivenBy.Text = client.GetPassport.given_by;
                if (client.GetPassport.given_when.Year != 1)
                {
                    GivenWhen.Text = client.GetPassport.given_when.ToShortDateString();
                }
                else
                {
                    GivenWhen.Text = "Не вказано";
                }
                if (client.GetPassport.given_the_time.Year != 1)
                {
                    GivenWhen_Copy.Text = client.GetPassport.given_the_time.ToShortDateString();
                }
                else
                {
                    GivenWhen_Copy.Text = "Не вказано";
                }
            }
            else
            {
                FIP.Text = "Не вказано";
                Series.Text = "Не вказано";
                GivenBy.Text = "Не вказано";
                GivenWhen.Text = "Не вказано";
                GivenWhen_Copy.Text = "Не вказано";
            }
        }

        private void SaveAddClient_Click(object sender, RoutedEventArgs e)
        {
            client.Save();
            this.Close();
        }

        private void CancelAddClient_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
