using DomainHelper.Interfaces;
using FLBackEnd.Domain.Entities;
using FLBackEnd.Infrastructure.DatabaseContext;
using InfrastructureHelper.EventDispatchHandler;
using InfrastructureHelper.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using PictureConvertExecuteService.Interfaces;
using PictureConvertExecuteService.Models;
using System;
using System.Threading.Tasks;

namespace PictureConvertExecuteService.Services;

public class DatabaseOperationsService(
    ILogger<DatabaseOperationsService> logger,
    IOptions<AppSettings> appSettings,
    iEventStore eventStore) : IDatabaseOperations
{
    #region Private Methods

    private FamilieLaissDbContext CreateDbContext()
    {
        NpgsqlConnectionStringBuilder postgresConnectionStringBuilder = new()
        {
            ApplicationName = "Picture-Convert-Execute-Service",
            Host = appSettings.Value.PostgresHost,
            Port = appSettings.Value.PostgresPort,
            Multiplexing = appSettings.Value.PostgresMultiplexing,
            Database = appSettings.Value.PostgresDatabase,
            Username = appSettings.Value.PostgresUser,
            Password = appSettings.Value.PostgresPassword
        };

        var optionsBuilder = new DbContextOptionsBuilder<FamilieLaissDbContext>();
        optionsBuilder.UseNpgsql(postgresConnectionStringBuilder.ConnectionString);

        var context = new FamilieLaissDbContext(optionsBuilder.Options);

        return context;
    }

    private iUnitOfWork GetUnitOfWork()
    {
        var context = CreateDbContext();

        UnitOfWork<FamilieLaissDbContext> workUnit = new(context, eventStore);

        return workUnit;
    }

    #endregion

    #region Interface iDatabaseOperations

    public async Task SetTransientErrorAsync(long id, string errorMessage)
    {
        logger.LogInformation("Get unit of work");
        iUnitOfWork workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<PictureConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        PictureConvertStatus entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to transient");
        entity.TransientError(errorMessage);

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetErrorAsync(long id, string errorMessage)
    {
        logger.LogInformation("Get unit of work");
        iUnitOfWork workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<PictureConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        PictureConvertStatus entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to finished with error");
        await entity.ConversionFinishedWithError(errorMessage);

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusReadExifBeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        iUnitOfWork workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<PictureConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        PictureConvertStatus entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to start reading EXIF info");
        entity.SetStatusStartExif();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusReadExifEndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        iUnitOfWork workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<PictureConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        PictureConvertStatus entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to end reading EXIF info");
        entity.SetStatusEndExif();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusReadInfoBeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        iUnitOfWork workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<PictureConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        PictureConvertStatus entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to start reading picture info");
        entity.SetStatusStartInfo();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusReadInfoEndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        iUnitOfWork workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<PictureConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        PictureConvertStatus entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to end reading picture info");
        entity.SetStatusEndInfo();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetSuccessAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        iUnitOfWork workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<PictureConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        PictureConvertStatus entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to successfully finished");
        await entity.ConversionFinished();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusConvertBeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        iUnitOfWork workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<PictureConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        PictureConvertStatus entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to begin picture convert");
        entity.SetStatusStartConvert();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusConvertEndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        iUnitOfWork workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<PictureConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        PictureConvertStatus entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to end picture convert");
        entity.SetStatusEndConvert();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetSizeForPicture(long id, int width, int height)
    {
        logger.LogInformation("Get unit of work");
        iUnitOfWork workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<UploadPicture>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        UploadPicture entity = await repo.GetOneAsync(id);

        logger.LogInformation("Set picture size");
        entity.UpdateSize(height, width);

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetExifInfoForPicture(long id, string? make, string? model, double? resolutionX,
        double? resolutionY, short? resolutionUnit, short? orientation, DateTimeOffset? ddlRecorded,
        double? exposureTime, short? exposureProgram, short? exposureMode, double? fNumber, int? isoSensitivity,
        double? shutterSpeed, short? meteringMode, short? flashMode, double? focalLength, short? sensingMode,
        short? whiteBalanceMode, short? sharpness, double? gpsLongitude, double? gpsLatitude, short? contrast,
        short? saturation)
    {
        logger.LogInformation("Get unit of work");
        iUnitOfWork workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<UploadPicture>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        UploadPicture entity = await repo.GetOneAsync(id);

        logger.LogInformation($"Set EXIF-Info for picture");
        entity.SetExifData(make ?? "", model ?? "", resolutionX, resolutionY, resolutionUnit, orientation,
            ddlRecorded, exposureTime,
            exposureProgram, exposureMode, fNumber, isoSensitivity, shutterSpeed, meteringMode, flashMode, focalLength,
            sensingMode, whiteBalanceMode, sharpness, gpsLongitude, gpsLatitude, contrast, saturation);

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion
}