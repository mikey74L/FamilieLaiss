using FamilieLaissEnums.GraphQL;
using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Mutations.MediaItem;

public static class MediaItemMutations
{
    public static GraphQLRequest GetAddMediaItemMutation(long mediaGroupID, EnumMediaType mediaType, string nameGerman, string nameEnglish,
        string descriptionGerman, string descriptionEnglish, bool onlyFamily, long uploadID, List<long> categoryValues)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation AddMediaItem ($mediaGroupID: Long!, 
                                       $mediaType: EnumMediaType!, 
                                       $nameGerman: String!, 
                                       $nameEnglish: String!, 
                                       $descriptionGerman: String!, 
                                       $descriptionEnglish: String!, 
                                       $onlyFamily: Boolean!, 
                                       $uploadID: Long!
                                       $categoryValues: [Long!]!) {
                  addMediaItem (input: {mediaGroupID: $mediaGroupID, 
                                        mediaType: $mediaType,  
                                        nameGerman: $nameGerman, 
                                        nameEnglish: $nameEnglish, 
                                        descriptionGerman: $descriptionGerman, 
                                        descriptionEnglish: $descriptionEnglish, 
                                        onlyFamily: $onlyFamily, 
                                        uploadID: $uploadID,
                                        categoryValues: $categoryValues}) {
                      mediaItem {
                         id
                         createDate
                      }
                  }
                }
            ",
            OperationName = "AddMediaItem",
            Variables = new
            {
                mediaGroupID,
                mediaType,
                nameGerman,
                nameEnglish,
                descriptionGerman,
                descriptionEnglish,
                onlyFamily,
                uploadID,
                categoryValues
            }
        };
    }

    public static GraphQLRequest GetUpdateMediaItemMutation(long id, string nameGerman, string nameEnglish,
        string descriptionGerman, string descriptionEnglish, bool onlyFamily, List<long> categoryValues)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation UpdateMediaItem ($id: Long!, 
                                          $nameGerman: String!, 
                                          $nameEnglish: String!, 
                                          $descriptionGerman: String!, 
                                          $descriptionEnglish: String!, 
                                          $onlyFamily: Boolean!
                                          $categoryValues: [Long!]!) {
                  updateMediaItem (input: {id: $id, 
                                           nameGerman: $nameGerman, 
                                           nameEnglish: $nameEnglish, 
                                           descriptionGerman: $descriptionGerman, 
                                           descriptionEnglish: $descriptionEnglish, 
                                           onlyFamily: $onlyFamily,
                                           categoryValues: $categoryValues}) {
                    mediaItem {
                        id
                        changeDate
                    }
                  }
                }
            ",
            OperationName = "UpdateMediaItem",
            Variables = new
            {
                id,
                nameGerman,
                nameEnglish,
                descriptionGerman,
                descriptionEnglish,
                onlyFamily,
                categoryValues
            }
        };
    }

    public static GraphQLRequest GetRemoveMediaItemMutation(long mediaGroupId, long mediaItemId, bool deleteUploadItem)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation RemoveMediaItem ($mediaGroupId: Long!, $mediaItemId: Long!, $deleteUploadItem: Boolean!) {
                  removeMediaItem (input: {mediaGroupId: $mediaGroupId, mediaItemId: $mediaItemId, deleteUploadItem: $deleteUploadItem}) {
                    mediaItem {
                      id
                    }
                  }
                }            
            ",
            OperationName = "RemoveMediaItem",
            Variables = new
            {
                mediaGroupId,
                mediaItemId,
                deleteUploadItem
            }
        };
    }
}
