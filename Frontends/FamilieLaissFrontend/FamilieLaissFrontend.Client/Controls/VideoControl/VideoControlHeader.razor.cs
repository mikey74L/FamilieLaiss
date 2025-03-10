using FamilieLaissFrontend.Client.ViewModels.Controls.VideoControl;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.VideoControl;

public partial class VideoControlHeader(VideoControlHeaderViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<VideoControlHeaderViewModel>(viewModel, navigationManager)
{
    
}