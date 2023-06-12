namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IPostRepository PostRepository { get; }
        Task StartTransaction();
        Task EndTransaction();
    }
}
