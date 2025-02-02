using FamilieLaissFrontend.Client.ViewModels.Pages.Converter.Picture;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Pages.Converter.Picture;

public partial class PictureConverterStatus(PictureConverterStatusViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<PictureConverterStatusViewModel>(viewModel, navigationManager)
{
    
}