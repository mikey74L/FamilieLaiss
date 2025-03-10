namespace VideoConvertExecuteService.Models;

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
    /// Postgres - Hostname or IP-Address
    /// </summary>
    public string PostgresHost { get; set; } = string.Empty;

    /// <summary>
    /// Postgres - Database name
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

    public string DirectoryUploadPicture { get; set; }

    public string DirectoryUploadVideo { get; set; }

    public string DirectoryConvertVideo { get; set; }

    #endregion

    #region Endpoint-Names

    /// <summary>
    /// Endpoint-Name for Video-Converter-Execute-Service
    /// </summary>
    public string EndpointNameExecutor { get; set; }

    /// <summary>
    /// Endpoint-Name for Upload-Service
    /// </summary>
    public string EndpointNameUploadService { get; set; }

    #endregion

    #region Conversion

    public int WidthThumbnailImage { get; set; }

    public int HeightThumbnailImage { get; set; }

    public int UsedThreads { get; set; }

    public int ThresholdStreaming { get; set; }

    public int SegmentSizeHls { get; set; }

    #endregion
}