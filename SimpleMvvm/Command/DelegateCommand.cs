using System;
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
            return GetCanExecute();
        }

        /// <inheritdoc/>
        void ICommand.Execute(object parameter)
        {
            Execute?.Invoke(parameter);
        }

        private bool _canExecute = true;
        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        public bool CanExecute
        {
            get => _canExecute;
            set
            {
                if (_canExecute != value)
                {
                    _canExecute = value;
                    RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Gets the value of CanExecute.
        /// </summary>
        protected virtual bool GetCanExecute()
        {
            return CanExecute;
        }

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        protected void RaiseCanExecuteChanged()
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
        public DelegateCommand(Action executeAction) : this(o => executeAction())
        {
        }
    }
}
