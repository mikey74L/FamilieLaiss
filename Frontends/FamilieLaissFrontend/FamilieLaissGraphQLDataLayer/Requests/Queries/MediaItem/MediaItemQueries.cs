using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Queries.MediaItem;

public static class MediaItemQueries
{
    public static GraphQLRequest GetMediaItemQuery(long id)
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetMediaItem ($id: Long!) {
                  mediaItems (where: {id: {eq: $id}}) {
                    id
                    mediaGroup {
                        id
                        nameGerman
                        nameEnglish
                        eventDate
                    }
                    mediaType
                    nameGerman
                    nameEnglish
                    descriptionGerman
                    descriptionEnglish
                    onlyFamily
                    createDate
                    changeDate
                    uploadPicture {
                        id
                        filename
                        height
                        width
                        createDate
                    }
                    uploadVideo {
                        id
                        filename
                        height
                        width
                        duration_Hour
                        duration_Minute
                        duration_Second
                        status
                        videoType
                        createDate
                    }
                    mediaItemCategoryValues {
                        id
                        categoryValue {
                            id
                            nameGerman
                            nameEnglish
                            category {
                                id
                                nameGerman
                                nameEnglish
                            }
                        }
                    }
                  }
                }
            ",
            Variables = new
            {
                id
            }
        };
    }

    public static GraphQLRequest GetAllItemsForMediaGroupQuery(long mediaGroupId)
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetAllItemsForMediaGroup ($mediaGroupId: Long!) {
                  mediaItems (where: {mediaGroup: {id: {eq: $mediaGroupId}}}) {
                    id
                    mediaGroup {
                        id
                        nameGerman
                        nameEnglish
                        eventDate
                    }
                    mediaType
                    nameGerman
                    nameEnglish
                    descriptionGerman
                    descriptionEnglish
                    onlyFamily
                    createDate
                    changeDate
                    uploadPicture {
                        id
                        filename
                        height
                        width
                        createDate
                    }
                    uploadVideo {
                        id
                        filename
                        height
                        width
                        duration_Hour
                        duration_Minute
                        duration_Second
                        status
                        videoType
                        createDate
                    }
                  }
                }
                ",
            Variables = new
            {
                mediaGroupId
            }
        };
    }

    public static GraphQLRequest GetVideosForMediaGroupQuery(long mediaGroupId)
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetVideosForMediaGroup ($mediaGroupId: Long!) {
                  mediaItems (where: {and: [{mediaType: {eq: VIDEO}}, {mediaGroup: {id: {eq: $mediaGroupId}}}]}) {
                    id
                    mediaGroup {
                        id
                        nameGerman
                        nameEnglish
                        eventDate
                    }
                    mediaType
                    nameGerman
                    nameEnglish
                    descriptionGerman
                    descriptionEnglish
                    onlyFamily
                    createDate
                    changeDate
                    uploadPicture {
                        id
                        filename
                        height
                        width
                        createDate
                    }
                    uploadVideo {
                        id
                        filename
                        height
                        width
                        duration_Hour
                        duration_Minute
                        duration_Second
                        status
                        videoType
                        createDate
                    }
                  }
                }
                ",
            Variables = new
            {
                mediaGroupId
            }
        };
    }

    public static GraphQLRequest GetFotosForMediaGroupQuery(long mediaGroupId)
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetFotosForMediaGroup ($mediaGroupId: Long!) {
                  mediaItems (where: {and: [{mediaType: {eq: PICTURE}}, {mediaGroup: {id: {eq: $mediaGroupId}}}]}) {
                    id
                    mediaGroup {
                        id
                        nameGerman
                        nameEnglish
                        eventDate
                    }
                    mediaType
                    nameGerman
                    nameEnglish
                    descriptionGerman
                    descriptionEnglish
                    onlyFamily
                    createDate
                    changeDate
                    uploadPicture {
                        id
                        filename
                        height
                        width
                        createDate
                    }
                    uploadVideo {
                        id
                        filename
                        height
                        width
                        duration_Hour
                        duration_Minute
                        duration_Second
                        status
                        videoType
                        createDate
                    }
                  }
                }
                ",
            Variables = new
            {
                mediaGroupId
            }
        };
    }

    public static GraphQLRequest GetGermanMediaItemNameExistsQuery(string germanName, long id, long mediaGroupId)
    {
        return new GraphQLRequest
        {
            Query = @"
                query GermanMediaItemNameExists ($germanName: String!, $id: Long!, $mediaGroupId: Long!) {
                    germanMediaItemNameExists(germanName: $germanName, iD: $id, mediaGroupId: $mediaGroupId)
                }
                ",
            Variables = new
            {
                germanName,
                id,
                mediaGroupId
            }
        };
    }

    public static GraphQLRequest GetEnglishMediaItemNameExistsQuery(string englishName, long id, long mediaGroupId)
    {
        return new GraphQLRequest
        {
            Query = @"
                query EnglishMediaItemNameExists ($englishName: String!, $id: Long!, $mediaGroupId: Long!) {
                    englishMediaItemNameExists(englishName: $englishName, iD: $id, mediaGroupId: $mediaGroupId)
                }
                ",
            Variables = new
            {
                englishName,
                id,
                mediaGroupId
            }
        };
    }
}
