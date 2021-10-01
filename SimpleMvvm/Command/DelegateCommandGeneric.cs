using System;

namespace SimpleMvvm.Command
{
    /// <summary>
    /// Defines delegate command without parameter.
    /// </summary>
    public class DelegateCommand<ParamType> : DelegateCommand
    {
        /// <summary>
        /// The method to be called when the command is invoked.
        /// </summary>
        public override Action<object> Execute => InvokeExecuteGeneric;

        /// <summary>
        /// Generic Execute delegate.
        /// </summary>
        public Action<ParamType> ExecuteGeneric { get; set; }

        private void InvokeExecuteGeneric(object parameter)
        {
            ExecuteGeneric?.Invoke((ParamType)parameter);
        }

        /// <summary>
        /// Instantiate a generic DelegateCommand.
        /// </summary>
        public DelegateCommand() { }

        /// <summary>
        /// Instantiate a generic DelegateCommand with Execute Action.
        /// </summary>
        public DelegateCommand(Action<ParamType> executeAction)
        {
            ExecuteGeneric = executeAction;
        }

        /// <summary>
        /// Wraps the delegate as a command.
        /// </summary>
        public static implicit operator DelegateCommand<ParamType>(Action<ParamType> executeAction)
        {
            return new DelegateCommand<ParamType>(executeAction);
        }
    }
}
