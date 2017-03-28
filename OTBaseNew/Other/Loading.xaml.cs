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
        public Loading()
        {
            InitializeComponent();
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(MainWindow.Exepath + @"\Data\Images\Other\wait.png");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(img, image);
        }

        public static Window Load(Window window)
        {
            Thread t = new Thread(ThreadProc);


            t.SetApartmentState(ApartmentState.STA);


            bool result = t.TrySetApartmentState(ApartmentState.MTA);

            t.Start();

            Thread.Sleep(500);
            Loading wi = null;
            try
            {
                wi = new Loading();
                wi.Owner = window;
                wi.Show();
                return wi;
            }
            catch (ThreadStateException)
            {
                return wi;
            }

            t.Join();

        }
        public static void ThreadProc()
        {
            Thread.Sleep(2000);
        }
    }
}
