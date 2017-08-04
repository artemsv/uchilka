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
        private MainViewModel _mainModel;
        private readonly AnswerChecker _answerChecker;

        public MainController()
        {
            _dataController = new DataController(ConfigurationManager.AppSettings["DataPath"]);
            _answerChecker = new AnswerChecker();
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
            _mainModel = new MainViewModel();
            _mainModel.UserAnswered += MainModel_UserAnswered;
            _mainModel.Started += MainModel_Started;

            _mainWindow = new MainWindow(_mainModel);
            _mainWindow.Show();
        }

        private void MainModel_Started(object sender, string name)
        {
        }

        private void MainModel_UserAnswered(int fieldId)
        {
            if (_answerChecker.Accepted(fieldId))
            {

            }
        }
    }
}
