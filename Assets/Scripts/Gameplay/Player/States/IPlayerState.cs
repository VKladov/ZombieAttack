namespace Gameplay
{
    public interface IPlayerState
    {
        void Enter(Player player);
        IPlayerState Update(Player player, PlayerInput input, float deltaTime);
        void Exit(Player player);
    }
}