using System;
using System.Threading;

namespace SimpleMvvm.Command
{
    /// <summary>
    /// Defines delegate command.
    /// </summary>
    public class DelegateCommand : AbstractCommand
    {
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

        /// <inheritdoc/>
        protected override bool GetCanExecute(object parameter)
        {
            return CanExecute && (CanExecuteFunc?.Invoke(parameter) ?? true);
        }

        /// <inheritdoc/>
        protected override void InvokeExecute(object parameter)
        {
            Execute?.Invoke(parameter);
        }
    }
}
