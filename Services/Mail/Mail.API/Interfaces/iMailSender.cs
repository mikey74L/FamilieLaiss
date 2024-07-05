using FamilieLaissSharedObjects.Enums;
using System.Threading.Tasks;

namespace Mail.API.Interfaces
{
    /// <summary>
    /// Interface for sending mails 
    /// </summary>
    public interface iMailSender
    {
        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="senderType">The sender type</param>
        /// <param name="mailData">The mail data</param>
        Task SendEmailAsync(enMailSenderType senderType, Mail.Domain.Entities.Mail mailData);
    }
}
