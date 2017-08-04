using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Uchilka.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Visibility _borderImageVisibility;

        public MainViewModel()
        {
            ControlViewModel = new ControlViewModel
            {
                ChoiceListBoxVisibility = System.Windows.Visibility.Visible,
                ChoiceItems = new List<string> { "Руслан", "Аделя" },
                MarksPanelVisibility = System.Windows.Visibility.Collapsed,
                StartButtonVisibility = Visibility.Visible
            };

            ControlViewModel.Started += ControlViewModel_Started;

            BorderImageVisibility = Visibility.Hidden;
        }

        private void ControlViewModel_Started(object sender, string name)
        {
            var controlView = sender as ControlViewModel;
            controlView.ChoiceListBoxVisibility = Visibility.Hidden;
            controlView.MarksPanelVisibility = Visibility.Visible;
            controlView.StartButtonVisibility = Visibility.Hidden;

            BorderImageVisibility = Visibility.Visible;

            Started(this, name);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event UserAnsweredEventHandler UserAnswered;
        public event StartedEventHandler Started;
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

        #endregion
    }
}
