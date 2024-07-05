using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Queries.VideoConvert;

public static class VideoConvertQueries
{
    public static GraphQLRequest GetVideoConvertWaitingQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetVideoConvertWaiting {
	                videoConvertStatus (where: {status: {eq: WAITING_FOR_CONVERSION }}) {
                        id
                        uploadVideo {
                            id
                            filename
                        }
	                }
                }
                "
        };
    }

    public static GraphQLRequest GetVideoConvertExecutingQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetVideoConvertExecuting {
                    videoConvertStatus (where: {status: {nin: [CONVERTED_WITH_ERRORS, SUCESSFULLY_CONVERTED, WAITING_FOR_CONVERSION, TRANSIENT_ERROR]}}) {
                        id
                        status
                        videoType
                        start_Date_MP4
                        finish_Date_MP4
                        start_Date_MP4_360
                        finish_Date_MP4_360
                        start_Date_MP4_480
                        finish_Date_MP4_480
                        start_Date_MP4_720
                        finish_Date_MP4_720
                        start_Date_MP4_1080
                        finish_Date_MP4_1080
                        start_Date_MP4_2160
                        finish_Date_MP4_2160
                        start_Date_HLS
                        finish_Date_HLS
                        start_Date_Thumbnail
                        finish_Date_Thumbnail
                        start_Date_MediaInfo
                        finish_Date_MediaInfo
                        start_Date_VTT
                        finish_Date_VTT
                        start_Date_Copy_Converted
                        finish_Date_Copy_Converted
                        start_Date_Delete_Temp
                        finish_Date_Delete_Temp
                        start_Date_Delete_Original
                        finish_Date_Delete_Original
                        start_Date_Convert_Picture
                        finish_Date_Convert_Picture
                        rest_Hour
                        rest_Minute
                        rest_Second
                        progress
                        uploadVideo {
                            id
                            filename
                        }
                    }
                }
                "
        };
    }

    public static GraphQLRequest GetVideoConvertSuccessQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetVideoConvertSuccess {
	                videoConvertStatus (where: {status: {eq: SUCESSFULLY_CONVERTED }}) {
                        id
                        uploadVideo {
                            id
                            filename
                        }
	                }
                }
                "
        };
    }

    public static GraphQLRequest GetVideoConvertErrorQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetVideoConvertError {
	                videoConvertStatus (where: {status: {eq: CONVERTED_WITH_ERRORS }}) {
                        id
                        error_Message
                        uploadVideo {
                            id
                            filename
                        }
	                }
                }                
                "
        };
    }
}
