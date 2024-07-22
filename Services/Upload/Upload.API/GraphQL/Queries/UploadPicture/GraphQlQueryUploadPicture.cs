using FamilieLaissSharedObjects.Enums;
using Upload.API.GraphQL.DataLoader.UploadPicture;
using Upload.API.Models;
using Upload.Infrastructure.DBContext;

namespace Upload.API.GraphQL.Queries.UploadPicture;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryUploadPicture
{
    [GraphQLDescription("Returns a list of upload picture")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Entities.UploadPicture> GetUploadPictures(UploadServiceDbContext context)
    {
        return context.UploadPictures;
    }

    [GraphQLDescription("Returns a upload picture")]
    [UseProjection]
    public async Task<Domain.Entities.UploadPicture> GetUploadPicture(long id, UploadPictureDataLoader dataLoader)
        => await dataLoader.LoadAsync(id);

    [GraphQLDescription("Returns the filter data for upload picture exif infos")]
    public UploadPictureExifInfoFilterData GetUploadPictureExifInfoFilterData(UploadServiceDbContext context)
    {
        var result = new UploadPictureExifInfoFilterData();

        //Get all distinct values for make
        var distinctValuesMake = context.UploadPictures
            .Where(x => x.UploadPictureExifInfo != null)
            .Select(x => x.UploadPictureExifInfo!.Make)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct();

        //Get all distinct values for models
        var distinctValuesModel = context.UploadPictures
            .Where(x => x.UploadPictureExifInfo != null)
            .Select(x => x.UploadPictureExifInfo!.Model)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct();

        //Get all distinct values for ISO Sensitivity
        var distinctValuesIsoSensitivity = context.UploadPictures
            .Where(x => x.UploadPictureExifInfo != null)
            .Select(x => x.UploadPictureExifInfo!.IsoSensitivity)
            .Where(x => x != null)
            .Cast<int>()
            .Distinct();

        //Get all distinct values for f numbers
        var distinctValuesFNumber = context.UploadPictures
            .Where(x => x.UploadPictureExifInfo != null)
            .Select(x => x.UploadPictureExifInfo!.FNumber)
            .Where(x => x != null)
            .Cast<double>()
            .Distinct();

        //Get all distinct values for exposure times
        var distinctValuesExposureTime = context.UploadPictures
            .Where(x => x.UploadPictureExifInfo != null)
            .Select(x => x.UploadPictureExifInfo!.ExposureTime)
            .Where(x => x != null)
            .Cast<double>()
            .Distinct();

        //Get all distinct values for shutter speeds
        var distinctValuesShutterSpeed = context.UploadPictures
            .Where(x => x.UploadPictureExifInfo != null)
            .Select(x => x.UploadPictureExifInfo!.ShutterSpeed)
            .Where(x => x != null)
            .Cast<double>()
            .Distinct();

        //Get all distinct values for focal lengths
        var distinctValuesFocalLength = context.UploadPictures
            .Where(x => x.UploadPictureExifInfo != null)
            .Select(x => x.UploadPictureExifInfo!.FocalLength)
            .Where(x => x != null)
            .Cast<double>()
            .Distinct();

        result.Make.AddRange(distinctValuesMake);
        result.Model.AddRange(distinctValuesModel);
        result.IsoSensitivities.AddRange(distinctValuesIsoSensitivity);
        result.FNumbers.AddRange(distinctValuesFNumber);
        result.ExposureTimes.AddRange(distinctValuesExposureTime);
        result.ShutterSpeeds.AddRange(distinctValuesShutterSpeed);
        result.FocalLengths.AddRange(distinctValuesFocalLength);

        return result;
    }

    [GraphQLDescription("Returns the current count for converted and unassigned upload pictures")]
    public int GetUploadPictureCount(UploadServiceDbContext context)
    {
        return context.UploadPictures.Count(x => x.Status == EnumUploadStatus.Converted);
    }
}