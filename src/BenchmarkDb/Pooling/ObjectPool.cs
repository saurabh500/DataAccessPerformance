﻿using System;
using System.Collections.Concurrent;
using System.Threading;

namespace BenchmarkDb.Pooling
{
    public class ObjectPool<T> where T : class
    {
        private readonly T[] _items;
        private readonly Func<T> _factory;

        public ObjectPool(Func<T> factory, int size)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            _items = new T[size];
            _factory = factory;
        }

        public T Get()
        {
            for (var i = 0; i < _items.Length; i++)
            {
                var item = _items[i];
                if (item != null && Interlocked.CompareExchange(ref _items[i], null, item) == item)
                {
                    return item;
                }
            }

            return _factory();
        }

        public bool Return(T obj)
        {
            for (var i = 0; i < _items.Length; i++)
            {
                if (Interlocked.CompareExchange(ref _items[i], obj, null) == null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
