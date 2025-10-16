using System;
using System.Collections.Generic;

namespace Game
{
    public class DataLocator
    {
        private readonly Dictionary<Type, object> _data = new();

        public DataLocator Register<T>(T data)
        {
            _data.Add(typeof(T), data);
            return this;
        }

        public T Get<T>() => (T) _data[typeof(T)];
    }
}