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
using Microsoft.Win32;
namespace OTBaseNew.Operators
{
    /// <summary>
    /// Interaction logic for EditOperatorWindow.xaml
    /// </summary>
    public partial class EditOperatorWindow : Window
    {
        Operators.Operator operat;
        public EditOperatorWindow(Operators.Operator operat)
        {
            InitializeComponent();
            this.operat = operat;
            Name.Text = operat.Name;
            Site.Text = operat.Site;
            var documents = operat.GetDocuments();
            DocumentsPanel.Children.Clear();
            foreach (var i in documents)
            {
                DocumentsPanel.Children.Add(CreateDocumentBorder(i));
            }
        }
        Border CreateDocumentBorder(Documents.Document doc)
        {
            Border b = new Border();
            b.MouseLeftButtonDown += b_MouseLeftButtonDown;
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
            t.ToolTip = doc.Name + doc.Extension;
            t.TextWrapping = TextWrapping.Wrap;
            sp.Children.Add(t);
            return b;
        }

        void b_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                string path = ofd.FileName;
                Documents.Document d = new Documents.Document() { Name = System.IO.Path.GetFileNameWithoutExtension(path), Extension = System.IO.Path.GetExtension(path) };
                d.Operator_id = operat.Id;
                d.Save();
                Border b = CreateDocumentBorder(d);
                DocumentsPanel.Children.Add(b);
                MainWindow.Message("Файл успішно додано!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var i in DocumentsPanel.Children)
            {
                Border b = i as Border;
                if (b.Style == (Style)Resources["SelectedDocumentBorder"])
                {
                    StackPanel sp = b.Child as StackPanel;
                    Label l = sp.Children[0] as Label;
                    int id = Convert.ToInt32(l.Content);
                    Documents.Document.FindById(id).Delete();
                }
            }
            DocumentsPanel.Children.Clear();
            var documents = operat.GetDocuments();
            foreach (var i in documents)
            {
                DocumentsPanel.Children.Add(CreateDocumentBorder(i));
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            operat.Name = Name.Text;
            operat.Site = Site.Text;
            operat.Save();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NameOfDock.Visibility = System.Windows.Visibility.Visible;
            SaveName.Visibility = System.Windows.Visibility.Visible;
        }

        private void SaveName_Click(object sender, RoutedEventArgs e)
        {
            if (NameOfDock.Text != "")
            {
                foreach (var i in DocumentsPanel.Children)
                {
                    Border b = i as Border;
                    if (b.Style == (Style)Resources["SelectedDocumentBorder"])
                    {
                        StackPanel sp = b.Child as StackPanel;
                        Label l = sp.Children[0] as Label;
                        int id = Convert.ToInt32(l.Content);
                        Documents.Document doc = Documents.Document.FindById(id);
                        doc.Name = NameOfDock.Text;
                        doc.Save();
                        break;
                    }
                }
                DocumentsPanel.Children.Clear();
                var documents = operat.GetDocuments();
                foreach (var i in documents)
                {
                    DocumentsPanel.Children.Add(CreateDocumentBorder(i));
                }
                NameOfDock.Text = "";
                NameOfDock.Visibility = System.Windows.Visibility.Hidden;
                SaveName.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
