﻿using System;
using System.Collections.Generic;

namespace SimpleMvvm.Messaging
{
    /// <summary>
    /// The Messenger is a class allowing objects to exchange messages.
    /// </summary>
    public class Messenger
    {
        private readonly Dictionary<string, List<WeakAction<object>>> _dic
            = new Dictionary<string, List<WeakAction<object>>>();

        /// <summary>
        /// Register a delegate to receive the message.
        /// </summary>
        public void Register(string token, Action<object> action)
        {
            var weakAction = new WeakAction<object>(action);

            if (!_dic.ContainsKey(token))
                _dic.Add(token, new List<WeakAction<object>>());

            var list = _dic[token];
            list.Add(weakAction);
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
                    if (list[i].Equals(action))
                    {
                        list.RemoveAt(i);
                        break;
                    }
                    else if (!list[i].IsAlive)
                    {
                        list.RemoveAt(i);
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
                    weakAction.TryInvoke(message);
            }
        }



        /// <summary>
        /// Global messenger object.
        /// </summary>
        public static Messenger Global { get; } = new Messenger();
    }
}
