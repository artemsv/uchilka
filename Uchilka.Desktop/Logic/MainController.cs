using System.Configuration;
using Uchilka.Multimedia;
using Uchilka.ViewModels;

namespace Uchilka.Logic
{
    internal class MainController
    {
        private RunMode _mode;
        private MainWindow _mainWindow;
        private readonly DataController _dataController;
        private MainViewModel _mainModel;
        private readonly AnswerChecker _answerChecker;

        public MainController()
        {
            _dataController = new DataController(ConfigurationManager.AppSettings["DataPath"]);
            _answerChecker = new AnswerChecker();

            _mode = RunMode.SelectName;
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
            _mainModel = new MainViewModel(new MultimediaFactory());
            _mainModel.UserAnswered += MainModel_UserAnswered;
            _mainModel.Started += MainModel_Started;
            _mainModel.Cancelled += MainModel_Cancelled;

            _mainWindow = new MainWindow(_mainModel);
            _mainWindow.Show();

            _mainModel.InitialPosition();
        }

        private void MainModel_Cancelled(object sender)
        {
            if (_mode == RunMode.SelectTest)
            {
                _mainModel.InitialPosition();
                _mode = RunMode.SelectName;
            }else if (_mode == RunMode.Working)
            {
                _mainModel.ReadyToSelectTest(_dataController.GetCatalog());
                _mode = RunMode.SelectTest;
            }
        }

        private void MainModel_Started(object sender, string name)
        {
            if (_mode == RunMode.SelectName)
            {
                _mainModel.ReadyToSelectTest(_dataController.GetCatalog());
                _mode = RunMode.SelectTest;
            }else if (_mode == RunMode.SelectTest)
            {
                _mainModel.StartTest();
                _mode = RunMode.Working;
                _mainWindow.WindowState = System.Windows.WindowState.Maximized;
            }
        }

        private void MainModel_UserAnswered(int fieldId)
        {
            if (_answerChecker.Accepted(fieldId))
            {

            }
        }
    }
}
