using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class ZombieSpawner : IDisposable
    {
        public event Action<Zombie> ZombieDied;

        private readonly ZombieConfig[] _configs;
        private readonly Vector3[] _spawnPoints;
        private readonly Player _target;
        private readonly Sounds _sounds;
        private readonly ObjectsPool<Zombie>[] _pools;
        private readonly List<Zombie> _aliveZombies = new();

        private float _zombieSpawnCooldown;

        public ZombieSpawner(ZombieConfig[] configs, Vector3[] spawnPoints, Player target, Sounds sounds)
        {
            _configs = configs;
            _spawnPoints = spawnPoints;
            _target = target;
            _sounds = sounds;
            _pools = new ObjectsPool<Zombie>[configs.Length];
            for (var i = 0; i < _pools.Length; i++)
            {
                var config = _configs[i];
                _pools[i] = new ObjectsPool<Zombie>(() => new Zombie(config, target, sounds), 10);
            }
        }

        public void Update(float deltaTime)
        {
            UpdateZombies(deltaTime);
            UpdateSpawn(deltaTime);
        }

        private void UpdateZombies(float deltaTime)
        {
            foreach (var aliveZombie in _aliveZombies) aliveZombie.Update(deltaTime);
        }

        private void UpdateSpawn(float deltaTime)
        {
            if (_zombieSpawnCooldown < 0f)
            {
                SpawnZombie(_spawnPoints[Random.Range(0, _spawnPoints.Length)]);
                _zombieSpawnCooldown = Random.Range(1f, 10f);
            }

            _zombieSpawnCooldown -= deltaTime;
        }

        private void SpawnZombie(Vector3 position)
        {
            var zombie = _pools[Random.Range(0, _pools.Length)].GetInstance();
            zombie.Reset();
            zombie.SetPosition(position);

            zombie.Died += HandleDeath;

            _aliveZombies.Add(zombie);
        }

        private void HandleDeath(Zombie zombie)
        {
            ZombieDied?.Invoke(zombie);
            zombie.Died -= HandleDeath;
            _aliveZombies.Remove(zombie);
        }

        public void Dispose()
        {
            foreach (var aliveZomby in _aliveZombies) aliveZomby.Died -= HandleDeath;
        }

        public void Reset()
        {
            foreach (var aliveZomby in _aliveZombies)
            {
                aliveZomby.Died -= HandleDeath;
                aliveZomby.Clear();
            }

            _aliveZombies.Clear();
        }
    }
}