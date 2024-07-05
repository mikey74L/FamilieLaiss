using AutoMapper;
using Upload.Domain.Entities;
using Upload.DTO.UploadPicture;

namespace Upload.API.AutoMapper;

public class UploadPictureProfile: Profile
{
    public UploadPictureProfile()
    {
        CreateMap<UploadPicture, UploadPictureDto>();
    }
}