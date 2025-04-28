using System;
using System.Windows.Input;

namespace SimpleMvvm.Command
{
    /// <summary>
    /// Defines delegate command.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute;
        }

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
                _canExecute = value;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
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
