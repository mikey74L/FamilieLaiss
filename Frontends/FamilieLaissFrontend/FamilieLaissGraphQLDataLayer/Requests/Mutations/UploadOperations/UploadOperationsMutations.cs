using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Mutations.UploadOperations;

public static class UploadOperationsMutations
{
    public static GraphQLRequest GetAddPictureUploadChunkMutation(string targetFilename, long chunkNumber, int chunkSize, string chunkData)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation AddPictureUploadChunk($targetFilename: String!, $chunkNumber: Long!, $chunkSize: Int!, $chunkData: String!) {
                    addUploadPictureChunk ( input: { targetFilename: $targetFilename, chunkNumber: $chunkNumber, chunkSize: $chunkSize, chunkData: $chunkData }) {
                        status
                    }
                }
            ",
            OperationName = "AddPictureUploadChunk",
            Variables = new
            {
                targetFilename,
                chunkNumber,
                chunkSize,
                chunkData
            }
        };
    }

    public static GraphQLRequest GetFinishPictureUploadMutation(string targetFilename, long lastChunkNumber, string originalFilename)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation FinishPictureUpload($targetFilename: String!, $lastChunkNumber: Long!, $originalFilename: String!) {
                    uploadPictureFinish( input: { targetFilename: $targetFilename, lastChunkNumber: $lastChunkNumber, originalFilename: $originalFilename }) {
                        status
                    }
                }
            ",
            OperationName = "FinishPictureUpload",
            Variables = new
            {
                targetFilename,
                lastChunkNumber,
                originalFilename
            }
        };
    }

    public static GraphQLRequest GetAddVideoUploadChunkMutation(string targetFilename, long chunkNumber, int chunkSize, string chunkData)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation AddVideoUploadChunk($targetFilename: String!, $chunkNumber: Long!, $chunkSize: Int!, $chunkData: String!) {
                    addUploadVideoChunk ( input: { targetFilename: $targetFilename, chunkNumber: $chunkNumber, chunkSize: $chunkSize, chunkData: $chunkData }) {
                        status
                    }
                }
            ",
            OperationName = "AddVideoUploadChunk",
            Variables = new
            {
                targetFilename,
                chunkNumber,
                chunkSize,
                chunkData
            }
        };
    }

    public static GraphQLRequest GetFinishVideoUploadMutation(string targetFilename, long lastChunkNumber, string originalFilename)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation FinishVideoUpload($targetFilename: String!, $lastChunkNumber: Long!, $originalFilename: String!) {
                    uploadVideoFinish( input: { targetFilename: $targetFilename, lastChunkNumber: $lastChunkNumber, originalFilename: $originalFilename }) {
                        status
                    }
                }            
            ",
            OperationName = "FinishVideoUpload",
            Variables = new
            {
                targetFilename,
                lastChunkNumber,
                originalFilename
            }
        };
    }
}
