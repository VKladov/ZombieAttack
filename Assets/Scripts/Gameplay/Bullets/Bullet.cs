using System;
using UnityEngine;

namespace Gameplay
{
    public class Bullet : MonoBehaviour
    {
        public event Action<Bullet> Hit;
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;

        private Vector3 _direction;
        private Vector3 _prevPosition;
        private RaycastHit2D[] _hits;

        public void Init(Vector3 position, Vector3 direction)
        {
            transform.position = position;
            _direction = direction;
        }

        private void Update()
        {
            var prevPosition = transform.position;
            UpdatePosition();
            CheckCollision(prevPosition, transform.position);
        }

        private void UpdatePosition()
        {
            transform.position += _direction * (Time.deltaTime * _speed);
        }

        private void CheckCollision(Vector3 from, Vector3 to)
        {
            var delta = to - from;
            var hit = Physics2D.Raycast(from, delta.normalized, delta.magnitude);
            if (hit.collider == null) return;

            if (hit.collider.TryGetComponent(out ZombieView character)) character.TakeDamage(_damage);

            Hit?.Invoke(this);
        }
    }
}