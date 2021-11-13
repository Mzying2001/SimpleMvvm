using System;
using System.Collections.Generic;

namespace SimpleMvvm.Messaging
{
    /// <summary>
    /// The Messenger is a class allowing objects to exchange messages.
    /// </summary>
    public class Messenger
    {
        private readonly Dictionary<string, Action<object>> _dic
            = new Dictionary<string, Action<object>>();

        private static void EmptyAction(object o) { }

        /// <summary>
        /// Register a delegate to receive the message.
        /// </summary>
        public void Register(string token, Action<object> action)
        {
            if (!_dic.ContainsKey(token))
                _dic.Add(token, EmptyAction);

            _dic[token] += action;
        }

        /// <summary>
        /// Unregister the delegate.
        /// </summary>
        public void Unregister(string token, Action<object> action)
        {
            _dic[token] -= action;

            if (_dic[token] == EmptyAction)
                _dic.Remove(token);
        }

        /// <summary>
        /// Send message.
        /// </summary>
        public void Send(string token, object message)
        {
            if (_dic.ContainsKey(token))
                _dic[token](message);
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
