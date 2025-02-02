using FamilieLaissConstants;
using FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Media;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Pages.BaseData.Media;

public partial class MediaList(MediaListViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<MediaListViewModel>(viewModel, navigationManager)
{
    
}