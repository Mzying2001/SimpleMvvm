using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimpleMvvm.Messaging
{
    internal class WeakAction<T>
    {
        public MethodInfo Method { get; }

        public WeakReference Target { get; }

        public bool IsAlive
        {
            get => Target.IsAlive;
        }

        public WeakAction(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (action.GetInvocationList().Length > 1)
                throw new ArgumentException("Only single cast actions are supported.", nameof(action));

            Method = action.Method;
            Target = new WeakReference(action.Target);
        }

        public void Invoke(T arg)
        {
            Method.Invoke(Target.Target, new object[] { arg });
        }

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

        public override int GetHashCode()
        {
            return Method.GetHashCode() ^ Target.GetHashCode();
        }

        public static bool operator ==(WeakAction<T> left, WeakAction<T> right)
        {
            return EqualityComparer<WeakAction<T>>.Default.Equals(left, right);
        }

        public static bool operator !=(WeakAction<T> left, WeakAction<T> right)
        {
            return !(left == right);
        }
    }
}
