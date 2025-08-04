using System;
using System.Threading;
using System.Windows.Input;

namespace SimpleMvvm.Command
{
    /// <summary>
    /// Defines delegate command.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        bool ICommand.CanExecute(object parameter)
        {
            return GetCanExecute(parameter);
        }

        /// <inheritdoc/>
        void ICommand.Execute(object parameter)
        {
            Execute?.Invoke(parameter);
        }

        private int _canExecuteFlag = 1;
        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// This property has a higher priority than the <see cref="CanExecuteFunc"/> delegate.
        /// </summary>
        public bool CanExecute
        {
            get => _canExecuteFlag != 0;
            set
            {
                int newval = value ? 1 : 0;
                int orival = Interlocked.Exchange(ref _canExecuteFlag, newval);
                if (orival != newval) RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Delegate to determine whether the command can execute. 
        /// </summary>
        public Func<object, bool> CanExecuteFunc { get; set; }

        /// <summary>
        /// Gets the value that indicates whether the command can execute in its current state.
        /// </summary>
        protected virtual bool GetCanExecute(object parameter)
        {
            return CanExecute && (CanExecuteFunc?.Invoke(parameter) ?? true);
        }

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        public virtual Action<object> Execute { get; set; }

        /// <summary>
        /// Instantiate a DelegateCommand.
        /// </summary>
        public DelegateCommand()
        {
        }

        /// <summary>
        /// Instantiate a DelegateCommand with Execute Action.
        /// </summary>
        public DelegateCommand(Action<object> executeAction)
        {
            Execute = executeAction;
        }

        /// <summary>
        /// Instantiate a DelegateCommand with Execute Action.
        /// </summary>
        public DelegateCommand(Action executeAction) : this(_ => executeAction())
        {
        }
    }
}
