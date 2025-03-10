using DomainHelper.Interfaces;
using InfrastructureHelper.EventDispatchHandler;
using InfrastructureHelper.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using PictureConvert.Domain.Entities;
using PictureConvert.Infrastructure.DBContext;
using PictureConvertExecuteService.Interfaces;
using PictureConvertExecuteService.Models;
using System.Threading.Tasks;

namespace PictureConvertExecuteService.Services;

public class DatabaseOperationsService(
    ILogger<DatabaseOperationsService> logger,
    IOptions<AppSettings> appSettings,
    iEventStore eventStore) : IDatabaseOperations
{
    #region Private Methods

    private PictureConvertServiceDbContext CreateDbContext()
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

        var optionsBuilder = new DbContextOptionsBuilder<PictureConvertServiceDbContext>();
        optionsBuilder.UseNpgsql(postgresConnectionStringBuilder.ConnectionString);

        var context = new PictureConvertServiceDbContext(optionsBuilder.Options);

        return context;
    }

    private iUnitOfWork GetUnitOfWork()
    {
        var context = CreateDbContext();

        UnitOfWork<PictureConvertServiceDbContext> workUnit = new(context, eventStore);

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
        iUnitOfWork workUnit = GetUnitOfWork();

        logger.LogInformation("Get repository from unit of work");
        var repo = workUnit.GetRepository<PictureConvertStatus>();

        logger.LogInformation($"Get entity from repository for ID = {id}");
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to finished with error");
        entity.ConversionFinishedWithError(errorMessage);

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
        var entity = await repo.GetOneAsync(id);

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
        var entity = await repo.GetOneAsync(id);

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
        var entity = await repo.GetOneAsync(id);

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
        var entity = await repo.GetOneAsync(id);

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
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to successfully finished");
        entity.ConversionFinished();

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
        var entity = await repo.GetOneAsync(id);

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
        var entity = await repo.GetOneAsync(id);

        logger.LogInformation("Setting status to end picture convert");
        entity.SetStatusEndConvert();

        logger.LogInformation("Save changes");
        await workUnit.SaveChangesAsync();

        logger.LogInformation("Disposing unit of work and database context");
        workUnit.Dispose();
    }

    #endregion
}