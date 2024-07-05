using HotChocolate;

namespace FamilieLaissSharedObjects.Enums;

/// <summary>
/// Enumeration for setting the video type for the converted upload video
/// </summary>
[GraphQLDescription("The video type for the converted upload video")]
public enum EnumVideoType : byte
{
    /// <summary>
    /// Not set. Is set when the video is uploaded but not yet converted
    /// </summary>
    [GraphQLDescription("Not set")] NotSet = 0,

    /// <summary>
    /// It is a progressive video
    /// </summary>
    [GraphQLDescription("Progressive video format")]
    Progressive = 1,

    /// <summary>
    /// It is a streaming video in HLS format
    /// </summary>
    [GraphQLDescription("HTTP Live Streaming format")]
    Hls = 2
}