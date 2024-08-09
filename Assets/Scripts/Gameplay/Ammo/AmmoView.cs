using System;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class AmmoView : MonoBehaviour
    {
        public event Action<AmmoView> Collected;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void ApplyForce(Vector2 impulse)
        {
            _rigidbody.AddForce(impulse, ForceMode2D.Impulse);
        }

        public void ResetSpeed()
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0f;
        }

        public void MarkCollected()
        {
            Collected?.Invoke(this);
        }
    }
}