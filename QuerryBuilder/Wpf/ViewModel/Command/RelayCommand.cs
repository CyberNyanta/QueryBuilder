using System;
using System.Windows.Input;

namespace Wpf.ViewModel.Command
{


    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            if (canExecute == null)
                _canExecute = () => true;
            else
                _canExecute = canExecute;
        }

        #region Члены ICommand

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }
        #endregion
        //class RelayCommand : ICommand
        //{
        //    public RelayCommand(Action<object> action)
        //    {
        //        ExecuteDelegate = action;
        //    }
        //    public Predicate<object> CanExecuteDelegate { get; set; }
        //    public Action<object> ExecuteDelegate { get; set; }

        //    public bool CanExecute(object parameter)
        //    {
        //        return CanExecuteDelegate == null || CanExecuteDelegate(parameter);
        //    }

        //    public event EventHandler CanExecuteChanged
        //    {
        //        add { CommandManager.RequerySuggested += value; }
        //        remove { CommandManager.RequerySuggested -= value; }
        //    }
        //    public void Execute(object parameter)
        //    {
        //        ExecuteDelegate?.Invoke(parameter);
        //    }
        //}
    }
}
