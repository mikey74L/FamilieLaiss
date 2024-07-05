using FamilieLaissEnums;
using Microsoft.AspNetCore.Components.Forms;

namespace FamilieLaissInterfaces.Models.Components;

public interface IUploadFileInfo
{
    #region Methods

    Task GeneratePreviewImage();

    Task DisposeData();

    Stream GetFileStream();

    #endregion

    #region Properties

    public void CalculatePercentage(long totalBytesTransfered);

    public void SetUploadingState();

    public void SetErrorState();

    public void SetSuccessState();

    public long Id { get; }

    public string FileName { get; }

    public string FileNameTarget { get; }

    public long FileSize { get; }

    public string FileSizeAsString { get; }

    public EnumUploadType UploadType { get; }

    public bool IsUploaded { get; }

    public bool WithError { get; }

    public int PercentageDone { get; }

    public EnumUploadStatus Status { get; }

    public string PreviewImage { get; }

    public string StatusAsString { get; }

    public string CssStatusText { get; }

    public IBrowserFile OriginalFile { get; }

    public long TimeRestSeconds { get; }

    #endregion
}