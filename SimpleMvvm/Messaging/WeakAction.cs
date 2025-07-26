using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimpleMvvm.Messaging
{
    /// <summary>
    /// Action that does not keep a strong reference to the target object.
    /// </summary>
    internal class WeakAction<T>
    {
        /// <summary>
        /// The method that will be invoked when the action is called.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// The weak reference to the target object that contains the method.
        /// </summary>
        public WeakReference Target { get; }

        /// <summary>
        /// True if the target object is still alive; otherwise, false.
        /// </summary>
        public bool IsAlive => Target.IsAlive;

        /// <summary>
        /// Set strong reference to keep the target object alive if needed.
        /// </summary>
        public object KeepAliveRef { get; set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="WeakAction{T}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public WeakAction(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (action.GetInvocationList().Length > 1)
                throw new ArgumentException("Only single cast actions are supported.", nameof(action));

            Method = action.Method;
            Target = new WeakReference(action.Target);
        }

        /// <summary>
        /// Invokes the action with the specified argument.
        /// </summary>
        public void Invoke(T arg)
        {
            Method.Invoke(Target.Target, new object[] { arg });
        }

        /// <summary>
        /// Tries to invoke the action with the specified argument.
        /// </summary>
        /// <returns>True if the action was invoked successfully; otherwise, false.</returns>
        public bool TryInvoke(T arg)
        {
            if (Method.IsStatic)
            {
                Method.Invoke(null, new object[] { arg });
                return true;
            }
            else
            {
                object target = Target.Target;

                if (!Target.IsAlive)
                    return false;

                Method.Invoke(target, new object[] { arg });
                return true;
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is WeakAction<T> weakAction)
            {
                if (Method.IsStatic && weakAction.Method.IsStatic)
                {
                    return Method == weakAction.Method;
                }
                else
                {
                    object target1 = Target.Target;
                    object target2 = weakAction.Target.Target;

                    return Target.IsAlive && weakAction.Target.IsAlive
                        && target1 == target2 && Method == weakAction.Method;
                }
            }
            else if (obj is Action<T> action)
            {
                if (Method.IsStatic && action.Method.IsStatic)
                {
                    return Method == action.Method;
                }
                else
                {
                    object target = action.Target;
                    return Target.IsAlive && target == action.Target && Method == action.Method;
                }
            }
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Method != null ? Method.GetHashCode() : 0);
                hash = hash * 23 + (Target != null ? Target.GetHashCode() : 0);
                return hash;
            }
        }

        /// <summary>
        /// Compares two <see cref="WeakAction{T}"/> instances for equality.
        /// </summary>
        public static bool operator ==(WeakAction<T> left, WeakAction<T> right)
        {
            return EqualityComparer<WeakAction<T>>.Default.Equals(left, right);
        }

        /// <summary>
        /// Compares two <see cref="WeakAction{T}"/> instances for inequality.
        /// </summary>
        public static bool operator !=(WeakAction<T> left, WeakAction<T> right)
        {
            return !EqualityComparer<WeakAction<T>>.Default.Equals(left, right);
        }
    }
}
