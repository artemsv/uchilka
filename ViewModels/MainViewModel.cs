using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Uchilka.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            ControlViewModel = new ControlViewModel
            {
                ChoiceListBoxVisibility = System.Windows.Visibility.Visible,
                ChoiceItems = new List<string> { "Руслан", "Аделя" },
                MarksPanelVisibility = System.Windows.Visibility.Collapsed
            };

            BorderImageVisibility = Visibility.Hidden;
        }


        public ControlViewModel ControlViewModel { get; set; }
        public Visibility BorderImageVisibility { get; set; }
    }
}
