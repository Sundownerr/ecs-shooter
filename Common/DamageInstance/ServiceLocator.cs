using System;
using System.Collections.Generic;

namespace Game
{
    public class ServiceLocator
    {
        private readonly Dictionary<Type, object> _services = new();

        public ServiceLocator Register<T>(T service)
        {
            _services.Add(typeof(T), service);

            return this;
        }

        public T Get<T>() => (T) _services[typeof(T)];
    }
}