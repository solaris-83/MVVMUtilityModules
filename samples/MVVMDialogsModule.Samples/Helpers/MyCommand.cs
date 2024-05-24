using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMDialogsModule.Samples.Helpers
{
    public class MyCommand : ICommand
    {
        Action<object> _execute;
        Func<object, bool> _canExecute;
        private Action ok;

        public MyCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #region ICommand Members  

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute(parameter);
            }
            else
            {
                return false;
            }
        }
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        #endregion
    }
}
