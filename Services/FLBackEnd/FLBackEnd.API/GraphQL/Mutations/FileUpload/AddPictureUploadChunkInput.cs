﻿namespace FLBackEnd.API.GraphQL.Mutations.FileUpload;

[GraphQLDescription("Input type for adding picture upload chunks")]
public class AddPictureUploadChunkInput
{
    [GraphQLDescription("The target filename for the file to upload")]
    public string TargetFilename { get; set; } = string.Empty;

    [GraphQLDescription("The chunk number for the chunk to upload")]
    public long ChunkNumber { get; set; }

    [GraphQLDescription("The size of the chunk in bytes")]
    public int ChunkSize { get; set; }

    [GraphQLDescription("The chunk data as base64 encoded string")]
    public string ChunkData { get; set; } = string.Empty;
}