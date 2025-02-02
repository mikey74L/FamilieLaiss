using FamilieLaissFrontend.Client.ViewModels.Controls.VideoControl;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.VideoControl;

public partial class VideoControl(VideoControlViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<VideoControlViewModel>(viewModel, navigationManager)
{
    
}