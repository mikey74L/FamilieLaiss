using DomainHelper.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainHelper.Interfaces;

/// <summary>
/// Interface for a read repository
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
/// <typeparam name="TIDType">The tpye for the id of the entity</typeparam>
public interface iReadRepository<TEntity> where TEntity : EntityBase
{
    /// <summary>
    /// Get all entities from repository that matching the given parameters
    /// </summary>
    /// <param name="predicateWhere">An additional predicate to filter data.</param>
    /// <param name="whereClause">An additional where clause to filter the result.</param>
    /// <param name="includeNav">An additional navigation instruction to expand navigation properties.</param>
    /// <param name="OrderBy">An additional order by instruction.</param>
    /// <param name="Take">An additional take ammount. When set to less than 0 take is not active.</param>
    /// <param name="Skip">An addtional skip ammount. When set to less than 0 skip is not active.</param>
    /// <returns>All entites from repository as <see cref="List<typeparamref name="TEntity"/>"/>.</returns>
    Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicateWhere = null, string whereClause = null, string includeNav = null, string OrderBy = null, int? Take = null, int? Skip = null);

    /// <summary>
    /// Gets the count of elements
    /// </summary>
    /// <param name="predicate">An additional predicate to filter data.</param>
    /// <param name="whereClause">An additional where clause to filter the result.</param>
    /// <returns>The count of elements</returns>
    Task<long> GetCount(Expression<Func<TEntity, bool>> predicate = null, string whereClause = null);

    /// <summary>
    /// Get a single entity from repository.
    /// </summary>
    /// <param name="id">The unique id (primary key) for the entity to get.</param>
    /// <param name="includeNav">An additional navigation instruction to expand navigation properties.</param>
    /// <returns>The entity that is equal the searched id.</returns>
    Task<TEntity> GetOneAsync(object id, string includeNav = "");
}
