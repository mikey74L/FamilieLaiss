using System.Reflection;
using Catalog.DTO.CategoryValue;
using Mapster;

namespace Catalog.API.Extensions;

public static class RegisterMapsterTypeConfigExtensions
{
    public static void AddMapsterTypeConfigurations(this IServiceCollection services)
    {
        #region Category Value

        TypeAdapterConfig<Catalog.Domain.Aggregates.CategoryValue, CategoryValueDTO>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CategoryId, src => src.CategoryID)
            .Map(dest => dest.NameGerman, src => src.NameGerman)
            .Map(dest => dest.NameEnglish, src => src.NameEnglish)
            .Map(dest => dest.CreateDate, src => src.CreateDate)
            .Map(dest => dest.ChangeDate, src => src.ChangeDate)
            .Compile();

        #endregion

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}