namespace Catalog.API.GraphQL.Mutations.MediaGroup;

[GraphQLDescription("InputData type for deleting media group")]
public class DeleteMediaGroupInput
{
    [GraphQLDescription("The Id for the media group")]
    public long Id { get; set; }

    [GraphQLDescription("Should assigned upload items be kept in the upload area?")]
    public bool KeepUploadItems { get; set; }
}