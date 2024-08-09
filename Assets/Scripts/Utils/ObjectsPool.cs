using System;
using System.Collections.Generic;

namespace Utils
{
    public class ObjectsPool<T> where T : IPoolableObject
    {
        private readonly List<T> _pool;
        private readonly Func<T> _create;

        public ObjectsPool(Func<T> create, int capacity)
        {
            _create = create;
            _pool = new List<T>(capacity);
        }

        public T GetInstance()
        {
            for (var i = 0; i < _pool.Count; i++)
            {
                var instance = _pool[i];
                if (instance.InUse) continue;

                instance.Reset();
                return instance;
            }

            var newInstance = _create();
            newInstance.Reset();
            _pool.Add(newInstance);
            return newInstance;
        }
    }
}