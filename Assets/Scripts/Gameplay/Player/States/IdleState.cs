using UnityEngine;

namespace Gameplay
{
    public class IdleState : IPlayerState
    {
        public void Enter(Player player)
        {
        }

        public IPlayerState Update(Player player, PlayerInput input, float deltaTime)
        {
            if (Mathf.Abs(input.HorizontalMove) > 0.1f) return new MoveState();

            if (input.IsShooting && player.AmmoCount.Value > 0) return new ShootState();

            return null;
        }

        public void Exit(Player player)
        {
        }
    }
}