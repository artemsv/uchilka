using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
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

namespace Uchilka
{
    public partial class MainWindow : Window
    {
        private readonly List<Image> Images;

        public MainWindow()
        {
            InitializeComponent();

            Images = new List<Image> { Image1, Image2, Image3, Image4, Image5,
                Image6, Image7, Image8, Image9, Image10};
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadImages();   
        }

        private void LoadImages()
        {
            var dataPath = ConfigurationManager.AppSettings["DataPath"];

            var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), dataPath);

            var di = new DirectoryInfo(path);

            if (di.Exists)
            {
                var dirs = di.EnumerateDirectories();

                var animals = dirs.FirstOrDefault(x => x.Name == "Животные");

                if (animals != null)
                {
                    var files = animals.EnumerateFiles().ToList();

                    var min = Math.Min(Images.Count, files.Count);

                    for(var k = 0; k < min; k++)
                    {
                        Images[k].Source = new BitmapImage(new Uri(files[k].FullName));
                    }
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Border).BorderThickness = new Thickness(2);
            (sender as Border).CaptureMouse();
            (sender as Border).Tag = true;
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            (sender as Border).BorderThickness = new Thickness(1);
            (sender as Border).ReleaseMouseCapture();

            if ((sender as Border).Tag is true)
            {
                Debug.WriteLine("CLICK!");
            }
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if ((sender as Border).IsMouseCaptured)
            {
                bool isInside = false;

                VisualTreeHelper.HitTest(
                    sender as Border,
                    d =>
                    {
                        if (d == sender as Border)
                        {
                            isInside = true;
                        }

                        return HitTestFilterBehavior.Stop;
                    },
                    ht => HitTestResultBehavior.Stop,
                    new PointHitTestParameters(e.GetPosition(sender as Border)));

                (sender as Border).Tag = isInside;
            }
        }
    }
}
