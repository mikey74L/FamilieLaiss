using FamilieLaissEnums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.Models;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace FamilieLaissServices;

public class UrlHelperService(IOptions<AppSettings> appSettings) : IUrlHelperService
{
    private readonly AppSettings _appSettings = appSettings.Value;

    private string BaseUrlForPicture(long id, string fileExtension)
    {
        return $"{_appSettings.UrlPicture}/picture/{id}{fileExtension}";
    }

    private string BaseUrlForVideo(long id, string fileExtension)
    {
        return $"{_appSettings.UrlPicture}/video/{id}{fileExtension}";
    }

    private string GetPictureQueryParams(int? width, int? height)
    {
        var returnValue = "?mode=png";

        if (width > 0)
        {
            returnValue += $"&width={width}";
        }

        if (height > 0)
        {
            returnValue += $"&height={height}";
        }

        return returnValue;
    }

    private string GetUrlForUploadItem(long id, string fileExtension, int? width, int? height)
    {
        var returnValue = BaseUrlForPicture(id, fileExtension);

        returnValue += GetPictureQueryParams(width, height);

        return returnValue;
    }

    public string GetUrlForUploadPictureInfo(IUploadPictureModel model)
    {
        if (model.Filename is not null)
        {
            return GetUrlForUploadItem(model.Id, Path.GetExtension(model.Filename),
                _appSettings.PictureUploadWidthGallery,
                _appSettings.PictureUploadHeightGallery);
        }

        return "";
    }

    public string GetUrlForUploadPictureCard(IUploadPictureModel model)
    {
        if (model.Filename is not null)
        {
            return GetUrlForUploadItem(model.Id, Path.GetExtension(model.Filename),
                _appSettings.PictureUploadWidthCard,
                _appSettings.PictureUploadHeightCard);
        }

        return "";
    }

    public string GetUrlForUploadPictureThumbnail(IUploadPictureModel model)
    {
        if (model.Filename is not null)
        {
            return GetUrlForUploadItem(model.Id, Path.GetExtension(model.Filename),
                _appSettings.PictureUploadWidthThumbnail,
                _appSettings.PictureUploadHeightThumbnail);
        }

        return "";
    }

    public string GetUrlForUploadPictureGallery(IUploadPictureModel model)
    {
        if (model.Filename is not null)
        {
            return GetUrlForUploadItem(model.Id, Path.GetExtension(model.Filename),
                _appSettings.PictureUploadWidthGallery,
                _appSettings.PictureUploadHeightGallery);
        }

        return "";
    }

    public string GetUrlForUploadVideoCard(IUploadVideoModel model)
    {
        return GetUrlForUploadItem(model.Id, ".jpg", _appSettings.PictureUploadWidthCard,
            _appSettings.PictureUploadHeightCard);
    }

    public string GetUrlForVideoResolutionIcon(EnumVideoResolutionIcon videoResolutionSize)
    {
        switch (videoResolutionSize)
        {
            case EnumVideoResolutionIcon.p360:
                return "png/360p_Logo.png";
            case EnumVideoResolutionIcon.p480:
                return "png/480p_Logo.png";
            case EnumVideoResolutionIcon.p720:
                return "png/720p_Logo.png";
            case EnumVideoResolutionIcon.p1080:
                return "png/1080p_Logo.png";
            case EnumVideoResolutionIcon.p2160:
                return "png/4K_Logo.png";
            default:
                throw new ArgumentException("Unknown video resolution size");
        }
    }

}