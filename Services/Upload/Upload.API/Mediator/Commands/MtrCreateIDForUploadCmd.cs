using MediatR;
using Upload.API.Interfaces;

namespace Upload.API.Mediator.Commands
{
    /// <summary>
    /// Mediatr Command for Creating a unique UploadID
    /// </summary>
    public class MtrCreateIDForUploadCmd : IRequest<long>
    {
    }

    /// <summary>
    /// Mediatr Command-Handler for Creating a unique UploadID
    /// </summary>
    public class MtrCreateIDForUploadCmdHandler(ILogger<MtrCreateIDForUploadCmdHandler> logger,
                                              IUniqueIdentifierGenerator uniqueIDGenerator) : IRequestHandler<MtrCreateIDForUploadCmd, long>
    {
        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task<long> Handle(MtrCreateIDForUploadCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            logger.LogInformation("Mediatr-Handler for create unique upload ID command was called");

            //Ergebnis zurückliefern
            return await uniqueIDGenerator.GetNextUploadIDAsync();
        }
        #endregion
    }
}
