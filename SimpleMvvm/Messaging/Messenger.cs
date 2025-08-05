using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleMvvm.Messaging
{
    /// <summary>
    /// The Messenger is a class allowing objects to exchange messages.
    /// </summary>
    public class Messenger
    {
        private readonly ConcurrentDictionary<string, List<WeakAction<object>>>
            _dic = new ConcurrentDictionary<string, List<WeakAction<object>>>();

        /// <summary>
        /// Register a delegate to receive the message.
        /// </summary>
        public bool Register(string token, Action<object> action, bool keepAlive = false)
        {
            var list = _dic.GetOrAdd(token, _ => new List<WeakAction<object>>());

            lock (list)
            {
                list.Add(new WeakAction<object>(action)
                {
                    KeepAliveRef = keepAlive ? action : null
                });
            }

            return true;
        }

        /// <summary>
        /// Unregister the delegate.
        /// </summary>
        public bool Unregister(string token, Action<object> action)
        {
            bool result = false;

            if (_dic.TryGetValue(token, out var list))
            {
                lock (list)
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        if (list[i] != null && list[i].Equals(action))
                        {
                            list[i] = null;
                            result = true;
                            break;
                        }
                    }

                    Cleanup(list);
                }
            }

            return result;
        }

        /// <summary>
        /// Unregister all delegates.
        /// </summary>
        public bool UnregisterAll(string token)
        {
            return _dic.TryRemove(token, out _);
        }

        /// <summary>
        /// Cleanup null or dead delegates for the specified list.
        /// </summary>
        private static int Cleanup(List<WeakAction<object>> list)
        {
            return list.RemoveAll(x => x == null || !x.IsAlive);
        }

        /// <summary>
        /// Cleanup null or dead delegates for the specified token.
        /// </summary>
        public int Cleanup(string token)
        {
            if (_dic.TryGetValue(token, out var list))
            {
                lock (list)
                {
                    return Cleanup(list);
                }
            }

            return 0;
        }

        /// <summary>
        /// Send message.
        /// </summary>
        public void Send(string token, object message)
        {
            if (_dic.TryGetValue(token, out var list))
            {
                WeakAction<object>[] actions;

                lock (list)
                {
                    actions = list.ToArray();
                }

                foreach (var item in actions)
                {
                    item?.TryInvoke(message);
                }
            }
        }

        /// <summary>
        /// Send message asynchronously.
        /// </summary>
        public async Task SendAsync(string token, object message)
        {
            await Task.Run(() => Send(token, message));
        }



        /// <summary>
        /// Global messenger object.
        /// </summary>
        public static Messenger Global { get; } = new Messenger();
    }
}
