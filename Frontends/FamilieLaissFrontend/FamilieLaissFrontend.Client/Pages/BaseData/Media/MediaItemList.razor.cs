using FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Media;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Pages.BaseData.Media;

public partial class MediaItemList(MediaItemListViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<MediaItemListViewModel>(viewModel, navigationManager)
{
    
}