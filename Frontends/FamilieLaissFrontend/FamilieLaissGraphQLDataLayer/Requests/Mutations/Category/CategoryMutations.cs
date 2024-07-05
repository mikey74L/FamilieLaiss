using FamilieLaissEnums.GraphQL;
using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Mutations.Category;

public static class CategoryMutations
{
    public static GraphQLRequest GetAddCategoryMutation(EnumCategoryType categoryType, string nameGerman, string nameEnglish)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation AddCategory ($categoryType: EnumCategoryType!, $nameGerman: String!, $nameEnglish: String!) {
                    addCategory(input: {nameGerman: $nameGerman, nameEnglish: $nameEnglish, categoryType: $categoryType}) {
                        category {
                            id
                            createDate
                        }
                    }
                }
            ",
            OperationName = "AddCategory",
            Variables = new
            {
                categoryType,
                nameGerman,
                nameEnglish
            }
        };
    }

    public static GraphQLRequest GetUpdateCategoryMutation(long id, string nameGerman, string nameEnglish)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation UpdateCategory ($id: Long!, $nameGerman: String!, $nameEnglish: String!) {
                    updateCategory(input: {id: $id, nameGerman: $nameGerman, nameEnglish: $nameEnglish}) {
                        category {
                            id
                            changeDate
                        }
                    }
                }
            ",
            OperationName = "UpdateCategory",
            Variables = new
            {
                id,
                nameGerman,
                nameEnglish
            }
        };
    }

    public static GraphQLRequest GetDeleteCategoryMutation(long id)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation DeleteCategory ($id: Long!) {
                    deleteCategory (input: { id : $id}) {
                        category {
                            id
                        }
                    }
                }
            ",
            OperationName = "DeleteCategory",
            Variables = new
            {
                id
            }
        };
    }
}
