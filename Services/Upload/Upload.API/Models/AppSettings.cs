namespace Upload.API.Models;

/// <summary>
/// App-Settings - Class
/// </summary>
public class AppSettings
{
    #region RabbitMQ

    /// <summary>
    /// RabbitMQ Connection-String - Filename 
    /// </summary>
    public string RabbitMqConnectionFile { get; set; } = string.Empty;

    /// <summary>
    /// RabbitMQ Connection-String for CloudAMP
    /// </summary>
    public string RabbitMqConnection
    {
        get
        {
            try
            {
                return File.ReadAllText(RabbitMqConnectionFile);
            }
            catch
            {
                return "withoutdocker";
            }
        }
    }

    #region Endpoints

    /// <summary>
    /// Endpoint for Upload-Service
    /// </summary>
    public string EndpointUploadService { get; set; } = string.Empty;

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
                return File.ReadAllText(PostgresUserFile);
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
                return File.ReadAllText(PostgresPasswordFile);
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
    /// Postgres database name
    /// </summary>
    public string PostgresDatabase { get; set; } = string.Empty;

    /// <summary>
    /// Postgres - Use Multiplexing for faster connections
    /// </summary>
    public bool PostgresMultiplexing { get; set; }

    #endregion

    #region File-Upload

    public string TempDirectoryUploadPicture { get; set; } = string.Empty;
    public string TempDirectoryUploadVideo { get; set; } = string.Empty;
    public string DirectoryUploadPicture { get; set; } = string.Empty;
    public string DirectoryUploadVideo { get; set; } = string.Empty;

    #endregion

    #region Google

    public string GoogleMicroserviceUrl { get; set; } = string.Empty;
    public string GoogleMicroserviceVersion { get; set; } = string.Empty;

    #endregion
}