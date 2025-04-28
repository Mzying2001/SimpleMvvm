using System;
using System.Threading.Tasks;

namespace SimpleMvvm.Command
{
    /// <summary>
    /// Defines an asynchronous delegate command.
    /// </summary>
    public class AsyncDelegateCommand : DelegateCommand
    {
        private bool _isExecuting = false;
        private object _lock = new object();

        /// <inheritdoc/>
        public override Action<object> Execute => InvokeExecuteAsync;

        /// <summary>
        /// Asynchronous method to be called when the command is invoked.
        /// </summary>
        public virtual Func<object, Task> ExecuteAsync { get; set; }

        /// <summary>
        /// Invokes the ExecuteAsync method.
        /// </summary>
        private async void InvokeExecuteAsync(object parameter)
        {
            try
            {
                lock (_lock)
                {
                    if (_isExecuting)
                        throw new InvalidOperationException("Command is executing.");

                    _isExecuting = true;
                    RaiseCanExecuteChanged();
                }

                var executeAsync = ExecuteAsync;
                if (executeAsync != null)
                    await executeAsync(parameter);
            }
            catch
            {
                throw;
            }
            finally
            {
                lock (_lock)
                {
                    if (_isExecuting)
                    {
                        _isExecuting = false;
                        RaiseCanExecuteChanged();
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override bool GetCanExecute()
        {
            return !_isExecuting && base.GetCanExecute();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDelegateCommand"/> class.
        /// </summary>
        public AsyncDelegateCommand()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDelegateCommand"/> class with the specified execute action.
        /// </summary>
        public AsyncDelegateCommand(Func<object, Task> executeAction)
        {
            ExecuteAsync = executeAction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDelegateCommand"/> class with the specified execute action.
        /// </summary>
        public AsyncDelegateCommand(Func<Task> executeAction) : this(_ => executeAction())
        {
        }
    }
}
