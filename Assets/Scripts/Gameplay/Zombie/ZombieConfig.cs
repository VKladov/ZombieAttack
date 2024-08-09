using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Zombie Config")]
    public class ZombieConfig : ScriptableObject
    {
        public int Speed => _speed;
        public int MaxHealth => _maxHealth;
        public ZombieView ViewPrefab => _viewPrefab;
        [SerializeField] private int _speed;
        [SerializeField] private int _maxHealth;
        [SerializeField] private ZombieView _viewPrefab;
    }
}