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
    /// Mailkit - Service - Class
    /// </summary>
    public class MailKitService: iMailSender
    {
        #region Private Members
        private readonly AppSettings _AppSettings;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="appSettings">App-Settings. Will be injected by DI-Container</param>
        public MailKitService(IOptions<AppSettings> appSettings)
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
            //Einen neuen SMTP-Client initialisieren für den Zugriff auf Mailserver
            SmtpClient client = new SmtpClient();

            //Mit dem SMTP-Server verbinden
            await client.ConnectAsync(_AppSettings.MailkitAdress, _AppSettings.MailkitPort, _AppSettings.MailkitUseSSL);

            //Da wir kein Token für eine OAUTH Autentifizierung haben entfernen wir dieses Protokoll
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            //Autentifizierung beim Server
            string MailkitUser = "";
            string MailkitPassword = "";
            switch (senderType)
            {
                case enMailSenderType.Account:
                    if (_AppSettings.MailkitUseSenderAdressAsUser)
                    {
                        MailkitUser = mailInfo.SenderAdress;
                    }
                    else
                    {
                        MailkitUser = _AppSettings.MailkitUserAccount;
                    }
                    MailkitPassword = _AppSettings.MailkitPasswordAccount;
                    break;
                case enMailSenderType.Administrator:
                    if (_AppSettings.MailkitUseSenderAdressAsUser)
                    {
                        MailkitUser = mailInfo.SenderAdress;
                    }
                    else
                    {
                        MailkitUser = _AppSettings.MailkitUserAdministrator;
                    }
                    MailkitPassword = _AppSettings.MailkitPasswordAdministrator;
                    break;
                case enMailSenderType.Notification:
                    if (_AppSettings.MailkitUseSenderAdressAsUser)
                    {
                        MailkitUser = mailInfo.SenderAdress;
                    }
                    else
                    {
                        MailkitUser = _AppSettings.MailkitUserNotification;
                    }
                    MailkitPassword = _AppSettings.MailkitPasswordNotification;
                    break;
                case enMailSenderType.Support:
                    if (_AppSettings.MailkitUseSenderAdressAsUser)
                    {
                        MailkitUser = mailInfo.SenderAdress;
                    }
                    else
                    {
                        MailkitUser = _AppSettings.MailkitUserSupport;
                    }
                    MailkitPassword = _AppSettings.MailkitPasswordSupport;
                    break;
            }
            await client.AuthenticateAsync(MailkitUser, MailkitPassword);

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
