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
        private readonly DelegateCommand _startCommand;
        private int _selectedChoiceIndex;

        public ControlViewModel()
        {
            _selectedChoiceIndex = -1;
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

        public int SelectedChoiceIndex
        {
            get
            {
                return _selectedChoiceIndex;
            }
            set
            {
                _selectedChoiceIndex = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public DelegateCommand StartCommand => _startCommand;

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
            if (_selectedChoiceIndex == -1)
            {
                await InfoBox.ShowMessageAsync("Пожалуйста, выберите имя...", "");
            }
        }

        #endregion
    }
}
