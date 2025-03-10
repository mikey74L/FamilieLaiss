using Microsoft.Extensions.Logging;
using ServiceHelper.Exceptions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VideoConvertExecuteService.Services;

public partial class VideoConverterService
{
    private async Task DeleteOriginalFile()
    {
        try
        {
            logger.LogInformation("Set Status in database to 'delete original - begin'");
            await databaseOperations.SetStatusDeleteOriginalBeginAsync(_convertStatusId);

            logger.LogInformation("Delete original video in service");
            File.Delete(_filenameSource);

            logger.LogInformation("Set Status in database to 'delete original - end'");
            await databaseOperations.SetStatusDeleteOriginalEndAsync(_convertStatusId);

            logger.LogInformation($"File \"{Path.GetFileName(_filenameSource)}\" successfully deleted in service");
        }
        catch (Exception ex)
        {
            throw new ServiceException("Error!!! Could not delete original video.", ex);
        }
    }

    private async Task DeleteGeneratedFilesHls(string filename360)
    {
        try
        {
            await databaseOperations.SetStatusDeleteConvertedBeginAsync(_convertStatusId);

            logger.LogInformation("Delete temporary image files");
            var filteredFiles = Directory.EnumerateFiles(Path.GetDirectoryName(filename360) ?? string.Empty)
                .Where(x => x.Contains("-Sprite-"))
                .ToList();
            foreach (var filename in filteredFiles)
            {
                File.Delete(filename);
            }

            logger.LogInformation("Temporary image files successfully deleted");

            await databaseOperations.SetStatusDeleteConvertedEndAsync(_convertStatusId);
        }
        catch (Exception ex)
        {
            throw new ServiceException("Error!!! Could not delete converted files.", ex);
        }
    }
}