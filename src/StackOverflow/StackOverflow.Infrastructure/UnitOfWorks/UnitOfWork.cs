using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StackOverflow.Domain;
using StackOverflow.Infrastructure.Utilities;

namespace StackOverflow.Infrastructure.UnitOfWorks
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        protected IAdoNetUtility AdoNetUtility { get;}

        protected UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
            AdoNetUtility = new AdoNetUtility(_dbContext.Database.GetDbConnection());
        }

        public void Dispose() => _dbContext?.Dispose();
        public ValueTask DisposeAsync() => _dbContext.DisposeAsync();
        public void Save() => _dbContext?.SaveChanges();
        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
        public DbTransaction BeginTransaction()
        {
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContext.Database.BeginTransaction();
            }
            return _dbContext.Database.CurrentTransaction!.GetDbTransaction();
        }
    }
}
