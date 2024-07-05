using DomainHelper.AbstractClasses;
using System;
using System.Threading.Tasks;

namespace DomainHelper.Interfaces
{
    public interface iUnitOfWork : IDisposable
    {
        iReadRepository<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : EntityBase;

        iRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase;

        Task<int> SaveChangesAsync();

        void AddContextParameter(string key, object value);
    }
}
