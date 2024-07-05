using FamilieLaissSharedObjects.Enums;
using Mail.API.Interfaces;
using Mail.API.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace Mail.API.Services
{
    /// <summary>
    /// Mailtrap - Service - Class
    /// </summary>
    public class MailtrapService: iMailSender
    {
        #region Private Members
        private readonly AppSettings _AppSettings;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="appSettings">App-Settings will be injected by DI-Container</param>
        public MailtrapService(IOptions<AppSettings> appSettings)
        {
            _AppSettings = appSettings.Value;
        }
        #endregion

        #region Interface iMailSender
        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="senderType">The sender type</param>
        /// <param name="mailInfo">The mail data</param>
        public async Task SendEmailAsync(enMailSenderType senderType, Mail.Domain.Entities.Mail mailInfo)
        {
            //Einen neuen SMTP-Client initialisieren für den Zugriff auf mailtrap
            SmtpClient client = new SmtpClient();

            //Mit dem SMTP-Server verbinden
            await client.ConnectAsync(_AppSettings.MailtrapAdress, _AppSettings.MailtrapPort, false);

            //Da wir kein Token für eine OAUTH Autentifizierung haben entfernen wir dieses Protokoll
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            //Autentifizierung beim Server
            await client.AuthenticateAsync(_AppSettings.MailtrapUsername, _AppSettings.MailtrapPassword);

            //Erstellen der Mime-Message
            var message = new MimeMessage();

            //Sender-Adresse festlegen
            message.From.Add(new MailboxAddress(mailInfo.SenderName, mailInfo.SenderAdress));

            //Empfänger-Adresse festlegen
            message.To.Add(new MailboxAddress(mailInfo.ReceiverName, mailInfo.ReceiverAdress));

            //Das Subject zuweisen
            message.Subject = mailInfo.Subject;

            //Den Body zuweisen
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = mailInfo.Body
            };

            //Versenden der Mail Async
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        #endregion
    }
}
