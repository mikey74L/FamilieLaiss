using FamilieLaissEnums.GraphQL;
using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Queries.Category;

public static class CategoryQueries
{
    public static GraphQLRequest GetAllCategoriesQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                    query GetAllCategories {
                        categories {
                            id
                            categoryType
                            nameGerman
                            nameEnglish
                            createDate
                            changeDate
                        }
                    }
                "
        };
    }

    public static GraphQLRequest GetPhotoCategoriesWithValuesQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                    query PhotoCategoriesWithValues {
                        categories(where: { categoryType: {in: [PICTURE, BOTH]} }) {
                            id
                            nameGerman
                            nameEnglish
                            categoryValues {
                                id
                                nameGerman
                                nameEnglish
                            }
                        }
                    }
                "
        };
    }

    public static GraphQLRequest GetVideoCategoriesWithValuesQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                    query VideoCategoriesWithValues {
                        categories (where: {categoryType: {in: [VIDEO, BOTH]}}) {
                            id
                            nameGerman
                            nameEnglish
                            categoryValues {
                                id
                                nameGerman
                                nameEnglish
                            }
                        }
                    }
                "
        };
    }

    public static GraphQLRequest GetCategoryQuery(long id)
    {
        return new GraphQLRequest
        {
            Query = @"
                    query GetCategory ($id: Long!) {
                        categories(where: {id: {eq: $id}}) {
                            id
                            categoryType
                            nameGerman
                            nameEnglish
                            createDate
                            changeDate
                        }
                    }
                ",
            Variables = new
            {
                id
            }
        };
    }

    public static GraphQLRequest GetCategoriesWithValuesQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                    query GetCategoriesWithValues
                    {
                        categories
                        {
                            nameGerman
                            nameEnglish
                                categoryValues {
                                    id
                                    nameGerman
                                    nameEnglish
                            }
                        }
                    }
                "
        };
    }

    public static GraphQLRequest GetEnglishCategoryNameExistsQuery(EnumCategoryType categoryType, string englishName, long id)
    {
        return new GraphQLRequest
        {
            Query = @"
                    query EnglishCategoryNameExists ($categoryType: EnumCategoryType!, $englishName: String!, $id: Long!) {
                        englishCategoryNameExists(categoryType: $categoryType, englishName: $englishName, iD: $id)
                    }
                ",
            Variables = new
            {
                id,
                categoryType,
                englishName
            }
        };
    }

    public static GraphQLRequest GetGermanCategoryNameExistsQuery(EnumCategoryType categoryType, string germanName, long id)
    {
        return new GraphQLRequest
        {
            Query = @"
                    query GermanCategoryNameExists ($categoryType: EnumCategoryType!, $germanName: String!, $id: Long!) {
                        germanCategoryNameExists(categoryType: $categoryType, germanName: $germanName, iD: $id)
                    }
                ",
            Variables = new
            {
                id,
                categoryType,
                germanName
            }
        };
    }
}
