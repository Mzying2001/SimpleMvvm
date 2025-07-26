using System;

namespace SimpleMvvm.Messaging
{
    /// <summary>
    /// Proxy to exchange data between the message handler and the message sender.
    /// </summary>
    public class MsgProxy
    {
        /// <summary>
        /// The result given by the message handler.
        /// </summary>
        public object Result { get; set; } = null;

        /// <summary>
        /// The arguments passed to the message handler.
        /// </summary>
        public object[] Args { get; set; } = new object[0];

        /// <summary>
        /// Gets or sets a value indicating whether the message has been handled.
        /// </summary>
        public bool Handled { get; set; } = false;

        /// <summary>
        /// Gets the result in the specified type.
        /// </summary>
        /// <exception cref="InvalidCastException"></exception>
        public T GetResult<T>()
        {
            return Cast<T>(Result);
        }

        /// <summary>
        /// Gets the argument at the specified index in the specified type.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public T GetArg<T>(int index)
        {
            if (Args != null && index >= 0 && index < Args.Length)
            {
                return Cast<T>(Args[index]);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range of the arguments array.");
            }
        }

        /// <summary>
        /// Casts the given object to the specified type.
        /// </summary>
        /// <exception cref="InvalidCastException"></exception>
        private T Cast<T>(object obj)
        {
            if (obj is T t)
            {
                return t;
            }
            else if (obj == null && typeof(T).IsValueType)
            {
                return default;
            }
            else
            {
                throw new InvalidCastException($"Cannot cast {Result?.GetType().Name ?? "null"} to {typeof(T).Name}.");
            }
        }
    }
}
