using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Gameplay
{
    public class BulletSpawner : IDisposable
    {
        public const int MAX_BULLETS_COUNT = 10;

        private readonly Bullet _prefab;
        private readonly GameObjectsPool<Bullet> _pool;
        private readonly List<Bullet> _flyingBullets;

        public BulletSpawner(Bullet prefab)
        {
            _prefab = prefab;
            _pool = new GameObjectsPool<Bullet>();
            _flyingBullets = new List<Bullet>(MAX_BULLETS_COUNT);
        }

        public void SpawnBullet(Vector3 position, Vector3 direction)
        {
            var bullet = _pool.GetInstance(_prefab);
            if (bullet == null)
            {
                Debug.LogWarning("Bullets finished in pool");
                return;
            }

            bullet.Init(position, direction);
            bullet.Hit += OnBulletHit;
            _flyingBullets.Add(bullet);
        }

        private void OnBulletHit(Bullet bullet)
        {
            bullet.Hit -= OnBulletHit;
            _pool.Recycle(bullet);
        }

        public void Dispose()
        {
            foreach (var bullet in _flyingBullets)
            {
                bullet.Hit -= OnBulletHit;
                _pool.Recycle(bullet);
            }
        }
    }
}