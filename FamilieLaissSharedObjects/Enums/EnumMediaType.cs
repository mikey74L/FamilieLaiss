using HotChocolate;

namespace FamilieLaissSharedObjects.Enums
{
    /// <summary>
    /// Setting the type for a media item
    /// </summary>
    [GraphQLDescription("Setting the type for a media item")]
    public enum EnumMediaType : byte
    {
        /// <summary>
        /// The media item is a photo
        /// </summary>
        [GraphQLDescription("The media item is a photo")]
        Picture = 1,
        /// <summary>
        /// The media is a video
        /// </summary>
        [GraphQLDescription("The media is a video")]
        Video = 2,
        /// <summary>
        /// The mixed media type (photo / video)
        /// </summary>
        [GraphQLDescription("The media type for mixed content (photo / video)")]
        Both = 3,
    }
}
