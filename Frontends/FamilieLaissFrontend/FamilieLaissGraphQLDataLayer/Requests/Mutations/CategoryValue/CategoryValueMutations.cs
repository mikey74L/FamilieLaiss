using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Mutations.CategoryValue;

public static class CategoryValueMutations
{
    public static GraphQLRequest GetAddCategoryValueMutation(long categoryId, string nameGerman, string nameEnglish)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation AddCategoryValue ($categoryId: Long!, $nameGerman: String!, $nameEnglish: String!) {
                    addCategoryValue(input: {categoryID: $categoryId, nameGerman: $nameGerman, nameEnglish: $nameEnglish}) {
                        categoryValue {
                            id
                            createDate
                        }
                    }
                }
            ",
            OperationName = "AddCategoryValue",
            Variables = new
            {
                categoryId,
                nameGerman,
                nameEnglish
            }
        };
    }

    public static GraphQLRequest GetUpdateCategoryValueMutation(long id, string nameGerman, string nameEnglish)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation UpdateCategoryValue ($id: Long!, $nameGerman: String!, $nameEnglish: String!) {
                    updateCategoryValue(input: {id: $id, nameGerman: $nameGerman, nameEnglish: $nameEnglish}) {
                        categoryValue {
                            id
                            changeDate
                        }
                    }
                }
            ",
            OperationName = "UpdateCategoryValue",
            Variables = new
            {
                id,
                nameGerman,
                nameEnglish
            }
        };
    }

    public static GraphQLRequest GetDeleteCategoryValueMutation(long id)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation DeleteCategoryValue ($id: Long!) {
                    deleteCategoryValue (input: {id : $id}) {
                        categoryValue {
                            id
                        }
                    }
                }            
            ",
            OperationName = "DeleteCategoryValue",
            Variables = new
            {
                id,
            }
        };
    }
}
