using FamilieLaissEnums;
using FamilieLaissInterfaces.Models.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FamilieLaissInterfaces.Services;

public interface IFileUploaderHelperService
{
    Task<IUploadFileInfo?> CreateUploadFileAsync(IBrowserFile browserFile, EnumUploadType uploadType);

    Task<bool> UploadFileAsync(IUploadFileInfo fileInfoToUpload, int chunkSize, EnumUploadType uploadType,
        Action stateHasChangedAction);
}