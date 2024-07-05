using FamilieLaissEnums.GraphQL;
using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Queries.MediaGroup;

public class MediaGroupQueries
{
    public static GraphQLRequest GetAllMediaGroupsQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetMediaGroups {
                    mediaGroups
                    {
                        id
                        nameGerman
                        nameEnglish
                        descriptionGerman
                        descriptionEnglish
                        eventDate
                        createDate
                        changeDate
                    }
                }
            "
        };
    }

    public static GraphQLRequest GetMediaGroupQuery(long id)
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetMediaGroup ($id: Long!) {
                    mediaGroups (where: {id: {eq: $id}}) {
                        id
                        nameGerman
                        nameEnglish
                        descriptionGerman
                        descriptionEnglish
                        eventDate
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

    public static GraphQLRequest GetMediaItemCountForMediaGroupsQuery(EnumMediaType mediaType)
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetMediaItemCountForMediaGroups ($mediaType: EnumMediaType!) {
                    mediaItemCountForMediaGroups (enumMediaType: $mediaType) {
                        mediaGroupId
                        count
                    }
                }
            ",
            Variables = new
            {
                mediaType
            }
        };
    }

    public static GraphQLRequest GetGermanMediaGroupNameExistsQuery(string germanName, long id)
    {
        return new GraphQLRequest
        {
            Query = @"
                query GermanMediaGroupNameExists ($germanName: String!, $id: Long!) {
                    germanMediaGroupNameExists(germanName: $germanName, iD: $id)
                }
            ",
            Variables = new
            {
                id,
                germanName
            }
        };
    }

    public static GraphQLRequest GetEnglishMediaGroupNameExistsQuery(string englishName, long id)
    {
        return new GraphQLRequest
        {
            Query = @"
                query EnglishMediaGroupNameExists ($englishName: String!, $id: Long!) {
                    englishMediaGroupNameExists(englishName: $englishName, iD: $id)
                }
            ",
            Variables = new
            {
                id,
                englishName
            }
        };
    }
}
