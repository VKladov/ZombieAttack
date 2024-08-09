using UnityEngine;

namespace Gameplay
{
    public class PlayerInput
    {
        public bool IsShooting { get; private set; }
        public float HorizontalMove { get; private set; }

        public void Update(float deltaTime)
        {
            IsShooting = Input.GetMouseButton(0);
            HorizontalMove = Input.GetAxis("Horizontal");
        }

        public void Reset()
        {
            IsShooting = false;
            HorizontalMove = 0f;
        }
    }
}