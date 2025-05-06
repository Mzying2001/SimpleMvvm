using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SimpleMvvm.Ioc
{
    /// <summary>
    /// A simple Inversion of Control (IoC) container.
    /// </summary>
    public class SimpleIoc
    {
        private class InstanceEntry
        {
            public Lifetime Lifetime { get; set; }
            public object Instance { get; set; }
            public Func<object[], object> Factory { get; set; }

            private readonly object _lock = new object();

            public object Get(params object[] args)
            {
                if (Lifetime == Lifetime.Transient)
                    return Factory(args);
                else
                {
                    if (Instance == null)
                    {
                        lock (_lock)
                        {
                            if (Instance == null)
                                Instance = Factory(args);
                        }
                    }
                    return Instance;
                }
            }
        }

        private readonly ConcurrentDictionary<Type, InstanceEntry>
            _instanceRegistry = new ConcurrentDictionary<Type, InstanceEntry>();

        /// <summary>
        /// Checks if the type is registered in the IoC container.
        /// </summary>
        public bool IsRegistered(Type type)
        {
            ThrowIfArgumentNull(type, nameof(type));
            return _instanceRegistry.ContainsKey(type);
        }

        /// <summary>
        /// Checks if the type is registered in the IoC container.
        /// </summary>
        public bool IsRegistered<T>()
        {
            return IsRegistered(typeof(T));
        }

        /// <summary>
        /// Registers an instance to the IoC container.
        /// </summary>
        /// <returns>True if the instance was registered, false if it was already registered.</returns>
        public bool Register<T>(T instance)
        {
            Type type = typeof(T);
            ThrowIfArgumentNull(instance, nameof(instance));

            return _instanceRegistry.TryAdd(type, new InstanceEntry
            { Lifetime = Lifetime.Singleton, Instance = instance });
        }

        /// <summary>
        /// Registers a type to the IoC container using constructor injection.
        /// </summary>
        /// <returns>True if the type was registered, false if it was already registered.</returns>
        public bool Register<T>(Lifetime lifetime) where T : new()
        {
            return Register<T>(false, lifetime);
        }

        /// <summary>
        /// Registers a type to the IoC container using constructor injection.
        /// </summary>
        /// <returns>True if the type was registered, false if it was already registered.</returns>
        public bool Register<T>(bool createImmediately = false, Lifetime lifetime = Lifetime.Singleton) where T : new()
        {
            return Register(() => new T(), createImmediately, lifetime);
        }

        /// <summary>
        /// Registers a factory method to the IoC container.
        /// </summary>
        /// <returns>True if the type was registered, false if it was already registered.</returns>
        public bool Register<T>(Func<T> factory, Lifetime lifetime)
        {
            return Register(factory, false, lifetime);
        }

        /// <summary>
        /// Registers a factory method to the IoC container.
        /// </summary>
        /// <returns>True if the type was registered, false if it was already registered.</returns>
        public bool Register<T>(Func<T> factory, bool createImmediately = false, Lifetime lifetime = Lifetime.Singleton)
        {
            Type type = typeof(T);
            ThrowIfArgumentNull(factory, nameof(factory));

            var entry = new InstanceEntry
            { Lifetime = lifetime, Factory = _ => factory() };

            if (!_instanceRegistry.TryAdd(type, entry))
                return false;

            if (createImmediately)
                entry.Get();

            return true;
        }

        /// <summary>
        /// Registers a factory method with parameters to the IoC container.
        /// </summary>
        /// <returns>True if the type was registered, false if it was already registered.</returns>
        public bool Register<T>(Func<object[], T> factory, Lifetime lifetime = Lifetime.Singleton)
        {
            Type type = typeof(T);
            ThrowIfArgumentNull(factory, nameof(factory));

            return _instanceRegistry.TryAdd(type, new InstanceEntry
            { Lifetime = lifetime, Factory = args => factory(args) });
        }

        /// <summary>
        /// Unregisters a type from the IoC container.
        /// </summary>
        public bool Unregister(Type type, bool disposeInstance = true)
        {
            ThrowIfArgumentNull(type, nameof(type));

            if (_instanceRegistry.TryRemove(type, out InstanceEntry entry))
            {
                if (disposeInstance && entry.Instance is IDisposable disposable)
                    disposable.Dispose();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Unregisters a type from the IoC container.
        /// </summary>
        public bool Unregister<T>(bool disposeInstance = true)
        {
            return Unregister(typeof(T), disposeInstance);
        }

        /// <summary>
        /// Gets an instance of the specified type from the IoC container.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public object GetInstance(Type type, params object[] args)
        {
            ThrowIfArgumentNull(type, nameof(type));
            InstanceEntry entry = null;

            if (!_instanceRegistry.TryGetValue(type, out entry))
            {
                foreach (var pair in _instanceRegistry)
                {
                    if (type.IsAssignableFrom(pair.Key))
                    {
                        entry = pair.Value;
                        break;
                    }
                }
            }

            if (entry == null)
                throw new KeyNotFoundException($"Unable to resolve type {type}.");

            return entry.Get(args);
        }

        /// <summary>
        /// Gets an instance of the specified type from the IoC container.
        /// </summary>
        public T GetInstance<T>(params object[] args)
        {
            return (T)GetInstance(typeof(T), args);
        }

        /// <summary>
        /// Throws an ArgumentNullException if the value is null.
        /// </summary>
        private static void ThrowIfArgumentNull(object value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }



        /// <summary>
        /// Global instance of the IoC container.
        /// </summary>
        public static SimpleIoc Global { get; } = new SimpleIoc();
    }
}
