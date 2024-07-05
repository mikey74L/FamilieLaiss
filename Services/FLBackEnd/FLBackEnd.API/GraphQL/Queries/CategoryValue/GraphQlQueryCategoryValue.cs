using FLBackEnd.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace FLBackEnd.API.GraphQL.Queries.CategoryValue;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryCategoryValue
{
    [GraphQLDescription("Returns a list of category values")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Entities.CategoryValue> GetCategoryValues(FamilieLaissDbContext context)
    {
        return context.CategoryValues;
    }

    [GraphQLDescription("Validate german category value name")]
    public async Task<bool> GermanCategoryValueNameExistsAsync(FamilieLaissDbContext context,
        long iD, long categoryId, string germanName)
    {
        var repository = context.CategoryValues;

        var count = await repository.CountAsync(x =>
            x.Id != iD && x.CategoryId == categoryId && x.NameGerman == germanName);

        return count > 0;
    }

    [GraphQLDescription("Validate english category value name")]
    public async Task<bool> EnglishCategoryValueNameExistsAsync(FamilieLaissDbContext context,
        long iD, long categoryId, string englishName)
    {
        var repository = context.CategoryValues;

        var count = await repository.CountAsync(x =>
            x.Id != iD && x.CategoryId == categoryId && x.NameEnglish == englishName);

        return count > 0;
    }
}