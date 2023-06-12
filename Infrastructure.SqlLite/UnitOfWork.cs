using Domain.Interfaces;

namespace Infrastructure.SqlLite
{
    internal class UnitOfWork : IUnitOfWork
    {
        public IPostRepository PostRepository => throw new NotImplementedException();

        public Task EndTransaction()
        {
            throw new NotImplementedException();
        }

        public Task StartTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
