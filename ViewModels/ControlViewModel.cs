using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using Uchilka.Presentation;

namespace Uchilka.ViewModels
{
    public class ControlViewModel : INotifyPropertyChanged
    {
        private Visibility _choiceListBoxVisibility;
        private IEnumerable<string> _choiceItems;
        private Visibility _marksPanelVisibility;
        private Visibility _startButtonVisibility;
        private string _startButtonCaption;
        private readonly DelegateCommand _startCommand;
        private int _choiceIndex;

        private int _correctAnswerCount;
        private int _wrongAnswerCount;

        public ControlViewModel()
        {
            _choiceIndex = -1;
            _startCommand = new DelegateCommand(HandleStart);
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

        public Visibility StartButtonVisibility
        {
            get
            {
                return _startButtonVisibility;
            }
            set
            {
                _startButtonVisibility = value;
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
                return _choiceIndex;
            }
            set
            {
                _choiceIndex = value;
                OnPropertyChanged();
            }
        }

        public string ChoiceItem
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event StartedEventHandler Started;

        public DelegateCommand StartCommand => _startCommand;

        public string StartButtonCaption
        {
            get
            {
                return _startButtonCaption;
            }
            set
            {
                _startButtonCaption = value;
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

        private async void HandleStart()
        {
            if (_choiceIndex == -1)
            {
                await InfoBox.ShowMessageAsync("Пожалуйста, выберите имя...", "");
            }
            else
            {
                Started(this, ChoiceItem);
            }
        }

        #endregion
    }
}
