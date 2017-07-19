using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Uchilka.ViewModels
{
    public class ControlViewModel
    {
        public Visibility ChoiceListBoxVisibility { get; set; }

        public IEnumerable<string> ChoiceItems { get; set; }
    }
}
