namespace Utils
{
    public interface IPoolableObject
    {
        bool InUse { get; }
        void Reset();
    }
}