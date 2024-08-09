using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        public float Speed => _speed;
        public float ShootDelay => _shootDelay;
        public int StartAmmoCount => _startAmmoCount;
        [SerializeField] private float _speed;
        [SerializeField] private float _shootDelay;
        [SerializeField] private int _startAmmoCount;
    }
}