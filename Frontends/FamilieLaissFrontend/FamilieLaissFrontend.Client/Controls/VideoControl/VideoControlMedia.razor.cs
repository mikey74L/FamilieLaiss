using FamilieLaissFrontend.Client.ViewModels.Controls.VideoControl;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.VideoControl;

public partial class VideoControlMedia(VideoControlMediaViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<VideoControlMediaViewModel>(viewModel, navigationManager)
{
    
}