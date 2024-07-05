namespace Message.API.Models
{
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
        public string Endpoint_MessageService { get; set; }
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

        #region OIDC
        public string OIDC_ClientID_FILE { get; set; } = string.Empty;

        public string OIDC_ClientID
        {
            get
            {
                return System.IO.File.ReadAllText(OIDC_ClientID_FILE);
            }
        }

        public string OIDC_ClientSecret_FILE { get; set; } = string.Empty;

        public string OIDC_ClientSecret
        {
            get
            {
                return System.IO.File.ReadAllText(OIDC_ClientSecret_FILE);
            }
        }

        public string OIDC_Scope_Identity { get; set; } = string.Empty;

        public string Authority { get; set; } = string.Empty;
        #endregion
    }
}
