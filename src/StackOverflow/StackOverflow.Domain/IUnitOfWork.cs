using System.Data.Common;

namespace StackOverflow.Domain
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        void Save();
        Task SaveAsync();
        DbTransaction BeginTransaction();
    }
}
