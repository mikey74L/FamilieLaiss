using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Mutations.UserSettings;

public static class UserSettingsMutations
{
    public static GraphQLRequest GetUpdateUserSettingsMutation(string id, bool allowZoomingWithMouseWheel, bool defaultKeepUploadWhenDelete,
        bool galleryCloseDimmer, bool galleryCloseEsc, bool galleryMouseWheelChangeSlide,
        bool galleryShowFullScreen, bool galleryShowThumbnails, int galleryTransitionDuration,
        string galleryTransitionType, bool questionKeepUploadWhenDelete, bool showButtonForward,
        bool showButtonRewind, bool showContextMenu, bool showMirrorButton, bool showPlayRateMenu,
        bool showQualityMenu, bool showTooltipForCurrentPlaytime, bool showTooltipForPlaytimeOnMouseCursor,
        bool showZoomInfo, bool showZoomMenu, bool simpleFilter, bool videoAutoPlay, bool videoAutoPlayOtherVideos,
        bool videoLoop, int videoTimeToPlayNextVideo, int videoVolume)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation UpdateUserSettings ($id: String!, $allowZoomingWithMouseWheel: Boolean!, $defaultKeepUploadWhenDelete: Boolean!,
                            $galleryCloseDimmer: Boolean!, $galleryCloseEsc: Boolean!, $galleryMouseWheelChangeSlide: Boolean!,
                            $galleryShowFullScreen: Boolean!, $galleryShowThumbnails: Boolean!, $galleryTransitionDuration: Int!,
                            $galleryTransitionType: String!, $questionKeepUploadWhenDelete: Boolean!, $showButtonForward: Boolean!,
                            $showButtonRewind: Boolean!, $showContextMenu: Boolean!, $showMirrorButton: Boolean!, $showPlayRateMenu: Boolean!,
                            $showQualityMenu: Boolean!, $showTooltipForCurrentPlaytime: Boolean!, $showTooltipForPlaytimeOnMouseCursor: Boolean!,
                            $showZoomInfo: Boolean!, $showZoomMenu: Boolean!, $simpleFilter: Boolean!, $videoAutoPlay: Boolean!,
                            $videoAutoPlayOtherVideos: Boolean!, $videoLoop: Boolean!, $videoTimeToPlayNextVideo: Int!, $videoVolume: Int!) {
                    updateUserSettings (input: {id: $id,
                                                allowZoomingWithMouseWheel: $allowZoomingWithMouseWheel, 
                                                defaultKeepUploadWhenDelete: $defaultKeepUploadWhenDelete,
                                                galleryCloseDimmer: $galleryCloseDimmer,
                                                galleryCloseEsc: $galleryCloseEsc,
                                                galleryMouseWheelChangeSlide: $galleryMouseWheelChangeSlide,
                                                galleryShowFullScreen: $galleryShowFullScreen,
                                                galleryShowThumbnails: $galleryShowThumbnails,
                                                galleryTransitionDuration: $galleryTransitionDuration,
                                                galleryTransitionType: $galleryTransitionType,
                                                questionKeepUploadWhenDelete: $questionKeepUploadWhenDelete,
                                                showButtonForward: $showButtonForward,
                                                showButtonRewind: $showButtonRewind,
                                                showContextMenu: $showContextMenu,
                                                showMirrorButton: $showMirrorButton,
                                                showPlayRateMenu: $showPlayRateMenu,
                                                showQualityMenu: $showQualityMenu,
                                                showTooltipForCurrentPlaytime: $showTooltipForCurrentPlaytime,
                                                showTooltipForPlaytimeOnMouseCursor: $showTooltipForPlaytimeOnMouseCursor,
                                                showZoomInfo: $showZoomInfo,
                                                showZoomMenu: $showZoomMenu,
                                                simpleFilter: $simpleFilter,
                                                videoAutoPlay: $videoAutoPlay,
                                                videoAutoPlayOtherVideos: $videoAutoPlayOtherVideos,
                                                videoLoop: $videoLoop,
                                                videoTimeToPlayNextVideo: $videoTimeToPlayNextVideo,
                                                videoVolume: $videoVolume}) {
                        userSettings {
                            id
                        }
                    }
                }
                ",
            OperationName = "UpdateUserSettings",
            Variables = new
            {
                id,
                allowZoomingWithMouseWheel,
                defaultKeepUploadWhenDelete,
                galleryCloseDimmer,
                galleryCloseEsc,
                galleryMouseWheelChangeSlide,
                galleryShowFullScreen,
                galleryShowThumbnails,
                galleryTransitionDuration,
                galleryTransitionType,
                questionKeepUploadWhenDelete,
                showButtonForward,
                showButtonRewind,
                showContextMenu,
                showMirrorButton,
                showPlayRateMenu,
                showQualityMenu,
                showTooltipForCurrentPlaytime,
                showTooltipForPlaytimeOnMouseCursor,
                showZoomInfo,
                showZoomMenu,
                simpleFilter,
                videoAutoPlay,
                videoAutoPlayOtherVideos,
                videoLoop,
                videoTimeToPlayNextVideo,
                videoVolume
            }
        };
    }
}
