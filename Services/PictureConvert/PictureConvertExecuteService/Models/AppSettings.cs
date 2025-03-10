namespace PictureConvertExecuteService.Models;

public class AppSettings
{
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
        get { return System.IO.File.ReadAllText(PostgresUser_FILE); }
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
        get { return System.IO.File.ReadAllText(PostgresPassword_FILE); }
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

    #region RabbitMQ

    /// <summary>
    /// RabbitMQ Connection-String - Filename 
    /// </summary>
    public string RabbitMQConnection_FILE { get; set; }

    /// <summary>
    /// RabbitMQ Connection-String for CloudAMP
    /// </summary>
    public string RabbitMqConnection
    {
        get { return System.IO.File.ReadAllText(RabbitMQConnection_FILE); }
    }

    #endregion

    #region Content Directories

    public string DirectoryUploadPicture { get; set; } = string.Empty;

    public string DirectoryUploadVideo { get; set; } = string.Empty;

    public string DirectoryConvertVideo { get; set; } = string.Empty;

    #endregion

    #region Convert-Sizes

    public int CardSizeWidth { get; set; }
    public int CardSizeHeight { get; set; }

    public int GallerySizeWidth { get; set; }
    public int GallerySizeHeight { get; set; }

    public int ThumbnailSizeWidth { get; set; }
    public int ThumbnailSizeHeight { get; set; }

    #endregion

    #region Endpoint-Names

    public string EndpointNameExecutor { get; set; } = string.Empty;
    public string EndpointNameUploadService { get; set; } = string.Empty;

    #endregion
}