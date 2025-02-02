using FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Upload.Picture;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Pages.BaseData.Upload.Picture;

public partial class PictureUploadList(PictureUploadListViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<PictureUploadListViewModel>(viewModel, navigationManager)
{
    
}