using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetupXamarin.Core.Commanding
{
    public class Command : ICommand
    {
        private readonly Action action;

        public Command (Action actionParam)
        {
            action = actionParam;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => action != null;

        public void Execute(object parameter) => action.Invoke();
    }

    public class Command<T> : ICommand
    {
        private readonly Action<T> action;

        public Command (Action<T> actionParam)
        {
            action = actionParam;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => action != null;

        public void Execute(object parameter) => action.Invoke((T)parameter);

    }


}
