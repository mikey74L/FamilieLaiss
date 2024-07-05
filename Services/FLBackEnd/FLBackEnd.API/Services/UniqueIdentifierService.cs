using DomainHelper.Interfaces;
using FLBackEnd.API.Interfaces;
using FLBackEnd.Domain.Entities;

namespace FLBackEnd.API.Services;

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