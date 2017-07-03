using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FrameworkAg
{
    public class MyDelegateCommand<T> : ICommand
    {
        public MyDelegateCommand(Action<T> executeAction, Func<T, bool> canExecuteAction)
        {
            this.executeAction = executeAction;
            this.canExecuteAction = canExecuteAction;
        }
        public MyDelegateCommand(Action<T> executeAction)
            : this(executeAction, null)
        {
        }
        public bool CanExecute(object parameter)
        {
            if (parameter != null) // TODO: Resolve this later - JHE
                return (canExecuteAction((T)parameter));
            else
                return canExecuteAction(default(T));
        }
        public event EventHandler CanExecuteChanged;
        protected virtual void OnCanExecuteChanged(object sender, EventArgs e)
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, e);
        }

        public void NotifyCanExecuteChanged()
        {
            OnCanExecuteChanged(null, null);
        }

        public void Execute(object parameter)
        {
            if (parameter != null) // TODO: Resolve this later - JHE
                executeAction((T)parameter);
            else
                executeAction(default(T));
        }
        Action<T> executeAction;
        Func<T, bool> canExecuteAction;
    }
}
