using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Uchilka.Integration.Abstractions;
using Uchilka.Logic;
using Uchilka.Multimedia;

namespace Uchilka.ViewModels
{
    public class MainViewModel : ICommChannelCommandHandler, INotifyPropertyChanged
    {
        private Visibility _borderImageVisibility;
        private readonly IPlayer _player;

        public MainViewModel(IMultimediaFactory mmFactory)
        {
            ControlViewModel = new ControlViewModel();

            ControlViewModel.Started += ControlViewModel_Started;
            ControlViewModel.Cancelled += ControlViewModel_Cancelled;

            _player = mmFactory.GetPlayer();
        }

        public void InitialPosition()
        {
            ControlViewModel.ChoiceListBoxVisibility = System.Windows.Visibility.Visible;
            ControlViewModel.ChoiceItems = new List<string> { "Руслан", "Аделя" };
            ControlViewModel.MarksPanelVisibility = System.Windows.Visibility.Hidden;
            ControlViewModel.StartButtonVisibility = Visibility.Visible;
            ControlViewModel.CancelButtonVisibility = Visibility.Collapsed;
            ControlViewModel.StartButtonCaption = "Выбор";
            ControlViewModel.CancelButtonCaption = "Назад";

            BorderImageVisibility = Visibility.Hidden;
        }

        internal void ReadyToSelectTest(IEnumerable<string> tests)
        {
            ControlViewModel.ChoiceListBoxVisibility = Visibility.Visible;
            ControlViewModel.MarksPanelVisibility = Visibility.Hidden;
            ControlViewModel.StartButtonVisibility = Visibility.Visible;

            BorderImageVisibility = Visibility.Hidden;

            ControlViewModel.ChoiceItems = tests;
            ControlViewModel.StartButtonCaption = "Старт";
            ControlViewModel.CancelButtonVisibility = Visibility.Visible;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event UserAnsweredEventHandler UserAnswered;
        public event StartedEventHandler Started;
        public event CancelledEventHandler Cancelled;
        public ControlViewModel ControlViewModel { get; set; }

        public Visibility BorderImageVisibility
        {
            get
            {
                return _borderImageVisibility;
            }
            set
            {
                _borderImageVisibility = value;
                OnPropertyChanged();
            }
        }

        internal void StartTest()
        {
            ReadyToTest(ControlViewModel);
        }

        #region ICommChannelCommandHandler

        public void HandleCommand(CommChannelCommandType cmd)
        {
        }

        public void HandleVoice(string path)
        {
            _player.Play(path);
        }

        public void HandlePhoto(string path)
        {
        }

        #endregion

        #region Private

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal void Click(string id)
        {
        }

        private void ReadyToTest(ControlViewModel controlView)
        {
            controlView.ChoiceListBoxVisibility = Visibility.Hidden;
            controlView.MarksPanelVisibility = Visibility.Visible;
            controlView.StartButtonVisibility = Visibility.Hidden;

            BorderImageVisibility = Visibility.Visible;
            
        }

        private void ControlViewModel_Started(object sender, string name)
        {
            //var controlView = sender as ControlViewModel;
            //ReadyToTest(controlView);
            
            Started(this, name);
        }


        private void ControlViewModel_Cancelled(object sender, string name)
        {
            Cancelled(this);
        }

        #endregion
    }
}
