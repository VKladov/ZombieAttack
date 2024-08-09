using System.Threading;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;
using Utils;

namespace Gameplay
{
    public class GameLoop
    {
        private readonly Player _player;
        private readonly ZombieSpawner _zombieSpawner;
        private readonly AmmoSpawner _ammoSpawner;
        private readonly GameUI _gameUI;
        private readonly Sounds _sounds;
        private UniTaskCompletionSource _playerDeath;

        public GameLoop(Player player, ZombieSpawner zombieSpawner, AmmoSpawner ammoSpawner, GameUI gameUI,
            Sounds sounds)
        {
            _player = player;
            _zombieSpawner = zombieSpawner;
            _ammoSpawner = ammoSpawner;
            _gameUI = gameUI;
            _sounds = sounds;
        }

        public async UniTask Play(CancellationToken cancellationToken)
        {
            _player.Reset();
            _player.AmmoCount.Changed += UpdateAmmoCount;

            _zombieSpawner.Reset();
            _zombieSpawner.ZombieDied += SpawnAmmo;

            _ammoSpawner.Reset();

            _gameUI.Show();
            UpdateAmmoCount(_player.AmmoCount.Value);

            var updateCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            UpdateAllLoop(updateCancellation.Token).Forget();
            await _player.WaitDeath(cancellationToken);

            updateCancellation.Cancel();

            _sounds.PlayClip("GameOver", CancellationToken.None).Forget();

            _gameUI.Hide();
            _player.AmmoCount.Changed -= UpdateAmmoCount;
            _zombieSpawner.ZombieDied -= SpawnAmmo;
        }

        private void UpdateAmmoCount(int count)
        {
            _gameUI.UpdateAmmoCount(count);
        }

        private async UniTask UpdateAllLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _player.Update(Time.deltaTime);
                _zombieSpawner.Update(Time.deltaTime);
                await UniTask.Yield();
            }
        }

        private void SpawnAmmo(Zombie zombie)
        {
            _ammoSpawner.SpawnAmmo(zombie.Position, Random.Range(zombie.MaxHealth - 1, zombie.MaxHealth + 3));
        }
    }
}