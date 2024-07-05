using DomainHelper.Interfaces;
using FamilieLaissSharedObjects.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Upload.API.Commands;
using Upload.API.GraphQL.Mutations.UploadPicture;
using Upload.API.GraphQL.Mutations.UploadVideo;
using Upload.Domain.Entities;

namespace Upload.API.Mediator.Commands
{
    /// <summary>
    /// Mediatr Command for delete all upload videos
    /// </summary>
    public class MtrDeleteAllUploadVideosCmd : IRequest<int>
    {
    }

    /// <summary>
    /// Mediatr Command-Handler for delete all upload videos
    /// </summary>
    public class MtrDeleteAllUploadVideosCmdHandler : IRequestHandler<MtrDeleteAllUploadVideosCmd, int>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly IMediator _Mediator;
        private readonly ILogger<MtrDeleteAllUploadVideosCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="mediator">The mediator. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrDeleteAllUploadVideosCmdHandler(iUnitOfWork unitOfWork, IMediator mediator, ILogger<MtrDeleteAllUploadVideosCmdHandler> logger)
        {
            _UnitOfWork = unitOfWork;
            _Mediator = mediator;
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
        public async Task<int> Handle(MtrDeleteAllUploadVideosCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for delete all upload videos command was called");

            //Repository ermitteln
            _Logger.LogDebug("Get repository for UploadVideo");
            var Repository = _UnitOfWork.GetRepository<UploadVideo>();

            //Ermitteln aller zu löschenden Upload-Videos
            _Logger.LogDebug($"Get all upload videos to delete");
            var ItemsToDelete = await Repository.GetAll(x => x.Status == EnumUploadStatus.Converted);

            //Durch die Liste durchgehen und für jedes Item ein Command absetzen
            foreach (var Item in ItemsToDelete)
            {
                _Logger.LogDebug($"Send command to delete upload video with id = {Item.Id} over mediator");
                await _Mediator.Send(new MtrDeleteUploadVideoCmd(new DeleteUploadVideoInput() { Id = Item.Id }));
            }

            return ItemsToDelete.Count;
        }
        #endregion
    }
}
