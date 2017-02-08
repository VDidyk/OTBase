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

namespace OTBaseNew.Other
{
    /// <summary>
    /// Interaction logic for AddList.xaml
    /// </summary>
    public partial class AddList : Window
    {
        string header = "";
        List<string> values;
        public AddList(string header, List<string> values)
        {
            InitializeComponent();
            this.header = header;
            this.values = values;
            Head.Content = header;
        }

        private void AddPhoneFieldAddClients_Click(object sender, RoutedEventArgs e)
        {
            TextBox t = new TextBox();
            t.Style = Resources["TexBoxStyle"] as Style;
            t.Margin = new Thickness(10, 0, 10, 10);
            Button b = new Button();
            b.Content = "+";
            b.Style = Resources["ButtonStyle"] as Style;
            b.Margin = new Thickness(5, 0, 5, 10);
            b.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            b.FontSize = 13;
            b.Click += AddPhoneFieldAddClients_Click;
            AddClientTelephonsTextboxStack.Children.Add(t);
            AddClientTelephonsBtnStack.Children.Add(b);
            ((Button)sender).Visibility = System.Windows.Visibility.Hidden;
            AddPhonesAddClientScroll.ScrollToBottom();
        }
    }
}
