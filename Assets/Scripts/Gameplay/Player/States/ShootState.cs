namespace Gameplay
{
    public class ShootState : IPlayerState
    {
        private float _shootCooldown;

        public void Enter(Player player)
        {
            player.View.SetShooting(true);
            if (player.AmmoCount.Value > 0 && _shootCooldown < 0)
            {
                player.Shoot();
                _shootCooldown = player.Config.ShootDelay;
            }
        }

        public IPlayerState Update(Player player, PlayerInput input, float deltaTime)
        {
            if (input.IsShooting)
            {
                if (player.AmmoCount.Value == 0) return new IdleState();

                if (_shootCooldown < 0f)
                {
                    player.Shoot();
                    _shootCooldown = player.Config.ShootDelay;
                }

                _shootCooldown -= deltaTime;
            }
            else
            {
                return new IdleState();
            }

            return null;
        }

        public void Exit(Player player)
        {
            player.View.SetShooting(false);
        }
    }
}