namespace Domain.Interfaces
{
    public interface ICachingService<T>
    {
        void Set(object key, DateTime validUntil, T id);
        bool TryGetValue(object id, out T post);
    }
}
