using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace UserInteraction.API.Commands
{
    /// <summary>
    /// Mediatr Command for reset password request
    /// </summary>
    public class MtrResetPasswordRequestCmd : IRequest
    {
        #region Public Properties
        /// <summary>
        /// The user name
        /// </summary>
        public string UserName { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="userName">The user name</param>
        public MtrResetPasswordRequestCmd(string userName)
        {
            UserName = userName;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for reset password request
    /// </summary>
    public class MtrResetPasswordRequestCmdHandler : IRequestHandler<MtrResetPasswordRequestCmd>
    {
        #region Private Members
        private readonly ILogger<MtrResetPasswordRequestCmdHandler> _Logger;
        private readonly IBus _Masstransit;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="logger">Logger. Injected by DI</param>
        /// <param name="massTransit">Masstransit. Injected by DI</param>
        public MtrResetPasswordRequestCmdHandler(ILogger<MtrResetPasswordRequestCmdHandler> logger, IBus massTransit)
        {
            //Übernehmen der injected Classes
            _Logger = logger;
            _Masstransit = massTransit;
        }
        #endregion

        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task Handle(MtrResetPasswordRequestCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for reset password request command was called with following parameters:");
            _Logger.LogDebug($"{request.UserName.Substring(1, 3)}");
            _Logger.LogDebug("Other properties are not logged because ov DSGVO");

            //Send command over service bus
            _Logger.LogDebug("Send command over service bus");
            var Command = new MtrResetPasswordRequestCmd(request.UserName);
            await _Masstransit.Send(Command);
        }
        #endregion
    }
}
