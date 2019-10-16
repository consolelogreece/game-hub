namespace Caching
{
    public interface ICache<T>
    {
        void Set(string key, T value);

        T Get(string key);
    }
}