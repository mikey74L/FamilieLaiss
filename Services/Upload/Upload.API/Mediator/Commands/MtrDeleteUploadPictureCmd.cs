﻿using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;
using Upload.API.GraphQL.Mutations.UploadPicture;
using Upload.Domain.Entities;

namespace Upload.API.Mediator.Commands
{
    /// <summary>
    /// Mediatr Command for delete upload picture
    /// </summary>
    public class MtrDeleteUploadPictureCmd : IRequest<UploadPicture>
    {
        #region Properties
        /// <summary>
        /// The Model with the input data
        /// </summary>
        public DeleteUploadPictureInput Model { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="model">The model with the input data</param>
        public MtrDeleteUploadPictureCmd(DeleteUploadPictureInput model)
        {
            Model = model;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for delete upload picture
    /// </summary>
    public class MtrDeleteUploadPictureCmdHandler : IRequestHandler<MtrDeleteUploadPictureCmd, UploadPicture>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<MtrDeleteUploadPictureCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrDeleteUploadPictureCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteUploadPictureCmdHandler> logger)
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
        public async Task<UploadPicture> Handle(MtrDeleteUploadPictureCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for delete upload picture command was called for {@Model}", request.Model);

            //Repository ermitteln
            _Logger.LogDebug("Get repository for UploadPicture");
            var Repository = _UnitOfWork.GetRepository<UploadPicture>();

            //Ermitteln des zu löschenden Items aus dem Store
            _Logger.LogDebug("Get model to delete in store");
            var ModelToDelete = await Repository.GetOneAsync(request.Model.Id);
            if (ModelToDelete == null)
            {
                _Logger.LogError("Could not find upload picture with id {ID}", request.Model.Id);
                throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find upload picture with id = {request.Model.Id}");
            }

            //Löschen des Items
            _Logger.LogDebug("Delete entity from data store");
            Repository.Delete(ModelToDelete);

            //Speichern der Änderungen
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();

            return ModelToDelete;
        }
        #endregion
    }
}
