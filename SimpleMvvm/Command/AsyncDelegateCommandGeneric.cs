using SimpleMvvm.Internal;
using System;
using System.Threading.Tasks;

namespace SimpleMvvm.Command
{
    /// <summary>
    /// Defines an asynchronous delegate command with a generic parameter.
    /// </summary>
    public class AsyncDelegateCommand<TParam> : AsyncDelegateCommand
    {
        /// <inheritdoc/>
        public override Func<object, Task> ExecuteAsync => InvokeExecuteAsyncGeneric;

        /// <summary>
        /// Asynchronous method to be called when the command is invoked.
        /// </summary>
        public Func<TParam, Task> ExecuteAsyncGeneric { get; set; }

        /// <summary>
        /// Invokes the ExecuteAsyncGeneric method.
        /// </summary>
        private async Task InvokeExecuteAsyncGeneric(object parameter)
        {
            var executeAsync = ExecuteAsyncGeneric;
            if (executeAsync != null)
            {
                var param = TypeHelper.Cast<TParam>(parameter);
                await executeAsync(param);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDelegateCommand{TParam}"/> class.
        /// </summary>
        public AsyncDelegateCommand()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDelegateCommand{TParam}"/> class with the specified execute action.
        /// </summary>
        public AsyncDelegateCommand(Func<TParam, Task> executeAsync)
        {
            ExecuteAsyncGeneric = executeAsync;
        }
    }
}
