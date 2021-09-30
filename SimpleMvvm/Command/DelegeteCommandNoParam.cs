using System;

namespace SimpleMvvm.Command
{
    /// <summary>
    /// Defines delegate command without parameter.
    /// </summary>
    public class DelegeteCommand : DelegateCommand<object>
    {
        /// <summary>
        /// Instantiate a DelegateCommand.
        /// </summary>
        public DelegeteCommand() : base() { }

        /// <summary>
        /// Instantiate a DelegateCommand with Execute Action.
        /// </summary>
        public DelegeteCommand(Action executeAction) : base(o => executeAction()) { }

        /// <summary>
        /// Wraps the delegate as a command.
        /// </summary>
        public static implicit operator DelegeteCommand(Action executeAction)
        {
            return new DelegeteCommand(executeAction);
        }
    }
}
