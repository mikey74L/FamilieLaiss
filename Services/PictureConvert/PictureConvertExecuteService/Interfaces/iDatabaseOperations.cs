using System.Threading.Tasks;

namespace PictureConvertExecuteService.Interfaces;

public interface IDatabaseOperations
{
    Task SetStatusReadInfoBeginAsync(long id);

    Task SetStatusReadInfoEndAsync(long id);

    Task SetStatusReadExifBeginAsync(long id);

    Task SetStatusReadExifEndAsync(long id);

    Task SetStatusConvertBeginAsync(long id);

    Task SetStatusConvertEndAsync(long id);

    Task SetSuccessAsync(long id);

    Task SetTransientErrorAsync(long id, string errorMessage);

    Task SetErrorAsync(long id, string errorMessage);
}