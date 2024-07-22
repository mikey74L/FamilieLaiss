using DomainHelper.Interfaces;
using Upload.API.Interfaces;
using Upload.Domain.Entities;

namespace Upload.API.Services;

public class UniqueIdentifierGeneratorService(iUnitOfWork unitOfWork) : IUniqueIdentifierGenerator
{
    #region IUniqueIdentifierGenerator

    public async Task<long> GetNextUploadIdAsync()
    {
        var repo = unitOfWork.GetRepository<UploadIdentifier>();

        var newEntity = new UploadIdentifier
        {
            PseudoText = "Pseudo"
        };

        await repo.AddAsync(newEntity);

        await unitOfWork.SaveChangesAsync();

        return newEntity.Id;
    }

    #endregion
}