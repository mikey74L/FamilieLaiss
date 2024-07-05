using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using System.Threading.Tasks;
using Upload.Domain.DomainEvents.UploadPortrait;

namespace Upload.Domain.Entities;

public class UploadPortrait : EntityCreation<long>
{
    #region Properties
    /// <summary>
    /// Username this portrait picture belongs to
    /// </summary>
    public string UserName { get; private set; } = string.Empty;
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor (called by EF.Core)
    /// </summary>
    private UploadPortrait()
    {

    }

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="ID">Identifier for upload portrait</param>
    /// <param name="userName">The username this portrait picture belongs to</param>
    public UploadPortrait(long id, string userName)
    {
        //Überprüfen ob ein Benutzername vorhanden ist
        if (string.IsNullOrEmpty(userName)) throw new DomainException("A user name is needed");

        //Übernehmen der Werte
        Id = id;
        UserName = userName;
    }
    #endregion

    #region Change-Tracker Aufrufe
    public override Task EntityAddedAsync()
    {
        AddDomainEvent(new DomainEventUploadPortraitCreated(Id, UserName));

        return Task.CompletedTask;
    }

    public override Task EntityDeletedAsync()
    {
        AddDomainEvent(new DomainEventUploadPortraitDeleted(Id));

        return Task.CompletedTask;
    }
    #endregion
}
