using Catalog.DTO.CategoryValue;
using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using Mapster;
using MediatR;

namespace Catalog.API.Mediator.Commands.CategoryValue
{
    /// <summary>
    /// Mediatr Command for update category value
    /// </summary>
    public class MtrUpdateCategoryValueCmd : IRequest<CategoryValueDTO>
    {
        #region Properties
        /// <summary>
        /// Input data
        /// </summary>
        public required CategoryValueUpdateDTO InputData { get; init; }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for update category value
    /// </summary>
    public class MtrUpdateCategoryValueCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrUpdateCategoryValueCmdHandler> logger) : IRequestHandler<MtrUpdateCategoryValueCmd, CategoryValueDTO>
    {
        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task<CategoryValueDTO> Handle(MtrUpdateCategoryValueCmd request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Mediatr-Handler for update category value command was called for {@Input}", request.InputData);

            logger.LogDebug("Get repository for category value");
            var repository = unitOfWork.GetRepository<Domain.Aggregates.CategoryValue>();

            logger.LogDebug("Get find model to update in store");
            var modelToUpdate = await repository.GetOneAsync(request.InputData.Id);
            if (modelToUpdate == null)
            {
                throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find category value with id = {request.InputData.Id}");
            }

            logger.LogDebug("Update category value data");
            modelToUpdate.Update(request.InputData.NameGerman, request.InputData.NameEnglish);
            repository.Update(modelToUpdate);

            logger.LogDebug("Saving changes to data store");
            await unitOfWork.SaveChangesAsync();

            logger.LogDebug("Mapping with mapster");
            var modelToUpdateMapped = modelToUpdate.Adapt<CategoryValueDTO>();

            return modelToUpdateMapped;
        }
        #endregion
    }
}
