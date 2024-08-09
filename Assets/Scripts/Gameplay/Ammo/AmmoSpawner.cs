using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Gameplay
{
    public class AmmoSpawner
    {
        private readonly AmmoView _viewPrefab;
        private readonly GameObjectsPool<AmmoView> _pool = new();
        private readonly List<AmmoView> _spawnedAmmo = new();

        public AmmoSpawner(AmmoView viewPrefab)
        {
            _viewPrefab = viewPrefab;
        }

        public void SpawnAmmo(Vector2 position, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var force = Random.insideUnitCircle;
                if (force.y < 0) force.y *= -1;

                var view = _pool.GetInstance(_viewPrefab);
                view.transform.position = position;
                view.Collected += Collected;
                view.ApplyForce(force);

                _spawnedAmmo.Add(view);
            }
        }

        public void Reset()
        {
            foreach (var ammoView in _spawnedAmmo)
            {
                ammoView.ResetSpeed();
                _pool.Recycle(ammoView);
            }

            _spawnedAmmo.Clear();
        }

        private void Collected(AmmoView ammoView)
        {
            _spawnedAmmo.Remove(ammoView);

            ammoView.ResetSpeed();
            _pool.Recycle(ammoView);
        }
    }
}