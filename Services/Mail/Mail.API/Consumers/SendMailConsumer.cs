using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using Mail.API.Interfaces;
using Mail.API.Models;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Mail.API.Consumers
{
    /// <summary>
    /// MassTransit consumer for "SendMail"-Command
    /// </summary>
    public class SendMailConsumer : IConsumer<iSendMailCmd>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly Func<string, iMailSender> _MailSenderServiceAccessor;
        private readonly AppSettings _AppSettings;
        private readonly ILogger<SendMailConsumer> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI-Container.</param>
        /// <param name="mailSenderServiceAccessor">The service accessor for the mail client type. Will be injected by DI-Container</param>
        /// <param name="appSettings">The current App-Settings. Will be injected by DI-Container.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public SendMailConsumer(iUnitOfWork unitOfWork, Func<string, iMailSender> mailSenderServiceAccessor, IOptions<AppSettings> appSettings,
            ILogger<SendMailConsumer> logger)
        {
            //Übernehmen der Injected Objects
            _UnitOfWork = unitOfWork;
            _MailSenderServiceAccessor = mailSenderServiceAccessor;
            _AppSettings = appSettings.Value;
            _Logger = logger;
        }
        #endregion

        #region Private Methods
        //Ermittelt den benötigten Mail-Sender-Service aus dem IOC-Container
        //je nach dem ob sich die Anwendung im Debug-Mode oder in der Produktion befindet
        private iMailSender GetMailSenderService()
        {
            return _MailSenderServiceAccessor(_AppSettings.MailType);
        }
        #endregion

        #region Interface IConsumer
        /// <summary>
        /// Would be called from Masstransit
        /// </summary>
        /// <param name="context">The context for this event</param>
        /// <returns>Task</returns>
        public async Task Consume(ConsumeContext<iSendMailCmd> context)
        {
            //Ausgeben von Logging
            _Logger.LogInformation("Consumer für \"Send mail\" Command wurde aufgerufen: {@Message}", context.Message);
            _Logger.LogDebug($"Mail-Sender-Type: {context.Message.MailSenderType}");
            _Logger.LogDebug($"Receiver-Adress : {context.Message.ReceiverAdress}");
            _Logger.LogDebug($"Receiver-Name   : {context.Message.ReceiverName}");
            _Logger.LogDebug($"Subject         : {context.Message.Subject}");
            _Logger.LogDebug($"IsBodyHTML      : {context.Message.IsBodyHTML}");

            //Ermitteln des Repositories für die Nachricht
            _Logger.LogDebug("Get repository for mail");
            var RepositoryMail = _UnitOfWork.GetRepository<Mail.Domain.Entities.Mail>();

            //Ablegen der Mail in der Datenbank
            _Logger.LogDebug("Add new mail");
            var NewMail = new Mail.Domain.Entities.Mail(context.Message.MailSenderType, context.Message.ReceiverAdress,
                context.Message.ReceiverName, context.Message.Subject, context.Message.IsBodyHTML, context.Message.Body);
            await RepositoryMail.AddAsync(NewMail);

            //Speichern der Änderungen
            _Logger.LogDebug("Saving changes to store");
            await _UnitOfWork.SaveChangesAsync();

            //Ermitteln des Mail-Sender-Service für die Environment
            _Logger.LogDebug("Get mail sender service");
            var MailSender = GetMailSenderService();

            //Versenden der Mail
            _Logger.LogDebug("Send mail with mail sender service");
            await MailSender.SendEmailAsync(context.Message.MailSenderType, NewMail);
        }
        #endregion
    }
}
