using System;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerView : MonoBehaviour
    {
        public event Action CollidedWithZombie;
        public event Action<AmmoView> CollidedWithAmmo;

        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");

        [SerializeField] private Vector3 _bulletSpawnOffset;
        [SerializeField] private AudioClip _pickUpSound;

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;

        private int _currentDirection;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void ResetVelocity()
        {
            _rigidbody.velocity = Vector2.zero;
        }

        public void SetRunning(bool isRunning)
        {
            _animator.SetBool(IsRunning, isRunning);
        }

        public void SetShooting(bool isShooting)
        {
            _animator.SetBool(IsShooting, isShooting);
        }

        public void SetDirection(int direction)
        {
            _currentDirection = direction;
            _spriteRenderer.flipX = direction < 0;
        }

        public void SetHorizontalVelocity(float velocity)
        {
            _rigidbody.velocity = new Vector2(velocity, _rigidbody.velocity.y);
            _spriteRenderer.flipX = velocity < 0f;
        }

        public Vector3 GetShootPoint()
        {
            var localShootPoint = _bulletSpawnOffset;
            localShootPoint.x *= _currentDirection;

            return transform.position + localShootPoint;
        }

        public Vector3 GetDirection()
        {
            return new Vector3(_currentDirection, 0, 0);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent(out ZombieView zombie))
                CollidedWithZombie?.Invoke();
            else if (collision.collider.TryGetComponent(out AmmoView ammo)) CollidedWithAmmo?.Invoke(ammo);
        }
    }
}