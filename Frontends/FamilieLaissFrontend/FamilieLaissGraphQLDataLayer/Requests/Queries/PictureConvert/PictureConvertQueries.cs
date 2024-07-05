using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Queries.PictureConvert;

public static class PictureConvertQueries
{
    public static GraphQLRequest GetPictureConvertWaitingQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetPictureConvertWaiting {
                    pictureConvertStatus (where: {status: {eq: WAITING_FOR_CONVERSION }}) {
                        id
                        uploadPicture {
                            id
                            filename
                        }
                    }
                }
                "
        };
    }

    public static GraphQLRequest GetPictureConvertExecutingQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetPictureConvertExecuting {
                    pictureConvertStatus (where: {status: {nin: [CONVERTED_WITH_ERRORS, SUCESSFULLY_CONVERTED, WAITING_FOR_CONVERSION, TRANSIENT_ERROR]}}) {
                        id
                        status
                        start_Date_Info
                        finish_Date_Info
                        start_Date_Exif
                        finish_Date_Exif
                        start_Date_Convert
                        finish_Date_Convert
                        uploadPicture {
                            id
                            filename
                        }
                    }
                }
                "
        };
    }

    public static GraphQLRequest GetPictureConvertSuccessQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetPictureConvertSuccess {
	                pictureConvertStatus (where: {status: {eq: SUCESSFULLY_CONVERTED }}) {
                        id
                        finish_Date_Convert
                        uploadPicture {
                            id
                            filename
                        }
	                }
                }
                "
        };
    }

    public static GraphQLRequest GetPictureConvertErrorQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetPictureConvertError {
	                pictureConvertStatus (where: {status: {eq: CONVERTED_WITH_ERRORS }}) {
                        id
                        error_Message
                        uploadPicture {
                            id
                            filename
                        }
	                }
                }                
                "
        };
    }
}
