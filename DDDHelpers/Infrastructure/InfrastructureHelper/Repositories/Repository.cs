using DomainHelper.AbstractClasses;
using DomainHelper.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace InfrastructureHelper.Repositories;

public class Repository<TEntity> : ReadRepository<TEntity>, iRepository<TEntity> where TEntity : EntityBase
{
    #region C'tor
    public Repository(DbContext context) : base(context)
    {
    }
    #endregion

    #region Interface iRepository
    public ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity)
    {
        return _DBSet.AddAsync(entity);
    }

    public void Delete(TEntity entity)
    {
        //Entfernen der Entität aus dem Store
        _DBSet.Remove(entity);
    }

    public void Attach(TEntity entity)
    {
        _DBSet.Attach(entity);
    }

    public void Update(TEntity entity)
    {
        _DBSet.Update(entity);
    }
    #endregion

    #region Interface IDisposable
    public void Dispose()
    {
        _DBContext?.Dispose();
    }
    #endregion
}
