using FamilieLaissSharedObjects.Enums;
using FLBackEnd.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace FLBackEnd.API.GraphQL.Queries.Category;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryCategory
{
    //[Authorize("Category.Read")]
    [GraphQLDescription("Returns a list of categories")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Entities.Category> GetCategories(FamilieLaissDbContext context)
    {
        return context.Category;
    }

    //[Authorize("Category.Validate")]
    [GraphQLDescription("Validate german category name ")]
    public async Task<bool> GermanCategoryNameExistsAsync(FamilieLaissDbContext context,
        long iD, EnumCategoryType categoryType, string germanName)
    {
        var repository = context.Category;

        long count;
        if (categoryType == EnumCategoryType.Both)
        {
            count = await repository.CountAsync(x => x.Id != iD && x.NameGerman == germanName);
        }
        else
        {
            count = await repository.CountAsync(x =>
                x.Id != iD && x.CategoryType == categoryType && x.NameGerman == germanName);
        }

        return count > 0;
    }

    //[Authorize("Category.Validate")]
    [GraphQLDescription("Validate english category name ")]
    public async Task<bool> EnglishCategoryNameExistsAsync(FamilieLaissDbContext context,
        long iD, EnumCategoryType categoryType, string englishName)
    {
        var repository = context.Category;

        long count;
        if (categoryType == EnumCategoryType.Both)
        {
            count = await repository.CountAsync(x => x.Id != iD && x.NameEnglish == englishName);
        }
        else
        {
            count = await repository.CountAsync(x =>
                x.Id != iD && x.CategoryType == categoryType && x.NameEnglish == englishName);
        }

        return count > 0;
    }
}