using System.Windows;
using Uchilka.Logic;

namespace Uchilka
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            new MainController().Run();
        }
    }
}
