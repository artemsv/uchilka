using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Uchilka.ViewModels;

namespace Uchilka
{
    public partial class MainWindow 
    {
        private readonly List<Image> Images;
        private MainViewModel mainViewModel;

        public MainWindow()
        {
            InitializeComponent();

            Images = new List<Image> { Image1, Image2, Image3, Image4, Image5,
                Image6, Image7, Image8, Image9, Image10};
        }

        public MainWindow(MainViewModel mainViewModel)
            : this()
        {
            this.mainViewModel = mainViewModel;
            DataContext = mainViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //var bot = new TelegramBot(mainViewModel);
            //var me = bot.TestApiAsync();

            //Picture.Source = new BitmapImage()
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
                mainViewModel.Click(((sender as Border).Child as Image).Name.Replace("Image", string.Empty));
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
