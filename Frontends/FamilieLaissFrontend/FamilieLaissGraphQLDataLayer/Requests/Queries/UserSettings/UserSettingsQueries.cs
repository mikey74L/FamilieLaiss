using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Queries.UserSettings;

public static class UserSettingsQueries
{
    public static GraphQLRequest GetAllUserSettingsQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetAllUserSettings {
                    userSettings {
                        id
                        videoAutoPlay
                        videoVolume
                        videoLoop
                        videoAutoPlayOtherVideos
                        videoTimeToPlayNextVideo
                        showButtonForward
                        showButtonRewind
                        showZoomMenu
                        showPlayRateMenu
                        showMirrorButton
                        showContextMenu
                        showQualityMenu
                        showTooltipForPlaytimeOnMouseCursor
                        showTooltipForCurrentPlaytime
                        showZoomInfo
                        allowZoomingWithMouseWheel
                        galleryCloseEsc
                        galleryCloseDimmer
                        galleryMouseWheelChangeSlide
                        galleryShowThumbnails
                        galleryShowFullScreen
                        galleryTransitionDuration
                        galleryTransitionType
                        simpleFilter
                        questionKeepUploadWhenDelete
                        defaultKeepUploadWhenDelete
                        createDate
                        changeDate
                    }
                }
                "
        };
    }

    public static GraphQLRequest GetUserSettingsForUserQuery(string id)
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetUserSettingsForUser ($id: String!) {
                    userSettings (where: {id: {eq: $id}}) {
                        id
                        videoAutoPlay
                        videoVolume
                        videoLoop
                        videoAutoPlayOtherVideos
                        videoTimeToPlayNextVideo
                        showButtonForward
                        showButtonRewind
                        showZoomMenu
                        showPlayRateMenu
                        showMirrorButton
                        showContextMenu
                        showQualityMenu
                        showTooltipForPlaytimeOnMouseCursor
                        showTooltipForCurrentPlaytime
                        showZoomInfo
                        allowZoomingWithMouseWheel
                        galleryCloseEsc
                        galleryCloseDimmer
                        galleryMouseWheelChangeSlide
                        galleryShowThumbnails
                        galleryShowFullScreen
                        galleryTransitionDuration
                        galleryTransitionType
                        simpleFilter
                        questionKeepUploadWhenDelete
                        defaultKeepUploadWhenDelete
                        createDate
                        changeDate
                    }
                }
                ",
            Variables = new
            {
                id
            }
        };
    }
}
