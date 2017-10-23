using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Uchilka.Presentation;

namespace Uchilka.ViewModels
{
    public class ControlViewModel : INotifyPropertyChanged
    {
        private Visibility _choiceListBoxVisibility;
        private IEnumerable<string> _choiceItems;
        private Visibility _marksPanelVisibility;
        private Visibility _doNextStepButtonVisibility;
        private Visibility _cancelButtonVisibility;
        private string _doNextStepButtonCaption;
        private string _cancelButtonCaption;
        private readonly DelegateCommand _doNextStepCommand;
        private readonly DelegateCommand _cancelCommand;
        private int _choiceListIndex;

        private int _correctAnswerCount;
        private int _wrongAnswerCount;

        public ControlViewModel()
        {
            _choiceListIndex = -1;
            _doNextStepCommand = new DelegateCommand(HandleNextStep, CanExecuteNextStep);
            _cancelCommand = new DelegateCommand(HandleCancel);
        }

        public Visibility ChoiceListBoxVisibility
        {
            get
            {
                return _choiceListBoxVisibility;
            }
            set
            {
                _choiceListBoxVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility DoNextStepButtonVisibility
        {
            get
            {
                return _doNextStepButtonVisibility;
            }
            set
            {
                _doNextStepButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility CancelButtonVisibility
        {
            get
            {
                return _cancelButtonVisibility;
            }
            set
            {
                _cancelButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<string> ChoiceItems
        {
            get
            {
                return _choiceItems;
            }
            set
            {
                _choiceItems = value;
                OnPropertyChanged();
            }
        }

        public Visibility MarksPanelVisibility
        {
            get
            {
                return _marksPanelVisibility;
            }
            set
            {
                _marksPanelVisibility = value;
                OnPropertyChanged();
            }
        }

        public int ChoiceIndex
        {
            get
            {
                return _choiceListIndex;
            }
            set
            {
                _choiceListIndex = value;
                OnPropertyChanged();

                DoNextStepCommand.RaiseCanExecuteChanged();
            }
        }

        public string ChoiceItem
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event DoNextStepEventHandler DoNextStep;
        public event CancelEventHandler Cancelled;

        public DelegateCommand DoNextStepCommand => _doNextStepCommand;
        public DelegateCommand CancelCommand => _cancelCommand;

        public string DoNextStepButtonCaption
        {
            get
            {
                return _doNextStepButtonCaption;
            }
            set
            {
                _doNextStepButtonCaption = value;
                OnPropertyChanged();
            }
        }

        public string CancelButtonCaption
        {
            get
            {
                return _cancelButtonCaption;
            }
            set
            {
                _cancelButtonCaption = value;
                OnPropertyChanged();
            }
        }

        public int CorrectAnswerCount
        {
            get
            {
                return _correctAnswerCount;
            }
            set
            {
                _correctAnswerCount = value;
                OnPropertyChanged();
            }
        }

        public int WrongAnswerCount
        {
            get
            {
                return _wrongAnswerCount;
            }
            set
            {
                _wrongAnswerCount = value;
                OnPropertyChanged();
            }
        }

        #region Private

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private async void HandleNextStep()
        {
            if (_choiceListIndex == -1)
            {
                await InfoBox.ShowMessageAsync("Пожалуйста, выберите имя...", "");
            }
            else
            {
                DoNextStep(this, ChoiceItem);
            }
        }

        private void HandleCancel()
        {
            Cancelled(this, ChoiceItem);
        }

        private bool CanExecuteNextStep()
        {
            return _choiceListIndex > -1;
        }

        #endregion
    }
}
