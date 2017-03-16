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
    public partial class ChangePosition : Window
    {
        User u;
        public ChangePosition(User u)
        {
            InitializeComponent();
            this.u = u;
            var pos = Positions.Position.GetAllPositions;
            for(int i=0;i<pos.Count;i++)
            {
                posit.Items.Add(pos[i].Name);
                if(u.Position.Id==pos[i].Id)
                {
                    posit.SelectedIndex = i;
                }
            }
            
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if(posit.SelectedIndex!=-1)
            {
                Positions.Position pos = Positions.Position.FindByName(posit.SelectedItem.ToString());
                u.Position = pos;
                u.Save();
                this.Close();
            }
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
