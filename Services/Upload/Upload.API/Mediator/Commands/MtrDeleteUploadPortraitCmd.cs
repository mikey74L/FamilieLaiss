using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;
using Upload.Domain.Entities;

namespace Upload.API.Mediator.Commands
{
    /// <summary>
    /// Mediatr Command for delete upload portrait
    /// </summary>
    public class MtrDeleteUploadPortraitCmd : IRequest
    {
        #region Properties
        /// <summary>
        /// ID for the upload portrait
        /// </summary>
        public string ID { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">ID for the upload portrait</param>
        public MtrDeleteUploadPortraitCmd(string id)
        {
            ID = id;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for delete upload portrait
    /// </summary>
    public class MtrDeleteUploadPortraitCmdHandler : IRequestHandler<MtrDeleteUploadPortraitCmd>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<MtrDeleteUploadPortraitCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrDeleteUploadPortraitCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteUploadPortraitCmdHandler> logger)
        {
            _UnitOfWork = unitOfWork;
            _Logger = logger;
        }
        #endregion

        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task Handle(MtrDeleteUploadPortraitCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for delete upload portrait command was called with following parameters:");
            _Logger.LogDebug($"ID: {request.ID}");

            //Repository ermitteln
            _Logger.LogDebug("Get repository for UploadPortrait");
            var Repository = _UnitOfWork.GetRepository<UploadPortrait>();

            //Ermitteln des zu ändernden Items aus dem Store
            _Logger.LogDebug($"Get upload portrait domain model with id = {request.ID} from data store");
            var ItemToChange = await Repository.GetOneAsync(request.ID);
            if (ItemToChange == null)
            {
                throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find upload portrait with id = {request.ID}");
            }

            //Löschen des Items
            _Logger.LogDebug("Delete entity from data store");
            Repository.Delete(ItemToChange);

            //Speichern der Änderungen
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
