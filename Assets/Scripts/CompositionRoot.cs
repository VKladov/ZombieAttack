using Cysharp.Threading.Tasks;
using Gameplay;
using UI;
using UnityEngine;
using Utils;

namespace MyNamespace
{
    public class CompositionRoot : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private Bullet _bulletPrefab;

        [SerializeField] private PlayerView _playerViewPrefab;
        [SerializeField] private AmmoView _ammoPrefab;

        [Header("Configs")] [SerializeField] private PlayerConfig _playerConfig;

        [SerializeField] private ZombieConfig[] _zombieConfigs;

        [Header("Level")] [SerializeField] private Vector3 _playerStartPosition;

        [SerializeField] private Vector3[] _spawnPoints;

        [Header("UI")] [SerializeField] private Menu _menu;

        [SerializeField] private GameUI _gameUI;

        [Header("Sound")] [SerializeField] private Sounds _sounds;

        public void Start()
        {
            var bulletSpawner = new BulletSpawner(_bulletPrefab);
            var player = new Player(_playerConfig, bulletSpawner, _playerViewPrefab, _playerStartPosition, _sounds);
            var zombieSpawner = new ZombieSpawner(_zombieConfigs, _spawnPoints, player, _sounds);
            var ammoSpawner = new AmmoSpawner(_ammoPrefab);
            var gameLoop = new GameLoop(player, zombieSpawner, ammoSpawner, _gameUI, _sounds);
            var metaLoop = new MetaLoop(gameLoop, _menu);

            metaLoop.Start(this.GetCancellationTokenOnDestroy());
        }
    }
}