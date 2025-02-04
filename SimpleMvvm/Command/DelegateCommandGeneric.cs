﻿using System;

namespace SimpleMvvm.Command
{
    /// <summary>
    /// Defines delegate command without parameter.
    /// </summary>
    public class DelegateCommand<TParam> : DelegateCommand
    {
        /// <summary>
        /// The method to be called when the command is invoked.
        /// </summary>
        public override Action<object> Execute => InvokeExecuteGeneric;

        /// <summary>
        /// Generic Execute delegate.
        /// </summary>
        public Action<TParam> ExecuteGeneric { get; set; }

        private void InvokeExecuteGeneric(object parameter)
        {
            ExecuteGeneric?.Invoke((TParam)parameter);
        }

        /// <summary>
        /// Instantiate a generic DelegateCommand.
        /// </summary>
        public DelegateCommand() { }

        /// <summary>
        /// Instantiate a generic DelegateCommand with Execute Action.
        /// </summary>
        public DelegateCommand(Action<TParam> executeAction)
        {
            ExecuteGeneric = executeAction;
        }

        /// <summary>
        /// Wraps the delegate as a command.
        /// </summary>
        public static implicit operator DelegateCommand<TParam>(Action<TParam> executeAction)
        {
            return new DelegateCommand<TParam>(executeAction);
        }
    }
}
