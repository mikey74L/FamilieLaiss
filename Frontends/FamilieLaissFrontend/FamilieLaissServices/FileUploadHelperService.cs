using FamilieLaissEnums;
using FamilieLaissInterfaces;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models.Components;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.Models.Components;
using FamilieLaissServices.Extensions;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace FamilieLaissServices;

public class FileUploadHelperService(
    IFileUploadDataService fileUploadService,
    IGlobalFunctions globalFunctions)
    : IFileUploaderHelperService
{
    #region Implementation of IFileUploaderHelperService

    /// <inheritdoc />
    public async Task<IUploadFileInfo?> CreateUploadFileAsync(IBrowserFile browserFile, EnumUploadType uploadType)
    {
        IUploadFileInfo? resultModel = null;

        await fileUploadService.GetIdForUploadFromServerAsync()
            .HandleSuccess(async (idResult) =>
            {
                var uniqueId = idResult;

                var myUploadFile =
                    new UploadFileInfo(globalFunctions, uniqueId, browserFile, uploadType);

                await myUploadFile.GeneratePreviewImage();

                resultModel = myUploadFile;
            })
            .HandleErrors((_) => Task.CompletedTask);

        return resultModel;
    }

    /// <inheritdoc />
    public async Task<bool> UploadFileAsync(IUploadFileInfo fileInfoToUpload, int chunkSize, EnumUploadType uploadType,
        Action stateHasChangedAction)
    {
        var fileStream = fileInfoToUpload.GetFileStream();

        var usedChunkSize = chunkSize > fileInfoToUpload.FileSize ? fileInfoToUpload.FileSize : chunkSize;

        var buffer = new byte[usedChunkSize];

        fileInfoToUpload.SetUploadingState();

        stateHasChangedAction();
        long totalBytesRead = 0;
        bool exitLoop = false;
        long chunkNumber = 0;

        int bytesRead;
        while ((bytesRead = await fileStream.ReadAsync(buffer)) != 0 || exitLoop)
        {
            chunkNumber += 1;

            totalBytesRead += bytesRead;

            switch (uploadType)
            {
                case EnumUploadType.Picture:
                    await fileUploadService
                        .UploadChunkPictureAsync(fileInfoToUpload.FileNameTarget, chunkNumber, bytesRead, buffer)
                        .HandleSuccess((result) =>
                        {
                            if (!result)
                                exitLoop = true;

                            return Task.CompletedTask;
                        })
                        .HandleErrors((_) =>
                        {
                            exitLoop = true;

                            return Task.CompletedTask;
                        });
                    break;
                case EnumUploadType.Video:
                    await fileUploadService
                        .UploadChunkVideoAsync(fileInfoToUpload.FileNameTarget, chunkNumber, bytesRead, buffer)
                        .HandleSuccess((result) =>
                        {
                            if (!result)
                                exitLoop = true;

                            return Task.CompletedTask;
                        })
                        .HandleErrors((_) =>
                        {
                            exitLoop = true;

                            return Task.CompletedTask;
                        });
                    break;
            }

            fileInfoToUpload.CalculatePercentage(totalBytesRead);

            stateHasChangedAction();
        }

        if (!exitLoop)
        {
            switch (uploadType)
            {
                case EnumUploadType.Picture:
                    await fileUploadService.FinishUploadChunkPictureAsync(fileInfoToUpload.FileNameTarget, chunkNumber,
                            fileInfoToUpload.FileName)
                        .HandleSuccess((result) =>
                        {
                            if (!result) exitLoop = true;

                            return Task.CompletedTask;
                        })
                        .HandleErrors((_) =>
                        {
                            exitLoop = true;

                            return Task.CompletedTask;
                        });
                    break;
                case EnumUploadType.Video:
                    await fileUploadService.FinishUploadChunkVideoAsync(fileInfoToUpload.FileNameTarget, chunkNumber,
                            fileInfoToUpload.FileName)
                        .HandleSuccess((result) =>
                        {
                            if (!result) exitLoop = true;

                            return Task.CompletedTask;
                        })
                        .HandleErrors((_) =>
                        {
                            exitLoop = true;

                            return Task.CompletedTask;
                        });
                    break;
            }
        }

        if (exitLoop)
        {
            fileInfoToUpload.SetErrorState();

            return false;
        }

        fileInfoToUpload.SetSuccessState();

        return true;
    }

    #endregion
}