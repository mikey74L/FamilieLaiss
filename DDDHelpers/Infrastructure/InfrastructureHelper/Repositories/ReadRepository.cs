using DomainHelper.AbstractClasses;
using DomainHelper.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InfrastructureHelper.Repositories;

public class ReadRepository<TEntity> : iReadRepository<TEntity> where TEntity : EntityBase
{
    #region Protected Members
    protected readonly DbContext _DBContext;
    protected readonly DbSet<TEntity> _DBSet;
    protected readonly IProperty _KeyProperty;
    #endregion

    #region C'tor
    public ReadRepository(DbContext context)
    {
        //Übernehmen des DB-Context
        _DBContext = context ?? throw new ArgumentException(nameof(context));

        //Setzen des Entity-Sets
        _DBSet = _DBContext.Set<TEntity>();

        //Ermitteln der Key-Property
        _KeyProperty = context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties.FirstOrDefault();
    }
    #endregion

    #region Interface iReadRepository
    public Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicateWhere = null, string whereClause = null, string includeNav = null, string OrderBy = null, int? Take = null, int? Skip = null)
    {
        //Deklaration
        IQueryable<TEntity> Data;

        //Query zusammenbauen
        Data = _DBSet;
        if (!string.IsNullOrEmpty(whereClause) || predicateWhere != null)
        {
            //Where-Bedingung über Dynamic-Linq dazufügen
            if (!string.IsNullOrEmpty(whereClause)) Data = Data.Where(whereClause);
            if (predicateWhere != null) Data = Data.Where(predicateWhere);
        }
        if (!string.IsNullOrEmpty(OrderBy))
        {
            //OrderBy-Bedingung über Dynamic-Linq dazufügen
            Data = Data.OrderBy(OrderBy);
        }
        if (Take.HasValue)
        {
            //Den Take dazufügen wenn vorhanden
            Data = Data.Take(Take.Value);
        }
        if (Skip.HasValue)
        {
            //Den Skip hinzufügen wenn vorhanden
            Data = Data.Skip(Skip.Value);
        }
        if (!string.IsNullOrEmpty(includeNav))
        {
            //Die Expands bzw. Includes hinzufügen wenn vorhanden
            List<string> includeList = includeNav.Split(',').ToList();
            foreach (var Item in includeList)
            {
                Data = Data.Include(Item.Trim());
            }
        }

        //Return-Value und Predicate anwenden
        return Data.ToListAsync();
    }

    public Task<long> GetCount(Expression<Func<TEntity, bool>> predicateWhere = null, string whereClause = null)
    {
        //Deklaration
        IQueryable<TEntity> Data;

        //Query zusammenbauen
        Data = _DBSet;
        if (!string.IsNullOrEmpty(whereClause))
        {
            //Where-Bedingung über Dynamic-Linq dazufügen
            Data = Data.Where(whereClause);
        }

        //Return-Value und Predicate anwenden
        if (predicateWhere != null)
        {
            return Data.Where(predicateWhere).LongCountAsync();
        }
        else
        {
            return Data.LongCountAsync();
        }
    }

    public async Task<TEntity> GetOneAsync(object id, string includeNav = "")
    {
        //Deklaration
        IQueryable<TEntity> Data;
        TEntity SingleValue;

        //Query zusammenbauen
        Data = _DBSet;
        if (!string.IsNullOrEmpty(includeNav))
        {
            List<string> includeList = includeNav.Split(',').ToList();
            foreach (var Item in includeList)
            {
                Data = Data.Include(Item);
            }
        }

        //Single Value ermitteln
        if (_KeyProperty.ClrType == typeof(string))
        {
            SingleValue = await Data.FirstOrDefaultAsync(e => EF.Property<string>(e, _KeyProperty.Name) == Convert.ToString(id));
        }
        else if (_KeyProperty.ClrType == typeof(long))
        {
            SingleValue = await Data.FirstOrDefaultAsync(e => EF.Property<long>(e, _KeyProperty.Name) == Convert.ToInt64(id));
        }
        else if (_KeyProperty.ClrType == typeof(int))
        {
            SingleValue = await Data.FirstOrDefaultAsync(e => EF.Property<int>(e, _KeyProperty.Name) == Convert.ToInt32(id));
        }
        else
        {
            SingleValue = await Data.FirstOrDefaultAsync(e => EF.Property<Int16>(e, _KeyProperty.Name) == Convert.ToInt16(id));
        }

        //Return-Value
        return SingleValue;
    }
    #endregion
}
