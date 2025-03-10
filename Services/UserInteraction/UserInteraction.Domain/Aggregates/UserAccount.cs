using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace UserInteraction.Domain.Aggregates;

public class UserAccount : EntityModify<string>
{
    #region Private Members
    private readonly ILazyLoader _LazyLoader;
    #endregion

    #region Properties
    /// <summary>
    /// Username for the user
    /// </summary>
    [Required]
    [GraphQLDescription("Username for the user")]
    public string UserName { get; private set; }

    /// <summary>
    /// The interaction info this user account is linked to
    /// </summary>
    [GraphQLDescription("The interaction info this user account is linked to")]
    public UserInteractionInfo UserInteractionInfo { get; private set; } = default!;
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor (Called by GraphQl)
    /// </summary>
    private UserAccount()
    {
    }

    /// <summary>
    /// C'tor (Called by EF.Core)
    /// </summary>
    /// <param name="lazyLoader">The EF.Core lazy loader. Will be injected by DI-Container</param>
    private UserAccount(ILazyLoader lazyLoader)
    {
        _LazyLoader = lazyLoader;
    }

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">Identifier for user account</param>
    /// <param name="userName">Username for the user</param>
    public UserAccount(string id, string userName)
    {
        //Überprüfen ob ein User-Name vorhanden ist
        if (string.IsNullOrEmpty(userName)) throw new DomainException("Username is required");

        //Übernehmen der Werte
        Id = id;
        UserName = userName;
    }
    #endregion

    #region Called from Change-Tracker
    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    public override async Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        //Laden der Werte wenn noch nicht geschehen
        await _LazyLoader.LoadAsync(this, navigationName: nameof(UserInteractionInfo));

        //Aufrufen der Delete-Routine für alle zugehörigen Ratings
        await UserInteractionInfo.EntityDeletedAsync(dbContext, dictContextParams);
    }

    public override Task EntityModifiedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        //Ergebnis zurückliefern
        return Task.CompletedTask;
    }
    #endregion
}
