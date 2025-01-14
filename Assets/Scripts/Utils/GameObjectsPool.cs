﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public class GameObjectsPool<T> where T : Behaviour
    {
        private readonly Dictionary<int, Queue<T>> _pool = new();
        private readonly Dictionary<int, int> _instaceToPrefabId = new();

        public T GetInstance(T prefab)
        {
            var prefabId = prefab.GetInstanceID();
            if (_pool.TryGetValue(prefabId, out var list) && list.Count > 0)
            {
                var instanceFromPool = list.Dequeue();
                instanceFromPool.gameObject.SetActive(true);
                return instanceFromPool;
            }

            var instance = Object.Instantiate(prefab);
            _instaceToPrefabId.Add(instance.GetInstanceID(), prefabId);
            return instance;
        }

        public void Recycle(T instance)
        {
            var instanceId = instance.GetInstanceID();
            if (!_instaceToPrefabId.TryGetValue(instanceId, out var prefabId))
                throw new Exception("Trying to return unknown object");

            instance.gameObject.SetActive(false);
            if (_pool.TryGetValue(prefabId, out var list))
            {
                list.Enqueue(instance);
            }
            else
            {
                var newList = new Queue<T>();
                newList.Enqueue(instance);
                _pool.Add(prefabId, newList);
            }
        }
    }
}