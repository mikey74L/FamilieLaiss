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
    public string RabbitMQConnection_FILE { get; set; } = string.Empty;

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
    /// Endpoint for Upload-Service
    /// </summary>
    public string Endpoint_UploadService { get; set; } = string.Empty;

    /// <summary>
    /// Endpoint for Message-Service
    /// </summary>
    public string Endpoint_MessageService { get; set; } = string.Empty;
    #endregion
    #endregion

    #region Google Microservice
    /// <summary>
    /// The Base-URL where the google micro service can be reached in docker network
    /// </summary>
    public string GoogleMicroserviceUrl { get; set; } = string.Empty;

    /// <summary>
    /// The API-Version for the google micro service
    /// </summary>
    public string GoogleMicroserviceVersion { get; set; } = string.Empty;
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

    #region General Properties
    /// <summary>
    /// Directory for upload pictures
    /// </summary>
    public string Directory_Upload_Picture { get; set; } = string.Empty;

    /// <summary>
    /// Directory for upload videos
    /// </summary>
    public string Directory_Upload_Video { get; set; } = string.Empty;

    /// <summary>
    /// Directory for upload portraits
    /// </summary>
    public string Directory_Upload_Portrait { get; set; } = string.Empty;

    /// <summary>
    /// Temporary directory for upload pictures
    /// </summary>
    public string Temp_Directory_Upload_Picture { get; set; } = string.Empty;

    /// <summary>
    /// Temporary directory for upload portraits
    /// </summary>
    public string Temp_Directory_Upload_Portrait { get; set; } = string.Empty;

    /// <summary>
    /// Temporary directory for upload videos
    /// </summary>
    public string Temp_Directory_Upload_Video { get; set; } = string.Empty;

    /// <summary>
    /// Maximum file size in bytes for upload pictures
    /// </summary>
    public long Max_File_Size_Picture { get; set; }

    /// <summary>
    /// Maximum file size in bytes for upload portraits
    /// </summary>
    public long Max_File_Size_Portrait { get; set; }

    /// <summary>
    /// Maximum file size in bytes for upload videos
    /// </summary>
    public long Max_File_Size_Video { get; set; }
    #endregion
}
