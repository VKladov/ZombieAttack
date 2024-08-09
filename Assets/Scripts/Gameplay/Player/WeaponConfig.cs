using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Weapon Config")]
    public class WeaponConfig : ScriptableObject
    {
        public float ShootDelay => _shootDelay;
        [SerializeField] private float _shootDelay;
    }
}