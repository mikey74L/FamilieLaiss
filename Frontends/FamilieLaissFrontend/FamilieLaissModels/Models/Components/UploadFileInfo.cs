using FamilieLaissEnums;
using FamilieLaissInterfaces;
using FamilieLaissInterfaces.Models.Components;
using FamilieLaissResources.Resources.ViewModels.Controls.UploadControl;
using Microsoft.AspNetCore.Components.Forms;

namespace FamilieLaissModels.Models.Components;

#region Private Classes

public class UploadFileInfo(
    IGlobalFunctions globalFunctions,
    long id,
    IBrowserFile file,
    EnumUploadType uploadType) : IUploadFileInfo
{
    #region Private Members

    private MemoryStream? _imageStream;

    #endregion

    #region Public Methods

    public async Task GeneratePreviewImage()
    {
        if (UploadType == EnumUploadType.Picture)
        {
            var resizedImage = await OriginalFile.RequestImageFileAsync(OriginalFile.ContentType, 120, 90);
            var buf = new byte[resizedImage.Size];
            using (var stream = resizedImage.OpenReadStream())
            {
                _ = await stream.ReadAsync(buf);
            }

            PreviewImage = @"data:image/" + OriginalFile.ContentType + ";base64," + Convert.ToBase64String(buf);
        }
    }

    public async Task DisposeData()
    {
        if (_imageStream != null)
        {
            _imageStream.Close();
            await _imageStream.DisposeAsync();
        }
    }

    public Stream GetFileStream()
    {
        return OriginalFile.OpenReadStream((long)1024 * 1024 * 1024 * 20);
    }

    public void CalculatePercentage(long totalBytesTransfered)
    {
        PercentageDone =
            Convert.ToInt32(Math.Round(
                Convert.ToDecimal(totalBytesTransfered) / Convert.ToDecimal(OriginalFile.Size) * 100, 0));
    }

    public void SetUploadingState()
    {
        PercentageDone = 0;

        WithError = false;

        Status = EnumUploadStatus.Uploading;
    }

    public void SetErrorState()
    {
        PercentageDone = 100;

        IsUploaded = false;

        WithError = true;

        Status = EnumUploadStatus.Error;
    }

    public void SetSuccessState()
    {
        PercentageDone = 100;

        IsUploaded = true;

        WithError = false;

        Status = EnumUploadStatus.Success;
    }

    #endregion

    #region Properties

    public long Id { get; private set; } = id;

    public string FileName => OriginalFile.Name;

    public string FileNameTarget => $"{Id}{System.IO.Path.GetExtension(FileName)}";

    public long FileSize => OriginalFile.Size;

    public string FileSizeAsString => globalFunctions.GetFileSizeAsString(FileSize);

    public EnumUploadType UploadType { get; private set; } = uploadType;

    public bool IsUploaded { get; private set; } = false;

    public bool WithError { get; private set; } = false;

    public int PercentageDone { get; private set; } = 0;

    public EnumUploadStatus Status { get; private set; } = EnumUploadStatus.Added;

    public string PreviewImage { get; private set; } = string.Empty;

    public string StatusAsString
    {
        get
        {
            switch (Status)
            {
                case EnumUploadStatus.Added:
                    return UploadFileInfoRes.StatusAdded;
                case EnumUploadStatus.Uploading:
                    return UploadFileInfoRes.StatusUploading;
                case EnumUploadStatus.Success:
                    return UploadFileInfoRes.StatusSuccess;
                case EnumUploadStatus.Error:
                    return UploadFileInfoRes.StatusError;
                default:
                    return "Not defined";
            }
        }
    }

    public string CssStatusText
    {
        get
        {
            var returnValue = "font-weight-bold";

            returnValue += Status switch
            {
                EnumUploadStatus.Success => " text-success",
                EnumUploadStatus.Error => " text-danger",
                _ => " text-muted"
            };

            return returnValue;
        }
    }

    public IBrowserFile OriginalFile { get; private set; } = file;

    public long TimeRestSeconds { get; set; }

    #endregion
}

#endregion