using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Gameplay
{
    public class Player
    {
        public readonly Observable<int> AmmoCount = new(0);
        public readonly PlayerConfig Config;
        public readonly PlayerView View;

        private readonly PlayerInput _input;
        private readonly BulletSpawner _bulletSpawner;
        private readonly Vector3 _startPosition;
        private readonly Sounds _sounds;

        private float _shootCooldown;
        private UniTaskCompletionSource _deathCompletition;
        private IPlayerState _state;

        public Player(PlayerConfig playerConfig, BulletSpawner bulletSpawner, PlayerView viewPrefab,
            Vector3 startPosition, Sounds sounds)
        {
            _input = new PlayerInput();
            Config = playerConfig;
            _bulletSpawner = bulletSpawner;
            View = Object.Instantiate(viewPrefab);
            _sounds = sounds;
            _startPosition = startPosition;

            View.CollidedWithZombie += Die;
            View.CollidedWithAmmo += PickAmmo;
        }

        public void Update(float deltaTime)
        {
            _input.Update(deltaTime);
            var nextState = _state?.Update(this, _input, deltaTime);
            if (nextState == null) return;

            _state?.Exit(this);
            _state = nextState;
            _state.Enter(this);
        }

        public async UniTask WaitDeath(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => _deathCompletition.TrySetCanceled());
            await _deathCompletition.Task.Preserve();
        }

        private void PickAmmo(AmmoView ammo)
        {
            ammo.MarkCollected();
            AmmoCount.Value++;
            _sounds.PlayClip("pickItem", CancellationToken.None).Forget();
        }

        private void Die()
        {
            _deathCompletition.TrySetResult();
        }

        public void Shoot()
        {
            var shootPoint = View.GetShootPoint();
            _bulletSpawner.SpawnBullet(shootPoint, View.GetDirection());
            AmmoCount.Value--;

            _sounds.PlayClip("gunshot", CancellationToken.None).Forget();
        }

        public void Reset()
        {
            _deathCompletition = new UniTaskCompletionSource();

            AmmoCount.Value = Config.StartAmmoCount;

            View.transform.position = _startPosition;
            View.SetDirection(1);
            View.SetRunning(false);
            View.SetShooting(false);
            View.ResetVelocity();

            _input.Reset();

            _state = new IdleState();
        }
    }
}