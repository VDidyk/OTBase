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
    /// Interaction logic for ChooseClients.xaml
    /// </summary>
    public partial class ChooseClients : Window
    {
        MainWindow mw;
        List<Clients.Client> client;
        public ChooseClients(MainWindow mw, List<Clients.Client> client)
        {
            InitializeComponent();
           
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
