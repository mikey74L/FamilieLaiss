using FamilieLaissFrontend.Client.ViewModels.Pages.Converter.Video;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Pages.Converter.Video;

public partial class VideoConverterStatus(VideoConverterStatusViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<VideoConverterStatusViewModel>(viewModel, navigationManager)
{
    
}