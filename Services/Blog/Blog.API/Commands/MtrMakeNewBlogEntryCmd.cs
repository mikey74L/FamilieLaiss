using Blog.API.GraphQL.Mutations.Blog;
using Blog.Domain.Entities;
using DomainHelper.Interfaces;
using MediatR;

namespace Blog.API.Commands.Category
{
    /// <summary>
    /// Mediatr Command for make new category entry
    /// </summary>
    public class MtrMakeNewBlogEntryCmd : IRequest<BlogEntry>
    {
        #region Properties
        /// <summary>
        /// Input data from GraphQL
        /// </summary>
        public AddBlogEntryInput InputData { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="inputData">Input data from GraphQL</param>
        public MtrMakeNewBlogEntryCmd(AddBlogEntryInput inputData)
        {
            InputData = inputData;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for Make new category entry command
    /// </summary>
    public class MtrMakeNewBlogEntryCmdHandler : IRequestHandler<MtrMakeNewBlogEntryCmd, BlogEntry>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<MtrMakeNewBlogEntryCmdHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">Unit of Work. Will be injected by DI-Container</param>
        /// <param name="logger">NLog-Logger. Will be injected by DI-Container</param>
        public MtrMakeNewBlogEntryCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrMakeNewBlogEntryCmdHandler> logger)
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
        public async Task<BlogEntry> Handle(MtrMakeNewBlogEntryCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for make new blog entry command was called for {@Input}", request.InputData);

            //Repository für Kategorie ermitteln
            _Logger.LogDebug("Get repository for blog entries");
            var Repository = _UnitOfWork.GetRepository<BlogEntry>();

            //Eine neue Kategorie erstellen
            _Logger.LogDebug("Adding new blog entry domain model to repository");
            var NewCategory = new BlogEntry(
                request.InputData.HeaderGerman,
                request.InputData.HeaderEnglish,
                request.InputData.TextGerman,
                request.InputData.TextEnglish);
            await Repository.AddAsync(NewCategory);

            //Speichern der Daten
            _Logger.LogDebug("Saving changes to data store");
            await _UnitOfWork.SaveChangesAsync();

            //Funktionsergebnis
            return NewCategory;
        }
        #endregion
    }
}
