using Catalog.DTO.CategoryValue;
using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using Mapster;
using MediatR;

namespace Catalog.API.Mediator.Commands.CategoryValue
{
    /// <summary>
    /// Mediatr Command for make new category value entry
    /// </summary>
    public class MtrMakeNewCategoryValueCmd() : IRequest<CategoryValueDTO>
    {
        #region Properties
        public required CategoryValueCreateDTO InputData { get; init; }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for Make new category value entry command
    /// </summary>
    public class MtrMakeNewCategoryValueCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrMakeNewCategoryValueCmdHandler> logger) : IRequestHandler<MtrMakeNewCategoryValueCmd, CategoryValueDTO>
    {
        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task<CategoryValueDTO> Handle(MtrMakeNewCategoryValueCmd request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Mediatr-Handler for make new category value entry command was called for {@Input}", request.InputData);

            logger.LogDebug("Get repository for category");
            var repository = unitOfWork.GetRepository<Domain.Aggregates.Category>();

            logger.LogDebug("Get category from store");
            var categoryEntity = await repository.GetOneAsync(request.InputData.CategoryId);
            if (categoryEntity == null)
            {
                logger.LogError("Could not find category with {ID}", request.InputData.CategoryId);
                throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find category with id = {request.InputData.CategoryId}");
            }

            logger.LogDebug("Adding category value to category");
            var addedEntity = categoryEntity.AddCategoryValue(request.InputData.NameGerman, request.InputData.NameEnglish);

            logger.LogDebug("Saving changes to data store");
            await unitOfWork.SaveChangesAsync();

            logger.LogDebug("Mapping with mapster");
            var addedEntityMapped = addedEntity.Adapt<CategoryValueDTO>();

            return addedEntityMapped;
        }
        #endregion
    }
}
