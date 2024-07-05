using FamilieLaissSharedObjects.Enums;
using Mail.API.Interfaces;
using Mail.API.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Mail.API.Services
{
    /// <summary>
    /// Sendgrid - Service - Class
    /// </summary>
    public class SendGridService: iMailSender
    {
        #region Private Members
        private readonly AppSettings _AppSettings;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="appSettings">App-Settings. Will be injected by DI-Container</param>
        public SendGridService(IOptions<AppSettings> appSettings)
        {
            _AppSettings = appSettings.Value;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="senderType">The sender type</param>
        /// <param name="mailInfo">The mail data</param>
        public async Task SendEmailAsync(enMailSenderType senderType, Mail.Domain.Entities.Mail mailInfo)
        {
            //Eine neue Send-Grid Message erzeugen
            var myMessage = new SendGridMessage();

            //Sender-Adresse festlegen
            myMessage.SetFrom(mailInfo.SenderAdress, mailInfo.SenderName);

            //Empfänger festlegen
            myMessage.AddTo(mailInfo.ReceiverAdress, mailInfo.ReceiverName);

            //Den Betreff festlegen
            myMessage.Subject = mailInfo.Subject;

            //Den Text der Mail als HTML-Content setzen
            myMessage.AddContent(MimeType.Html, mailInfo.Body);

            //Send-Grid-Credentials zur Anmeldung festlegen
            string APIKey = _AppSettings.SendGridAPIKey;

            //Einen Web-Transport für Send-Grid erzeugen
            SendGridClient client = new SendGridClient(APIKey);

            //Die eMail über Send-Grid versenden
            await client.SendEmailAsync(myMessage);
        }
        #endregion
    }
}
