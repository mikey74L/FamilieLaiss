namespace Mail.API.Models
{
    /// <summary>
    /// App-Settings - Class
    /// </summary>
    public class AppSettings
    {
        #region RabbitMQ
        /// <summary>
        /// RabbitMQ Connection-String - Filename 
        /// </summary>
        public string RabbitMQConnection_FILE { get; set; }

        /// <summary>
        /// RabbitMQ Connection-String for CloudAMP
        /// </summary>
        public string RabbitMQConnection
        {
            get
            {
                return System.IO.File.ReadAllText(RabbitMQConnection_FILE);
            }
        }

        #region Endpoints
        /// <summary>
        /// Endpoint for Mail-Service
        /// </summary>
        public string Endpoint_MailService { get; set; }
        #endregion
        #endregion

        #region Postgres
        /// <summary>
        /// Postgres user - Filename for Secret
        /// </summary>
        public string PostgresUser_FILE { get; set; } = string.Empty;

        /// <summary>
        /// Postgres user
        /// </summary>
        public string PostgresUser
        {
            get
            {
                return System.IO.File.ReadAllText(PostgresUser_FILE);
            }
        }

        /// <summary>
        /// Postgres password - Filename for Secret
        /// </summary>
        public string PostgresPassword_FILE { get; set; } = string.Empty;

        /// <summary>
        /// Postgres password
        /// </summary>
        public string PostgresPassword
        {
            get
            {
                return System.IO.File.ReadAllText(PostgresPassword_FILE);
            }
        }

        /// <summary>
        /// Postgres - Port
        /// </summary>
        public int PostgresPort { get; set; }

        /// <summary>
        /// Postgres - Hostname or IP-Adress
        /// </summary>
        public string PostgresHost { get; set; } = string.Empty;

        /// <summary>
        /// Postgres - Databasename
        /// </summary>
        public string PostgresDatabase { get; set; } = string.Empty;

        /// <summary>
        /// Postgres - Use Multiplexing for faster connections
        /// </summary>
        public bool PostgresMultiplexing { get; set; }
        #endregion

        #region Mail
        #region Mailtrap
        /// <summary>
        /// URL for Mailtrap
        /// </summary>
        public string MailtrapAdress { get; set; }

        /// <summary>
        /// Portnumber
        /// </summary>
        public int MailtrapPort { get; set; }

        /// <summary>
        /// Username - Secrets Filename
        /// </summary>
        public string MailtrapUsername_FILE { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string MailtrapUsername
        {
            get
            {
                //return "";
                return System.IO.File.ReadAllText(MailtrapUsername_FILE);
            }
        }

        /// <summary>
        /// Password - Secrets Filename
        /// </summary>
        public string MailtrapPassword_FILE { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string MailtrapPassword
        {
            get
            {
                return System.IO.File.ReadAllText(MailtrapPassword_FILE);
            }
        }
        #endregion

        #region SendGrid
        /// <summary>
        /// API-Key - Secrets Filename
        /// </summary>
        public string SendGridAPIKey_FILE { get; set; }

        /// <summary>
        /// API-Key
        /// </summary>
        public string SendGridAPIKey
        {
            get
            {
                return System.IO.File.ReadAllText(SendGridAPIKey_FILE);
            }
        }
        #endregion

        #region Mailkit 
        /// <summary>
        /// URL for Mailserver
        /// </summary>
        public string MailkitAdress { get; set; }

        /// <summary>
        /// Portnumber
        /// </summary>
        public int MailkitPort { get; set; }

        /// <summary>
        /// Use SSL for sending mails
        /// </summary>
        public bool MailkitUseSSL { get; set; }

        /// <summary>
        /// Use the sender adress as username
        /// </summary>
        public bool MailkitUseSenderAdressAsUser { get; set; }

        /// <summary>
        /// User for Account-Mails - Secrets Filename
        /// </summary>
        public string MailkitUserAccount_FILE { get; set; }

        /// <summary>
        /// User for Account-Mails
        /// </summary>
        public string MailkitUserAccount
        {
            get
            {
                return System.IO.File.ReadAllText(MailkitUserAccount_FILE);
            }
        }

        /// <summary>
        /// Password for Account-Mails - Secrets Filename
        /// </summary>
        public string MailkitPasswordAccount_FILE { get; set; }

        /// <summary>
        /// Password for Account-Mails
        /// </summary>
        public string MailkitPasswordAccount
        {
            get
            {
                return System.IO.File.ReadAllText(MailkitPasswordAccount_FILE);
            }
        }

        /// <summary>
        /// User for Administrator-Mails - Secrets Filename
        /// </summary>
        public string MailkitUserAdministrator_FILE { get; set; }

        /// <summary>
        /// User for Administrator-Mails
        /// </summary>
        public string MailkitUserAdministrator
        {
            get
            {
                return System.IO.File.ReadAllText(MailkitUserAdministrator_FILE);
            }
        }

        /// <summary>
        /// Password for Administrator-Mails - Secrets Filename
        /// </summary>
        public string MailkitPasswordAdministrator_FILE { get; set; }

        /// <summary>
        /// Password for Administrator-Mails
        /// </summary>
        public string MailkitPasswordAdministrator
        {
            get
            {
                return System.IO.File.ReadAllText(MailkitPasswordAdministrator_FILE);
            }
        }

        /// <summary>
        /// User for Notification-Mails - Secrets Filename
        /// </summary>
        public string MailkitUserNotification_FILE { get; set; }

        /// <summary>
        /// User for Notification-Mails
        /// </summary>
        public string MailkitUserNotification
        {
            get
            {
                return System.IO.File.ReadAllText(MailkitUserNotification_FILE);
            }
        }

        /// <summary>
        /// Password for Notification-Mails - Secrets Filename
        /// </summary>
        public string MailkitPasswordNotification_FILE { get; set; }

        /// <summary>
        /// Password for Notification-Mails
        /// </summary>
        public string MailkitPasswordNotification
        {
            get
            {
                return System.IO.File.ReadAllText(MailkitPasswordNotification_FILE);
            }
        }

        /// <summary>
        /// User for Support-Mails - Secrets Filename
        /// </summary>
        public string MailkitUserSupport_FILE { get; set; }

        /// <summary>
        /// User for Support-Mails
        /// </summary>
        public string MailkitUserSupport
        {
            get
            {
                return System.IO.File.ReadAllText(MailkitUserSupport_FILE);
            }
        }

        /// <summary>
        /// Password for Support-Mails - Secrets Filename
        /// </summary>
        public string MailkitPasswordSupport_FILE { get; set; }

        /// <summary>
        /// Password for Support-Mails
        /// </summary>
        public string MailkitPasswordSupport
        {
            get
            {
                return System.IO.File.ReadAllText(MailkitPasswordSupport_FILE);
            }
        }
        #endregion

        #region Mail Sender
        /// <summary>
        /// Mailtype to use
        /// </summary>
        public string MailType { get; set; }
        #endregion
        #endregion
    }
}
