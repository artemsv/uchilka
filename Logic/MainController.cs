using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uchilka.Logic
{
    internal class MainController
    {
        private MainWindow _mainWindow;
        private readonly DataController _dataController;

        public MainController()
        {
            _dataController = new DataController(ConfigurationManager.AppSettings["DataPath"]);
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
        }
    }
}
