namespace Domain.Interfaces
{
    public interface ICachingService
    {
        void Set<T>(object key, DateTime validUntil, T id);
        bool TryGetValue<T>(object id, out T post);
    }
}
