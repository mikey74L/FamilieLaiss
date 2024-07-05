namespace Upload.API.GraphQL.Mutations.UploadPicture
{
    public class DeleteUploadPicturePayload
    {
        public Upload.Domain.Entities.UploadPicture UploadPicture { get; set; } = default!;
    }
}
