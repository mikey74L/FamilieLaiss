using FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Upload.Picture;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Pages.BaseData.Upload.Picture;

public partial class PictureUploadPage(PictureUploadPageViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<PictureUploadPageViewModel>(viewModel, navigationManager)
{
    
}