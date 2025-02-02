using FamilieLaissFrontend.Client.ViewModels.Controls.VideoControl;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.VideoControl;

public partial class VideoControlInfo(VideoControlInfoViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<VideoControlInfoViewModel>(viewModel, navigationManager)
{
    
}