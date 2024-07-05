using Blog.API.GraphQL.Mutations.Blog;
using Blog.Domain.Entities;
using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;

namespace Blog.API.Commands.Category
{
    /// <summary>
    /// Mediatr Command for delete blog entry
    /// </summary>
    public class MtrDeleteBlogEntryCmd : IRequest<Blog.Domain.Entities.BlogEntry>
    {
        #region Properties
        /// <summary>
        /// Input data from GraphQL
        /// </summary>
        public DeleteBlogEntryInput Input { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="input">Input data from GraphQL</param>
        public MtrDeleteBlogEntryCmd(DeleteBlogEntryInput input)
        {
            Input = input;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for delete blog entry
    /// </summary>
    public class MtrDeleteBlogEntryCmdHandler : IRequestHandler<MtrDeleteBlogEntryCmd, Blog.Domain.Entities.BlogEntry>
    {
        #region Private Members
        private readonly ILogger<MtrDeleteBlogEntryCmdHandler> _Logger;
        private readonly iUnitOfWork _UnitOfWork;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">Unit of work. Will be injected by DI-Container</param>
        /// <param name="logger">NLog-Logger. Will be injected by DI-Container</param>
        public MtrDeleteBlogEntryCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrDeleteBlogEntryCmdHandler> logger)
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
        public async Task<Blog.Domain.Entities.BlogEntry> Handle(MtrDeleteBlogEntryCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for delete category command was called for {@Input}", request.Input);

            //Repository für Kategorie ermitteln
            _Logger.LogDebug("Get repository for blog entry");
            var Repository = _UnitOfWork.GetRepository<BlogEntry>();

            //Ermitteln des bisherigen Models
            _Logger.LogDebug("Find model to delete in store");
            var ModelToDelete = await Repository.GetOneAsync(request.Input.ID);
            if (ModelToDelete == null)
            {
                _Logger.LogError("Could not find blog entry with id {ID}", request.Input.ID);
                throw new DomainException(DomainExceptionType.NoDataFound, $"Could not find blog entry with id = {request.Input.ID}");
            }

            //Eine neue Kategorie erstellen
            _Logger.LogDebug("Delete blog entry domain model from store");
            Repository.Delete(ModelToDelete);

            //Speichern der Daten
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();

            return ModelToDelete;
        }
        #endregion
    }
}
