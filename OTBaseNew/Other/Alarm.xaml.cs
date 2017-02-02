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
namespace OTBaseNew
{
    /// <summary>
    /// Interaction logic for SystemMessage.xaml
    /// </summary>
    public partial class Alarm : Window
    {
        double top;
        double left;
        string pathsong = "";
        bool fix = false;
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        int seconds = 0;

        public Alarm(string message, string pathsong)
        {
            InitializeComponent();
            this.pathsong = pathsong;
            var primaryMonitorArea = SystemParameters.WorkArea;
            left = primaryMonitorArea.Right - Width - 10;
            top = primaryMonitorArea.Bottom - Height + 300;

            Text.Text = message;
            this.Show();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(200);
            playSound();
            timer.Start();

        }
        private void timerTick(object sender, EventArgs e)
        {
            if(seconds>500)
            {
                timer.Stop();
                this.Close();
            }
            if (!fix)
            {
                var primaryMonitorArea = SystemParameters.WorkArea;
                if (top == primaryMonitorArea.Bottom - Height)
                {
                    fix = true;
                }
                top -= 10;
                Top = top;
                Left = left;
                this.Opacity = 1;
            }
            seconds++;
        }
        private void playSound()
        {
            try
            {
                //Отримання директорії з програмою
                System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(pathsong+@"\Data\Sounds\Message.wav");
                simpleSound.Play();
            }
            catch
            { }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
