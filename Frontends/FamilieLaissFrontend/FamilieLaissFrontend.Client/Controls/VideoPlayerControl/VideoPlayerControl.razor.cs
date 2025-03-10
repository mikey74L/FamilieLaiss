using FamilieLaissFrontend.Client.ViewModels.Controls.VideoPlayerControl;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.VideoPlayerControl;

public partial class VideoPlayerControl(VideoPlayerControlViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<VideoPlayerControlViewModel>(viewModel, navigationManager)
{
    
}