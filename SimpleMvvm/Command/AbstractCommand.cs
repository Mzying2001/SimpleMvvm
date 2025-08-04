using System;
using System.Windows.Input;

namespace SimpleMvvm.Command
{
    /// <summary>
    /// Abstract base class for commands that implements the <see cref="ICommand"/> interface.
    /// </summary>
    public abstract class AbstractCommand : ICommand
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
            InvokeExecute(parameter);
        }

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the value that indicates whether the command can execute in its current state.
        /// </summary>
        protected abstract bool GetCanExecute(object parameter);

        /// <summary>
        /// Invokes the command execution logic.
        /// </summary>
        protected abstract void InvokeExecute(object parameter);
    }
}
