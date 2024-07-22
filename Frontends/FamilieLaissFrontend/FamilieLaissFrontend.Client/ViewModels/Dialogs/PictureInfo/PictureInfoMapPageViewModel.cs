using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.JSInterop;
using MudBlazor;
using Radzen;
using Radzen.Blazor;

namespace FamilieLaissFrontend.Client.ViewModels.Dialogs.PictureInfo;

public partial class PictureInfoMapPageViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    ISecretDataService secretDataService,
    IJSRuntime jsRuntime)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Services
    public readonly ISecretDataService SecretDataService = secretDataService;

    #endregion

    #region Parameters
    public IMediaItemModel? MediaItem { get; set; }
    public IUploadPictureModel? UploadItem { get; set; }
    #endregion

    #region Public Properties
    [ObservableProperty]
    private GoogleMapPosition _googleMapPosition = default!;

    [ObservableProperty]
    private IUploadPictureModel _pictureModel = default!;

    public RadzenGoogleMap MapControl { get; set; } = default!;
    #endregion

    #region Lifecycle
    public override void OnInitialized()
    {
        base.OnInitialized();

        if (MediaItem?.UploadPicture is not null)
        {
            PictureModel = MediaItem.UploadPicture;
        }
        else
        {
            if (UploadItem is not null)
            {
                PictureModel = UploadItem;
            }
        }

        if (PictureModel.GoogleGeoCodingAddress?.Latitude is not null && PictureModel.GoogleGeoCodingAddress?.Longitude is not null)
        {
            GoogleMapPosition = new()
            {
                Lat = PictureModel.GoogleGeoCodingAddress.Latitude.Value,
                Lng = PictureModel.GoogleGeoCodingAddress.Longitude.Value
            };
        }
        else
        {
            GoogleMapPosition = new()
            {
                Lat = 48.70109739572691,
                Lng = 9.009845828712576
            };
        }
    }
    #endregion

    #region Commands
    [RelayCommand]
    private async Task MarkerClick(RadzenGoogleMapMarker marker)
    {
        string message = string.Empty;
        if (PictureModel.GoogleGeoCodingAddress is not null)
        {
            message = $"<b>{marker.Title}</b></br>{PictureModel.GoogleGeoCodingAddress.StreetName} {PictureModel.GoogleGeoCodingAddress.Hnr}</br>{PictureModel.GoogleGeoCodingAddress.Zip} {PictureModel.GoogleGeoCodingAddress.City}</br>{PictureModel.GoogleGeoCodingAddress.Country}";
        }

        var code = $@"
            var map = Radzen['{MapControl.UniqueID}'].instance;
            var marker = map.markers.find(m => m.title == '{marker.Title}');
            if(window.infoWindow) {{window.infoWindow.close();}}
            window.infoWindow = new google.maps.InfoWindow({{content: '{message}'}});
            setTimeout(() => window.infoWindow.open(map, marker), 200);
            ";

        await jsRuntime.InvokeVoidAsync("eval", code);
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
