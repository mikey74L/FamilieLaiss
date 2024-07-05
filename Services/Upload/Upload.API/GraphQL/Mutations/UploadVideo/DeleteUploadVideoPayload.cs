namespace Upload.API.GraphQL.Mutations.UploadVideo
{
    public class DeleteUploadVideoPayload
    {
        public Upload.Domain.Entities.UploadVideo UploadVideo { get; set; } = default!;
    }
}
