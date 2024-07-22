using DomainHelper.Interfaces;
using InfrastructureHelper.EventDispatchHandler;
using InfrastructureHelper.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Threading.Tasks;
using VideoConvert.Domain.Entities;
using VideoConvert.Infrastructure.DBContext;
using VideoConvertExecuteService.Interfaces;
using VideoConvertExecuteService.Models;

namespace VideoConvertExecuteService.Services;

public class DatabaseOperationsService(
    ILogger<DatabaseOperationsService> logger,
    IOptions<AppSettings> appSettings,
    iEventStore eventStore)
    : IDatabaseOperations
{
    #region Private Methods

    private VideoConvertServiceDbContext CreateDbContext()
    {
        NpgsqlConnectionStringBuilder postgresConnectionStringBuilder = new()
        {
            ApplicationName = "Video-Convert-Execute-Service",
            Host = appSettings.Value.PostgresHost,
            Port = appSettings.Value.PostgresPort,
            Multiplexing = appSettings.Value.PostgresMultiplexing,
            Database = appSettings.Value.PostgresDatabase,
            Username = appSettings.Value.PostgresUser,
            Password = appSettings.Value.PostgresPassword
        };

        var optionsBuilder = new DbContextOptionsBuilder<VideoConvertServiceDbContext>();
        optionsBuilder.UseNpgsql(postgresConnectionStringBuilder.ConnectionString);

        VideoConvertServiceDbContext context = new(optionsBuilder.Options);

        return context;
    }

    private iUnitOfWork GetUnitOfWork()
    {
        var context = CreateDbContext();

        UnitOfWork<VideoConvertServiceDbContext> workUnit = new(context, eventStore);

        return workUnit;
    }

    #endregion

    #region Interface iDatabaseOperations

    public async Task SetTransientErrorAsync(long id, string errorMessage)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

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
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to finished with error");
        await entity.ConversionFinishedWithError(errorMessage);

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetSuccessAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to successfully finished");
        await entity.ConversionFinished();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task UpdateProgressAsync(long id, int progressCurrent, TimeSpan currentTime, TimeSpan restTime)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting progress");
        entity.UpdateProgress(progressCurrent, currentTime, restTime);

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #region Read Media-Info

    public async Task SetStatusReadMediaInfoBeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Read-MediaInfo-Begin");
        entity.SetStatusStartMediaInfo();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusReadMediaInfoEndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Read-MediaInfo-End");
        entity.SetStatusEndMediaInfo();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion

    #region Convert MP4

    public async Task SetStatusConvertMp4BeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Convert-MP4-Begin");
        entity.SetStatusStartConvertMp4();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusConvertMp4EndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Convert-MP4-End");
        entity.SetStatusEndConvertMp4();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion

    #region Convert MP4 x 360

    public async Task SetStatusConvert640X360BeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Convert-MP4-360-Begin");
        entity.SetStatusStartConvertMP4_360();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusConvert640X360EndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Convert-MP4-360-End");
        entity.SetStatusEndConvertMP4_360();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion

    #region Convert MP4 x 480

    public async Task SetStatusConvert852X480BeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Convert-MP4-480-Begin");
        entity.SetStatusStartConvertMP4_480();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusConvert852X480EndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Convert-MP4-480-End");
        entity.SetStatusEndConvertMP4_480();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion

    #region Convert MP4 x 720

    public async Task SetStatusConvert1280X720BeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Convert-MP4-720-Begin");
        entity.SetStatusStartConvertMP4_720();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusConvert1280X720EndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Convert-MP4-720-End");
        entity.SetStatusEndConvertMP4_720();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion

    #region Convert MP4 x 1080

    public async Task SetStatusConvert1920X1080BeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Convert-MP4-1080-Begin");
        entity.SetStatusStartConvertMP4_1080();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusConvert1920X1080EndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Convert-MP4-1080-End");
        entity.SetStatusEndConvertMP4_1080();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion

    #region Convert MP4 x 2160

    public async Task SetStatusConvert3840X2160BeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Convert-MP4-2160-Begin");
        entity.SetStatusStartConvertMP4_2160();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusConvert3840X2160EndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Convert-MP4-2160-End");
        entity.SetStatusEndConvertMP4_2160();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion

    #region Create Thumbnail

    public async Task SetStatusCreateThumbnailBeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Create-Thumbnails-Begin");
        entity.SetStatusStartCreateThumbnails();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusCreateThumbnailEndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Create-Thumbnails-End");
        entity.SetStatusEndCreateThumbnails();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion

    #region Create VTT-File

    public async Task SetStatusCreateVttBeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Create-VTT-Begin");
        entity.SetStatusStartCreateVttFile();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusCreateVttEndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Create-VTT-End");
        entity.SetStatusEndCreateVttFile();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion

    #region Create HLS-Content

    public async Task SetStatusCreateHlsBeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Create-HLS-Begin");
        entity.SetStatusStartCreateHls();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusCreateHlsEndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Create-HLS-End");
        entity.SetStatusEndCreateHls();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion

    #region Convert Picture

    public async Task SetStatusConvertPictureBeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Create-DASH-Begin");
        entity.SetStatusStartConvertPicture();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusConvertPictureEndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Create-DASH-Begin");
        entity.SetStatusEndConvertPicture();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion

    #region Copy converted

    public async Task SetStatusCopyConvertedBeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Copy-Converted-Begin");
        entity.SetStatusStartCopyConvertedFiles();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusCopyConvertedEndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Copy-Converted-End");
        entity.SetStatusEndCopyConvertedFiles();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion

    #region Delete Original

    public async Task SetStatusDeleteOriginalBeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Delete-Original-Begin");
        entity.SetStatusStartDeleteOriginalFile();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusDeleteOriginalEndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Delete-Original-End");
        entity.SetStatusEndDeleteOriginalFile();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion

    #region Delete temporary

    public async Task SetStatusDeleteConvertedBeginAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Delete-Temporary-Begin");
        entity.SetStatusStartDeleteTemporaryFiles();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    public async Task SetStatusDeleteConvertedEndAsync(long id)
    {
        logger.LogInformation("Get unit of work");
        var workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<VideoConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to Delete-Temporary-End");
        entity.SetStatusEndDeleteTemporaryFiles();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion

    #endregion
}