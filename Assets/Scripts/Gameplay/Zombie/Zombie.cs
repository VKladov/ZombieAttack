using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Gameplay
{
    public class Zombie : IPoolableObject
    {
        public event Action<Zombie> Died;

        public int MaxHealth => _config.MaxHealth;
        public bool InUse { get; private set; }

        public Vector2 Position => _view.transform.position;

        private readonly Player _target;
        private readonly ZombieView _view;
        private readonly ZombieConfig _config;
        private readonly Sounds _sounds;

        private int _health;

        public Zombie(ZombieConfig config, Player target, Sounds sounds)
        {
            _config = config;
            _target = target;
            _sounds = sounds;
            _view = Object.Instantiate(_config.ViewPrefab);
            _view.DamageTaken += TakeDamage;
        }

        public void Reset()
        {
            _view.gameObject.SetActive(true);
            _view.UpdateHealthBar(1f);
            _health = _config.MaxHealth;
            InUse = true;
        }

        public void Update(float deltaTime)
        {
            var toPlayer = _target.View.transform.position - _view.transform.position;
            _view.SetHorizontalVelocity(Mathf.Sign(toPlayer.x) * _config.Speed);
        }

        public void SetPosition(Vector3 position)
        {
            _view.transform.position = position;
        }

        private void TakeDamage(int damage)
        {
            var newHealth = Math.Clamp(_health - damage, 0, MaxHealth);
            if (newHealth == _health) return;

            _sounds.PlayClip("impact", CancellationToken.None).Forget();

            _health = newHealth;
            _view.UpdateHealthBar((float)newHealth / MaxHealth);
            if (_health == 0) Die();
        }

        private void Die()
        {
            _sounds.PlayClip("death", CancellationToken.None).Forget();
            Died?.Invoke(this);
            _view.gameObject.SetActive(false);
            InUse = false;
        }

        public void Clear()
        {
            _view.gameObject.SetActive(false);
            InUse = false;
        }
    }
}