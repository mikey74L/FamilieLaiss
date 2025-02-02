using FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Upload.Video;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Pages.BaseData.Upload.Video;

public partial class VideoUploadPage(VideoUploadPageViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<VideoUploadPageViewModel>(viewModel, navigationManager)
{
    
}