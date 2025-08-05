using System;

namespace SimpleMvvm.Internal
{
    /// <summary>
    /// Internal helper class for type casting operations.
    /// </summary>
    internal static class TypeHelper
    {
        /// <summary>
        /// Casts the given object to the specified type.
        /// </summary>
        /// <exception cref="InvalidCastException"></exception>
        public static T Cast<T>(object obj)
        {
            if (obj == null)
            {
                return default;
            }
            else if (obj is T t)
            {
                return t;
            }
            else
            {
                throw new InvalidCastException($"Cannot cast object of type {obj.GetType()} to {typeof(T)}.");
            }
        }
    }
}
