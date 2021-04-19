namespace Core.ObjectPool
{
    public interface IPoolable
    {
        bool IsAvailable { get; set; }

        void OnPooled();
        void OnRecycled();
    }
}