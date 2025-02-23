using System;
using System.Collections.Generic;

namespace SimpleMvvm.Messaging
{
    /// <summary>
    /// The Messenger is a class allowing objects to exchange messages.
    /// </summary>
    public class Messenger
    {
        private readonly Dictionary<string, List<WeakReference<Action<object>>>> _dic
            = new Dictionary<string, List<WeakReference<Action<object>>>>();

        /// <summary>
        /// Register a delegate to receive the message.
        /// </summary>
        public void Register(string token, Action<object> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (!_dic.ContainsKey(token))
                _dic.Add(token, new List<WeakReference<Action<object>>>());

            var list = _dic[token];
            list.Add(new WeakReference<Action<object>>(action));
        }

        /// <summary>
        /// Unregister the delegate.
        /// </summary>
        public void Unregister(string token, Action<object> action)
        {
            if (_dic.TryGetValue(token, out var list))
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (!list[i].TryGetTarget(out var target))
                    {
                        list.RemoveAt(i);
                    }
                    else if (target == action)
                    {
                        list.RemoveAt(i);
                        break;
                    }
                }

                if (list.Count == 0)
                    _dic.Remove(token);
            }
        }

        /// <summary>
        /// Unregister all delegates.
        /// </summary>
        public void UnregisterAll(string token)
        {
            _dic.Remove(token);
        }

        /// <summary>
        /// Send message.
        /// </summary>
        public void Send(string token, object message)
        {
            if (_dic.TryGetValue(token, out var list))
            {
                foreach (var weakAction in list)
                {
                    if (weakAction.TryGetTarget(out var action))
                    {
                        action(message);
                    }
                }
            }
        }



        /// <summary>
        /// Global messenger object.
        /// </summary>
        public static Messenger Global { get; }

        static Messenger()
        {
            Global = new Messenger();
        }
    }
}
