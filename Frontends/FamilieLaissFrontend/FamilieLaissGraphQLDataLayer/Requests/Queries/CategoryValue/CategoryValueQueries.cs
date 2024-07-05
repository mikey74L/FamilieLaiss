using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Queries.CategoryValue;

public static class CategoryValueQueries
{
    public static GraphQLRequest GetCategoryValuesForCategoryQuery(long categoryId)
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetCategoryValuesForCategory($categoryId: Long!) {
                    categories (where: {id: {eq: $categoryId}}) {
                        id
                        categoryType
                        nameGerman
                        nameEnglish
                        createDate
                        changeDate
                    }

                    categoryValues (where: {category: {id: {eq: $categoryId}}}) {
                        id
                        nameGerman
                        nameEnglish
                        createDate
                        changeDate
                    }
                }
            ",
            Variables = new
            {
                categoryId
            }
        };
    }

    public static GraphQLRequest GetCategoryValueQuery(long id)
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetCategoryValue ($id: Long!) {
                    categoryValues (where: {id: {eq: $id}}) {
                        id
                        nameGerman
                        nameEnglish
                        createDate
                        changeDate
                    }
                }
            ",
            OperationName = "GetCategoryValue",
            Variables = new
            {
                id
            }
        };
    }

    public static GraphQLRequest GetEnglishCategoryValueNameExistsQuery(long categoryId, string englishName, long id)
    {
        return new GraphQLRequest
        {
            Query = @"
                query EnglishCategoryValueNameExists ($categoryId: Long!, $englishName: String!, $id: Long!) {
                    englishCategoryValueNameExists(categoryID: $categoryId, englishName: $englishName, id: $iD)
                }
            ",
            OperationName = "EnglishCategoryValueNameExists",
            Variables = new
            {
                categoryId,
                englishName,
                id
            }
        };
    }

    public static GraphQLRequest GetGermanCategoryValueNameExistsQuery(long categoryId, string germanName, long id)
    {
        return new GraphQLRequest
        {
            Query = @"
                query GermanCategoryValueNameExists ($categoryId: Long!, $germanName: String!, $id: Long!) {
                    germanCategoryValueNameExists(categoryID: $categoryId, germanName: $germanName, id: $iD)
                }
            ",
            OperationName = "GermanCategoryValueNameExists",
            Variables = new
            {
                categoryId,
                germanName,
                id
            }
        };
    }
}
