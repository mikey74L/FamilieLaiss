﻿namespace FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;

public interface IMassPictureConvertedEvent
{
    /// <summary>
    /// ID of upload picture
    /// </summary>
    long UploadPictureId { get; }

    /// <summary>
    /// ID for status entry
    /// </summary>
    long ConvertStatusId { get; }
}