using Blog.API.GraphQL.Mutations.Blog;
using Blog.Domain.Entities;
using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;

namespace Blog.API.Commands.Category
{
    /// <summary>
    /// Mediatr Command for update blog entry 
    /// </summary>
    public class MtrUpdateBlogEntryCmd : IRequest<BlogEntry>
    {
        #region Properties
        /// <summary>
        /// Input data from GraphQL
        /// </summary>
        public UpdateBlogEntryInput InputData { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="inputData">Input data from GraphQL</param>
        public MtrUpdateBlogEntryCmd(UpdateBlogEntryInput inputData)
        {
            InputData = inputData;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for update blog entry
    /// </summary>
    public class MtrUpdateBlogEntryCmdHandler : IRequestHandler<MtrUpdateBlogEntryCmd, BlogEntry>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<MtrUpdateBlogEntryCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">Unit of work. Will be injected by DI-Container</param>
        /// <param name="logger">NLog-Logger. Will be injected by DI-Container</param>
        public MtrUpdateBlogEntryCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrUpdateBlogEntryCmdHandler> logger)
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
        public async Task<BlogEntry> Handle(MtrUpdateBlogEntryCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for update blog entry command was called for {@Input}", request.InputData);

            //Repository für BlogEntry ermitteln
            _Logger.LogDebug("Get repository for blogs");
            var Repository = _UnitOfWork.GetRepository<BlogEntry>();

            //Ermitteln des bisherigen Models
            _Logger.LogDebug("Find model to update in store");
            var ModelToUpdate = await Repository.GetOneAsync(request.InputData.ID);
            if (ModelToUpdate == null)
            {
                _Logger.LogError("Could not find blog entry with {ID}", request.InputData.ID);
                throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find blog entry with id = {request.InputData.ID}");
            }

            //Kategoriedaten aktualisieren
            _Logger.LogDebug("Update blog entry data");
            ModelToUpdate.Update(request.InputData.HeaderGerman, request.InputData.HeaderEnglish, request.InputData.TextGerman, request.InputData.TextEnglish);
            Repository.Update(ModelToUpdate);

            //Speichern der Daten
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();

            return ModelToUpdate;
        }
        #endregion
    }
}
