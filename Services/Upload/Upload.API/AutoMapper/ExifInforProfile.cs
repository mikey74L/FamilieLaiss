using AutoMapper;
using Upload.Domain.ValueObjects;
using Upload.DTO.UploadPicture;

namespace Upload.API.AutoMapper;

public class ExifInforProfile: Profile
{
    public ExifInforProfile()
    {
        CreateMap<UploadPictureExifInfo, UploadPictureExifInfoDto>();
    }
}