using Catalog.API.GraphQL.DataLoaders.Category;
using Catalog.Infrastructure.DBContext;
using FamilieLaissSharedObjects.Enums;
using HotChocolate.Fusion.SourceSchema.Types;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.GraphQL.Queries.Category;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryCategory
{
    //[Authorize("Category.Read")]
    [GraphQLDescription("Returns a list of categories")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Aggregates.Category> GetCategories(CatalogServiceDbContext context)
    {
        return context.Category;
    }

    [GraphQLDescription("Returns a category")]
    [Lookup]
    public async Task<Domain.Aggregates.Category> GetCategory(long id, CategoryDataLoader dataLoader)
    => await dataLoader.LoadAsync(id);

    //[Authorize("Category.Validate")]
    [GraphQLDescription("Validate german category name ")]
    public async Task<bool> GermanCategoryNameExistsAsync(CatalogServiceDbContext context,
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
    public async Task<bool> EnglishCategoryNameExistsAsync(CatalogServiceDbContext context,
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