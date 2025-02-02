namespace UserInteraction.API.Models;

/// <summary>
/// App-Settings - Class
/// </summary>
public class AppSettings
{
    #region RabbitMQ
    /// <summary>
    /// RabbitMQ Connection-String - Filename 
    /// </summary>
    public string RabbitMqConnectionFile { get; set; }

    /// <summary>
    /// RabbitMQ Connection-String for CloudAMP
    /// </summary>
    public string RabbitMqConnection
    {
        get
        {
            try
            {
                return System.IO.File.ReadAllText(RabbitMqConnectionFile);
            }
            catch
            {
                return "withoutdocker";
            }
        }
    }

    #region Endpoints
    /// <summary>
    /// Endpoint for User-Interaction-Service
    /// </summary>
    public string EndpointUserInteractionService { get; set; }
    #endregion
    #endregion

    #region Postgres
    /// <summary>
    /// Postgres user - Filename for Secret
    /// </summary>
    public string PostgresUserFile { get; set; } = string.Empty;

    /// <summary>
    /// Postgres user
    /// </summary>
    public string PostgresUser
    {
        get
        {
            try
            {
                return System.IO.File.ReadAllText(PostgresUserFile);
            }
            catch
            {
                return "withoutdocker";
            }
        }
    }

    /// <summary>
    /// Postgres password - Filename for Secret
    /// </summary>
    public string PostgresPasswordFile { get; set; } = string.Empty;

    /// <summary>
    /// Postgres password
    /// </summary>
    public string PostgresPassword
    {
        get
        {
            try
            {
                return System.IO.File.ReadAllText(PostgresPasswordFile);
            }
            catch
            {
                return "withoutdocker";
            }
        }
    }

    /// <summary>
    /// Postgres - Port
    /// </summary>
    public int PostgresPort { get; set; }

    /// <summary>
    /// Postgres - Hostname or IP-Address
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
}