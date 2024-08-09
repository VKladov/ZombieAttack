using UnityEngine;

namespace Gameplay
{
    public class MoveState : IPlayerState
    {
        public void Enter(Player player)
        {
            player.View.SetRunning(true);
        }

        public IPlayerState Update(Player player, PlayerInput input, float deltaTime)
        {
            var horizontalAxisInput = input.HorizontalMove;
            if (Mathf.Abs(horizontalAxisInput) > 0.1f)
            {
                player.View.SetDirection(horizontalAxisInput > 0 ? 1 : -1);
                player.View.SetHorizontalVelocity(horizontalAxisInput * player.Config.Speed);
            }
            else
            {
                return new IdleState();
            }

            return null;
        }

        public void Exit(Player player)
        {
            player.View.SetRunning(false);
        }
    }
}