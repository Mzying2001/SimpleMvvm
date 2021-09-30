using System;

namespace SimpleMvvm.Command
{
    /// <summary>
    /// Defines delegate command without parameter.
    /// </summary>
    public class DelegateCommand : DelegateCommand<object>
    {
        /// <summary>
        /// Instantiate a DelegateCommand.
        /// </summary>
        public DelegateCommand() : base() { }

        /// <summary>
        /// Instantiate a DelegateCommand with Execute Action.
        /// </summary>
        public DelegateCommand(Action<object> executeAction) : base(executeAction) { }

        /// <summary>
        /// Instantiate a DelegateCommand with Execute Action.
        /// </summary>
        public DelegateCommand(Action executeAction) : this(o => executeAction()) { }

        /// <summary>
        /// Wraps the delegate as a command.
        /// </summary>
        public static implicit operator DelegateCommand(Action executeAction)
        {
            return new DelegateCommand(executeAction);
        }
    }
}
