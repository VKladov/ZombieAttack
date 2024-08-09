using System;
using UnityEngine;
using Utils;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ZombieView : MonoBehaviour
    {
        public event Action<int> DamageTaken;

        [SerializeField] private ProgressBar _healthBar;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetHorizontalVelocity(float velocity)
        {
            _rigidbody.velocity = new Vector2(velocity, _rigidbody.velocity.y);
            _spriteRenderer.flipX = velocity < 0f;
        }

        public void UpdateHealthBar(float value)
        {
            _healthBar.SetValue(value);
        }

        public void TakeDamage(int damage)
        {
            DamageTaken?.Invoke(damage);
        }
    }
}