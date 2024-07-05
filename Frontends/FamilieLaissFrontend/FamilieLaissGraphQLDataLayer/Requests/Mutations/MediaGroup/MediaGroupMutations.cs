using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Mutations.MediaGroup;

public class MediaGroupMutations
{
    public static GraphQLRequest GetAddMediaGroupMutation(string nameGerman, string nameEnglish, string descriptionGerman,
        string descriptionEnglish, DateTimeOffset eventDate)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation AddMediaGroup ($nameGerman: String!, $nameEnglish: String!, $descriptionGerman: String!, $descriptionEnglish: String!, $eventDate: DateTime!) {
                    addMediaGroup (input: {nameGerman: $nameGerman, nameEnglish: $nameEnglish, descriptionGerman: $descriptionGerman, descriptionEnglish: $descriptionEnglish, eventDate: $eventDate}) {
                        mediaGroup {
                            id
                            createDate
                        }
                    }
                }
            ",
            OperationName = "AddMediaGroup",
            Variables = new
            {
                nameGerman,
                nameEnglish,
                descriptionGerman,
                descriptionEnglish,
                eventDate
            }
        };
    }

    public static GraphQLRequest GetUpdateMediaGroupMutation(long id, string nameGerman, string nameEnglish, string descriptionGerman,
        string descriptionEnglish, DateTimeOffset eventDate)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation UpdateMediaGroup ($id: Long!, $nameGerman: String!, $nameEnglish: String!, $descriptionGerman: String!, $descriptionEnglish: String!, $eventDate: DateTime!) {
                    updateMediaGroup (input: { id: $id, nameGerman: $nameGerman, nameEnglish: $nameEnglish, descriptionGerman: $descriptionGerman, descriptionEnglish: $descriptionEnglish, eventDate: $eventDate}) {
                        mediaGroup {
                            id
                            changeDate
                        }
                    }
                }
            ",
            OperationName = "UpdateMediaGroup",
            Variables = new
            {
                id,
                nameGerman,
                nameEnglish,
                descriptionGerman,
                descriptionEnglish,
                eventDate
            }
        };
    }

    public static GraphQLRequest GetDeleteMediaGroupMutation(long id)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation DeleteMediaGroup ($id: Long!) {
                    deleteMediaGroup (input: {id: $id}) {
                        mediaGroup {
                            id
                        }
                    }
                }            
            ",
            OperationName = "DeleteMediaGroup",
            Variables = new
            {
                id
            }
        };
    }
}
