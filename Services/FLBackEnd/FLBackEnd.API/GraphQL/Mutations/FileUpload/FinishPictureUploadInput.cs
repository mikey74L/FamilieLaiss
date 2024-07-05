namespace FLBackEnd.API.GraphQL.Mutations.FileUpload;

[GraphQLDescription("Input type for finish picture upload")]
public class FinishPictureUploadInput
{
    [GraphQLDescription("The target filename for the file to upload")]
    public string TargetFilename { get; set; } = string.Empty;

    [GraphQLDescription("The original filename for the file to upload")]
    public string OriginalFilename { get; set; } = string.Empty;

    [GraphQLDescription("The last chunk number")]
    public long LastChunkNumber { get; set; }
}