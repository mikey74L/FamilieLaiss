﻿namespace FlBackEnd.API.Models;

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
    public string RabbitMqConnection => System.IO.File.ReadAllText(RabbitMqConnectionFile);

    #region Endpoints

    /// <summary>
    /// Endpoint for Catalog-Service
    /// </summary>
    public string EndpointFlBackEndService { get; set; } = string.Empty;

    /// <summary>
    /// Endpoint for Picture-Convert-Service
    /// </summary>
    public string EndpointPictureConvertService { get; set; } = string.Empty;

    /// <summary>
    /// Endpoint for Video-Convert-Service
    /// </summary>
    public string EndpointVideoConvertService { get; set; } = string.Empty;

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
    public string PostgresUser => System.IO.File.ReadAllText(PostgresUserFile);

    /// <summary>
    /// Postgres password - Filename for Secret
    /// </summary>
    public string PostgresPasswordFile { get; set; } = string.Empty;

    /// <summary>
    /// Postgres password
    /// </summary>
    public string PostgresPassword => System.IO.File.ReadAllText(PostgresPasswordFile);

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

    #region CORS

    public string CorsOrigin { get; set; } = string.Empty;

    #endregion

    #region File-Upload

    public string TempDirectoryUploadPicture { get; set; } = string.Empty;
    public string TempDirectoryUploadVideo { get; set; } = string.Empty;
    public string DirectoryUploadPicture { get; set; } = string.Empty;
    public string DirectoryUploadVideo { get; set; } = string.Empty;

    #endregion

    #region Google
    public string GoogleMapsApiKey { get; set; } = string.Empty;
    public string GoogleMicroserviceUrl { get; set; } = string.Empty;
    public string GoogleMicroserviceVersion { get; set; } = string.Empty;

    #endregion
}