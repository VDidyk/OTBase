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
using System.Threading;
using WpfAnimatedGif;
namespace OTBaseNew.Other
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : Window
    {
        public static bool loaded = false;
        int rotate = 0;
        public Loading()
        {
            InitializeComponent();
            //img.Source = new BitmapImage(new Uri(MainWindow.Exepath + @"\Data\Images\Other\wait.png"));
            //  DispatcherTimer setup
           var  dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(10000);
            dispatcherTimer.Start();
            //img.RenderTransformOrigin = new Point(1, 1);

            
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
          
            rotate+=5;
            RotateTransform rotateTransform = new RotateTransform(rotate);
            can.RenderTransform = rotateTransform;
            //img.RenderTransform = rotateTransform;
        }
        public static Window Load(Window window)
        {
            Loading wi = null;
            wi = new Loading();
            wi.Owner = window;
            //wi.Show();
            return wi;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        


    }
}
