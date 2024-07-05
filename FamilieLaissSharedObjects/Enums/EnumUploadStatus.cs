using HotChocolate;

namespace FamilieLaissSharedObjects.Enums
{
    /// <summary>
    /// Enumaration for setting the state for upload items
    /// </summary>
    [GraphQLDescription("State for upload item")]
    public enum EnumUploadStatus : byte
    {
        /// <summary>
        /// File is only uploaded and not converted
        /// </summary>
        [GraphQLDescription("File is only uploaded and not converted")]
        Uploaded = 0,
        /// <summary>
        /// File is converted by a converter service
        /// </summary>
        [GraphQLDescription("File is converted by a converter service")]
        Converted = 1,
        /// <summary>
        /// The file is uploaded, converted and assigned to a media item
        /// </summary>
        [GraphQLDescription("The file is uploaded, converted and assigned to a media item")]
        Assigned = 2
    }
}
