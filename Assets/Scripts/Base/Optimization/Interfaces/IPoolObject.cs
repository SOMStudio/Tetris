namespace Base.Optimization.Interfaces
{
    interface IPoolObject
    {
        void OnPoolAdjusting(ObjectPool value);
        void OnPoolLiberation(ObjectPool value);
    }
}
