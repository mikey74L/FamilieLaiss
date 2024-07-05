using DomainHelper.AbstractClasses;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelper.Interfaces
{
    public interface iRepository<TEntity> : iReadRepository<TEntity>, IDisposable where TEntity : EntityBase
    {
        /// <summary>
        /// Adding new entity to repository.
        /// </summary>
        /// <param name="entity">Entity that should be added.</param>
        ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity);

        /// <summary>
        /// Setting a entity to state unchanged and tracking changes
        /// </summary>
        /// <param name="entity"></param>
        void Attach(TEntity entity);

        /// <summary>
        /// Setting an entity to state modified
        /// </summary>
        /// <param name="entity">Entity that should be updated.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Delete entity from repository.
        /// </summary>
        /// <param name="entity">Entity that should be deleted.</param>
        void Delete(TEntity entity);
    }
}
