using SimpleMvvm.Internal;
using System;

namespace SimpleMvvm.Command
{
    /// <summary>
    /// Defines delegate command with generic parameter.
    /// </summary>
    public class DelegateCommand<TParam> : DelegateCommand
    {
        /// <inheritdoc/>
        public override Action<object> Execute => InvokeExecuteGeneric;

        /// <summary>
        /// Generic Execute delegate.
        /// </summary>
        public Action<TParam> ExecuteGeneric { get; set; }

        /// <summary>
        /// Invokes the ExecuteGeneric method.
        /// </summary>
        private void InvokeExecuteGeneric(object parameter)
        {
            ExecuteGeneric?.Invoke(TypeHelper.Cast<TParam>(parameter));
        }

        /// <summary>
        /// Instantiate a generic DelegateCommand.
        /// </summary>
        public DelegateCommand()
        {
        }

        /// <summary>
        /// Instantiate a generic DelegateCommand with Execute Action.
        /// </summary>
        public DelegateCommand(Action<TParam> executeAction)
        {
            ExecuteGeneric = executeAction;
        }
    }
}
