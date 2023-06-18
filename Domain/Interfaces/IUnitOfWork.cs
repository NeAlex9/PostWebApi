namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IPostRepository PostRepository { get; }
        Task SaveChanges(CancellationToken cancellationToken);
    }
}
