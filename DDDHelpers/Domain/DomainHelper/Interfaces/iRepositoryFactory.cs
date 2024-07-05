using DomainHelper.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainHelper.Interfaces
{
    public interface iRepositoryFactory
    {
        iReadRepository<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : EntityBase;

        iRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase;
    }
}
