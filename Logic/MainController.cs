using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uchilka.ViewModels;

namespace Uchilka.Logic
{
    internal class MainController
    {
        private MainWindow _mainWindow;
        private readonly DataController _dataController;

        public MainController()
        {
            _dataController = new DataController(ConfigurationManager.AppSettings["DataPath"]);

            ControlViewModel = new ControlViewModel
            {
                ChoiceListBoxVisibility = System.Windows.Visibility.Visible,
                ChoiceItems = new List<string> { "Ruslan", "Adelya" }
            };
        }

        public void Run()
        {
            CreateUI();
            ShowStartChoice();
        }

        private void ShowStartChoice()
        {
            var catalogItems = _dataController.GetCatalog();
        }

        private void CreateUI()
        {
            _mainWindow = new MainWindow();
            _mainWindow.Show();
            _mainWindow.DataContext = this;
        }

        public ControlViewModel ControlViewModel { get; set; }
    }
}
