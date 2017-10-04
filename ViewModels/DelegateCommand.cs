using System;
using System.Windows.Input;

namespace Uchilka.ViewModels
{
    public class DelegateCommand : ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action<object> _execute;

        public DelegateCommand(Action execute)
            : this((o) => execute(), null)
        {
        }
        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
            : this((o) => execute(), canExecute)
        {
        }

        public DelegateCommand(Action<object> execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            return _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute(parameter);

            RaiseCanExecuteChanged();
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
