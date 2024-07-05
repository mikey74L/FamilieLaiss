using DomainHelper.Interfaces;
using Upload.API.Interfaces;
using Upload.Domain.Entities;

namespace Upload.API.Services;

public class UniqueIdentifierGeneratorService(iUnitOfWork unitOfWork) : IUniqueIdentifierGenerator
{
    #region IUniqueIdentifierGenerator
    public async Task<long> GetNextUploadIDAsync()
    {
        //Ermitteln des Repo
        var repo = unitOfWork.GetRepository<UploadIdentifier>();

        //Neue Entity erzeugen
        var newEntity = new UploadIdentifier();
        newEntity.PseudoText = "Pseudo";

        //Entity hinzufügen
        await repo.AddAsync(newEntity);

        //Speichern der Entity um die ID zu generieren
        await unitOfWork.SaveChangesAsync();

        //Zurückliefern des Wertes
        return newEntity.Id;
    }
    #endregion
}