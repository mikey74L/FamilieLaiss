using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainHelper.Interfaces
{
    public interface iEntityBase
    {
        Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams);
    }

    public interface IEntityCreation : iEntityBase
    {
        void SetCreateDate();

        DateTimeOffset? CreateDate { get; }
    }

    public interface iEntityModify
    {
        void SetChangeDate();

        Task EntityModifiedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams);

        DateTimeOffset? ChangeDate { get; }
    }

    public interface iEntityDeleted
    {
        Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams);
    }
}
